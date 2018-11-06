Imports log4net
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.InvoiceControlGroup.BusinessLogic
    Public Enum ControlGroupStatus
        Open
        Closed
    End Enum

    Public Class ControlGroupBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _controlGroupId As Integer = -1
        Private _expectedGrossAmt As Decimal = -1
        Private _expectedInvoiceCount As Integer = -1
        Private _status As ControlGroupStatus
        Private _statusDesc As String = Nothing
        Private _updateUserId As Integer = -1
        Private _updateUserName As String = Nothing
        Private _updateTime As Date = Nothing
        Private _orders As New ArrayList ' list of WholeFoods.IRMA.InvoiceControlGroup.BusienssLogic.ControlGroupInvoiceBO objects
        Private _ordersInvoiceTotal As Double = 0
        Private _closeStatus As Integer = -1
        Private _closeErrorMsg As String = Nothing

#Region "Property Accessors"
        Public Property ControlGroupId() As Integer
            Get
                Return _controlGroupId
            End Get
            Set(ByVal value As Integer)
                _controlGroupId = value
            End Set
        End Property

        Public Property ExpectedGrossAmt() As Decimal
            Get
                Return _expectedGrossAmt
            End Get
            Set(ByVal value As Decimal)
                _expectedGrossAmt = value
            End Set
        End Property

        Public Property ExpectedInvoiceCount() As Integer
            Get
                Return _expectedInvoiceCount
            End Get
            Set(ByVal value As Integer)
                _expectedInvoiceCount = value
            End Set
        End Property

        Public Property Status() As ControlGroupStatus
            Get
                Return _status
            End Get
            Set(ByVal value As ControlGroupStatus)
                _status = value
            End Set
        End Property

        Public Property StatusDesc() As String
            Get
                Return _statusDesc
            End Get
            Set(ByVal value As String)
                _statusDesc = value
            End Set
        End Property

        Public Property UpdateUserId() As Integer
            Get
                Return _updateUserId
            End Get
            Set(ByVal value As Integer)
                _updateUserId = value
            End Set
        End Property

        Public Property UpdateUserName() As String
            Get
                Return _updateUserName
            End Get
            Set(ByVal value As String)
                _updateUserName = value
            End Set
        End Property

        Public Property UpdateTime() As Date
            Get
                Return _updateTime
            End Get
            Set(ByVal value As Date)
                _updateTime = value
            End Set
        End Property

        Public ReadOnly Property Orders() As ArrayList
            ' Note: This property is read only to ensure the business methods are used to add and remove 
            ' orders.  The business methods also keep the running totals assigned to this object in sync.
            Get
                Return _orders
            End Get
        End Property

        Public ReadOnly Property OrdersInvoiceTotal() As Double
            Get
                Return _ordersInvoiceTotal
            End Get
        End Property

        Public Property CloseStatus() As Integer
            Get
                Return _closeStatus
            End Get
            Set(ByVal value As Integer)
                _closeStatus = value
            End Set
        End Property

        Public Property CloseErrorMsg() As String
            Get
                Return _closeErrorMsg
            End Get
            Set(ByVal value As String)
                _closeErrorMsg = value
            End Set
        End Property

#End Region

#Region "Constructors"
        ''' <summary>
        ''' Default constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            logger.Debug("New entry")
            logger.Debug("New exit")
        End Sub

        ''' <summary>
        ''' Constructor that populates the object using the data from the result set.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef results As SqlDataReader)
            logger.Debug("New entry with results")
            If (Not results.IsDBNull(results.GetOrdinal("OrderInvoice_ControlGroup_ID"))) Then
                _controlGroupId = results.GetInt32(results.GetOrdinal("OrderInvoice_ControlGroup_ID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ExpectedGrossAmt"))) Then
                _expectedGrossAmt = results.GetDecimal(results.GetOrdinal("ExpectedGrossAmt"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ExpectedInvoiceCount"))) Then
                _expectedInvoiceCount = results.GetInt32(results.GetOrdinal("ExpectedInvoiceCount"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("OrderInvoice_ControlGroupStatus_ID"))) Then
                Select Case results.GetInt32(results.GetOrdinal("OrderInvoice_ControlGroupStatus_ID"))
                    Case 1
                        _status = ControlGroupStatus.Open
                    Case 2
                        _status = ControlGroupStatus.Closed
                End Select
            End If
            If (Not results.IsDBNull(results.GetOrdinal("OrderInvoice_ControlGroupStatus_Desc"))) Then
                _statusDesc = results.GetString(results.GetOrdinal("OrderInvoice_ControlGroupStatus_Desc"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("UpdateTime"))) Then
                _updateTime = results.GetDateTime(results.GetOrdinal("UpdateTime"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("UpdateUser_ID"))) Then
                _updateUserId = results.GetInt32(results.GetOrdinal("UpdateUser_ID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("UserName"))) Then
                _updateUserName = results.GetString(results.GetOrdinal("UserName"))
            End If
            logger.Debug("New exit")
        End Sub
#End Region

#Region "Business Methods"
        ''' <summary>
        ''' Add 
        ''' </summary>
        ''' <param name="currentInvoice"></param>
        ''' <remarks></remarks>
        Public Sub AddInvoiceToOrder(ByVal currentInvoice As ControlGroupInvoiceBO)
            logger.Debug("AddInvoiceToOrder entry: InvoiceNum=" + currentInvoice.InvoiceNum.ToString())
            ' Add the order to the array list
            _orders.Add(currentInvoice)
            ' Update the running invoice total accordingly
            If currentInvoice.CreditInv Then
                _ordersInvoiceTotal = _ordersInvoiceTotal - (currentInvoice.InvoiceCost + currentInvoice.InvoiceFreight)
            Else
                _ordersInvoiceTotal = _ordersInvoiceTotal + (currentInvoice.InvoiceCost + currentInvoice.InvoiceFreight)
            End If
            logger.Debug("AddInvoiceToOrder exit")
        End Sub

        ''' <summary>
        ''' A control group is considered balanced when the expected gross amount equals the entered gross amount and
        ''' the expected invoice count equals the entered invoice count.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsControlGroupBalanced() As Boolean
            logger.Debug("IsControlGroupBalanced entry")
            Dim isBalanced As Boolean = False
            If (_orders.Count >= 1) AndAlso (_expectedInvoiceCount = _orders.Count) AndAlso (_expectedGrossAmt = _ordersInvoiceTotal) Then
                isBalanced = True
            End If
            logger.Debug("IsControlGroupBalanced exit: isBalanced=" + isBalanced.ToString())
            Return isBalanced
        End Function

#End Region
    End Class
End Namespace
