Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.InvoiceThirdPartyFreight.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess

Namespace WholeFoods.IRMA.InvoiceThirdPartyFreight.BusinessLogic

    Public Enum ThirdPartyFreightInvoiceValidation
        Success
        Error_VendorRequired
        Error_VendorKeyNotUnique
        Error_VendorIdAndKeyMismatch
        Error_InvalidVendor
        Error_InvoiceDateRequired
        Error_InvoiceNumRequired
        Error_InvoiceNumPreviouslyUsed
        Error_InvoiceCostRequired
        Error_InvoiceFreightRequired
        Error_PONumRequired
        Warning_InvoiceCostOverThreshold
    End Enum

    Public Class ThirdPartyFreightInvoiceBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Dim daoConfig As ConfigurationDataDAO = New ConfigurationDataDAO

        Private _invoiceDate As Date = Nothing
        Private _invoiceNum As String
        Private _invoiceCost As Decimal = 0
        Private _poNum As Integer = -1
        Private _vendorID As Integer = -1
        Private _vendorKey As String
        Private _vendorName As String
        Private _uploadedDate As Date = Nothing
        Private _updateUserId As Integer = -1

#Region "Property Accessors"
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

        Public Property UploadedDate() As Date
            Get
                Return _uploadedDate
            End Get
            Set(ByVal value As Date)
                _uploadedDate = value
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
            If (Not results.IsDBNull(results.GetOrdinal("InvoiceCost"))) Then
                _invoiceCost = results.GetDecimal(results.GetOrdinal("InvoiceCost"))
            Else
                _invoiceCost = 0
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
            If (Not results.IsDBNull(results.GetOrdinal("UploadedDate"))) Then
                _uploadedDate = results.GetDateTime(results.GetOrdinal("UploadedDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("CompanyName"))) Then
                _vendorName = results.GetString(results.GetOrdinal("CompanyName"))
            Else
                _vendorName = Nothing
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
        Public Function ValidateData(ByVal editExisting As Boolean) As ThirdPartyFreightInvoiceValidation
            logger.Debug("ValidateData entry: editExisting=" + editExisting.ToString)
            Dim status As ThirdPartyFreightInvoiceValidation = ThirdPartyFreightInvoiceValidation.Success

            ' Vendor ID or Vendor Key must be entered
            If status = ThirdPartyFreightInvoiceValidation.Success AndAlso _
               _vendorID = -1 AndAlso (_vendorKey Is Nothing Or _vendorKey = "") Then
                status = ThirdPartyFreightInvoiceValidation.Error_VendorRequired
            End If

            ' If both vendor id and vendor key are entered, they must correspond to the same vendor
            If status = ThirdPartyFreightInvoiceValidation.Success AndAlso _
               _vendorID <> -1 AndAlso Not (_vendorKey Is Nothing Or _vendorKey = "") Then
                If Not ThirdPartyFreightDAO.CheckForVendorIdAndVendorKeyMatch(_vendorID, _vendorKey) Then
                    status = ThirdPartyFreightInvoiceValidation.Error_VendorIdAndKeyMismatch
                End If
            End If

            ' If only the vendor key was entered, verify it corresponds to just a single vendor and
            ' populate the vendor id for the db updates
            If status = ThirdPartyFreightInvoiceValidation.Success AndAlso _
               _vendorID = -1 AndAlso Not (_vendorKey Is Nothing Or _vendorKey = "") Then
                Dim vendorData As ArrayList = ThirdPartyFreightDAO.GetVendorCountByVendorKey(_vendorKey)
                Dim vendorCount As Integer = CInt(vendorData(0))
                If vendorCount <> 1 Then
                    status = ThirdPartyFreightInvoiceValidation.Error_VendorKeyNotUnique
                Else
                    _vendorID = CInt(vendorData(1))
                End If
            End If

            ' The Vendor_ID must be in the Vendor table and the Vendor.PS_Export_Vendor_ID cannot be null
            If status = ThirdPartyFreightInvoiceValidation.Success AndAlso _
               Not ThirdPartyFreightDAO.IsValidVendor(_vendorID) Then
                status = ThirdPartyFreightInvoiceValidation.Error_InvalidVendor
            End If

            ' Invoice date is required
            If status = ThirdPartyFreightInvoiceValidation.Success AndAlso _
               _invoiceDate = Nothing Then
                status = ThirdPartyFreightInvoiceValidation.Error_InvoiceDateRequired
            End If

            ' Invoice number is required
            If status = ThirdPartyFreightInvoiceValidation.Success AndAlso _
               _invoiceNum Is Nothing Or _invoiceNum = "" Then
                status = ThirdPartyFreightInvoiceValidation.Error_InvoiceNumRequired
            End If

            ' Invoice number cannot be the same as another vendor invoice from the same vendor
            If status = ThirdPartyFreightInvoiceValidation.Success AndAlso _
                ThirdPartyFreightDAO.ExistingVendorInvoiceNum(Me) Then
                status = ThirdPartyFreightInvoiceValidation.Error_InvoiceNumPreviouslyUsed
            End If

            ' Invoice cost is required for all invoices
            If status = ThirdPartyFreightInvoiceValidation.Success AndAlso _
               _invoiceCost < 0 Then
                status = ThirdPartyFreightInvoiceValidation.Error_InvoiceCostRequired
            End If

            ' If Invoice cost is over threshold, show warning
            If InstanceDataDAO.IsFlagActive("3rdPartyFreightInvoiceValidation") Then
                Dim nThreshold As Decimal
                nThreshold = CDec(daoConfig.GetConfigValue("3PartyFreightInvoiceValidationAmt"))
                If status = ThirdPartyFreightInvoiceValidation.Success AndAlso _
                   _invoiceCost >= nThreshold Then
                    status = ThirdPartyFreightInvoiceValidation.Warning_InvoiceCostOverThreshold
                End If
            End If


            logger.Debug("ValidateData exit: status=" + status.ToString)
            Return status
        End Function
#End Region

    End Class
End Namespace
