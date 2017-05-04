Public Class VoidItem

    Private _Date As String
    Private _StoreNumber As Integer
    Private _RegisterNumber As Integer
    Private _TransactionNumber As Integer
    Private _OperatorNumber As Integer
    Private _Time As String
    Private _SalesValue As Single


    Sub New(ByVal Record As String)
        Me._Date = Record.Substring(8, 8)
        Me._StoreNumber = CInt(Record.Substring(16, 6))
        Me._RegisterNumber = CInt(Record.Substring(22, 2))
        Me._TransactionNumber = CInt(Record.Substring(24, 6))
        Me._OperatorNumber = CInt(Record.Substring(30, 4))
        Me._Time = Record.Substring(34, 6)
        Me._SalesValue = CSng(Record.Substring(40, 8)) / 100
    End Sub

    Public Property VoidDate() As String
        Get
            Return _Date
        End Get
        Set(ByVal value As String)
            _Date = value
        End Set
    End Property
    Public Property StoreNumber() As Integer
        Get
            Return _StoreNumber
        End Get
        Set(ByVal value As Integer)
            _StoreNumber = value
        End Set
    End Property
    Public Property RegisterNumber() As Integer
        Get
            Return _RegisterNumber
        End Get
        Set(ByVal value As Integer)
            _RegisterNumber = value
        End Set
    End Property
    Public Property TransactionNumber() As Integer
        Get
            Return _TransactionNumber
        End Get
        Set(ByVal value As Integer)
            _TransactionNumber = value
        End Set
    End Property

    Public Property OperatorNumber() As Integer
        Get
            Return _OperatorNumber
        End Get
        Set(ByVal value As Integer)
            _OperatorNumber = value
        End Set
    End Property

    Public Property VoidTime() As String
        Get
            Return _Time
        End Get
        Set(ByVal value As String)
            _Time = value
        End Set
    End Property

    Public Property SalesValue() As Single
        Get
            Return _SalesValue
        End Get
        Set(ByVal value As Single)
            _SalesValue = value
        End Set
    End Property





End Class
