#pragma once
#ifndef __SYSTEMTIMEEX_H__
#define __SYSTEMTIMEEX_H__


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Version 0.1																												 //
// ����:																													 //
//      SYSTEMTIME�� Milliseconds�� exchange																				 //
//		Milliseconds�� SYSTEMTIME�� exchange																				 //
//		1600���� �������� �̱⿡ �Է��� ���� 1600���϶�� Error																 //
//		�Է��� SYSTEMTIME�� �⺻���� ������ ������ ����� Error															 //
//		����� ������ ������ Ȯ���ϹǷ� �����Ұ�.																			 //
//																															 //
//																															 //
// ����:																													 //
//		AfterThisDays(1,2,3)				<= 1�� 3�� �����ؼ� ��������� 2�� ����.										 //
//		BeforeThisDays(1,2,3)				<= ���� ��																	 //
//		DifferDays(1,2)						<= 1�� �ֱٰ�(������) 2�� ���Ű�(������)�� ���̸� �и� �ʷ� �����Ѵ�.			 //
//											   ������ ��� -���� ���ϵȴ�.													 //
// �����Լ�(���ο�):																										 //
//		IsBissextileYear(1)					<= 1���� �������� �Ǻ��ؼ� �����̸� TRUE �ƴϸ� FALSE�� �����Ѵ�.				 //
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
	//����� ����� ��¥																		//
	//1582�� 10��4�� ���� ��¥�� 10�� 15���̴�.													//
	//1582�� ������ ���̴� �޷��� �����콺�� ������������ �׷����¼���... 10�� �ǳʶ�			//
	//////////////////////////////////////////////////////////////////////////////////////////////

	


//���س��̻��� �Է��ؾ��Ѵ�.
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




















