#pragma once



#include "AvMemoryMgr.h"
#include "SfCompileOption.h"


#if !defined(__AFX_H__) && !defined(POSITION)
struct __POSITION { };
typedef __POSITION* POSITION;
#endif

#include <windows.h>
#include <string>
using namespace std;

#define CALC_PATH_MOVE_ING				0
#define CALC_PATH_MOVE_OK				1
#define CALC_PATH_FIND_AI				2
#define CALC_PATH_SAME_DEST				3

#define INVALID_ATTACK_RADIAN			10000
#define RADIAN_MAX						314

extern void		InitializeDistanceTable();

extern float	GetDistanceFromTable( long, long );
extern float	GetDistanceFromTable( long, long, long );
extern float	GetDistanceFromOnlyTable( long, long );
extern float	GetSquareDistanceFromTable( long, long );
extern float	GetSquareDistanceFromTable( long, long, long );
extern float	GetRadian( long dx, long dy );
extern float	GetSIN( long dx, long dy );
extern float	GetCOS( long dx, long dy );
extern float	GetRSIN( int nRadian );
extern float	GetRCOS( int nRadian );

extern short	Calc_Degree_To_Short( long, long, long, long );

//extern void		Calc_Vector_From_Direction(	unsigned short, unsigned short, unsigned short&, 
//											unsigned short&, unsigned char, unsigned short );

// stan.location
extern long		Calc_Vector_From_Coord( unsigned short, unsigned short, unsigned short, unsigned short, unsigned short, 
										unsigned short&, unsigned short&/*, unsigned short range=0 */);
extern long		Calc_Vector_From_Coord( float, float, float, float, float, 
									   float&, float&/*, unsigned short range=0 */);

extern void		Calc_Knock_Position(	unsigned short, unsigned short, unsigned short, unsigned short, unsigned short, 
										unsigned short&, unsigned short& );

extern int  ConvertToDigit(char* szNumber, int nType);

//////////////////////////////////////////////////////////////////////////
inline unsigned __int64 to_integral( const SYSTEMTIME& st ) {   
	FILETIME ft;
	SystemTimeToFileTime( &st, &ft );
	ULARGE_INTEGER integer;
	integer.LowPart = ft.dwLowDateTime;
	integer.HighPart = ft.dwHighDateTime;
	return integer.QuadPart;
}


/*
	랜덤함수들

*/
extern	int		GetRandomValue( int min, int max );

// 전투 및 기타 오퍼레이션용
extern	int		GetRandomEx();					// 만분율로 하던것용 return 0~9999 values
extern	BOOL	PPercent( int nRatio );			// 0~100% return TRUE
extern	BOOL	PDefaultRate( int nRatio );		// 만분율 0 : 0%  - 10000 : 100 % 로 TRUE 리턴
		 
// 인챈트용
extern	int		GetRandomForEnchant();						// return 0~9999 values
extern	BOOL	PPercentForEnchant( int nRatio  );			// 0~100% return TRUE
extern	BOOL	PDefaultRateForEnchant( int nRatio  );		//  만분율 0 : 0%  - 10000 : 100 % 로 TRUE 리턴


extern void FindFiles( string szDirectory);
extern bool SetRegistyStartProgram(bool bAutoExec, LPCSTR lpValueName, LPCSTR lpExeFileName);


//typedef unsigned (__stdcall *PTHREAD_START)(void*);
/*
typedef unsigned (__cdecl *PTHREAD_START)(void*);

#define BEGINTHREADEX(psa, cbStack, pfnStartAddr, pvParam, fdwCreate, pdwThreadId) \
	((HANDLE)_beginthreadex((void*)(psa), \
	(unsigned)(cbStack), (PTHREAD_START)(pfnStartAddr),\
	(void*)(pvParam), (unsigned)(fdwCreate), (unsigned*)(pdwThreadId)))
*/


#define CHECK_TIME_START \
						__int64 freq, start, end; \
						if( QueryPerformanceFrequency((_LARGE_INTEGER*)&freq) ) \
						{\
							QueryPerformanceCounter((_LARGE_INTEGER*)&start);

#define CHECK_TIME_END(a,b) \
							QueryPerformanceCounter((_LARGE_INTEGER*)&end); \
							a=(float)((double)(end-start)/freq*1000); b=TRUE; \
						} \
						else b=FALSE; \
						


extern void AvInitializeTickCount();
extern unsigned long AvGetTickCount();


#include "JlAssert.h"
#include "JlDbgOut.h"

#include "JlBitSet32.h"
#include "SfIniFile.h"
#include "JlRandom.h"
#include "CBSyncObject.h"

#include "JlList.h"
#include "JlMap.h"

#include "CBBlockMemory.h"
#include "CBThreadCtrl.h"
#include "JlMemPool.h"
#include "ado_db.h"

#define	REPORT_ONCE()																				\
{																									\
	static BOOL __ERROR_ONCE = TRUE;																\
	if ( __ERROR_ONCE ) { DBGERR(L"[ERROR] %s %d\n", M2W_CHANGE_UNICODE(__FILE__), __LINE__ ); __ERROR_ONCE = FALSE; }	\
}

#define	REPORT_CRITICAL()													\
{																			\
	DBGERR(L"[CRITICAL] %s %d\n", M2W_CHANGE_UNICODE(__FILE__), __LINE__ ); 	\
}

#define	REPORT_ONCE_EX(X)																						\
{																												\
	static BOOL __ERROR_ONCE_EX = TRUE;																			\
	if ( __ERROR_ONCE_EX ) { DBGERR(L"[ERROR][%s] %s %d\n",X, M2W_CHANGE_UNICODE(__FILE__), __LINE__ ); __ERROR_ONCE_EX = FALSE; }	\
}

#define	REPORT_ONCE_NUM(X)																						\
{																												\
	static BOOL __ERROR_ONCE_EX = TRUE;																			\
	if ( __ERROR_ONCE_EX ) { DBGERR(L"[ERROR][%d] %s %d\n",X, M2W_CHANGE_UNICODE(__FILE__), __LINE__ ); __ERROR_ONCE_EX = FALSE; }	\
}

#define	REPORT_ERROR(STRING)										\
{																	\
	DBGERR(L"[ERROR] %s(%d) %s\n", M2W_CHANGE_UNICODE(__FILE__), __LINE__, STRING ); 	\
}

#define IS_INVALID_POINTER(x)	( (DWORD)(x) >= 0 && (DWORD)(x) < 0x0800 )
#define IS_VALID_POINTER(x)		(!IS_INVALID_POINTER(x))

#define CHECK_INVALID_POINTER(x) \
	if (IS_INVALID_POINTER(x)) { \
		DBGERR(L"[ERROR] pointer invalid (%x) %s %d\n", x, M2W_CHANGE_UNICODE(__FILE__), __LINE__ ); \
		return; \
	}

#define CHECK_INVALID_POINTER_RETVAL(x, y) \
	if (IS_INVALID_POINTER(x)) { \
		DBGERR(L"[ERROR] pointer invalid (%x) %s %d\n", x, M2W_CHANGE_UNICODE(__FILE__), __LINE__ ); \
		return (y); \
	}

#define IS_VALID_ARRAY_INDEX(val, maxval) \
	((val) >= 0 && (val) < (maxval))

// 버그 확률을 줄이기 위해 기존의 매크로에서 인라인 펑션으로 대체
__inline void COPY_STRING( char* dst, const char* src, size_t maxlen) 
{ 
	strncpy( dst, src, maxlen ); 
	dst[maxlen-1] = 0; 
}

// 버그 확률을 줄이기 위해 기존의 매크로에서 인라인 펑션으로 대체
__inline void COPY_WSTRING( wchar_t* dst, const wchar_t* src, size_t maxlen) 
{ 
	wcsncpy( dst, src, maxlen ); 
	dst[maxlen-1] = 0; 
}

//포인터 연산등 길이를 계산하여 쓰는 부분들은 maxlen이 존재할 수 없으므로 모두 복사하자
__inline void COPY_STRING_EX( char* dst, const char* src, size_t cpylen) 
{ 
	strncpy( dst, src, cpylen ); 
}
__inline void COPY_WSTRING_EX( wchar_t* dst, const wchar_t* src, size_t cpylen) 
{ 
	wcsncpy( dst, src, cpylen ); 
}

#ifndef	SAFE_DELETE
#define	SAFE_DELETE( X )		{ if (X) delete X; X=0; }
#endif

#ifndef	SAFE_DELETE_ARRAY
#define	SAFE_DELETE_ARRAY( X )	{ if (X) delete [] X; X=0; }
#endif

#define	DBGERR_INVALID_PROTOCOL( p )		\
	DBGERR(L"[INVALID PROTOCOL] Main=%d/Sub=%d at %s(%d)\n", p->nMain, p->nSub, M2W_CHANGE_UNICODE(__FUNCTION__), __LINE__ );


#pragma comment( lib, "Psapi.lib")
#pragma comment( lib, "pdh" )
