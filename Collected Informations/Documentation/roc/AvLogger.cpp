#include "stdafx.h"
#include "AvLogger.h"
#include "Exception.h"

// 로깅을 해야하는지...
volatile bool AvLogger::ms_bDoLog = true;

//-------------------------------------------------------------------------------------------------

static int ExceptFilter(unsigned int code, struct _EXCEPTION_POINTERS *ep) 
{
	g_expManager.DumpException(ep);

	return EXCEPTION_EXECUTE_HANDLER;
}

//-------------------------------------------------------------------------------------------------

AvLogger::AvLogger( const char* szFileName )
{
	unsigned int Dummy = 0;

	m_hIOCP				= CreateIoCompletionPort( INVALID_HANDLE_VALUE, 0, 0, 1 );
	m_hFileWriteThread	= (HANDLE)_beginthreadex(	NULL, 0, AvLogger::FileWriteThread, this, 0, &Dummy );
	m_nLogHour			= 25;
	m_nOpenHour			= 25;
	m_pFile				= NULL;
	m_pBulkBatchFile	= NULL;
	m_bAddPrefix		= true;
	m_bBulkInserterBatchFile = false;
	m_bMakeNewFileOnEveryHour = true;
	m_bFlushOnEveryWrite = false;

	strcpy_s( m_szFileName, 256, szFileName );
}

//-------------------------------------------------------------------------------------------------


AvLogger::~AvLogger()
{
	Term();
}


//-------------------------------------------------------------------------------------------------


void AvLogger::Term()
{
	if ( m_hIOCP )
	{
		PostQueuedCompletionStatus( m_hIOCP, 0, 0, 0 );
		WaitForSingleObject( m_hFileWriteThread, INFINITE );
		CloseHandle( m_hIOCP );
		Close();

		m_hIOCP = 0;
	}
}

//-------------------------------------------------------------------------------------------------

void	AvLogger::Log( const wchar_t* szformat, ... )
{
	if (!ms_bDoLog)
		return;

	va_list args;
	va_start(args, szformat);

	int		nPrintSize = 0;

	__try
	{
		nPrintSize = _vscwprintf( szformat, args );
	}
	__except( ExceptFilter( GetExceptionCode(), GetExceptionInformation() ) )
	{
		return;
	}

	if ( nPrintSize > 0 && nPrintSize < MAX_LOG_CHARACTER_PER_LINE )
	{
		wchar_t	header[0x80];
		int		nHeaderSize = 0;

		__try
		{
			SYSTEMTIME t;
			::GetLocalTime(&t);

			if ( m_nLogHour != t.wHour )
			{
				m_nLogHour = t.wHour;
			}

			if ( m_bAddPrefix )
			{
				swprintf_s( header, 0x80, L"(%4d)%04d-%02d-%02d[%02d:%02d:%02d:%03d] ",
					GetCurrentThreadId(),
					t.wYear, t.wMonth, t.wDay,
					t.wHour, t.wMinute, t.wSecond, t.wMilliseconds);
				
				nHeaderSize = (int)wcslen(header);


				_Log( header, nHeaderSize, szformat, args, nPrintSize ); 
			}
			else
			{
				_Log( header, 0, szformat, args, nPrintSize );
			}
		}
		__except( ExceptFilter( GetExceptionCode(), GetExceptionInformation() ) )
		{
			int a = 0;
		}
	}

	va_end( args );
}

//-------------------------------------------------------------------------------------------------
#ifndef	KEEP_KEEP_GOING
#define	LOG_DEBUG_OUT
#endif

void	AvLogger::_Log( const wchar_t* szHeader, int headerlen, const wchar_t* format, va_list& args, int nPrintsize )
{
	if (!ms_bDoLog)
		return;

	wchar_t*	pLogBuffer = new wchar_t[ headerlen + nPrintsize + 4 ];		// nPrintsize dosen't contain 0 terminate
	
	if ( pLogBuffer )
	{
		if ( headerlen > 0 ) wmemcpy( pLogBuffer, szHeader, headerlen );

		vswprintf_s( pLogBuffer + headerlen, nPrintsize + 4, format, args );

		DWORD	nTotalLen = DWORD( headerlen + nPrintsize );

#ifdef	LOG_DEBUG_OUT
		if ( !m_bBulkInserterBatchFile ) OutputDebugStringW( pLogBuffer );
#endif

		if ( nTotalLen && PostQueuedCompletionStatus( m_hIOCP, sizeof(wchar_t)*nTotalLen, (ULONG_PTR)pLogBuffer, 0 ) )
		{
			return;			
		}

		delete [] pLogBuffer;
	}
}

//-------------------------------------------------------------------------------------------------

void	AvLogger::_LogBuff( const char* szHeader, int headerlen, const char* data, int nPrintsize )
{
	if (!ms_bDoLog)
		return;

	char*	pLogBuffer = new char[ headerlen + nPrintsize + 4 ];		// nPrintsize dosen't contain 0 terminate
	
	if ( pLogBuffer )
	{
		if ( headerlen > 0 ) memcpy( pLogBuffer, szHeader, headerlen );
		if ( nPrintsize > 0 )  memcpy( pLogBuffer + headerlen, data, nPrintsize );

		DWORD	nTotalLen = DWORD( headerlen + nPrintsize );

#ifdef	LOG_DEBUG_OUT
		if ( !m_bBulkInserterBatchFile ) OutputDebugStringA( pLogBuffer );
#endif

		if ( nTotalLen && PostQueuedCompletionStatus( m_hIOCP, nTotalLen, (ULONG_PTR)pLogBuffer, 0 ) )
		{
			return;			
		}

		delete [] pLogBuffer;
	}
}

//-------------------------------------------------------------------------------------------------

unsigned int __stdcall	AvLogger::FileWriteThread( void* pAvLogger )
{
	AvLogger* pLogger = (AvLogger*)pAvLogger;

	if ( pLogger ) pLogger->RunWrite();

	return 0;
}

//-------------------------------------------------------------------------------------------------

void	AvLogger::RunWrite()
{
	LONG	nFileSize = 0;
	DWORD	dwLastFlushTime = 0;
	BOOL	bNeedFlush = FALSE;

	for ( ; ms_bDoLog; )
	{
		DWORD			dwBytesTransferred = 0;
		wchar_t*		pBuffer = 0;
		LPOVERLAPPED	pDummy = 0;
		BOOL			bResult = TRUE;

		__try
		{
			if (bNeedFlush && m_pFile)
			{
				// 항상 플러시하는 세팅이거나, 60초에 한번씩은 플러시
				if (m_bFlushOnEveryWrite || GetTickCount() - dwLastFlushTime > 60*1000)
				{
					dwLastFlushTime = GetTickCount();
					fflush( m_pFile );
					bNeedFlush = FALSE;
				}
			}

			bResult = GetQueuedCompletionStatus( m_hIOCP, &dwBytesTransferred, (PULONG_PTR)&pBuffer, (LPOVERLAPPED*)&pDummy, 1000 );
			if ( !bResult || dwBytesTransferred == 0 ) 
			{
				if (GetLastError() == WAIT_TIMEOUT)
					continue;
				else
					break;
			}

			bool bClose = ( nFileSize > 1024 * 1024 * 16 );

			if ( m_bMakeNewFileOnEveryHour && m_nLogHour != m_nOpenHour )
			{
				bClose = true;
			}

			if ( bClose ) 
			{
				Close();
				nFileSize = 0;
			}

			Open();

			if ( m_pFile )
			{
				fwrite( pBuffer, dwBytesTransferred, 1, m_pFile );
				bNeedFlush = TRUE;
			}

			nFileSize = nFileSize +	 dwBytesTransferred;

			if ( pBuffer ) delete [] pBuffer;
		}
		__except( ExceptFilter( GetExceptionCode(), GetExceptionInformation() ) )
		{

		}
	}

	if ( m_pFile )
	{
		fwprintf( m_pFile, L"====================== Log closed ======================\n" );
		Close();
	}
}

//-------------------------------------------------------------------------------------------------


void	AvLogger::Open()
{
	if ( m_pFile ) return;

	SYSTEMTIME localTime;
	::GetLocalTime(&localTime);

	// 현재 위치에서의 상대 패스로 (날짜 시간을 이용하여 다른 이름을 만듬)
	char szDate[MAX_PATH];


	sprintf(szDate, "-%04d%02d%02d_%02d%02d%02d_%d", localTime.wYear,
		localTime.wMonth, localTime.wDay, localTime.wHour, localTime.wMinute,
		localTime.wSecond, GetTickCount() );

	char	szFileName[ 1024 ];

	sprintf_s( szFileName, 1024, "%s%s.txt", m_szFileName, szDate );

	m_pFile		= fopen( szFileName, "wtc, ccs=UNICODE" );	 // c means : Enable the commit flag for the associated filename so that the contents of the file buffer are written directly to disk if either fflush or _flushall is called.

	m_nOpenHour	= localTime.wHour;

	if ( m_bBulkInserterBatchFile )
	{
		if ( m_pBulkBatchFile == NULL )
		{
			sprintf_s( szFileName, 1024, "%s%s.bat", m_szFileName, szDate );

			m_pBulkBatchFile = fopen( szFileName, "wtc" );	 // c means : Enable the commit flag for the associated filename so that the contents of the file buffer are written directly to disk if either fflush or _flushall is called.
		}

		if ( m_pBulkBatchFile ) 
		{
			fprintf( m_pBulkBatchFile, "bcp %s..%s in \"%s\" -c -T -t,\r\n", m_dbname.c_str(), m_tablename.c_str(), szFileName );
			fflush( m_pBulkBatchFile );
		}
	}
}

//-------------------------------------------------------------------------------------------------

void	AvLogger::Close()
{
	if ( m_pFile )
	{

		fflush( m_pFile );
		fclose( m_pFile );

		m_pFile = NULL;
	}

	if ( m_pBulkBatchFile ) 
	{
		fflush( m_pBulkBatchFile );
		fclose( m_pBulkBatchFile );
		m_pBulkBatchFile =  NULL; 
	}
}

//-------------------------------------------------------------------------------------------------

__declspec( thread )	int		t_nCurrentWritePos = 0;
__declspec( thread )	BOOL	t_bLogOn = TRUE;
__declspec( thread )	char	t_buffer[ AvLogger::MAX_LOG_CHARACTER_PER_LINE ];

void	AvLogger::operator<<( AvLogger::End& term )
{
	*this << "\n";

	if ( t_nCurrentWritePos > 0 && t_nCurrentWritePos < MAX_LOG_CHARACTER_PER_LINE )
	{
		char	header[0x80];
		int		nHeaderSize = 0;

		t_buffer[ t_nCurrentWritePos ] = 0;	// NULL_TERMINATE;
		++t_nCurrentWritePos;

		__try
		{
			SYSTEMTIME t;
			::GetLocalTime(&t);

			if ( m_nLogHour != t.wHour )
			{
				m_nLogHour = t.wHour;
			}

			if ( m_bAddPrefix )
			{

				sprintf_s( header, 0x80, "(%4d)%04d-%02d-%02d[%02d:%02d:%02d:%03d] ",
					GetCurrentThreadId(),
					t.wYear, t.wMonth, t.wDay,
					t.wHour, t.wMinute, t.wSecond, t.wMilliseconds);
				
				nHeaderSize = (int)strlen(header);

				_LogBuff( header, nHeaderSize, t_buffer, t_nCurrentWritePos ); 
			}
			else
			{
				_LogBuff( header, 0, t_buffer, t_nCurrentWritePos );
			}
		}
		__except( 1 )
		{

		}
	}

	t_nCurrentWritePos = 0;

}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( AvLogger::Critical&	term )
{
	t_nCurrentWritePos = 0;

	*this << "[CRITICAL]" << term.m_szFunc << "(" << term.m_nLine << ")";

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( AvLogger::Error&		term )
{
	t_nCurrentWritePos = 0;

	*this << "[ERROR]" << term.m_szFunc << "(" << term.m_nLine << ")";

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( AvLogger::Info&		term )
{
	t_nCurrentWritePos = 0;

	*this << "[INFO]";

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( AvLogger::DebugOnly&	term )
{
#ifdef	KEEP_KEEP_GOING
	t_nCurrentWritePos = -1;
#else
	t_nCurrentWritePos = 0;
	*this << "[DEBUG]";
#endif

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( const char* data )
{
	

	if ( data && t_nCurrentWritePos >= 0 )
	{
		int nSize = (int)strlen( data );

		if ( nSize > 0 && nSize + t_nCurrentWritePos < MAX_LOG_CHARACTER_PER_LINE )
		{
			memcpy( t_buffer + t_nCurrentWritePos, (void*)data, nSize );
			t_nCurrentWritePos += nSize;
		}
	}

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( const wchar_t* data )
{
	if ( data && t_nCurrentWritePos >= 0 && wcslen( data ) < 1024 )
	{
		char	temp[ 4096 ];
		sprintf_s( temp, 4096, "%S", data );
		*this << temp;
	}

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( std::wstring&		data )
{
	if ( data.length() > 0 )
	{
		*this << data.c_str();
	}

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( std::string&		data )
{
	if ( data.length() > 0 )
	{
		*this << data.c_str();
	}

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( unsigned char		data )
{
	char	temp[ 1024 ];
	sprintf_s( temp, 1024, "%d", data );
	*this << temp;

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( bool				data )
{
	if ( data )		*this << "(true)";
	else			*this << "(false)";

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( __int16			data )
{
	char	temp[ 1024 ];
	sprintf_s( temp, 1024, "%d", data );
	*this << temp;

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( __int32			data )
{
	char	temp[ 1024 ];
	sprintf_s( temp, 1024, "%d", data );
	*this << temp;

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( __int64			data )
{
	char	temp[ 1024 ];
	sprintf_s( temp, 1024, "%I64d", data );
	*this << temp;

	return *this;
}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( unsigned __int16	data )
{
	char	temp[ 1024 ];
	sprintf_s( temp, 1024, "%d", data );
	*this << temp;

	return *this;

}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( unsigned __int32	data )
{
	char	temp[ 1024 ];
	sprintf_s( temp, 1024, "%d", data );
	*this << temp;

	return *this;

}

//-------------------------------------------------------------------------------------------------

AvLogger&	AvLogger::operator<<( unsigned __int64	data )
{
	char	temp[ 1024 ];
	sprintf_s( temp, 1024, "%I64d", data );
	*this << temp;

	return *this;

}

//-------------------------------------------------------------------------------------------------
#ifdef _SERVER_
static AvLogger serverlogger( "Logfile\\Server" );
#else
static AvLogger serverlogger( "Avatars" );
#endif

static AvLogger chatlogger( "Logfile\\ChatLog" );

AvLogger* GetAvGlobalLogger()
{
	return &serverlogger;
}

AvLogger* GetAvChatLogger()
{
	return &chatlogger;
}


void MakeNullTerminatedString( char* szString, const unsigned int nMaxLength )
{
	szString[nMaxLength-1] = 0;
}

void MakeNullTerminatedString( wchar_t* wcsString, const unsigned int nMaxLength )
{
	wcsString[nMaxLength-1] = 0;
}
