Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class AdminSetting

#Region "Private Properties"
    Private _AdminID As Integer
    Private _AdminKey As String
    Private _AdminValue As String
#End Region

#Region "Public Properties"

    Public Property AdminID() As Integer
        Get
            Return _AdminID
        End Get
        Friend Set(ByVal value As Integer)
            _AdminID = value
        End Set
    End Property

    Public Property AdminKey() As String
        Get
            Return _AdminKey
        End Get
        Set(ByVal value As String)
            _AdminKey = Trim(value)
        End Set
    End Property

    Public Property AdminValue() As String
        Get
            Return _AdminValue
        End Get
        Set(ByVal value As String)
            _AdminValue = Trim(value)
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function GetAdminSettings() As Data.DataSet
        Dim Dal As New Dal

        Return Dal.GetAdminSettings()
    End Function

    Public Function GetAdminSetting(ByVal AdminKey As String) As String
        Dim Dal As New Dal

        Return Dal.GetAdminSetting(AdminKey)
    End Function

    Public Function AddAdminSetting(ByVal AdminKey As String, ByVal AdminValue As String, ByVal AdminID As Integer) As Boolean
        Dim Dal As New Dal

        Me.AdminKey = AdminKey
        Me.AdminValue = AdminValue

        Dal.AddAdminSetting(Me)
    End Function

    Public Function SetAdminSetting(ByVal AdminKey As String, ByVal AdminValue As String, ByVal AdminID As Integer) As Boolean
        Dim Dal As New Dal

        Me.AdminID = AdminID
        Me.AdminKey = AdminKey
        Me.AdminValue = AdminValue

        Dal.SetAdminSetting(Me, AdminID)
    End Function

    Public Function DelAdminSetting(ByVal AdminID As Integer) As Boolean
        Dim Dal As New Dal

        Return Dal.DelAdminSetting(AdminID)
    End Function

#End Region

End Class
