Public Class LineItem
    Private _itemKey As Integer
    Private _upc As String
    Private _description As String
    Private _quantity As Integer
    Private _retailUom As String
    Private _weightedRetailUom As Byte
    Private _vendorPack As String
    Private _cost As Decimal

#Region " Public Properties"

    Public Property Item_key() As Integer
        Get
            Return _itemKey
        End Get
        Set(ByVal value As Integer)
            _itemKey = value
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

    Public Property DESCRIPTION() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Public Property Quantity() As Integer
        Get
            Return _quantity
        End Get
        Set(ByVal value As Integer)
            _quantity = value
        End Set
    End Property

    Public Property retailUOM() As String
        Get
            Return _retailUom
        End Get
        Set(ByVal value As String)
            _retailUom = value
        End Set
    End Property

    Public Property weightedItem() As Byte
        Get
            Return _weightedRetailUom
        End Get
        Set(ByVal value As Byte)
            _weightedRetailUom = value
        End Set
    End Property

    Public Property vendorPack() As String
        Get
            Return _vendorPack
        End Get
        Set(ByVal value As String)
            _vendorPack = value
        End Set
    End Property

    Public Property COST() As Decimal
        Get
            Return _cost
        End Get
        Set(ByVal value As Decimal)
            _cost = value
        End Set
    End Property
#End Region

End Class
