Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class OrderItem

#Region "Private Properties"
    Private _CatalogOrderID As Integer
    Private _CatalogItemID As Integer
    Private _Quantity As Integer
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

    Public Property CatalogItemID() As Integer
        Get
            Return _CatalogItemID
        End Get
        Set(ByVal value As Integer)
            _CatalogItemID = value
        End Set
    End Property

    Public Property Quantity() As Integer
        Get
            Return _Quantity
        End Get
        Set(ByVal value As Integer)
            _Quantity = value
        End Set
    End Property
#End Region

#Region "Public Methods"
    Public Function AddOrderItem(ByVal CatalogOrderID As Integer, ByVal CatalogItemID As Integer, ByVal Quantity As Integer) As Boolean
        Dim Dal As New Dal

        Me.CatalogOrderID = CatalogOrderID
        Me.CatalogItemID = CatalogItemID
        Me.Quantity = Quantity

        Dal.AddOrderItem(Me, CatalogOrderID)
    End Function
#End Region
End Class