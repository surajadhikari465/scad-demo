Option Strict Off
Option Explicit On

Imports WholeFoods.Utility

Module API

  Public Const INTERNET_OPEN_TYPE_DIRECT As Short = 1
  Public Const scUserAgent As String = "vb wininet"
  Public Const FTP_TRANSFER_TYPE_ASCII As Short = &H1S
  Public Const SW_SHOWNORMAL As Short = 1
  Public Const SW_SHOWMAXIMIZED As Short = 3

  Public Declare Function BC_UPCA Lib "bcfont32.DLL" (ByVal s As String) As String
  Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
  Public Declare Function FtpPutFile Lib "wininet.dll" Alias "FtpPutFileA" (ByVal hFtpSession As Integer, ByVal lpszLocalFile As String, ByVal lpszRemoteFile As String, ByVal dwFlags As Integer, ByVal dwContext As Integer) As Boolean
  Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
  Public Declare Function GetSystemMetrics Lib "user32" (ByVal nIndex As Integer) As Integer
  Private Declare Function GetUserName Lib "advapi32.dll" Alias "GetUserNameA" (ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
  Public Declare Function InternetCloseHandle Lib "wininet.dll" (ByVal hInet As Integer) As Short
  Public Declare Function InternetConnect Lib "wininet.dll" Alias "InternetConnectA" (ByVal hInternetSession As Integer, ByVal sServerName As String, ByVal nServerPort As Short, ByVal sUserName As String, ByVal sPassword As String, ByVal lService As Integer, ByVal lFlags As Integer, ByVal lContext As Integer) As Integer
  Public Declare Function InternetOpen Lib "wininet.dll" Alias "InternetOpenA" (ByVal sAgent As String, ByVal lAccessType As Integer, ByVal sProxyName As String, ByVal sProxyBypass As String, ByVal lFlags As Integer) As Integer
  Private Declare Function LookupAccountName Lib "advapi32.dll" Alias "LookupAccountNameA" (ByRef lpSystemName As String, ByVal lpAccountName As String, ByRef sid As Byte, ByRef cbSid As Integer, ByVal ReferencedDomainName As String, ByRef cbReferencedDomainName As Integer, ByRef peUse As Integer) As Integer
  Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer)
  Public Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer


  '	Function ReadINI(ByRef SectionName As String, ByRef ItemName As String, ByRef INIFileName As String, ByRef GetAppPath As Boolean) As String
  '		'07/31/1998 - Copyright 1998 RESAM Systems, Inc.  - All rights Reserved

  '		Dim lReturnLength As Integer
  '		Dim lSize As Integer
  '		Dim sReturnValue As String
  '		Dim sDefaultValue As String
  '        Dim sINIReturn As New VB6.FixedLengthString(255)= String.Empty

  '		On Error GoTo ErrorReadINI

  '		If GetAppPath Then INIFileName = CStr(My.Application.Info.DirectoryPath) & "\" & INIFileName

  '		sDefaultValue = ""
  '        sINIReturn.Value = Space(255)
  '		lSize = Len(sINIReturn.Value)

  '		lReturnLength = GetPrivateProfileString(SectionName, ItemName, sDefaultValue, sINIReturn.Value, lSize, INIFileName)
  '		sReturnValue = Left(sINIReturn.Value, lReturnLength)
  '		ReadINI = sReturnValue

  '		Exit Function

  'ErrorReadINI: 

  '		MsgBox(Err.Number & Chr(13) & ErrorToString(), MsgBoxStyle.Exclamation, "Read INI")
  '		Exit Function

  '    End Function

  Public Function GetADUserInfo(ByVal userName As String, ByVal LDAPKey As String) As String

    Dim _writeInfo As Boolean = False
    Dim strPath As String
    Dim strSearchFilter As String = "(&(objectClass=user)(samaccountname={0}))"

    Dim objDirEntry As System.DirectoryServices.DirectoryEntry = Nothing
    Dim objDirSearcher As System.DirectoryServices.DirectorySearcher = Nothing
    Dim objCollSearchResult As System.DirectoryServices.SearchResultCollection = Nothing
    Dim objlSearchResult As System.DirectoryServices.SearchResult = Nothing
    Dim objCollResultProperty As System.DirectoryServices.ResultPropertyCollection = Nothing
    Dim objCollResultPropertyValue As System.DirectoryServices.ResultPropertyValueCollection = Nothing



    GetADUserInfo = String.Empty

    Try

      Try
        strPath = String.Format("LDAP://{0}", ConfigurationServices.AppSettings("LDAP_Server"))
        If strPath = "LDAP://" Then Throw New Exception()
      Catch exLDAP As Exception
        Throw New Exception("LDAP_Server has not been configured")
      End Try



      ' setup the search in AD for the user's details
      objDirEntry = New System.DirectoryServices.DirectoryEntry(strPath)
      objDirSearcher = New System.DirectoryServices.DirectorySearcher(objDirEntry)
      objDirSearcher.Filter = (String.Format(strSearchFilter, userName))

      ' search AD for the user account
      objCollSearchResult = objDirSearcher.FindAll()

      Select Case objCollSearchResult.Count

        Case 1

          ' pull the AD properties that we can and load the text boxes
          objlSearchResult = objCollSearchResult.Item(0)
          objCollResultProperty = objlSearchResult.Properties

          objCollResultPropertyValue = objCollResultProperty.Item(LDAPKey)
          If objCollResultPropertyValue.Count > 0 Then
            GetADUserInfo = objCollResultPropertyValue.Item(0).ToString
          End If

        Case Else

          GetADUserInfo = String.Empty

          Exit Try

      End Select

      ' dispose of all the AD objects
      objDirEntry.Dispose()
      objDirSearcher.Dispose()
      objCollSearchResult.Dispose()
      objlSearchResult = Nothing
      objCollResultProperty = Nothing
      objCollResultPropertyValue = Nothing

      Return GetADUserInfo

    Catch ex As System.Exception


      Dim ex1 As New Exception(String.Format("GetADUserInfo() lookup failed. [ {0} ]", ex.Message), ex)
      Throw ex1


    End Try

  End Function

End Module