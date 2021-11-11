#pragma once
#ifndef __SYSTEMTIMEEX_H__
#define __SYSTEMTIMEEX_H__


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Version 0.1																												 //
// 설명:																													 //
//      SYSTEMTIME에 Milliseconds로 exchange																				 //
//		Milliseconds를 SYSTEMTIME로 exchange																				 //
//		1600년을 기준으로 뽑기에 입력한 년이 1600이하라면 Error																 //
//		입력한 SYSTEMTIME이 기본적인 연월의 범위를 벗어나면 Error															 //
//		윤년과 각월의 말일을 확인하므로 참고할것.																			 //
//																															 //
//																															 //
// 사용법:																													 //
//		AfterThisDays(1,2,3)				<= 1과 3을 연산해서 결과값으로 2를 쓴다.										 //
//		BeforeThisDays(1,2,3)				<= 위와 상동																	 //
//		DifferDays(1,2)						<= 1에 최근값(높은값) 2에 과거값(낮은값)의 차이를 밀리 초로 리턴한다.			 //
//											   역순의 경우 -값이 리턴된다.													 //
// 참고함수(내부용):																										 //
//		IsBissextileYear(1)					<= 1년이 윤년인지 판별해서 윤년이면 TRUE 아니면 FALSE를 리턴한다.				 //
//		MillisecondsToSystemtime(1,2)		<= 1 = INPUT,  2 = OUT															 //
//		SystemtimeToMilliseconds(1,2)		<= 1 = INPUT,  2 = OUT															 //
//																															 //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
typedef unsigned short	WORD;
typedef unsigned char	BYTE;
typedef __int64			INT64;
typedef unsigned int	UINT32;


#ifndef DWORD
typedef unsigned long	DWORD;
#endif

#ifndef IN
#define IN
#define OUT
#endif //IN


enum SYSTEMTIMEEX_ERRORCODE
{
	ERRORCODE_SUCCEED			=	0,
	ERRORCODE_NOT_SUPPORT_DAY,
	ERRORCODE_NOT_SUPPORT_MONTH,
	ERRORCODE_NOT_SUPPORT_YEAR,
	ERRORCODE_PARAM_ERROR,
	ERRORCODE_NOT_SUPPORT_CALC
};

//
//#ifndef _SYSTEMTIME
//typedef struct _SYSTEMTIME 
//{
//    WORD wYear;
//    WORD wMonth;
//    WORD wDayOfWeek;
//    WORD wDay;
//    WORD wHour;
//    WORD wMinute;
//    WORD wSecond;
//    WORD wMilliseconds;
//} SYSTEMTIME, *PSYSTEMTIME, *LPSYSTEMTIME;
//#endif //_SYSTEMTIME



const INT64  BASIC_YEAR					=1600;
const INT64  COMMON_YEAR_DAYS			=365;
const INT64  BISSEXTILE_YEAR_DAYS		=366;
const INT64  BISSEXTILE_MONTH			=2;
const INT64  DAY_MILLISECONDS			=86400000;
const INT64  TIME_MILLISECONDS			=(1000*3600);
const INT64  MINUTE_MILLISECONDS			=(1000*60);
const INT64  SECOND_MILLISECONDS			=1000;

const INT64 COMMON_YEAR_MILLISECONDS		= COMMON_YEAR_DAYS*DAY_MILLISECONDS;
const INT64 BISSEXTILE_YEAR_MILLISECONDS	= BISSEXTILE_YEAR_DAYS*DAY_MILLISECONDS;





	//////////////////////////////////////////////////////////////////////////////////////////////
	//역사상 사라진 날짜																		//
	//1582년 10월4일 다음 날짜는 10월 15일이다.													//
	//1582년 이전에 쓰이던 달력은 율리우스력 오차가생기자 그레고리력선포... 10일 건너뜀			//
	//////////////////////////////////////////////////////////////////////////////////////////////

	


//기준년이상을 입력해야한다.
#define STANDARDYEARS		1600



class SystemtimeEx : public SYSTEMTIME
{
public:
	SystemtimeEx(void);
	~SystemtimeEx(void);

	//SystemtimeEx operator +( INT64 & Second );
	SystemtimeEx operator -( SystemtimeEx & rhs ) ;


	static SYSTEMTIMEEX_ERRORCODE	AfterThisDays(IN LPSYSTEMTIME, OUT LPSYSTEMTIME, IN INT64);
	static SYSTEMTIMEEX_ERRORCODE	BeforeThisDays(IN LPSYSTEMTIME, OUT LPSYSTEMTIME,INT64);
	static INT64					DifferDays(IN LPSYSTEMTIME, IN LPSYSTEMTIME);

	static BOOL						IsBissextileYear( DWORD nYear );	
	static SYSTEMTIMEEX_ERRORCODE	MillisecondsToSystemtime( IN INT64 Milliseconds, OUT LPSYSTEMTIME lpOutTime );
	static SYSTEMTIMEEX_ERRORCODE	SystemtimeToMilliseconds( IN LPSYSTEMTIME lpInTime, OUT INT64* p64MSeconds );

public:
	static DWORD	ErrorCodeNumber;
};



VOID AddSystemTime( SYSTEMTIME* pDescTime,SYSTEMTIME* pCurTime, DWORD nDay, DWORD nHour, DWORD nMinute, DWORD nSecond );
long DiffSecond( SYSTEMTIME* pEndTime ,SYSTEMTIME* pCurTime);

#endif //__SYSTEMTIMEEX_H__




















