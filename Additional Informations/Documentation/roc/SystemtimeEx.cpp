
#include "stdafx.h"

#ifndef __SYSTEMTIMEEX_H__
#include ".\SystemtimeEx.h"
#endif //__SYSTEMTIMEEX_H__





DWORD SystemtimeEx::ErrorCodeNumber = NULL;


BYTE CommonYearsArray[12]		=	{ 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };		//���
BYTE BissextileYearsArray[12]	=	{ 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };		//����	


SystemtimeEx::SystemtimeEx(void)
{
	wYear			= MAXWORD;
    wMonth			= MAXWORD;
    wDayOfWeek		= MAXWORD;
    wDay			= MAXWORD;
    wHour			= MAXWORD;
    wMinute			= MAXWORD;
    wSecond			= MAXWORD;
    wMilliseconds	= MAXWORD;
}

SystemtimeEx::~SystemtimeEx(void)
{
}



//SystemtimeEx SystemtimeEx :: operator +( INT64 & MilliSeconds )
//{
//	INT64 ThisMilliSeconds; 
//	SystemtimeEx stResult;
//
//	SYSTEMTIMEEX_ERRORCODE nErrorCode = SystemtimeToMilliseconds(this, &ThisMilliSeconds);
//	if( nErrorCode != ERRORCODE_SUCCEED )
//	{	return stResult;
//	}
//
//	ThisMilliSeconds += MilliSeconds;
//
//	MillisecondsToSystemtime( ThisMilliSeconds, &stResult );
//
//	return stResult;
//}

SystemtimeEx SystemtimeEx :: operator -( SystemtimeEx & rhs )
{

	INT64 MSecond = DifferDays( this, &rhs );

	SystemtimeEx stResult;

	MillisecondsToSystemtime( MSecond, &stResult );
	return stResult;
}





//static
SYSTEMTIMEEX_ERRORCODE SystemtimeEx :: SystemtimeToMilliseconds( IN LPSYSTEMTIME lpSystemTime, OUT INT64* p64MSeconds )
{
	//���� Ȯ��
	if( !lpSystemTime || !p64MSeconds )
	{	return ERRORCODE_PARAM_ERROR;
	}	

	//���� Ȯ��
	if( 0 == lpSystemTime->wDay  )		// DAY: 1- 28,29,30,31 MONTH : 1 -12
	{	ErrorCodeNumber += 1;
		return ERRORCODE_NOT_SUPPORT_DAY;
	}
	if( !IsBissextileYear(lpSystemTime->wYear) && lpSystemTime->wDay >  CommonYearsArray[lpSystemTime->wMonth] )		//���
	{	ErrorCodeNumber += 10;
		return ERRORCODE_NOT_SUPPORT_DAY;
	}
	if( IsBissextileYear(lpSystemTime->wYear) && lpSystemTime->wDay >  BissextileYearsArray[lpSystemTime->wMonth] )		//����
	{	ErrorCodeNumber += 100;
		return ERRORCODE_NOT_SUPPORT_DAY;
	}
	//�� Ȯ��
	if( 0 == lpSystemTime->wMonth || 12 < lpSystemTime->wMonth )
	{	ErrorCodeNumber += 1000;
		return ERRORCODE_NOT_SUPPORT_MONTH;
	}
	//����Ȯ��
	if( BASIC_YEAR > lpSystemTime->wYear )						//YEAR : 1600 ~
	{	ErrorCodeNumber += 10000;
		return ERRORCODE_NOT_SUPPORT_YEAR;
	}


	INT64 AccumulatedDays = 0;
	INT64 nYear = 0;

	//�⵵�� ���ڷ� �ٲٱ�
	for( nYear = BASIC_YEAR; nYear < lpSystemTime->wYear; nYear++ )
	{
		if( IsBissextileYear((DWORD)nYear) )
		{	AccumulatedDays += BISSEXTILE_YEAR_DAYS;
		}
		else
		{	AccumulatedDays += COMMON_YEAR_DAYS;
		}
	}

	//���� ���ڷ� �ٲٱ�
	INT64 nMonths = 0;
	for( nMonths = 1; nMonths < lpSystemTime->wMonth; nMonths++  )
	{		
		
		if( IsBissextileYear((DWORD)nYear) && nMonths == BISSEXTILE_MONTH ) // �����̸鼭 2���̸�
		{
			AccumulatedDays += BissextileYearsArray[nMonths-1];
		}
		else
		{	AccumulatedDays += CommonYearsArray[nMonths-1];
		}
	}

	AccumulatedDays += (lpSystemTime->wDay-1);						// 1�� ���� ���� : �Ѵ��� 1�Ϻ��� �����ϹǷ� 
																	// ��Ȯ�� ������ �������� Ȯ���ϱ� ����...




	INT64 AccumulatedMiliSeconds = 0;

	//�и�
	AccumulatedMiliSeconds = lpSystemTime->wMilliseconds;

	//��
	AccumulatedMiliSeconds += lpSystemTime->wSecond * 1000;

	//��
	AccumulatedMiliSeconds += (lpSystemTime->wMinute * 60 ) * 1000;

	//��
	AccumulatedMiliSeconds += ( ( lpSystemTime->wHour * 60 ) * 60 ) * 1000;



	//���� �и�������
	AccumulatedMiliSeconds += (AccumulatedDays * DAY_MILLISECONDS);

	*p64MSeconds = AccumulatedMiliSeconds;

	return ERRORCODE_SUCCEED;

}


//static
BOOL SystemtimeEx::IsBissextileYear( DWORD nYear )				//���ش� �����ΰ� �ƴѰ�.
{
	return ( ( nYear % 4 ) == 0 && ( ( nYear % 100 ) != 0 || ( nYear % 400 ) == 0 ) );
}



//static								
SYSTEMTIMEEX_ERRORCODE SystemtimeEx :: MillisecondsToSystemtime(  IN INT64 Milliseconds, OUT LPSYSTEMTIME lpOutTime )
{																											
	if(!lpOutTime )
	{	return ERRORCODE_PARAM_ERROR;
	}


	INT64		nYears				= BASIC_YEAR;
	INT64		nMonths				= 0;

	INT64		nDays				= Milliseconds / DAY_MILLISECONDS;
	INT64		nModDays			= Milliseconds % DAY_MILLISECONDS;			//�Է°��� ��¥�� �ɰ��� �� ������ ��
	

	INT64		nTimes				= nModDays / TIME_MILLISECONDS;
	INT64		nModTimes			= nModDays % TIME_MILLISECONDS;				//��¥�� �ɰ��� �� ������ ������ �ð����� �ɰ��� �� ������ ��

	INT64		nMinutes			= nModTimes / MINUTE_MILLISECONDS;
//	INT64		nModMinutes			= nModTimes % MINUTE_MILLISECONDS;			//...// ieeie@w4build

	INT64		nSeconds			= nModTimes / SECOND_MILLISECONDS;
	INT64		nModSeconds			= nModTimes % SECOND_MILLISECONDS;			//...

	INT64		nMiliSeconds		= nModSeconds;								//...

	
	
//	INT64 nAccumulatedDays = 0;		// // ieeie@w4build

	for( nYears; ; nYears++ )													// ������ �� BASIC_YEAR���� �⵵�� �������� ���Ѵ�.
	{
		
		if( IsBissextileYear((DWORD)nYears) )
		{
			if( nDays < BISSEXTILE_YEAR_DAYS )
			{	break;
			}
			nDays -= BISSEXTILE_YEAR_DAYS;			

		}
		else
		{
			if( nDays < COMMON_YEAR_DAYS )
			{	break;
			}
			nDays -= COMMON_YEAR_DAYS;
		}

	}

	for( nMonths = 1; ; nMonths++ )												//�⵵�� �������� �������� ���� ���Ѵ�.(���� 1������ �̹Ƿ� 1���� �����Ѵ�.0
	{		
		if( IsBissextileYear((DWORD)nYears) && nMonths == BISSEXTILE_MONTH )
		{	
			if( nDays <= BissextileYearsArray[nMonths-1] )
			{	break;
			}
			nDays -= BissextileYearsArray[nMonths-1];
		}
		else
		{
			if( nDays <= CommonYearsArray[nMonths-1] )
			{	break;
			}
			nDays -= CommonYearsArray[nMonths-1];
		}

		if( !nDays )		
		{	break;
		}
		
	}
	
	if( nModDays )
	{	nDays+=1;
	}

	lpOutTime->wYear		= (WORD)nYears;
    lpOutTime->wMonth		= (WORD)nMonths;
    //lpOutTime->wDayOfWeek;
    lpOutTime->wDay			= (WORD)nDays;
    lpOutTime->wHour		= (WORD)nTimes;
    lpOutTime->wMinute		= (WORD)nMinutes;
    lpOutTime->wSecond		= (WORD)nSeconds;
    lpOutTime->wMilliseconds= (WORD)nMiliSeconds;


	return ERRORCODE_SUCCEED;
}



//static 
SYSTEMTIMEEX_ERRORCODE	SystemtimeEx::AfterThisDays( IN LPSYSTEMTIME lpThisDay , OUT LPSYSTEMTIME lpAfterDay, IN INT64 nAfterMiliSeconds )
{
	if( !lpThisDay || !lpAfterDay )
	{	return ERRORCODE_PARAM_ERROR;
	}

	SYSTEMTIMEEX_ERRORCODE res;
	INT64 nMiliSeconds;

	res = SystemtimeEx::SystemtimeToMilliseconds( lpThisDay, &nMiliSeconds );		//�Է°��� �и� �ʷ� �ٲٰ�
	if( res != ERRORCODE_SUCCEED )
	{	return res;
	}
	nMiliSeconds += nAfterMiliSeconds;												//�и� �� + �и� ��

	return SystemtimeEx::MillisecondsToSystemtime( nMiliSeconds, lpAfterDay );		//�װ��� �ٽ� SYSTEMTIME�� �ٲ۴�.
}

//static 
SYSTEMTIMEEX_ERRORCODE	SystemtimeEx::BeforeThisDays( IN LPSYSTEMTIME lpThisDay, OUT LPSYSTEMTIME lpBeforeDay, IN INT64 nBeforeMiliSeconds )
{
	if( !lpThisDay || !lpBeforeDay )
	{	return ERRORCODE_PARAM_ERROR;
	}

	SYSTEMTIMEEX_ERRORCODE res;
	INT64 nMiliSeconds;

	res = SystemtimeEx::SystemtimeToMilliseconds( lpThisDay, &nMiliSeconds );		
	if( res != ERRORCODE_SUCCEED )
	{	return res;
	}

	if( nMiliSeconds < nBeforeMiliSeconds )
	{	return ERRORCODE_NOT_SUPPORT_CALC;
	}

	nMiliSeconds -= nBeforeMiliSeconds;												//�и� �� - �и� ��

	return SystemtimeEx::MillisecondsToSystemtime( nMiliSeconds, lpBeforeDay );
}

//static 
INT64 SystemtimeEx::DifferDays( IN LPSYSTEMTIME lpFromDay, OUT LPSYSTEMTIME lpToDay )
{
	if( !lpFromDay || !lpToDay || lpFromDay->wYear < BASIC_YEAR )
	{	return ERRORCODE_PARAM_ERROR;
	}

	SYSTEMTIMEEX_ERRORCODE res;
	INT64 nFromMiliSeconds;
	INT64 nToMiliSeconds;

	res = SystemtimeEx::SystemtimeToMilliseconds( lpFromDay, &nFromMiliSeconds );		//�и� ��
	if( res != ERRORCODE_SUCCEED )
	{	return res;
	}

	res = SystemtimeEx::SystemtimeToMilliseconds( lpToDay, &nToMiliSeconds );			//�и� ��
	if( res != ERRORCODE_SUCCEED )
	{	return res;
	}

	

	if(nFromMiliSeconds < nToMiliSeconds)
	{
		nFromMiliSeconds = 0xFFFFFFFF + (nFromMiliSeconds - nToMiliSeconds );
		ErrorCodeNumber += 100000;
		return nFromMiliSeconds;
	}

	nFromMiliSeconds -= nToMiliSeconds;											//�и� �ʸ� ���� �Ѵ�.

	return nFromMiliSeconds;

}



const __int64 IN_DAY  = (__int64)10000000*60*60*24;
const __int64 IN_HOUR = (__int64)10000000*60*60;
const __int64 IN_MIN  = (__int64)10000000*60;
const __int64 IN_SEC  = (__int64)10000000; 

VOID AddSystemTime( SYSTEMTIME* pDescTime,SYSTEMTIME* pCurTime, DWORD nDay, DWORD nHour, DWORD nMinute, DWORD nSecond )
{
	FILETIME ftm;
	SystemTimeToFileTime(pCurTime, &ftm); 
	LARGE_INTEGER largeInt; 
	memcpy(&largeInt, &ftm, sizeof(FILETIME));  

	largeInt.QuadPart += IN_DAY		* nDay;
	largeInt.QuadPart += IN_HOUR	* nHour;
	largeInt.QuadPart += IN_MIN		* nMinute;
	largeInt.QuadPart += IN_SEC		* nSecond;

	memcpy(&ftm, &largeInt, sizeof(FILETIME));   
	FileTimeToSystemTime(&ftm, pDescTime); 
}


long DiffSecond( SYSTEMTIME* pEndTime, SYSTEMTIME* pCurTime )
{
    long nRtn;
    FILETIME fTm1, fTm2;
    ULONGLONG *ullVal1, *ullVal2;
    ULONGLONG ullDif;

    SystemTimeToFileTime(pEndTime, &fTm1);
    SystemTimeToFileTime(pCurTime, &fTm2);

    ullVal1 = (ULONGLONG *)&fTm1;
    ullVal2 = (ULONGLONG *)&fTm2;

    if(*ullVal1 > *ullVal2)
    {
        ullDif = *ullVal1 - *ullVal2;
    }
    else
    {
       return 0;
    }

    nRtn = long(ullDif / 10000000 /*/ 60 / 60*/);
    return nRtn;
}






















