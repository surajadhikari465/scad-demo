Imports log4net
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Ordering.BusinessLogic
    Public Class AllSuspendedInvoiceBO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ' Purchase Order Data
        Private _orderHeaderID As Integer
        Private _closeDate As Date
        Private _vStoreName As String
        Private _subteamName As String
        Private _GLAccount As String
        Private _vendorID As Integer
        Private _vendorName As String
        Private _vendorPSExportID As String
        Private _vendorKey As String
        Private _invoiceNum As String
        Private _invoiceTotal As Decimal
        Private _invoiceTotalNoCharges As Decimal
        Private _invoiceFreight As Decimal
        Private _poTotal As Decimal
        Private _costDiff As Decimal
        Private _createDate As Date
        Private _invoiceDate As Date
        Private _sentDate As Date
        Private _costEffectiveDate As Date
        Private _creator As String
        Private _closer As String
        Private _matchedEInvoice As Boolean
        Private _transmissionType As String
        Private _notes As String
        Private _paymentTerms As String
        Private _payByAgreedCost As Boolean
        Private _paymentType As String
        Private _numOfDaysSuspended As Integer
        Private _beverageSubteam As Boolean
        Private _creditPO As Boolean
        Private _adjustedCost As Boolean
        Private _docTypeOtherOrNone As Boolean
        Private _CbW As Boolean
        Private _qtyMismatch As String
        Private _costNotByCASE As Boolean
        Private _orderUnitMismatch As Boolean
        Private _adminNotes As String
        Private _invoiceMatchingTotal As Decimal
        Private _resolutionCode As String
        Private _resolutionCodeId As String
        Private _orderType As Integer
        Private _eInvoiceID As Integer
        Private _createdBy As Integer
        Private _closedBy As Integer
        Private _status As String
        Private _inReviewUsername As String
        Private _inReviewFullname As String
        Private _dsdPO As Boolean
        Private _partialShipmentPO As Boolean
        Private _eInvoiceRequired As Boolean
        Private _totalRefused As Decimal
        Private _sourcePONumber As String

#Region "Property Accessors"
        Public Property InvoiceDate() As Date
            Get
                Return _invoiceDate
            End Get
            Set(ByVal value As Date)
                _invoiceDate = value
            End Set
        End Property
        Public Property ClosedDate() As Date
            Get
                Return _closeDate
            End Get
            Set(ByVal value As Date)
                _closeDate = value
            End Set
        End Property
        Public Property CreateDate() As Date
            Get
                Return _createDate
            End Get
            Set(ByVal value As Date)
                _createDate = value
            End Set
        End Property
        Public Property SentDate() As Date
            Get
                Return _sentDate
            End Get
            Set(ByVal value As Date)
                _sentDate = value
            End Set
        End Property
        Public Property CostEffectiveDate() As Date
            Get
                Return _costEffectiveDate
            End Get
            Set(ByVal value As Date)
                _costEffectiveDate = value
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
        Public Property GLAccount() As String
            Get
                Return _GLAccount
            End Get
            Set(ByVal value As String)
                _GLAccount = value
            End Set
        End Property
        Public Property SubTeamName() As String
            Get
                Return _subteamName
            End Get
            Set(ByVal value As String)
                _subteamName = value
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
        Public Property VendorPSExportID() As String
            Get
                Return _vendorPSExportID
            End Get
            Set(ByVal value As String)
                _vendorPSExportID = value
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
        Public Property InvoiceTotalNoCharges() As Decimal
            Get
                Return _invoiceTotalNoCharges
            End Get
            Set(ByVal value As Decimal)
                _invoiceTotalNoCharges = value
            End Set
        End Property
        Public Property InvoiceFreight() As Decimal
            Get
                Return _invoiceFreight
            End Get
            Set(ByVal value As Decimal)
                _invoiceFreight = value
            End Set
        End Property

        Public ReadOnly Property DocTypeOther() As String
            Get
                Return IIf(_docTypeOtherOrNone, "Y", "N")
            End Get
        End Property

        Public ReadOnly Property CreditPO() As String
            Get
                Return IIf(_creditPO, "Y", "N")
            End Get
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
        Public Property Creator() As String
            Get
                Return _creator
            End Get
            Set(ByVal value As String)
                _creator = value
            End Set
        End Property
        Public Property Closer() As String
            Get
                Return _closer
            End Get
            Set(ByVal value As String)
                _closer = value
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
        Public Property VStoreName() As String
            Get
                Return _vStoreName
            End Get
            Set(ByVal value As String)
                _vStoreName = value
            End Set
        End Property
        Public Property TransmissionType() As String
            Get
                Return _transmissionType
            End Get
            Set(ByVal value As String)
                _transmissionType = value
            End Set
        End Property

        Public Property Notes() As String
            Get
                Return _notes
            End Get
            Set(ByVal value As String)
                _notes = value
            End Set
        End Property

        Public Property PaymentTerms() As String
            Get
                Return _paymentTerms
            End Get
            Set(ByVal value As String)
                _paymentTerms = value
            End Set
        End Property
        Public ReadOnly Property MatchedEInvoice() As String
            Get
                Return IIf(_matchedEInvoice, "Y", "N")
            End Get
        End Property

        Public ReadOnly Property PayByAgreedCost() As String
            Get
                Return IIf(_payByAgreedCost, "Y", "N")
            End Get
        End Property

        Public ReadOnly Property PaymentType() As String
            Get
                Return _paymentType
            End Get
        End Property

        Public ReadOnly Property BeverageSubteam() As String
            Get
                Return IIf(_beverageSubteam, "Y", "N")
            End Get
        End Property

        Public Property NumberOfDaysSuspended() As Decimal
            Get
                Return _numOfDaysSuspended
            End Get
            Set(ByVal value As Decimal)
                _numOfDaysSuspended = value
            End Set
        End Property

        Public ReadOnly Property AdjustedCost() As String
            Get
                Return IIf(_adjustedCost, "Y", "N")
            End Get
        End Property

        Public ReadOnly Property CbW() As String
            Get
                Return IIf(_CbW, "Y", "N")
            End Get
        End Property

        Public ReadOnly Property QtyMismatch() As String
            Get
                Return _qtyMismatch
            End Get
        End Property
        Public ReadOnly Property CostNotByCase() As String
            Get
                Return IIf(_costNotByCASE, "Y", "N")
            End Get
        End Property
        Public ReadOnly Property OrderUnitMismatch() As String
            Get
                Return IIf(_orderUnitMismatch, "Y", "N")
            End Get
        End Property
        Public Property POAdminNotes() As String
            Get
                Return _adminNotes
            End Get
            Set(ByVal value As String)
                _adminNotes = value
            End Set
        End Property
        Public Property InvoiceMatchingTotal() As Decimal
            Get
                Return _invoiceMatchingTotal
            End Get
            Set(ByVal value As Decimal)
                _invoiceMatchingTotal = value
            End Set
        End Property
        Public Property ResolutionCode() As String
            Get
                Return _resolutionCode
            End Get
            Set(ByVal value As String)
                _resolutionCode = value
            End Set
        End Property

        Public Property ResolutionCodeId() As String
            Get
                Return _resolutionCodeId
            End Get
            Set(ByVal value As String)
                _resolutionCodeId = value
            End Set
        End Property

        Public Property OrderType() As Integer
            Get
                Return _orderType
            End Get
            Set(ByVal value As Integer)
                _orderType = value
            End Set
        End Property

        Public Property eInvoiceID() As Integer
            Get
                Return _eInvoiceID
            End Get
            Set(ByVal value As Integer)
                _eInvoiceID = value
            End Set
        End Property

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
                Return _closedBy
            End Get
            Set(ByVal value As Integer)
                _closedBy = value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                _status = value
            End Set
        End Property

        Public ReadOnly Property InReviewUsername As String
            Get
                Return _inReviewUsername
            End Get

        End Property


        Public ReadOnly Property InReviewFullname As String
            Get
                Return _inReviewFullname
            End Get

        End Property

        Public ReadOnly Property ReceivingDocument As String

            Get
                Return IIf(_dsdPO, "Y", "N")
            End Get

        End Property

        Public ReadOnly Property PartialShipment As String

            Get
                Return IIf(_partialShipmentPO, "Y", "N")
            End Get

        End Property

        Public ReadOnly Property EInvoiceRequired As String

            Get
                Return IIf(_eInvoiceRequired, "Y", "N")
            End Get

        End Property

        Public Property TotalRefused() As Decimal
            Get
                Return _totalRefused
            End Get
            Set(ByVal value As Decimal)
                _totalRefused = value
            End Set
        End Property
        Public Property SourcePONumber() As String
            Get
                Return _sourcePONumber
            End Get
            Set(ByVal value As String)
                _sourcePONumber = value
            End Set
        End Property

#End Region

#Region "Constructors"
        ''' <summary>
        ''' Constructor that populates the object using the data from the result set.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        ''' 


        Sub New()

        End Sub

        Sub New(ByVal OrderHeaderId As Integer)
            _orderHeaderID = OrderHeaderId
        End Sub
        Public Sub New(ByRef results As SqlDataReader)
            logger.Debug("New entry with results")
            If (Not results.IsDBNull(results.GetOrdinal("OrderHeader_ID"))) Then
                _orderHeaderID = results.GetInt32(results.GetOrdinal("OrderHeader_ID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("SourcePONumber"))) Then
                _sourcePONumber = results.GetString(results.GetOrdinal("SourcePONumber"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("CloseDate"))) Then
                _closeDate = results.GetDateTime(results.GetOrdinal("CloseDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VStoreName"))) Then
                _vStoreName = results.GetString(results.GetOrdinal("VStoreName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("SubTeamName"))) Then
                _subteamName = results.GetString(results.GetOrdinal("SubTeamName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("GLAccount"))) Then
                _GLAccount = results.GetString(results.GetOrdinal("GLAccount"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Vendor_ID"))) Then
                _vendorID = results.GetInt32(results.GetOrdinal("Vendor_ID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VendorName"))) Then
                _vendorName = results.GetString(results.GetOrdinal("VendorName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VendorPSExportID"))) Then
                _vendorPSExportID = results.GetString(results.GetOrdinal("VendorPSExportID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Vendor_Key"))) Then
                _vendorKey = results.GetString(results.GetOrdinal("Vendor_Key"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceNumber"))) Then
                _invoiceNum = results.GetString(results.GetOrdinal("InvoiceNumber"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TotalInvoiceCost"))) Then
                _invoiceTotal = results.GetDecimal(results.GetOrdinal("TotalInvoiceCost"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TotalInvoiceCostNoCharges"))) Then
                _invoiceTotalNoCharges = results.GetDecimal(results.GetOrdinal("TotalInvoiceCostNoCharges"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TotalInvoiceFreight"))) Then
                _invoiceFreight = results.GetDecimal(results.GetOrdinal("TotalInvoiceFreight"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TotalOrderCost"))) Then
                _poTotal = results.GetDecimal(results.GetOrdinal("TotalOrderCost"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("OrderDate"))) Then
                _createDate = results.GetDateTime(results.GetOrdinal("OrderDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceDate"))) Then
                _invoiceDate = results.GetDateTime(results.GetOrdinal("InvoiceDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("SentDate"))) Then
                _sentDate = results.GetDateTime(results.GetOrdinal("SentDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("POCostEffectiveDate"))) Then
                _costEffectiveDate = results.GetDateTime(results.GetOrdinal("POCostEffectiveDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("POCreator"))) Then
                _creator = results.GetString(results.GetOrdinal("POCreator"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("POCloser"))) Then
                _closer = results.GetString(results.GetOrdinal("POCloser"))
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
            If (Not results.IsDBNull(results.GetOrdinal("MatchedeEInvoiceExists"))) Then
                _matchedEInvoice = results.GetInt32(results.GetOrdinal("MatchedeEInvoiceExists"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("POTransmissionType"))) Then
                _transmissionType = results.GetString(results.GetOrdinal("POTransmissionType"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Notes"))) Then
                _notes = results.GetString(results.GetOrdinal("Notes"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PaymentTerms"))) Then
                _paymentTerms = results.GetString(results.GetOrdinal("PaymentTerms"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PayByAgreedCost"))) Then
                _payByAgreedCost = results.GetBoolean(results.GetOrdinal("PayByAgreedCost"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PaymentType"))) Then
                _paymentType = results.GetString(results.GetOrdinal("PaymentType"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("NoOfDaysSuspended"))) Then
                _numOfDaysSuspended = results.GetInt32(results.GetOrdinal("NoOfDaysSuspended"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Beverage"))) Then
                _beverageSubteam = results.GetBoolean(results.GetOrdinal("Beverage"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("CreditPO"))) Then
                _creditPO = results.GetBoolean(results.GetOrdinal("CreditPO"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("AdjustedCost"))) Then
                If results.GetInt32(results.GetOrdinal("AdjustedCost")) = 1 Then
                    _adjustedCost = True
                Else
                    _adjustedCost = False
                End If
            End If
            If (Not results.IsDBNull(results.GetOrdinal("DocTypeOther"))) Then
                If results.GetInt32(results.GetOrdinal("DocTypeOther")) = 1 Then
                    _docTypeOtherOrNone = True
                Else
                    _docTypeOtherOrNone = False
                End If
            End If
            If (Not results.IsDBNull(results.GetOrdinal("CbW"))) Then
                If results.GetInt32(results.GetOrdinal("CbW")) = 1 Then
                    _CbW = True
                Else
                    _CbW = False
                End If
            End If
            If (Not results.IsDBNull(results.GetOrdinal("QtyMismatch"))) Then
                _qtyMismatch = results.GetString(results.GetOrdinal("QtyMismatch"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("CostNotByCase"))) Then
                If results.GetInt32(results.GetOrdinal("CostNotByCase")) = 1 Then
                    _costNotByCASE = True
                Else
                    _costNotByCASE = False
                End If
            End If
            If (Not results.IsDBNull(results.GetOrdinal("OrderUnitMismatch"))) Then
                If results.GetInt32(results.GetOrdinal("OrderUnitMismatch")) = 1 Then
                    _orderUnitMismatch = True
                Else
                    _orderUnitMismatch = False
                End If
            End If

            If (Not results.IsDBNull(results.GetOrdinal("POAdminNotes"))) Then
                _adminNotes = results.GetString(results.GetOrdinal("POAdminNotes"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Charges")) And Not results.IsDBNull(results.GetOrdinal("TotalInvoiceCost"))) Then
                _invoiceMatchingTotal = results.GetDecimal(results.GetOrdinal("TotalInvoiceCostNoCharges")) - results.GetDecimal(results.GetOrdinal("Charges"))
            End If

            _costDiff = _invoiceMatchingTotal - _poTotal

            If (Not results.IsDBNull(results.GetOrdinal("ResolutionCode"))) Then
                _resolutionCode = results.GetString(results.GetOrdinal("ResolutionCode"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ResolutionCodeId"))) Then
                _resolutionCodeId = results.GetInt32(results.GetOrdinal("ResolutionCodeId")).ToString
            End If

            If (Not results.IsDBNull(results.GetOrdinal("OrderType_ID"))) Then
                _orderType = results.GetInt32(results.GetOrdinal("OrderType_ID"))
            End If


            If (Not results.IsDBNull(results.GetOrdinal("eInvoice_ID"))) Then
                _eInvoiceID = results.GetInt32(results.GetOrdinal("eInvoice_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("CreatedBy"))) Then
                _createdBy = results.GetInt32(results.GetOrdinal("CreatedBy"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ClosedBy"))) Then
                _closedBy = results.GetInt32(results.GetOrdinal("ClosedBy"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("Status"))) Then
                _status = results.GetString(results.GetOrdinal("Status"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("InReviewUsername"))) Then
                _inReviewUsername = results.GetString(results.GetOrdinal("InReviewUsername"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("InReviewFullname"))) Then
                _inReviewFullname = results.GetString(results.GetOrdinal("InReviewFullname"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ReceivingDocument"))) Then
                _dsdPO = results.GetBoolean(results.GetOrdinal("ReceivingDocument"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PartialShipment"))) Then
                _partialShipmentPO = results.GetBoolean(results.GetOrdinal("PartialShipment"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("EinvoiceRequired"))) Then
                _eInvoiceRequired = results.GetBoolean(results.GetOrdinal("EinvoiceRequired"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("TotalRefused"))) Then
                _totalRefused = results.GetDecimal(results.GetOrdinal("TotalRefused"))
            End If

            logger.Debug("New exit")
        End Sub
#End Region
    End Class
End Namespace
