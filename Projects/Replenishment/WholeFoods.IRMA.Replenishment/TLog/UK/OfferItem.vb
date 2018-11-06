Public Class OfferItem


    Private _OfferQty As Integer
    Private _OfferDiscount As Single
    Private _TableNumber As Integer
    Private _OfferDescription As String
    Private _OfferReference As String
    Private _ItemBarcode As String



#Region "Properties"
    Public Property OfferQty() As Integer
        Get
            Return _OfferQty
        End Get
        Set(ByVal value As Integer)
            _OfferQty = value
        End Set
    End Property

    Public Property OfferDiscount() As Single
        Get
            Return _OfferDiscount
        End Get
        Set(ByVal value As Single)
            _OfferDiscount = value
        End Set
    End Property
    Public Property TableNumber() As Integer
        Get
            Return _TableNumber
        End Get
        Set(ByVal value As Integer)
            _TableNumber = value
        End Set
    End Property

    Public Property OfferDescription() As String
        Get
            Return _OfferDescription
        End Get
        Set(ByVal value As String)
            _OfferDescription = value
        End Set
    End Property

    Public Property OfferReference() As String
        Get
            Return _OfferReference
        End Get
        Set(ByVal value As String)
            _OfferReference = value
        End Set
    End Property

    Public Property ItemBarcode() As String
        Get
            Return _ItemBarcode
        End Get
        Set(ByVal value As String)
            _ItemBarcode = value
        End Set
    End Property
#End Region

    Sub New(ByVal Record As String, ByVal TriggerItem As String)
        Me._OfferQty = CInt(Record.Substring(3, 3))
        Me._OfferDiscount = CSng(Record.Substring(6, 8)) / 100
        Me._TableNumber = CInt(Record.Substring(14, 5))
        Me._OfferDescription = Record.Substring(19, 20)
        Me._ItemBarcode = TriggerItem.Substring(7, 13)
        Try
            Me._OfferReference = Record.Substring(39, 12)
        Catch
            Me._OfferReference = ""
        End Try
    End Sub

End Class
