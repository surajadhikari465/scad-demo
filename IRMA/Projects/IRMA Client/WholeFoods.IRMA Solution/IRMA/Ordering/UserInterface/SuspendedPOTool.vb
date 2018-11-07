Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Shared
Imports Infragistics.Win
Imports Infragistics.Win.Misc
Imports log4net
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Net.Mail
Imports System.Collections
Imports System.ComponentModel
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WFM.UserAuthentication
Imports System.Data
Imports System.Linq


Public Class SuspendedPOTool
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ' Business object that holds the search criteria being used by the user
    Private searchBO As OrderSearchBO

    Dim mrsStore As ADODB.Recordset

    Private mdt As DataTable
    Private mdv As DataView

    Private gridLayoutFilePath As String = String.Format("{0}\GridLayout\{1}\{2}\", Application.StartupPath, My.Application.Info.Version, My.Application.CurrentUserID)
    Private gridLayoutFileName As String = "UltraGridAllSuspendedLayout.xml"

    Dim msStores As String
    Dim msStoreList As String
    Dim msState As String
    Dim miZoneID As Integer

    Const sNotification_PayByInvoice As String = "The selected invoices will be paid according to the invoice amounts."

    Private dtResolutionCodes As New DataTable

    Private Property item As Object
    Public Property blnPOApprovedByLineItem As Boolean = False

    Private Delegate Sub PaymentAction()
    Private callingAction As PaymentAction = Nothing

    Dim changedRows As List(Of UltraGridRow) = New List(Of UltraGridRow)

    ''' <summary>
    ''' Formats the suspended invoices data grid.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDataGrids()
        logger.Debug("FormatDataGrid entry")
        Dim currentPos As Integer

        ' Format the grid - suspended invoices due to missing invoice data
        If UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns.Count > 0 Then
            currentPos = 0
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Status").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Status").Header.Caption = "Status"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Status").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Status").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Status").Header.Fixed = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Status").CellAppearance.BackColor = Color.LightGray

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCode").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCode").Header.Caption = "Resolution Code"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCode").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCode").Header.Fixed = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCode").CellAppearance.BackColor = Color.LightGray
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCode").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Resolutioncode").CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POAdminNotes").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POAdminNotes").Header.Caption = "PO Admin Notes"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POAdminNotes").Header.Fixed = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POAdminNotes").CellAppearance.BackColor = Color.LightGray
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POAdminNotes").MaxLength = 5000
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POAdminNotes").CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderHeaderID").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderHeaderID").Header.Caption = "PO #"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderHeaderID").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderHeaderID").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderHeaderID").Header.Fixed = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderHeaderID").CellAppearance.BackColor = Color.LightGray

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SourcePONumber").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SourcePONumber").Header.Caption = "Source PO #"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SourcePONumber").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SourcePONumber").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SourcePONumber").Header.Fixed = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SourcePONumber").CellAppearance.BackColor = Color.LightGray
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SourcePONumber").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ClosedDate").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ClosedDate").Header.Caption = "Closed Date"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ClosedDate").Header.ToolTipText = "Closed Date"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ClosedDate").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ClosedDate").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ClosedDate").CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorName").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorName").Header.Caption = "Vendor Name"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorName").Header.ToolTipText = "Vendor Name"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorName").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorName").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceNum").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceNum").Header.Caption = "Invoice #"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceNum").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceNum").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SubTeamName").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SubTeamName").Header.Caption = "Subteam"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SubTeamName").Header.ToolTipText = "Subteam Name"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SubTeamName").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SubTeamName").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VStoreName").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VStoreName").Header.Caption = "Store"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VStoreName").Header.ToolTipText = "Store Name"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VStoreName").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VStoreName").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("NumberOfDaysSuspended").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("NumberOfDaysSuspended").Header.Caption = "Days Suspended"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("NumberOfDaysSuspended").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("NumberOfDaysSuspended").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PaymentTerms").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PaymentTerms").Header.Caption = "Payment Terms"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PaymentTerms").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PaymentTerms").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotal").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotal").Header.Caption = "Invoice Total"
            'UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotal").Format = "###,###,##0.00"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotal").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotal").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotal").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceMatchingTotal").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceMatchingTotal").Header.Caption = "Invoice Matching Total"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceMatchingTotal").Format = "###,###,##0.00"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceMatchingTotal").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceMatchingTotal").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceMatchingTotal").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POTotal").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POTotal").Header.Caption = "PO Total"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POTotal").Format = "###,###,##0.00"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POTotal").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POTotal").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("POTotal").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostDiff").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostDiff").Header.Caption = "Difference"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostDiff").Format = "###,###,##0.00"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostDiff").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostDiff").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostDiff").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceFreight").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceFreight").Header.Caption = "Invoice Freight"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceFreight").Format = "###,###,##0.00"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceFreight").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceFreight").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceFreight").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceFreight").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("AdjustedCost").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("AdjustedCost").Header.Caption = "Adjusted Cost"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("AdjustedCost").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("AdjustedCost").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("QtyMismatch").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("QtyMismatch").Header.Caption = "EInv Qty Mismatch"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("QtyMismatch").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("QtyMismatch").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderUnitMismatch").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderUnitMismatch").Header.Caption = "IRMA Order Unit Mismatch"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderUnitMismatch").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderUnitMismatch").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CreditPO").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CreditPO").Header.Caption = "Credit PO"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CreditPO").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CreditPO").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PaymentType").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PaymentType").Header.Caption = "Vendor Type"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PaymentType").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PaymentType").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CreateDate").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CreateDate").Header.Caption = "PO Create Date"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CreateDate").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CreateDate").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SentDate").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SentDate").Header.Caption = "Sent Date"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SentDate").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("SentDate").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostEffectiveDate").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostEffectiveDate").Header.Caption = "PO Cost Effective Date"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostEffectiveDate").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostEffectiveDate").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("DocTypeOther").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("DocTypeOther").Header.Caption = "Doc Type is Other/None"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("DocTypeOther").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("DocTypeOther").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("GLAccount").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("GLAccount").Header.Caption = "GL Account"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("GLAccount").Header.ToolTipText = "General Ledger Account"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("GLAccount").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("GLAccount").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("GLAccount").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorID").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorID").Header.Caption = "Vendor ID"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorID").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorID").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorID").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorPSExportID").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorPSExportID").Header.Caption = "Vendor PS Export#"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorPSExportID").Header.ToolTipText = "Vendor PS Export Number"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorPSExportID").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorPSExportID").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorPSExportID").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorKey").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorKey").Header.Caption = "Vendor Key"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorKey").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorKey").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("VendorKey").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PayByAgreedCost").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PayByAgreedCost").Header.Caption = "Pay Agreed Cost"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PayByAgreedCost").Hidden = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PayByAgreedCost").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PayByAgreedCost").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceDate").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceDate").Header.Caption = "Invoice Date"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceDate").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceDate").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceDate").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Creator").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Creator").Header.Caption = "PO Creator"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Creator").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Creator").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Creator").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("MatchedEInvoice").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("MatchedEInvoice").Header.Caption = "EInvoice Matched to PO"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("MatchedEInvoice").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("MatchedEInvoice").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("MatchedEInvoice").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TransmissionType").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TransmissionType").Header.Caption = "PO Transmission Type"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TransmissionType").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TransmissionType").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TransmissionType").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Notes").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Notes").Header.Caption = "PO Notes"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Notes").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Notes").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Notes").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("BeverageSubteam").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("BeverageSubteam").Header.Caption = "Beverage Subteam"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("BeverageSubteam").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("BeverageSubteam").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("BeverageSubteam").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CbW").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CbW").Header.Caption = "CbW"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CbW").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CbW").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Cbw").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostNotByCase").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostNotByCase").Header.Caption = "Cost not by Case"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostNotByCase").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostNotByCase").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CostNotByCase").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCodeId").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCodeId").Header.Caption = "Resolution Code Id"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCodeId").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InReviewUsername").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InReviewUsername").Header.Caption = "InReviewUsername"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InReviewUsername").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InReviewFullname").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InReviewFullname").Header.Caption = "InReviewFullname"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InReviewFullname").Hidden = True

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotalNoCharges").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotalNoCharges").Header.Caption = "Invoice Total No Charges"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotalNoCharges").Format = "###,###,##0.00"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotalNoCharges").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotalNoCharges").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("InvoiceTotalNoCharges").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ReceivingDocument").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ReceivingDocument").Header.Caption = "Receiving Document"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ReceivingDocument").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ReceivingDocument").CellClickAction = CellClickAction.RowSelect

            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PartialShipment").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PartialShipment").Header.Caption = "Partial Shipment"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PartialShipment").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("PartialShipment").CellClickAction = CellClickAction.RowSelect


            currentPos = currentPos + 1

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TotalRefused").Header.VisiblePosition = currentPos
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TotalRefused").Header.Caption = "Total Refused Amount"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TotalRefused").Format = "###,###,##0.00"
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TotalRefused").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TotalRefused").CellActivation = Activation.ActivateOnly
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TotalRefused").CellClickAction = CellClickAction.RowSelect
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("TotalRefused").Hidden = True

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("OrderType").Hidden = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("EInvoiceID").Hidden = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("CreatedBy").Hidden = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ClosedBy").Hidden = True
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Closer").Hidden = True

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("EInvoiceRequired").Hidden = True

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("Closer").CellActivation = Activation.NoEdit
            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Override.MaxSelectedRows = 200

            ' Default the first row to selected
            If UltraGrid_AllSuspended.Rows.Count > 0 Then
                UltraGrid_AllSuspended.Rows(0).Selected = True
            End If
        End If

        If UltraGrid_AllSuspended.Rows.Count > 0 Then
            With UltraGrid_AllSuspended.DisplayLayout.Bands(0)
                .Columns("OrderHeaderID").CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
                .Columns("OrderHeaderID").CellAppearance.ForeColor = Color.Blue
                .Columns("OrderHeaderID").CellAppearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True
                .Columns("OrderHeaderID").CellAppearance.Cursor = Cursors.Hand
                .Columns("OrderHeaderID").Tag = "[Left Click] to view Order information.  [Right Click] to view Line Item information."

                .Columns("InvoiceNum").CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
                .Columns("InvoiceNum").CellAppearance.ForeColor = Color.Blue
                .Columns("InvoiceNum").CellAppearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True
                .Columns("InvoiceNum").CellAppearance.Cursor = Cursors.Hand
                .Columns("InvoiceNum").Tag = "[Left Click] to view Invoice information."

                .Columns("VendorName").CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
                .Columns("VendorName").CellAppearance.ForeColor = Color.Blue
                .Columns("VendorName").CellAppearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True
                .Columns("VendorName").CellAppearance.Cursor = Cursors.Hand
                .Columns("VendorName").Tag = "[Left Click] to view Vendor information."
            End With
        End If

        ' After the grid has been setup, load the user's layout file.  This will restore the user's Column Chooser selections.
        LoadDisplayLayout()

        ' After the user's layout file has been loaded, load resolution codes into grid combo boxes if there are rows returned
        If UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns.Count > 0 Then
            Dim valList As New ValueList

            ComboBox_ResolutionCodesCopy.Items.Clear()
            UltraGrid_AllSuspended.DisplayLayout.ValueLists.Clear()

            For Each dr As DataRow In dtResolutionCodes.Rows
                valList.ValueListItems.Add(dr("ReasonCodeDetailId").ToString, dr("ReasonCodeDesc").ToString)
                ComboBox_ResolutionCodesCopy.Items.Add(New VB6.ListBoxItem(dr("ReasonCodeDesc"), dr("ReasonCodeDetailId")))
            Next

            UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns("ResolutionCode").ValueList = valList
            ComboBox_ResolutionCodesCopy.SelectedIndex = -1
        End If

    End Sub

    ''' <summary>
    ''' Refresh the suspended invoices grid, using the search criteria currently defined in the
    ''' searchBO object.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RefreshData()
        logger.Debug("RefreshData()")

        Dim bRemediatedPOOnly As Boolean = False
        Dim susInvoices As ArrayList

        'remove the InvoiceMatchingTotal column which will be readded if a row is returned by the search
        UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns.ClearUnbound()

        If cmbState.SelectedIndex = -1 Then
            msState = ""
        Else
            msState = cmbState.Text
        End If

        If cmbZones.SelectedIndex = -1 Then
            miZoneID = 0
        Else
            miZoneID = (VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
        End If

        ' Refresh the grid with the search results.
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        ' Perform the search.
        bRemediatedPOOnly = CheckBox_RemediatedSuspensions.Checked

        If (bRemediatedPOOnly = True) Then
            ' Do remediated search, passing selected res-code or 0 (either causes remediated search).
            searchBO.ResolutionCodeID = IIf(ComboBox_ResolutionCodes.SelectedIndex = -1, 0, ComboVal(ComboBox_ResolutionCodes))
        Else
            ' Pass -1 to indicate we're doing a suspended search rather than remediated search.
            searchBO.ResolutionCodeID = -1
        End If

        ' Perform the search.
        susInvoices = OrderingDAO.SearchForSuspendedOrders(searchBO, msStoreList, msState, miZoneID)
        If susInvoices.Count > 0 Then
            UltraGrid_AllSuspended.DataSource = susInvoices(0)
        End If

        ApplyGridFilter(CType(UltraToolbarsManager1.Tools("ComboBox_GridFilter"), Infragistics.Win.UltraWinToolbars.ComboBoxTool).Text)

        FormatDataGrids()

        UltraGrid_AllSuspended.UpdateData()

        changedRows.Clear()

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    ''' <summary>
    ''' Populates DataTables with SuspendedPO Resolution Codes to be used as needed.
    ''' </summary>
    ''' <remarks>This will only retrieve all resolution codes from DB and populate a separate DataTable for only active codes</remarks>
    Private Sub GetResolutionCodes()
        logger.Debug("GetResolutionCodes() entry")

        Try
            ' Retrieve all active codes from DB and load into a single DataTable
            dtResolutionCodes = InvoiceMatchingDAO.GetResolutionCodes("SP", 0)

        Catch ex As Exception
            logger.Debug(String.Format("Error getting Resolution Codes from Database and inserting them into DataTables: {0}", ex.Message))

        End Try

        logger.Debug("GetResolutionCodes() exit")
    End Sub

    ''' <summary>
    ''' Search for suspended orders that match the criteria input by the user.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_OrderSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OrderSearch.Click
        logger.Debug("Button_OrderSearch_Click entry")

        Dim result As DialogResult = Nothing
        Dim originalText As String = Button_OrderSearch.Text

        ' Save the user's display layout before proceeding (unless the grid is empty).  This will capture the user's Column Chooser preferences.
        If UltraGrid_AllSuspended.Rows.Count > 0 Then
            SaveDisplayLayout()
        End If

        If changedRows.Count > 0 Then
            result = MessageBox.Show("Unsaved changes will be lost if you refresh this data. Do you wish to save now?", "Warning - Unsaved Data", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
            If result = DialogResult.Yes Then
                SaveData()
            End If
        End If

        Try
            Button_OrderSearch.Text = "Searching..."
            Button_OrderSearch.Enabled = False
            changedRows.Clear()

            ' Prepare Additional Store filters
            Dim sStores As String
            Dim sState As String
            Dim lZone_ID As Integer
            Dim sFilter As String

            sStores = String.Empty
            sState = String.Empty
            sFilter = String.Empty

            Select Case True
                Case _optSelection_0.Checked
                    sStores = ComboValue(cmbStore)

                Case _optSelection_1.Checked
                    If cmbZones.SelectedIndex = -1 Then
                        MsgBox(ResourcesItemHosting.GetString("SelectZone"), MsgBoxStyle.Critical, Me.Text)
                        Exit Sub
                    Else
                        lZone_ID = CInt(ComboValue(cmbZones))
                    End If

                Case _optSelection_2.Checked
                    If cmbState.SelectedIndex = -1 Then
                        MsgBox(ResourcesItemHosting.GetString("SelectState"), MsgBoxStyle.Critical, Me.Text)
                        Exit Sub
                    Else
                        sState = cmbState.Text
                    End If
            End Select

            If Len(sStores) = 0 Then
                msStoreList = GetStoreListString(lZone_ID, sState, _optSelection_3.Checked, _optSelection_4.Checked, _optSelection_5.Checked)
            Else
                msStoreList = sStores
            End If

            ' Populate the search BO with the parameters entered by the user.
            searchBO = New OrderSearchBO()
            If (UltraNumericEditor_PONum.Value IsNot Nothing) AndAlso (UltraNumericEditor_PONum.Value IsNot DBNull.Value) Then
                searchBO.OrderHeaderID = CInt(UltraNumericEditor_PONum.Value)
            Else
                searchBO.OrderHeaderID = -1
            End If
            If (UltraNumericEditor_ControlGroupID.Value IsNot Nothing) AndAlso (UltraNumericEditor_ControlGroupID.Value IsNot DBNull.Value) Then
                searchBO.OrderInvoiceControlGroupID = CInt(UltraNumericEditor_ControlGroupID.Value)
            Else
                searchBO.OrderInvoiceControlGroupID = -1
            End If
            If (UltraNumericEditor_VendorID.Value IsNot Nothing) AndAlso (UltraNumericEditor_VendorID.Value IsNot DBNull.Value) Then
                searchBO.VendorID = CInt(UltraNumericEditor_VendorID.Value)
            Else
                searchBO.VendorID = -1
            End If
            If Trim(Text_VendorInvoice.Text).Length > 0 Then
                searchBO.InvoiceNumber = Trim(Text_VendorInvoice.Text)
            Else
                searchBO.InvoiceNumber = Nothing
            End If
            If Not DateTime_InvDateStart.Value Is Nothing AndAlso Not DateTime_InvDateStart.Value Is DBNull.Value Then
                searchBO.InvoiceDateStart = CDate(DateTime_InvDateStart.Value)
            Else
                searchBO.InvoiceDateStart = Date.MinValue
            End If
            If Not DateTime_InvDateEnd.Value Is Nothing AndAlso Not DateTime_InvDateEnd.Value Is DBNull.Value Then
                searchBO.InvoiceDateEnd = CDate(DateTime_InvDateEnd.Value)
            Else
                searchBO.InvoiceDateEnd = Date.MinValue
            End If
            If Not DateTime_OrderDateStart.Value Is Nothing AndAlso Not DateTime_OrderDateStart.Value Is DBNull.Value Then
                searchBO.OrderDateStart = CDate(DateTime_OrderDateStart.Value)
            Else
                searchBO.OrderDateStart = Date.MinValue
            End If
            If Not DateTime_OrderDateEnd.Value Is Nothing AndAlso Not DateTime_OrderDateEnd.Value Is DBNull.Value Then
                searchBO.OrderDateEnd = CDate(DateTime_OrderDateEnd.Value)
            Else
                searchBO.OrderDateEnd = Date.MinValue
            End If
            If Trim(TextBox_VIN.Text).Length > 0 Then
                searchBO.VIN = Trim(TextBox_VIN.Text)
            Else
                searchBO.VIN = Nothing
            End If
            If Trim(TextBox_Identifier.Text).Length > 0 Then
                searchBO.Identifier = Trim(TextBox_Identifier.Text)
            Else
                searchBO.Identifier = Nothing
            End If

            searchBO.EInvoiceOnly = CheckBox_eInvoice.Checked

            searchBO.VendorKey = TextBox_VendorKey.Text

            ' Validate the input data for required fields and business rules
            Select Case searchBO.ValidateSuspendedInvoiceSearchData()
                Case OrderSearchStatus.Error_InvoiceStartAndEndDateRequired
                    MsgBox("The " + Me.Label_InvDateStart.Text.Replace(":", "") + " or " + Me.Label_InvDateEnd.Text.Replace(":", "") + " was entered.  You must enter both dates or clear the single date to perform the search.", MsgBoxStyle.Critical, Me.Text)
                Case OrderSearchStatus.Error_InvoiceStartDateInFuture
                    MsgBox("The " + Me.Label_InvDateStart.Text.Replace(":", "") + " cannot be in the future.", MsgBoxStyle.Critical, Me.Text)
                Case OrderSearchStatus.Error_InvoiceEndDateInFuture
                    MsgBox("The " + Me.Label_InvDateEnd.Text.Replace(":", "") + " cannot be in the future.", MsgBoxStyle.Critical, Me.Text)
                Case OrderSearchStatus.Error_InvoiceEndDateAfterStartDate
                    MsgBox("The " + Me.Label_InvDateEnd.Text.Replace(":", "") + " is before the " + Me.Label_InvDateStart.Text.Replace(":", "") + ".  The end date must be the same as the start date or after the start date.", MsgBoxStyle.Critical, Me.Text)
                Case OrderSearchStatus.Error_OrderStartAndEndDateRequired
                    MsgBox("The " + Me.Label_OrderStartDate.Text.Replace(":", "") + " or " + Me.Label_OrderDateEnd.Text.Replace(":", "") + " was entered.  You must enter both dates or clear the single date to perform the search.", MsgBoxStyle.Critical, Me.Text)
                Case OrderSearchStatus.Error_OrderStartDateInFuture
                    MsgBox("The " + Me.Label_OrderStartDate.Text.Replace(":", "") + " cannot be in the future.", MsgBoxStyle.Critical, Me.Text)
                Case OrderSearchStatus.Error_OrderEndDateInFuture
                    MsgBox("The " + Me.Label_OrderDateEnd.Text.Replace(":", "") + " cannot be in the future.", MsgBoxStyle.Critical, Me.Text)
                Case OrderSearchStatus.Error_OrderEndDateAfterStartDate
                    MsgBox("The " + Me.Label_OrderDateEnd.Text.Replace(":", "") + " is before the " + Me.Label_OrderStartDate.Text.Replace(":", "") + ".  The end date must be the same as the start date or after the start date.", MsgBoxStyle.Critical, Me.Text)
                Case OrderSearchStatus.Success
                    ' The data is valid.  Perform the search.

                    'Clear all grids first
                    UltraGrid_AllSuspended.DataSource = Nothing

                    'Displays search result
                    SetApprovePermissions()
                    RefreshData()
            End Select

        Finally
            Button_OrderSearch.Text = "Search for Suspended Orders"
            Button_OrderSearch.Enabled = True

            If UltraGrid_AllSuspended.Rows.Count = 0 Then
                UltraNumericEditor_PONum.Focus()
            Else
                UltraGrid_AllSuspended.Focus()

                ' Enable Select All Button if Row Count is not 0.
                Button_SelectAll.Enabled = True

                ' Begin with the first row selected.
                UltraGrid_AllSuspended.Rows(0).Selected = True
            End If

        End Try

        logger.Debug("Button_OrderSearch_Click exit")
    End Sub


    '
    ''' <summary>
    ''' Search for an existing purchase order.  The OrderSearch screen used to perform the search 
    ''' is shared with the ordering code.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub Button_SearchOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SearchOrder.Click
        logger.Debug("Button_SearchOrder_Click entry")
        '-- Set glOrderHeaderID to none found before beginning the search
        glOrderHeaderID = 0

        '-- Open the search form
        Dim fSearch As New frmOrdersSearch
        fSearch.ShowDialog()
        fSearch.Close()
        fSearch.Dispose()

        '-- if the glOrderHeaderID is not zero, then something was found
        If glOrderHeaderID <> 0 Then
            Me.UltraNumericEditor_PONum.Value = glOrderHeaderID
        Else
            Me.UltraNumericEditor_PONum.Value = DBNull.Value
        End If
        logger.Debug("Button_SearchOrder_Click exit")
    End Sub

    ''' <summary>
    ''' Search for an existing vendor.  The Common vendor search screen is used to perform the search.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub Button_SearchVendor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SearchVendor.Click
        logger.Debug("Button_SearchVendor_Click entry")
        '-- Set glVendorID to none found before beginning the search
        glVendorID = 0
        '-- Set the search type
        giSearchType = iSearchVendorCompany

        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = "Vendor Search"
        fSearch.ShowDialog()
        fSearch.Close()
        fSearch.Dispose()

        '-- if the glVendorID is not zero, then something was found
        If glVendorID <> 0 Then
            Me.UltraNumericEditor_VendorID.Value = glVendorID
            Me.TextBox_VendorKey.Text = ""
        Else
            Me.UltraNumericEditor_VendorID.Value = DBNull.Value
            Me.TextBox_VendorKey.Text = ""
        End If

        Me.UltraNumericEditor_VendorID.Focus()
        Me.UltraNumericEditor_VendorID.SelectAll()

        logger.Debug("Button_SearchVendor_Click exit")
    End Sub

    Private Sub InvoiceMatching_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        logger.Debug("InvoiceMatching_Load Entry")

        CenterForm(Me)

        LoadComboData()

        InitApproveButtons()

        CheckBox_RemediatedSuspensions.Checked = False
        ComboBox_ResolutionCodes.Enabled = False

        ' Disable Refresh Cost Button until orders are selected
        Button_RefreshCost.Enabled = False
        ' Disable Select All Button until a search has been performed
        Button_SelectAll.Enabled = False

        ' Get Resolution Codes from Database and load them into a DataTable
        GetResolutionCodes()

        ' Load Search parameter combobox for Resolution Codes
        For Each dr As DataRow In dtResolutionCodes.Rows
            ComboBox_ResolutionCodes.Items.Add(New VB6.ListBoxItem(dr("ReasonCodeDesc"), dr("ReasonCodeDetailId")))
        Next
        ComboBox_ResolutionCodes.SelectedIndex = -1

        'If the user is restricted to a single store, lock the combo box to that store.
        'MD 7/31/2009: WI 10609, also disable the options to select other stores or location.
        If glStore_Limit > 0 Then
            DisableOptions()
            _optSelection_0.Checked = True
            Call SetActive(fraStores, False)
            Call SetCombo(cmbStore, glStore_Limit)
            Call SetActive(cmbStore, False)
        End If

        Call SetActive(cmbZones, False)
        Call SetActive(cmbState, False)

        logger.Debug("InvoiceMatching_Load Exit")

        UltraPanel_ResolutionCodes.Hide()
        UltraPanel_ResolutionCodes.SendToBack()

        UltraPanel_SavingChanges.Hide()
        UltraPanel_SavingChanges.SendToBack()

        CType(UltraToolbarsManager1.Tools("ComboBox_GridFilter"), Infragistics.Win.UltraWinToolbars.ComboBoxTool).SelectedIndex = 0

    End Sub

    Private Sub LoadComboData()

        Dim sPrevZone As String
        Dim sPrevState As String
        Dim iStoreNo As Integer
        Dim sStoreName As String
        Dim iZoneID As Integer
        Dim sZoneName As String
        Dim sState As String

        sPrevZone = String.Empty
        sPrevState = String.Empty

        logger.Debug("LoadComboData Entry")

        Try
            logger.Debug("BEGIN: EXEC GetStoresAndDist")
            gRSRecordset = SQLOpenRecordSet("EXEC GetStoresAndDist", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            CreateEmptyADORS_FromDAO(gRSRecordset, mrsStore, bAllowNulls:=False)
            mrsStore.Open()
            While Not gRSRecordset.EOF
                iStoreNo = CType(gRSRecordset.Fields("Store_No").Value, Integer)
                sStoreName = CType(gRSRecordset.Fields("Store_Name").Value, String)

                CopyDAORecordToADORecord(gRSRecordset, mrsStore, bAllowNulls:=False)
                cmbStore.Items.Add(New VB6.ListBoxItem(sStoreName, iStoreNo))

                gRSRecordset.MoveNext()
            End While
            logger.Debug("END: EXEC GetStoresAndDist")

        Catch ex As Exception
            logger.Error("Failed to load store list with error: " + ex.Message)

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        If mrsStore.RecordCount > 0 Then
            mrsStore.Sort = "Zone_Name"

            Do
                iZoneID = CType(mrsStore.Fields("Zone_ID").Value, Integer)
                sZoneName = CType(mrsStore.Fields("Zone_Name").Value, String)
                If sZoneName <> sPrevZone Then
                    sPrevZone = sZoneName
                    cmbZones.Items.Add(New VB6.ListBoxItem(sZoneName, iZoneID))
                End If
                mrsStore.MoveNext()
            Loop Until mrsStore.EOF

            mrsStore.Sort = "State"

            Do
                sState = CType(mrsStore.Fields("State").Value, String)
                If (sState <> sPrevState) And (sState <> "") Then
                    sPrevState = sState
                    cmbState.Items.Add(sState)
                End If
                mrsStore.MoveNext()
            Loop Until mrsStore.EOF

            mrsStore.Sort = ""
        End If

        logger.Debug("LoadCombo Exit")
    End Sub

    Public Function GetStoreListString(ByRef lZone_ID As Integer, ByRef msState As String, ByRef bMega_Stores As Boolean, ByRef bWFM_Stores As Boolean, ByRef bAllStores As Boolean) As String
        Dim msStores As String
        Dim sFilter As String
        sFilter = String.Empty
        msStores = String.Empty

        If lZone_ID > 0 Then
            sFilter = "Zone_ID = " & lZone_ID
        ElseIf Len(msState) > 0 Then
            sFilter = "State = '" & msState & "'"
        ElseIf bMega_Stores Then
            sFilter = "Mega_Store = 1"
        ElseIf bWFM_Stores Then
            sFilter = "WFM_Store = 1"
        End If

        mrsStore.Filter = ADODB.FilterGroupEnum.adFilterNone
        mrsStore.Filter = sFilter
        Do While Not mrsStore.EOF
            If Len(msStores) > 0 Then
                msStores = msStores & "|" & CType(mrsStore.Fields("Store_No").Value, String)
            Else
                msStores = CType(mrsStore.Fields("Store_No").Value, String)
            End If
            mrsStore.MoveNext()
        Loop
        mrsStore.Filter = ADODB.FilterGroupEnum.adFilterNone

        GetStoreListString = msStores
    End Function

    Private Sub InitApproveButtons()
        UltraToolbarsManager1.Toolbars("MainGridToolBar").Tools("Pay By Invoice").SharedProps.Enabled = False
    End Sub

    Private Sub SetApprovePermissions()
        UltraToolbarsManager1.Toolbars("MainGridToolBar").Tools("Pay By Invoice").SharedProps.Enabled = Not BlankInvoiceExists()
    End Sub

    Private Sub ClearComboBoxes()
        SetActive(cmbStore, False)
        SetActive(cmbZones, False)
        SetActive(cmbState, False)
        cmbStore.SelectedIndex = -1
        cmbZones.SelectedIndex = -1
        cmbState.SelectedIndex = -1

        ComboBox_ResolutionCodes.SelectedIndex = -1
    End Sub
    Private Sub DisableOptions()
        _optSelection_0.Enabled = False
        _optSelection_1.Enabled = False
        _optSelection_2.Enabled = False
        _optSelection_3.Enabled = False
        _optSelection_4.Enabled = False
        _optSelection_5.Enabled = False
    End Sub

    Private Sub _optSelection_0_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_0.CheckedChanged
        ' Stores Radio Button
        If _optSelection_0.Checked Then
            SetActive(cmbStore, True)
            cmbZones.SelectedIndex = -1
            cmbState.SelectedIndex = -1
        Else
            SetActive(cmbStore, False)
        End If
    End Sub

    Private Sub _optSelection_1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_1.CheckedChanged
        ' Zones Radio Button
        If _optSelection_1.Checked Then
            SetActive(cmbZones, True)
        Else
            SetActive(cmbZones, False)
        End If
    End Sub

    Private Sub _optSelection_2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_2.CheckedChanged
        ' State Radio Button
        If _optSelection_2.Checked Then
            SetActive(cmbState, True)
        Else
            SetActive(cmbState, False)
        End If
    End Sub

    Private Sub _optSelection_3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_3.CheckedChanged
        ' All HFM Radio Button (Mega Stores)
        If _optSelection_3.Checked Then
            Call ClearComboBoxes()
        End If
    End Sub

    Private Sub _optSelection_4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_4.CheckedChanged
        ' All WFM Radio Button (Mega Stores)
        If _optSelection_4.Checked Then
            Call ClearComboBoxes()
        End If
    End Sub

    Private Sub _optSelection_5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optSelection_5.CheckedChanged
        ' All Stores Radio Button (Mega Stores)
        If _optSelection_5.Checked Then
            Call ClearComboBoxes()
        End If
    End Sub

    Private Sub UltraGrid_AllSuspended_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid_AllSuspended.AfterRowActivate
        UltraGrid_AllSuspended.ActiveRow.Selected = False
    End Sub

    Private Sub UltraGrid_AllSuspended_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles UltraGrid_AllSuspended.AfterSelectChange
        Dim blnPayByAgreedCost As Boolean = False

        If e.Type Is GetType(UltraGridRow) Then
            If UltraGrid_AllSuspended.Selected.Rows.Count > 0 Then
                For Each dr As UltraGridRow In UltraGrid_AllSuspended.Selected.Rows
                    If dr.Cells("PayByAgreedCost").Value = "Y" Then
                        blnPayByAgreedCost = True
                    End If
                Next
            ElseIf Not IsNothing(UltraGrid_AllSuspended.ActiveRow) Then
                If UltraGrid_AllSuspended.ActiveRow.GetType = GetType(UltraGridRow) Then
                    If UltraGrid_AllSuspended.ActiveRow.Cells("PayByAgreedCost").Value = "Y" Then
                        blnPayByAgreedCost = True
                    End If
                End If
            End If

            UltraToolbarsManager1.Tools("Pay By Invoice").SharedProps.Enabled = Not blnPayByAgreedCost And Not BlankInvoiceExists()
            Button_RefreshCost.Enabled = True
        End If

    End Sub

    Private Sub UltraGrid_AllSuspended_BeforeCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles UltraGrid_AllSuspended.BeforeCellUpdate
        If e.Cell.Band.Index = 1 Then Exit Sub

        If e.Cell.Column.Key = "POAdminNotes" Then

            If e.NewValue Is System.DBNull.Value Then
                e.Cancel = True
            End If
        End If

        If e.Cancel = False Then
            If e.Cell.OriginalValue Is Nothing And e.NewValue IsNot Nothing Then

                changedRows.Add(e.Cell.Row)
            Else


                If Not e.Cell.OriginalValue.Equals(e.NewValue) Then
                    If Not changedRows.Contains(e.Cell.Row) Then
                        changedRows.Add(e.Cell.Row)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub CheckBox_RemediatedSuspensions_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_RemediatedSuspensions.CheckedChanged
        If CheckBox_RemediatedSuspensions.Checked = True Then
            ComboBox_ResolutionCodes.Enabled = True
            ComboBox_ResolutionCodes.SelectedIndex = -1

            UltraToolbarsManager1.Toolbars("MainGridToolBar").Tools("Pay By Invoice").SharedProps.Visible = False

            'Disable Refresh Cost Button
            Button_RefreshCost.Enabled = False
        Else
            UltraToolbarsManager1.Toolbars("MainGridToolBar").Tools("Pay By Invoice").SharedProps.Visible = True
            ComboBox_ResolutionCodes.Enabled = False

            If UltraGrid_AllSuspended.Selected.Rows.Count > 0 Then
                'Enable Refresh Cost Button only if there are selected rows
                Button_RefreshCost.Enabled = True
            End If
        End If
    End Sub

    Private Sub UltraGrid_AllSuspended_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles UltraGrid_AllSuspended.InitializeRow
        If e.Row.Cells("DocTypeOther").Value = "Y" Then
            e.Row.Cells("DocTypeOther").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("CreditPO").Value = "Y" Then
            e.Row.Cells("CreditPO").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("AdjustedCost").Value = "Y" Then
            e.Row.Cells("AdjustedCost").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("MatchedEInvoice").Value = "Y" Then
            e.Row.Cells("MatchedEInvoice").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("PayByAgreedCost").Value = "Y" Then
            e.Row.Cells("PayByAgreedCost").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("BeverageSubteam").Value = "Y" Then
            e.Row.Cells("BeverageSubteam").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("cbW").Value = "Y" Then
            e.Row.Cells("cbW").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("QtyMismatch").Value = "Y" Then
            e.Row.Cells("QtyMismatch").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("CostNotByCase").Value = "Y" Then
            e.Row.Cells("CostNotByCase").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("OrderUnitMismatch").Value = "Y" Then
            e.Row.Cells("OrderUnitMismatch").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("ReceivingDocument").Value = "Y" Then
            e.Row.Cells("ReceivingDocument").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("PartialShipment").Value = "Y" Then
            e.Row.Cells("PartialShipment").Appearance.BackColor = Color.Yellow
        End If

        If e.Row.Cells("Status").Value = "In Review" Then
            e.Row.Cells("Status").Tag = String.Format("This order is currently being reviewed by {0} ({1})", e.Row.Cells("Inreviewusername").Value, e.Row.Cells("Inreviewfullname").Value)
        ElseIf e.Row.Cells("status").Value = "Open" Then
            e.Row.Cells("Status").Tag = String.Format("[Double Click] to mark as ""In Review""")
        Else
            e.Row.Cells("Status").Tag = String.Empty
        End If

        If e.Row.Cells("ClosedDate").Value IsNot Nothing Then
            '   e.Row.Activation = Activation.Disabled
        End If
    End Sub

    Private Sub SaveData()
        Dim Notes As String = String.Empty
        Dim ResolutionCodeID As Integer
        Dim OrderNo As Integer = Nothing
        Dim SQLStr As String = String.Empty

        UltraGrid_AllSuspended.UpdateData()

        If changedRows.Count > 0 Then
            With UltraProgressBar_SavingChanges
                .Minimum = 0
                .Maximum = changedRows.Count
                .Value = .Minimum
                UltraLabel_SaveChangesCount.Text = String.Format("{0} of {1}", .Value + 1, .Maximum)
                UltraLabel_SaveChangesCount.Refresh()
                ShowSaveChangesPopup()

                For Each row As UltraGridRow In changedRows
                    If row.Cells("POAdminNotes").Value IsNot Nothing Or _
                            row.Cells("ResolutionCode").Value <> String.Empty Then

                        OrderNo = row.Cells("OrderHeaderID").Value
                        If row.Cells("POAdminNotes").Value IsNot Nothing Then
                            Notes = row.Cells("POAdminNotes").Value.ToString.Replace("'", "''")
                            If Notes.Length > 5000 Then
                                Notes = Notes.Substring(0, 5000)
                            End If
                        Else
                            Notes = String.Empty
                        End If

                        If Integer.TryParse(row.Cells("ResolutionCode").Value, ResolutionCodeID) = False Then
                            If ComboBox_ResolutionCodesCopy.SelectedItem IsNot Nothing Then
                                ResolutionCodeID = DirectCast(ComboBox_ResolutionCodesCopy.SelectedItem, Compatibility.VB6.ListBoxItem).ItemData
                            End If
                        End If

                        OrderingDAO.UpdateSuspendedPONotes(OrderNo, 0, Notes, ResolutionCodeID)
                    End If
                    .IncrementValue(1)
                    Thread.Sleep(200)

                    UltraLabel_SaveChangesCount.Text = String.Format("{0} of {1}", .Value, .Maximum)
                    UltraLabel_SaveChangesCount.Refresh()
                    .Refresh()
                Next
                changedRows.Clear()
                UltraPanel_SavingChanges.SendToBack()
                UltraPanel_SavingChanges.Hide()
            End With
        End If
    End Sub

    Private Sub UltraGrid_AllSuspended_BeforeCellActivate(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles UltraGrid_AllSuspended.BeforeCellActivate
        ' Only PO Admin can edit data
        If Not gbPOApprovalAdmin And Not gbPOAccountant Then
            e.Cancel = True
        End If
    End Sub

    Private Sub UltraGrid_AllSuspended_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UltraGrid_AllSuspended.MouseDown
        Dim mousePoint As Point = New Point(e.X, e.Y)
        Dim element As UIElement = CType(sender, UltraGrid).DisplayLayout.UIElement.ElementFromPoint(mousePoint)
        Dim notes As String = String.Empty
        Dim cell As UltraGridCell = CType(element.GetContext(GetType(UltraGridCell)), UltraGridCell)

        If Not cell Is Nothing Then
            If Not cell.Row.IsActiveRow Then
                cell.Row.Activate()
            End If

            If cell.Column.Key.Equals("OrderHeaderID") Then
                If e.Button = MouseButtons.Left Then
                    ShowOrder(cell.Value, False)
                ElseIf e.Button = MouseButtons.Right Then
                    UltraGrid_AllSuspended.Selected.Rows.Clear()
                    UltraGrid_AllSuspended.ActiveRow = Nothing
                    cell.Row.Activate()

                    If (UltraGrid_AllSuspended.DisplayLayout.Bands(0).Columns.Exists("POAdminNotes") = True) Then
                        notes = cell.Row.Cells("POAdminNotes").Value
                    End If

                    ShowOrderDetails(cell.Value, cell.Row.Cells("VendorID").Value, cell.Row.Cells("CreatedBy").Value, cell.Row.Cells("ClosedBy").Value, notes, cell.Row.Cells("PayByAgreedCost").Value)

                    If Me.blnPOApprovedByLineItem Then
                        UltraGrid_AllSuspended.Rows(cell.Row.Index).Delete(False)
                        UltraGrid_AllSuspended.Refresh()
                        Me.blnPOApprovedByLineItem = False
                    End If
                End If
            ElseIf cell.Column.Key.Equals("VendorName") Then
                If e.Button = MouseButtons.Left Then
                    ShowVendor(cell.Row.Cells("VendorID").Value)
                End If
            ElseIf cell.Column.Key.Equals("InvoiceNum") Then
                If e.Button = MouseButtons.Left Then
                    If cell.Value IsNot Nothing Then
                        ShowOrder(cell.Row.Cells("OrderHeaderID").Value, True)
                    End If
                End If
            ElseIf cell.Column.Key.Equals("POAdminNotes") Then
                Dim frm As New frmOrdersDesc

                frm.blnIsFromSuspendedPOTool = True
                frm.iOrderHeaderID = UltraGrid_AllSuspended.ActiveRow.Cells("OrderHeaderID").Value
                frm.iOrderItemID = 0

                frm.ShowDialog()
                UltraGrid_AllSuspended.ActiveRow.Cells("POAdminNotes").Value = frm.txtField.Text
                frm.Close()
                frm.Dispose()
                SaveData()
            End If
        End If
    End Sub
    Private Sub ShowVendor(ByVal VendorId As Integer)
        glVendorID = VendorId
        bSpecificVendor = True
        frmVendor.ShowDialog()
        frmVendor.Dispose()
        glVendorID = -1
        bSpecificVendor = False
    End Sub

    Private Sub ShowOrder(ByRef OrderHeaderId As Integer, Optional ByVal JumpToOrderStatus As Boolean = False)
        Dim ViewPO As frmOrders
        bSpecificOrder = True
        ViewPO = New frmOrders(OrderHeaderId, JumpToOrderStatus)
        ViewPO.ShowDialog()
        ViewPO.Dispose()
        bSpecificOrder = False
    End Sub

    Private Sub ShowOrderDetails(ByRef OrderHeaderId As Integer, ByRef VendorId As Integer, ByRef POCreator As String, ByRef POCloser As String, ByRef notes As String, ByVal sPayByAgreedCost As String)
        frmLineItems.POOrderHeaderId = OrderHeaderId
        frmLineItems.POCreator = POCreator
        frmLineItems.POCloser = POCloser
        frmLineItems.POAdminNotes = notes
        frmLineItems.VendorId = VendorId

        If sPayByAgreedCost = "N" Then
            frmLineItems.IsPayByAgreedCost = False
        Else
            frmLineItems.IsPayByAgreedCost = True
        End If

        frmLineItems.ShowDialog()
        frmLineItems.Dispose()
    End Sub

    Private Sub UltraGrid_AllSuspended_MouseEnterElement(ByVal sender As Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles UltraGrid_AllSuspended.MouseEnterElement
        'If the mouse enters the cell display the tooltip if one has been set in the .Tag property. 

        Dim acell As UltraGridCell = e.Element.GetContext(GetType(Infragistics.Win.UltraWinGrid.UltraGridCell))

        If Not acell Is Nothing Then
            If Not acell.Tag Is Nothing Then
                ToolTip1.Active = True
                ToolTip1.SetToolTip(sender, acell.Tag.ToString())
            Else
                If Not acell.Column.Tag Is Nothing Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(sender, acell.Column.Tag.ToString())
                End If
            End If
        End If


    End Sub

    Private Sub UltraGrid_AllSuspended_MouseLeaveElement(ByVal sender As Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles UltraGrid_AllSuspended.MouseLeaveElement
        ' tool tip over cell 
        Dim acell As UltraGridCell = e.Element.GetContext(GetType(Infragistics.Win.UltraWinGrid.UltraGridCell))

        If Not acell Is Nothing Then
            ToolTip1.Active = False
        End If

    End Sub

    Private Sub UltraToolbarsManager1_ToolClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles UltraToolbarsManager1.ToolClick
        'user clicks on toolbar button. decide what to do based on what button they clicked.
        Select Case e.Tool.Key
            Case "Export Excel"
                ExportToExcel()

            Case "Email PO Creator"
                EmailPOCreator()

            Case "Email PO Closer"
                EmailPOCloser()

            Case "Email Other"
                ' Do nothing here. Handled in UltraTextEditor1_EditorButtonClick

            Case "Pay By Invoice"
                SaveData()
                PayByInvoice()
            Case "SaveChanges"
                SaveData()
        End Select

    End Sub

    Private Sub DisplayInvoiceDiscrepancyReport()
        InvoiceDiscrepanciesReport.ShowDialog()
        InvoiceDiscrepanciesReport.Dispose()
    End Sub

    Private Sub ExportToExcel()
        Dim dlg As SaveFileDialog = New SaveFileDialog()

        dlg.Filter = "(Excel files)|*.xls"
        dlg.Title = "Enter destination Excel filename"

        Dim res As DialogResult = dlg.ShowDialog()
        Dim str As String = dlg.FileName

        If str <> String.Empty Then
            UltraGridExcelExporter1.Export(UltraGrid_AllSuspended, str)
        End If
    End Sub

    Private Function ValidateResolutionCodes() As Boolean
        Dim AreAllCodesValid As Boolean = True

        'loop through selected and make sure they all have ResonCodes.
        For Each ugr As UltraGridRow In UltraGrid_AllSuspended.Selected.Rows
            If ugr.Cells("ResolutionCode").Text.Equals(String.Empty) Then
                AreAllCodesValid = False
            End If
        Next
        'if they dont, prompt the user for one. 
        If Not AreAllCodesValid Then ShowResolutionCodePopup()

        Return AreAllCodesValid
    End Function
    Private Sub ShowResolutionCodePopup()
        ComboBox_ResolutionCodesCopy.SelectedItem = Nothing
        UltraPanel_ResolutionCodes.BringToFront()
        UltraPanel_ResolutionCodes.Show()
    End Sub

    Private Sub ShowSaveChangesPopup()
        UltraPanel_SavingChanges.BringToFront()
        UltraPanel_SavingChanges.Show()
        UltraPanel_SavingChanges.Refresh()
    End Sub


    Private Sub PayByInvoice()
        logger.Debug("PayByInvoice entry")
        Dim row As UltraGridRow
        Dim currentOrderHeaderID As Integer
        Dim payByAgreedCost As String
        Dim resolutionDesc As String = String.Empty
        Dim ResolutionId As Integer



        ' set the value of callingAction to the AddressOf the PayByInvoice Sub Routine. So we know where the call came from and can call it again later. The magic of delegates.
        callingAction = AddressOf PayByInvoice

        If UltraGrid_AllSuspended.Selected.Rows.Count >= 1 Then
            If ValidateResolutionCodes() Then

                If MsgBox(sNotification_PayByInvoice, MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                    For Each row In UltraGrid_AllSuspended.Selected.Rows
                        payByAgreedCost = row.Cells("PayByAgreedCost").Value
                        currentOrderHeaderID = CInt(row.Cells("OrderHeaderID").Value)
                        resolutionDesc = row.Cells("ResolutionCode").Text
                        If Integer.TryParse(row.Cells("ResolutionCode").Value, ResolutionId) = False Then
                            If ComboBox_ResolutionCodesCopy.SelectedItem IsNot Nothing Then
                                ResolutionId = DirectCast(ComboBox_ResolutionCodesCopy.Items(ComboBox_ResolutionCodesCopy.FindStringExact(row.Cells("ResolutionCode").Value)), VB6.ListBoxItem).ItemData
                            End If
                        End If

                        logger.Info("btnPayByInvoice_Click - approving the selected invoice using the Pay by Invoice option: orderHeader_ID=" + currentOrderHeaderID.ToString)
                        OrderingDAO.ApproveInvoiceUsingPayByInvoiceOption(currentOrderHeaderID, ResolutionId)
                    Next
                    ' Refresh the data grid to remove the invoices that have been processed.
                    UltraGrid_AllSuspended.DeleteSelectedRows(False)
                Else
                    logger.Info("btnPayByInvoice_Click - user elected not to pay the selected invoices using the Pay by Invoice option after the confirmation was presented")
                End If
            End If
        ElseIf UltraGrid_AllSuspended.ActiveRow.IsDataRow Then
            UltraGrid_AllSuspended.Selected.Rows.Add(UltraGrid_AllSuspended.ActiveRow)
            If ValidateResolutionCodes() Then
                If MsgBox(sNotification_PayByInvoice, MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                    payByAgreedCost = UltraGrid_AllSuspended.ActiveRow.Cells("PayByAgreedCost").Value
                    currentOrderHeaderID = CInt(UltraGrid_AllSuspended.ActiveRow.Cells("OrderHeaderID").Value)
                    resolutionDesc = UltraGrid_AllSuspended.ActiveRow.Cells("ResolutionCode").Text

                    If Integer.TryParse(UltraGrid_AllSuspended.ActiveRow.Cells("ResolutionCode").Value, ResolutionId) = False Then
                        ResolutionId = DirectCast(ComboBox_ResolutionCodesCopy.Items(ComboBox_ResolutionCodesCopy.FindStringExact(UltraGrid_AllSuspended.ActiveRow.Cells("ResolutionCode").Value)), VB6.ListBoxItem).ItemData
                    End If

                    logger.Info("btnPayByInvoice_Click - approving the selected invoice using the Pay by Invoice option: orderHeader_ID=" + currentOrderHeaderID.ToString)
                    OrderingDAO.ApproveInvoiceUsingPayByInvoiceOption(currentOrderHeaderID, ResolutionId)

                    UltraGrid_AllSuspended.DeleteSelectedRows(False)
                Else
                    logger.Info("btnPayByInvoice_Click - user elected not to pay the selected invoices using the Pay by Invoice option after the confirmation was presented")
                End If
            End If
        Else
            ' Alert the user that they must select at least one invoice
            MsgBox("An invoice from the list must be selected.", MsgBoxStyle.Exclamation, Me.Text)
        End If

        logger.Debug("PayByInvoice exit")
    End Sub
    Private Sub EmailPOCreator()
        'Dim currentTabNumber As Integer = TabControl1.SelectedIndex
        Dim currentGrid As UltraGrid

        Dim SendMailClient As SmtpClient

        Dim emailFrom As String = "IRMA@WholeFoods.com"
        Dim emailTo As String
        Dim emailMessage As String
        Dim subject As String = "Suspended PO Alert"
        Dim message As MailMessage

        Dim POCreator As String
        Dim currentUser As String
        Dim notes As String

        currentGrid = UltraGrid_AllSuspended

        If currentGrid.ActiveRow Is Nothing Then
            If currentGrid.Selected.Rows.Count > 0 Then
                If currentGrid.Selected.Rows(0).IsDataRow Then
                    glOrderHeaderID = CInt(currentGrid.Selected.Rows(0).Cells("OrderHeaderID").Value)
                    POCreator = currentGrid.Selected.Rows(0).Cells("CreatedBy").Value
                    notes = ""
                    If (currentGrid.DisplayLayout.Bands(0).Columns.Exists("POAdminNotes") = True) Then
                        notes = currentGrid.Selected.Rows(0).Cells("POAdminNotes").Value
                    End If
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            If currentGrid.ActiveRow.IsDataRow Then
                glOrderHeaderID = CInt(currentGrid.ActiveRow.Cells("OrderHeaderID").Value)
                POCreator = currentGrid.ActiveRow.Cells("CreatedBy").Value
                notes = ""
                If (currentGrid.DisplayLayout.Bands(0).Columns.Exists("POAdminNotes") = True) Then
                    notes = currentGrid.ActiveRow.Cells("POAdminNotes").Value
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        Dim u As UserBO = New UserBO(POCreator)
        If u.Email = vbNullString Or Trim(u.Email) = String.Empty Then
            MsgBox("IRMA does not have PO creator's email in the database", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        emailTo = u.Email
        POCreator = u.FullName

        Dim cu As UserBO = New UserBO(My.Application.CurrentUserID)
        currentUser = cu.FullName

        emailMessage = "Hello " + POCreator + ": " + vbCrLf + _
                    currentUser + " has identified a suspended PO that needs your attention. Please log into IRMA to review this item:" + vbCrLf + _
                    "PO# " + glOrderHeaderID.ToString() + vbCrLf + vbCrLf + _
                    "PO Admin Notes:" + vbCrLf + _
                    notes + vbCrLf + vbCrLf + _
                    "This is a system-generated email from the IRMA client."
        Try
            SendMailClient = New SmtpClient(ConfigurationServices.AppSettings("EmailSMTPServer"))
            message = New MailMessage(emailFrom, emailTo, subject, emailMessage)
            SendMailClient.Send(message)

            MsgBox("Mail sent to " + POCreator, MsgBoxStyle.Information, "IRMA")

        Catch ex As Exception
            'e-mail confirmation shouldn't be a fatal error!
            MsgBox("Mail not sent - " & ex.Message, MsgBoxStyle.Critical, "Mail Error")
        Finally
            SendMailClient = Nothing
            message = Nothing
        End Try
    End Sub
    Private Sub EmailPOCloser()
        'Dim currentTabNumber As Integer = TabControl1.SelectedIndex
        Dim currentGrid As UltraGrid

        Dim SendMailClient As SmtpClient

        Dim emailFrom As String = "IRMA@WholeFoods.com"
        Dim emailTo As String
        Dim emailMessage As String
        Dim subject As String = "Suspended PO Alert"
        Dim message As MailMessage

        Dim POCloser As String
        Dim currentUser As String
        Dim notes As String

        currentGrid = UltraGrid_AllSuspended

        If currentGrid.ActiveRow Is Nothing Then
            If currentGrid.Selected.Rows.Count > 0 Then
                If currentGrid.Selected.Rows(0).IsDataRow Then
                    glOrderHeaderID = CInt(currentGrid.Selected.Rows(0).Cells("OrderHeaderID").Value)
                    POCloser = currentGrid.Selected.Rows(0).Cells("ClosedBy").Value
                    notes = ""
                    If (currentGrid.DisplayLayout.Bands(0).Columns.Exists("POAdminNotes") = True) Then
                        notes = currentGrid.Selected.Rows(0).Cells("POAdminNotes").Value
                    End If
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            If currentGrid.ActiveRow.IsDataRow Then
                glOrderHeaderID = CInt(currentGrid.ActiveRow.Cells("OrderHeaderID").Value)
                POCloser = currentGrid.ActiveRow.Cells("ClosedBy").Value
                notes = ""
                If (currentGrid.DisplayLayout.Bands(0).Columns.Exists("POAdminNotes") = True) Then
                    notes = currentGrid.ActiveRow.Cells("POAdminNotes").Value
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        Dim u As UserBO = New UserBO(POCloser)
        If u.Email = vbNullString Or Trim(u.Email) = String.Empty Then
            MsgBox("IRMA does not have PO closer's email is the database", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        emailTo = u.Email
        POCloser = u.FullName

        Dim cu As UserBO = New UserBO(My.Application.CurrentUserID)
        currentUser = cu.FullName

        emailMessage = "Hello " + POCloser + ": " + vbCrLf + _
                    currentUser + " has identified a suspended PO that needs your attention. Please log into IRMA to review this item:" + vbCrLf + _
                    "PO# " + glOrderHeaderID.ToString() + vbCrLf + vbCrLf + _
                    "PO Admin Notes:" + vbCrLf + _
                    notes + vbCrLf + vbCrLf + _
                    "This is a system-generated email from the IRMA client."
        Try
            SendMailClient = New SmtpClient(ConfigurationServices.AppSettings("EmailSMTPServer"))
            message = New MailMessage(emailFrom, emailTo, subject, emailMessage)
            SendMailClient.Send(message)

            MsgBox("Mail sent to " + POCloser, MsgBoxStyle.Information, "IRMA")

        Catch ex As Exception
            'e-mail confirmation shouldn't be a fatal error!
            MsgBox("Mail not sent - " & ex.Message, MsgBoxStyle.Critical, "Mail Error")
        Finally
            SendMailClient = Nothing
            message = Nothing
        End Try
    End Sub

    Private Sub SendEmailOther(ByVal emailaddress As String)

        Dim currentGrid As UltraGrid

        Dim SendMailClient As SmtpClient

        Dim emailFrom As String = "IRMA@WholeFoods.com"
        Dim emailTo As String
        Dim emailMessage As String
        Dim subject As String = "Suspended PO Alert"
        Dim message As MailMessage

        Dim POCloser As String
        Dim currentUser As String
        Dim notes As String


        currentGrid = UltraGrid_AllSuspended

        If currentGrid.ActiveRow Is Nothing Then
            If currentGrid.Selected.Rows.Count > 0 Then
                If currentGrid.Selected.Rows(0).IsDataRow Then
                    glOrderHeaderID = CInt(currentGrid.Selected.Rows(0).Cells("OrderHeaderID").Value)
                    POCloser = currentGrid.Selected.Rows(0).Cells("ClosedBy").Value
                    notes = ""
                    If (currentGrid.DisplayLayout.Bands(0).Columns.Exists("POAdminNotes") = True) Then
                        notes = currentGrid.Selected.Rows(0).Cells("POAdminNotes").Value
                    End If
                Else
                    MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            If currentGrid.ActiveRow.IsDataRow Then
                glOrderHeaderID = CInt(currentGrid.ActiveRow.Cells("OrderHeaderID").Value)
                POCloser = currentGrid.ActiveRow.Cells("ClosedBy").Value
                notes = ""
                If (currentGrid.DisplayLayout.Bands(0).Columns.Exists("POAdminNotes") = True) Then
                    notes = currentGrid.ActiveRow.Cells("POAdminNotes").Value
                End If
            Else
                MsgBox("Please select a data row", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        emailTo = emailaddress

        Dim cu As UserBO = New UserBO(My.Application.CurrentUserID)
        currentUser = cu.FullName

        emailMessage = currentUser + " has identified a suspended PO that needs your attention. Please log into IRMA to review this item:" + vbCrLf + _
                    "PO# " + glOrderHeaderID.ToString() + vbCrLf + vbCrLf + _
                    "PO Admin Notes:" + vbCrLf + _
                    notes + vbCrLf + vbCrLf + _
                    "This is a system-generated email from the IRMA client."
        Try
            SendMailClient = New SmtpClient(ConfigurationServices.AppSettings("EmailSMTPServer"))
            message = New MailMessage(emailFrom, emailTo, subject, emailMessage)
            SendMailClient.Send(message)

            MsgBox("Mail sent to " + emailTo, MsgBoxStyle.Information, "IRMA")

        Catch ex As Exception
            'e-mail confirmation shouldn't be a fatal error!
            MsgBox("Mail not sent - " & ex.Message, MsgBoxStyle.Critical, "Mail Error")
        Finally
            SendMailClient = Nothing
            message = Nothing
        End Try
    End Sub


    Private Sub UltraTextEditor1_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles UltraTextEditor1.EditorButtonClick
        Dim _popup As Infragistics.Win.UltraWinToolbars.PopupControlContainerTool

        If e.Button.Key = "SendEmailOther" Then

            'make sure its a valid email address.
            Dim validation As Infragistics.Win.Misc.Validation = Me.UltraValidator1.Validate(UltraTextEditor1)

            'if validated send email. 
            If validation Is Nothing Then
                SendEmailOther(e.Button.Editor.Value)

                'close the popup after email is sent.
                _popup = UltraToolbarsManager1.Toolbars(0).Tools("Email Other")
                _popup.ClosePopup()
            End If
        End If

    End Sub

    Private Sub UltraGrid_AllSuspended_BeforeColumnChooserDisplayed(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs) Handles UltraGrid_AllSuspended.BeforeColumnChooserDisplayed
        ' when user opens the Field Chooser, make sure the columns listed are in the same order as the grid instead of alphabetical.
        e.Dialog.ColumnChooserControl.ColumnDisplayOrder = ColumnDisplayOrder.SameAsGrid
    End Sub

    Private Sub UltraToolbarsManager1_ToolValueChanged(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinToolbars.ToolEventArgs) Handles UltraToolbarsManager1.ToolValueChanged
        ' when user changes the filter dropdown. Apply chosen filter.
        Dim cb As Infragistics.Win.UltraWinToolbars.ComboBoxTool
        cb = e.Tool

        If UltraGrid_AllSuspended.DisplayLayout.Rows.Count > 0 Then
            If e.Tool.Key.Equals("ComboBox_GridFilter") Then
                ApplyGridFilter(cb.SelectedItem.ToString())
            End If
        End If
        UltraGrid_AllSuspended.ActiveRowScrollRegion.Scroll(RowScrollAction.Top) ' reset the scrollbar (tfs 7462)
    End Sub

    Private Sub ApplyGridFilter(ByVal Filter As String)

        With UltraGrid_AllSuspended.DisplayLayout.Bands(0)
            .ColumnFilters.ClearAllFilters() ' clear all filters to start with a clean slate. 
            Select Case Filter.ToLower()
                Case "all"
                    'do nothing
                Case "pay by invoice"
                    .ColumnFilters("PaymentType").FilterConditions.Add(FilterComparisionOperator.Match, "Pay By Invoice")  ' filter by PaymentType
                Case "pay by agreed cost"
                    .ColumnFilters("PaymentType").FilterConditions.Add(FilterComparisionOperator.Match, "Pay Agreed Cost")  ' filter by PaymentType
                Case "missing doc/inv data"
                    .ColumnFilters("DocTypeOther").FilterConditions.Add(FilterComparisionOperator.Match, "Y")  ' filter by DocTypeOther
            End Select
        End With
    End Sub


    Private Sub UltraGrid_AllSuspended_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UltraGrid_AllSuspended.MouseDoubleClick

        ' if user double clicks on the Status column, change the status. 

        Dim mousePoint As Point = New Point(e.X, e.Y)
        Dim element As UIElement = CType(sender, UltraGrid).DisplayLayout.UIElement.ElementFromPoint(mousePoint)


        Dim cell As UltraGridCell = CType(element.GetContext(GetType(UltraGridCell)), UltraGridCell)

        If Not cell Is Nothing Then
            If cell.Column.Key.Equals("Status") Then
                ModifyStatus(cell, cell.Row.Cells("OrderHeaderID").Value, cell.Row.Cells("Status").Value)
            End If
        End If

    End Sub


    Private Sub ModifyStatus(ByRef cell As UltraGridCell, ByVal OrderHeaderId As Integer, ByVal status As String)

        'update the database field first. then update the status cell for the corresponding record so we dont have to refresh the entire grid. 
        'faster and more responsive for the user. 
        Dim data As ArrayList = Nothing
        Dim list As List(Of AllSuspendedInvoiceBO) = Nothing
        If status IsNot Nothing Then
            Select Case status.ToLower
                Case "open"
                    Try
                        'set the value in the database.
                        OrderingDAO.ModifyInReviewStatus(OrderHeaderId, My.Application.CurrentUserID, 1)
                        data = UltraGrid_AllSuspended.DataSource

                        ' a faster way could be implemented to find the record we are looking for. AllSuspendedInvoiceBO would have to implement ICopmparable or IEquatable and overload several Methods. 
                        ' Didnt have time to venture down this path. Current method is fast enough for now. Future enhancement?

                        ' look through local datasource. find the record we clicked on. Update the value in the datasource and re-bind. Faster than querying the db and rebuliding the grid from scratch.
                        For Each item As AllSuspendedInvoiceBO In data
                            If item.OrderHeaderID = OrderHeaderId Then
                                item.Status = "In Review"
                            End If
                        Next

                        UltraGrid_AllSuspended.DataSource = data
                        UltraGrid_AllSuspended.DataBind()


                    Catch ex As Exception
                        'TODO: how do we handle this error?
                    End Try

                Case "in review"
                    Try
                        OrderingDAO.ModifyInReviewStatus(OrderHeaderId, My.Application.CurrentUserID, 0)

                        data = UltraGrid_AllSuspended.DataSource

                        For Each item As AllSuspendedInvoiceBO In data
                            If item.OrderHeaderID = OrderHeaderId Then
                                item.Status = "Open"
                            End If
                        Next

                        UltraGrid_AllSuspended.DataSource = data
                        UltraGrid_AllSuspended.DataBind()


                    Catch ex As Exception
                        'TODO: how do we handle this error?
                    End Try

                Case Else
                    'do nothing
            End Select
        End If

    End Sub

    Private Sub UltraButton_ApplyResolutionCodes_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton_ApplyResolutionCodes_Cancel.Click
        UltraPanel_ResolutionCodes.Hide()
        UltraPanel_ResolutionCodes.SendToBack()
    End Sub

    Private Sub UltraButton_ApplyResolutionCodes_Apply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton_ApplyResolutionCodes_Apply.Click
        If Not ComboBox_ResolutionCodesCopy.SelectedItem Is Nothing Then
            'hide the popup panel.
            UltraPanel_ResolutionCodes.Hide()
            UltraPanel_ResolutionCodes.SendToBack()

            ApplyChosenResolutionCode()

            'at this point callingAction will be AddressOf PayByAgreedCost or PayByInvoice (whichever one was called.) Since changes have been made. Lets call it again! 
            SaveData()
            callingAction()
        Else
            ComboBox_ResolutionCodesCopy.Focus()
        End If
    End Sub

    Private Sub ApplyChosenResolutionCode()
        ' apply the chosen Resolution Code to every row that we have selected.
        Dim resolution As String = ComboBox_ResolutionCodesCopy.SelectedItem.ToString()

        For Each ugr As UltraGridRow In UltraGrid_AllSuspended.Selected.Rows
            If ugr.Cells("ResolutionCode").Text.Equals(String.Empty) Then
                ugr.Cells("ResolutionCode").Value = resolution
            End If
        Next
    End Sub


    Private Sub LinkLabel_InvoiceDiscrepancyReport_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_InvoiceDiscrepancyReport.LinkClicked
        DisplayInvoiceDiscrepancyReport()

        Dim _popup As Infragistics.Win.UltraWinToolbars.PopupControlContainerTool
        _popup = UltraToolbarsManager1.Toolbars(0).Tools("Reports")
        _popup.ClosePopup()
    End Sub

    Private Sub LinkLabel_EinvoiceItemExceptionsReport_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_EinvoiceItemExceptionsReport.LinkClicked

        '***********************************************
        'Updated By:    Denis Ng
        'Date:          08/28/2011
        'Comment:       The form EInvoiceExceptionReport 
        '               reqests parameters from users before 
        '               generating the report EInovoice Exception 
        '               Report.
        '***********************************************
        EInvoiceExceptionReport.ShowDialog()
        'END TODO #######################################


        Dim _popup As Infragistics.Win.UltraWinToolbars.PopupControlContainerTool
        _popup = UltraToolbarsManager1.Toolbars(0).Tools("Reports")
        _popup.ClosePopup()
    End Sub

    Private Sub UltraGridExcelExporter1_BeginExport(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.ExcelExport.BeginExportEventArgs) Handles UltraGridExcelExporter1.BeginExport
        Dim column As UltraWinGrid.UltraGridColumn

        For Each column In e.Layout.Bands(0).Columns
            column.Hidden = False
        Next
    End Sub

    Private Function BlankInvoiceExists() As Boolean
        Dim blnResult As Boolean = False

        If UltraGrid_AllSuspended.Selected.Rows.Count > 0 Then
            For Each dr As UltraGridRow In UltraGrid_AllSuspended.Selected.Rows
                If IsNothing(dr.Cells("InvoiceNum").Value) And dr.Cells("EinvoiceRequired").Value = "N" Then
                    blnResult = True
                End If

                If dr.Cells("EinvoiceRequired").Value = "Y" And (IsNothing(dr.Cells("EInvoiceID").Value) Or dr.Cells("EInvoiceID").Value = 0) Then
                    blnResult = True
                End If
            Next
        ElseIf Not IsNothing(UltraGrid_AllSuspended.ActiveRow) Then
            If UltraGrid_AllSuspended.ActiveRow.GetType = GetType(UltraGridRow) Then
                blnResult = (IsNothing(UltraGrid_AllSuspended.ActiveRow.Cells("InvoiceNum").Value) And UltraGrid_AllSuspended.ActiveRow.Cells("EinvoiceRequired").Value = "N") Or _
                            (UltraGrid_AllSuspended.ActiveRow.Cells("EinvoiceRequired").Value = "Y" _
                             And (IsNothing(UltraGrid_AllSuspended.ActiveRow.Cells("EInvoiceID").Value) Or UltraGrid_AllSuspended.ActiveRow.Cells("EInvoiceID").Value = 0))
            End If
        End If

        Return blnResult
    End Function

    Private Sub UltraTextEditor1_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles UltraTextEditor1.KeyDown
        Dim _popup As Infragistics.Win.UltraWinToolbars.PopupControlContainerTool

        If e.KeyCode = Keys.Enter Then

            'make sure its a valid email address.
            Dim validation As Infragistics.Win.Misc.Validation = Me.UltraValidator1.Validate(UltraTextEditor1)

            'if validated send email. 
            If validation Is Nothing Then
                SendEmailOther(UltraTextEditor1.Text)

                'close the popup after email is sent.
                _popup = UltraToolbarsManager1.Toolbars(0).Tools("Email Other")
                _popup.ClosePopup()
            End If
        End If
    End Sub

    Private Sub Button_SelectAll_Click(sender As System.Object, e As System.EventArgs) Handles Button_SelectAll.Click
        Dim allRows As RowsCollection = UltraGrid_AllSuspended.Rows

        ' Clear any selected rows to start from nothing for adding rows to Selected RowCollection
        Me.UltraGrid_AllSuspended.Selected.Rows.Clear()

        ' Special method to select all rows and account for GroupByRows
        TraverseAllRowsAndSelectNonGroupByRows(allRows)
    End Sub

    Private Sub Button_RefreshCost_Click(sender As System.Object, e As System.EventArgs) Handles Button_RefreshCost.Click
        logger.Debug("Button_RefreshCost_Click entry")

        If UltraGrid_AllSuspended.Selected.Rows.Count = 0 Then
            MsgBox("Please select one or more rows using the row selector on the left side of the grid.", MsgBoxStyle.Information, "No Rows Selected")
            Exit Sub
        Else
            ' Save the user's display layout before proceeding (unless the grid is empty).  This will capture the user's Column Chooser preferences.
            SaveDisplayLayout()
        End If

        Dim selectedRows = UltraGrid_AllSuspended.Selected.Rows
        UltraPanel_UpdateCost.Location = New Point(195, 158)
        UltraPanel_UpdateCost.Visible = True
        Button_RefreshCost.Enabled = False
        Me.BackgroundWorker1.RunWorkerAsync(selectedRows)

        logger.Debug("Button_RefreshCost_Click exit")
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim poNumber As Integer = Nothing
        Dim userID As Integer = 0
        Dim updateCostRows As SelectedRowsCollection = e.Argument
        Dim bw As BackgroundWorker = DirectCast(sender, BackgroundWorker)

        ' rowCount and cnt are to determine the percentage for progress bar
        Dim rowCount As Integer = updateCostRows.Count
        Dim cnt As Integer = 0

        ' update the cost for each PO selected
        For Each row As UltraGridRow In updateCostRows
            If Not bw.CancellationPending Then
                Try
                    cnt += 1
                    poNumber = row.Cells("OrderHeaderID").Value
                    ' Refresh the Cost
                    OrderingDAO.UpdateOrderRefreshCosts(poNumber, enumRefreshCostSource.SuspendedPOTool.ToString())
                    ' Run suspension logic; send UserID = 0
                    OrderingDAO.CheckOrderSuspension(poNumber, userID)
                    bw.ReportProgress((cnt / rowCount) * 100)
                Catch ex As Exception
                    bw.ReportProgress((cnt / rowCount) * 100)
                End Try
            End If
        Next
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As System.Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ' Update the progress bar
        UltraProgressBar_UpdateCost.Value = e.ProgressPercentage
    End Sub

    Private Sub Button_CancelUpdateCost_Click(sender As System.Object, e As System.EventArgs) Handles Button_CancelUpdateCost.Click
        UltraLabel_UpdateCost.Text = "Cancelling Update..."
        Button_CancelUpdateCost.Enabled = False
        If BackgroundWorker1.IsBusy Then
            ' Cancel BackgroundWorker
            BackgroundWorker1.CancelAsync()
        End If
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As System.Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        ' reset things
        UltraPanel_UpdateCost.Visible = False
        Button_CancelUpdateCost.Enabled = True
        UltraLabel_UpdateCost.Text = "Updating Cost..."
        Button_RefreshCost.Enabled = True
        UltraProgressBar_UpdateCost.Value = 0

        ' Refresh the grid with search criteria
        RefreshData()
        Threading.Thread.Sleep(2500)
    End Sub

    Private Sub TraverseAllRowsAndSelectNonGroupByRows(ByVal rows As RowsCollection)
        ' Loop through every row and only select the NonGroupByRows that are under and expanded GroupByRow
        For Each row As UltraGridRow In rows
            If TypeOf row Is UltraGridGroupByRow Then
                If row.IsExpanded Then
                    For Each childBand As UltraGridChildBand In row.ChildBands
                        ' Recursively loop through child rows
                        Me.TraverseAllRowsAndSelectNonGroupByRows(childBand.Rows)
                    Next
                Else
                    ' If GroupByRow is not exanded, continue to next row
                    Continue For
                End If
            Else
                ' Add row to Selected RowCollection
                Me.UltraGrid_AllSuspended.Selected.Rows.Add(row)
            End If
        Next
    End Sub

    Private Sub ButtonClearSearchFields_Click(sender As System.Object, e As System.EventArgs) Handles ButtonClearSearchFields.Click

        ' Clear TextBoxes.
        UltraNumericEditor_PONum.Value = Nothing
        UltraNumericEditor_ControlGroupID.Value = Nothing
        TextBox_Identifier.Text = String.Empty
        TextBox_VIN.Text = String.Empty
        Text_VendorInvoice.Text = String.Empty
        UltraNumericEditor_VendorID.Value = Nothing
        TextBox_VendorKey.Text = Nothing

        ' Clear ComboBoxes and RadioButtons.
        ClearComboBoxes()
        _optSelection_5.Checked = True

        ' Clear DateTimePickers.
        DateTime_InvDateStart.Value = Nothing
        DateTime_InvDateEnd.Value = Nothing
        DateTime_OrderDateStart.Value = Nothing
        DateTime_OrderDateEnd.Value = Nothing

        ' Clear CheckBoxes.
        CheckBox_eInvoice.Checked = False
        CheckBox_RemediatedSuspensions.Checked = False

    End Sub

    Private Sub SaveDisplayLayout()
        If Not System.IO.Directory.Exists(gridLayoutFilePath) Then
            System.IO.Directory.CreateDirectory(gridLayoutFilePath)
        End If

        Try
            UltraGrid_AllSuspended.DisplayLayout.SaveAsXml(gridLayoutFilePath + gridLayoutFileName)
        Catch ex As Exception
            logger.Debug(String.Format("Error saving UltraGrid layout file: {0}", ex.Message))
        End Try
    End Sub

    Private Sub LoadDisplayLayout()
        If System.IO.File.Exists(gridLayoutFilePath + gridLayoutFileName) Then
            Try
                UltraGrid_AllSuspended.DisplayLayout.LoadFromXml(gridLayoutFilePath + gridLayoutFileName)
            Catch ex As Exception
                logger.Debug(String.Format("Unable to load the UltraGrid layout file.  Proceeding with default layout.  Error: {0}", ex.Message))
            End Try
        End If
    End Sub

    Private Sub SuspendedPOTool_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        ' Save the user's display layout before proceeding (unless the grid is empty).  This will capture the user's Column Chooser preferences.
        If UltraGrid_AllSuspended.Rows.Count > 0 Then
            SaveDisplayLayout()
        End If
    End Sub

End Class