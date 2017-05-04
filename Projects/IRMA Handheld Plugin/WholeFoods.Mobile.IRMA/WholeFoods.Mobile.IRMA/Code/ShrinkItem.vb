Public Class ShrinkItem

    Private itemUpc As String
    Private itemQty As Integer
    Private itemUom As String

#Region " Public Properties"

    Public Property UPC() As String
        Get
            Return itemUpc
        End Get
        Set(ByVal value As String)
            itemUpc = value
        End Set
    End Property

    Public Property QTY() As Integer
        Get
            Return itemQty
        End Get
        Set(ByVal value As Integer)
            itemQty = value
        End Set
    End Property

    Public Property UOM() As String
        Get
            Return itemUom
        End Get
        Set(ByVal value As String)
            itemUom = value
        End Set
    End Property

#End Region

End Class
