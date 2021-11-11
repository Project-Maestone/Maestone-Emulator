#include "stdafx.h"
#include "AvIocpFile.h"

AvIocpFile::AvIocpFile() : 
	m_hFile(NULL), m_lWrittenLength(0), m_pLastBuffer(0), m_pFirstBuffer(0), m_nWritePending(0), m_bIsClosing(0), m_bDeleteOnClose(0)
{
	m_FileOffset.QuadPart = 0;
	m_nRecursion = 0;
}

AvIocpFile::~AvIocpFile() 
{
	_Close();
}

void AvIocpFile::_Close()
{
	if (m_hFile) {
		CloseHandle(m_hFile);
		m_hFile = 0;
	}

	if (m_bDeleteOnClose) {
		m_bDeleteOnClose = FALSE;
		delete this;
	}
}

void AvIocpFile::Close(BOOL deleteOnClose)
{
	m_csLock.Enter();
	m_bDeleteOnClose = deleteOnClose;
	if (m_nWritePending > 0) {
		m_bIsClosing = TRUE;
	}
	else {
		_Close();
	}
	m_csLock.Leave();
}

bool AvIocpFile::Open(TCHAR *lpszFileName)
{
	AvIocpBuffer::InitFreeList();

	m_hFile = ::CreateFile(lpszFileName, GENERIC_WRITE, FILE_SHARE_READ, 0, CREATE_ALWAYS, FILE_FLAG_WRITE_THROUGH|FILE_FLAG_OVERLAPPED, 0);
	if (m_hFile == INVALID_HANDLE_VALUE) {
#ifndef KEEP_KEEP_GOING
		dprintfW(L"AvIocpFile::Open() - CreateFile %s failed, error = 0x%08x\n", M2W_CHANGE_UNICODE(lpszFileName), GetLastError() );
#endif
		return false;
	}

	if (AvIoBase::BindIOCP( (SOCKET)m_hFile, this ) != TRUE ) {
#ifndef KEEP_KEEP_GOING
		dprintfW(L"AvIocpFile::Open()- BindIOCP Failed (%u)\n", GetLastError() );
#endif
		Close();
		return false;
	}

	return true;
}

BOOL AvIocpFile::Write(const BYTE* pBuffer, int size)
{
	// Write에서 Write를 다시 부르는 상황 회피
/*	if ( m_nRecursion > 1) {
		dprintfW(L"AvIocpFile::Write - depth %d\n", m_nRecursion);
		m_nRecursion--;
		return FALSE;
	}
*/
	m_csLock.Enter();
	// 더 이상 못쓴다....
	if (m_bIsClosing) {
//		m_nRecursion--;
		m_csLock.Leave();
		return FALSE;
	}

	LONG pendingSize = ::InterlockedExchangeAdd(&m_nWritePending, (LONG)size);
	LONG currentPending = pendingSize + size;
	if (currentPending > LIMIT_PENDING_WRITE)
	{
//		m_nRecursion--;
		m_csLock.Leave();
//		dprintfW(L"AvIocpFile::Write() - too many pending bytes (%u)\n", currentPending );
		return FALSE;
	}

	if ( size > SIZE_IOCP_BUFFER )
	{
		m_csLock.Leave();
		return TRUE;
	}

	// pendingSize가 0보다 크면, 이미 pending buffer가 있는 것이니 거기에 추가하고 리턴하면 된다.
	if (pendingSize > 0)
	{
		if (0 == m_pLastBuffer)
			m_pFirstBuffer = m_pLastBuffer = AvIocpBuffer::PopFreeBuffer();

		if ((m_pLastBuffer->m_nLength + size) > SIZE_IOCP_BUFFER)
			m_pLastBuffer = m_pLastBuffer->m_pNext = AvIocpBuffer::PopFreeBuffer();

		memcpy(m_pLastBuffer->m_buffer + m_pLastBuffer->m_nLength, pBuffer, size);
		m_pLastBuffer->m_nLength += size;
//		m_nRecursion--;
		m_csLock.Leave();
		return TRUE;
	}
	else
	{
		AvIocpBuffer* p = AvIocpBuffer::PopFreeBuffer();
		memcpy(p->m_buffer, pBuffer, size);
		p->m_nLength = size;
//		m_nRecursion--;
		BOOL retval = DoWrite(p);
		m_csLock.Leave();
		return retval;
	}
}

void AvIocpFile::OnWriteDone(AvIocpBuffer *pBuffer, DWORD dwBytesTransfered)
{
	if ( m_hFile == INVALID_HANDLE_VALUE)
		return;

	// 에러가 발생한 것이니, 더 진행 못한다.
	if ( dwBytesTransfered == 0 )
	{
		_Close();
		return;
	}

	m_csLock.Enter();

	::InterlockedExchangeAdd(&m_nWritePending, dwBytesTransfered * (-1));

	m_lWrittenLength = m_lWrittenLength + dwBytesTransfered;

	if (m_pFirstBuffer)
	{
		AvIocpBuffer* p = m_pFirstBuffer;
		m_pFirstBuffer = m_pFirstBuffer->m_pNext;

		DoWrite(p);
	}

	if (0 == m_pFirstBuffer)
	{
		m_pLastBuffer = 0;
	}

	AvIocpBuffer::PushFreeBuffer(pBuffer);

	// 더 이상 쓸 게 없고, m_bIsClosing이 TRUE이면 닫아줌
	if (m_nWritePending == 0 && m_bIsClosing)
		_Close();
	m_csLock.Leave();
}

BOOL AvIocpFile::DoWrite(AvIocpBuffer* p)
{
	DWORD dwBytes = 0;

	p->Init(AvIoBase::Send, p->m_buffer, p->m_nLength);

	// 사실 전체가 CRITICAL SECTION으로 감싸져있으니.... Interlocked를 쓰는 게...
	/*
	DWORD offset = ::InterlockedExchangeAdd((LONG *)&m_FileOffset.LowPart, p->m_nLength);
	if (offset > m_FileOffset.LowPart)
		m_FileOffset.HighPart++;
	p->Offset = offset;
	p->OffsetHigh = m_FileOffset.HighPart;
	*/

	p->Offset = m_FileOffset.LowPart;
	p->OffsetHigh = m_FileOffset.HighPart;

	m_FileOffset.QuadPart += p->m_nLength;


	if ( WriteFile( m_hFile, p->m_buffer, p->m_nLength, &dwBytes, (WSAOVERLAPPED *)p ) != 0 )
	{
		DWORD errcode = GetLastError();
		if( ERROR_IO_PENDING != errcode )
		{
			if (errcode != ERROR_SUCCESS) {
#ifndef KEEP_KEEP_GOING
				dprintfW(L"AvIocpFile::DoWrite - WriteFile failed %d\n", errcode);
#endif
				Close();
				return FALSE;
			}
		}
	}

	return TRUE;
}

void AvIocpFile::Flush()
{
	while (m_pFirstBuffer)
	{
		AvIocpBuffer* p = m_pFirstBuffer;
		m_pFirstBuffer = m_pFirstBuffer->m_pNext;

		DWORD written;
		::WriteFile(m_hFile, p->m_buffer, p->m_nLength, &written, NULL);
	}
}

BOOL AvIocpFile::Read(AvIocpBuffer* pBuffer)
{
	// not supported 
	return FALSE;
}

void AvIocpFile::OnReadDone(AvIocpBuffer* pBuffer, DWORD dwBytesTransfered)
{

}

VOID AvIocpFile::OnIoComplete( DWORD dwBytesTransfered, AvOverlap* pBuffer )
{
	if ( pBuffer->nIoType == AvIoBase::Send) 
		OnWriteDone( (AvIocpBuffer *)pBuffer, dwBytesTransfered );
	else 
		OnReadDone( (AvIocpBuffer *)pBuffer, dwBytesTransfered );
}
