Imports log4net
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Ordering.BusinessLogic
    Public Class SuspendedInvoiceBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ' Vendor Invoice Data
        Private _invoiceDate As Date
        Private _invoiceNum As String
        Private _invoiceSubteamNo As Integer
        Private _vendorID As Integer
        Private _vendorKey As String
        Private _invoiceTotal As Decimal              ' Combined total for vendor freight + vendor cost
        Private _docDataExists As Boolean

        ' Purchase Order Data
        Private _orderHeaderID As Integer
        Private _poTotal As Decimal                   ' Combined total for vendor freight + vendor cost
        Private _costDiff As Decimal
        Private _vendorName As String                 ' TFS 10291
        Private _vStoreID As Integer                  ' TFS 10291
        Private _vStoreName As String                 ' TFS 10291
        Private _payByAgreedCost As Boolean           ' TFS 10291
        Private _vState As String                     ' TFS 10291
        Private _vZoneID As Integer                   ' TFS 10291

        Private _createdBy As Integer
        Private _closedBy As Integer


#Region "Property Accessors"
        Public Property CreatedBy() As Integer
            Get
                Return _createdBy
            End Get
            Set(ByVal value As Integer)
                _createdBy = value
            End Set
        End Property

        Public Property ClosedBy() As Integer
            Get
                Return _closedby
            End Get
            Set(ByVal value As Integer)
                _closedby = value
            End Set
        End Property

        Public Property InvoiceDate() As Date
            Get
                Return _invoiceDate
            End Get
            Set(ByVal value As Date)
                _invoiceDate = value
            End Set
        End Property

        Public Property InvoiceNum() As String
            Get
                Return _invoiceNum
            End Get
            Set(ByVal value As String)
                _invoiceNum = value
            End Set
        End Property

        Public Property InvoiceSubteamNo() As Integer
            Get
                Return _invoiceSubteamNo
            End Get
            Set(ByVal value As Integer)
                _invoiceSubteamNo = value
            End Set
        End Property

        Public Property VendorID() As Integer
            Get
                Return _vendorID
            End Get
            Set(ByVal value As Integer)
                _vendorID = value
            End Set
        End Property

        Public Property VendorKey() As String
            Get
                Return _vendorKey
            End Get
            Set(ByVal value As String)
                _vendorKey = value
            End Set
        End Property

        Public Property InvoiceTotal() As Decimal
            Get
                Return _invoiceTotal
            End Get
            Set(ByVal value As Decimal)
                _invoiceTotal = value
            End Set
        End Property

        Public Property DocDataExists() As Boolean
            Get
                Return _docDataExists
            End Get
            Set(ByVal value As Boolean)
                _docDataExists = value
            End Set
        End Property

        Public Property OrderHeaderID() As Integer
            Get
                Return _orderHeaderID
            End Get
            Set(ByVal value As Integer)
                _orderHeaderID = value
            End Set
        End Property

        Public Property POTotal() As Decimal
            Get
                Return _poTotal
            End Get
            Set(ByVal value As Decimal)
                _poTotal = value
            End Set
        End Property

        Public Property CostDiff() As Decimal
            Get
                Return _costDiff
            End Get
            Set(ByVal value As Decimal)
                _costDiff = value
            End Set
        End Property

        Public Property VendorName() As String
            Get
                Return _vendorName
            End Get
            Set(ByVal value As String)
                _vendorName = value
            End Set
        End Property

        Public Property VStoreID() As Integer
            Get
                Return _vStoreID
            End Get
            Set(ByVal value As Integer)
                _vStoreID = value
            End Set
        End Property

        Public Property VStoreName() As String
            Get
                Return _vStoreName
            End Get
            Set(ByVal value As String)
                _vStoreName = value
            End Set
        End Property

        Public Property PayByAgreedCost() As Boolean
            Get
                Return _payByAgreedCost
            End Get
            Set(ByVal value As Boolean)
                _payByAgreedCost = value
            End Set
        End Property

        Public Property VState() As String
            Get
                Return _vState
            End Get
            Set(ByVal value As String)
                _vState = value
            End Set
        End Property

        Public Property VZoneID() As Integer
            Get
                Return _vZoneID
            End Get
            Set(ByVal value As Integer)
                _vZoneID = value
            End Set
        End Property
#End Region

#Region "Constructors"
        ''' <summary>
        ''' Constructor that populates the object using the data from the result set.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef results As SqlDataReader)
            logger.Debug("New entry with results")
            If (Not results.IsDBNull(results.GetOrdinal("OrderHeader_ID"))) Then
                _orderHeaderID = results.GetInt32(results.GetOrdinal("OrderHeader_ID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceDate"))) Then
                _invoiceDate = results.GetDateTime(results.GetOrdinal("InvoiceDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceNumber"))) Then
                _invoiceNum = results.GetString(results.GetOrdinal("InvoiceNumber"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VStoreName"))) Then
                _vStoreName = results.GetString(results.GetOrdinal("VStoreName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Vendor_ID"))) Then
                _vendorID = results.GetInt32(results.GetOrdinal("Vendor_ID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VendorName"))) Then
                _vendorName = results.GetString(results.GetOrdinal("VendorName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Vendor_Key"))) Then
                _vendorKey = results.GetString(results.GetOrdinal("Vendor_Key"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceSubteam_No"))) Then
                _invoiceSubteamNo = results.GetInt32(results.GetOrdinal("InvoiceSubteam_No"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TotalInvoiceCost"))) Then
                _invoiceTotal = results.GetDecimal(results.GetOrdinal("TotalInvoiceCost"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TotalOrderCost"))) Then
                _poTotal = results.GetDecimal(results.GetOrdinal("TotalOrderCost"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TotalCostDiff"))) Then
                _costDiff = results.GetDecimal(results.GetOrdinal("TotalCostDiff"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VStoreID"))) Then
                _vStoreID = results.GetInt32(results.GetOrdinal("VStoreID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PayByAgreedCost"))) Then
                _payByAgreedCost = results.GetBoolean(results.GetOrdinal("PayByAgreedCost"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VState"))) Then
                _vState = results.GetString(results.GetOrdinal("VState"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VZoneID"))) Then
                _vZoneID = results.GetInt32(results.GetOrdinal("VZoneID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("DocumentDataExists"))) Then
                If results.GetInt32(results.GetOrdinal("DocumentDataExists")) = 1 Then
                    _docDataExists = True
                Else
                    _docDataExists = False
                End If
            End If

            If (Not results.IsDBNull(results.GetOrdinal("CreatedBy"))) Then
                _createdBy = results.GetInt32(results.GetOrdinal("CreatedBy"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ClosedBy"))) Then
                _closedBy = results.GetInt32(results.GetOrdinal("ClosedBy"))
            End If
            logger.Debug("New exit")
        End Sub
#End Region
    End Class
End Namespace
