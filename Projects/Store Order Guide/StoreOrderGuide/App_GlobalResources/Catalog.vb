Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class Catalog

#Region "Private Properties"
    Private _CatalogID As Integer
    Private _ManagedByID As Integer
    Private _ManagedBy As String
    Private _CatalogCode As String
    Private _Description As String
    Private _Details As String
    Private _Published As Boolean
    Private _ExpectedDate As Boolean
    Private _SubTeam As Integer
    Private _InsertDate As Date
    Private _UpdateDate As Date
    Private _InsertUser As String
    Private _UpdateUser As String
#End Region

#Region "Public Properties"
    Public Property CatalogID() As Integer
        Get
            Return _CatalogID
        End Get
        Set(ByVal value As Integer)
            _CatalogID = value
        End Set
    End Property

    Public Property ManagedByID() As Integer
        Get
            Return _ManagedByID
        End Get
        Set(ByVal value As Integer)
            _ManagedByID = value
        End Set
    End Property

    Public Property ManagedBy() As String
        Get
            Return _ManagedBy
        End Get
        Set(ByVal value As String)
            _ManagedBy = value
        End Set
    End Property

    Public Property CatalogCode() As String
        Get
            Return _CatalogCode
        End Get
        Set(ByVal value As String)
            _CatalogCode = Trim(value)
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = Trim(value)
        End Set
    End Property

    Public Property Details() As String
        Get
            Return _Details
        End Get
        Set(ByVal value As String)
            _Details = Trim(value)
        End Set
    End Property

    Public Property Published() As Boolean
        Get
            Return _Published
        End Get
        Set(ByVal value As Boolean)
            _Published = value
        End Set
    End Property

    Public Property ExpectedDate() As Boolean
        Get
            Return _ExpectedDate
        End Get
        Set(ByVal value As Boolean)
            _ExpectedDate = value
        End Set
    End Property

    Public Property SubTeam() As Integer
        Get
            Return _SubTeam
        End Get
        Set(ByVal value As Integer)
            _SubTeam = value
        End Set
    End Property

    Public Property InsertDate() As Date
        Get
            Return _InsertDate
        End Get
        Set(ByVal value As Date)
            _InsertDate = value
        End Set
    End Property

    Public Property UpdateDate() As Date
        Get
            Return _UpdateDate
        End Get
        Set(ByVal value As Date)
            _UpdateDate = value
        End Set
    End Property

    Public Property InsertUser() As String
        Get
            Return _InsertUser
        End Get
        Set(ByVal value As String)
            _InsertUser = Trim(value)
        End Set
    End Property

    Public Property UpdateUser() As String
        Get
            Return _UpdateUser
        End Get
        Set(ByVal value As String)
            _UpdateUser = Trim(value)
        End Set
    End Property
#End Region

#Region "Public Methods"
    Public Function GetCatalogs(ByVal StoreID As Integer, ByVal SubTeamID As Integer, ByVal ZoneID As Integer, ByVal Published As Boolean, ByVal CatalogCode As String, ByVal Order As Boolean, ByVal CatalogID As Integer) As Data.DataSet
        Dim Dal As New Dal

        Return Dal.GetCatalogs(StoreID, SubTeamID, ZoneID, Published, CatalogCode, Order, CatalogID)
    End Function

    Public Function AddCatalog(ByVal ManagedByID As Integer, ByVal CatalogCode As String, ByVal Description As String, ByVal Published As Boolean, ByVal SubTeam As Integer, ByVal ExpectedDate As Boolean, ByVal InsertUser As String, ByVal Copy As Boolean, ByVal CatalogID As Integer, ByVal Details As String) As Boolean
        Dim Dal As New Dal

        Me.ManagedByID = ManagedByID
        Me.CatalogCode = CatalogCode
        Me.Description = Description
        Me.Published = Published
        Me.SubTeam = SubTeam
        Me.ExpectedDate = ExpectedDate
        Me.InsertUser = InsertUser
        Me.Details = Details

        Dal.AddCatalog(Me, Copy, CatalogID)
    End Function

    Public Function SetCatalog(ByVal CatalogID As Integer, ByVal ManagedByID As Integer, ByVal CatalogCode As String, ByVal Description As String, ByVal Details As String, ByVal Published As Boolean, ByVal SubTeam As Integer, ByVal ExpectedDate As Boolean, ByVal UpdateUser As String) As Boolean
        Dim Dal As New Dal

        Me.CatalogID = CatalogID
        Me.ManagedByID = ManagedByID
        Me.CatalogCode = CatalogCode
        Me.Description = Description
        Me.Details = Details
        Me.Published = Published
        Me.SubTeam = SubTeam
        Me.ExpectedDate = ExpectedDate
        Me.UpdateUser = UpdateUser

        Dal.SetCatalog(Me, CatalogID)
    End Function

    Public Function DelCatalog(ByVal CatalogID As Integer) As Boolean
        Dim Dal As New Dal

        Return Dal.DelCatalog(CatalogID)
    End Function
#End Region

End Class