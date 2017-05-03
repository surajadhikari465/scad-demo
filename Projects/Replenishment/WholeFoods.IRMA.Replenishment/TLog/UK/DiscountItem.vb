Public Class DiscountItem

    Private Enum DiscountTypes
        Transaction
        Item
    End Enum
    Private _DiscountType As DiscountTypes
    Private _DiscountQty As Integer
    Private _DiscountAmount As Single
    Private _DiscountReason As String
    Private _DiscountReference As String
    Private _DiscountBarcode As String
    Private _DiscountIdentifier As String



    Sub New(ByVal Record As String)
        Select Case Record.Substring(0, 1)
            Case "D"
                Me._DiscountIdentifier = "D"
                Me._DiscountType = DiscountTypes.Transaction
                Me._DiscountQty = 1
                Me._DiscountAmount = CSng(Record.Substring(1, 8)) / 100
                Me._DiscountReason = Record.Substring(9, 20)
                Me._DiscountReference = Record.Substring(29, 10)
                Me._DiscountBarcode = ""
            Case "F"
                Me._DiscountIdentifier = "F"
                Me._DiscountType = DiscountTypes.Item
                Me._DiscountQty = 1
                Me._DiscountAmount = CSng(Record.Substring(1, 8)) / 100
                Me._DiscountBarcode = Record.Substring(9, 13)
                Me._DiscountReason = ""
                Me._DiscountReference = Record.Substring(29, 20)
                'Case "M"
                '    Me._DiscountType = DiscountTypes.Offer
                '    Me._DiscountQty = CInt(Record.Substring(4, 3))
                '    Me._DiscountAmount = CSng(Record.Substring(7, 8))
                '    Me._DiscountReason = Record.Substring(20, 19)
                '    'Me._DiscountReference = Record.Substring(40, 12)
                '    Me._DiscountBarcode = ""
        End Select
    End Sub

    Public Property DiscountIdentifier() As String
        Get
            Return _DiscountIdentifier
        End Get
        Set(ByVal value As String)
            _DiscountIdentifier = value
        End Set
    End Property

    Public Property DiscountAmount() As Single
        Get
            Return _DiscountAmount
        End Get
        Set(ByVal value As Single)
            _DiscountAmount = value
        End Set
    End Property

    Public Property DiscountReason() As String
        Get
            Return _DiscountReason
        End Get
        Set(ByVal value As String)
            _DiscountReason = value
        End Set
    End Property

    Public Property DiscountReference() As String
        Get
            Return _DiscountReference
        End Get
        Set(ByVal value As String)
            _DiscountReference = value
        End Set
    End Property

    Public Property DiscountBarcode() As String
        Get
            Return _DiscountBarcode
        End Get
        Set(ByVal value As String)
            _DiscountBarcode = value
        End Set
    End Property
End Class
