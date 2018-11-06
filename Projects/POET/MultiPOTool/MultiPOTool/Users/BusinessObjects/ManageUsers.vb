Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class BOManageUsers

    Private _userID As Integer
    Private _lastUserID As Integer
    Private _userFields As ArrayList
    Public Property UserID() As Integer
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Property LastAddedUserID() As Integer
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Property UserFields() As ArrayList
        Get

        End Get
        Set(ByVal value As ArrayList)

        End Set
    End Property

    ''' <summary>
    ''' DEFINE USER TABLE
    ''' </summary>
    Private Property UserTable() As DataTable
        Get
            Dim Ut As New DataTable
            Dim pk As DataColumn = Ut.Columns.Add("UserID", Type.GetType("System.Int32"))
            Ut.Columns.Add("UserName", Type.GetType("System.String"))
            Ut.Columns.Add("RegionID", Type.GetType("System.Int32"))
            Ut.Columns.Add("GlobalBuyer", Type.GetType("System.Boolean"))
            Ut.Columns.Add("Administrator", Type.GetType("System.Boolean"))
            Ut.Columns.Add("Active", Type.GetType("System.Boolean"))
            Ut.Columns.Add("InsertDate", Type.GetType("System.DateTime"))
            Ut.Columns.Add("Email", Type.GetType("System.String"))
            Ut.Columns.Add("CCEmail", Type.GetType("System.String"))
            Ut.PrimaryKey = New DataColumn() {pk}

            Return Ut

        End Get
        Set(ByVal value As DataTable)
            Dim Ut As New DataTable("Users")
            Dim pk As DataColumn = Ut.Columns.Add("UserID", Type.GetType("System.Int32"))
            Ut.Columns.Add("UserName", Type.GetType("System.String"))
            Ut.Columns.Add("RegionID", Type.GetType("System.Int32"))
            Ut.Columns.Add("GlobalBuyer", Type.GetType("System.Boolean"))
            Ut.Columns.Add("Administrator", Type.GetType("System.Boolean"))
            Ut.Columns.Add("Active", Type.GetType("System.Boolean"))
            Ut.Columns.Add("InsertDate", Type.GetType("System.DateTime"))
            Ut.Columns.Add("Email", Type.GetType("System.String"))
            Ut.Columns.Add("CCEmail", Type.GetType("System.String"))
            Ut.PrimaryKey = New DataColumn() {pk}

            Ut = value
        End Set
    End Property

    Public Function IsAdmin(ByVal UserID As Integer) As Boolean

        Dim a As New DALManageUsers

        Return a.IsAdmin(UserID)

    End Function

    ''' <summary>
    ''' ADD USER
    ''' </summary>
    Public Function InsertUser(ByVal param As ArrayList) As Integer

        Dim az As New DALManageUsers

        az.InsertUser(param)

    End Function

    ''' <summary>
    ''' DELETE USER
    ''' </summary>
    Public Function DeleteUser(ByVal UserID As Integer) As Integer

        Dim az As New DALManageUsers
        az.DeleteUser(UserID)

    End Function

    ''' <summary>
    ''' UPDATE USER
    ''' </summary>
    Public Function UpdateUser(ByVal param As ArrayList) As Integer

        Dim az As New DALManageUsers

        az.UpdateUser(param)

    End Function

    ''' <summary>
    ''' SELECTALLUSERS - DATAGRID FILL
    ''' </summary>
    Public Function SelectAllUsers() As DataSet

        Dim az As New DALManageUsers

        Return az.GetAllUsers

    End Function

    ''' <summary>
    ''' SELECT ONE USER BY ID
    ''' </summary>
    Public Function SelectUserByID(ByVal UserId As Integer) As DataSet


    End Function

    Public Function SelectRegions() As DataSet

        Dim az As New DALManageUsers

        Return az.GetRegions


    End Function

    Public Function SelectAppVersion() As String

        Dim az As New DALManageUsers

        Return az.GetAppVersion


    End Function

End Class
