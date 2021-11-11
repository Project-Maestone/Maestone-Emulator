#pragma once

#pragma warning(push)

#ifdef _M_X64
////////////////////////////////////////
// 64bit processor
#ifdef _WIN64
#import "C:\Program Files\Common Files\System\ADO\msado15.dll" no_namespace rename("EOF", "EndOfFile")
#pragma message ( "64Bit OS. 64Bit Target." )
#else
#import "C:\Program Files (x86)\Common Files\System\ADO\msado15.dll" no_namespace rename("EOF", "EndOfFile")
#pragma message ( "64Bit OS. 32Bit Target." )
#endif // _WIN64
////////////////////////////////////////
#else
////////////////////////////////////////
// 32bit processor
#import "C:\Program Files\Common Files\System\ADO\msado15.dll" no_namespace rename("EOF", "EndOfFile")
#pragma message ( "32Bit OS. 32Bit Target." )
////////////////////////////////////////
#endif // _M_X64

#pragma warning(pop) 

class CADORecordSet;

#define LOG_PROCEDURE_CALL

/**
	@remarks
*/

class CVariant
{

protected :	
	_bstr_t		 m_bstr;
	_variant_t	 m_var;	 
public:	
	__declspec(property(get=GetAsBool, put=PutAsBool))		   bool AsBool;
	__declspec(property(get=GetAsChar, put=PutAsChar))		   char AsChar;
	__declspec(property(get=GetAsByte, put=PutAsByte))		   BYTE AsByte;
	__declspec(property(get=GetAsShort, put=PutAsShort))	   short AsShort;
	__declspec(property(get=GetAsInteger, put=PutAsInteger))   long AsInteger;
	__declspec(property(get=GetAsInt64, put=PutAsInt64))	   __int64 AsInt64;
	__declspec(property(get=GetAsFloat, put=PutAsFloat))	   float AsFloat;
	__declspec(property(get=GetAsDouble, put=PutAsDouble))	   double AsDouble;
	__declspec(property(get=GetAsDateTime, put=PutAsDateTime)) double AsDateTime;
	__declspec(property(get=GetAsText, put=PutAsText))		   char* AsText;
	__declspec(property(get=GetAsWText, put=PutAsWText))	   wchar_t* AsWText;
	__declspec(property(get=GetAsTime, put=PutAsTime))         time_t AsTime;

	bool			IsNull();
	virtual bool	GetAsBool();
	virtual char    GetAsChar();
	virtual BYTE	GetAsByte();
	virtual short 	GetAsShort();
	virtual long	GetAsInteger();
	virtual __int64	GetAsInt64();
	virtual float	GetAsFloat();
	virtual double	GetAsDouble();
	virtual double	GetAsDateTime();
	virtual int		GetAsSystemTime(LPSYSTEMTIME);
	virtual char   *GetAsText();
	virtual wchar_t*GetAsWText();
	virtual size_t	GetAsBinary(BYTE*,size_t);	
	virtual time_t  GetAsTime();

	virtual void	PutAsBool(bool){};
	virtual void    PutAsChar(char){};
	virtual void    PutAsByte(BYTE){};
	virtual void 	PutAsShort(short){};
	virtual void	PutAsInteger(long){};
	virtual void	PutAsInt64(__int64){};
	virtual void	PutAsFloat(float){};
	virtual void	PutAsDouble(double){};
	virtual void	PutAsDateTime(double){};
	virtual void	PutAsText (char*){};
	virtual void	PutAsWText (wchar_t *pData){};
	virtual void	PutAsSystemTime(LPSYSTEMTIME){};	
	virtual void	PutAsBinary(BYTE*,size_t){};
	virtual void	PutAsTime(time_t nTime){};
	virtual void	PutNull(){};
};

class CADODataBase;

class CADOParam : public CVariant
{
private:
	_variant_t	m_vcnv;
	friend	 CADODataBase;
	_ParameterPtr m_Param;
	void	 SetParam(_ParameterPtr Param);

#ifdef LOG_PROCEDURE_CALL
protected:
	CADODataBase *m_pParent;
	void	SetParent(CADODataBase *pParent)	{	m_pParent = pParent;	}
#endif

public:
	CADOParam() : CVariant()
	{
#ifdef LOG_PROCEDURE_CALL
		m_pParent = 0;
#endif
	}
	void	PutAsBool(bool);
	void    PutAsChar(char);
	void    PutAsByte(BYTE);
	void 	PutAsShort(short);
	void	PutAsInteger(long);
	void	PutAsInt64(__int64);
	void	PutAsFloat(float);
	void	PutAsDouble(double);
	void	PutAsDateTime(double);
	void	PutAsText (char *pData);
	void	PutAsWText (wchar_t *pData);
	void	PutAsSystemTime(LPSYSTEMTIME lpSystemTime);
	void	PutAsBinary(BYTE *pData ,size_t len);	
	void	PutAsTime(time_t nTime);
	void	PutNull();
};

class CADOField : public CVariant
{
private:
	friend	 CADODataBase;
	FieldPtr m_Field;
	void	 SetField(FieldPtr Field);
};

#define MAX_DBFILE_LOGGER		16

class AvLogger;

class CADODataBase
{
private:
	LONG			m_RowEffect;	
	CADOField		m_ADOField;
	CADOParam		m_ADOParam;
	short			m_param_nowpos;
	char			m_ConnectionString[256];

	_ConnectionPtr	m_Connection;
	_CommandPtr		m_Command;
	_RecordsetPtr	m_pRecord;

	BOOL			m_nDBOK;
	char			m_szErrorMessage[1024];

public:
#ifdef LOG_PROCEDURE_CALL
	SYSTEMTIME		m_ExecuteTime;
	unsigned		m_nLoggerIdx;
	unsigned		m_nLogStrLen;
	wchar_t			m_szCommentStr[256];
	wchar_t			m_szLogStr[4096];
	char			m_dbName[256];

	void			WriteLog(char *lpExecStr, BOOL bResult, DWORD difftick, wchar_t *lpResultStr, wchar_t *lpszComment);

public:
	static AvLogger	*ms_pLogger[MAX_DBFILE_LOGGER];

	static int		_StartLog(LPCSTR filename);
	static void		_WriteLog(unsigned nIdx, const wchar_t* szTail, const wchar_t *fmt, ...);
#endif

public:
	CADODataBase();
	virtual ~CADODataBase();	
	
	HRESULT Connect();
    HRESULT Connect(const char *ConnectStr);
	HRESULT Connect( const char *ip, const char *db, const char *id, const char *passwd);
	HRESULT Disconnect();
	BOOL	Connected() { return m_Connection->State != adStateClosed; }	
	
	HRESULT Commit();
	HRESULT RollBack();
	
	BOOL EndOf(LONG);
	BOOL BeginOf();
	
	BOOL Open(const char* OpenCmd = NULL);
	BOOL Opened(){ return m_pRecord->State == adStateOpen; }
	char* CommandText(const char *Str = NULL);

	BOOL Execute(const char* CmdText = NULL, wchar_t *lpszComment = NULL, BOOL bLogSuccResult2File = TRUE);
	BOOL StoreProc(const char* CmdText = NULL, wchar_t *lpszComment = NULL, BOOL bLogSuccResult2File = TRUE);	
	
	LONG AppendInputParam (char *Cmd, enum DataTypeEnum Type = adVariant,long Size = sizeof(_variant_t));
	LONG AppendOutputParam(char *Cmd, enum DataTypeEnum Type = adVariant,long Size = sizeof(_variant_t));
	void  ClearParam();
	DWORD ParamCount();
	void Close();
	HRESULT Last();
	HRESULT First();
	HRESULT Next(LONG);
	HRESULT Perv();
	HRESULT Move(int mPos);
	HRESULT NextRecordSet();
	DWORD   RecordCount();
	
	_RecordsetPtr GetRecoRPDeftr();
	CADOField &GetFieldByName(const char* FieldName);
//	CADOField &GetFieldByIdx(long idx,long dbg=0);

//	CADOField &GetFieldByIdx(long idx,long dbg=0);

	CADOField &GetFieldByIdx(long idx, const char* szFileName, int nLine );
	CADOField &GetFieldByIdxEx(long idx, const char* szFileName, int nLine, __int64 nItemSerial );

	CADOParam &GetParamByName(const char* FieldName);
	CADOParam &GetParamByIdx (long idx);

	char *GetErrorMessage();
	DWORD GetErrorNumber();
	virtual void Error(const char *MethodStat, const char *eStr);

	void AddDBParam(LPCSTR paramName, __int64 paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, int paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, long paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, short paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, char paramVal, bool bIsNull = false);

	void AddDBParam(LPCSTR paramName, unsigned __int64  paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, unsigned int paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, unsigned long paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, unsigned short paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, unsigned char paramVal, bool bIsNull = false);

	void AddDBParam(LPCSTR paramName, float paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, double paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, bool paramVal, bool bIsNull = false);
	void AddDBParam(LPCSTR paramName, char * paramVal, unsigned maxlen);		// paramVal이 NULL이면 null로 들어감
	void AddDBParam(LPCSTR paramName, wchar_t * paramVal, unsigned maxlen);		// paramVal이 NULL이면 null로 들어감
	void AddDBParam(LPCSTR paramName, SYSTEMTIME * paramVal);					// paramVal이 NULL이면 null로 들어감
	void AddDBParamTime(LPCSTR paramName, time_t timeVal, bool bIsNull = false);

public:
	BOOL	GetDBOK()	{ return m_nDBOK;}
	
};


#define	ADO_GET_FIELDBYINDEX( DB, X )	DB->GetFieldByIdx( X, __FILE__, __LINE__ )
#define	ADO_GET_FIELDBYINDEX_DEBUG( DB, X, Z )	DB->GetFieldByIdxEx( X, __FILE__, __LINE__, Z ) //임시 디버깅용
#define	GET_DB_FIELD( DB, X, Y )		DB->GetFieldByIdx( X, __FILE__, __LINE__ ).Y
