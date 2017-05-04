Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.InvoiceControlGroup.DataAccess

Namespace WholeFoods.IRMA.InvoiceControlGroup.BusinessLogic
    Public Enum ControlGroupInvoiceStatus
        InvoiceSuccess
        InvoiceError
        InvoiceWarning
    End Enum

    Public Enum ControlGroupInvoiceType
        Vendor
        ThirdPartyFreight
    End Enum

    Public Enum ControlGroupInvoiceValidation
        Success
        Error_VendorRequired
        Error_VendorKeyNotUnique
        Error_VendorIdAndKeyMismatch
        Error_InvoiceDateRequired
        Error_InvoiceNumRequired
        Error_InvoiceCostRequired
        Error_InvoiceFreightRequired
        Error_PONumRequired
        Error_DupInvoiceInControlGroup
        Warning_DupInvoiceInOpenControlGroup
    End Enum

    Public Class ControlGroupInvoiceBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _invoiceType As ControlGroupInvoiceType
        Private _invoiceDate As Date = Nothing
        Private _invoiceNum As String
        Private _invoiceCost As Decimal = 0
        Private _invoiceFreight As Decimal = 0
        Private _invoiceTotal As Decimal = 0
        Private _poNum As Integer = -1
        Private _vendorID As Integer = -1
        Private _vendorKey As String
        Private _vendorName As String
        Private _creditInv As Boolean = False
        Private _invoiceStatus As ControlGroupInvoiceStatus
        Private _invoiceStatusMsg As String
        Private _updateUserId As Integer = -1

#Region "Property Accessors"
        Public Property InvoiceType() As ControlGroupInvoiceType
            Get
                Return _invoiceType
            End Get
            Set(ByVal value As ControlGroupInvoiceType)
                _invoiceType = value
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

        Public Property InvoiceCost() As Decimal
            Get
                Return _invoiceCost
            End Get
            Set(ByVal value As Decimal)
                _invoiceCost = value
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

        Public Property InvoiceTotal() As Decimal
            Get
                Return _invoiceTotal
            End Get
            Set(ByVal value As Decimal)
                _invoiceTotal = value
            End Set
        End Property

        Public Property PONum() As Integer
            Get
                Return _poNum
            End Get
            Set(ByVal value As Integer)
                _poNum = value
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

        Public Property VendorName() As String
            Get
                Return _vendorName
            End Get
            Set(ByVal value As String)
                _vendorName = value
            End Set
        End Property

        Public Property CreditInv() As Boolean
            Get
                Return _creditInv
            End Get
            Set(ByVal value As Boolean)
                _creditInv = value
            End Set
        End Property

        Public Property InvoiceStatus() As ControlGroupInvoiceStatus
            Get
                Return _invoiceStatus
            End Get
            Set(ByVal value As ControlGroupInvoiceStatus)
                _invoiceStatus = value
            End Set
        End Property

        Public Property InvoiceStatusMsg() As String
            Get
                Return _invoiceStatusMsg
            End Get
            Set(ByVal value As String)
                _invoiceStatusMsg = value
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
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceType"))) Then
                Select Case results.GetInt32(results.GetOrdinal("InvoiceType"))
                    Case 1
                        _invoiceType = ControlGroupInvoiceType.Vendor
                    Case 2
                        _invoiceType = ControlGroupInvoiceType.ThirdPartyFreight
                End Select
            Else
                _invoiceType = ControlGroupInvoiceType.Vendor
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Return_Order"))) Then
                _creditInv = results.GetBoolean(results.GetOrdinal("Return_Order"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceCost"))) Then
                _invoiceCost = results.GetDecimal(results.GetOrdinal("InvoiceCost"))
            Else
                _invoiceCost = 0
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceFreight"))) Then
                _invoiceFreight = results.GetDecimal(results.GetOrdinal("InvoiceFreight"))
            Else
                _invoiceFreight = 0
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceTotal"))) Then
                _invoiceTotal = results.GetDecimal(results.GetOrdinal("InvoiceTotal"))
            Else
                _invoiceTotal = _invoiceCost + _invoiceFreight
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceDate"))) Then
                _invoiceDate = results.GetDateTime(results.GetOrdinal("InvoiceDate"))
            Else
                _invoiceDate = Nothing
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceNumber"))) Then
                _invoiceNum = results.GetString(results.GetOrdinal("InvoiceNumber"))
            Else
                _invoiceNum = Nothing
            End If
            If (Not results.IsDBNull(results.GetOrdinal("OrderHeader_ID"))) Then
                _poNum = results.GetInt32(results.GetOrdinal("OrderHeader_ID"))
            Else
                _poNum = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Vendor_ID"))) Then
                _vendorID = results.GetInt32(results.GetOrdinal("Vendor_ID"))
            Else
                _vendorID = -1
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Vendor_Key"))) Then
                _vendorKey = results.GetString(results.GetOrdinal("Vendor_Key"))
            Else
                _vendorKey = Nothing
            End If
            If (Not results.IsDBNull(results.GetOrdinal("CompanyName"))) Then
                _vendorName = results.GetString(results.GetOrdinal("CompanyName"))
            Else
                _vendorName = Nothing
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ValidationCodeType"))) Then
                Select Case results.GetInt32(results.GetOrdinal("ValidationCodeType"))
                    Case 1
                        _invoiceStatus = ControlGroupInvoiceStatus.InvoiceSuccess
                    Case 2
                        _invoiceStatus = ControlGroupInvoiceStatus.InvoiceError
                    Case 3
                        _invoiceStatus = ControlGroupInvoiceStatus.InvoiceWarning
                End Select
            Else
                _invoiceStatus = Nothing
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ValidationDescription"))) Then
                _invoiceStatusMsg = results.GetString(results.GetOrdinal("ValidationDescription"))
            Else
                _invoiceStatusMsg = Nothing
            End If
            logger.Debug("New exit")
        End Sub

        ''' <summary>
        ''' Constructor that populates the object using the data from a selected row in the UltraGrid.
        ''' </summary>
        ''' <param name="currentRow"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef currentRow As UltraGridRow)
            logger.Debug("New entry with selected row")
            If currentRow.Cells("InvoiceType").Value IsNot Nothing AndAlso currentRow.Cells("InvoiceType").Value IsNot DBNull.Value Then
                _invoiceType = CType(currentRow.Cells("InvoiceType").Value, ControlGroupInvoiceType)
            End If
            If currentRow.Cells("InvoiceDate").Value IsNot Nothing AndAlso currentRow.Cells("InvoiceDate").Value IsNot DBNull.Value Then
                _invoiceDate = CDate(currentRow.Cells("InvoiceDate").Value)
            End If
            If currentRow.Cells("InvoiceNum").Value IsNot Nothing AndAlso currentRow.Cells("InvoiceNum").Value IsNot DBNull.Value Then
                _invoiceNum = currentRow.Cells("InvoiceNum").Value.ToString()
            End If
            If currentRow.Cells("InvoiceCost").Value IsNot Nothing AndAlso currentRow.Cells("InvoiceCost").Value IsNot DBNull.Value Then
                _invoiceCost = CDec(currentRow.Cells("InvoiceCost").Value)
            End If
            If currentRow.Cells("InvoiceFreight").Value IsNot Nothing AndAlso currentRow.Cells("InvoiceFreight").Value IsNot DBNull.Value Then
                _invoiceFreight = CDec(currentRow.Cells("InvoiceFreight").Value)
            End If
            If currentRow.Cells("InvoiceTotal").Value IsNot Nothing AndAlso currentRow.Cells("InvoiceTotal").Value IsNot DBNull.Value Then
                _invoiceTotal = CDec(currentRow.Cells("InvoiceTotal").Value)
            End If
            If currentRow.Cells("PONum").Value IsNot Nothing AndAlso currentRow.Cells("PONum").Value IsNot DBNull.Value Then
                _poNum = CInt(currentRow.Cells("PONum").Value)
            End If
            If currentRow.Cells("VendorID").Value IsNot Nothing AndAlso currentRow.Cells("VendorID").Value IsNot DBNull.Value Then
                _vendorID = CInt(currentRow.Cells("VendorID").Value)
            End If
            If currentRow.Cells("VendorKey").Value IsNot Nothing AndAlso currentRow.Cells("VendorKey").Value IsNot DBNull.Value Then
                _vendorKey = currentRow.Cells("VendorKey").Value.ToString()
            End If
            If currentRow.Cells("VendorName").Value IsNot Nothing AndAlso currentRow.Cells("VendorName").Value IsNot DBNull.Value Then
                _vendorName = currentRow.Cells("VendorName").Value.ToString()
            End If
            If currentRow.Cells("CreditInv").Value IsNot Nothing AndAlso currentRow.Cells("CreditInv").Value IsNot DBNull.Value Then
                _creditInv = CType(currentRow.Cells("CreditInv").Value, Boolean)
            End If
            logger.Debug("New exit")
        End Sub
#End Region

#Region "Business Methods"
        ''' <summary>
        ''' Validate the data entry values to verify that all required fields are entered
        ''' and that all data passes the business rules.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateData(ByVal editExisting As Boolean, ByVal controlGroupID As Integer) As ControlGroupInvoiceValidation
            logger.Debug("ValidateData entry: editExisting=" + editExisting.ToString + ", controlGroupID=" + controlGroupID.ToString)
            Dim status As ControlGroupInvoiceValidation = ControlGroupInvoiceValidation.Success

            ' Vendor ID or Vendor Key must be entered
            If status = ControlGroupInvoiceValidation.Success AndAlso _
               _vendorID = -1 AndAlso (_vendorKey Is Nothing Or _vendorKey = "") Then
                status = ControlGroupInvoiceValidation.Error_VendorRequired
            End If

            ' If both vendor id and vendor key are entered, they must correspond to the same vendor
            If status = ControlGroupInvoiceValidation.Success AndAlso _
               _vendorID <> -1 AndAlso Not (_vendorKey Is Nothing Or _vendorKey = "") Then
                If Not ControlGroupDAO.CheckForVendorIdAndVendorKeyMatch(_vendorID, _vendorKey) Then
                    status = ControlGroupInvoiceValidation.Error_VendorIdAndKeyMismatch
                End If
            End If

            ' If only the vendor key was entered, verify it corresponds to just a single vendor and
            ' populate the vendor id for the db updates
            If status = ControlGroupInvoiceValidation.Success AndAlso _
               _vendorID = -1 AndAlso Not (_vendorKey Is Nothing Or _vendorKey = "") Then
                Dim vendorData As ArrayList = ControlGroupDAO.GetVendorCountByVendorKey(_vendorKey)
                Dim vendorCount As Integer = CInt(vendorData(0))
                If vendorCount <> 1 Then
                    status = ControlGroupInvoiceValidation.Error_VendorKeyNotUnique
                Else
                    _vendorID = CInt(vendorData(1))
                End If
            End If

            ' Invoice date is required
            If status = ControlGroupInvoiceValidation.Success AndAlso _
               _invoiceDate = Nothing Then
                status = ControlGroupInvoiceValidation.Error_InvoiceDateRequired
            End If

            ' Invoice number is required
            If status = ControlGroupInvoiceValidation.Success AndAlso _
               _invoiceNum Is Nothing Or _invoiceNum = "" Then
                status = ControlGroupInvoiceValidation.Error_InvoiceNumRequired
            End If

            ' Invoice cost is required for all invoices
            If status = ControlGroupInvoiceValidation.Success AndAlso _
               _invoiceCost < 0 Then
                status = ControlGroupInvoiceValidation.Error_InvoiceCostRequired
            End If

            ' Invoice freight is required for vendor invoices
            If status = ControlGroupInvoiceValidation.Success AndAlso _
               _invoiceType = ControlGroupInvoiceType.Vendor AndAlso _invoiceFreight < 0 Then
                status = ControlGroupInvoiceValidation.Error_InvoiceFreightRequired
            End If

            ' PO Number is required
            If status = ControlGroupInvoiceValidation.Success AndAlso _
               _poNum = -1 Then
                status = ControlGroupInvoiceValidation.Error_PONumRequired
            End If

            ' Check to see if this invoice already exists in the current control group
            If status = ControlGroupInvoiceValidation.Success AndAlso _
                Not editExisting AndAlso _
                ControlGroupDAO.CheckInvoiceExistsInCurrentControlGroup(Me, controlGroupID) Then
                status = ControlGroupInvoiceValidation.Error_DupInvoiceInControlGroup
            End If

            ' Check to see if this invoice already exists in another open control group
            If status = ControlGroupInvoiceValidation.Success AndAlso _
                Not editExisting AndAlso _
                ControlGroupDAO.CheckInvoiceExistsInOpenControlGroup(Me, controlGroupID) Then
                status = ControlGroupInvoiceValidation.Warning_DupInvoiceInOpenControlGroup
            End If

            logger.Debug("ValidateData exit: status=" + status.ToString)
            Return status
        End Function
#End Region

    End Class
End Namespace
