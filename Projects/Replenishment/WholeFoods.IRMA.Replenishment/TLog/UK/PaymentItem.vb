Public Class PaymentItem

    Public Enum EnumPaymentTypes
        Credit
        Check
        Cash
        FreeLunch
        GiftVoucher
        LuncheonVoucher
        Currency
        Coupons
    End Enum

    Private _PaymentType As String
    Private _PaymentAmount As Single
    Private _PaymentCount As Integer

    Sub New(ByVal Record As String, ByVal PaymentCount As Integer)

        _PaymentAmount = CSng(Record.Substring(5, 8)) / 100
        _PaymentCount = PaymentCount
        _PaymentType = Record.Substring(0, 5)

    End Sub

    Public Property PaymentAmount() As Single
        Get
            Return _PaymentAmount
        End Get
        Set(ByVal value As Single)
            _PaymentAmount = value
        End Set
    End Property

    Public Property PaymentCount() As Integer
        Get
            Return _PaymentCount
        End Get
        Set(ByVal value As Integer)
            _PaymentCount = value
        End Set
    End Property

    Public Property PaymentType() As String
        Get
            Return _PaymentType
        End Get
        Set(ByVal value As String)
            _PaymentType = value
        End Set
    End Property
    


End Class
