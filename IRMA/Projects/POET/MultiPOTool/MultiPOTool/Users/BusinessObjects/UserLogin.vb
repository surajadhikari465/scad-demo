Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Caching

Public Class BOUserLogin
    Inherits System.Web.UI.Page

    Private Shadows user As String
    Private passw As String
    Sub New(ByVal UserName As String, ByVal Password As String)
        user = UserName
        passw = Password
    End Sub

    Public Property UserName() As String
        Get
            Return user
        End Get
        Set(ByVal value As String)
            user = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return passw
        End Get
        Set(ByVal value As String)
            passw = value
        End Set
    End Property

    ''' <summary>
    ''' GET ALL USER CREDENTIALS BY USER_ID
    ''' </summary>
    ''' <returns>DataSet</returns>
    Public Function GetUserCredentials() As DataTable

        Dim val As New DALGetValidUser()

        Return val.GetValidUserCredentials(val.GetValidUserID(user))

    End Function

    ''' <summary>
    ''' VALIDATE ACTIVE DIRECTORY
    ''' </summary>
    Public Function ValidateUserActiveDirectory() As Boolean

        Dim AD As New AD_Access.AD_Access(user, passw)

        Dim ValidUser As Boolean

        Try
            ValidUser = AD.IsValidUser
            If ValidUser Then
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' VALIDATE USER TABLE
    ''' </summary>
    Public Function ValidateUserTable() As Boolean

        Dim val As New DALGetValidUser()

        Return CBool(val.IsValidUser(user))

    End Function

    ''' <summary>
    ''' FILL UP THE CACHE WITH THE CREDENTIALS TO PERSIST STATE IN WEBSITES
    ''' </summary>
    Public Sub FillCacheUserCredentials(ByVal dt As DataTable)

        Dim dc As DataColumn

        For Each dc In dt.Columns

            Session(dc.ColumnName) = dt.Rows(0).Item(dc.ColumnName)

        Next


    End Sub
End Class
