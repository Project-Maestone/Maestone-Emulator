/*

	Logger for RocWorks

	Simple and NonBlocking Logger Class

	by IEEIE

*/
#pragma once

#include <string>

class	AvLogger
{
public:

	const	static	int	MAX_LOG_CHARACTER_PER_LINE = 4096;

	struct	End				{};
	struct	Info			{};
	struct	DebugOnly		{};

	class	Critical
	{
	public:	

		const char*		m_szFunc;
		unsigned int	m_nLine;

		Critical( const char* szFunc, unsigned int nLine )
		{
			m_szFunc = szFunc;
			m_nLine = nLine;
		}
	};

	class	Error
	{
	public:

		const char*		m_szFunc;
		unsigned int	m_nLine;

		Error( const char* szFunc, unsigned int nLine )
		{
			m_szFunc = szFunc;
			m_nLine = nLine;
		}
	};




	AvLogger( const char* szFileName );
	~AvLogger();

	void	Term();

	void	SetAddLinePrefix( bool bAddPrefix )				{ m_bAddPrefix = bAddPrefix; }
	void	SetMakeNewFileOnEveryHour(bool bMakeNewFile)	{ m_bMakeNewFileOnEveryHour = bMakeNewFile; }
	void	SetFlushOnEveryWrite(bool bFlush)				{ m_bFlushOnEveryWrite = bFlush; }

	void	MakeBulkInserterBatchFile( const char* dbname, const char* tablename )				
	{ 
		m_bBulkInserterBatchFile = true; 
		m_dbname	= dbname;
		m_tablename = tablename;
	}

	void	Log( const wchar_t* format, ... );

	// stream like methods
	void		operator<<( End&				term );
	AvLogger&	operator<<( Critical&			term );
	AvLogger&	operator<<( Error&				term );
	AvLogger&	operator<<( Info&				term );
	AvLogger&	operator<<( DebugOnly&			term );

	AvLogger&	operator<<( const char*			data );
	AvLogger&	operator<<( const wchar_t*		data );
	AvLogger&	operator<<( std::wstring&		data );
	AvLogger&	operator<<( std::string&		data );
	AvLogger&	operator<<( unsigned char		data );
	AvLogger&	operator<<( bool				data );
	AvLogger&	operator<<( __int16				data );
	AvLogger&	operator<<( __int32				data );
	AvLogger&	operator<<( __int64				data );
	AvLogger&	operator<<( unsigned __int16	data );
	AvLogger&	operator<<( unsigned __int32	data );
	AvLogger&	operator<<( unsigned __int64	data );

	// usage	LOG_CRITICAL << "What the hell of value username is" << GetNickName() << LOG_END

public:
	static volatile bool ms_bDoLog;

	static void		StopLogger()	{ ms_bDoLog = false; }

public:
	char *		GetPath()	{ return m_szFileName; }

private:

	void	_Log( const wchar_t* szHeader, int headerlen, const wchar_t* format, va_list& args, int nPrintSize );
	void	_LogBuff( const char* szHeader, int headerlen, const char* str, int nPrintSize );
	void	Open();
	void	Close();

 	FILE*			m_pFile;
	FILE*			m_pBulkBatchFile;
	HANDLE			m_hIOCP;
	HANDLE			m_hFileWriteThread;
	WORD			m_nLogHour;
	WORD			m_nOpenHour;
	char			m_szFileName[ 256 ];
	bool			m_bAddPrefix;
	bool			m_bBulkInserterBatchFile;
	bool			m_bMakeNewFileOnEveryHour;
	bool			m_bFlushOnEveryWrite;
	std::string		m_dbname;
    std::string		m_tablename;


	// Write Thread
	void							RunWrite();
	static unsigned int __stdcall	FileWriteThread( void* pAvLogger );
};

extern	AvLogger* GetAvGlobalLogger();
extern	AvLogger* GetAvChatLogger();

#define	LOG_CRITICAL		*GetAvGlobalLogger() <<	AvLogger::Critical( __FUNCTION__, __LINE__ )
#define	LOG_ERROR			*GetAvGlobalLogger() <<	AvLogger::Error( __FUNCTION__, __LINE__ )
#define	LOG_INFO			*GetAvGlobalLogger() <<	AvLogger::Info()
#define	LOG_DEBUG			*GetAvGlobalLogger() <<	AvLogger::DebugOnly( __FUNCTION__, __LINE__ )
#define	LOG_END				AvLogger::End()


//#ifndef KEEP_KEEP_GOING
//#define PRINT( str, ... ) OutputDebugStringA( FormatString(str, __VA_ARGS__) );
//#else
//#define PRINT( A, ... ) OutputDebugStringA( FormatString(A, __VA_ARGS__) );
//#endif

enum ELOG_LEVEL
{
	ELL_BASIC,
	ELL_EXTENDED,
	ELL_DEBUG
};


class FormatString
{
public:
	FormatString(const char *str, ...) {
		va_list vl;
		va_start(vl,str);
		vsprintf_s(m_szBuffer, 0x1000, str, vl);
		va_end(vl);
	}

	operator const char* () {
		return m_szBuffer;
	}

private:
	char m_szBuffer[0x1000];
};

class FormatStringW
{
public:
	FormatStringW(const wchar_t *str, ...) {
		va_list vl;
		va_start(vl,str);
		vswprintf_s(m_szBuffer, 0x1000, str, vl);
		va_end(vl);
	}

	operator const wchar_t* () {
		return m_szBuffer;
	}

private:
	wchar_t m_szBuffer[0x1000];
};

void MakeNullTerminatedString( char* szString, const unsigned int nMaxLength );
void MakeNullTerminatedString( wchar_t* wcsString, const unsigned int nMaxLength );


//#define 