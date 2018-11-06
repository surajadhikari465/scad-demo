Public Class TransactionItem

    Private _SalesQty As Integer
    Private _VatCode As Integer
    Private _BarCode As String
    Private _TransType As String
    Private _RetailPrice As Single
    Private _DepartmentNumber As Integer
    Private _SalesValue As Single
    Private _SerialNumber As String
    Private _WeightIndicator As String
    Private _Weight As Single
    Private _TurnoverDepartment As String
    Private _Size As String
    Private _RefundReason As String
    Private _ItemDescription As String
    Private _VehicleRegistration As String
    Private _Litres As String
    Private _IsVoid As Boolean = False
    Private _RowNo As Integer = 0





    Sub New(ByVal Record As String, Optional ByVal RowNo As Integer = 0)
        Me._SalesQty = CInt(Record.Substring(1, 4))
        Me._VatCode = CInt(Record.Substring(5, 2))
        Me._BarCode = Record.Substring(7, 13)
        'Debug.WriteLine(_BarCode)
        Me._TransType = Record.Substring(20, 4)
        If _TransType = "VOID" Then _IsVoid = True
        Me._RetailPrice = CSng(Record.Substring(24, 8)) / 100
        Me._DepartmentNumber = CInt(Record.Substring(32, 3))
        Me._SalesValue = CSng(Record.Substring(35, 8)) / 100
        Me._SerialNumber = Record.Substring(43, 6)
        Me._WeightIndicator = Record.Substring(49, 1)
        If Record.Substring(50, 4).Trim().Equals("") Then Me._Weight = 0 Else Me._Weight = CSng(Record.Substring(50, 4)) / 1000
        Me._TurnoverDepartment = Record.Substring(54, 4)
        Me._Size = Record.Substring(58, 4)
        Me._RefundReason = Record.Substring(62, 1)
        Me._ItemDescription = Record.Substring(63, 25)
        Me._VehicleRegistration = Record.Substring(88, 9)
        Me._Litres = Record.Substring(92, 6)
        Me._RowNo = RowNo
    End Sub

#Region "Properties"
    Public Property BarCode() As String
        Get
            Return _BarCode
        End Get
        Set(ByVal value As String)
            _BarCode = value
        End Set
    End Property

    Public Property DepartmentNumber() As Integer
        Get
            Return _DepartmentNumber
        End Get
        Set(ByVal value As Integer)
            _DepartmentNumber = value
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

    Public Property Litres() As String
        Get
            Return _Litres
        End Get
        Set(ByVal value As String)
            _Litres = value
        End Set
    End Property

    Public Property RefundReason() As String
        Get
            Return _RefundReason
        End Get
        Set(ByVal value As String)
            _RefundReason = value
        End Set
    End Property

    Public Property RetailPrice() As Single
        Get
            Return _RetailPrice
        End Get
        Set(ByVal value As Single)
            _RetailPrice = value
        End Set
    End Property

    Public Property SalesQty() As Integer
        Get
            Return _SalesQty
        End Get
        Set(ByVal value As Integer)
            _SalesQty = value
        End Set
    End Property

    Public Property Salesvalue() As Single
        Get
            Return _SalesValue
        End Get
        Set(ByVal value As Single)
            _SalesValue = value
        End Set
    End Property

    Public Property SerialNumber() As String
        Get
            Return _SerialNumber
        End Get
        Set(ByVal value As String)
            _SerialNumber = value
        End Set
    End Property

    Public Property Size() As String
        Get
            Return _Size
        End Get
        Set(ByVal value As String)
            _Size = value
        End Set
    End Property

    Public Property TransType() As String
        Get
            Return _TransType
        End Get
        Set(ByVal value As String)
            _TransType = value
        End Set
    End Property

    Public Property TurnoverDepartment() As String
        Get
            Return _TurnoverDepartment
        End Get
        Set(ByVal value As String)
            _TurnoverDepartment = value
        End Set
    End Property

    Public Property VatCode() As Integer
        Get
            Return _VatCode
        End Get
        Set(ByVal value As Integer)
            _VatCode = value
        End Set
    End Property

    Public Property VehicleRegistration() As String
        Get
            Return _VehicleRegistration
        End Get
        Set(ByVal value As String)
            _VehicleRegistration = value
        End Set
    End Property

    Public Property Weight() As Single
        Get
            Return _Weight
        End Get
        Set(ByVal value As Single)
            _Weight = value
        End Set
    End Property

    Public Property WeightIndicator() As String
        Get
            Return _WeightIndicator
        End Get
        Set(ByVal value As String)
            _WeightIndicator = value
        End Set
    End Property

    Public Property IsVoid() As Boolean
        Get
            Return _IsVoid
        End Get
        Set(ByVal value As Boolean)
            _IsVoid = value

        End Set
    End Property

    Public Property RowNo() As Integer
        Get
            Return _RowNo
        End Get
        Set(ByVal value As Integer)
            _RowNo = value
        End Set
    End Property
#End Region

End Class
