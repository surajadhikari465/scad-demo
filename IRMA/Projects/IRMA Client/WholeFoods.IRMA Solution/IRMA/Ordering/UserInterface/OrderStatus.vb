Option Strict Off
Option Explicit On
Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.Utility.DataAccess
Imports System.ComponentModel
Imports WholeFoods.Utility
Imports System.Net.Mail
Imports System.Text.RegularExpressions


Friend Class frmOrderStatus
    Inherits System.Windows.Forms.Form

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _ApprovalInfo As ApprovalInformationBO = New ApprovalInformationBO()
    Private IsInitializing As Boolean
    Private pbLoading As Boolean
    Private pbDataChanged As Boolean
    Private pbClosedOrder As Boolean
    Private pbApproved As Boolean
    Private pbUploaded As Boolean
    Private pbVendorWFM As Boolean
    Private plStore_No As Integer
    Private plTransfer_SubTeam As Integer
    Private plTransfer_To_SubTeam As Integer
    Private plVendorID As Integer
    Private plReceiveLocationID As Integer
    Private plPurchaseLocationID As Integer
    Private psPS_Vendor_ID As String
    Private psPS_Location_Code As String
    Private psPS_Address_Sequence As String
    Private pcOrderFreight, pcOrderCost, pcInvFrght, pcInvAmount As Decimal
    Private pbTypeClicked As Boolean
    Private pltxtCostDifferenceTop As Integer
    Private plCommandsTop As Integer
    Private plMeHeight As Integer
    Private mlSubteamGLAcctIndex As Integer
    Private InvoiceCostMinusNonAllocatedCharges As Double
    Private pbPayByAgreedCost As Boolean
    Private pbCreditOrder As Boolean
    Private isPrevDayPOReopenAllowed As Boolean
    Private bRefuseReceiving As Boolean
    Private bEinvoiceRequired As Boolean
    Private plReceivingDiscrepancyItemCount As Integer
    Private m_bSent As Boolean
    Private pbDSDOrder As Boolean
    Private dTotalRefused As Double
    Private currencyCulture As CultureInfo
    Private currencyCode As String
    Private invoiceSpecChargesList As New DataTable
    Private Const CLOSING_HELP_NOTE As String = "(NOTE:  The order can be closed even if there is no invoice information.  Simply leave all boxes blank and click this button.)"

    Dim dAllocatedChargesAllowances As Double = 0.0
    Dim dNonAllocatedChargesAllowances As Double = 0.0
    Dim dLineItemChargesAllowances As Double = 0.0
    Dim dChargesOnly As Double = 0.0

    ' Approval form
    Dim WithEvents approvalForm As ApproveInvoice

    Private Shared _partialShippment As Boolean = False
    Public Shared Property PartialShippment() As Boolean
        Get
            Return _partialShippment
        End Get
        Set(ByVal value As Boolean)
            _partialShippment = value
        End Set
    End Property

    Private Shared _EinvoiceId As Integer
    Public Shared Property EInvoiceid() As Integer
        Get
            Return _EinvoiceId
        End Get
        Set(ByVal value As Integer)
            _EinvoiceId = value
        End Set
    End Property

    Private _orderType As enumOrderType
    Public Property OrderType() As enumOrderType
        Get
            Return _orderType
        End Get
        Set(ByVal value As enumOrderType)
            _orderType = value
        End Set
    End Property

    Sub New(ByVal EInvoiceId As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _EinvoiceId = EInvoiceId

    End Sub

    ''' <summary>
    ''' When the form is closed by the user, the application checks to see if the order can be closed.
    ''' If it can, the close is performed automatically.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AutoCloseOrder() As Boolean
        logger.Debug("=> AutoCloseOrder")

        Dim closeSuccess As Boolean = False

        Try
            'TFS#9787
            'If the order is closed and can't be edited, just get out of it.
            If pbClosedOrder Or bRefuseReceiving Then
                closeSuccess = True
                Exit Try
            End If

            'If the order can be closed and is not already and this is not a distribution order
            If (cmdCloseOrder.Enabled AndAlso (Not pbClosedOrder) AndAlso (plTransfer_SubTeam = -1)) And frmOrders.ItemsReceived Then
                'For orders from E-Invoince required vendor, don't force users to close the order before exit the screen.
                If bEinvoiceRequired Then
                    If MsgBox(String.Format(ResourcesOrdering.GetString("msg_ReceivedOrderNotClosed"), Environment.NewLine), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                        logger.Info("AutoCloseOrder - EInvoice required vendor. Item(s) received, but user chose to exist without closing the order: glOrderHeaderID=" + glOrderHeaderID.ToString)
                        closeSuccess = SaveData(False, False)
                    Else
                        logger.Info("AutoCloseOrder - EInvoice required vendor. Item(s) received, and user chose to close the order before exit: glOrderHeaderID=" + glOrderHeaderID.ToString)
                        closeSuccess = False
                    End If
                Else
                    'If any data was entered
                    If (Len(Trim(txtVendorPO.Text)) > 0) OrElse (dtpSearchDate.Value IsNot Nothing) OrElse (CDec(txtTotalInvoiceCost.Text) > 0) Then
                        'Auto-close the order
                        If MsgBox(String.Format(ResourcesOrdering.GetString("msg_autoCloseConfirm"), Environment.NewLine), MsgBoxStyle.Question + MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                            logger.Info("AutoCloseOrder - calling the CloseOrder function to close the order: glOrderHeaderID=" + glOrderHeaderID.ToString)
                            closeSuccess = CloseOrder()
                        Else
                            MsgBox(ResourcesOrdering.GetString("msg_closeOrderBeforeExiting"), MsgBoxStyle.Information, Me.Text)
                            logger.Info("AutoCloseOrder - the user elected not to close the order, they must clear all data before exiting the form: glOrderHeaderID=" + glOrderHeaderID.ToString)
                            closeSuccess = False
                        End If
                    Else
                        'Regular save
                        logger.Info("AutoCloseOrder - no data was entered by the user or all data was cleared; call SaveData to save the OrderHeader, OrderInvoice, and Distribute Invoice Freight without changing the order status: glOrderHeaderID=" + glOrderHeaderID.ToString)
                        closeSuccess = SaveData(False, False)
                    End If
                End If
            Else
                'Regular save
                logger.Info("AutoCloseOrder - the close order button is not enabled AND the order has not been closed AND this is a distribution order; call SaveData to save the OrderHeader, OrderInvoice, and Distribute Invoice Freight values without changing the order status: glOrderHeaderID=" + glOrderHeaderID.ToString + ", plTransfer_SubTeam=" = plTransfer_SubTeam.ToString)
                closeSuccess = SaveData(False, False)
            End If
        Finally
            logger.Debug("AutoCloseOrder exit: closeSuccess=" + closeSuccess.ToString)
        End Try

        Return closeSuccess
    End Function

    ''' <summary>
    ''' Allow the user to move the Refuse Receiving Order to the CLOSED and APPROVED state.
    ''' along with saving off the Reason Code.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RefuseReceivingAndClose(ByVal RefusalID As Integer) As Boolean
        ' Close the order and save the Refuse Receiving Code
        OrderingDAO.RefuseReceivingAndClose(glOrderHeaderID, RefusalID)
        RefuseReceivingAndClose = True
    End Function

    ''' <summary>
    ''' Allow the user to move the order to the CLOSED state.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CloseOrder() As Boolean
        logger.Debug("CloseOrder()")

        '-- Check if ANY received items exist at all
        If Not OrderingDAO.ReceivedItemsExist(glOrderHeaderID) Then
            ' Cannot close PO since no items were received.  Notify the user.
            logger.Info("Cannot close PO since no items were received: OrderHeader_ID=" + glOrderHeaderID.ToString)
            MsgBox(String.Format(ResourcesOrdering.GetString("msg_NoItemsReceived"), Environment.NewLine), MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If

        'Make sure required Sustainability Rankings ok
        If IsSustainabilityRankingRankingCheckOK() = False Then
            Exit Function
        End If

        '-- Make sure that receiving discrepancy reason codes have been entered for any items
        '-- that were received via the handheld and the received qty does not match the einvoice qty
        If plReceivingDiscrepancyItemCount > 0 And PartialShippment = False Then
            logger.Info("CloseOrder exit=False;  There are items that need receiving reason codes entered, OrderHeader_ID=" + glOrderHeaderID.ToString)
            MsgBox(String.Format(ResourcesOrdering.GetString("msg_ReceivingDiscrepanciesExist"), Environment.NewLine, plReceivingDiscrepancyItemCount), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If

        ' Check that all line items have a received quantity, even if that quantity is zero.  Allow an exception for partial shipments.
        If Not OrderingDAO.IsOrderReceivingComplete(glOrderHeaderID) And PartialShippment = False Then
            MsgBox("Not all line items have been received.  Please open the Receiving List and enter a received quantity for each line.", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If

        '-- Make sure everything necessary for AP is entered
        If Not SaveData(True, False) Then
            logger.Debug("CloseOrder exit=False")
            Exit Function
        End If

        ' Close the order
        If OrderingDAO.CloseOrder(glOrderHeaderID) Then
            ' The order was closed, but it is in the suspended state.  Notify the user.
            If PartialShippment = True Then
                MsgBox(String.Format(ResourcesOrdering.GetString("msg_ClosedOrderIsPartialShipment"), Environment.NewLine), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Else
                logger.Info("Order has been closed; it is in the suspended state: OrderHeader_ID=" + glOrderHeaderID.ToString)
                MsgBox(String.Format(ResourcesOrdering.GetString("msg_ClosedOrderIsSuspended"), Environment.NewLine), MsgBoxStyle.OkOnly, Me.Text)
            End If
        End If

        CloseOrder = True
    End Function

    Private Sub ApproveOrder()
        Select Case _ApprovalInfo.ApprovalType
            Case ApprovalInformationBO.ApprovalTypeEnum.PayByInvoice
                OrderingDAO.ApproveInvoiceUsingPayByInvoiceOption(glOrderHeaderID, _ApprovalInfo.Resolution.ResolutionCodeId)
            Case ApprovalInformationBO.ApprovalTypeEnum.PayByPO
                OrderingDAO.ApproveInvoiceUsingPayByPurchaseOrderOption(glOrderHeaderID, _ApprovalInfo.Resolution.ResolutionCodeId)
            Case ApprovalInformationBO.ApprovalTypeEnum.None
                If MsgBox(String.Format(ResourcesOrdering.GetString("msg_ApproveWithDocumentData"), Environment.NewLine), MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                    OrderingDAO.ApproveInvoiceUsingDocumentDataOption(glOrderHeaderID, _ApprovalInfo.Resolution.ResolutionCodeId)
                End If
        End Select

        ' The approval has been processed.  Refresh the form data.
        RefreshData()
    End Sub

    ''' <summary>
    ''' The user has elected to close this order.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdCloseOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCloseOrder.Click
        logger.Debug("cmdCloseOrder_Click entry")

        ' Before saving, verify the order state has not changed between the time the form was loaded and the save request.
        ' This check was added to the V3 release since the user can now close orders using the control group functionality.
        If Not OrderingDAO.IsOrderStateCurrent(pbClosedOrder, pbApproved, pbUploaded, glOrderHeaderID) Then
            logger.Info("cmdCloseOrder_Click - unable to process the close order request because the order state on the form is out of sync with the database order state.  The user is notified, and the form is being reloaded with current data. OrderHeader_ID=" + glOrderHeaderID.ToString)
            MsgBox(String.Format(ResourcesOrdering.GetString("msg_DataOutOfDate"), Environment.NewLine), MsgBoxStyle.Information, Me.Text)
            RefreshData()
        Else
            ' Verify the order has not already been closed.
            If Not pbClosedOrder Then
                ' Confirm date is not in the past year as this causes issues to metrics
                If dtpSearchDate.DateTime.Year < DateTime.Now.Year AndAlso MsgBox("You are attempting to close an invoice with a year other than the current year. Is the year accurate?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Verify Close Order") = MsgBoxResult.No Then
                    Exit Sub
                End If

                ' Confirm the closure
                If MsgBox(ResourcesOrdering.GetString("msg_ConfirmCloseOrder"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Verify Close Order") = MsgBoxResult.Yes Then
                    logger.Info("cmdCloseOrder_Click - the user confirmed to close the order: OrderHeader_ID=" + glOrderHeaderID.ToString)
                    If CloseOrder() Then
                        ' The close was a success; refresh the UI.
                        RefreshData()
                    End If
                End If
            Else
                ' The order has already been closed.  Prompt the user to re-open the order.
                ' This can only be done if the order has not been uploaded to PeopleSoft.
                If MsgBox(ResourcesOrdering.GetString("msg_ConfirmReOpenOrder"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Verify Re-Open Order") = MsgBoxResult.Yes Then
                    '-- Reopen the order
                    logger.Info("cmdCloseOrder_Click - the user confirmed to re-open a closed order: OrderHeader_ID=" + glOrderHeaderID.ToString)
                    If Not OrderingDAO.ReOpenClosedOrder(glOrderHeaderID) Then
                        ' Let the user know the order was not reopened because it has already been uploaded to PeopleSoft.
                        logger.Info("cmdCloseOrder_Click - the order could not be re-opened because it has alredy been uploaded to PeopleSoft: OrderHeader_ID=" + glOrderHeaderID.ToString)
                        MsgBox(ResourcesOrdering.GetString("msg_CannotReOpenUploadedOrder"), MsgBoxStyle.Information, Me.Text)
                    Else
                        ' The reopen was a success; refresh the UI.
                        RefreshData()
                    End If
                End If
            End If
        End If

        logger.Debug("cmdCloseOrder_Click exit")
    End Sub

    ''' <summary>
    ''' The user is exiting the form.  The form closing steps are performed, which include trying to 
    ''' automatically close the order.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click entry")
        Me.Close()
        logger.Debug("cmdExit_Click exit")
    End Sub

    ''' <summary>
    ''' The form is closing, which includes auto-closure of the order.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmOrderStatus_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("OrderStatus Closing")
        e.Cancel = Not AutoCloseOrder()
    End Sub

    Private Sub FormatGrid()
        With UltraGrid_Charges.DisplayLayout.Bands(0)
            .Columns("Type").Width = 75
            .Columns("GLAccount").Width = 70
            .Columns("Description").Width = 211
            .Columns("Value").Width = 65
            .Columns("Value").Format = "C"
            .Columns("Value").FormatInfo = currencyCulture
        End With
    End Sub

    Private Sub frmOrderStatus_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("OrderStatus Loading...")

        ' Set up currencies and format the charge labels at the bottom of the form.
        LoadCurrency(cmbCurrency)
        currencyCode = DetermineOrderCurrency(glOrderHeaderID)
        currencyCulture = CultureInfo.CreateSpecificCulture(CurrencyCultureMapping.Item(currencyCode))

        If InstanceDataDAO.IsFlagActive("MultipleCurrencies") Then
            If Not PrimaryCurrency.Contains(currencyCode) Then
                btnCurrency.Visible = True
                btnCurrency.Enabled = (gbSuperUser Or gbPOAccountant) And (Not pbUploaded)
            End If
        End If

        Label_AllocatedCharges.Text = String.Format(currencyCulture, "{0:C}", 0.0)
        Label_NonAllocCharges.Text = String.Format(currencyCulture, "{0:C}", 0.0)
        Label_LineItemCharges.Text = String.Format(currencyCulture, "{0:C}", 0.0)

        ' Get app-setting that controls whether or not a receiver can unsuspend POs for a previous day.  Defaults to "NO/FALSE" if app-setting does not exist, to keep behavior consistent with previous version.
        Try
            isPrevDayPOReopenAllowed = CBool(ConfigurationServices.AppSettings("ReceiverCanReopenHistoricalSuspendedPOs"))
        Catch ex As Exception
            logger.Warn("App-setting not found or invalid, so not allowing receivers to re-open historical suspended POs.")
            isPrevDayPOReopenAllowed = False
        End Try

        ' Match order e-invoice button.
        If _EinvoiceId > 0 Then
            cmdDisplayEInvoice.Enabled = True
        Else
            cmdDisplayEInvoice.Enabled = False
        End If

        If frmOrders.ItemsReceived Then

            ' make grid read ony.
            UltraGrid_Charges.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False

            pltxtCostDifferenceTop = txtCostDifference.Top
            plCommandsTop = cmdCloseOrder.Top
            plMeHeight = Me.Height

            RefreshData()

            txtTotalInvoiceCost.Visible = False

            ' TFS 8325, 02/28/2013, Faisal Ahmed, Refusal functionality
            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
            'If IsRefusalAllowed(glOrderHeaderID) And Not pbDSDOrder Then
            '    cmdItemsRefused.Enabled = True
            'Else
            '    cmdItemsRefused.Enabled = False
            'End If

            CenterForm(Me)

            ' Populate Subteam/GLAcct combo box for SAC.
            LoadSubTeamGLAcct(glOrderHeaderID)

            ' Populate Subteam/GLAcct Charges check list for SAC.
            LoadOrderInvoiceSpecCharges(glOrderHeaderID)

            pbLoading = False

            ' make SubTeam Read-Only
            _txtInvoiceCost_0.Enabled = False

            ' Check sustainability Ranking and notify
            IsSustainabilityRankingRankingCheckOK()

            'TFS 2455 - Modify receiving for Einvoice Required (DSD) Vendors
            'TFS 5584 - DBS - check if order is credit along w/other einvoicing control disabling that goes on
            If ((bEinvoiceRequired And Not pbCreditOrder) And _EinvoiceId < 0 And (Not gbEInvoicing)) Or PartialShippment Then

                'Close the PO with document type of NONE
                _optType_2.Checked = True

                'Disable the other two options for the document type per TFS 3124
                _optType_0.Enabled = False
                _optType_1.Enabled = False
            End If
        Else
            'TFS 2460, Refuse Receiving Enhancement 
            'TFS 4782 - Disable Refuse Receiving button if PO was not Sent
            'm_rsOrder = SQLOpenRecordSet("GetOrderInfo " & glOrderHeaderID & ", 0", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            'm_rsOrder.Close()

            LoadReasonCodesUltraCombo(ucReasonCode, enumReasonCodeType.RR)
            RefreshData()

            ' If an eInvoice is loaded, default to the Invoice tab.  If the user does not have eInvoicing permissions, then disallow access to the Other or None tabs.
            If _EinvoiceId > 0 Then
                _optType_0.Checked = True
                If Not gbEInvoicing Then
                    _optType_1.Enabled = False
                    _optType_2.Enabled = False
                End If
            Else
                _optType_2.Checked = True
            End If
        End If

        ' Add invoice method to window title.
        Me.Text += " " & frmOrders.GetInvoiceMethodDisplayText(glOrderHeaderID, pbPayByAgreedCost, _EinvoiceId)

        'TFS 10824, Faisal Ahmed, 02/08/2013 - Selects document type = NONE if einvoice is not loaded for DSD orders
        If pbDSDOrder And Not (EInvoiceid > 0) Then
            _optType_2.Checked = True
        End If

        Global.SetUltraGridSelectionStyle(UltraGrid_Charges)
    End Sub

    ''' <summary>
    ''' Reload the OrderStatus UI form with the current data from the database.  The buttons on the form
    ''' are enabled/disabled based on the current state of the order and the user role.
    ''' SENT orders - all data can be changed; only the close order button is enabled
    ''' CLOSED orders - data can't be changed; only the close order button is enabled to allow the user to re-open the order
    ''' APPROVED orders - data can't be changed; only the close order button is enabled to allow the user to re-open the order
    ''' SUSPENDED orders - data can't be changed;  the close order button is enabled to allow the user to re-open the order 
    '''               and the approved button is enabled to allow the user to approve a suspended order
    ''' UPLOADED orders - data can't be changed; no buttons are enabled   
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshData()
        logger.Debug("RefreshData()")
        Dim rsOrder As DAO.Recordset = Nothing
        Dim X As Integer
        Dim dtCloseDate As Date

        pbLoading = True

        pcOrderFreight = 0
        pcOrderCost = 0
        pcInvFrght = 0
        pcInvAmount = 0

        txtInvoiceCost(0).Text = ""
        lblSubTeam(0).Text = ""
        lblSubTeam(0).Tag = ""

        '-- Unload Extra textboxes, in case it changed
        For X = txtInvoiceCost.UBound To (txtInvoiceCost.LBound + 1) Step -1
            txtInvoiceCost.Unload(X)
            lblSubTeam.Unload(X)
        Next X

        Try
            '-- Retreive Order Info
            logger.Info("Calling GetOrderStatus to populate the OrderStatus.vb form: OrderHeader_ID=" + glOrderHeaderID.ToString)
            rsOrder = SQLOpenRecordSet("GetOrderStatus " & glOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            pbClosedOrder = Not IsDBNull(rsOrder.Fields("CloseDate").Value)

            If Not IsDBNull(rsOrder.Fields("CloseDate").Value) Then
                dtCloseDate = rsOrder.Fields("CloseDate").Value
            Else
                dtCloseDate = #12:00:00 PM#
            End If

            If (pbTypeClicked AndAlso optType(1).Checked) OrElse ((Not pbTypeClicked) AndAlso (rsOrder.Fields("VendorDoc_ID").Value & "" <> "")) Then

                optType(1).Checked = True
                lblDocNo.Text = "Document #"
                txtVendorPO.Text = rsOrder.Fields("VendorDoc_ID").Value & ""
                dtpSearchDate.Value = rsOrder.Fields("VendorDocDate").Value

                If dtpSearchDate.Value > SystemDateTime(True) And Not pbClosedOrder Then
                    MsgBox("The document associated with this order has an invalid future document date.  The document date will be reset to today's date.", MsgBoxStyle.Exclamation, "Invalid Date")
                    dtpSearchDate.Value = SystemDateTime(True)
                End If

                txtInvTot.Text = ""
                SetActive(txtInvTot, False)

            ElseIf (pbTypeClicked AndAlso _optType_2.Checked) _
                            OrElse ((Not pbTypeClicked) AndAlso (rsOrder.Fields("VendorDoc_ID").Value & "" = "") _
                            AndAlso (rsOrder.Fields("InvoiceNumber").Value & "" = "") _
                            AndAlso (rsOrder.Fields("CloseDate").Value & "" <> "")) OrElse PartialShippment Then
                _optType_2.Checked = True

            Else

                optType(0).Checked = True
                lblDocNo.Text = "Invoice #"

                txtVendorPO.Text = rsOrder.Fields("InvoiceNumber").Value & ""
                dtpSearchDate.Value = rsOrder.Fields("InvoiceDate").Value

                If dtpSearchDate.Value > SystemDateTime(True) And Not pbClosedOrder Then
                    MsgBox("The invoice associated with this order has an invalid future invoice date.  The invoice date will be reset to today's date.", MsgBoxStyle.Exclamation, "Invalid Date")
                    dtpSearchDate.Value = SystemDateTime(True)
                End If
            End If

            pbUploaded = Not (IsDBNull(rsOrder.Fields("UploadedDate").Value) And IsDBNull(rsOrder.Fields("AccountingUploadDate").Value))
            pbApproved = Not IsDBNull(rsOrder.Fields("ApprovedDate").Value)
            pbVendorWFM = rsOrder.Fields("WFM").Value
            plVendorID = rsOrder.Fields("Vendor_ID").Value
            plReceiveLocationID = rsOrder.Fields("ReceiveLocation_ID").Value
            plPurchaseLocationID = rsOrder.Fields("PurchaseLocation_ID").Value

            If (IsDBNull(rsOrder.Fields("Store_No").Value)) Then
                plStore_No = -1
            Else
                plStore_No = rsOrder.Fields("Store_No").Value
            End If

            If (IsDBNull(rsOrder.Fields("Transfer_SubTeam").Value)) Then
                plTransfer_SubTeam = -1
            Else
                plTransfer_SubTeam = rsOrder.Fields("Transfer_SubTeam").Value
            End If

            If IsDBNull(rsOrder.Fields("Transfer_To_SubTeam").Value) Then
                plTransfer_To_SubTeam = -1
            Else
                plTransfer_To_SubTeam = rsOrder.Fields("Transfer_To_SubTeam").Value
            End If

            psPS_Vendor_ID = rsOrder.Fields("PS_Vendor_ID").Value & ""
            psPS_Location_Code = rsOrder.Fields("PS_Location_Code").Value & ""
            psPS_Address_Sequence = rsOrder.Fields("PS_Address_Sequence").Value & ""
            pbPayByAgreedCost = rsOrder.Fields("PayByAgreedCost").Value
            SetCombo(cmbCurrency, rsOrder.Fields("CurrencyID").Value)
            ucReasonCode.Value = rsOrder.Fields("RefuseReceivingReasonID").Value
            bEinvoiceRequired = rsOrder.Fields("EinvoiceRequired").Value

            If IsDBNull(rsOrder.Fields("Einvoice_Id").Value) Then
                _EinvoiceId = -1
            Else
                _EinvoiceId = rsOrder.Fields("Einvoice_Id").Value
            End If

            plReceivingDiscrepancyItemCount = rsOrder.Fields("ReceivingDiscrepancyItemCount").Value
            pbCreditOrder = rsOrder.Fields("Return_Order").Value
            pbDSDOrder = rsOrder.Fields("DSDOrder").Value

            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
            'dTotalRefused = rsOrder.Fields("TotalRefused").Value

        Finally
            If rsOrder IsNot Nothing Then
                rsOrder.Close()
                rsOrder = Nothing
            End If
        End Try

        '-- Set up our department list
        SetActive(txtInvoiceCost(0), False)

        '-- Get the Invoice info if this is this is an invoice
        If optType(0).Checked Then
            logger.DebugFormat("GetOrderInvoice({0})", glOrderHeaderID.ToString)

            SQLOpenRS(rsOrder, "EXEC GetOrderInvoice " & glOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            ' NOTE: Orders are no longer able to be created with more than one subteam, but the UI and database structure has not
            ' been updated to reflect that business rule.  There should only be one subteam for an order, which means there should
            ' only be one invoice assigned to the order.
            While Not rsOrder.EOF
                If X > 0 Then
                    txtInvoiceCost.Load(X)
                    lblSubTeam.Load(X)

                    txtInvoiceCost(X).Top = txtInvoiceCost(X - 1).Top + txtInvoiceCost(X).Height
                    lblSubTeam(X).Top = txtInvoiceCost(X).Top
                End If

                'User can only change invoice information on the Invoice/Document Data screen when closing a credit PO for an Einvoice required vendor.
                'The invoice information cannot be changed on this screen on a debit order for an Einvoice required vendor except the user has an eInvoicing role.
                SetActive(txtInvoiceCost(X), (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM) AndAlso (gbEInvoicing Or pbCreditOrder Or Not bEinvoiceRequired))
                SetActive(lblSubTeam(X), True)

                lblSubTeam(X).Tag = rsOrder.Fields("SubTeam_No").Value
                lblSubTeam(X).Text = CStr(rsOrder.Fields("SubTeam_Name").Value).Trim()

                pcOrderCost = rsOrder.Fields("OrderCost").Value
                pcInvFrght = pcInvFrght + rsOrder.Fields("InvoiceFreight").Value

                ' If a distribution or transfer order, the order cost = invoice cost
                If geOrderType = enumOrderType.Distribution Or geOrderType = enumOrderType.Transfer Then
                    If Not pbClosedOrder Then
                        txtInvoiceCost(X).Text = VB6.Format(rsOrder.Fields("OrderCost").Value, "#####0.00##")
                        pcInvAmount = pcInvFrght + rsOrder.Fields("OrderCost").Value
                    Else
                        txtInvoiceCost(X).Text = VB6.Format("0", "#####0.00##")
                    End If
                Else
                    txtInvoiceCost(X).Text = VB6.Format(rsOrder.Fields("InvoiceCost").Value, "#####0.00##")
                    pcInvAmount = pcInvFrght + rsOrder.Fields("InvoiceCost").Value
                End If

                X = X + 1
                rsOrder.MoveNext()
            End While
            rsOrder.Close()

            LoadOrderInvoiceSpecCharges(glOrderHeaderID)

            ' if an einvoice, add NonAllocated Charges back in. For EInvoices, OrderInvoice.InvoiceCost = Cost - NonAllocated Charges. hack to make PeopleSoft work.
            pcInvAmount = pcInvAmount + dNonAllocatedChargesAllowances
            txtInvTot.Text = VB6.Format(pcInvAmount, "#####0.00##")
            SetActive(txtInvTot, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM) AndAlso (gbEInvoicing Or pbCreditOrder Or Not bEinvoiceRequired))

        End If

        logger.DebugFormat("OrderHeader_ID: [{0}] Closed: [{1}] Approved: [{2}] Suspended: [{3}] Uploaded: [{4}]", glOrderHeaderID.ToString, pbClosedOrder.ToString, pbApproved.ToString, (pbClosedOrder AndAlso Not pbApproved).ToString, pbUploaded.ToString)
        ' NOTE: It is important that data on the form can only be changed if the order is in the SENT state (NOT pbClosedOrder)
        ' to ensure that the three way matching processing works as designed.

        'Also, user can only change invoice information on the Invoice/Document Data screen when closing a credit PO for an Einvoice required vendor.
        'The invoice information cannot be changed on this screen on a debit order for an Einvoice required vendor.

        SetActive(optType(0), (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM) AndAlso (gbEInvoicing Or pbCreditOrder Or Not bEinvoiceRequired))
        SetActive(optType(1), (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM) AndAlso (gbEInvoicing Or pbCreditOrder Or Not bEinvoiceRequired))
        SetActive(_optType_2, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM) AndAlso (gbEInvoicing Or pbCreditOrder Or Not bEinvoiceRequired))
        SetActive(txtVendorPO, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM) AndAlso (gbEInvoicing Or pbCreditOrder Or Not bEinvoiceRequired))
        SetActive(dtpSearchDate, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM) AndAlso (gbEInvoicing Or pbCreditOrder Or Not bEinvoiceRequired))
        SetActive(UltraNumericEditor_SpecCharges, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM))
        SetActive(cboSubteamGLAcct, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM))
        SetActive(btnAddSubteamGLAcct, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM))
        SetActive(btnRemoveSubteamGLAcct, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM))
        SetActive(Button_AddCharges, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM))
        SetActive(Button_RemoveCharges, (glSubTeamNo = -1) AndAlso (Not pbClosedOrder) AndAlso (Not pbVendorWFM))
        SetActive(txtTotalInvoiceCost, False, True)
        SetActive(txtCostDifference, False)

        ' The close order button is enabled when the order state is SENT (the order has not been CLOSED yet)
        ' OR the user is a PO_Accountant and the order has not been UPLOADED yet, allowing these users to re-open a CLOSED, SUSPENDED, or APPROVED order
        ' OR the user is a receiver and the app-setting to allow receivers to reopen suspended POs prior to today is TRUE (TFS 2141)
        ' OR the order close date is today and the order has not been UPLOADED yet
        SetActive(cmdCloseOrder,
                    ((gbDistributor AndAlso (glStore_Limit = 0 OrElse glStore_Limit = plStore_No)) _
                            OrElse gbCoordinator _
                            OrElse gbAccountant) _
                    AndAlso ((Not pbClosedOrder) _
                                OrElse gbPOAccountant _
                                OrElse (gbDistributor AndAlso isPrevDayPOReopenAllowed) _
                                OrElse DateDiff(Microsoft.VisualBasic.DateInterval.Day, dtCloseDate, SystemDateTime) = 0
                            ) _
                    AndAlso (Not pbUploaded) _
                    AndAlso (IsNothing(ucReasonCode.Value)))

        SetActive(lblReasonCode,
                  ((gbDistributor AndAlso (glStore_Limit = 0 OrElse glStore_Limit = plStore_No) _
                    OrElse gbSuperUser OrElse gbCoordinator) _
                    AndAlso Not frmOrders.ItemsReceived _
                    AndAlso frmOrders.Sent _
                    AndAlso Not pbClosedOrder))

        SetActive(ucReasonCode,
                  ((gbDistributor AndAlso (glStore_Limit = 0 OrElse glStore_Limit = plStore_No) _
                    OrElse gbSuperUser OrElse gbCoordinator) _
                    AndAlso Not frmOrders.ItemsReceived _
                    AndAlso frmOrders.Sent _
                    AndAlso Not pbClosedOrder))

        SetActive(btnRefuseReceiving,
                  ((gbDistributor AndAlso (glStore_Limit = 0 OrElse glStore_Limit = plStore_No) _
                    OrElse gbSuperUser OrElse gbCoordinator) _
                    AndAlso Not frmOrders.ItemsReceived _
                    AndAlso frmOrders.Sent _
                    AndAlso Not pbClosedOrder))

        'Make sure bottom controls tab indices are at the end
        cmdCloseOrder.TabIndex = 99
        cmdExit.TabIndex = 99

        '-- Show totals/difference
        CalculateTotals()

        If Not pbClosedOrder Then
            'TFS 13247  Edits to Invoice/Document Data screen cause Close Order button mouseover oddness
            ToolTip1.SetToolTip(cmdCloseOrder, "Close Order" & " " & CLOSING_HELP_NOTE)
        End If

        pbLoading = False

        ' Reset the data changes flag since the screen has been refreshed from the database.
        pbDataChanged = False

        ' If "NONE" is selected. Disable All Data Entry.
        If _optType_2.Checked Then
            SetActive(txtInvoiceCost(0), False)
            SetActive(lblSubTeam(0), False)
            SetActive(txtInvTot, False)

            ' TFS#10642 Invoice type of "none" is not clearing the invoice total field
            txtInvTot.Text = String.Empty

            SetActive(txtVendorPO, False)
            SetActive(dtpSearchDate, False)
            SetActive(UltraNumericEditor_SpecCharges, False)
            SetActive(cboSubteamGLAcct, False)
            SetActive(btnAddSubteamGLAcct, False)
            SetActive(btnRemoveSubteamGLAcct, False)
            SetActive(Button_AddCharges, False)
            SetActive(Button_RemoveCharges, False)
            SetActive(txtTotalInvoiceCost, False, True)
            SetActive(txtCostDifference, False)
        End If

        If gbBuyer And Not (gbSuperUser OrElse gbPOAccountant OrElse gbCoordinator OrElse gbDistributor) Then
            SetActive(txtVendorPO, False)
            SetActive(dtpSearchDate, False)
            SetActive(txtInvTot, False)
            UltraGrid_Charges.Enabled = False
            Button_AddCharges.Enabled = False
            Button_RemoveCharges.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' Set up Special Charges (subteam) Combo Box
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadSubTeamGLAcct(ByVal OrderHeader_Id As Integer)
        Dim subteamDAO As New OrderingDAO
        Dim subteamList As BindingList(Of SubTeamBO) = OrderingDAO.LoadSubTeamGLAcct(OrderHeader_Id)

        cboSubteamGLAcct.DataSource = subteamList
        cboSubteamGLAcct.ValueMember = "SubTeamNo"
        cboSubteamGLAcct.DisplayMember = "SubTeamName"
    End Sub

    Private Sub ClearInvoiceFields()
        txtVendorPO.Text = String.Empty
        dtpSearchDate.Value = Nothing
    End Sub

    ''' <summary>
    ''' Save the updates to the OrderHeader, OrderInvoice, and OrderItem tables based on the current data on the
    ''' UI form.  The order must be in the SENT state for the save to succeed (OrderHeader.SENT = TRUE and 
    ''' OrderHeader.CloseDate IS NULL).
    ''' This save does not close the order, it only updates the invoice or document data and distributes the invoice
    ''' freight among the order items.
    ''' </summary>
    ''' <param name="bClosing">when TRUE, business rule validation is performed before saving the data</param>
    ''' <param name="bApproving">when TRUE, business rule validation is performed before saving the data</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveData(ByRef bClosing As Boolean, ByRef bApproving As Boolean) As Boolean
        logger.DebugFormat("SaveData({0},{1})", bClosing.ToString, bApproving.ToString)

        Dim X As Integer
        Dim rsResult As DAO.Recordset = Nothing
        Dim sInvoiceNumberUse As String
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        sInvoiceNumberUse = String.Empty

        ' Allow only positive value for Invoice amount
        If Trim(txtInvTot.Text) <> "" Then
            If CInt(txtInvTot.Text) < 0 Then
                MsgBox("Invoice Amount can not be negative. Try again.")
                txtInvTot.Focus()
                Exit Function
            End If
        End If

        ' -- Show totals/difference
        CalculateTotals()

        ' If this is not a distribution order and closing or approving the order
        'TFS Bug 13615 - If a vendor has Vendor.WFM flag set to true then this screen is disabled as there is no invoice info.  
        '                A condition was added so that the validation is skipped if pbVendorWFM is true.
        If plTransfer_SubTeam = -1 AndAlso (bClosing OrElse bApproving) And Not pbVendorWFM Then
            logger.DebugFormat("Validating Invoice Data => plTransfer_SubTeam: [{0}] bClosing: [{1}] bApproving: [{2}]", plTransfer_SubTeam.ToString, bClosing.ToString, bApproving.ToString)
            If Trim(txtVendorPO.Text) = "" And Not _optType_2.Checked Then
                MsgBox("Please enter the " + lblDocNo.Text + ".", MsgBoxStyle.Information, Me.Text)
                txtVendorPO.Focus()
                SaveData = False
                Exit Function
            Else
                If optType(0).Checked Then 'Invoice
                    Try
                        'Invoice Number must be unique
                        rsResult = SQLOpenRecordSet("EXEC GetInvoiceNumberUse " & CStr(plVendorID) & ",'" & Trim(txtVendorPO.Text) & "'," & CStr(glOrderHeaderID), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        While Not rsResult.EOF
                            sInvoiceNumberUse = sInvoiceNumberUse & rsResult.Fields("OrderHeader_ID").Value & vbCrLf
                            rsResult.MoveNext()
                        End While
                    Finally
                        If rsResult IsNot Nothing Then
                            rsResult.Close()
                            rsResult = Nothing
                        End If
                    End Try
                    If Len(sInvoiceNumberUse) > 0 Then
                        logger.Info("SaveData - invoice number is already in use for another PO so the order cannot be closed or approved: sInvoiceNumberUse (PO Number)=" + sInvoiceNumberUse)
                        MsgBox(String.Format(ResourcesOrdering.GetString("msg_InvoiceNumUsedByAnotherPO"), Environment.NewLine, sInvoiceNumberUse), MsgBoxStyle.Information, Me.Text)
                        txtVendorPO.Focus()
                        SaveData = False
                        Exit Function
                    End If
                End If
            End If

            If dtpSearchDate.Value Is Nothing And Not _optType_2.Checked Then
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), _lblLabel_16.Text.Replace(":", "")), MsgBoxStyle.Information, Me.Text)
                dtpSearchDate.Focus()
                SaveData = False
                Exit Function
            ElseIf Not dtpSearchDate.IsDateValid And Not _optType_2.Checked Then
                MsgBox(ResourcesIRMA.GetString("DateInvalid"), MsgBoxStyle.Information, Me.Text)
                dtpSearchDate.Focus()
                SaveData = False
                Exit Function
            End If

            If bApproving AndAlso Not optType(0).Checked Then
                MsgBox(ResourcesOrdering.GetString("msg_InvoiceRequiredForApproval"), MsgBoxStyle.Information, Me.Text)
                optType(1).Focus()
                SaveData = False
                logger.Debug("SaveData exit: SaveData=False")
                Exit Function
            End If

            'If this is invoice data
            If optType(0).Checked Then
                If CDec(txtTotalInvoiceCost.Text) = 0 Then
                    If bClosing Then
                        If MsgBox(String.Format(ResourcesOrdering.GetString("msg_ConfirmClosingWithZeroCost"), Environment.NewLine), MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Close Order") = MsgBoxResult.No Then
                            txtInvoiceCost(0).Focus()
                            SaveData = False

                            Exit Function
                        End If
                    Else
                        MsgBox(ResourcesOrdering.GetString("msg_CannotApproveWithZeroCost"), MsgBoxStyle.Information, Me.Text)
                        txtInvoiceCost(0).Focus()
                        SaveData = False
                        Exit Function
                    End If
                End If
            End If
        End If

        ' Before saving, verify the order state has not changed between the time the form was loaded and the save request.
        ' This check was added to the V3 release since the user can now close orders using the control group functionality.
        If Not OrderingDAO.IsOrderStateCurrent(pbClosedOrder, pbApproved, pbUploaded, glOrderHeaderID) Then
            logger.Info("SaveData - unable to process the save request because the order state on the form is out of sync with the database order state.  The user is notified, and the form is being reloaded with current data.")
            MsgBox(String.Format(ResourcesOrdering.GetString("msg_DataOutOfDate"), Environment.NewLine), MsgBoxStyle.Information, Me.Text)
            RefreshData()
            SaveData = False
            Exit Function
        End If

        ' Create a new business object for the save to OrderHeader
        Dim currentOrderInvoice As New OrderHeaderInvoiceBO()
        currentOrderInvoice.OrderHeaderID = glOrderHeaderID

        If optType(0).Checked Then
            ' Changed the following date = nothing to fix the bug 5691.
            If dtpSearchDate.Value = Nothing Then
                logger.Info("SaveData - updating the OrderHeader table with an invoice: OrderHeader_ID=" + glOrderHeaderID.ToString + ", InvoiceNumber=" + txtVendorPO.Text + ", InvoiceDate=NONE")
            Else
                logger.Info("SaveData - updating the OrderHeader table with an invoice: OrderHeader_ID=" + glOrderHeaderID.ToString + ", InvoiceNumber=" + txtVendorPO.Text + ", InvoiceDate=" + dtpSearchDate.Value.ToString)
            End If

            currentOrderInvoice.InvoiceNumber = txtVendorPO.Text
            currentOrderInvoice.InvoiceDate = dtpSearchDate.Value
            currentOrderInvoice.VendorDocId = Nothing
            currentOrderInvoice.VendorDocDate = Nothing
        Else
            ' Changed the following date = nothing to fix the bug 5691.
            If dtpSearchDate.Value = Nothing Then
                logger.Info("SaveData - updating the OrderHeader table with a document (not invoice): OrderHeader_ID=" + glOrderHeaderID.ToString)
            Else
                logger.Info("SaveData - updating the OrderHeader table with a document (not invoice): OrderHeader_ID=" + glOrderHeaderID.ToString + ", VendorDocDate=" + dtpSearchDate.Value.ToString)
            End If

            currentOrderInvoice.InvoiceNumber = Nothing
            currentOrderInvoice.InvoiceDate = Nothing
            currentOrderInvoice.VendorDocId = txtVendorPO.Text
            currentOrderInvoice.VendorDocDate = dtpSearchDate.Value
        End If

        currentOrderInvoice.PartialShippment = PartialShippment

        'BR 12/30/09 added try/catch block to prevent transactions from staying open after errors occur
        ' Start the database transaction
        logger.Info("SaveData - BEGIN database transaction")

        Dim transaction As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

        Try
            logger.Info("SaveData - updating the OrderHeader table with invoice or document data: OrderHeader_ID=" + currentOrderInvoice.OrderHeaderID.ToString)
            If Not OrderingDAO.UpdateOrderHeaderInvoiceData(currentOrderInvoice, transaction) Then
                ' the save did not succeed because of order state business rules
                logger.Info("SaveData - unable to process the save request because the OrderHeader.CloseDate IS NOT NULL.  The user is notified, and the form is being reloaded with current data.")
                MsgBox(String.Format(ResourcesOrdering.GetString("msg_DataOutOfDate"), Environment.NewLine), MsgBoxStyle.Information, Me.Text)
                RefreshData()
                SaveData = False
                logger.Debug("SaveData exit: SaveData=False")
                Exit Function
            Else
                logger.Info("SaveData - deleting existing OrderInvoice records for the order: OrderHeader_ID=" + glOrderHeaderID.ToString)
                OrderingDAO.DeleteOrderInvoices(glOrderHeaderID, transaction)

                ' NOTE: Orders are no longer able to be created with more than one subteam, but the UI and database structure has not
                ' been updated to reflect that business rule.  There should only be one subteam for an orders.
                For X = lblSubTeam.LBound To lblSubTeam.UBound
                    If lblSubTeam(X).Tag.ToString() <> String.Empty Then
                        logger.Info("SaveData - inserting a new OrderInvoice record for the order: OrderHeader_ID=" + glOrderHeaderID.ToString + ", SubTeam_No=" + lblSubTeam(X).Tag.ToString)
                        OrderingDAO.CreateOrderInvoice(glOrderHeaderID, CInt(lblSubTeam(X).Tag), InvoiceCostMinusNonAllocatedCharges.ToString(), transaction)
                    End If
                Next X
            End If

            If cmbCurrency.Enabled = True Then
                OrderingDAO.UpdateOrderCurrency(glOrderHeaderID, ComboValue(cmbCurrency), transaction)
            End If

            logger.Info("SaveData - COMMIT database transaction")
            transaction.Commit()

        Catch ex As Exception
            logger.Info("SaveData - ROLLBACK Error occured on database transaction - " & ex.Message)
            transaction.Rollback()

            MsgBox("An error has occured while saving the data.  The order has NOT been closed.  Please try again.", MsgBoxStyle.Information, Me.Text)
            SaveData = False
            Exit Function
        End Try

        pbDataChanged = False

        '20080816 - Update Allocation and Line Item Costs due to EInvoicing
        If Not _optType_2.Checked Then OrderingDAO.UpdateAllocationLineItemCharge(glOrderHeaderID)

        logger.Debug("SaveData exit: SaveData=True")
        SaveData = True
    End Function

    Public Sub LoadOrderInvoiceSpecCharges(ByVal OrderHeader_Id As Integer)

        Dim dt As DataTable = OrderingDAO.LoadOrderInvoiceSpecChargeTable(OrderHeader_Id)

        UltraGrid_Charges.DataSource = dt
        UltraGrid_Charges.DataBind()
        FormatGrid()

        dAllocatedChargesAllowances = 0
        dNonAllocatedChargesAllowances = 0
        dLineItemChargesAllowances = 0

        If UltraGrid_Charges.Rows.Count > 0 Then
            For Each ugRow As Infragistics.Win.UltraWinGrid.UltraGridRow In UltraGrid_Charges.Rows
                Select Case ugRow.Cells("type").Value.ToString().ToLower()
                    Case "allocated"
                        'DaveStacey 20110326 TFS 1619 - Added Case statement to no longer include allocated allowances in charges total
                        Select Case ugRow.Cells("IsAllowance").Value.ToString().ToLower()
                            Case "false", "0"
                                dAllocatedChargesAllowances = dAllocatedChargesAllowances + CType(ugRow.Cells("value").Value, Double)
                        End Select
                    Case "not allocated"
                        dNonAllocatedChargesAllowances = dNonAllocatedChargesAllowances + CType(ugRow.Cells("value").Value, Double)
                    Case "line item"
                        dLineItemChargesAllowances = dLineItemChargesAllowances + CType(ugRow.Cells("value").Value, Double)
                End Select

                Select Case ugRow.Cells("IsAllowance").Value.ToString().ToLower()
                    Case "true", "1"
                        ' This is an allowance, ignore.
                    Case "false", "0"
                        ' This is a charge, add to charges total.
                        dChargesOnly = dChargesOnly + CType(ugRow.Cells("value").Value, Double)
                    Case String.Empty
                        ' This is an unknown value. May be from a legacy EInvoice. If value > 0 then treat as a charge. if value < 0 treat as an allowance.
                        If CType(ugRow.Cells("value").Value, Double) >= 0 Then
                            'charge.
                            dChargesOnly = dChargesOnly + CType(ugRow.Cells("value").Value, Double)
                        Else
                            'allowance. ignore.
                        End If
                End Select
            Next
        End If

        Label_AllocatedCharges.Text = String.Format(currencyCulture, "{0:C}", dAllocatedChargesAllowances)
        Label_NonAllocCharges.Text = String.Format(currencyCulture, "{0:C}", dNonAllocatedChargesAllowances)
        Label_LineItemCharges.Text = String.Format(currencyCulture, "{0:C}", dLineItemChargesAllowances)

        'Only Display Charges (Not Allowances) in this text box.
        txtSACTotal.Text = String.Format("{0:C}", dAllocatedChargesAllowances + dNonAllocatedChargesAllowances + dLineItemChargesAllowances)

        CalculateTotals()
    End Sub

    Private Sub CalculateTotals()

        pbLoading = True

        '-- Fill in totals/differences
        Dim cInvoiceCost As Decimal
        Dim cInvoiceTotal As Decimal
        Dim cSACTotal As Decimal
        Dim X As Integer
        Dim dChargesAllowances As Decimal
        'Dim cRefusalTotal As Decimal

        dChargesAllowances = dAllocatedChargesAllowances + dNonAllocatedChargesAllowances

        '-- 20090708 - Dave Stacey - TFS 9688 - handle non-numeric entry on invoice total
        If Not IsNumeric(FixNumber(txtInvTot.Text)) Then
            logger.Info("CalculateTotals - unable to process the save request because the invoice total is not numeric")
            MsgBox(String.Format(ResourcesOrdering.GetString("msg_NumericInvoiceAmt"), Environment.NewLine), MsgBoxStyle.Information, Me.Text)
            txtInvTot.Text = ""
            RefreshData()
            Exit Sub
        Else
            cInvoiceTotal = CDec(FixNumber(txtInvTot.Text))
        End If

        ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
        'cRefusalTotal = dTotalRefused

        cInvoiceCost = cInvoiceTotal - pcInvFrght

        '**TFS Bug #2343 (M. Zhao) 
        '**Non-Allocated charges can be either negative or positive. Both of the negative and positive non-allocated 
        '**  charges will be used in the determination of the approval status of a PO.
        '**Allocated charges is separated as charges (positive) and allowance (negative). Only the allocated charges will be used in
        '**  the determination of the approval status of a PO. The allocated allowance should have been included in the invoice amount.

        'dChargesAllowances is the total of non-allocated charges and allocated charges (allowances are excluded - yes, the variable names are misleading).
        cSACTotal = dLineItemChargesAllowances + dChargesAllowances

        txtSACTotal.Text = VB6.Format(cSACTotal, "#####0.00##")

        For X = txtInvoiceCost.LBound To txtInvoiceCost.UBound
            ' txtInvoiceCost(X) refers to the subteam total TextBox.
            txtInvoiceCost(X).Text = VB6.Format((cInvoiceTotal - pcInvFrght - cSACTotal), "#####0.00##")
        Next X

        txtTotalInvoiceCost.Text = VB6.Format(cInvoiceCost, "#####0.00##")

        ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
        'txtRefusalTotal.Text = VB6.Format(cRefusalTotal, "#####0.00##")

        If (cInvoiceTotal - cSACTotal) = pcOrderCost Then
            txtCostDifference.Text = VB6.Format(0, "#####0.00##")
        Else
            Dim dChargesAllowance As Decimal = dChargesAllowances

            If dChargesAllowances > 0 Then
                txtCostDifference.Text = VB6.Format((cInvoiceCost - pcOrderCost) - dChargesAllowance, "#####0.00##")
            Else
                txtCostDifference.Text = VB6.Format((cInvoiceCost - pcOrderCost) + dChargesAllowance, "#####0.00##")
            End If
        End If

        If geOrderType = enumOrderType.Purchase Then
            Dim dNonAllocatedChargesAllowance As Decimal = dNonAllocatedChargesAllowances
            InvoiceCostMinusNonAllocatedCharges = cInvoiceTotal - Math.Round(dNonAllocatedChargesAllowance, 2) - pcInvFrght
        Else
            If Not pbClosedOrder Then
                InvoiceCostMinusNonAllocatedCharges = pcOrderCost
            Else
                InvoiceCostMinusNonAllocatedCharges = 0
            End If

        End If

        pbLoading = False
        logger.Debug("CalculateTotals exit")
    End Sub

    Private Sub optType_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optType.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then
            If pbLoading OrElse pbTypeClicked Then Exit Sub

            Dim Index As Short = optType.GetIndex(eventSender)

            pbDataChanged = True

            pbTypeClicked = True
            RefreshData()
            pbTypeClicked = False

        End If
    End Sub

    Private Sub txtInvTot_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtInvTot.TextChanged
        If Me.IsInitializing = True Then Exit Sub

        If Not pbLoading Then
            pbDataChanged = True
        End If

        CalculateTotals()
    End Sub

    Private Sub txtVendorPO_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtVendorPO.TextChanged
        If Me.IsInitializing = True OrElse pbLoading Then Exit Sub
        pbDataChanged = True
    End Sub

    Private Sub txtVendorPO_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtVendorPO.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        ' Convert lowercase letters to uppercase.
        If KeyAscii >= ASCII_LOWERCASE_A AndAlso KeyAscii <= ASCII_LOWERCASE_Z Then
            KeyAscii = KeyAscii - ASCII_LOWERCASE_A + ASCII_UPPERCASE_A
        End If

        eventArgs.KeyChar = Chr(KeyAscii)

        If KeyAscii = ASCII_PIPE Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub dtpSearchDate_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles dtpSearchDate.Validating
        If Me.IsInitializing OrElse pbLoading Then
            Exit Sub
        End If

        If dtpSearchDate.Value > SystemDateTime(True) Then
            MsgBox("The invoice date may not be a future date.  Please make another selection.", MsgBoxStyle.Information, "Invalid Date")
            e.Cancel = True
        End If
    End Sub

    Private Sub dtpSearchDate_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpSearchDate.ValueChanged
        If Me.IsInitializing OrElse pbLoading Then
            Exit Sub
        End If

        pbDataChanged = True
    End Sub

    Private Sub btnRemoveSubteamGLAcct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveSubteamGLAcct.Click
        Dim subteamDAO As New OrderingDAO
        Dim invoiceSpecChargesList As DataTable = OrderingDAO.LoadOrderInvoiceSpecChargeTable(glOrderHeaderID)

        'This holds the row number on your datatable        
        Dim rowNum As Integer

        'This holds the ID taken from the datarow at index = rowNum        
        Dim mlSubteamGLAcct As Integer

        'Now loop thru the checkedIndices collection and get the actual index        
        'of each checked item which is necessarily the row number on the datatable        
        For i As Integer = 0 To CheckedListBox_SpecCharges.CheckedIndices.Count - 1
            rowNum = CheckedListBox_SpecCharges.CheckedIndices.Item(i)

            'Once you have the row number, go directly to the datatable and get what you need            
            mlSubteamGLAcct = CInt(invoiceSpecChargesList.Rows(rowNum).Item("Charge_ID"))
            OrderingDAO.DeleteControlGroupInvoiceCharge(mlSubteamGLAcct)
        Next

        ' Populate Subteam/GLAcct Charges list for SAC.
        LoadOrderInvoiceSpecCharges(glOrderHeaderID)
    End Sub

    Private Sub cmdDisplayEInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDisplayEInvoice.Click
        EInvoiceHTMLDisplay.EinvoiceId = _EinvoiceId
        EInvoiceHTMLDisplay.StoreNo = plStore_No
        EInvoiceHTMLDisplay.ShowDialog()
    End Sub

    Private Sub Button_AddCharges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddCharges.Click
        Dim frm As AddInvoiceCharges = New AddInvoiceCharges(glOrderHeaderID, Me)
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub Button_RemoveCharges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RemoveCharges.Click
        With UltraGrid_Charges.Selected
            If .Rows.Count > 0 Then
                Dim ugRow As Infragistics.Win.UltraWinGrid.UltraGridRow
                For Each ugRow In .Rows
                    OrderingDAO.DeleteControlGroupInvoiceCharge(CType(ugRow.Cells("Charge_ID").Value, Integer))
                Next
                LoadOrderInvoiceSpecCharges(glOrderHeaderID)
            End If
        End With
    End Sub

    Private Sub _optType_2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optType_2.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If CType(sender, RadioButton).Checked Then

            ClearInvoiceFields()

            If pbLoading OrElse pbTypeClicked Then Exit Sub

            pbDataChanged = True
            pbTypeClicked = True
            RefreshData()
            pbTypeClicked = False

        End If
    End Sub

    Private Function IsSustainabilityRankingRankingCheckOK() As Boolean
        'Make sure required Sustainability Rankings have been entered for non-transfer orders
        Dim returnValue As Boolean
        returnValue = True
        If geOrderType <> enumOrderType.Transfer Then
            Dim rsOrder As DAO.Recordset
            rsOrder = SQLOpenRecordSet("EXEC CountUnrankedSustainabilityRequiredOrderItems " & glOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsOrder.Fields("OrderItemCount").Value > 0 Then
                rsOrder.Close()
                MsgBox("Cannot close the order:  Please select a Sustainability Ranking for all indicated items on the Line Item Information window before closing the order.", MsgBoxStyle.Information, Me.Text)
                txtInvTot.Text = String.Empty
                returnValue = False
            Else
                rsOrder.Close()
            End If
        End If
        Return returnValue
    End Function

    Private Sub btnCurrency_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrency.Click
        If MsgBox("Enable currency selector?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
            cmbCurrency.Enabled = True
            pbDataChanged = True
        End If
    End Sub

    Private Sub btnRefuseReceiving_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefuseReceiving.Click
        If IsNothing(ucReasonCode.Value) Then
            MsgBox("Please select a Reason Code to refuse receiving on this order.", MsgBoxStyle.Information, Me.Text)
        Else
            If MsgBox("Refuse and close this order?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                bRefuseReceiving = True
                RefuseReceivingAndClose(ucReasonCode.Value)
                MsgBox("The order has been closed, and the refusal receiving reason code has been saved.", MsgBoxStyle.Information, Me.Text)

                'Send email to the buyer
                Dim sToEmail As String = API.GetADUserInfo(gsPOCreatorUserName, "mail")

                SendRefusedPOEmail(sToEmail)

                Me.Close()
            End If
        End If
    End Sub

    Private Sub SendRefusedPOEmail(ByVal sToEmail As String)
        Dim sFromEmail As String = API.GetADUserInfo(Environment.UserName, "mail")
        Dim sBody As String = ""
        Dim sSubject As String
        Dim SendMailClient As SmtpClient
        Dim msg As MailMessage

        sSubject = "PO " & glOrderHeaderID.ToString() & " " & gsStoreName & " Store, " & gsVendorName & " was refused today " & Today.ToShortDateString()

        sBody = "PO " & glOrderHeaderID.ToString() & " for the " & gsStoreName & " Store from " & gsVendorName & " was refused on " & Today.ToShortDateString() & " with the reason code: " & ucReasonCode.SelectedRow.Cells(1).Text & " - " & ucReasonCode.SelectedRow.Cells(2).Text

        If Not String.IsNullOrEmpty(sToEmail) Then
            Try
                SendMailClient = New SmtpClient(ConfigurationServices.AppSettings("EmailSMTPServer"))
                msg = New MailMessage(sFromEmail, sToEmail, sSubject, sBody)
                SendMailClient.Send(msg)
            Catch ex As Exception
                MsgBox("The refusal notification could not be sent to the buyer due to the following error:" & vbCrLf & vbCrLf & ex.Message)
            Finally
                SendMailClient = Nothing
                msg = Nothing
            End Try
        Else
            MsgBox("The refusal notification could not be sent to the buyer because (" & gsPOCreatorUserName & ") dosen't have an email account.")
        End If
    End Sub

    Private Sub cmdItemsRefused_Click(sender As System.Object, e As System.EventArgs) Handles cmdItemsRefused.Click

        ' Show the RefusedOrderItem form, and based upon the user's actions, re-calculate the totals.
        Dim orderItemsRefused As New frmOrderItemsRefused

        If orderItemsRefused.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ' Changes were made in OrderItemsRefused.  Refresh the TotalRefused and re-calculate the totals on the UI.
            Dim rsOrder As DAO.Recordset = Nothing
            SQLExecute(String.Format("UpdateOrderHeaderCosts {0}", glOrderHeaderID), DAO.RecordsetOptionEnum.dbSQLPassThrough)
            rsOrder = SQLOpenRecordSet("GetOrderStatus " & glOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            dTotalRefused = rsOrder.Fields("TotalRefused").Value
            CalculateTotals()
        End If

        orderItemsRefused.Dispose()

    End Sub

    Private Sub cmbCurrency_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbCurrency.SelectedIndexChanged

        ' If the currency is changed, then the form should update the UI to match the cultural formatting of
        ' the newly selected currency.  In practice, this won't change much (USD and CAD are both formatted with $, for example), 
        ' but it should provide the capability for this form to handle multiple currency formats in the future.

        Dim currencyValue As Decimal

        ' Update the currencyCode and currencyCulture based on the chosen value of cmbCurrency.
        currencyCode = cmbCurrency.SelectedItem.ToString()
        currencyCulture = CultureInfo.CreateSpecificCulture((CurrencyCultureMapping.Item(currencyCode)))

        ' Update the cultural format of the Charges grid to that of the chosen culture.
        UltraGrid_Charges.DisplayLayout.Bands(0).Columns("Value").Format = "C"
        UltraGrid_Charges.DisplayLayout.Bands(0).Columns("Value").FormatInfo = currencyCulture

        ' Update the charges summary labels at the bottom of the screen to match the new culture.
        currencyValue = Decimal.Parse(Label_AllocatedCharges.Text.Substring(1))
        Label_AllocatedCharges.Text = String.Format(currencyCulture, "{0:C}", currencyValue)

        currencyValue = Decimal.Parse(Label_LineItemCharges.Text.Substring(1))
        Label_LineItemCharges.Text = String.Format(currencyCulture, "{0:C}", currencyValue)

        currencyValue = Decimal.Parse(Label_NonAllocCharges.Text.Substring(1))
        Label_NonAllocCharges.Text = String.Format(currencyCulture, "{0:C}", currencyValue)

    End Sub

    Private Sub txtVendorPO_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles txtVendorPO.Validating
        If Me.IsInitializing OrElse pbLoading Then
            Exit Sub
        End If

        ' This is the regex for only allowing alphanumeric characters.  It may come in handy one day.
        'Dim regex As New Regex("[^a-zA-Z0-9]+")

        ' Disallow the pipe character.
        Dim regex As New Regex("[|]+")
        If regex.IsMatch(txtVendorPO.Text) Then
            MsgBox("The invoice number may not contain the character '|'.  Please enter an alphanumeric value.", MsgBoxStyle.Information, "Invalid Invoice/Document Number")
            txtVendorPO.SelectAll()
            e.Cancel = True
        End If
    End Sub

End Class