Imports log4net

Public Class GLBO

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _sDate As String
    Private _eDate As String
    Private _cDate As String
    Private _storeNo As Integer
    Private _type As Integer
    Private _journalDate As String
    Private _orderHeaderID As String
    Private _amount As Decimal
    Private _businessUnitID As Integer
    Private _accountNumber As String
    Private _departmentID As Integer
    Private _productId As Integer
    Private _transferBusinessUnitID As Integer
    Private _sameStore As Integer
    Private _currency As String = "USD"

    Public Property Currency() As String
        Get
            Return _currency
        End Get
        Set(ByVal value As String)
            _currency = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return _sDate
        End Get
        Set(ByVal value As String)
            _sDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _eDate
        End Get
        Set(ByVal value As String)
            _eDate = value
        End Set
    End Property

    Public Property CurrentDate() As String
        Get
            Return _cDate
        End Get
        Set(ByVal value As String)
            _cDate = value
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return _storeNo
        End Get
        Set(ByVal value As Integer)
            _storeNo = value
        End Set
    End Property

    Public Property TransactionType() As Integer
        Get
            Return _type
        End Get
        Set(ByVal value As Integer)
            _type = value
        End Set
    End Property

    Public Property JournalDate() As String
        Get
            Return _journalDate
        End Get
        Set(ByVal value As String)
            _journalDate = value
        End Set
    End Property

    Public Property OrderHeaderID() As String
        Get
            Return _orderHeaderID
        End Get
        Set(ByVal value As String)
            _orderHeaderID = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return _amount
        End Get
        Set(ByVal value As Decimal)
            _amount = value
        End Set
    End Property

    Public Property BusinessUnitId() As Integer
        Get
            Return _businessUnitID
        End Get
        Set(ByVal value As Integer)
            _businessUnitID = value
        End Set
    End Property

    Public Property AccountNumber() As String
        Get
            Return _accountNumber
        End Get
        Set(ByVal value As String)
            _accountNumber = value
        End Set
    End Property

    Public Property DepartmentId() As Integer
        Get
            Return _departmentID
        End Get
        Set(ByVal value As Integer)
            _departmentID = value
        End Set
    End Property

    Public Property ProductId() As Integer
        Get
            Return _productId
        End Get
        Set(ByVal value As Integer)
            _productId = value
        End Set
    End Property

    Public Property TransferBusinessUnitID() As Integer
        Get
            Return _transferBusinessUnitID
        End Get
        Set(ByVal value As Integer)
            _transferBusinessUnitID = value
        End Set
    End Property

    Public Property SameStore() As Integer
        Get
            Return _sameStore
        End Get
        Set(ByVal value As Integer)
            _sameStore = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal dr As DataRow)
        If dr.Table.Columns.Contains("OrderHeader_ID") Then
            If (dr("OrderHeader_ID").ToString & "" <> "") Then
                OrderHeaderID = dr("OrderHeader_ID").ToString
            End If
        End If

        If (dr("Amount").ToString & "" <> "") Then
            Amount = CDec(dr("Amount"))
        End If

        If (dr("Unit").ToString & "" <> "") Then
            BusinessUnitId = CInt(dr("Unit"))
        End If

        If (dr("Account").ToString & "" <> "") Then
            AccountNumber = dr("Account").ToString
        End If

        If (dr("DeptId").ToString & "" <> "") Then
            DepartmentId = CInt(dr("DeptId"))
        End If

        If (dr("Product").ToString & "" <> "") Then
            ProductId = CInt(dr("Product"))
        End If

        If (dr("SameStore").ToString & "" <> "") Then
            SameStore = CInt(dr("SameStore"))
        End If

        If (dr("Currency").ToString & "" <> "") Then
            Currency = dr("Currency").ToString
        End If

        'task BR 11422 added output for transfer business units for transfer orders only
        If dr.Table.Columns.Contains("TransferUnit") Then
            If (dr("TransferUnit").ToString & "" <> "") Then
                TransferBusinessUnitID = CInt(dr("TransferUnit"))
            End If
        End If

    End Sub
End Class
