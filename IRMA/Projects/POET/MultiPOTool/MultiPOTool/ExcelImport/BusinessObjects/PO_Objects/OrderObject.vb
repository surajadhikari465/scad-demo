Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class OrderObject

    Private _expectedDate As DateTime
    Private _autoPushDate As DateTime
    Private _pushedByUserID As Integer
    Private _subTeam As Integer
    Private _orderItems As List(Of OrderItemObject)
    Private _poNumber As PONumberObject
    Private _store As StoreObject
    Private _vendor As VendorObject
    Private _numberOfItems As Integer
    Private _poHeaderID As Integer
    Private _notes As String
    Private _discountType As String
    Private _discountAmount As Double
    Private _HeaderReasonCode As String

    Public Property HeaderReasonCode() As String
        Get
            Return _HeaderReasonCode
        End Get
        Set(ByVal value As String)
            _HeaderReasonCode = value
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

    Public Property SubTeam() As Integer
        Get
            Return _subTeam
        End Get
        Set(ByVal value As Integer)
            _subTeam = value
        End Set
    End Property

    Public Property ExpectedDate() As DateTime
        Get
            Return _expectedDate
        End Get
        Set(ByVal value As DateTime)
            _expectedDate = value
        End Set
    End Property

    Public Property AutoPushDate() As DateTime
        Get
            Return _autoPushDate
        End Get
        Set(ByVal value As DateTime)
            _autoPushDate = value
        End Set
    End Property

    Public Property PushedByUserID() As Integer
        Get
            Return _pushedByUserID
        End Get
        Set(ByVal value As Integer)
            _pushedByUserID = value
        End Set
    End Property

    Public Property OrderItems() As List(Of OrderItemObject)
        Get
            Return _orderItems
        End Get
        Set(ByVal value As List(Of OrderItemObject))
            _orderItems = value
            Me.GetNumberOfItems()
        End Set
    End Property

    Public Property PONumber() As PONumberObject
        Get
            Return _poNumber
        End Get
        Set(ByVal value As PONumberObject)
            _poNumber = value
        End Set
    End Property

    Public Property Store() As StoreObject
        Get
            Return _store
        End Get
        Set(ByVal value As StoreObject)
            _store = value
        End Set
    End Property

    Public Property Vendor() As VendorObject
        Get
            Return _vendor
        End Get
        Set(ByVal value As VendorObject)
            _vendor = value
        End Set
    End Property

    Public Property NumberOfItems() As Integer
        Get
            Return _numberOfItems
        End Get
        Set(ByVal value As Integer)
            _numberOfItems = value
        End Set
    End Property

    Public Property POHeaderID() As Integer
        Get
            Return _poHeaderID
        End Get
        Set(ByVal value As Integer)
            _poHeaderID = value
        End Set
    End Property

    Public Property Notes() As String
        Get
            Return _notes
        End Get
        Set(ByVal value As String)
            _notes = value
        End Set
    End Property

    Public Function GetNumberOfItems() As Integer

        _numberOfItems = OrderItems.Count

    End Function

    Public Function GetTotalItemQuantity() As Integer

        Dim totalQuantity As Integer
        Dim orderitem As New OrderItemObject

        For Each orderitem In _orderItems

            totalQuantity += orderitem.OrderQuantity

        Next

        Return totalQuantity

    End Function

End Class
