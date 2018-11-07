Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class OrderItemObject

    Private _upc As String
    Private _freeQuantity As Integer
    Private _ItemBrand As String
    Private _ItemDescription As String
    Private _orderQuantity As Integer
    Private _rowOrdinal As Integer
    Private _vin As String
    Private _itemKey As Integer
    Private _discountType As String
    Private _discountAmount As Double
    Private _ItemReasonCode As String

    Sub New()

    End Sub

    Public Property ItemReasonCode() As String
        Get
            Return _ItemReasonCode
        End Get
        Set(ByVal value As String)
            _ItemReasonCode = value
        End Set
    End Property
    Public Property DiscountType() As String
        Get
            Return _discountType
        End Get
        Set(ByVal value As String)
            _discountType = value
        End Set
    End Property

    Public Property DiscountAmount() As Double
        Get
            Return _discountAmount
        End Get
        Set(ByVal value As Double)
            _discountAmount = value
        End Set
    End Property

    Public Property UPC() As String
        Get
            Return _upc
        End Get
        Set(ByVal value As String)
            _upc = value
        End Set
    End Property

    Public Property VIN() As String
        Get
            Return _vin
        End Get
        Set(ByVal value As String)
            _vin = value
        End Set
    End Property

    Public Property Item_key() As Integer
        Get
            Return _itemKey
        End Get
        Set(ByVal value As Integer)
            _itemKey = value
        End Set
    End Property

    Public Property ItemBrand() As String
        Get
            Return _ItemBrand
        End Get
        Set(ByVal value As String)
            _ItemBrand = value
        End Set
    End Property

    Public Property ItemDescription() As String
        Get
            Return _ItemDescription
        End Get
        Set(ByVal value As String)
            _ItemDescription = value
        End Set
    End Property

    Public Property OrderQuantity() As Integer
        Get
            Return _orderQuantity
        End Get
        Set(ByVal value As Integer)
            _orderQuantity = value
        End Set
    End Property

    Public Property FreeQuantity() As Integer
        Get
            Return _freeQuantity
        End Get
        Set(ByVal value As Integer)
            _freeQuantity = value
        End Set
    End Property

    Public Property RowOrdinal() As Integer
        Get
            Return _rowOrdinal
        End Get
        Set(ByVal value As Integer)
            _rowOrdinal = value
        End Set
    End Property
End Class
