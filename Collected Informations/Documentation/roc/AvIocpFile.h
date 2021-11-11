#pragma once

#include "AvIoBaseCore.h"
#include "CBSyncObject.h"

#define SIZE_IOCP_BUFFER				(1024*16)	// 16K�� ����
#define LIMIT_PENDING_WRITE				(0x200000)	// �������ų� �����ų��� ���� ���Ѱ�
#define MAX_AVIOCP_FREE_BUFFER_COUNT	10

class AvIocpBuffer;

typedef struct _AvIocpBufferItem {
	SLIST_ENTRY ItemEntry;
	AvIocpBuffer *pBuffer; 

	_AvIocpBufferItem();
	~_AvIocpBufferItem();
} AvIocpBufferItem;

//
// File write�� ���ؼ� ����ϴ� IOCP ����
//
// SList�� MEMORY_ALLOCATION_ALIGNMENT�� �°� �Ҵ�� ��鸸 ���� �ִ�. 
// alignment�Ҵ��� ���� �����Ƿ� ���������� ����
//
// From MSDN : All list items must be aligned on a MEMORY_ALLOCATION_ALIGNMENT boundary. 
// Unaligned items can cause unpredictable results.
//
// 
class AvIocpBuffer : public AvOverlap
{
	friend class AvIocpFile;

public:
	AvIocpBuffer(AvIocpBufferItem* pListEntry = NULL) {
//		m_pListEntry = pListEntry;
		m_pNext = 0;
		m_nLength = 0;
	}
	virtual ~AvIocpBuffer()	{}

private:
	char				m_buffer[ SIZE_IOCP_BUFFER ];

	AvIocpBuffer*		m_pNext;
	unsigned int		m_nLength;

//	AvIocpBufferItem*	m_pListEntry;

public:
	/*
	static SLIST_HEADER  ms_freeBufferHead;
	static LONG			 ms_freeBufferInited;
	*/

	static void InitFreeList() {
		/*if (!InterlockedCompareExchange(&ms_freeBufferInited, 1, 0))
			InitializeSListHead(&ms_freeBufferHead);
		*/
	}

	static AvIocpBuffer *PopFreeBuffer() {
		/*
		AvIocpBufferItem *pEntry = (AvIocpBufferItem *)InterlockedPopEntrySList(&ms_freeBufferHead);
		if (pEntry) {
			pEntry->pBuffer->m_nLength = 0;
			return pEntry->pBuffer;
		}
		*/

		AvIocpBufferItem *pItem = new AvIocpBufferItem;
		if (pItem) 
			return pItem->pBuffer;
		else
			return NULL;
	}

	static void PushFreeBuffer(AvIocpBuffer *pBuffer) {
		//if (!pBuffer->m_pListEntry || QueryDepthSList(&ms_freeBufferHead) > MAX_AVIOCP_FREE_BUFFER_COUNT)
			delete pBuffer;
		/*else
			InterlockedPushEntrySList(&ms_freeBufferHead, &pBuffer->m_pListEntry->ItemEntry);
		*/
	}
};


inline _AvIocpBufferItem::_AvIocpBufferItem() 
{
	pBuffer = new AvIocpBuffer(this);
	ItemEntry.Next = NULL;
}

inline _AvIocpBufferItem::~_AvIocpBufferItem() 
{
	if (pBuffer)
		delete pBuffer;
}

/*
__declspec(selectany) SLIST_HEADER AvIocpBuffer::ms_freeBufferHead;
__declspec(selectany) LONG AvIocpBuffer::ms_freeBufferInited;
*/

class AvIocpFile : public AvIoBase {
	HANDLE				m_hFile;

	CBCriticalSection	m_csLock;

	LONGLONG			m_lWrittenLength;

	AvIocpBuffer*		m_pFirstBuffer;
	AvIocpBuffer*		m_pLastBuffer;

	LONG				m_nWritePending;
	LARGE_INTEGER		m_FileOffset;

	BOOL				m_bIsClosing;
	BOOL				m_bDeleteOnClose;

	LONG				m_nRecursion;

public:
	AvIocpFile();
	~AvIocpFile();

	HANDLE	GetHandle()	{ return m_hFile; }

	virtual void OnIoComplete( DWORD dwBytesTransfered, AvOverlap * pOverlapped );

	bool Open(TCHAR *lpszFileName);
	void Close(BOOL bDeleteOnClose = FALSE);

	BOOL Write(const BYTE* pBuffer, int size);
	void Flush();
	BOOL Read(AvIocpBuffer* pBuffer);;

	LONGLONG GetFileOffset() { return m_FileOffset.QuadPart; }
	LONG GetLowOffset() { return m_FileOffset.LowPart; }

protected:
	void _Close();
	void OnWriteDone(AvIocpBuffer *pBuffer, DWORD dwBytesTransfered);
	BOOL DoWrite(AvIocpBuffer* p);
	void OnReadDone(AvIocpBuffer* pBuffer, DWORD dwBytesTransfered);
};
