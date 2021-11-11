// CADODBConnection.cpp: implementation of the ADODDBConnection class.
//
//////////////////////////////////////////////////////////////////////
#include "stdafx.h"
#include <stdio.h>
#include <comutil.h>
#include "ADO_DB.h"
#include "JlDbgOut.h"
#include <time.h>
#include "Exception.h"

using namespace _com_util;

#ifdef LOG_PROCEDURE_CALL
#include "../AvNet/AvLogger.h"

AvLogger *CADODataBase::ms_pLogger[MAX_DBFILE_LOGGER];
#endif

//===================================================================================================
//	class CADODBConnection
//===================================================================================================


HRESULT CADODataBase::Connect()
{
#ifdef LOG_PROCEDURE_CALL
	if (m_nLoggerIdx == -1)
		m_nLoggerIdx = _StartLog(m_dbName);
	DWORD stime = GetTickCount();
#endif

	try{
		m_Connection->ConnectionTimeout = 3;

		HRESULT hr = m_Connection->Open(m_ConnectionString,"","",adConnectUnspecified);	
#ifdef LOG_PROCEDURE_CALL
		GetSystemTime(&m_ExecuteTime);
		_WriteLog(m_nLoggerIdx, 0, L"OK\t%04d%02d%02d.%02d%02d%02d.%d\t%d\tCONNECT\t%S'\n", 
				m_ExecuteTime.wYear, m_ExecuteTime.wMonth, m_ExecuteTime.wDay, m_ExecuteTime.wHour, m_ExecuteTime.wMinute, m_ExecuteTime.wSecond, m_ExecuteTime.wMilliseconds, 
				GetTickCount()-stime,
				m_ConnectionString);
#endif
		return hr;
	}catch(_com_error &e){
		char buffer[1024];
		sprintf(buffer,"::Connect('%s')",m_ConnectionString);
		Error(buffer,e.Description());
#ifdef LOG_PROCEDURE_CALL
		GetSystemTime(&m_ExecuteTime);
		_WriteLog(m_nLoggerIdx, 0, L"FAIL\t%04d%02d%02d.%02d%02d%02d.%d\t%d\tCONNECT\t'%S'\terror = %s ----- \n", 
			m_ExecuteTime.wYear, m_ExecuteTime.wMonth, m_ExecuteTime.wDay, m_ExecuteTime.wHour, m_ExecuteTime.wMinute, m_ExecuteTime.wSecond, m_ExecuteTime.wMilliseconds, 
			GetTickCount()-stime,
			m_ConnectionString, (wchar_t *)e.Description());
#endif
	}
	return -1;
}

HRESULT CADODataBase::Connect( const char *ip, const char *db, const char *id, const char *passwd)
{
	sprintf(m_ConnectionString,"Provider=sqloledb;Data Source=%s;Initial Catalog=%s;uid=%s;pwd=%s;", ip, db, id, passwd);
#ifdef LOG_PROCEDURE_CALL
	strcpy(m_dbName, db);
#endif
	return  Connect();
}

HRESULT CADODataBase::Connect(const char *ConnectStr)
{
	sprintf(m_ConnectionString,ConnectStr);
	return  Connect();
}

HRESULT CADODataBase::Disconnect()
{
	try{
		return Connected()? m_Connection->Close():-1;
	}catch(...){
		return E_FAIL;
	}	
}

HRESULT CADODataBase::Commit()
{
	return m_Connection->CommitTrans();
}

HRESULT CADODataBase::RollBack()
{
	return m_Connection->RollbackTrans();
}


//===================================================================================================
//	class 
//===================================================================================================

CADODataBase::CADODataBase()
{
	m_nDBOK = 0;
	m_RowEffect   = 0;
	m_param_nowpos = 0;
	::CoInitialize(NULL);	
		
	m_Connection.CreateInstance("ADODB.Connection");	
	if( m_Connection == NULL ) 
	{	DBGOUT(L"[CADODataBase][ERROR][ADODB.Connection]\n");		
		return;
	}
	
	m_pRecord.CreateInstance("ADODB.RecordSet");		
	if( m_pRecord == NULL)	
	{	DBGOUT(L"[CADODataBase][ERROR][ADODB.RecordSet]\n");
		return;
	}	

	m_Command.CreateInstance("ADODB.Command");
	if( m_Command == NULL )	
	{	DBGOUT(L"[CADODataBase][ERROR][ADODB.Command]\n");
		return;
	}

	m_nDBOK = TRUE;

#ifdef LOG_PROCEDURE_CALL
	m_nLogStrLen = 0;
	m_szLogStr[0] = 0;
	m_nLoggerIdx = -1;
	m_dbName[0] = 0;

	m_ADOParam.SetParent(this);
#endif
}

CADODataBase::~CADODataBase()
{	
	Close();		
	::CoUninitialize();
}

char* CADODataBase::CommandText(const char *Str)
{
	if(Str != NULL)
	{
		m_Command->CommandText = Str;
	}

	return m_Command->CommandText;
}

LONG CADODataBase::AppendInputParam(char *Cmd, enum DataTypeEnum Type,long Size)
{
	if(!Cmd) return false;
	_ParameterPtr pParam;	
	if (Type == adEmpty)
	{
		pParam = m_Command->CreateParameter(Cmd, Type, adParamInput, Size, adFldUnspecified);
		m_Command->Parameters->raw_Append(pParam);
	}
	else
	{
		pParam = m_Command->CreateParameter(Cmd, Type, adParamInput, Size);
		m_Command->Parameters->Append(pParam);
	}
	return m_Command->Parameters->Count;
}


LONG CADODataBase::AppendOutputParam(char *Cmd, enum DataTypeEnum Type, long Size)
{
	if(!Cmd) return false;
	_ParameterPtr pParam;
	pParam = m_Command->CreateParameter(Cmd, Type,adParamOutput,Size);
	m_Command->Parameters->Append(pParam);
	return m_Command->Parameters->Count;
}

void CADODataBase::ClearParam()
{
	try {
#ifdef LOG_PROCEDURE_CALL
		m_nLogStrLen = 0;
		m_szLogStr[0] = 0;
#endif
		while( m_Command->Parameters->Count > 0 ) m_Command->Parameters->Delete(0L);
	} catch(_com_error &e) {
		char buffer[2048];
		sprintf(buffer,"::ClearParam()");
		Error(buffer,e.Description());
		
		try{
			Disconnect();
		}catch(...){
		}
		while( Connect() != 0 )
		{	Sleep(1000);
			DBGOUT(L",");
		}
		return;
	}catch( ... ){		
	}
}

DWORD CADODataBase::ParamCount()
{
	return m_Command->Parameters->Count;
}

BOOL CADODataBase::Open(const char* OpenCmd)
{
	m_pRecord.CreateInstance("ADODB.RecordSet");
	Close();		
	try{
		
		m_pRecord->Open( OpenCmd, _variant_t((IDispatch *)m_Connection, true),
						adOpenStatic, adLockOptimistic, adCmdText  );
		return true;
	}catch(_com_error &e){
		char buffer[2048];
		sprintf(buffer,"::Open('%s')",OpenCmd);
		Error(buffer,e.Description());
		
		try{
			Disconnect();
		}catch(...){
		}
		while( Connect() != 0 )
		{	Sleep(1000);
			DBGOUT(L",");
		}
		return FALSE;
	}catch( ... ){		
	}
	return true;
}

BOOL CADODataBase::Execute(const char* CmdText, wchar_t *lpszComment, BOOL bLogSuccResult2File)
{
	Close();

#ifdef LOG_PROCEDURE_CALL
	DWORD stime = GetTickCount();
	::GetLocalTime(&m_ExecuteTime);
#endif

	try{
		_variant_t vrow;
		m_Command->ActiveConnection = m_Connection;
		CommandText(CmdText);
		m_pRecord = m_Command->Execute(&vrow,NULL,adCmdText);
		m_RowEffect = vrow;

#ifdef LOG_PROCEDURE_CALL
		WriteLog((char *)CmdText, TRUE, GetTickCount() - stime, L"", lpszComment);
#endif

		return true;
	}catch(_com_error &e){
		char buffer[1024];
		sprintf(buffer,"::Execute('%s')",CmdText);
		Error(buffer, e.Description());

#ifdef LOG_PROCEDURE_CALL
		if (bLogSuccResult2File)
			WriteLog((char *)CmdText, FALSE, GetTickCount() - stime, e.Description(), lpszComment);
#endif

		try{
			Disconnect();
		}catch(...){
		}
		while( Connect() != 0 ) 
		{	Sleep(1000); DBGOUT(L"e");			
		}
		return false;
	}catch( ... ){		
	}
	return true;
}

BOOL CADODataBase::StoreProc(const char* CmdText, wchar_t *lpszComment, BOOL bLogSuccResult2File)
{
	Close();

#ifdef LOG_PROCEDURE_CALL
	DWORD stime = GetTickCount();
	::GetLocalTime(&m_ExecuteTime);
#endif

	try	{
		_variant_t vrow;
		m_Command->ActiveConnection = m_Connection;
		CommandText(CmdText);
		m_pRecord = m_Command->Execute(&vrow,NULL,adCmdStoredProc);		
		m_RowEffect = vrow;

#ifdef LOG_PROCEDURE_CALL
		if (bLogSuccResult2File)
			WriteLog((char *)CmdText, TRUE, GetTickCount() - stime, L"", lpszComment);
#endif

		return true;
	}
	catch(_com_error &e) {
		char buffer[8096];
		sprintf(buffer,"::StoreProc('%s')",CmdText);
		Error(buffer,e.Description());

#ifdef LOG_PROCEDURE_CALL
		WriteLog((char *)CmdText, FALSE, GetTickCount() - stime, e.Description(), lpszComment);
#endif

		char sqlstate[6] = { 0, };
		if( m_Connection->Errors->Count > 0)
		{
			ErrorPtr pErr = m_Connection->Errors->GetItem(0);
			strncpy(sqlstate, pErr->SQLState, 5);

			// ADO는 Severity와 State 에러 정보를 얻을 수 없음
			DBGOUT(L"[ADO ERROR] ::StoreProc('%s')[SQLSTATE: %s, NativeError: 0x%08X]\n", M2W_CHANGE_UNICODE(CmdText), M2W_CHANGE_UNICODE(sqlstate), pErr->GetNativeError());
		}

		// 재접 해줌
		if (!strcmp(sqlstate, "08006") || !strcmp(sqlstate, "08003") || 
			!strcmp(sqlstate, "08S01"))	// 10054 "Communication link failure" 나 "An existing connection was forcibly closed by the remote host"의 경우 발생
		{
			try {
				Disconnect();
			} catch(...) 
			{
			}

			while( Connect() != 0 ) 
			{	
				Sleep(100); DBGOUT(L"s");
			}
		}

		return false;
	}
	catch( ... ) {	

	}

	return true;
}



BOOL CADODataBase::EndOf( LONG nDbg )
{
	if(m_pRecord == NULL) return true;
	try{
		return m_pRecord->EndOfFile;
	}catch(_com_error &e){		
		char tBuf[32]={0,};
		sprintf( tBuf,"::EndOf(%d)", nDbg );
		Error( tBuf,e.Description());
		return true;
	}
}

BOOL CADODataBase::BeginOf()
{
	short result = 0;
	if(m_pRecord == NULL) return true;
	try{
		m_pRecord->get_BOF(&result);
		return result;
	}catch(_com_error &e){
		Error("::BeginOf()",e.Description());
		return true;
	}
}

void CADODataBase::Close()
{	
	try{
		if( m_pRecord != NULL && Opened() ) m_pRecord->Close();		
	}catch(_com_error &e){
		e.Description();
		//Error("::Close()",e.Description());
	}
}

HRESULT CADODataBase::Last()
{
	if(m_pRecord == NULL) return 0;
	try{
		return m_pRecord->MoveLast();
	}catch(_com_error &e){
		Error("::Last()",e.Description());
		return 0;
	}
}

HRESULT CADODataBase::First()
{
	if(m_pRecord == NULL) return 0;
	try{
		return m_pRecord->MoveFirst();
	}catch(_com_error &e){
		Error("::First()",e.Description());
		return 0;
	}
}

HRESULT CADODataBase::Next(LONG nDbg)
{
	try{
		return m_pRecord->MoveNext();
	}catch(_com_error &e){
		char tBuf[32]={0,};
		sprintf( tBuf,"::Next(%d)", nDbg );		
		Error( tBuf,e.Description());
		return 0;
	}
}

HRESULT CADODataBase::Perv()
{
	if(m_pRecord == NULL) return 0;
	try{
		return m_pRecord->MovePrevious();
	}catch(_com_error &e){
		Error("::Perv()",e.Description());
		return 0;
	}
}

HRESULT CADODataBase::Move(int mPos)
{
	if(m_pRecord == NULL) return 0;
	try{
		return m_pRecord->Move(mPos);
	}catch(_com_error &e){
		Error("::Move()",e.Description());
		return 0;
	}
}

HRESULT CADODataBase::NextRecordSet()
{
	if(m_pRecord == NULL) return E_FAIL;
	long    lngRec = 0;
	try
	{
		m_pRecord = m_pRecord->NextRecordset((VARIANT *)lngRec);

		if ( m_pRecord ) return ERROR_SUCCESS;
		return E_FAIL;

	}
	catch(_com_error &e)
	{
		Error("::NextRecordSet()",e.Description());
		return E_FAIL;
	}
}

DWORD   CADODataBase::RecordCount()
{
	if(m_pRecord == NULL) return 0;
	try{
		return m_pRecord->RecordCount;
	}catch(_com_error &e){
		Error("::RecordCount()",e.Description());
		return 0;
	}
}

CADOField &CADODataBase::GetFieldByName(const char* FieldName)
{		
	try{
		m_ADOField.SetField( NULL );
		if(m_pRecord) m_ADOField.SetField(m_pRecord->Fields->GetItem( FieldName ));
		return m_ADOField;
	}catch(_com_error &e){
		char buffer[1024];
		sprintf(buffer,"::GetFieldByName(%s)",FieldName);
		Error(buffer,e.Description());
		return m_ADOField;	
	}
}

/*
CADOField & CADODataBase::GetFieldByIdx(long idx,long dbg)
{
	try{
		m_ADOField.SetField( NULL );
		//if(m_pRecord) m_ADOField.SetField(m_pRecord->Fields->GetItem(idx));
		if(m_pRecord) m_ADOField.SetField(m_pRecord->Fields->Item[idx]);
		return m_ADOField;
	}catch(_com_error &e){
		char buffer[1024];
		sprintf(buffer,"::GetFieldByIdx(%d)(dbg=%d)",idx, dbg);
		Error(buffer,e.Description());
		return m_ADOField;	
	}
}
*/

CADOField & CADODataBase::GetFieldByIdx(long idx, const char* szFileName, int nLine )
{
	try{
		m_ADOField.SetField( NULL );
		//if(m_pRecord) m_ADOField.SetField(m_pRecord->Fields->GetItem(idx));
		if(m_pRecord) m_ADOField.SetField(m_pRecord->Fields->Item[idx]);
		return m_ADOField;
	}catch(_com_error &e){
		char buffer[1024];
		sprintf(buffer,"::GetFieldByIdx(%d)(call by %s(%d)",idx, szFileName, nLine );
		Error(buffer,e.Description());
		return m_ADOField;	
	}
}


CADOField & CADODataBase::GetFieldByIdxEx(long idx, const char* szFileName, int nLine, __int64 nItemBigSerial )
{
	try{
		m_ADOField.SetField( NULL );
		//if(m_pRecord) m_ADOField.SetField(m_pRecord->Fields->GetItem(idx));
		if(m_pRecord) m_ADOField.SetField(m_pRecord->Fields->Item[idx]);
		return m_ADOField;
	}catch(_com_error &e){
		char buffer[1024];
		sprintf(buffer,"::GetFieldByIdxEx(%d)(call by %s(%d), ItemSerial =%I64d ",idx, szFileName, nLine, nItemBigSerial );
		Error(buffer,e.Description());
		return m_ADOField;	
	}
}


CADOParam &CADODataBase::GetParamByName(const char* ParamName)
{		
	try{
		m_ADOParam.SetParam( NULL );
		if(m_Command) m_ADOParam.SetParam(m_Command->Parameters->GetItem( ParamName ));
		return m_ADOParam;
	}catch(_com_error &e){
		char buffer[1024];
		sprintf(buffer,"::GetParamByName(%s)",ParamName);
		Error(buffer,e.Description());
		return m_ADOParam;	
	}
}

CADOParam &CADODataBase::GetParamByIdx(long idx)
{
	try{		
		m_ADOParam.SetParam( NULL );
		if(m_Command) m_ADOParam.SetParam(m_Command->Parameters->GetItem( idx ));
		return m_ADOParam;
	}catch(_com_error &e){
		char buffer[1024];
		sprintf(buffer,"::GetParamByIdx(%d)",idx);
		Error(buffer,e.Description());
		return m_ADOParam;	
	}
}

char *CADODataBase::GetErrorMessage()
{	
   m_szErrorMessage[0] = 0;
   if( m_Connection->Errors->Count > 0)
   {
	   long nCount = m_Connection->Errors->Count;
	   for( long i = 0; i < nCount; i++ ){
		   ErrorPtr pErr = m_Connection->Errors->GetItem(i);
		   int len = (int)strlen(m_szErrorMessage);
		   _snprintf(&m_szErrorMessage[len], sizeof(m_szErrorMessage)-len-1, "%x:%s\n", pErr->Number,(LPCSTR)pErr->Description);
	   }
   }
   return m_szErrorMessage;
}

DWORD CADODataBase::GetErrorNumber()
{	
	if( m_Connection->Errors->Count > 0)
	{
		ErrorPtr pErr = m_Connection->Errors->GetItem(0);
		return pErr->GetNumber();
	}
	return 0;
}

void CADODataBase::Error(const char *sql ,const char *err)
{	//printf("[ADO ERROR][%s][%s]\n", sql, err);
	DBGOUT(L"[ADO ERROR][%s][%s]\n", M2W_CHANGE_UNICODE(sql), M2W_CHANGE_UNICODE(err) );
}


void CADODataBase::AddDBParam(LPCSTR paramName, __int64 paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adBigInt, sizeof(__int64));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsInt64	= paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, int paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adInteger, sizeof(int));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsInteger	= paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, long paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adInteger, sizeof(long));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsInteger	= paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, short paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adSmallInt, sizeof(short));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsShort = paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, char paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adChar, sizeof(char));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsChar = paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, unsigned __int64 paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adBigInt, sizeof(unsigned __int64));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsInt64	= paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, unsigned int paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adInteger, sizeof(unsigned int));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsInteger	= paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, unsigned long paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adInteger, sizeof(unsigned long));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsInteger	= paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, unsigned short paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adSmallInt, sizeof(unsigned short));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsShort = paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, unsigned char paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adUnsignedTinyInt, sizeof(unsigned char));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsByte = paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, float paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adSingle, sizeof(float));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsFloat = paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, double paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adDouble, sizeof(double));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsDouble = paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, bool paramVal, bool bIsNull)
{
	AppendInputParam((char*)paramName, adBoolean, sizeof(bool));
	if (bIsNull)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsBool = paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, char * paramVal, unsigned maxlen)
{
	AppendInputParam((char*)paramName, adVarChar, maxlen);
	if (paramVal == NULL)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsText = paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, wchar_t * paramVal, unsigned maxlen)
{
	AppendInputParam((char*)paramName, adVarWChar, maxlen);
	if (paramVal == NULL)
		GetParamByName(paramName).PutNull();
	else
		GetParamByName(paramName).AsWText = paramVal;
}

void CADODataBase::AddDBParam(LPCSTR paramName, SYSTEMTIME * paramVal)
{
	AppendInputParam((char*)paramName, adDBTimeStamp, sizeof(double));
	if (paramVal)
	{
		double dtVal;
		SystemTimeToVariantTime(paramVal, &dtVal);
		GetParamByName(paramName).PutAsDateTime(dtVal);
	}
	else
	{
		GetParamByName(paramName).PutNull();
	}
}

void CADODataBase::AddDBParamTime(LPCSTR paramName, time_t timeVal, bool bIsNull)
{
	if (bIsNull)
	{
		AddDBParam(paramName, (SYSTEMTIME *)NULL);
	}
	else
	{
		struct tm _tm;
		SYSTEMTIME st;

		ZeroMemory(&st, sizeof(st));

		localtime_s(&_tm, &timeVal);
		st.wYear = _tm.tm_year + 1900;
		st.wMonth = _tm.tm_mon + 1;
		st.wDay = _tm.tm_mday;
		st.wHour = _tm.tm_hour;
		st.wMinute = _tm.tm_min;
		st.wSecond = _tm.tm_sec;

		AddDBParam(paramName, &st);
	}
}

/*
void CADODataBase::AddDBParamNull(LPCSTR paramName, DataTypeEnum _type)
{
	AppendInputParam((char*)paramName, adEmpty, 0);
	GetParamByName(paramName).PutNull();
}
*/

//===================================================================================================
//	class CADOField
//===================================================================================================


void CADOField::SetField(FieldPtr Field)
{
	m_Field = Field;
	if( m_Field == NULL ) return;
	m_var	= m_Field->GetValue();
}

void CADOParam::SetParam(_ParameterPtr Param)
{	
	m_Param = Param;
	if( m_Param == NULL ) return;
	m_var	= m_Param->GetValue();
}

void CADOParam::PutAsBool(bool bVal)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adBoolean);
	m_Param->PutSize(sizeof(bool));
	m_Param->PutValue(_variant_t(bVal));
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = %d", (wchar_t *)m_Param->GetName(), bVal);
#endif
}
void CADOParam::PutAsChar(char cVal)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adTinyInt);
	m_Param->PutSize(sizeof(char));
	m_Param->PutValue(cVal);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = %d", (wchar_t *)m_Param->GetName(), cVal);
#endif
}
void CADOParam::PutAsByte(BYTE bVal)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adTinyInt);
	m_Param->PutSize(sizeof(bVal));
	m_Param->PutValue(bVal);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = %d", (wchar_t *)m_Param->GetName(), bVal);
#endif
}
void CADOParam::PutAsShort(short nVal )
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adSmallInt);
	m_Param->PutSize(sizeof(short));
	m_Param->PutValue(nVal);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = %d", (wchar_t *)m_Param->GetName(), nVal);
#endif
}
void CADOParam::PutAsInteger(long lVal)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adInteger);
	m_Param->PutSize(sizeof(long));
	m_Param->PutValue(lVal);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = %d", (wchar_t *)m_Param->GetName(), lVal);
#endif
}
void CADOParam::PutAsInt64(__int64 lo64)
{	
	if( m_Param == NULL ) return;		
// 	m_vcnv.vt = VT_DECIMAL;	
//     DECIMAL dec = { 0, };
//     dec.Lo64 = (LONGLONG)lo64;
//     m_vcnv = dec;
// 	m_Param->PutSize(sizeof(ULONGLONG));
// 	m_Param->PutValue(m_vcnv);
	m_Param->PutType(adBigInt);
	m_Param->PutSize(sizeof(long));
	m_Param->PutValue(lo64);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = %I64d", (wchar_t *)m_Param->GetName(), lo64);
#endif
}
void CADOParam::PutAsFloat(float ftVal)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adDouble);
	m_Param->PutSize(sizeof(float));
	m_Param->PutValue(ftVal);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = %f", (wchar_t *)m_Param->GetName(), ftVal);
#endif
}
void CADOParam::PutAsDouble(double dbVal)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adDouble);
	m_Param->PutSize(sizeof(double));
	m_Param->PutValue(dbVal);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = %f", (wchar_t *)m_Param->GetName(), dbVal);
#endif
}
void CADOParam::PutAsDateTime(double dtVal)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adDBTimeStamp);
	m_Param->PutSize(sizeof(double));
	m_Param->PutValue(dtVal);
#ifdef LOG_PROCEDURE_CALL
	SYSTEMTIME st;
	VariantTimeToSystemTime(dtVal,&st);
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = '%d-%d-%d %d:%d:%d.%d", (wchar_t *)m_Param->GetName(), st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond, st.wMilliseconds);
#endif
}
void CADOParam::PutAsSystemTime(LPSYSTEMTIME lpSystemTime)
{	
	if( m_Param == NULL ) return;
	double dtVal;
	SystemTimeToVariantTime(lpSystemTime,&dtVal);
	m_Param->PutType(adDBTimeStamp);
	m_Param->PutSize(sizeof(double));
	m_Param->PutValue(dtVal);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = '%d-%d-%d %d:%d:%d.%d", (wchar_t *)m_Param->GetName(), lpSystemTime->wYear, lpSystemTime->wMonth, lpSystemTime->wDay, lpSystemTime->wHour, lpSystemTime->wMinute, lpSystemTime->wSecond, lpSystemTime->wMilliseconds);
#endif
}
void CADOParam::PutAsBinary(BYTE *pData ,size_t len)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adVarBinary);
	m_Param->PutSize((ADO_LONGPTR)len);
	m_Param->PutValue((const char*)pData);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = ", (wchar_t *)m_Param->GetName());
	for (int i = 0; i < len; i++)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%02x", pData[i]);
#endif
}
void CADOParam::PutAsText (char *pData)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adVarChar);
	m_Param->PutSize((ADO_LONGPTR)(strlen(pData)+1));
	m_Param->PutValue(pData);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = '%S'", (wchar_t *)m_Param->GetName(), pData);
#endif
}

void CADOParam::PutAsWText (wchar_t *pData)
{
	if( m_Param == NULL ) return;
	m_Param->PutType(adVarWChar);
	m_Param->PutSize((ADO_LONGPTR)((wcslen(pData)+1)*2));
	m_Param->PutValue(pData);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = '%s'", (wchar_t *)m_Param->GetName(), pData);
#endif
}

void CADOParam::PutAsTime (time_t nValue)
{
	struct tm _tm;
	SYSTEMTIME st;

	ZeroMemory(&st, sizeof(st));

	localtime_s(&_tm, &nValue);
	st.wYear = _tm.tm_year + 1900;
	st.wMonth = _tm.tm_mon + 1;
	st.wDay = _tm.tm_mday;
	st.wHour = _tm.tm_hour;
	st.wMinute = _tm.tm_min;
	st.wSecond = _tm.tm_sec;

	PutAsSystemTime(&st);
#ifdef LOG_PROCEDURE_CALL
	if (m_pParent->m_nLogStrLen > 0)
		m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L", ");
	LPSYSTEMTIME lpSystemTime = &st;
	m_pParent->m_nLogStrLen += _snwprintf(&m_pParent->m_szLogStr[m_pParent->m_nLogStrLen], _countof(m_pParent->m_szLogStr)-m_pParent->m_nLogStrLen, L"%s = '%d-%d-%d %d:%d:%d", (wchar_t *)m_Param->GetName(), lpSystemTime->wYear, lpSystemTime->wMonth, lpSystemTime->wDay, lpSystemTime->wHour, lpSystemTime->wMinute, lpSystemTime->wSecond, lpSystemTime->wMilliseconds);
#endif
}

void CADOParam::PutNull()
{
	if( m_Param == NULL ) return;
	// PutValue를 사용하면 vtMissing을 넣을 때 오류 발생함
	m_Param->put_Value(vtMissing);
}


bool CVariant::GetAsBool()
{
	return IsNull() ? 0 : (m_var.boolVal != 0);
}

char  CVariant::GetAsChar()
{
	return IsNull() ? 0 : m_var.cVal;	
}

BYTE CVariant::GetAsByte()
{
	return IsNull() ? 0 : m_var.bVal;	
}

short  CVariant::GetAsShort()
{
	return IsNull() ? 0 : m_var.iVal;	
}

long  CVariant::GetAsInteger()
{
	return IsNull() ? 0 : m_var.intVal;	
}

static char* szNull = "X";
static wchar_t* wszNull = L"X";

bool CVariant::IsNull()
{
	return m_var.vt == VT_NULL;
}

char *CVariant::GetAsText()
{	
	if (IsNull() || !m_var.bstrVal) return "";
	m_bstr = m_var.bstrVal;
	char * retval = m_bstr;
	if ((int)retval >= 0 && (int)retval < 0x1000)
		return szNull;
	return retval;
}

wchar_t *CVariant::GetAsWText()
{	
	if (IsNull() || !m_var.bstrVal) return L"";
	m_bstr = m_var.bstrVal;
	wchar_t * retval = m_bstr;
	if ((int)retval >= 0 && (int)retval < 0x1000)
		return wszNull;
	return retval;
}

size_t  CVariant::GetAsBinary(BYTE *pData ,size_t len)
{
	if( m_var.vt != (VT_ARRAY|VT_UI1) ) return 0;	
	LONG nDataLength=m_var.parray->rgsabound[0].cElements;	
	void *pArrayData;
	SafeArrayAccessData(m_var.parray,&pArrayData);
	memcpy(pData, pArrayData, nDataLength);
	SafeArrayUnaccessData(m_var.parray);
	return nDataLength;
}

time_t CVariant::GetAsTime()
{
	return IsNull() ? 0 : m_var.intVal;	
}

float CVariant::GetAsFloat()
{
	return IsNull() ? 0 : m_var.fltVal;	
}
double CVariant::GetAsDouble()
{
	return IsNull() ? 0 : m_var.dblVal;
}
__int64 CVariant::GetAsInt64()
{	
	return IsNull() ? 0 : m_var.decVal.Lo64;
}

double CVariant::GetAsDateTime()
{
	return m_var.date;
}

int CVariant::GetAsSystemTime(LPSYSTEMTIME lpSystemTime )
{
	if( ( m_var.vt == VT_EMPTY ) || ( m_var.vt == VT_NULL ) )
		return FALSE;

	return VariantTimeToSystemTime(m_var.date,lpSystemTime);
}



#ifdef LOG_PROCEDURE_CALL

static int ExceptFilter(unsigned int code, struct _EXCEPTION_POINTERS *ep) 
{
	g_expManager.DumpException(ep);

	return EXCEPTION_EXECUTE_HANDLER;
}

#define LOG_PATH		"DBProcLog"

void CADODataBase::WriteLog(char *lpExecStr, BOOL bResult, DWORD difftick, wchar_t *lpResultStr, wchar_t *lpszComment)
{
	__try
	{
		char szExecStr[1024];

		// EXEC dbname.dbo.storeproc 인자
		// 형식으로 출력하여 실행되지 못한 프로시져를 실행할 수 있도록 함
		_snprintf(szExecStr, _countof(szExecStr), "EXEC %s.", m_dbName);
		if (_strnicmp(lpExecStr, "dbo.", 4))
			strcat(szExecStr, "dbo.");
		// strcat으로 size 넘는 것 체크할 수 없으니, strncpy로 변경
		int len = strlen(szExecStr);
		strncpy(&szExecStr[len], lpExecStr, _countof(szExecStr)-len);
		szExecStr[_countof(szExecStr)-1] = 0;

		if (bResult)
		{
			_WriteLog(m_nLoggerIdx, m_szLogStr, L"OK\t%04d%02d%02d.%02d%02d%02d.%d\t%d\t%s\t%S ",
				m_ExecuteTime.wYear, m_ExecuteTime.wMonth, m_ExecuteTime.wDay, m_ExecuteTime.wHour, m_ExecuteTime.wMinute, m_ExecuteTime.wSecond, m_ExecuteTime.wMilliseconds, 
				difftick, lpszComment ? lpszComment : L"", szExecStr );
		}
		else
		{
			wchar_t szTail[4096];
			_snwprintf( szTail, 4096, L"%s\t--%s\n", m_szLogStr, lpResultStr );
			_WriteLog(m_nLoggerIdx, szTail, L"FAIL\t%04d%02d%02d.%02d%02d%02d%d\t%d\t%s\t %S ", 
				m_ExecuteTime.wYear, m_ExecuteTime.wMonth, m_ExecuteTime.wDay, m_ExecuteTime.wHour, m_ExecuteTime.wMinute, m_ExecuteTime.wSecond, m_ExecuteTime.wMilliseconds, 
				difftick, lpszComment ? lpszComment : L"", szExecStr );
		}
	}
	__except( ExceptFilter( GetExceptionCode(), GetExceptionInformation() ) )
	{
		unsigned int code = GetExceptionCode();

		if ( code == EXCEPTION_STACK_OVERFLOW )
		{
			_resetstkoflw();		
		}
	}
}

int CADODataBase::_StartLog(LPCSTR filename) 
{
	char datapath[256];
	sprintf (datapath, LOG_PATH "\\%s", filename);

	CreateDirectory(LOG_PATH, NULL);

	for (int i = 0; i < MAX_DBFILE_LOGGER; i++)
	{
		// 동일한 것이 있는지
		if (ms_pLogger[i] && !strcmp(ms_pLogger[i]->GetPath(), datapath))
		{
			return i;
		}
	}

	for (int i = 0; i < MAX_DBFILE_LOGGER; i++)
	{
		// 동일한 것이 있는지
		if (!ms_pLogger[i])
		{
			AvLogger *pLogFile = new AvLogger( datapath );
			pLogFile->SetAddLinePrefix( false );
			pLogFile->SetMakeNewFileOnEveryHour(false);
			pLogFile->SetFlushOnEveryWrite(true);

			ms_pLogger[i] = pLogFile;
			return i;
		}
	}

	return -1;
}

void CADODataBase::_WriteLog(unsigned nIdx, const wchar_t* szTail, const wchar_t *fmt, ...)
{
	if ( nIdx >= MAX_DBFILE_LOGGER || !ms_pLogger[nIdx] ) 
		return;

	wchar_t strBuf[8192];

	va_list vl;
	va_start(vl, fmt);
	int len =  _vsnwprintf(strBuf, _countof(strBuf), fmt, vl);
	va_end(vl);

	strBuf[_countof(strBuf)-1] = 0;

	ms_pLogger[nIdx]->Log( strBuf );
	if( szTail )
		ms_pLogger[nIdx]->Log( L"%s \n", szTail );
}

#endif