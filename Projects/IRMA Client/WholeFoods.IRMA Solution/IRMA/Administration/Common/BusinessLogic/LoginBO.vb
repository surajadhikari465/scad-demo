Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WFM.UserAuthentication

Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic
    ''' <summary>
    ''' Status for the LoginBO validation.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum LoginDataStatus
        Valid
        Error_Required_UserName
        Error_Required_Password
    End Enum

    Public Enum LoginVerificationStatus
        Valid
        Error_WindowsAuthenticationFailure
        Error_MissingIRMAAccount
        Error_DisabledIRMAAccount
    End Enum

    Public Class LoginBO
        Const DOMAIN As String = "WFM"
        Const MAX_RETRIES As Integer = 6

#Region "Class Level Vars and Property Definitions"
        ''' <summary>
        ''' user name for login attempt
        ''' </summary>
        ''' <remarks></remarks>
        Private _userName As String

        ''' <summary>
        ''' password for login attempt
        ''' </summary>
        ''' <remarks></remarks>
        Private _password As String

        ''' <summary>
        ''' if the LoginVerificationStatus is Error_WindowsAuthenticationFailure, the windows 
        ''' authentication error message text is stored in this property
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorMessage As String

        ''' <summary>
        ''' Value of the current user configuration
        ''' </summary>
        ''' <remarks></remarks>
        Private _userConfig As UserBO
#End Region

#Region "Business Logic Methods"
        ''' <summary>
        ''' Check to see if the maximum number of login tries has been reached.
        ''' </summary>
        ''' <param name="retryCount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function MaximumRetriesReached(ByVal retryCount As Integer) As Boolean
            Dim maxReached As Boolean = False
            If retryCount >= MAX_RETRIES Then
                maxReached = True
            End If
            Return maxReached
        End Function

        ''' <summary>
        ''' This method sets the user name, if it is not already populated, with the
        ''' login named used to log the user on to their PC.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetUserName()
            If (_userName Is Nothing) Or (_userName = "") Then
                _userName = GetLogonDomainUser()
            End If
        End Sub

        ''' <summary>
        ''' validates data elements of current instance of LoginBO object
        ''' </summary>
        ''' <returns>ArrayList of LoginStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidateData() As ArrayList
            Dim errorList As New ArrayList

            ' required fields
            If _userName.Equals("") Then
                errorList.Add(LoginDataStatus.Error_Required_UserName)
            End If
            If _password.Equals("") Then
                errorList.Add(LoginDataStatus.Error_Required_Password)
            End If

            'no errors - return Valid status
            If errorList.Count = 0 Then
                errorList.Add(LoginDataStatus.Valid)
            End If

            Return errorList
        End Function

        ''' <summary>
        ''' Authenticate a user's login - performing Windows authentication and IRMA client authentication.
        ''' </summary>
        ''' <param name="irmaLogin">If false, only Windows authentication is checked.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AuthenticateUser(ByVal irmaLogin As Boolean) As LoginVerificationStatus
            Dim loginStatus As LoginVerificationStatus

            ' Always Validate the Windows Login
            Dim sLogonErrMsg As String = WindowsAuthentication.ValidUser(_userName, _password)
            If sLogonErrMsg.Length > 0 Then
                loginStatus = LoginVerificationStatus.Error_WindowsAuthenticationFailure
                _errorMessage = sLogonErrMsg
            Else
                ' The IRMA validation does not have to be duplicated in the case of a timeout
                If irmaLogin Then
                    _userConfig = New UserBO()
                    _userConfig.UserName = _userName
                    _userConfig = UserDAO.ValidateIRMALogin(_userConfig)

                    If _userConfig Is Nothing Then
                        loginStatus = LoginVerificationStatus.Error_MissingIRMAAccount
                    ElseIf Not _userConfig.AccountEnabled Then
                        loginStatus = LoginVerificationStatus.Error_DisabledIRMAAccount
                    Else
                        loginStatus = LoginVerificationStatus.Valid
                    End If
                End If
            End If
            Return loginStatus
        End Function
#End Region

        Private Declare Function GetUserName Lib "advapi32.dll" Alias "GetUserNameA" (ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
        Private Declare Function LookupAccountName Lib "advapi32.dll" Alias "LookupAccountNameA" (ByRef lpSystemName As String, ByVal lpAccountName As String, ByRef sid As Byte, ByRef cbSid As Integer, ByVal ReferencedDomainName As String, ByRef cbReferencedDomainName As Integer, ByRef peUse As Integer) As Integer

        Private Function GetLogonDomainUser() As String
            Dim userName As String
            Dim domainName As String
            Dim lResult As Integer ' Result of various API calls.
            Dim i As Short ' Used in looping.
            Dim bUserSid(255) As Byte ' This will contain your SID.
            Dim lDomainNameLength As Integer ' Length of domain name needed.
            Dim lSIDType As Integer ' The type of SID info we are getting back.

            ' Get the SID of the user. (Refer to the MSDN for more information on SIDs
            ' and their function/purpose in the operating system.) Get the SID of this
            ' user by using the LookupAccountName API. 
            ' In order to use the SID of the current user account, call the LookupAccountName API
            ' twice. The first time is to get the required sizes of the SID
            ' and the DomainName string. The second call is to actually get
            ' the desired information.

            On Error Resume Next

            lDomainNameLength = 255
            domainName = Space(lDomainNameLength)

            userName = GetLogonUser()

            If Err.Number <> 0 Then
                userName = ""
                Err.Clear()
            End If

            lResult = LookupAccountName(vbNullString, userName, bUserSid(0), 255, domainName, lDomainNameLength, lSIDType)

            If Err.Number <> 0 Then
                domainName = ""
                Err.Clear()
            Else
                ' Call the LookupAccountName again to get the actual SID for user.
                lResult = LookupAccountName(vbNullString, userName, bUserSid(0), 255, domainName, lDomainNameLength, lSIDType)

                If Err.Number <> 0 Then
                    domainName = ""
                    Err.Clear()
                Else
                    domainName = Trim(Left(domainName, InStr(domainName, Chr(0)) - 1))

                    If Err.Number <> 0 Then
                        domainName = ""
                        Err.Clear()
                    End If
                End If
            End If

            ' Return the user name
            Return userName
        End Function

        Private Function GetLogonUser() As String
            Dim strTemp, strUserName As String
            'Create a buffer
            strTemp = New String(Chr(0), 100)
            'strip the rest of the buffer
            strTemp = Left(strTemp, InStr(strTemp, Chr(0)) - 1)

            'Create a buffer
            strUserName = New String(Chr(0), 100)
            'Get the username
            GetUserName(strUserName, 100)
            'strip the rest of the buffer
            strUserName = Left(strUserName, InStr(strUserName, Chr(0)) - 1)
            GetLogonUser = strUserName
        End Function

#Region "Property access methods"
        Public Property UserName() As String
            Get
                Return _userName
            End Get
            Set(ByVal value As String)
                _userName = value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property

        Public Property ErrorMessage() As String
            Get
                Return _errorMessage
            End Get
            Set(ByVal value As String)
                _errorMessage = value
            End Set
        End Property

        Public Property UserConfig() As UserBO
            Get
                Return _userConfig
            End Get
            Set(ByVal value As UserBO)
                _userConfig = value
            End Set
        End Property
#End Region
    End Class
End Namespace
