Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Xml
Imports log4net
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.Encryption
Imports VB = Microsoft.VisualBasic

Friend Class frmMain
  Inherits System.Windows.Forms.Form

  Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

  Public Const ENVIRONMENT_LABEL_PRD As String = "Production"

  Private Sub frmMain_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
    logger.Info(String.Format("Application Startup Path: {0}", Application.StartupPath))

    Dim hWindowHandle As Integer
    Dim sNewCaption As String = "IRMA Client"

    Dim VersionDAO As New VersionDAO
    Dim blnShowMessage As Boolean

    ' initially hide the toolbar container
    ToolStripContainer1.TopToolStripPanelVisible = False
    tslTitle.Text = String.Empty
    tslTitle.Alignment = ToolStripItemAlignment.Right

    ' do version check
    If String.Compare(VersionDAO.GetVersionInfo("IRMA CLIENT"), Application.ProductVersion) > 0 Then
      If MessageBox.Show(String.Format("You are using old version of IRMA client (version {0}).{1}{1}It's strongly recommended to update your IRMA client application!{1}{1}Would you like to proceed using old IRMA client?", Application.ProductVersion, vbCrLf), "System Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.Cancel Then
        Application.Exit()
        End
      End If
    End If

    'Make sure they don't have more than two copies open
    hWindowHandle = FindWindow(vbNullString, sNewCaption)
    glInstance = 1

    If hWindowHandle <> 0 Then
      glInstance = 2
      sNewCaption = sNewCaption & " (copy 2)"
      hWindowHandle = FindWindow(vbNullString, sNewCaption)

      If hWindowHandle <> 0 Then
        MsgBox(String.Format(ResourcesIRMA.GetString("AppAlreadyRunning"), vbCrLf, "two"), MsgBoxStyle.Critical, Me.Text)
        End
      End If
    End If

    InitializeGlobalSettings(sNewCaption)

    ' Set the tool status values
    ToolStripStatusLabel_Region.Text = ToolStripStatusLabel_Region.Text & gsRegionCode
    ToolStripStatusLabel_Environment.Text = ToolStripStatusLabel_Environment.Text & ConfigurationServices.AppSettings("environment")
    ToolStripStatusLabel_Version.Text = ToolStripStatusLabel_Version.Text & "System: " & VersionDAO.GetVersionInfo("SYSTEM") & "     Application: " & VersionDAO.GetVersionInfo("IRMA CLIENT")
    ToolStripStatusLabelRegionalSetting.Text = ToolStripStatusLabelRegionalSetting.Text & gsUG_CultureDisplayName

    InitializeDatabase()

    '-- Login
    ValidateLogon()
    tslTitle.Text = String.Format("User: {0}  ({1})", gsUserName, gsTitleDescription)

    '-- Splash
    frmSplash.Show()
    Application.DoEvents()

    Try
      blnShowMessage = ConfigurationServices.AppSettings("ShowMainMessage")
    Catch ex As Exception
      blnShowMessage = False
    End Try

    pnlMainMessage.Visible = blnShowMessage

    InitializeLocalDatabase()

    ' Set menu access based on user's assigned roles
    SetMenuAccess()
    SetToolbarAccess()

    ' TFS 13036, v4.0, 7/21/2010, Tom Lux: Moved LoadItemUnits() to ItemUnitDAO class.  Fixing issue where global UOM IDs were not being reloaded if an item unit was added via UI.
    ItemUnitDAO.LoadItemUnits()

    ReDim sDiscountType(4)
    sDiscountType(0) = "No Discount"
    sDiscountType(1) = "Cash Discount"
    sDiscountType(2) = "Percent Discount"
    sDiscountType(3) = "Free Items"
    sDiscountType(4) = "Landed Percent"

    GridWidth = giGridScrollBarWidth 'GetScrollWidth

    '-- GetItemInfo
    PopulateAWeight()

    LoadItemUnitConversion()

    InitializeCurrencyCultureMapping()

    Sleep(1000) 'Let the splash screen display
    frmSplash.Close()
    frmSplash.Dispose()

    ' Display a warning page if this is not production
    If ConfigurationServices.AppSettings("environment") <> ENVIRONMENT_LABEL_PRD Then
      MsgBox(ResourcesIRMA.GetString("TestWarning"), MsgBoxStyle.Information, Me.Text)
    End If
  End Sub

  Private Sub frmMain_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    logger.Debug("frmMain_FormClosed entry")

    On Error Resume Next

    If Not grsUnitConversion Is Nothing Then
      If grsUnitConversion.State = ADODB.ObjectStateEnum.adStateOpen Then grsUnitConversion.Close()
      grsUnitConversion = Nothing
    End If

    '-- close the dao database
    If Not gDBInventory Is Nothing Then
      gDBInventory.Close()
      gDBInventory = Nothing
    End If

    If Not gDBReport Is Nothing Then
      gDBReport.Close()
      gDBReport = Nothing
      gJetFlush = Nothing
    End If

    logger.Debug("frmMain_FormClosed exit")
  End Sub

#Region "Menu"

  Public Sub mnuCycleCountReports_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Inventory_CycleCountReports.Click
    logger.Debug("mnuCycleCountReports_Click entry")
    frmCycleCountMasterList.ShowDialog()
    frmCycleCountMasterList.Dispose()
    logger.Debug("mnuCycleCountReports_Click exit")
  End Sub

  Public Sub mnuOrderItemQueue_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEdit_Orders_ItemQueue.Click
    logger.Debug("mnuOrderItemQueue_Click entry")
    frmOrderItemQueue.ShowDialog()
    frmOrderItemQueue.Dispose()
    logger.Debug("mnuOrderItemQueue_Click exit")
  End Sub

  Public Sub mnuLocations_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuInventory_Locations.Click
    logger.Debug("mnuLocations_Click entry")
    frmLocationList.ShowDialog()
    frmLocationList.Dispose()
    logger.Debug("mnuLocations_Click exit")
  End Sub

  Public Sub mnuActiveItemList_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_ActiveItemList.Click
    logger.Debug("mnuActiveItemList_Click entry")
    frmActiveItemReport.ShowDialog()
    frmActiveItemReport.Dispose()
    logger.Debug("mnuActiveItemList_Click exit")
  End Sub


  Public Sub mnuAvgCostDist_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_ReceivingDistribution_AverageCost_DistributionMargin.Click
    logger.Debug("mnuAvgCostDist_Click entry")
    frmAvgCostMarginReports.ShowDialog()
    frmAvgCostMarginReports.Dispose()
    logger.Debug("mnuAvgCostDist_Click exit")
  End Sub

  Public Sub mnuAvgCostList_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_ReceivingDistribution_AverageCost_List.Click
    logger.Debug("mnuAvgCostList_Click entry")
    frmAvgCostReport.ShowDialog()
    frmAvgCostReport.Dispose()
    logger.Debug("mnuAvgCostList_Click exit")
  End Sub

  Private Sub mnuAdministration_ScheduledJobs_CloseReceiving_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_CloseReceiving.Click
    Form_CloseReceivingJobController.ShowDialog()
    Form_CloseReceivingJobController.Dispose()
  End Sub

  Public Sub mnuCasesByDepartment_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Miscellaneous_CasesBySubTeam.Click
    logger.Debug("mnuCasesByDepartment_Click entry")
    frmCasesByDepartment.ShowDialog()
    frmCasesByDepartment.Dispose()
    logger.Debug("mnuCasesByDepartment_Click exit")
  End Sub

  Public Sub mnuCasesByDeptAudit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Miscellaneous_CasesBySubTeamAudit.Click
    logger.Debug("mnuCasesByDeptAudit_Click entry")
    frmCasesByDepartmentAudit.ShowDialog()
    frmCasesByDepartmentAudit.Dispose()
    logger.Debug("mnuCasesByDeptAudit_Click exit")
  End Sub

  Public Sub mnuClosedOrders_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Order_ClosedOrders.Click
    logger.Debug("mnuClosedOrders_Click entry")
    frmClosedOrdersReport.ShowDialog()
    frmClosedOrdersReport.Dispose()
    logger.Debug("mnuClosedOrders_Click exit")
  End Sub

  Public Sub mnuCostException_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Buyer_CostException.Click
    logger.Debug("mnuCostException_Click entry")
    frmCostExceptionReport.ShowDialog()
    frmCostExceptionReport.Dispose()
    logger.Debug("mnuCostException_Click exit")
  End Sub

  Public Sub mnuCreditReason_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Order_CreditReason.Click
    logger.Debug("mnuCreditReason_Click entry")
    'frmCreditReason.ShowDialog()
    'frmCreditReason.Dispose()
    MessageBox.Show("You will now be directed to Reporting Services to provide report parameters.", "Credit Reason Report", MessageBoxButtons.OK, MessageBoxIcon.Information)
    Call ReportingServicesReport("CreditReason&rs:Command=Render&rc:Parameters=True")
    logger.Debug("mnuCreditReason_Click exit")
  End Sub

  Public Sub mnuDeletedOrders_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Order_DeletedOrders.Click
    logger.Debug("mnuDeletedOrders_Click entry")
    frmDeletedOrderReport.ShowDialog()
    frmDeletedOrderReport.Dispose()
    logger.Debug("mnuDeletedOrders_Click exit")
  End Sub

  Public Sub mnuDiscontinueItemsWithInventory_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_DiscontinueItemsWithInventory.Click
    logger.Debug("mnuDiscontinueItemsWithInventory_Click entry")
    frmDiscontinueItemsWithInventory.ShowDialog()
    frmDiscontinueItemsWithInventory.Dispose()
    logger.Debug("mnuDiscontinueItemsWithInventory_Click exit")
  End Sub

  Public Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore.Click
    logger.Debug("mnuEditStore_Click entry")
    Dim lStore_No As Integer
    Dim fSelStore As frmSelStore

    If IsDBNull(glStore_Limit) OrElse glStore_Limit = 0 Then
      'prompt user to select store
      fSelStore = New frmSelStore(False)
      fSelStore.ShowDialog()
      lStore_No = fSelStore.Store_No
      fSelStore.Close()
      fSelStore.Dispose()
    Else
      lStore_No = glStore_Limit
    End If

    If lStore_No > 0 Then
      frmStore.Store_No = lStore_No
      frmStore.ShowDialog()
      frmStore.Dispose()
    End If
    logger.Debug("mnuEditStore_Click exit")
  End Sub

  Public Sub mnuGLDistributionCheck_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Accounting_GLSummary_GLDistributionCheck.Click
    logger.Debug("mnuGLDistributionCheck_Click entry")
    frmGLDistributionCheckReport.ShowDialog()
    frmGLDistributionCheckReport.Dispose()
    logger.Debug("mnuGLDistributionCheck_Click exit")
  End Sub

  Public Sub mnuGLSales_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Accounting_GLSummary_GLSales.Click
    logger.Debug("mnuGLSales_Click entry")
    frmGLSalesReport.ShowDialog()
    frmGLSalesReport.Dispose()
    logger.Debug("mnuGLSales_Click exit")
  End Sub


  Public Sub mnuCycleCounts_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuInventory_CycleCounts.Click
    logger.Debug("mnuCycleCounts_Click entry")
    frmCycleCountMasterList.ShowDialog()
    frmCycleCountMasterList.Dispose()
    logger.Debug("mnuCycleCounts_Click exit")
  End Sub

  Public Sub mnuInventoryBalance_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Buyer_InventoryBalance.Click
    logger.Debug("mnuInventoryBalance_Click entry")
    frmInventoryBalanceReport.ShowDialog()
    frmInventoryBalanceReport.Dispose()
    logger.Debug("mnuInventoryBalance_Click exit")
  End Sub

  Private Sub mnuInventoryValue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Inventory_InventoryValue.Click
    logger.Debug("mnuInventoryValue_Click entry")
    frmInventoryValueReport.ShowDialog()
    frmInventoryValueReport.Close()
    frmInventoryValueReport.Dispose()
    logger.Debug("mnuInventoryValue_Click exit")
  End Sub

  Public Sub mnuInventoryGuideReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_InventoryGuide.Click
    logger.Debug("mnuInventoryGuideReport_Click entry")
    frmInventoryGuideReport.ShowDialog()
    frmInventoryGuideReport.Dispose()
    logger.Debug("mnuInventoryGuideReport_Click exit")
  End Sub

  Public Sub mnuInvoiceManifest_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Accounting_InvoiceManifest.Click
    logger.Debug("mnuInvoiceManifest_Click entry")
    frmInvoiceManifest.ShowDialog()
    frmInvoiceManifest.Dispose()
    logger.Debug("mnuInvoiceManifest_Click exit")
  End Sub

  Private Sub mnuInvoiceDiscrepancies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Accounting_InvoiceDiscrepancies.Click
    logger.Debug("mnuInvoiceDiscrepancies_Click entry")
    InvoiceDiscrepanciesReport.ShowDialog()
    InvoiceDiscrepanciesReport.Dispose()
    logger.Debug("mnuInvoiceDiscrepancies_Click exit")
  End Sub

  Public Sub mnuItemOnHandComparison_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_ItemOnHandComparisonBetweenLocation.Click
    logger.Debug("mnuItemOnHandComparison_Click entry")
    frmItemOnHandComparison.ShowDialog()
    frmItemOnHandComparison.Dispose()
    logger.Debug("mnuItemOnHandComparison_Click exit")
  End Sub

  Public Sub mnuItemOrderHistory_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_ItemOrderHistory.Click
    logger.Debug("mnuItemOrderHistory_Click entry")
    ItemOrderHistory.ShowDialog()
    ItemOrderHistory.Dispose()
    logger.Debug("mnuItemOrderHistory_Click exit")
  End Sub

  Public Sub mnuItemPrice_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Price_ItemPrice.Click
    logger.Debug("mnuItemPrice_Click entry")
    frmItemPriceReport.ShowDialog()
    frmItemPriceReport.Dispose()
    logger.Debug("mnuItemPrice_Click exit")
  End Sub

  Public Sub mnuLocationItemsReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Inventory_LocationItemsReport.Click
    logger.Debug("mnuLocationItemsReport_Click entry")
    frmLocationItemsReport.ShowDialog()
    frmLocationItemsReport.Dispose()
    logger.Debug("mnuLocationItemsReport_Click exit")
  End Sub

  Public Sub mnuLocationManulCountSheet_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Inventory_LocationItemsManulCountSheet.Click
    logger.Debug("mnuLocationManulCountSheet_Click entry")
    frmLocationItemManualCountReport.ShowDialog()
    frmLocationItemManualCountReport.Dispose()
    logger.Debug("mnuLocationManulCountSheet_Click exit")
  End Sub

  Public Sub mnuLocationsReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Inventory_LocationsReport.Click
    logger.Debug("mnuLocationsReport_Click entry")
    frmLocationsReport.ShowDialog()
    frmLocationsReport.Dispose()
    logger.Debug("mnuLocationsReport_Click exit")
  End Sub

    Public Sub mnuNotAvailableItems_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_NotAvailableItems.Click
        logger.Debug("mnuNotAvailableItems_Click entry")
        frmNotAvailableReport.ShowDialog()
        frmNotAvailableReport.Dispose()
        logger.Debug("mnuNotAvailableItems_Click exit")
    End Sub

    Public Sub mnuOrderReports_APUpload_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Order_APUpload.Click
    logger.Debug("mnuOrderReports_APUpload_Click entry")
    frmAPUploadReports.ShowDialog()
    frmAPUploadReports.Dispose()
    logger.Debug("mnuOrderReports_APUpload_Click exit")
  End Sub

  Public Sub mnuOrdersAddEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEdit_Orders_AddEdit.Click, tsbEdit_Orders_AddEdit.Click
    logger.Debug("mnuOrdersAddEdit_Click entry")
    frmOrders.ShowDialog()
    frmOrders.Dispose()
    logger.Debug("mnuOrdersAddEdit_Click exit")
  End Sub

  Public Sub mnuOrdersAllocate_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEdit_Orders_Allocate.Click
    logger.Debug("mnuOrdersAllocate_Click entry")
    Cursor = Cursors.WaitCursor
    frmOrdersAllocate.ShowDialog()
    frmOrdersAllocate.Dispose()
    Cursor = Cursors.Default
    logger.Debug("mnuOrdersAllocate_Click exit")
  End Sub

  Public Sub mnuOutOfPeriodInvRpt_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Accounting_OutOfPeriodInvoiceReport.Click
    logger.Debug("mnuOutOfPeriodInvRpt_Click entry")
    ' Commented the Legacy CryStal Report
    Dim frm As frmOutOfPeriodInvoice = New frmOutOfPeriodInvoice

    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuOutOfPeriodInvRpt_Click exit")
  End Sub

  Public Sub mnuPricingBatches_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEdit_Pricing_Batches.Click, tsbEdit_Pricing_Batches.Click
    logger.Debug("mnuPricingBatches_Click entry")
    frmPricingBatch.ShowDialog()
    frmPricingBatch.Dispose()
    logger.Debug("mnuPricingBatches_Click exit")
  End Sub

  Public Sub mnuPricingLineDrive_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEdit_Pricing_LineDrive.Click
    logger.Debug("mnuPricingLineDrive_Click entry")
    frmLineDrive.ShowDialog()
    frmLineDrive.Dispose()
    logger.Debug("mnuPricingLineDrive_Click exit")
  End Sub

  Public Sub mnuReceivedNotClosed_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Order_OrdersReceivedNotClosed.Click
    logger.Debug("mnuReceivedNotClosed_Click entry")
    frmReceivedNotClosedReport.ShowDialog()
    frmReceivedNotClosedReport.Dispose()
    logger.Debug("mnuReceivedNotClosed_Click exit")
  End Sub

  Public Sub mnuPriceHistory_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Price_PriceChanges.Click
    logger.Debug("mnuPriceHistory_Click entry")
    frmPriceHistory.ShowDialog()
    frmPriceHistory.Dispose()
    logger.Debug("mnuPriceHistory_Click exit")
  End Sub

  Public Sub mnuReceivingLog_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_ReceivingDistribution_ReceivingLog.Click
    logger.Debug("mnuReceivingLog_Click entry")
    frmReceivingLog.ShowDialog()
    frmReceivingLog.Dispose()
    logger.Debug("mnuReceivingLog_Click exit")
  End Sub

  Public Sub mnuAbout_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuHelp_About.Click
    logger.Debug("mnuAbout_Click entry")
    frmSplash.ShowDialog()
    frmSplash.Dispose()
    logger.Debug("mnuAbout_Click exit")
  End Sub

  Public Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName.Click
    logger.Debug("mnuBrandName_Click entry")
    frmBrand.ShowDialog()
    frmBrand.Dispose()
    logger.Debug("mnuBrandName_Click exit")
  End Sub

  Public Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion.Click
    logger.Debug("mnuConversion_Click entry")
    frmConversion.ShowDialog()
    frmConversion.Dispose()
    logger.Debug("mnuConversion_Click exit")
  End Sub

  Public Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode.Click
    logger.Debug("mnuInventoryAdjustmentCode_Click entry")
    selInventoryAdjustmentCode.ShowDialog()
    selInventoryAdjustmentCode.Dispose()
    logger.Debug("mnuInventoryAdjustmentCode_Click exit")
  End Sub

  Public Sub mnuDailyReceiving_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_ReceivingDistribution_DailyReceiving.Click
    logger.Debug("mnuDailyReceiving_Click entry")
    frmReportReceiving.ShowDialog()
    frmReportReceiving.Dispose()
    logger.Debug("mnuDailyReceiving_Click exit")
  End Sub

  Public Sub mnuExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFile_Exit.Click
    logger.Debug("mnuExit_Click entry")
    Me.Close()
    logger.Debug("mnuExit_Click exit")
  End Sub

  Public Sub mnuInvItmAdjust_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuInventory_Adjustments.Click
    logger.Debug("mnuInvItmAdjust_Click entry")
    frmInventoryAdjustment.ShowDialog()
    frmInventoryAdjustment.Dispose()
    logger.Debug("mnuInvItmAdjust_Click exit")
  End Sub

  Public Sub mnuItemList_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_ItemList.Click
    logger.Debug("mnuItemList_Click entry")
    frmItemList.ShowDialog()
    frmItemList.Dispose()
    logger.Debug("mnuItemList_Click exit")
  End Sub

  Public Sub mnuLoadTGM_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuTools_TGM_LoadTGMView.Click
    Using form As frmTGMLoad = New frmTGMLoad()
      form.ShowDialog()
      If form.IsCanceled Then Exit Sub
    End Using

    frmTGMLast.ShowDialog(Me)
    frmTGMLast.Dispose()
  End Sub

  Public Sub mnuMarginReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Buyer_Margin.Click
    logger.Debug("mnuMarginReport_Click entry")
    frmMarginReport.ShowDialog()
    frmMarginReport.Dispose()
    logger.Debug("mnuMarginReport_Click exit")
  End Sub

  Public Sub mnuNewTGM_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuTools_TGM_NewTGMView.Click
    logger.Debug("mnuNewTGM_Click entry")
    frmTGMCreate.ShowDialog()
    frmTGMCreate.Dispose()
    logger.Debug("mnuNewTGM_Click exit")
  End Sub

  Public Sub mnuOpenPOReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Order_OpenPurchaseOrders.Click
    logger.Debug("mnuOpenPOReport_Click entry")
    frmOpenOrdersReport.ShowDialog()
    frmOpenOrdersReport.Dispose()
    logger.Debug("mnuOpenPOReport_Click exit")
  End Sub

  Public Sub mnuOrderFulfillment_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_ReceivingDistribution_OrderFulfillment.Click
    logger.Debug("mnuOrderFulfillment_Click entry")
    frmFulfillmentReport.ShowDialog()
    frmFulfillmentReport.Dispose()
    logger.Debug("mnuOrderFulfillment_Click exit")
  End Sub

  Public Sub mnuOrigin_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin.Click
    logger.Debug("mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin entry")
    frmOrigin.ShowDialog()
    frmOrigin.Dispose()
    logger.Debug("mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin exit")
  End Sub

  Private Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts.Click
    logger.Debug("mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts_Click entry")
    Dim frm As New Form_ManageRoleConflicts

    frm.IsReadOnly = False
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts_Click exit")
  End Sub

  Public Sub mnuPrintSigns_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEdit_Pricing_ReprintSigns.Click, tsbEdit_Pricing_ReprintSigns.Click
    logger.Debug("mnuPrintSigns_Click entry")
    frmPricingPrintSigns.ShowDialog()
    frmPricingPrintSigns.Dispose()
    logger.Debug("mnuPrintSigns_Click exit")
  End Sub

  Public Sub mnuReportPurchToSalesComp_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Accounting_PurchaseToSalesComp.Click
    logger.Debug("mnuReportPurchToSalesComp_Click entry")
    PurchToSalesComp.ShowDialog()
    PurchToSalesComp.Dispose()
    logger.Debug("mnuReportPurchToSalesComp_Click exit")
  End Sub

  Public Sub mnuReportsAcctGLUploads_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Accounting_GLUploads.Click
    logger.Debug("mnuReportsAcctGLUploads_Click entry")
    frmGLUploads.ShowDialog()
    frmGLUploads.Dispose()
    logger.Debug("mnuReportsAcctGLUploads_Click exit")
  End Sub

  Public Sub mnuReportsItemVendorItems_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_VendorItems.Click
    logger.Debug("mnuReportsItemVendorItems_Click entry")
    glVendorID = 0
    frmVendorReports.ShowDialog()
    frmVendorReports.Dispose()
    logger.Debug("mnuReportsItemVendorItems_Click exit")
  End Sub

  Public Sub mnuReportsMiscellaneousLotNo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Miscellaneous_LotNo.Click
    logger.Debug("mnuReportsMiscellaneousLotNo_Click entry")
    frmLotNoReports.ShowDialog()
    frmLotNoReports.Dispose()
    logger.Debug("mnuReportsMiscellaneousLotNo_Click exit")
  End Sub

  Public Sub mnuReportsPriceBatches_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Price_Batches.Click
    logger.Debug("mnuReportsPriceBatches_Click entry")
    frmPricingBatchReports.ShowDialog()
    frmPricingBatchReports.Dispose()
    frmPricingBatch.Close()
    frmPricingBatch.Dispose()
    logger.Debug("mnuReportsPriceBatches_Click exit")
  End Sub

  Public Sub mnuRIPEOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFile_Import_RIPEOrder.Click
    logger.Debug("mnuRIPEOrder_Click entry")
    On Error Resume Next

    frmRIPEImportSelection.ShowDialog()

    If Err.Number <> 0 Then
      If Err.Number <> 364 Then
        MsgBox("Error in mnuRIPEOrder_Click(): " & Err.Description, MsgBoxStyle.Critical, Me.Text)
      End If
    End If

me_exit:
    frmRIPEImportSelection.Dispose()
    logger.Debug("mnuRIPEOrder_Click exit")
  End Sub

  Public Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife.Click
    logger.Debug("mnuShelfLife_Click entry")
    frmShelfLife.ShowDialog()
    frmShelfLife.Dispose()
    logger.Debug("mnuShelfLife_Click exit")
  End Sub

  Public Sub mnuShipperItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_ShipperItems.Click
    logger.Info("Running Shippers report.")
    ReportingServicesReport("Shippers&rs:Command=Render&rc:Parameters=True")
  End Sub

  Public Sub mnuSpecialsByEndDate_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Price_Specials.Click
    logger.Debug("mnuSpecialsByEndDate_Click entry")
    frmSpecialsByEndDate.ShowDialog()
    frmSpecialsByEndDate.Dispose()
    logger.Debug("mnuSpecialsByEndDate_Click exit")
  End Sub

  Public Sub mnuUPCLabelReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Item_UPCLabels.Click
    logger.Debug("mnuUPCLabelReport_Click entry")
    frmUpcLabelReport.ShowDialog()
    frmUpcLabelReport.Dispose()
    logger.Debug("mnuUPCLabelReport_Click exit")
  End Sub

  Public Sub mnuVendorEfficiency_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Miscellaneous_VendorEfficiency.Click
    logger.Debug("mnuVendorEfficiency_Click entry")
    frmVendorEfficiency.ShowDialog()
    frmVendorEfficiency.Dispose()
    logger.Debug("mnuVendorEfficiency_Click exit")
  End Sub

  Public Sub mnuVendorList_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuTools_VendorList.Click
    logger.Debug("mnuVendorList_Click entry")
    frmVendorList.ShowDialog()
    frmVendorList.Dispose()
    logger.Debug("mnuVendorList_Click exit")
  End Sub

  Public Sub mnuVendorsAddEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEdit_Vendors_AddEdit.Click
    logger.Debug("mnuVendorsAddEdit_Click entry")
    frmVendor.ShowDialog()
    frmVendor.Dispose()
    logger.Debug("mnuVendorsAddEdit_Click exit")
  End Sub

  Public Sub mnuVendorsCostImportExceptions_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEdit_Vendors_CostImportExceptions.Click
    logger.Debug("mnuVendorsCostImportExceptions_Click entry")
    frmVCAI_Exceptions.ShowDialog()
    frmVCAI_Exceptions.Dispose()
    logger.Debug("mnuVendorsCostImportExceptions_Click exit")
  End Sub

  Public Sub mnuVendorTransferSummary_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Accounting_GLSummary_GLTransfers.Click
    logger.Debug("mnuVendorTransferSummary_Click entry")
    frmTransferVendingAcctReport.ShowDialog()
    frmTransferVendingAcctReport.Dispose()
    logger.Debug("mnuVendorTransferSummary_Click exit")
  End Sub

  Public Sub mnuWarehouseMovement_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Buyer_WarehouseMovement.Click
    logger.Debug("mnuWarehouseMovement_Click entry")
    frmWarehouseMovementReport.ShowDialog()
    frmWarehouseMovementReport.Dispose()
    logger.Debug("mnuWarehouseMovement_Click exit")
  End Sub

  Public Sub mnuAdjustmentSummaryReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Accounting_AdjustmentSummary.Click
    logger.Debug("mnuAdjustmentSummaryReport_Click entry")
    frmAdjustmentSummaryReport.ShowDialog()
    frmAdjustmentSummaryReport.Dispose()
    logger.Debug("mnuAdjustmentSummaryReport_Click exit")
  End Sub

  Public Sub mnuZeroCostReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Buyer_ZeroAverageCostItems.Click
    logger.Debug("mnuZeroCostReport_Click entry")
    ' Commented the code which calls Legacy crystal report page.
    ' frmZeroCostReport.ShowDialog()
    ' frmZeroCostReport.Dispose()

    ' Calling ZeroCost Items report.
    frmZeroCostItemsReport.ShowDialog()
    frmZeroCostItemsReport.Dispose()
    logger.Debug("mnuZeroCostReport_Click exit")
  End Sub

  Public Sub mnuZeroPriceReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuReports_Buyer_ZeroPriceItems.Click
    logger.Debug("mnuZeroPriceReport_Click entry")
    frmZeroPriceReport.ShowDialog()
    frmZeroPriceReport.Dispose()
    logger.Debug("mnuZeroPriceReport_Click exit")
  End Sub

  Public Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution.Click
    logger.Debug("mnuZoneDistribution_Click entry")
    frmZoneDistribution.ShowDialog()
    frmZoneDistribution.Dispose()
    'frmVendorDepartments.Show(1)
    'frmVendorDepartments.Dispose()
    logger.Debug("mnuZoneDistribution_Click exit")
  End Sub

  Public Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup.Click
    logger.Debug("mnuZoneMarkup_Click entry")
    Dim fSupplyZone As New frmSupplyZone
    fSupplyZone.ShowDialog()
    fSupplyZone.Dispose()
    logger.Debug("mnuZoneMarkup_Click exit")
  End Sub

  Private Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName.Click
    frmCategory.ShowDialog()
    frmCategory.Dispose()
  End Sub

#End Region

  Private Sub InitializeGlobalSettings(ByVal sCaption As String)
    logger.Debug("InitializeGlobalSettings entry")
    '-- Set global varaibles
    gsReportingServicesURL = ConfigurationServices.AppSettings("reportingServicesURL")
    gsReportDirectory = ConfigurationServices.AppSettings("reportDirectory")

    'get regional InstanceData values and set globally
    '20100215 - Dave Stacey - Add Culture and DateMask to global variable collection
    Dim instance As InstanceDataBO = InstanceDataDAO.GetInstanceData
    gsRegionName = instance.RegionName
    gsRegionCode = instance.RegionCode
    gsPluDigitsSentToScale = instance.PluDigitsSentToScale
    gsUG_Culture = CultureInfo.CurrentCulture.Name
    gsUG_CultureDisplayName = CultureInfo.CurrentCulture.DisplayName
    gsUG_DateMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern

        ' are connection string encrypted?
        Dim encrypted As Boolean = CType(ConfigurationManager.AppSettings("encryptedConnectionStrings"), Boolean)
        ' Removed unused legacy vars from if-encrypted blocks below: gsCrystal_Connect, gsCrystalUser, gsCrystalPassword
        If encrypted Then
            Dim encryptor As New Encryptor()
            gsODBCConnect = encryptor.Decrypt(ConfigurationManager.ConnectionStrings("ODBC").ConnectionString())
        Else
            gsODBCConnect = ConfigurationManager.ConnectionStrings("ODBC").ConnectionString()
        End If

        '-- Check for any command line parameters.
        gbUseLocalTime = False
    If InStr(1, UCase(VB.Command()), "USELOCALTIME") Then
      sCaption = sCaption & " --- TEST MODE (Using Local Time) ---"
      gbUseLocalTime = True
    End If

    ' Save off what the windows regional settings are before we override them
    gsWindowsCulture = My.Application.Culture.Name
    '20100215 - Dave Stacey - Add Culture to UltraGrid via config lookup value
    Thread.CurrentThread.CurrentUICulture = New CultureInfo(gsUG_Culture, True)
    Me.Text = sCaption
    logger.Debug("InitializeGlobalSettings exit")
  End Sub
  Private Sub InitializeDatabase()
    logger.Debug("InitializeDatabase entry")
    '-- Open up a dao database
    Err.Clear()
    gDBInventory = DAODBEngine_definst.OpenDatabase("", DAO.DriverPromptEnum.dbDriverNoPrompt, False, gsODBCConnect & Me.Text & ";")
    If Err.Number <> 0 OrElse gDBInventory Is Nothing Then
      MsgBox("You do not have a SQL database account defined." & vbCrLf & "You cannot use this application.", MsgBoxStyle.Critical, "Login Failed")
      End
    End If
    On Error GoTo 0

    '-- Set time out to 10 minutes
    gDBInventory.QueryTimeout = 600

    logger.Debug("InitializeDatabase exit")
  End Sub
  Private Sub InitializeLocalDatabase()
    logger.Debug("InitializeLocalDatabase entry")
    On Error Resume Next
    'Try to compact the local Access database
    If Dir(gsLocalDbDirectory & "\INVENTORY_COMPACT.MDB") <> "" Then Kill(gsLocalDbDirectory & "\INVENTORY_COMPACT.MDB")
    Err.Clear()
    DAODBEngine_definst.CompactDatabase(gsLocalDbDirectory & "\" & LOCAL_DB, gsLocalDbDirectory & "\INVENTORY_COMPACT.MDB")
    If Err.Number = 0 Then
      Kill(gsLocalDbDirectory & "\" & LOCAL_DB)
      FileCopy(gsLocalDbDirectory & "\INVENTORY_COMPACT.MDB", gsLocalDbDirectory & "\" & LOCAL_DB)
      If Err.Number = 0 Then Kill(gsLocalDbDirectory & "\INVENTORY_COMPACT.MDB")
    End If

    'Connect to the local Access database
    Err.Clear()
    gDBReport = New ADODB.Connection
    gDBReport.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & gsLocalDbDirectory & "\" & LOCAL_DB)
    If Err.Number <> 0 Then
      gDBReport = Nothing
      MsgBox("Could not open local database", MsgBoxStyle.Critical, Me.Text)
      End
    End If
    'gJetFlush = CreateObject("JRO.JetEngine")
    gJetFlush = New JRO.JetEngine()

    On Error GoTo 0
    logger.Debug("InitializeLocalDatabase exit")
  End Sub

  Private Sub SetMenuAccess()
    logger.Debug("SetMenuAccess entry")

    Dim mnuTopLevel As ToolStripMenuItem
    Dim ds As DataSet = Nothing
    Dim blnIsAdminOnly As Boolean = False

    blnIsAdminOnly = (gbApplicationConfigurationAdministrator Or gbDataAdministrator Or gbJobAdministrator Or
                          gbPOSInterfaceAdministrator Or gbSecurityAdministrator Or gbStoreAdministrator Or
                          gbSystemConfigurationAdministrator Or gbUserMaintenance) _
                          And
                          (Not gbAccountant And Not gbBatchBuildOnly And Not gbBuyer And Not gbCoordinator And
                           Not gbCostAdmin And Not gbFacilityCreditProcessor And Not gbDCAdmin And Not gbDistributor And
                           Not gbEInvoicing And Not gbInventoryAdministrator And Not gbItemAdministrator And
                           Not gbLockAdministrator And Not gbManufacturer And Not gbPOAccountant And
                           Not gbPOApprovalAdmin And Not gbPOEditor And Not gbPriceBatchProcessor And Not gbTaxAdministrator And Not gbUserShrink And
                           Not gbUserShrinkAdmin And Not gbVendorAdministrator And Not gbVendorCostDiscrepancyAdmin And
                           Not gbWarehouse)

    ds = MenuAccessDAO.GetMenuAccessRecords

    '######################
    ' Client Menu access
    '######################

    'File Menu
    mnuFile_Export.Enabled = True
    mnuFile_Import.Enabled = True
    mnuFile_ProcessMonitor.Enabled = True And GetRegionalMenuAccess(ds, "mnuFile_ProcessMonitor")
    mnuFile_Exit.Enabled = True

    'File -> Export Menu
    mnuFile_Export_Planogram.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuFile_Export_Planogram")
    mnuFile_Export_RGIS.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuFile_Export_RGIS")
    mnuFile_Export.Enabled = IsTopLevelEnabled(mnuFile_Export)

    'File -> Import Menu
    mnuFile_Import_ExtendedItemMaintenance.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuFile_Import_ExtendedItemMaintenance")
    mnuFile_Import_ImportOrder.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuFile_Import_ImportOrder")
    mnuFile_Import_ItemMaintenanceBulkLoad.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuFile_Import_ItemMaintenanceBulkLoad")
    mnuFile_Import_ItemStoreMaintenanceBulkLoad.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuFile_Import_ItemStoreMaintenanceBulkLoad")
    mnuFile_Import_ItemVendorMaintenanceBulkLoad.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuFile_Import_ItemVendorMaintenanceBulkLoad")
    mnuFile_Import_Planogram.Enabled = (gbSuperUser Or gbItemAdministrator Or gbPriceBatchProcessor)
    mnuFile_Import.Enabled = IsTopLevelEnabled(mnuFile_Import)

    'Ripe Order Menu is visible only to the South region
    If gsRegionCode = "SO" Then
      mnuFile_Import_RIPEOrder.Enabled = (gbSuperUser Or gbCoordinator) And GetRegionalMenuAccess(ds, "mnuFile_Import_RIPEOrder")
    Else
      mnuFile_Import_RIPEOrder.Enabled = False
    End If

    'Edit -> Competitor Store
    mnuEdit_CompetitorStore_CompetitorPrices.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuEdit_CompetitorStore_CompetitorPrices")
    mnuEdit_CompetitorStore_CompetitorTrend.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuEdit_CompetitorStore_CompetitorTrend")
    mnuEdit_CompetitorStore_ImportCompetitorPrices.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuEdit_CompetitorStore_ImportCompetitorPrices")
    mnuEdit_CompetitorStore_CompetitorMarginImpact.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuEdit_CompetitorStore_CompetitorMarginImpact")
    mnuEdit_CompetitorStore_NationalPurchasingValues.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuEdit_CompetitorStore_NationalPurchasingValues")
    mnuEdit_CompetitorStore.Enabled = IsTopLevelEnabled(mnuEdit_CompetitorStore)

    'Edit -> Item
    mnuEdit_Item_Add.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_Item_Add")
    mnuEdit_Item_EditItem.Enabled = (gbSuperUser Or gbItemAdministrator Or gbDistributor Or gbBuyer Or gbDCAdmin Or gbPriceBatchProcessor Or gbTaxAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_Item_EditItem")
    mnuEdit_Item_ItemChain.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_Item_ItemChain")
    mnuEdit_Item_BulkLoadAuditHistory.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_Item_BulkLoadAuditHistory")
    mnuEdit_Item.Enabled = IsTopLevelEnabled(mnuEdit_Item)

    'Edit -> Orders
    mnuEdit_Orders_AddEdit.Enabled = (gbSuperUser Or gbAccountant Or gbBuyer Or gbCoordinator Or gbFacilityCreditProcessor Or gbDistributor Or gbDeletePO Or gbPOAccountant Or gbPOEditor Or gbWarehouse) And GetRegionalMenuAccess(ds, "mnuEdit_Orders_AddEdit")
    mnuEdit_Orders_Allocate.Enabled = (gbSuperUser Or gbWarehouse) And GetRegionalMenuAccess(ds, "mnuEdit_Orders_Allocate")
    mnuEdit_Orders_EInvoicing.Enabled = (gbSuperUser Or gbEInvoicing) And GetRegionalMenuAccess(ds, "mnuEdit_Orders_EInvoicing")
    mnuEdit_Orders_ItemQueue.Enabled = (gbSuperUser Or gbBuyer Or gbCoordinator Or gbPOAccountant Or gbPOApprovalAdmin) And GetRegionalMenuAccess(ds, "mnuEdit_Orders_ItemQueue")
    mnuEdit_Orders_InvoiceEntry.Enabled = (gbSuperUser Or gbAccountant Or gbPOAccountant Or gbPOApprovalAdmin Or gbDistributor) And GetRegionalMenuAccess(ds, "mnuEdit_Orders_InvoiceEntry")
    mnuEdit_Orders_SuspendedPOTool.Enabled = (gbSuperUser Or gbPOAccountant Or gbPOApprovalAdmin) And GetRegionalMenuAccess(ds, "mnuEdit_Orders_InvoiceMatching")
    mnuEdit_Orders_BatchReceiveClose.Enabled = (gbSuperUser Or gbPOAccountant) And GetRegionalMenuAccess(ds, "mnuEdit_Orders_BatchReceiveClose")
    mnuEdit_Orders.Enabled = IsTopLevelEnabled(mnuEdit_Orders)

    'Edit -> Pricing
    mnuEdit_Pricing_Batches.Enabled = (gbSuperUser Or gbItemAdministrator Or gbPriceBatchProcessor Or gbBatchBuildOnly) And GetRegionalMenuAccess(ds, "mnuEdit_Pricing_Batches")
    mnuEdit_Pricing_LineDrive.Enabled = False
    mnuEdit_Pricing_ReprintSigns.Enabled = (gbSuperUser Or gbPriceBatchProcessor Or gbItemAdministrator Or gbBuyer)
    mnuEdit_Pricing_PromotionalOffers.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_Pricing_PromotionalOffers")
    mnuEdit_Pricing_PriceChangeWizard.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_Pricing_PriceChangeWizard")
    mnuEdit_Pricing.Enabled = IsTopLevelEnabled(mnuEdit_Pricing)

    'Edit -> Tax Hosting
    mnuEdit_TaxHosting_TaxClassification.Enabled = (gbSuperUser Or gbTaxAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_TaxHosting_TaxClassification")
    mnuEdit_TaxHosting_TaxFlag.Enabled = (gbSuperUser Or gbTaxAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_TaxHosting_TaxFlag")
    mnuEdit_TaxHosting_TaxJurisdiction.Enabled = (gbSuperUser Or gbTaxAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_TaxHosting_TaxJurisdiction")
    mnuEdit_TaxHosting.Enabled = IsTopLevelEnabled(mnuEdit_TaxHosting)

    'Edit -> Vendors
    mnuEdit_Vendors_AddEdit.Enabled = (gbSuperUser Or gbVendorAdministrator Or gbDistributor Or gbDCAdmin Or gbBuyer) And GetRegionalMenuAccess(ds, "mnuEdit_Vendors_AddEdit")
    mnuEdit_Vendors_CostImportExceptions.Enabled = (gbSuperUser Or gbItemAdministrator) And GetRegionalMenuAccess(ds, "mnuEdit_Vendors_CostImportExceptions")
    mnuEdit_Vendors.Enabled = IsTopLevelEnabled(mnuEdit_Vendors)

    'Reports -> Accounting -> GL Summary
    mnuReports_Accounting_GLSummary_GLDistributionCheck.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_GLSummary_GLDistributionCheck")
    mnuReports_Accounting_GLSummary_GLSales.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_GLSummary_GLSales")
    mnuReports_Accounting_GLSummary_GLTransfers.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_GLSummary_GLTransfers")
    mnuReports_Accounting_GLSummary.Enabled = IsTopLevelEnabled(mnuReports_Accounting_GLSummary)

    'Reports -> Accounting
    mnuReports_Accounting_AdjustmentSummary.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_AdjustmentSummary")
    mnuReports_Accounting_GLUploads.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_GLUploads")
    mnuReports_Accounting_InvoiceDiscrepancies.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_InvoiceDiscrepancies")
    mnuReports_Accounting_InvoiceManifest.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_InvoiceManifest")
    mnuReports_Accounting_OutOfPeriodInvoiceReport.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_OutOfPeriodInvoiceReport")
    mnuReports_Accounting_PJReconcile.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_PJReconcile")
    mnuReports_Accounting_PurchaseAccrualReport.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_PurchaseAccrualReport")
    mnuReports_Accounting_PurchaseToSalesComp.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Accounting_PurchaseToSalesComp")
    mnuReports_Accounting.Enabled = IsTopLevelEnabled(mnuReports_Accounting)

    'Reports -> Audit
    mnuReports_Audit_AVCIExceptions.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Audit_AVCIExceptions")
    mnuReports_Audit.Enabled = IsTopLevelEnabled(mnuReports_Audit)

    'Reports -> Buyer
    mnuReports_Buyer_CostException.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Buyer_CostException")
    mnuReports_Buyer_InventoryBalance.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Buyer_InventoryBalance")
    mnuReports_Buyer_Margin.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Buyer_Margin")
    mnuReports_Buyer_StoreOrdersTotalBySKU.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Buyer_StoreOrdersTotalBySKU")
    mnuReports_Buyer_ZeroAverageCostItems.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Buyer_ZeroAverageCostItems")
    mnuReports_Buyer_ZeroPriceItems.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Buyer_ZeroPriceItems")
    mnuReports_Buyer_WarehouseMovement.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Buyer_WarehouseMovement")
    mnuReports_Buyer.Enabled = IsTopLevelEnabled(mnuReports_Buyer)

    'Reports -> Item
    mnuReports_Item_ActiveItemList.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_ActiveItemList")
    mnuReports_Item_DiscontinueItemsWithInventory.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_DiscontinueItemsWithInventory")
    mnuReports_Item_InventoryGuide.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_InventoryGuide")
    mnuReports_Item_ItemList.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_ItemList")
    mnuReports_Item_ItemOnHandComparisonBetweenLocation.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_ItemOnHandComparisonBetweenLocation")
    mnuReports_Item_ItemOrderHistory.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_ItemOrderHistory")
    mnuReports_Item_NotAvailableItems.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_NotAvailableItems")
    mnuReports_Item_ReorderSummaryByVendor.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_ReorderSummaryByVendor")
    mnuReports_Item_ShipperItems.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_ShipperItems")
    mnuReports_Item_UPCLabels.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_UPCLabels")
    mnuReports_Item_VendorItems.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Item_VendorItems")
    mnuReports_Item.Enabled = IsTopLevelEnabled(mnuReports_Item)

    'Reports -> Inventory
    mnuReports_Inventory_BOHCompare.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Inventory_BOHCompare")
    mnuReports_Inventory_CycleCountReports.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Inventory_CycleCountReports")
    mnuReports_Inventory_InventoryValue.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Inventory_InventoryValue")
    mnuReports_Inventory_InventoryWeeklyHistory.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Inventory_InventoryWeeklyHistory")
    mnuReports_Inventory_LocationItemsManulCountSheet.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Inventory_LocationItemsManulCountSheet")
    mnuReports_Inventory_LocationItemsReport.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Inventory_LocationItemsReport")
    mnuReports_Inventory_LocationsReport.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Inventory_LocationsReport")
    mnuReports_Inventory.Enabled = IsTopLevelEnabled(mnuReports_Inventory)

    'Reports -> Miscellaneous
    mnuReports_Miscellaneous_CasesBySubTeam.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Miscellaneous_CasesBySubTeam")
    mnuReports_Miscellaneous_CasesBySubTeamAudit.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Miscellaneous_CasesBySubTeamAudit")
    mnuReports_Miscellaneous_LotNo.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Miscellaneous_LotNo")
    mnuReports_Miscellaneous_VendorEfficiency.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Miscellaneous_VendorEfficiency")
    mnuReports_Miscellaneous.Enabled = IsTopLevelEnabled(mnuReports_Miscellaneous)

    'Reports -> Order
    mnuReports_Order_APUpload.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_APUpload")
    mnuReports_Order_APUPAccrualsClosed.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_APUPAccrualsClosed")
    mnuReports_Order_ClosedOrders.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_ClosedOrders")
    mnuReports_Order_ClosedOrdersMissingInvoiceData.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_ClosedOrdersMissingInvoiceData")
    mnuReports_Order_CreditReason.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_CreditReason")
    mnuReports_Order_DeletedOrders.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_DeletedOrders")
    mnuReports_Order_KitchenCaseTransfer.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_KitchenCaseTransfer")
    mnuReports_Order_OpenPurchaseOrders.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_OpenPurchaseOrders")
    mnuReports_Order_OrdersReceivedNotClosed.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_OrdersReceivedNotClosed")
    mnuReports_Order_OutOfStock.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_OutOfStock")
    mnuReports_Order_POCostChanges.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_POCostChanges")
    mnuReports_Order_Short.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_Short")
    mnuReports_Order.Enabled = IsTopLevelEnabled(mnuReports_Order)

    'Reports -> Price
    mnuReports_Price_Batches.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_Short")
    mnuReports_Price_ItemPrice.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_Short")
    mnuReports_Price_PriceChanges.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_Short")
    mnuReports_Price_Specials.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Order_Short")
    mnuReports_Price.Enabled = IsTopLevelEnabled(mnuReports_Price)

    'Reports -> Receiving/Distribution -> Average Cost
    mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance")
    mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport")
    mnuReports_ReceivingDistribution_AverageCost_DistributionMargin.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_AverageCost_DistributionMargin")
    mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge")
    mnuReports_ReceivingDistribution_AverageCost_List.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_AverageCost_List")
    mnuReports_ReceivingDistribution_AverageCost.Enabled = IsTopLevelEnabled(mnuReports_ReceivingDistribution_AverageCost)

    'Reports -> Receiving/Distribution -> COOL
    mnuReports_ReceivingDistribution_COOL_ReceivingReport.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_COOL_ReceivingReport")
    mnuReports_ReceivingDistribution_COOL_ShippingReport.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_COOL_ShippingReport")
    mnuReports_ReceivingDistribution_COOL.Enabled = IsTopLevelEnabled(mnuReports_ReceivingDistribution_COOL)

    'Reports -> Receiving/Distribution
    mnuReports_ReceivingDistribution_DailyReceiving.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_DailyReceiving")
    mnuReports_ReceivingDistribution_DCStoreRetailPriceReport.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_DCStoreRetailPriceReport")
    mnuReports_ReceivingDistribution_OrderFulfillment.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_OrderFulfillment")
    mnuReports_ReceivingDistribution_ReceivingLog.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReceivingDistribution_ReceivingLog")
    mnuReports_ReceivingDistribution.Enabled = IsTopLevelEnabled(mnuReports_ReceivingDistribution)

    'Reports -> 3-Way Matching
    mnuReports_3WayMatching_ControlGroupLog.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_3WayMatching_ControlGroupLog")
    mnuReports_3WayMatching_ControlGroup3WayMatch.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_3WayMatching_ControlGroup3WayMatch")
    mnuReports_3WayMatching.Enabled = IsTopLevelEnabled(mnuReports_3WayMatching)

    'Reports -> Tax
    mnuReports_Tax_NewItemTaxClass.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Tax_NewItemTaxClass")
    mnuReports_Tax_ModifiedItemTaxClass.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Tax_ModifiedItemTaxClass")
    mnuReports_Tax.Enabled = IsTopLevelEnabled(mnuReports_Tax)

    'Reports
    mnuReports_Purchases.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_Purchases")
    mnuReports_ReportManager.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuReports_ReportManager")
    mnuReports.Enabled = IsTopLevelEnabled(mnuReports)

    'Inventory Menu
    mnuInventory_Adjustments.Enabled = (gbSuperUser Or gbDCAdmin Or gbInventoryAdministrator) And GetRegionalMenuAccess(ds, "mnuInventory_Adjustments")
    mnuInventory_InventoryCosting.Enabled = (gbSuperUser Or gbCostAdmin) And GetRegionalMenuAccess(ds, "mnuInventory_InventoryCosting")
    mnuInventory_CycleCounts.Enabled = (gbSuperUser Or gbDCAdmin Or gbInventoryAdministrator) And GetRegionalMenuAccess(ds, "mnuInventory_CycleCounts")
    mnuInventory_Locations.Enabled = (gbSuperUser Or gbDCAdmin Or gbInventoryAdministrator) And GetRegionalMenuAccess(ds, "mnuInventory_Locations")
    mnuInventory_ShrinkCorrections.Enabled = (gbUserShrink Or gbUserShrinkAdmin Or gbCoordinator Or gbPOAccountant Or gbSuperUser) And GetRegionalMenuAccess(ds, "mnuInventory_WasteCorrections")

    'Inventory -> InventoryCosting
    mnuInventory_InventoryCosting_AverageCostAdjustment.Enabled = (gbSuperUser Or gbCostAdmin) And GetRegionalMenuAccess(ds, "mnuInventory_InventoryCosting_AverageCostAdjustment")
    ' The following menu has been disabled - We use the average cost update under Admin -> Scheduled Jobs    - Alex Z
    mnuInventory_InventoryCosting_AverageCostUpdate.Enabled = False

    'Tools Menu
    mnuTools_VendorList.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuTools_VendorList")
    mnuTools_UnitConverter.Enabled = OrderSearchDAO.IsMultipleJurisdiction()

    'Tools -> TGM
    mnuTools_TGM_NewTGMView.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuTools_TGM_NewTGMView")
    mnuTools_TGM_LoadTGMView.Enabled = (gbSuperUser Or Not blnIsAdminOnly) And GetRegionalMenuAccess(ds, "mnuTools_TGM_LoadTGMView")
    mnuTools_TGM.Enabled = mnuTools_TGM_NewTGMView.Enabled Or mnuTools_TGM_LoadTGMView.Enabled

    'Data Menu
    mnuData_BatchRollback.Enabled = (gbSuperUser Or gbDataAdministrator) And GetRegionalMenuAccess(ds, "mnuData_BatchRollback")
    mnuData_RestoreDeletedItem.Enabled = (gbSuperUser Or gbDataAdministrator) And GetRegionalMenuAccess(ds, "mnuData_RestoreDeletedItem")
    mnuData_ScalePOSPush.Enabled = (gbSuperUser Or gbDataAdministrator) And GetRegionalMenuAccess(ds, "mnuData_ScalePOSPush")
    mnuData_UnprocessedPushFiles.Enabled = (gbSuperUser Or gbDataAdministrator) And GetRegionalMenuAccess(ds, "mnuData_ScalePOSPush")
    mnuData_CancelAllSales.Enabled = (gbCancelAllSales) AndAlso InstanceDataDAO.IsFlagActive("EnableCancelAllSales")
    R10ItemRefreshToolStripMenuItem.Enabled = gbItemAdministrator
    IconItemRefreshMenuItem.Enabled = gbItemAdministrator
    SupportRestoreDeleteItemToolStripMenuItem.Enabled = gbSupportUser

    '######################
    ' Admin Menu access
    '######################

    'IRMA Configuration -> Application Configuration Menu
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmentReasons.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmentReasons")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementConfiguration.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementConfiguration")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingTolerance.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingTolerance")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits")
    LabelTypesToolStripMenuItem.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator)
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator Or gbTaxAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore")
    PriceTypesToolStripMenuItem.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator)
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelationships.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelationships")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance")
    mnuAdministration_Stores_BuildESTFile.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_Stores_BuildESTFile")
    StaticStoreFTPToolStripMenuItem.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator)
    SubteamDiscountExceptionsToolStripMenuItem.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator)
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_EatBy.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_EatBy")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_ExtraText.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_ExtraText")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Grade.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Grade")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelStyle.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelStyle")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelFormat.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelFormat")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelType.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelType")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_NutriFact.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_NutriFact")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_RandomWeightType.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_RandomWeightType")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tare.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tare")
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance.Enabled = IsTopLevelEnabled(mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance)
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration.Enabled = IsTopLevelEnabled(mnuAdministration_IRMAConfiguration_ApplicationConfiguration)
    mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement")
    NoTagLogicToolStripMenuItem.Enabled = (gbSuperUser Or gbApplicationConfigurationAdministrator)

    'IRMA Configuration -> System Configuration Menu
    mnuAdministration_SystemConfiguration_BuildConfiguration.Enabled = (gbSuperUser Or gbSystemConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_BuildConfiguration")
    mnuAdministration_SystemConfiguration_DataArchiving.Enabled = (gbSystemConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_DataArchiving")
    mnuAdministration_SystemConfiguration_InstanceData.Enabled = (gbSuperUser Or gbSystemConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_InstanceData")
    mnuAdministration_SystemConfiguration_InstanceDataFlags.Enabled = (gbSuperUser Or gbSystemConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_InstanceDataFlags")
    mnuAdministration_SystemConfiguration_ManageConfiguration.Enabled = (gbSuperUser Or gbSystemConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_ManageConfiguration")
    StaticStoreFTPToolStripMenuItem.Enabled = (gbSuperUser Or gbSystemConfigurationAdministrator Or gbPriceBatchProcessor) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_ManageConfiguration")

    mnuAdministration_SystemConfiguration_ManageRetentionPolicies.Enabled = (gbSuperUser Or gbSystemConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_ManageRetentionPolicies")

    ' As of 4.3 this menu has been made obsolete. Use reasoncode maintenance instead.
    'mnuAdministration_SystemConfiguration_ResolutionCodes.Enabled = (gbSuperUser Or gbSystemConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_ResolutionCodes")
    mnuAdministration_SystemConfiguration_ResolutionCodes.Visible = False
    mnuAdministration_SystemConfiguration_ResolutionCodes.Enabled = False

    mnuAdministration_SystemConfiguration_ReasonCodeMaintenance.Enabled = (gbSuperUser Or gbSystemConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_ReasonCodeMaintenance")

    mnuAdministration_SystemConfiguration_MenuAccess.Enabled = (gbSuperUser Or gbSystemConfigurationAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_SystemConfiguration_MenuAccess")

    mnuAdministration_IRMAConfiguration_SystemConfiguration.Enabled = IsTopLevelEnabled(mnuAdministration_IRMAConfiguration_SystemConfiguration)

    mnuAdministration_IRMAConfiguration.Enabled = mnuAdministration_IRMAConfiguration_ApplicationConfiguration.Enabled Or mnuAdministration_IRMAConfiguration_SystemConfiguration.Enabled

    'POS Interface Menu
    mnuAdministration_POSInterface_FileWriter.Enabled = (gbSuperUser Or gbPOSInterfaceAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_POSInterface_FileWriter")
    mnuAdministration_POSInterface_StoreFTPConfiguration.Enabled = (gbSuperUser Or gbPOSInterfaceAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_POSInterface_StoreFTPConfiguration")
    mnuAdministration_POSInterface_PricingMethods.Enabled = (gbSuperUser Or gbPOSInterfaceAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_POSInterface_PricingMethods")
    mnuAdministration_POSInterface.Enabled = IsTopLevelEnabled(mnuAdministration_POSInterface)

    'Scheduled Jobs Menu
    mnuAdministration_ScheduledJobs_APUpload.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_APUpload")
    mnuAdministration_ScheduledJobs_AuditExceptionsReport.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_AuditExceptionsReport")
    mnuAdministration_ScheduledJobs_AverageCostUpdate.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_AverageCostUpdate")
    mnuAdministration_ScheduledJobs_CloseReceiving.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_CloseReceiving")
    mnuAdministration_ScheduledJobs_PLUMHost.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_PLUMHost")
    mnuAdministration_ScheduledJobs_POSPull.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_POSPull")
    mnuAdministration_ScheduledJobs_SendOrders.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_SendOrders")
    mnuAdministration_ScheduledJobs_TlogProcessing.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_TlogProcessing")
    mnuAdministration_ScheduledJobs_ViewAppLogs.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_ViewAppLogs")
    mnuAdministration_ScheduledJobs_WeeklySalesRollup.Enabled = (gbSuperUser Or gbJobAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_ScheduledJobs_WeeklySalesRollup")
    mnuAdministration_ScheduledJobs.Enabled = IsTopLevelEnabled(mnuAdministration_ScheduledJobs)

    'Store Menu
    mnuAdministration_Stores_CreateStore.Enabled = (gbSuperUser Or gbStoreAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_Stores_CreateStore")
    mnuAdministration_Stores_BuildPOSFile.Enabled = (gbSuperUser Or gbStoreAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_Stores_BuildPOSFile")
    mnuAdministration_Stores_BuildScaleFile.Enabled = (gbSuperUser Or gbStoreAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_Stores_BuildScaleFile")
    mnuAdministration_Stores_SendStoreToMammoth.Enabled = (gbSuperUser Or gbStoreAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_Stores_SendStoreToMammoth")
    mnuAdministration_Stores.Enabled = IsTopLevelEnabled(mnuAdministration_Stores)

    'Users Menu
    mnuAdministration_Users_ManageTitles.Enabled = (gbSuperUser Or gbSecurityAdministrator) And GetRegionalMenuAccess(ds, "mnuAdministration_Users_ManageTitles")
    mnuAdministration_Users_ManageUsers.Enabled = (gbSuperUser Or gbSecurityAdministrator Or gbUserMaintenance) And GetRegionalMenuAccess(ds, "mnuAdministration_Users_ManageUsers")
    mnuAdministration_Users.Enabled = IsTopLevelEnabled(mnuAdministration_Users)

    'hide all menus that have Enabled = False
    For Each mnuTopLevel In Me.MainMenu1.Items
      SetMenuVisibility(mnuTopLevel)
    Next

    logger.Debug("SetMenuAccess exit")
  End Sub

  Private Sub SetMenuVisibility(ByVal mnu As ToolStripMenuItem)
    Dim mnuSubItem As ToolStripMenuItem

    For Each mnuSubItem In mnu.DropDownItems
      If mnuSubItem.HasDropDownItems Then
        SetMenuVisibility(mnuSubItem)
      Else
        If Not mnuSubItem.Enabled Then
          mnuSubItem.Visible = False
          mnuSubItem.Text = ""
          mnuSubItem.ShortcutKeys = Nothing
        Else
          mnuSubItem.Visible = True
        End If
      End If
    Next

    mnu.Visible = mnu.HasDropDownItems
  End Sub

  Private Function GetRegionalMenuAccess(ByVal ds As DataSet, ByVal sMenuName As String) As Boolean
    Dim dv As New DataView(ds.Tables(0))
    Dim row As DataRowView = Nothing

    'Never hide the MenuAccess Screen
    If sMenuName = "mnuAdministration_SystemConfiguration_MenuAccess" Then
      Return True
    Else
      dv.RowFilter = "MenuName = '" & sMenuName & "'"
      If dv.Count > 0 Then row = dv.Item(0)

      If Not row Is Nothing Then
        Return CBool(row("Visible"))
      Else
        Return True
      End If
    End If
  End Function

  Private Sub AVCIExceptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Audit_AVCIExceptions.Click
    logger.Debug("AVCIExceptions_Click entry")
    frmAVCIAudit.ShowDialog()
    frmAVCIAudit.Close()
    frmAVCIAudit.Dispose()
    logger.Debug("AVCIExceptions_Click exit")
  End Sub

  Private Sub TaxClassificationToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_TaxHosting_TaxClassification.Click
    logger.Debug("TaxClassificationToolStripMenuItem1_Click entry")
    Dim taxClassForm As New Form_ManageTaxClass
    taxClassForm.ShowDialog()
    taxClassForm.Close()
    taxClassForm.Dispose()
    logger.Debug("TaxClassificationToolStripMenuItem1_Click exit")
  End Sub

  Private Sub TaxFlagToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_TaxHosting_TaxFlag.Click
    logger.Debug("TaxFlagToolStripMenuItem1_Click entry")
    Dim taxFlagForm As New Form_ManageTaxFlag
    taxFlagForm.ShowDialog()
    taxFlagForm.Close()
    taxFlagForm.Dispose()
    logger.Debug("TaxFlagToolStripMenuItem1_Click exit")
  End Sub

  Private Sub TaxJurisdictionToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_TaxHosting_TaxJurisdiction.Click
    logger.Debug("TaxJurisdictionToolStripMenuItem1_Click entry")
    Dim taxJurisdictionForm As New Form_ManageTaxJurisdiction
    taxJurisdictionForm.ShowDialog()
    taxJurisdictionForm.Close()
    taxJurisdictionForm.Dispose()
    logger.Debug("TaxJurisdictionToolStripMenuItem1_Click exit")
  End Sub

  Private Sub EPromotionsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Pricing_PromotionalOffers.Click, tsbEdit_Pricing_PromotionalOffers.Click
    logger.Debug("EPromotionsToolStripMenuItem1_Click entry")
    Dim frm As Form_PromotionOfferGrid
    frm = New Form_PromotionOfferGrid
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("EPromotionsToolStripMenuItem1_Click exit")
  End Sub

  Private Sub mnuItemBulkLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFile_Import_ItemMaintenanceBulkLoad.Click
    logger.Debug("mnuItemBulkLoad_Click entry")
    ImportData.ShowDialog()
    ImportData.Close()
    ImportData.Dispose()
    logger.Debug("mnuItemBulkLoad_Click exit")
  End Sub

  Private Sub mnuImportCompetitorPrices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_CompetitorStore_ImportCompetitorPrices.Click
    logger.Debug("mnuImportCompetitorPrices_Click entry")
    Dim result As DialogResult
    'Dim _selectedCompetitorID As Nullable(Of Integer)
    'Dim _selectedCompetitorLocationID As Nullable(Of Integer)
    'Dim _selectedCompetitorStoreID As Nullable(Of Integer)
    Dim _selectedCompetitorStore As WholeFoods.IRMA.CompetitorStore.BusinessLogic.CompetitorStoreDataSet.CompetitorStoreRow
    Dim _selectedFiscalWeekDescription As String

    Using form As New WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorFileUpload
      result = form.ShowDialog()
      '_selectedCompetitorID = 
      '_selectedCompetitorLocationID = 
      '_selectedCompetitorStoreID = form.SelectedCompetitorStoreID
      _selectedCompetitorStore = form.SelectedCompetitorStore
      _selectedFiscalWeekDescription = form.SelectedFiscalWeekDescription
    End Using

    If result = Windows.Forms.DialogResult.OK Then
      ' Import succeeded
      'If _selectedCompetitorStoreID.HasValue AndAlso 
      If _selectedCompetitorStore IsNot Nothing AndAlso Not String.IsNullOrEmpty(_selectedFiscalWeekDescription) Then
        ' Open up the competitor price management form
        Using form As New WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorDataManagement(_selectedCompetitorStore.CompetitorID, _selectedCompetitorStore.CompetitorLocationID, _selectedCompetitorStore.CompetitorStoreID, _selectedFiscalWeekDescription)
          form.ShowDialog()
        End Using
      Else
        ' More than one store or fiscal week was imported
        MessageBox.Show("Imported prices can be viewed on the Competitor Data Management screen.")
      End If
    End If
    logger.Debug("mnuImportCompetitorPrices_Click exit")
  End Sub

  Private Sub mnuCompetitorPrices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_CompetitorStore_CompetitorPrices.Click
    logger.Debug("mnuCompetitorPrices_Click entry")
    Using form As New WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorDataManagement()
      form.ShowDialog()
    End Using
    logger.Debug("mnuCompetitorPrices_Click exit")
  End Sub

  Private Sub mnuNationalPurchasingValues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_CompetitorStore_NationalPurchasingValues.Click
    logger.Debug("mnuNationalPurchasingValues_Click entry")
    Using form As New WholeFoods.IRMA.CompetitorStore.UserInterface.frmNationalPurchasingValue
      form.ShowDialog()
    End Using
    logger.Debug("mnuNationalPurchasingValues_Click exit")
  End Sub

  Private Sub mnuCompetitorMarginImpact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_CompetitorStore_CompetitorMarginImpact.Click
    logger.Debug("mnuCompetitorMarginImpact_Click entry")
    Using form As New WholeFoods.IRMA.Reporting.UserInterface.frmCompetitorMarginImpact()
      form.ShowDialog()
    End Using
    logger.Debug("mnuCompetitorMarginImpact_Click exit")
  End Sub

  Private Sub mnuCompetitorTrend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_CompetitorStore_CompetitorTrend.Click
    logger.Debug("mnuCompetitorTrend_Click entry")
    Using form As New WholeFoods.IRMA.Reporting.UserInterface.frmCompetitiorTrend()
      form.ShowDialog()
    End Using
    logger.Debug("mnuCompetitorTrend_Click exit")
  End Sub

  Private Sub mnuItemStoreBulkLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFile_Import_ItemStoreMaintenanceBulkLoad.Click
    logger.Debug("mnuItemStoreBulkLoad_Click entry")
    MsgBox("Coming Soon...")
    logger.Debug("mnuItemStoreBulkLoad_Click exit")
  End Sub

  Private Sub mnuItemVendorBulkLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFile_Import_ItemVendorMaintenanceBulkLoad.Click
    logger.Debug("mnuItemVendorBulkLoad_Click entry")
    MsgBox("Coming Soon...")
    logger.Debug("mnuItemVendorBulkLoad_Click exit")
  End Sub

  Private Sub mnuItemAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Item_Add.Click, tsbEdit_Item_AddNewItem.Click
    logger.Debug("mnuItemAdd_Click entry")
    glItemID = 0

    If InstanceDataDAO.IsFlagActive("NewItemAutoSku") Then
      '-- Using Auto SKU
      '-- The identifier will be a sku and its value will be the auto generated
      '-- value from the Item's PK. This is set in a stored proc when the user adds the item.
      Dim fItemAdd As New frmItemAdd

      '-- Hardcode ident type to SKU and set the value to nothing
      '-- to tell the stored proc to assign the Item's PK value as the SKU value.
      fItemAdd.psIdentifierType = "S"
      fItemAdd.psIdentifier = Nothing
      fItemAdd.psCheckDigit = Nothing
      fItemAdd.ShowDialog()
      fItemAdd.Close()
      fItemAdd.Dispose()
    Else
      '-- Call the ITEM IDENTIFIER form to start the add process
      With frmItemIdentifierAdd
        .pbAddToDatabase = False
        .ShowDialog()
        .Close()
        .Dispose()
      End With
    End If

    '-- a new Item was added; show the item
    If glItemID <> 0 Then
      frmItem.ShowDialog()
      frmItem.Close()
      frmItem.Dispose()
    End If
    logger.Debug("mnuItemAdd_Click exit")
  End Sub

  Private Sub mnuItemEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Item_EditItem.Click, tsbEdit_Item_EditExistingItem.Click
    logger.Debug("mnuItemEdit_Click entry")

    frmItemSearch.ShowDialog()
    frmItemSearch.Close()
    frmItemSearch.Dispose()

    '-- if its not zero, then something was found
    If glItemID <> 0 Then
      frmItem.ShowDialog()
      frmItem.Close()
      frmItem.Dispose()
    End If
    logger.Debug("mnuItemEdit_Click exit")
  End Sub

  Private Sub mnuItemChain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Item_ItemChain.Click
    logger.Debug("mnuItemChain_Click entry")
    Dim frmItemChaining As WholeFoods.IRMA.ItemChaining.UserInterface.ItemChaining = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemChaining()

    frmItemChaining.ShowDialog()
    frmItemChaining.Dispose()
    logger.Debug("mnuItemChain_Click exit")
  End Sub

  Private Sub mnuItemChainingBatchWizardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Pricing_PriceChangeWizard.Click
    logger.Debug("mnuItemChainingBatchWizardToolStripMenuItem_Click entry")
    Dim frmPriceChange As WholeFoods.IRMA.ItemChaining.UserInterface.frmPriceChange2 = New WholeFoods.IRMA.ItemChaining.UserInterface.frmPriceChange2()

    frmPriceChange.ShowDialog()
    frmPriceChange.Dispose()
    logger.Debug("mnuItemChainingBatchWizardToolStripMenuItem_Click exit")
  End Sub

  Private Sub mnuItemBulkLoadAuditHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Item_BulkLoadAuditHistory.Click
    logger.Debug("mnuItemBulkLoadAuditHistory_Click entry")
    ImportDataHistory.ShowDialog()
    ImportDataHistory.Close()
    ImportDataHistory.Dispose()
    logger.Debug("mnuItemBulkLoadAuditHistory_Click exit")
  End Sub

  Private Sub mnuReportManager_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuReports_ReportManager.Click
    logger.Debug("mnuReportManager_Click entry")
    Dim reportmanager As String
    reportmanager = ConfigurationServices.AppSettings("reportManagerURL")
    Call GoToURL(reportmanager)
    logger.Debug("mnuReportManager_Click exit")
  End Sub

  Private Sub mnuAttributeDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults.Click
    logger.Debug("mnuAttributeDefaults_Click entry")
    Dim frmDefaultAttributeValues As New DefaultAttributeValues()

    frmDefaultAttributeValues.ShowDialog()
    frmDefaultAttributeValues.Close()
    frmDefaultAttributeValues.Dispose()
    logger.Debug("mnuAttributeDefaults_Click exit")
  End Sub

  Private Sub menuTeamMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team.Click
    logger.Debug("menuTeamMaintenance_Click entry")
    Dim frm As Team = New Team()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("menuTeamMaintenance_Click exit")
  End Sub

  Private Sub mnuSubTeamMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance.Click
    logger.Debug("mnuSubTeamMaintenance_Click entry")
    Dim frm As SubTeam = New SubTeam()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuSubTeamMaintenance_Click exit")
  End Sub

  Private Sub mnuStoreSubTeamRelationships_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelationships.Click
    logger.Debug("mnuStoreSubTeamRelationships_Click entry")
    Dim frm As StoreSubTeam = New StoreSubTeam()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuStoreSubTeamRelationships_Click exit")
  End Sub

  Private Sub mnuScaleEatBy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_EatBy.Click
    logger.Debug("mnuScaleEatBy_Click entry")
    Dim frm As Form_EatBy = New Form_EatBy()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuScaleEatBy_Click exit")
  End Sub

  Private Sub mnuScaleGradeTool_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Grade.Click
    logger.Debug("mnuScaleGradeTool_Click entry")
    Dim frm As Form_Grade = New Form_Grade()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuScaleGradeTool_Click exit")
  End Sub

  Private Sub mnuKitchenCaseXfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Order_KitchenCaseTransfer.Click
    logger.Debug("mnuKitchenCaseXfer_Click entry")
    KitchenCaseXferRpt.ShowDialog()
    KitchenCaseXferRpt.Dispose()
    logger.Debug("mnuKitchenCaseXfer_Click exit")
  End Sub

  Private Sub mnuScaleLabelFormat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelFormat.Click
    logger.Debug("mnuScaleLabelFormat_Click entry")
    Dim frm As Form_LabelFormat = New Form_LabelFormat()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuScaleLabelFormat_Click exit")
  End Sub

  Private Sub menuLabelStyle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelStyle.Click
    logger.Debug("menuLabelStyle_Click entry")
    Dim frm As Form_LabelStyle = New Form_LabelStyle()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("menuLabelStyle_Click exit")
  End Sub

  Private Sub mnuLabelType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelType.Click
    logger.Debug("mnuLabelType_Click entry")
    Dim frm As Form_LabelType = New Form_LabelType()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuLabelType_Click exit")
  End Sub

  Private Sub mnuRandomWeightType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_RandomWeightType.Click
    logger.Debug("mnuRandomWeightType_Click entry")
    Dim frm As Form_RandomWeightType = New Form_RandomWeightType()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuRandomWeightType_Click exit")
  End Sub

  Private Sub mnuTareTool_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tare.Click
    logger.Debug("mnuTareTool_Click entry")
    Dim frm As Form_Tare = New Form_Tare()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuTareTool_Click exit")
  End Sub

  Private Sub mnuEIM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFile_Import_ExtendedItemMaintenance.Click
    logger.Debug("mnuEIM_Click entry")
    Dim frm As New ExtendedItemMaintenanceForm()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuEIM_Click exit")
  End Sub

  Private Sub mnuNutriFact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_NutriFact.Click
    logger.Debug("mnuNutriFact_Click entry")
    Dim nutrifactForm As New Form_Nutrifact()
    nutrifactForm.ShowDialog(Me)
    nutrifactForm.Close()
    nutrifactForm.Dispose()
    logger.Debug("mnuNutriFact_Click exit")
  End Sub

  Private Sub mnuScaleExtraText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_ExtraText.Click
    logger.Debug("mnuScaleExtraText_Click entry")
    Dim extraTextForm As New ExtraTextLookup
    extraTextForm.ShowDialog(Me)
    extraTextForm.Close()
    extraTextForm.Dispose()
    logger.Debug("mnuScaleExtraText_Click exit")
  End Sub

  Private Sub mnuPlanogramImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFile_Import_Planogram.Click
    logger.Debug("mnuPlanogramImport_Click entry")

    If SlawPrintBatchConfigurationDAO.SlawPrintRequestsEnabledForRegion() Then
      Using planogramImportForm As PlanogramImport = New PlanogramImport()
        planogramImportForm.ShowDialog()
      End Using
    Else
      Using planogramImportForm As Form_PlanogramImport = New Form_PlanogramImport()
        planogramImportForm.ShowDialog()
      End Using
    End If

    logger.Debug("mnuPlanogramImport_Click exit")
  End Sub

  Private Sub mnuPlanogramExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFile_Export_Planogram.Click
    logger.Debug("mnuPlanogramExport_Click entry")
    Dim frm As Form_PlanogramExport = New Form_PlanogramExport()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuPlanogramExport_Click exit")
  End Sub

  Private Sub mnuStoreOrdersTotalBySKU_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Buyer_StoreOrdersTotalBySKU.Click
    logger.Debug("mnuStoreOrdersTotalBySKU_Click entry")
    Dim frm As frmStoreOrdersTotBySKUReport = New frmStoreOrdersTotBySKUReport()
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuStoreOrdersTotalBySKU_Click exit")
  End Sub

#Region "ToolStrip Container"

  Private Sub SetToolbarAccess()
    logger.Debug("SetToolbarAccess entry")

    If Not WholeFoods.IRMA.Common.DataAccess.InstanceDataDAO.IsFlagActive("ShowToolStripContainer") Then
      'toolstrip container remains hidden
      ToolStripContainer1.TopToolStripPanelVisible = False
      Exit Sub
    End If

    ToolStripContainer1.TopToolStripPanelVisible = True

    'blank out the text if the menu is invisible so that the menu size will autosize without being too large
    tsbFile_Export.Visible = mnuFile_Export.Available
    tsbFile_Export_RGIS.Visible = mnuFile_Export_RGIS.Available
    tsbFile_Export_RGIS.Text = IIf(mnuFile_Export_RGIS.Available, tsbFile_Export_RGIS.Text, "")
    tsbFile_Export_Planogram.Visible = mnuFile_Export_Planogram.Available
    tsbFile_Export_Planogram.Text = IIf(mnuFile_Export_Planogram.Available, tsbFile_Export_Planogram.Text, "")

    'blank out the text if the menu is invisible so that the menu size will autosize without being too large
    tsbFile_Import.Visible = mnuFile_Import.Available
    tsbFile_Import_EIM.Visible = mnuFile_Import_ExtendedItemMaintenance.Available
    tsbFile_Import_EIM.Text = IIf(mnuFile_Import_ExtendedItemMaintenance.Available, tsbFile_Import_EIM.Text, "")
    tsbFile_Import_Order.Visible = mnuFile_Import_ImportOrder.Available
    tsbFile_Import_Order.Text = IIf(mnuFile_Import_ImportOrder.Available, tsbFile_Import_Order.Text, "")
    tsbFile_Import_ItemMaintenanceBulkLoad.Visible = mnuFile_Import_ItemMaintenanceBulkLoad.Available
    tsbFile_Import_ItemMaintenanceBulkLoad.Text = IIf(mnuFile_Import_ItemMaintenanceBulkLoad.Available, tsbFile_Import_ItemMaintenanceBulkLoad.Text, "")
    tsbFile_Import_ItemStoreMaintenanceBulkLoad.Visible = mnuFile_Import_ItemStoreMaintenanceBulkLoad.Available
    tsbFile_Import_ItemStoreMaintenanceBulkLoad.Text = IIf(mnuFile_Import_ItemStoreMaintenanceBulkLoad.Available, tsbFile_Import_ItemStoreMaintenanceBulkLoad.Text, "")
    tsbFile_Import_ItemVendorMaintenanceBulkLoad.Visible = mnuFile_Import_ItemVendorMaintenanceBulkLoad.Available
    tsbFile_Import_ItemVendorMaintenanceBulkLoad.Text = IIf(mnuFile_Import_ItemVendorMaintenanceBulkLoad.Available, tsbFile_Import_ItemVendorMaintenanceBulkLoad.Text, "")
    tsbFile_Import_Planogram.Visible = mnuFile_Import_Planogram.Available
    tsbFile_Import_Planogram.Text = IIf(mnuFile_Import_Planogram.Available, tsbFile_Import_Planogram.Text, "")
    tsbFile_Import_RipeOrder.Visible = mnuFile_Import_RIPEOrder.Available
    tsbFile_Import_RipeOrder.Text = IIf(mnuFile_Import_RIPEOrder.Available, tsbFile_Import_RipeOrder.Text, "")

    tsbEdit_Item_AddNewItem.Visible = mnuEdit_Item_Add.Available
    tsbEdit_Item_EditExistingItem.Visible = mnuEdit_Item_EditItem.Available
    tsbEdit_Orders_AddEdit.Visible = mnuEdit_Orders_AddEdit.Available
    tsbEdit_Item_InventoryLevel.Visible = mnuEdit_Item_EditItem.Available And Not gbTaxAdministrator 'This is the Inventory Level button on the item screen
    tsbEdit_Pricing_Batches.Visible = mnuEdit_Pricing_Batches.Available
    tsbEdit_Pricing_ReprintSigns.Visible = mnuEdit_Pricing_ReprintSigns.Available
    tsbEdit_Pricing_PromotionalOffers.Visible = mnuEdit_Pricing_PromotionalOffers.Available
    tsbReports_ReportManager.Visible = mnuReports_ReportManager.Available

    logger.Debug("SetToolbarAccess exit")
  End Sub

#Region "ToolStrip_FileImportExport"
  Private Sub tsbFile_Export_RGIS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbFile_Export_RGIS.Click
    logger.Debug("tsbFile_Export_RGIS_Click entry")
    Call mnuFile_Export_RGIS.PerformClick()
    logger.Debug("tsbFile_Export_RGIS_Click exit")
  End Sub

  Private Sub tsbFile_Export_Planogram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbFile_Export_Planogram.Click
    logger.Debug("tsbFile_Export_Planogram_Click entry")
    Call mnuFile_Export_Planogram.PerformClick()
    logger.Debug("tsbFile_Export_Planogram_Click exit")
  End Sub

  Private Sub tsbFile_Import_EIM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbFile_Import_EIM.Click
    logger.Debug("tsbFile_Import_EIM_Click entry")
    Call mnuFile_Import_ExtendedItemMaintenance.PerformClick()
    logger.Debug("tsbFile_Import_EIM_Click exit")
  End Sub

  Private Sub tsbFile_Import_ItemMaintenanceBulkLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbFile_Import_ItemMaintenanceBulkLoad.Click
    logger.Debug("tsbFile_Import_ItemMaintenanceBulkLoad_Click entry")
    Call mnuFile_Import_ItemMaintenanceBulkLoad.PerformClick()
    logger.Debug("tsbFile_Import_ItemMaintenanceBulkLoad_Click exit")
  End Sub

  Private Sub tsbFile_Import_ItemStoreMaintenanceBulkLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbFile_Import_ItemStoreMaintenanceBulkLoad.Click
    logger.Debug("tsbFile_Import_ItemStoreMaintenanceBulkLoad_Click entry")
    Call mnuFile_Import_ItemStoreMaintenanceBulkLoad.PerformClick()
    logger.Debug("tsbFile_Import_ItemStoreMaintenanceBulkLoad_Click exit")
  End Sub

  Private Sub tsbFile_Import_ItemVendorMaintenanceBulkLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbFile_Import_ItemVendorMaintenanceBulkLoad.Click
    logger.Debug("tsbFile_Import_ItemVendorMaintenanceBulkLoad_Click entry")
    Call mnuFile_Import_ItemVendorMaintenanceBulkLoad.PerformClick()
    logger.Debug("tsbFile_Import_ItemVendorMaintenanceBulkLoad_Click exit")
  End Sub

  Private Sub tsbFile_Import_Order_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbFile_Import_Order.Click
    logger.Debug("tsbFile_Import_Order_Click entry")
    Call mnuFile_Import_ImportOrder.PerformClick()
    logger.Debug("tsbFile_Import_Order_Click exit")
  End Sub

  Private Sub tsbFile_Import_Planogram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbFile_Import_Planogram.Click
    logger.Debug("tsbFile_Import_Planogram_Click entry")
    Call mnuFile_Import_Planogram.PerformClick()
    logger.Debug("tsbFile_Import_Planogram_Click exit")
  End Sub

  Private Sub tsbFile_Import_RipeOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbFile_Import_RipeOrder.Click
    logger.Debug("tsbFile_Import_RipeOrder_Click entry")
    Call mnuFile_Import_RIPEOrder.PerformClick()
    logger.Debug("tsbFile_Import_RipeOrder_Click exit")
  End Sub
#End Region

#End Region

  Private Sub BOHToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Inventory_BOHCompare.Click
    logger.Debug("BOHToolStripMenuItem_Click entry")
    Dim frm As BOHCompareReport = New BOHCompareReport
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("BOHToolStripMenuItem_Click exit")
  End Sub

  Private Sub mnuPurchaseAccrualReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Accounting_PurchaseAccrualReport.Click
    logger.Debug("mnuPurchaseAccrualReportToolStripMenuItem_Click entry")
    ' Calling Purchase Accrual Report
    Dim frm As PurchaseAccrualsReport = New PurchaseAccrualsReport
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuPurchaseAccrualReportToolStripMenuItem_Click exit")
  End Sub

  Private Sub APUPAccrualsClosed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Order_APUPAccrualsClosed.Click
    logger.Debug("mnuOrderReports_APUploadAccruals_Click entry")
    frmAPUPAccrualsClosedReport.ShowDialog()
    frmAPUPAccrualsClosedReport.Dispose()
    logger.Debug("mnuOrderReports_APUploadAccruals_Click exit")
  End Sub

  Private Sub mnuDailyItemAverageCostChgsRpt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport.Click
    logger.Debug("mnuDailyItemAverageCostChgsRpt_Click entry")
    DailyItemAvgCostChgsRpt.ShowDialog()
    DailyItemAvgCostChgsRpt.Dispose()
    logger.Debug("mnuDailyItemAverageCostChgsRpt_Click exit")
  End Sub

  Private Sub mnuShort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Order_Short.Click
    logger.Debug("mnuShort_Click entry")
    ShortReport.ShowDialog()
    ShortReport.Dispose()
    logger.Debug("mnuShort_Click exit")
  End Sub

  Private Sub mnu_ControlGroup3WayMatchLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_3WayMatching_ControlGroupLog.Click
    logger.Debug("mnu_ControlGroup3WayMatchLog_Click entry")
    ControlGroup3WayMatchLog.ShowDialog()
    ControlGroup3WayMatchLog.Dispose()
    logger.Debug("mnu_ControlGroup3WayMatchLog_Click exit")
  End Sub

  Private Sub mnu_ControlGroup3WayMatchDetailSummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_3WayMatching_ControlGroup3WayMatch.Click
    logger.Debug("mnu_ControlGroup3WayMatchDetailSummaryToolStripMenuItem_Click entry")
    ThreeWayMatchDetailSummary.ShowDialog()
    ThreeWayMatchDetailSummary.Dispose()
    logger.Debug("mnu_ControlGroup3WayMatchDetailSummaryToolStripMenuItem_Click exit")
  End Sub

  Private Sub mnuOrderInvoiceEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Orders_InvoiceEntry.Click
    logger.Debug("mnuOrderInvoiceEntry_Click entry")
    InvoiceControlGroup.ShowDialog()
    InvoiceControlGroup.Dispose()
    logger.Debug("mnuOrderInvoiceEntry_Click exit")
  End Sub

  Private Sub mnuOrderInvoiceMatching_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Orders_SuspendedPOTool.Click
    logger.Debug("mnuOrderInvoiceMatching_Click entry")
    SuspendedPOTool.ShowDialog()
    SuspendedPOTool.Dispose()
    logger.Debug("mnuOrderInvoiceMatching_Click exit")
  End Sub
  Private Sub mnuInvoiceMatchingTolerance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingTolerance.Click
    logger.Debug("mnuInvoiceMatchingTolerance_Click entry")
    frm_InvoiceMatchingTolerance.ShowDialog()
    frm_InvoiceMatchingTolerance.Dispose()
    logger.Debug("mnuInvoiceMatchingTolerance_Click exit")
  End Sub

  Private Sub MnuInventoryWeeklyHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Inventory_InventoryWeeklyHistory.Click
    logger.Debug("MnuInventoryWeeklyHistory_Click entry")
    InventoryWeeklyHistoryReport.ShowDialog()
    InventoryWeeklyHistoryReport.Dispose()
    logger.Debug("MnuInventoryWeeklyHistory_Click exit")
  End Sub

  Private Sub mnuReportsDASHandlingCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge.Click
    logger.Debug("mnuReportsDASHandlingCharge_Click entry")
    DASHandlingChargeReport.ShowDialog()
    DASHandlingChargeReport.Dispose()
    logger.Debug("mnuReportsDASHandlingCharge_Click exit")
  End Sub

  Private Sub OrderWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow.Click
    logger.Debug("OrderWindowToolStripMenuItem_Click entry")
    Dim orderwindowForm As New OrderWindow
    orderwindowForm.ShowDialog()
    orderwindowForm.Dispose()
    logger.Debug("OrderWindowToolStripMenuItem_Click exit")
  End Sub

  Private Sub mnuCOOLReceiving_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_ReceivingDistribution_COOL_ReceivingReport.Click
    logger.Debug("mnuCOOLReceiving_Click entry")
    COOLReceiving.ShowDialog()
    COOLReceiving.Dispose()
    logger.Debug("mnuCOOLReceiving_Click exit")
  End Sub

  Private Sub mnuCOOLShipping_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_ReceivingDistribution_COOL_ShippingReport.Click
    logger.Debug("mnuCOOLShipping_Click entry")
    COOLShipping.ShowDialog()
    COOLShipping.Dispose()
    logger.Debug("mnuCOOLShipping_Click exit")
  End Sub
  Private Sub mnuDCStoreRetailPriceReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_ReceivingDistribution_DCStoreRetailPriceReport.Click
    logger.Debug("mnuDCStoreRetailPriceReport entry")
    frmDCStoreRetailPriceReport.ShowDialog()
    frmDCStoreRetailPriceReport.Dispose()
    logger.Debug("mnuDCStoreRetailPriceReport exit")
  End Sub

  Private Sub mnuAvgCostAdjust_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuInventory_InventoryCosting_AverageCostAdjustment.Click
    logger.Debug("mnuAvgCostAdjust_Click entry")
    frmAvgCostAdjustment.ShowDialog()
    frmAvgCostAdjustment.Dispose()
    logger.Debug("mnuAvgCostAdjust_Click exit")
  End Sub

  Private Sub mnuAvgCostHistoryVariance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance.Click
    logger.Debug("mnuAvgCostHistoryVariance_Click entry")
    frmAvgCostHistoryReport.ShowDialog()
    frmAvgCostHistoryReport.Dispose()
    logger.Debug("mnuAvgCostHistoryVariance_Click exit")
  End Sub

  Private Sub mnuAvgCostAdjReasons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmentReasons.Click
    logger.Debug("mnuAvgCostAdjReasons_Click entry")
    frmAvgCostAdjReason.ShowDialog()
    frmAvgCostAdjReason.Dispose()
    logger.Debug("mnuAvgCostAdjReasons_Click exit")
  End Sub

  Private Sub mnuBatchReceiveClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Orders_BatchReceiveClose.Click
    logger.Debug("mnuBatchReceiveClose_Click( entry")
    frmBatchReceiveClose.ShowDialog()
    frmBatchReceiveClose.Dispose()
    logger.Debug("mnuBatchReceiveClose_Click(_Click exit")
  End Sub

  Private Sub ClosedOrdersMissingInvoiceDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReports_Order_ClosedOrdersMissingInvoiceData.Click
    ClosedOrdersMissingInvoiceDataReport.ShowDialog()
    ClosedOrdersMissingInvoiceDataReport.Dispose()

  End Sub

  Private Sub mnuIRMAProcessMonitor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFile_ProcessMonitor.Click
    logger.Debug("mnuIRMAProcessMonitor_Click entry")
    Cursor = Cursors.WaitCursor
    Form_ManageScheduledJobs.ShowDialog()
    Form_ManageScheduledJobs.Dispose()
    Cursor = Cursors.Default
    logger.Debug("mnuIRMAProcessMonitor_Click exit")
  End Sub

  Private Sub mnuViewStores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_POSInterface_StoreFTPConfiguration.Click
    logger.Debug("mnuViewStores_Click entry")
    Form_ManageStores.ShowDialog()
    Form_ManageStores.Dispose()
    logger.Debug("mnuViewStores_Click exit")
  End Sub

  Private Sub mnuCreateStore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_Stores_CreateStore.Click
    logger.Debug("mnuCreateStore_Click entry")
    StoreAdd.ShowDialog()
    StoreAdd.Dispose()
    logger.Debug("mnuCreateStore_Click exit")
  End Sub

  Private Sub mnuViewWriters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_POSInterface_FileWriter.Click
    logger.Debug("mnuViewWriters_Click entry")
    Form_ManagePOSWriter.ShowDialog()
    Form_ManagePOSWriter.Dispose()
    logger.Debug("mnuViewWriters_Click exit")
  End Sub

  Private Sub mnuAPUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_APUpload.Click
    logger.Debug("mnuAPUpload_Click entry")
    Form_PeopleSoftUploadJobController.ShowDialog()
    Form_PeopleSoftUploadJobController.Dispose()
    logger.Debug("mnuAPUpload_Click exit")
  End Sub

  Private Sub mnuAuditExceptionsReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_AuditExceptionsReport.Click
    logger.Debug("mnuAuditExceptionsReport_Click entry")
    Form_AuditExceptionsJobController.ShowDialog()
    Form_AuditExceptionsJobController.Dispose()
    logger.Debug("mnuAuditExceptionsReport_Click exit")
  End Sub

  Private Sub mnuAverageCostUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_AverageCostUpdate.Click
    logger.Debug("mnuAverageCostUpdate_Click entry")
    Form_AverageCostUpdateController.ShowDialog()
    Form_AverageCostUpdateController.Dispose()
    logger.Debug("mnuAverageCostUpdate_Click exit")
  End Sub

  Private Sub mnuBuildStorePOSFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_Stores_BuildPOSFile.Click
    logger.Debug("mnuBuildStorePOSFile_Click entry")
    Form_AuditReportJobController.ShowDialog()
    Form_AuditReportJobController.Dispose()
    logger.Debug("mnuBuildStorePOSFile_Click exit")
  End Sub

  Private Sub mnuBuildStoreScaleFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_Stores_BuildScaleFile.Click
    logger.Debug("mnuBuildStoreScaleFile_Click entry")
    Form_BuildFullScaleFileForStore.ShowDialog()
    Form_BuildFullScaleFileForStore.Dispose()
    logger.Debug("mnuBuildStoreScaleFile_Click exit")
  End Sub

  Private Sub mnuBuildStoreESTFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_Stores_BuildESTFile.Click
    logger.Debug("mnuBuildStoreESTFile_Click entry")
    Form_BuildFullESTFileForStore.ShowDialog()
    Form_BuildFullESTFileForStore.Dispose()
    logger.Debug("mnuBuildStoreESTFile_Click exit")
  End Sub
  Private Sub mnuSendStoreToMammoth_Click(sender As Object, e As EventArgs) Handles mnuAdministration_Stores_SendStoreToMammoth.Click
    logger.Debug("mnuSendStoreToMammoth_Click entry")
    frmSendStoreToMammoth.ShowDialog()
    frmSendStoreToMammoth.Dispose()
    logger.Debug("mnuSendStoreToMammoth_Click exit")
  End Sub
  Private Sub mnuEInvoicing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdit_Orders_EInvoicing.Click
    logger.Debug("mnuEInvoicingIntegration_Click entry")
    EInvoicing_ViewInvoices.ShowDialog()
    EInvoicing_ViewInvoices.Dispose()
    logger.Debug("mnuEInvoicingIntegration_Click exit")
  End Sub

  Private Sub mnuEInvoicingElementConfiguration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementConfiguration.Click
    Dim frm As EInvoicing_SAC_Edit = New EInvoicing_SAC_Edit
    frm.ShowDialog()
    frm.Dispose()
  End Sub

  Private Sub mnuPLUMHost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_PLUMHost.Click
    logger.Debug("mnuPLUMHost_Click entry")
    Form_PLUMHostJobController.ShowDialog()
    Form_PLUMHostJobController.Dispose()
    logger.Debug("mnuPLUMHost_Click exit")
  End Sub

  Private Sub mnuPOSPull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_POSPull.Click
    logger.Debug("mnuPOSPull_Click entry")
    Form_POSPullJobController.ShowDialog()
    Form_POSPullJobController.Dispose()
    logger.Debug("mnuPOSPull_Click exit")
  End Sub

  Private Sub mnuScalePOSPush_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuData_ScalePOSPush.Click
    Using form As Form = New Form_POSPushJobController()
      form.ShowDialog()
    End Using
  End Sub

  Private Sub mnuSendOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_SendOrders.Click
    logger.Debug("mnuSendOrders_Click entry")
    Form_SendOrders.ShowDialog()
    Form_SendOrders.Dispose()
    logger.Debug("mnuSendOrders_Click exit")
  End Sub

  Private Sub mnuTlogProcessing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_TlogProcessing.Click
    logger.Debug("mnuTlogProcessing_Click entry")
    Select Case My.Application.Region.ToUpper
      Case "EU"
        Dim frm As Form_IRISTlogProcessing = New Form_IRISTlogProcessing()
        frm.ShowDialog()
        frm.Dispose()
      Case "FL", "SP", "NA", "NE", "MA", "SW", "RM", "PN", "NC", "MW"
        Dim frm As Form_HouseTlogProcessing = New Form_HouseTlogProcessing()
        frm.ShowDialog()
        frm.Dispose()
      Case Else
        MessageBox.Show("Tlog Processing does not support your region at this time. (" & My.Application.Region.ToUpper & ")")
    End Select
    logger.Debug("mnuTlogProcessing_Click exit")
  End Sub

  Private Sub mnuWeeklySalesRollup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_WeeklySalesRollup.Click
    logger.Debug("mnuWeeklySalesRollup_Click entry")
    Form_WeeklySalesRollup.ShowDialog()
    Form_WeeklySalesRollup.Dispose()
    logger.Debug("mnuWeeklySalesRollup_Click exit")
  End Sub

  Private Sub mnuManageUsers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_Users_ManageUsers.Click
    logger.Debug("mnuManageUsers_Click entry")
    Form_ManageUsers.ShowDialog()
    Form_ManageUsers.Dispose()
    logger.Debug("mnuManageUsers_Click exit")
  End Sub

  Private Sub mnuManageTitles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_Users_ManageTitles.Click
    logger.Debug("mnuManageTitles_Click entry")
    Form_ManageTitles.ShowDialog()
    Form_ManageTitles.Dispose()
    logger.Debug("mnuManageTitles_Click exit")
  End Sub

  Private Sub mnuManageConfigurationSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_SystemConfiguration_ManageConfiguration.Click
    logger.Debug("mnuManageConfigurationSettings_Click entry")
    Dim frm As Form_ConfigurationData = New Form_ConfigurationData
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuManageConfigurationSettings_Click exit")
  End Sub

  Private Sub mnuBuildConfigurationSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_SystemConfiguration_BuildConfiguration.Click
    logger.Debug("mnuBuildConfigurationSettings_Click entry")
    Dim frm As Form_ConfigurationData_Build = New Form_ConfigurationData_Build
    frm.ShowDialog()
    frm.Dispose()
    logger.Debug("mnuBuildConfigurationSettings_Click exit")
  End Sub

  Private Sub mnuDataArchiving_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_SystemConfiguration_DataArchiving.Click
    logger.Debug("mnuDataArchiving_Click entry")
    Dim frm As Form_ManageDataArchiving = New Form_ManageDataArchiving
    frm.ShowDialog()
    frm.Close()
    frm.Dispose()
    logger.Debug("mnuDataArchiving_Click exit")
  End Sub

  Private Sub mnuInstanceDataFlags_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_SystemConfiguration_InstanceDataFlags.Click
    logger.Debug("mnuInstanceDataFlags_Click entry")
    Form_InstanceDataFlags.ShowDialog()
    Form_InstanceDataFlags.Dispose()
    logger.Debug("mnuInstanceDataFlags_Click exit")
  End Sub

  Private Sub mnuInstanceData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_SystemConfiguration_InstanceData.Click
    logger.Debug("mnuInstanceData_Click entry")
    Form_RegionalInstanceData.ShowDialog()
    Form_RegionalInstanceData.Dispose()
    logger.Debug("mnuInstanceData_Click exit")
  End Sub

  Private Sub mnuItemAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes.Click
    logger.Debug("mnuItemAttributes_Click entry")
    Form_ManageItemAttributes.ShowDialog()
    Form_ManageItemAttributes.Dispose()
    logger.Debug("mnuItemAttributes_Click exit")
  End Sub

  Private Sub mnuPricingMethods_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_POSInterface_PricingMethods.Click
    logger.Debug("mnuPricingMethods_Click entry")
    Form_ManagePricingMethods.ShowDialog()
    Form_ManagePricingMethods.Dispose()
    logger.Debug("mnuPricingMethods_Click exit")
  End Sub

  Private Sub mnuMenuAccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_SystemConfiguration_MenuAccess.Click
    logger.Debug("mnuMenuAccess_Click entry")
    Form_ManageMenuAccess.ShowDialog()
    Form_ManageMenuAccess.Dispose()
    logger.Debug("mnuMenuAccess_Click exit")
  End Sub

  Private Sub mnuTaxJurisdictions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions.Click
    logger.Debug("mnuTaxJurisdictions_Click entry")
    TaxJurisdictionAdmin.ShowDialog()
    TaxJurisdictionAdmin.Dispose()
    logger.Debug("mnuTaxJurisdictions_Click exit")
  End Sub

  Private Sub mnuZones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones.Click
    logger.Debug("mnuZones_Click entry")
    ZoneAdmin.ShowDialog()
    ZoneAdmin.Dispose()
    logger.Debug("mnuZones_Click exit")
  End Sub

  Private Sub mnuCurrency_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency.Click
    logger.Debug("mnuCurrency_Click entry")
    CurrencyAdmin.ShowDialog()
    CurrencyAdmin.Dispose()
    logger.Debug("mnuCurrency_Click exit")
  End Sub

  Private Sub mnuChangeBatchState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuData_BatchRollback.Click
    logger.Debug("mnuChangeBatchState_Click entry")
    Form_ChangeBatchState.ShowDialog()
    Form_ChangeBatchState.Dispose()
    logger.Debug("mnuChangeBatchState_Click exit")
  End Sub

  Private Sub mnuRestoreDeletedItems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuData_RestoreDeletedItem.Click
    logger.Debug("mnuRestoreDeletedItems_Click entry")
    Form_ItemRestore.ShowDialog()
    Form_ItemRestore.Dispose()
    logger.Debug("mnuRestoreDeletedItems_Click exit")
  End Sub

  Private Sub mnuShrinkCorrections_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuInventory_ShrinkCorrections.Click
    logger.Debug("mnuShrinkCorrections_Click entry")
    frmShrinkCorrections.ShowDialog()
    frmShrinkCorrections.Dispose()
    logger.Debug("mnuShrinkCorrections_Click exit")
  End Sub

  Private Sub tsbEdit_Item_InventoryLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEdit_Item_InventoryLevel.Click
    logger.Debug("tsbInventory_InventoryAdjustment_Click entry")
    ItemOnHandPrompt.ShowDialog()
    ItemOnHandPrompt.Dispose()
    logger.Debug("tsbInventory_InventoryAdjustment_Click exit")
  End Sub

  Private Sub tsbReports_ReportManager_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbReports_ReportManager.Click
    logger.Debug("mnuReportManager_Click entry")
    Dim reportmanager As String
    reportmanager = ConfigurationServices.AppSettings("reportManagerURL")
    Call GoToURL(reportmanager)
    logger.Debug("mnuReportManager_Click exit")
  End Sub

  Private Sub ViewAppLogsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_ScheduledJobs_ViewAppLogs.Click
    Dim form As Form_ViewLogs
    form = New Form_ViewLogs
    form.ShowDialog()
    form.Dispose()
  End Sub

  Private Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits.Click
    Dim frm As Form_ManageItemUnit
    frm = New Form_ManageItemUnit
    frm.ShowDialog()
    frm.Dispose()
  End Sub

  Private Function IsTopLevelEnabled(ByVal mnu As ToolStripMenuItem) As Boolean
    Dim mnuSub As ToolStripMenuItem = Nothing
    Dim blnIsTopLevelEnabled As Boolean = False

    For Each mnuSub In mnu.DropDownItems
      blnIsTopLevelEnabled = blnIsTopLevelEnabled Or mnuSub.Enabled
    Next

    Return blnIsTopLevelEnabled
  End Function

  Private Sub UnprocessedPushFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuData_UnprocessedPushFiles.Click
    logger.Debug("mnuScalePOSPush_Click entry")
    Form_UnprocessedPushFiles.ShowDialog()
    Form_UnprocessedPushFiles.Dispose()
    logger.Debug("mnuScalePOSPush_Click exit")
  End Sub

  Private Sub mnuAdministration_SystemConfiguration_ReasonCodeMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_SystemConfiguration_ReasonCodeMaintenance.Click
    logger.Debug("mnuAdministration_SystemConfiguration_ReasonCodeMaintenance_Click entry")
    ReasonCodeMaintenance.ShowDialog()
    ReasonCodeMaintenance.Dispose()
    logger.Debug("mnuAdministration_SystemConfiguration_ReasonCodeMaintenance_Click exit")
  End Sub

  Private Sub mnuAdministration_SystemConfiguration_ResolutionCodes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_SystemConfiguration_ResolutionCodes.Click
    logger.Debug("mnuAdministration_SystemConfiguration_ResolutionCodes_Click")
    SuspededPOResolutionCode.ShowDialog()
    SuspededPOResolutionCode.Dispose()
    logger.Debug("mnuAdministration_SystemConfiguration_ResolutionCodes_exit")
  End Sub

  Private Sub tsbEdit_Search_PO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEdit_Search_PO.Click
    logger.Debug("tsbEdit_Search_PO_Click")

    If tsbEdit_SearchText.Text = String.Empty Then
      MsgBox("Please enter a PO number.", MsgBoxStyle.Information, Me.Text)
      Exit Sub
    End If

    If Not IsNumeric(tsbEdit_SearchText.Text) Then
      MsgBox("Please enter a numeric value for the PO number.", MsgBoxStyle.Information, Me.Text)
      Exit Sub
    End If

    Try
      Dim poNumber As Integer = Integer.Parse(Trim(tsbEdit_SearchText.Text))

      Dim dataTable As New DataTable
      dataTable = OrderingDAO.GetOrderSearch(poNumber)

      If dataTable.Rows.Count = 0 Then
        MsgBox(String.Format("No orders were found with PO# {0}.  Please try another PO number.", poNumber), MsgBoxStyle.Information, Me.Text)
        Exit Sub
      ElseIf dataTable.Rows.Count = 1 Then
        frmOrders.AutoSearch_OrderHeaderId_OneKnownMatch = dataTable.Rows(0).Item("OrderHeader_ID")
        frmOrders.ShowDialog()
        frmOrders.Dispose()
      ElseIf dataTable.Rows.Count > 1 Then
        frmOrders.AutoSearch_OrderHeaderId = poNumber
        frmOrders.ShowDialog()
        frmOrders.Dispose()
      End If

    Catch ex As OverflowException
      MsgBox("The PO number is too large.  Please try a smaller number.", MsgBoxStyle.Information, Me.Text)
    End Try

    logger.Debug("tsbEdit_Search_PO_exit")
  End Sub

  'TFS 2371 - Faisal Ahmed, 08/20/2011
  Private Sub tsbEdit_Search_Identifier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEdit_Search_Identifier.Click
    logger.Debug("tsbEdit_Search_Identifier_Click")

    tsbEdit_SearchText.Text = tsbEdit_SearchText.Text.Replace(" ", String.Empty)

    If Not IsNumeric(tsbEdit_SearchText.Text) Then
      MsgBox("Identifier is missing or is not numeric." + VB.vbCrLf + "Please verify your input and try again.", MsgBoxStyle.Information)
      tsbEdit_SearchText.SelectAll()
      tsbEdit_SearchText.Select()
      Exit Sub
    End If

    gbQuickSearch = True
    glIdentifier = tsbEdit_SearchText.Text

    frmItemSearch.ShowDialog()
    frmItemSearch.Dispose()

    gbQuickSearch = False
    glIdentifier = String.Empty

    '-- if its not zero, then something was found
    If glItemID <> 0 Then
      frmItem.ShowDialog()
      frmItem.Dispose()
    End If

    logger.Debug("tsbEdit_Search_Identifier_exit")
  End Sub

  Private Sub StaticStoreFTPToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles StaticStoreFTPToolStripMenuItem.Click
    logger.Debug("StaticStoreFTPToolStripMenuItem_Click entry")
    Form_ManageStaticStoreFtpInfo.ShowDialog(Me)
    Form_ManageStaticStoreFtpInfo.Close()
    Form_ManageStaticStoreFtpInfo.Dispose()
    logger.Debug("StaticStoreFTPToolStripMenuItem_Click exit")
  End Sub

  Private Sub SubteamDiscountExceptionsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SubteamDiscountExceptionsToolStripMenuItem.Click
    logger.Debug("SubteamDiscountExceptionsToolStripMenuItem_Click entry")
    Form_StoreSubTeamDiscountException.ShowDialog(Me)
    Form_StoreSubTeamDiscountException.Close()
    Form_StoreSubTeamDiscountException.Dispose()
    logger.Debug("SubteamDiscountExceptionsToolStripMenuItem_Click exit")
  End Sub

  Private Sub PriceTypesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PriceTypesToolStripMenuItem.Click
    logger.Debug("PriceTypesToolStripMenuItem_Click entry")
    Form_ManagePriceChgType.ShowDialog(Me)
    Form_ManagePriceChgType.Close()
    Form_ManagePriceChgType.Dispose()
    logger.Debug("PriceTypesToolStripMenuItem_Click exit")
  End Sub

  Private Sub LabelTypesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LabelTypesToolStripMenuItem.Click
    logger.Debug("LabelTypesToolStripMenuItem_Click entry")
    Form_ManageLabelType.ShowDialog(Me)
    Form_ManageLabelType.Close()
    Form_ManageLabelType.Dispose()
    logger.Debug("LabelTypesToolStripMenuItem_Click exit")
  End Sub

  Private Sub mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement.Click
    logger.Debug("mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement_Click entry")
    ManageDefaultAttributes.ShowDialog(Me)
    ManageDefaultAttributes.Close()
    ManageDefaultAttributes.Dispose()
    logger.Debug("mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement_Click exit")
  End Sub

  Private Sub mnuReports_Tax_NewItemTaxClass_Click(sender As System.Object, e As System.EventArgs) Handles mnuReports_Tax_NewItemTaxClass.Click
    logger.Debug("mnuReports_Tax_NewItemTaxClass_Click entry")
    Dim sReportURL As New System.Text.StringBuilder
    sReportURL.Append("Item+Tax+Class+Report")
    sReportURL.Append("&rs:Command=Render")
    sReportURL.Append("&rc:Parameters=True")
    sReportURL.Append("&Type=0") '0 = New Items, 1 = Modified Items, as specified in Item Tax Class.rdl
    Call ReportingServicesReport(sReportURL.ToString)
    logger.Debug("mnuReports_Tax_NewItemTaxClass_Click exit")
  End Sub

  Private Sub mnuReports_Tax_ModifiedItemTaxClass_Click(sender As System.Object, e As System.EventArgs) Handles mnuReports_Tax_ModifiedItemTaxClass.Click
    logger.Debug("mnuReports_Tax_ModifiedItemTaxClass_Click entry")
    Dim sReportURL As New System.Text.StringBuilder
    sReportURL.Append("Item+Tax+Class+Report")
    sReportURL.Append("&rs:Command=Render")
    sReportURL.Append("&rc:Parameters=True")
    sReportURL.Append("&Type=1") '0 = New Items, 1 = Modified Items, as specified in Item Tax Class.rdl
    Call ReportingServicesReport(sReportURL.ToString)
    logger.Debug("mnuReports_Tax_ModifiedItemTaxClass_Click exit")
  End Sub

  Private Sub PartialShippmentToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PartialShippmentToolStripMenuItem.Click
    logger.Debug("mnuReports_PartialShipment_Click entry")
    Dim sReportURL As New System.Text.StringBuilder
    sReportURL.Append("Partial+Shipment+Report")
    sReportURL.Append("&rs:Command=Render")
    sReportURL.Append("&rc:Parameters=True")
    Call ReportingServicesReport(sReportURL.ToString)
    logger.Debug("mnuReports_PartialShipment_Click exit")
  End Sub

  Private Sub mnuTools_UnitConverter_Click(sender As System.Object, e As System.EventArgs) Handles mnuTools_UnitConverter.Click
    logger.Debug("mnuTools_UnitConverter_Click Entry")
    FrmConvertMeasures.Show(Me)
    logger.Debug("mnuTools_UnitConverter_Click Exit")
  End Sub

  Private Sub mnuEdit_Orders_Search_Click(sender As System.Object, e As System.EventArgs) Handles mnuEdit_Orders_Search.Click
    logger.Debug("mnuEdit_Orders_Search Entry")

    '-- Set it to no order selected
    glOrderHeaderID = 0

    frmOrdersSearch.ShowDialog()
    frmOrdersSearch.Close()
    frmOrdersSearch.Dispose()

    If glOrderHeaderID <> 0 Then
      frmOrders.AutoSearch_OrderHeaderId_OneKnownMatch = glOrderHeaderID
      frmOrders.ShowDialog()
      frmOrders.Dispose()
    End If

    logger.Debug("mnuEdit_Orders_Search Exit")
  End Sub

  Private Sub tsbEdit_OrderSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbEdit_OrderSearch.Click
    logger.Debug("tsbEditOrder_Search_Click entry")

    '-- Set it to no order selected
    glOrderHeaderID = 0

    frmOrdersSearch.ShowDialog()
    frmOrdersSearch.Close()
    frmOrdersSearch.Dispose()

    If glOrderHeaderID <> 0 Then
      frmOrders.AutoSearch_OrderHeaderId_OneKnownMatch = glOrderHeaderID
      frmOrders.ShowDialog()
      frmOrders.Dispose()
    End If

    logger.Debug("tsbEditOrder_Search_Click exit")
  End Sub

  Private Sub tsbOrchard_Click(sender As Object, e As EventArgs) Handles tsbOrchard.Click
    logger.Debug("mnuOrchard_Click entry")
    Dim orchard As String
    orchard = ConfigurationServices.AppSettings("OrchardURL")
    Call GoToURL(orchard)
    logger.Debug("mnuOrchard_Click exit")
  End Sub

  Private Sub IconItemRefresh_Click(sender As Object, e As EventArgs) Handles IconItemRefreshMenuItem.Click
    logger.Debug("IconItemRefresh_Click entry")
    Dim iconItemRefreshWindow As New IconItemRefresh()
    iconItemRefreshWindow.ShowDialog()
    logger.Debug("IconItemRefresh_Click exit")
  End Sub

  Private Sub R10ItemRefreshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles R10ItemRefreshToolStripMenuItem.Click
    logger.Debug("R10ItemRefresh_Click entry")
    Dim r10ItemRefreshWindow As New PosItemRefresh()
    r10ItemRefreshWindow.ShowDialog()
    logger.Debug("R10ItemRefresh_Click exit")
  End Sub

  Private Sub SlawRefreshMenuItem_Click(sender As Object, e As EventArgs) Handles SlawItemLocaleMenuItem.Click, SlawPriceMenuItem.Click
    Using form As New SlawItemRefresh(If(sender Is SlawItemLocaleMenuItem, SlawItemRefresh.ItemRefreshType.ItemLocale, SlawItemRefresh.ItemRefreshType.Price))
      form.ShowDialog()
    End Using
  End Sub

  Private Sub NoTagLogicToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NoTagLogicToolStripMenuItem.Click
    logger.Debug("NoTagLogicToolStripMenuItem_Click entry")

    Cursor = Cursors.WaitCursor

    Using noTagConfigForm As New NoTagLogicConfiguration(New NoTagDataAccess())
      noTagConfigForm.ShowDialog()
    End Using

    Cursor = Cursors.Default

    logger.Debug("NoTagLogicToolStripMenuItem_Click exit")
  End Sub

  Private Sub ViewClientLogFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewClientLogFileToolStripMenuItem.Click
    logger.Debug("ViewClientLogFileToolStripMenuItem_Click entry")

    Dim filePath As String = Nothing

    Dim section As XmlElement = ConfigurationManager.GetSection("log4net")

    For Each childNode As Object In section
      If childNode.Name = "appender" Then
        If childNode.Attributes IsNot Nothing Then
          For Each attribute As XmlAttribute In childNode.Attributes
            If attribute.Value.Contains("RollingFileAppender") Then
              Dim value As String = attribute.Value
              filePath = childNode.ChildNodes.Item(0).Attributes(0).Value
            End If
          Next
        End If
      End If
    Next

    ' Proudction file path: "./logs/logfile.txt"

    If Not String.IsNullOrEmpty(filePath) Then
      If filePath.StartsWith("./") Then
        Dim appStartPath As String = Application.StartupPath
        filePath = filePath.Replace("/", "\")
        filePath = appStartPath + filePath.Substring(1)
      End If

      If File.Exists(filePath) Then
        logger.Info(String.Format("Opening log file at path: {0}", filePath))
        Process.Start(filePath)
      Else
        logger.Info(String.Format("No log file found at path: {0}", filePath))
      End If
    End If

    logger.Debug("ViewClientLogFileToolStripMenuItem_Click exit")
  End Sub

  Private Sub ManageRetentionPoliciesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles mnuAdministration_SystemConfiguration_ManageRetentionPolicies.Click
    Using form As frmRetentionPolicyList = New frmRetentionPolicyList()
      form.ShowDialog()
    End Using
  End Sub

  Private Sub mnuData_CancelAllSales_Click(sender As Object, e As EventArgs) Handles mnuData_CancelAllSales.Click
    logger.Debug("CancelSalesMultipleItemsToolStripMenuItem_Click entry")
    CancelSalesMultipleItems.ShowDialog()
    logger.Debug("CancelSalesMultipleItemsToolStripMenuItem_Click exit")
  End Sub

  Private Sub SupportRestoreDeleteItemToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupportRestoreDeleteItemToolStripMenuItem.Click
    logger.Debug("SupportRestoreDeleteItemToolStripMenuItem_Click entry")
    Dim window As New SupportRestoreDeletedItems()
    window.ShowDialog()
    logger.Debug("SupportRestoreDeleteItemToolStripMenuItem_Click exit")
  End Sub

  Private Sub tsbEdit_SearchText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tsbEdit_SearchText.KeyPress
    If Asc(e.KeyChar) <> 8 Then
      e.Handled = (Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57)
    End If
  End Sub

  Private Sub tsbEdit_SearchText_KeyDown(sender As Object, e As KeyEventArgs) Handles tsbEdit_SearchText.KeyDown
    If (e.Control) Then
      Dim control As ToolStripTextBox = DirectCast(sender, ToolStripTextBox)

      Select Case e.KeyCode
        Case Keys.A : control.SelectAll()
        Case Keys.C : If control.SelectedText.Length > 0 Then Clipboard.SetText(control.SelectedText)
        Case Keys.V
          Dim txt As String = Clipboard.GetText()

          If Not String.IsNullOrEmpty(txt) Then
            Dim index = control.SelectionStart
            control.Text = String.Format("{0}{1}{2}", control.Text.Substring(0, index), txt, control.Text.Substring(index + control.SelectionLength))
            control.SelectionStart = index + txt.Length
          End If
        Case Keys.X
          If (control.SelectionLength > 0) Then
            Dim index = control.SelectionStart
            Clipboard.SetText(control.SelectedText)

            control.Text = String.Format("{0}{1}", control.Text.Substring(0, index), control.Text.Substring(index + control.SelectionLength))
            control.SelectionStart = index
          End If
        Case Keys.I : If control.Text.Length > 0 Then tsbEdit_Search_Identifier_Click(Nothing, Nothing)
        Case Keys.P : If control.Text.Length > 0 Then tsbEdit_Search_PO_Click(Nothing, Nothing)
      End Select
    End If
  End Sub
End Class