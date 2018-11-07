Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Caching

Public Class User

#Region "Private Properties"
    Private _UserID As Integer
    Private _UserName As String
    Private _Password As String
    Private _Admin As Boolean
    Private _Warehouse As Boolean
    Private _Schedule As Boolean
    Private _SuperUser As Boolean
    Private _Buyer As Boolean
    Private _StoreNo As Integer
    Private _Email As String
#End Region

#Region "Public Properties"
    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value.Trim
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value.Trim
        End Set
    End Property

    Public Property Admin() As Boolean
        Get
            Return _Admin
        End Get
        Set(ByVal value As Boolean)
            _Admin = value
        End Set
    End Property

    Public Property Warehouse() As Boolean
        Get
            Return _Warehouse
        End Get
        Set(ByVal value As Boolean)
            _Warehouse = value
        End Set
    End Property

    Public Property Schedule() As Boolean
        Get
            Return _Schedule
        End Get
        Set(ByVal value As Boolean)
            _Schedule = value
        End Set
    End Property

    Public Property Buyer() As Boolean
        Get
            Return _Buyer
        End Get
        Set(ByVal value As Boolean)
            _Buyer = value
        End Set
    End Property

    Public Property SuperUser() As Boolean
        Get
            Return _SuperUser
        End Get
        Set(ByVal value As Boolean)
            _SuperUser = value
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return _StoreNo
        End Get
        Set(ByVal value As Integer)
            _StoreNo = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property
#End Region

#Region "Class Methods"
    Sub New(ByVal UserName As String, ByVal Password As String)
        If UserName.Length = 0 Then
            Throw New Exception("The UserName was not provided.")
        End If

        If Password.Length = 0 Then
            Throw New Exception("The Password was not provided.")
        End If

        Me.UserName = UserName
        Me.Password = Password
    End Sub
#End Region

#Region "Public Methods"
    Public Shared Sub Logout()
        HttpContext.Current.Session.Clear()
        FormsAuthentication.RedirectToLoginPage()
    End Sub

    Public Function Login() As Boolean
        Dim Dal As New Dal

        Dim ValidUser As Boolean

        '//Validate user against AD
        Try
            Dim entry As New DirectoryServices.DirectoryEntry("LDAP://wfm.pvt/DC=wfm,DC=pvt", UserName, Password)
            Dim mySearcher As New DirectoryServices.DirectorySearcher(entry)
            mySearcher.Filter = "(samaccountname=" & UserName & ")"
            mySearcher.PropertiesToLoad.Add("cn")
            Dim result As DirectoryServices.SearchResult = mySearcher.FindOne()
            entry = Nothing
            mySearcher = Nothing
            If result Is Nothing Then
                ValidUser = False
            Else
                ValidUser = True
            End If
        Catch ex As Exception
            Throw
        End Try

        '//Validate user against IRMA
        Try
            If ValidUser Then
                Dal.GetUserDetails(Me)

                Return True
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        End Try
    End Function
#End Region

End Class