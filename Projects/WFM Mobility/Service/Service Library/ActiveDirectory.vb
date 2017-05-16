Imports System.Net
Imports System.DirectoryServices

Imports UniversalHandheldServiceLibrary.Enumerations

Namespace UniversalHandheldServiceLibrary

    ''' <summary>
    ''' A class for authenticating a user's Active Directory username and password.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ActiveDirectory

        Private _path As String = "LDAP://WFM.pvt"

        Private _authenticated As Boolean
        Private _authenticationMessage As String

        Private _username As String
        Private _password As String

        Private _plugin As String
        Private _region As String


#Region " Public Properties"

        ''' <summary>
        ''' Indicates whether the specified user and password was authenticated against Active Directory successfully.
        ''' </summary>
        ''' <returns>True indicates a valid username and password. False indicates authentication failure.</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Authenticated() As Boolean
            Get
                Return _authenticated
            End Get
        End Property

        ''' <summary>
        ''' The return message when Authenticate() is called.
        ''' </summary>
        ''' <returns>One of 3 possible values: 1) User account not found, 2) Incorrect Password, or 3) the user's email address.</returns>
        ''' <remarks>The user's AD email address property is always returned when Authenticated = True.</remarks>
        Public ReadOnly Property AuthenticationMessage() As String
            Get
                Return _authenticationMessage
            End Get
        End Property

        ''' <summary>
        ''' The name of the plugin requesting authentication.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property PluginName() As String
            Get
                Return _plugin
            End Get
            Set(ByVal value As String)
                _plugin = value
            End Set
        End Property

        ''' <summary>
        ''' The region code provided by the hand held.
        ''' </summary>
        ''' <value><paramref name="Region"></paramref></value>
        ''' <returns></returns>
        ''' <remarks>This is used to lookup any security groups the calling user must be a member of for successful authentication.</remarks>
        Public Property RegionCode() As String
            Get
                Return _region
            End Get
            Set(ByVal value As String)
                _region = value
            End Set
        End Property

#End Region

#Region " Constructors"

        Public Sub New(ByVal Username As String, ByVal Password As String, ByVal EncryptedPassword As Boolean)

            _username = Username

            If EncryptedPassword Then

                Dim _encryptor As New Encryption.Encryptor

                _encryptor.Decrypt(Password)

                _password = _encryptor.Decrypt(Password)

            Else

                _password = Password

            End If

        End Sub

#End Region

#Region " Public Methods"

        ''' <summary>
        ''' Authenticates the username and password of the user in Active Directory.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Authenticate()

            Dim _authenticationResponse As AuthenticationResponse = IsValidUser(_username, _password, _region, _plugin)

            Select Case _authenticationResponse

                Case AuthenticationResponse.AccountNotFound

                    _authenticated = False
                    _authenticationMessage = "User account not found."

                Case AuthenticationResponse.Authenticated

                    _authenticated = True
                    _authenticationMessage = GetUserADProperty(_username, "mail")

                    If _authenticationMessage = "" Then
                        _authenticationMessage = _username + "@wholefoods.com"
                    End If

                Case AuthenticationResponse.BadPassword

                    _authenticated = False
                    _authenticationMessage = "Incorrect password."

                Case AuthenticationResponse.NotInPluginADGroup

                    _authenticated = False
                    _authenticationMessage = "User is not a member of the " & ConfigurationIO.GetSecurityGroupName(_plugin, _region) & " Security Group."

                Case AuthenticationResponse.NotInRegionADGroup

                    _authenticated = False
                    _authenticationMessage = "User is not a member of the " & ConfigurationIO.GetSecurityGroupName(_region) & " Security Group."

                Case AuthenticationResponse.UnknownError

                    _authenticated = False
                    _authenticationMessage = "User cannot be authenticated: There may be a problem with the service. Try again later."

            End Select

        End Sub

#End Region

#Region " Private Methods"

        ''' <summary>
        ''' This method does the actual lookup in AD of the user's account using the specified credentials.
        ''' </summary>
        ''' <param name="UserName">Domain Username</param>
        ''' <param name="Password">Domain Password</param>
        ''' <returns>An enumerated response value indicating successful authentication or the reason for authentication failure.</returns>
        ''' <remarks>Because active directory doesn't distunguish between a username not found and a bad password when the DirectorySercher
        ''' is constructed with a path, username, and password, we first construct it with a path to look for the account and return the correct
        ''' not found message when the account doesn't exist. If the account does exist, then we reconstruct the DirectorySearcher with
        ''' the path, username, and password parameters to validate the password.</remarks>
        Private Function IsValidUser(ByVal UserName As String, ByVal Password As String, ByVal RegionCode As String, ByVal PluginName As String) As AuthenticationResponse

            Dim entry As DirectoryEntry
            Dim search As DirectorySearcher
            Dim obj As Object
            Dim result As SearchResult

            Try
                ' first, lets just try to find out if the account exists
                ' construct the DirectoryEntry with just the path
                entry = New DirectoryEntry(_path)

                obj = entry.NativeObject

                search = New DirectorySearcher(entry)

                ' this is the account name we're looking for
                search.Filter = "(SAMAccountName=" + UserName + ")"
                search.PropertiesToLoad.Add("cn")
                search.PropertiesToLoad.Add("memberOf")

                ' try to find the account
                result = search.FindOne

                If result Is Nothing Then

                    ' account not found, return the appropriate response
                    Return AuthenticationResponse.AccountNotFound

                End If

                ' get the security groups the user is a member of
                Dim groupsList As Text.StringBuilder = New Text.StringBuilder
                Dim groupCount As Integer = result.Properties("memberOf").Count
                Dim counter As Integer = 0

                Do While (counter < groupCount)
                    groupsList.Append(CType(result.Properties("memberOf")(counter), String))
                    groupsList.Append("|")
                    counter = (counter + 1)
                Loop

                ' we found the account, now let's try passing in the username and password to validate the credentials
                obj = entry.NativeObject

                entry = New DirectoryEntry(_path, UserName, Password)

                search = New DirectorySearcher(entry)

                search.PropertiesToLoad.Add("cn")

                Try
                    result = search.FindOne
                Catch ex As Exception
                    Return AuthenticationResponse.BadPassword
                End Try

                ' if a region was provided, we need to check security group membership
                If RegionCode.Equals(Nothing) Then

                    Return AuthenticationResponse.RegionNotSpecified

                Else

                    ' get the regional security group reguirement
                    Dim _rGroup As String = ConfigurationIO.GetSecurityGroupName(RegionCode)

                    ' if the regional group is populated...
                    If Not _rGroup = String.Empty Then

                        If Not groupsList.ToString.Contains(_rGroup) Then

                            ' they are not in the required plugin security group
                            Return AuthenticationResponse.NotInRegionADGroup

                        End If

                    End If

                End If

                If PluginName.Equals(Nothing) Then

                    Return AuthenticationResponse.PluginNotSpecified

                Else

                    ' get the plugin-specific security group requirement
                    Dim _pGroup As String = ConfigurationIO.GetSecurityGroupName(PluginName, RegionCode)

                    If Not _pGroup = String.Empty Then

                        If Not groupsList.ToString.Contains(_pGroup) Then

                            ' they are not in the required plugin security group
                            Return AuthenticationResponse.NotInPluginADGroup

                        End If

                    End If

                End If

                ' credentials and group membership are valid
                Return AuthenticationResponse.Authenticated

            Catch ex As Exception

                ' something else went wrong
                Return AuthenticationResponse.UnknownError

            End Try

        End Function

        ''' <summary>
        ''' Retrieves a specific property associated with the specified user account.
        ''' </summary>
        ''' <param name="UserName">Domain Username</param>
        ''' <param name="PropertyName">Active Directory Property Name to retrieve</param>
        ''' <returns>Active Directory Property value.</returns>
        ''' <remarks></remarks>
        Private Function GetUserADProperty(ByVal UserName As String, ByVal PropertyName As String) As String

            Dim entry As New DirectoryEntry(_path)

            Try

                Dim obj As Object = entry.NativeObject

                Dim search As New DirectorySearcher(entry)

                search.Filter = "(SAMAccountName=" + UserName + ")"

                search.PropertiesToLoad.Add(PropertyName)

                Dim result As SearchResult = search.FindOne()

                If result Is Nothing Then
                    Return String.Empty
                End If

                Dim properties As System.DirectoryServices.ResultPropertyValueCollection = result.Properties.Item(PropertyName)

                If properties.Count > 0 Then
                    Return properties(0).ToString
                Else
                    Return String.Empty
                End If

            Catch ex As Exception

                Throw ex

            End Try

        End Function

#End Region

    End Class

End Namespace
