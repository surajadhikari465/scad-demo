Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class Order
    Dim Common As StoreOrderGuide.Common

#Region "Private Properties"
    Private _CatalogOrderID As Integer
    Private _CatalogID As Integer
    Private _VendorID As Integer
    Private _StoreID As Integer
    Private _UserID As Integer
    Private _FromSubTeamID As Integer
    Private _ToSubTeamID As Integer
    Private _ExpectedDate As Date
#End Region

#Region "Public Properties"
    Public Property CatalogOrderID() As Integer
        Get
            Return _CatalogOrderID
        End Get
        Set(ByVal value As Integer)
            _CatalogOrderID = value
        End Set
    End Property

    Public Property CatalogID() As Integer
        Get
            Return _CatalogID
        End Get
        Set(ByVal value As Integer)
            _CatalogID = value
        End Set
    End Property

    Public Property VendorID() As Integer
        Get
            Return _VendorID
        End Get
        Set(ByVal value As Integer)
            _VendorID = value
        End Set
    End Property

    Public Property StoreID() As Integer
        Get
            Return _StoreID
        End Get
        Set(ByVal value As Integer)
            _StoreID = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property FromSubTeamID() As Integer
        Get
            Return _FromSubTeamID
        End Get
        Set(ByVal value As Integer)
            _FromSubTeamID = value
        End Set
    End Property

    Public Property ToSubTeamID() As Integer
        Get
            Return _ToSubTeamID
        End Get
        Set(ByVal value As Integer)
            _ToSubTeamID = value
        End Set
    End Property

    Property ExpectedDate() As Date
        Get
            Return IIf(_ExpectedDate.Date = #12:00:00 AM#, "1/1/1900", _ExpectedDate.Date)
        End Get
        Set(ByVal value As Date)
            _ExpectedDate = value
        End Set
    End Property
#End Region

#Region "Public Methods"
    Public Function AddOrder(ByVal CatalogID As Integer, ByVal VendorID As Integer, ByVal StoreID As Integer, ByVal UserID As Integer, ByVal FromSubTeamID As Integer, ByVal ToSubTeamID As Integer, ByVal CatalogOrderID As Integer, ByVal ExpectedDate As Date) As Integer
        Dim Dal As New Dal

        Me.CatalogID = CatalogID
        Me.VendorID = VendorID
        Me.StoreID = StoreID
        Me.UserID = UserID
        Me.FromSubTeamID = FromSubTeamID
        Me.ToSubTeamID = ToSubTeamID
        Me.ExpectedDate = ExpectedDate

        Try
            Me.CatalogOrderID = Dal.AddOrder(Me, CatalogID)
        Catch ex As Exception
            Common.LogError(ex)
        End Try

        Return Me.CatalogOrderID
    End Function

    Public Function SetOrder(ByVal CatalogOrderID As Integer) As Data.DataSet
        Dim Dal As New Dal

        Me.CatalogOrderID = CatalogOrderID

        Return Dal.SetOrder(Me, CatalogOrderID)
    End Function
#End Region

End Class