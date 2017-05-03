<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmMain
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
    Public WithEvents mnuFile_Export As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFile_Import_ImportOrder As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFile_Import_RIPEOrder As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFile_Import As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFile_Exit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Item As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Orders_AddEdit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Orders_Allocate As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Orders_ItemQueue As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Orders As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Pricing_Batches As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Pricing_LineDrive As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Pricing_ReprintSigns As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Pricing As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Vendors_AddEdit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Vendors_CostImportExceptions As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit_Vendors As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuEdit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_GLSummary_GLDistributionCheck As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_GLSummary_GLSales As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_GLSummary_GLTransfers As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_GLSummary As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_GLUploads As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_InvoiceManifest As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_OutOfPeriodInvoiceReport As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_PJReconcile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_PurchaseToSalesComp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting_AdjustmentSummary As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Accounting As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Buyer_CostException As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Buyer_InventoryBalance As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Buyer_Margin As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Buyer_WarehouseMovement As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Buyer_ZeroAverageCostItems As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Buyer_ZeroPriceItems As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Buyer As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_ActiveItemList As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_DiscontinueItemsWithInventory As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_InventoryGuide As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_ItemList As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_ItemOnHandComparisonBetweenLocation As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_ItemOrderHistory As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_NotAvailableItems As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_OrderGuide As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_ReorderSummaryByVendor As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_ShipperItems As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_UPCLabels As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item_VendorItems As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Item As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Inventory_CycleCountReports As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Inventory_LocationsReport As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Inventory_LocationItemsReport As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Inventory_LocationItemsManulCountSheet As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Inventory As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Order_APUpload As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Order_ClosedOrders As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Order_CreditReason As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Order_DeletedOrders As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Order_OpenPurchaseOrders As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Order_OrdersReceivedNotClosed As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Order_OutOfStock As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Order_POCostChanges As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Order As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Price_Batches As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Price_ItemPrice As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Price_PriceChanges As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Price_Specials As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Price As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_ReceivingDistribution_AverageCost_DistributionMargin As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_ReceivingDistribution_AverageCost_List As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuInventory_InventoryCosting_AverageCostUpdate As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuInventory_InventoryCosting_AverageCostAdjustment As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_ReceivingDistribution_AverageCost As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_ReceivingDistribution_DailyReceiving As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_ReceivingDistribution_OrderFulfillment As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_ReceivingDistribution_ReceivingLog As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_ReceivingDistribution As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuInventory_Adjustments As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuInventory_CycleCounts As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuInventory_Locations As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuInventory As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTools_TGM_NewTGMView As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTools_TGM_LoadTGMView As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTools_TGM As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTools_VendorList As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelp_About As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents mnuInventory_InventoryCosting As System.Windows.Forms.ToolStripMenuItem
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.pnlMainMessage = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ToolStrip_MenuItems = New System.Windows.Forms.ToolStrip()
        Me.tsbFile_Export = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsbFile_Export_Planogram = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbFile_Export_RGIS = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbFile_Import = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsbFile_Import_EIM = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbFile_Import_Order = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbFile_Import_ItemMaintenanceBulkLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbFile_Import_ItemStoreMaintenanceBulkLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbFile_Import_ItemVendorMaintenanceBulkLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbFile_Import_Planogram = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbFile_Import_RipeOrder = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbEdit_Item_AddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit_Item_EditExistingItem = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit_Orders_AddEdit = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit_OrderSearch = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit_Item_InventoryLevel = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit_Pricing_Batches = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit_Pricing_ReprintSigns = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit_Pricing_PromotionalOffers = New System.Windows.Forms.ToolStripButton()
        Me.tsbReports_ReportManager = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit_SearchText = New System.Windows.Forms.ToolStripTextBox()
        Me.tsbEdit_Search = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsbEdit_Search_PO = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbEdit_Search_Identifier = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbOrchard = New System.Windows.Forms.ToolStripButton()
        Me.ToolStrip_Inventory = New System.Windows.Forms.ToolStrip()
        Me.tsbInventory_InventoryAdjustment = New System.Windows.Forms.ToolStripButton()
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Export = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Export_Planogram = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Export_RGIS = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Import = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Import_ExtendedItemMaintenance = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Import_ImportOrder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Import_ItemMaintenanceBulkLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Import_ItemStoreMaintenanceBulkLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Import_ItemVendorMaintenanceBulkLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Import_Planogram = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Import_RIPEOrder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_ProcessMonitor = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFile_Exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_CompetitorStore = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_CompetitorStore_CompetitorPrices = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_CompetitorStore_CompetitorTrend = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_CompetitorStore_ImportCompetitorPrices = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_CompetitorStore_CompetitorMarginImpact = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_CompetitorStore_NationalPurchasingValues = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Item = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Item_Add = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Item_BulkLoadAuditHistory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Item_EditItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Item_ItemChain = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Orders = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Orders_AddEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Orders_Search = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Orders_Allocate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Orders_BatchReceiveClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Orders_EInvoicing = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Orders_InvoiceEntry = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Orders_SuspendedPOTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Orders_ItemQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Pricing = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Pricing_Batches = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Pricing_LineDrive = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Pricing_PriceChangeWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Pricing_PromotionalOffers = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Pricing_ReprintSigns = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_TaxHosting = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_TaxHosting_TaxClassification = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_TaxHosting_TaxFlag = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_TaxHosting_TaxJurisdiction = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Vendors = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Vendors_AddEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit_Vendors_CostImportExceptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_AdjustmentSummary = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_GLSummary = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_GLSummary_GLDistributionCheck = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_GLSummary_GLSales = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_GLSummary_GLTransfers = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_GLUploads = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_InvoiceDiscrepancies = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_InvoiceManifest = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_OutOfPeriodInvoiceReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_PJReconcile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_PurchaseAccrualReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Accounting_PurchaseToSalesComp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Audit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Audit_AVCIExceptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Buyer = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Buyer_CostException = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Buyer_InventoryBalance = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Buyer_Margin = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Buyer_StoreOrdersTotalBySKU = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Buyer_ZeroAverageCostItems = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Buyer_ZeroPriceItems = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Buyer_WarehouseMovement = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_ActiveItemList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_DiscontinueItemsWithInventory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_InventoryGuide = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_ItemList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_ItemOnHandComparisonBetweenLocation = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_ItemOrderHistory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_NotAvailableItems = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_OrderGuide = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_ReorderSummaryByVendor = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_ShipperItems = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_UPCLabels = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Item_VendorItems = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Inventory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Inventory_BOHCompare = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Inventory_CycleCountReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Inventory_InventoryValue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Inventory_InventoryWeeklyHistory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Inventory_LocationItemsManulCountSheet = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Inventory_LocationItemsReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Inventory_LocationsReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Miscellaneous = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Miscellaneous_CasesBySubTeam = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Miscellaneous_CasesBySubTeamAudit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Miscellaneous_LotNo = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Miscellaneous_VendorEfficiency = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_APUpload = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_APUPAccrualsClosed = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_ClosedOrders = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_ClosedOrdersMissingInvoiceData = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_CreditReason = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_DeletedOrders = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_KitchenCaseTransfer = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_OpenPurchaseOrders = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_OrdersReceivedNotClosed = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_OutOfStock = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_POCostChanges = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Order_Short = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Price = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Price_Batches = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Price_ItemPrice = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Price_PriceChanges = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Price_Specials = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_AverageCost = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_AverageCost_DistributionMargin = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_AverageCost_List = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_COOL = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_COOL_ReceivingReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_COOL_ShippingReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_DailyReceiving = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_DCStoreRetailPriceReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_OrderFulfillment = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReceivingDistribution_ReceivingLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.PartialShippmentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Purchases = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_ReportManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Tax = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Tax_NewItemTaxClass = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_Tax_ModifiedItemTaxClass = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_3WayMatching = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_3WayMatching_ControlGroupLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports_3WayMatching_ControlGroup3WayMatch = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInventory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInventory_Adjustments = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInventory_CycleCounts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInventory_InventoryCosting = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInventory_InventoryCosting_AverageCostAdjustment = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInventory_InventoryCosting_AverageCostUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInventory_Locations = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInventory_ShrinkCorrections = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_TGM = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_TGM_LoadTGMView = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_TGM_NewTGMView = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_VendorList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_UnitConverter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuData = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuData_BatchRollback = New System.Windows.Forms.ToolStripMenuItem()
        Me.IconItemRefreshMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.R10ItemRefreshToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuData_RestoreDeletedItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuData_ScalePOSPush = New System.Windows.Forms.ToolStripMenuItem()
        Me.SlawItemRefreshMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SlawItemLocaleMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SlawPriceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuData_UnprocessedPushFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmentReasons = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingTolerance = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits = New System.Windows.Forms.ToolStripMenuItem()
        Me.LabelTypesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoTagLogicToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin = New System.Windows.Forms.ToolStripMenuItem()
        Me.PriceTypesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_EatBy = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_ExtraText = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Grade = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelFormat = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelStyle = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelType = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_NutriFact = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_RandomWeightType = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tare = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelationships = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance = New System.Windows.Forms.ToolStripMenuItem()
        Me.SubteamDiscountExceptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_IRMAConfiguration_SystemConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_SystemConfiguration_BuildConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_SystemConfiguration_DataArchiving = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_SystemConfiguration_InstanceData = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_SystemConfiguration_InstanceDataFlags = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_SystemConfiguration_ManageConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_SystemConfiguration_ResolutionCodes = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_SystemConfiguration_ReasonCodeMaintenance = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_SystemConfiguration_MenuAccess = New System.Windows.Forms.ToolStripMenuItem()
        Me.StaticStoreFTPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_SystemConfiguration_ManageRetentionPolicies = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_POSInterface = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_POSInterface_FileWriter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_POSInterface_PricingMethods = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_POSInterface_StoreFTPConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_APUpload = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_AuditExceptionsReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_AverageCostUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_CloseReceiving = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_PLUMHost = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_POSPull = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_SendOrders = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_TlogProcessing = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_ViewAppLogs = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_ScheduledJobs_WeeklySalesRollup = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_Stores = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_Stores_CreateStore = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_Stores_BuildPOSFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_Stores_BuildScaleFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_Stores_BuildESTFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_Stores_SendStoreToMammoth = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_Users = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_Users_ManageTitles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdministration_Users_ManageUsers = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp_About = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewClientLogFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel_Region = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_Environment = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_Version = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelRegionalSetting = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.pnlMainMessage.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip_MenuItems.SuspendLayout()
        Me.ToolStrip_Inventory.SuspendLayout()
        Me.MainMenu1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripContainer1
        '
        Me.ToolStripContainer1.BottomToolStripPanelVisible = False
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.pnlMainMessage)
        resources.ApplyResources(Me.ToolStripContainer1.ContentPanel, "ToolStripContainer1.ContentPanel")
        resources.ApplyResources(Me.ToolStripContainer1, "ToolStripContainer1")
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.RightToolStripPanelVisible = False
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ToolStrip_MenuItems)
        '
        'pnlMainMessage
        '
        Me.pnlMainMessage.BackColor = System.Drawing.Color.ForestGreen
        Me.pnlMainMessage.Controls.Add(Me.PictureBox1)
        resources.ApplyResources(Me.pnlMainMessage, "pnlMainMessage")
        Me.pnlMainMessage.Name = "pnlMainMessage"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.My.Resources.Resources.WFM_Splash
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'ToolStrip_MenuItems
        '
        resources.ApplyResources(Me.ToolStrip_MenuItems, "ToolStrip_MenuItems")
        Me.ToolStrip_MenuItems.BackColor = System.Drawing.Color.Transparent
        Me.ToolStrip_MenuItems.CanOverflow = False
        Me.ToolStrip_MenuItems.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip_MenuItems.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbFile_Export, Me.tsbFile_Import, Me.tsbEdit_Item_AddNewItem, Me.tsbEdit_Item_EditExistingItem, Me.tsbEdit_Orders_AddEdit, Me.tsbEdit_OrderSearch, Me.tsbEdit_Item_InventoryLevel, Me.tsbEdit_Pricing_Batches, Me.tsbEdit_Pricing_ReprintSigns, Me.tsbEdit_Pricing_PromotionalOffers, Me.tsbReports_ReportManager, Me.tsbEdit_SearchText, Me.tsbEdit_Search, Me.tsbOrchard})
        Me.ToolStrip_MenuItems.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.ToolStrip_MenuItems.Name = "ToolStrip_MenuItems"
        Me.ToolStrip_MenuItems.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        '
        'tsbFile_Export
        '
        Me.tsbFile_Export.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFile_Export.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbFile_Export_Planogram, Me.tsbFile_Export_RGIS})
        resources.ApplyResources(Me.tsbFile_Export, "tsbFile_Export")
        Me.tsbFile_Export.Name = "tsbFile_Export"
        '
        'tsbFile_Export_Planogram
        '
        Me.tsbFile_Export_Planogram.Name = "tsbFile_Export_Planogram"
        resources.ApplyResources(Me.tsbFile_Export_Planogram, "tsbFile_Export_Planogram")
        '
        'tsbFile_Export_RGIS
        '
        Me.tsbFile_Export_RGIS.Name = "tsbFile_Export_RGIS"
        resources.ApplyResources(Me.tsbFile_Export_RGIS, "tsbFile_Export_RGIS")
        '
        'tsbFile_Import
        '
        Me.tsbFile_Import.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFile_Import.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbFile_Import_EIM, Me.tsbFile_Import_Order, Me.tsbFile_Import_ItemMaintenanceBulkLoad, Me.tsbFile_Import_ItemStoreMaintenanceBulkLoad, Me.tsbFile_Import_ItemVendorMaintenanceBulkLoad, Me.tsbFile_Import_Planogram, Me.tsbFile_Import_RipeOrder})
        resources.ApplyResources(Me.tsbFile_Import, "tsbFile_Import")
        Me.tsbFile_Import.Name = "tsbFile_Import"
        '
        'tsbFile_Import_EIM
        '
        Me.tsbFile_Import_EIM.Name = "tsbFile_Import_EIM"
        resources.ApplyResources(Me.tsbFile_Import_EIM, "tsbFile_Import_EIM")
        '
        'tsbFile_Import_Order
        '
        Me.tsbFile_Import_Order.Name = "tsbFile_Import_Order"
        resources.ApplyResources(Me.tsbFile_Import_Order, "tsbFile_Import_Order")
        '
        'tsbFile_Import_ItemMaintenanceBulkLoad
        '
        Me.tsbFile_Import_ItemMaintenanceBulkLoad.Name = "tsbFile_Import_ItemMaintenanceBulkLoad"
        resources.ApplyResources(Me.tsbFile_Import_ItemMaintenanceBulkLoad, "tsbFile_Import_ItemMaintenanceBulkLoad")
        '
        'tsbFile_Import_ItemStoreMaintenanceBulkLoad
        '
        Me.tsbFile_Import_ItemStoreMaintenanceBulkLoad.Name = "tsbFile_Import_ItemStoreMaintenanceBulkLoad"
        resources.ApplyResources(Me.tsbFile_Import_ItemStoreMaintenanceBulkLoad, "tsbFile_Import_ItemStoreMaintenanceBulkLoad")
        '
        'tsbFile_Import_ItemVendorMaintenanceBulkLoad
        '
        Me.tsbFile_Import_ItemVendorMaintenanceBulkLoad.Name = "tsbFile_Import_ItemVendorMaintenanceBulkLoad"
        resources.ApplyResources(Me.tsbFile_Import_ItemVendorMaintenanceBulkLoad, "tsbFile_Import_ItemVendorMaintenanceBulkLoad")
        '
        'tsbFile_Import_Planogram
        '
        Me.tsbFile_Import_Planogram.Name = "tsbFile_Import_Planogram"
        resources.ApplyResources(Me.tsbFile_Import_Planogram, "tsbFile_Import_Planogram")
        '
        'tsbFile_Import_RipeOrder
        '
        Me.tsbFile_Import_RipeOrder.Name = "tsbFile_Import_RipeOrder"
        resources.ApplyResources(Me.tsbFile_Import_RipeOrder, "tsbFile_Import_RipeOrder")
        '
        'tsbEdit_Item_AddNewItem
        '
        Me.tsbEdit_Item_AddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbEdit_Item_AddNewItem, "tsbEdit_Item_AddNewItem")
        Me.tsbEdit_Item_AddNewItem.Name = "tsbEdit_Item_AddNewItem"
        '
        'tsbEdit_Item_EditExistingItem
        '
        Me.tsbEdit_Item_EditExistingItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbEdit_Item_EditExistingItem, "tsbEdit_Item_EditExistingItem")
        Me.tsbEdit_Item_EditExistingItem.Name = "tsbEdit_Item_EditExistingItem"
        '
        'tsbEdit_Orders_AddEdit
        '
        Me.tsbEdit_Orders_AddEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbEdit_Orders_AddEdit, "tsbEdit_Orders_AddEdit")
        Me.tsbEdit_Orders_AddEdit.Name = "tsbEdit_Orders_AddEdit"
        '
        'tsbEdit_OrderSearch
        '
        Me.tsbEdit_OrderSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbEdit_OrderSearch, "tsbEdit_OrderSearch")
        Me.tsbEdit_OrderSearch.Name = "tsbEdit_OrderSearch"
        '
        'tsbEdit_Item_InventoryLevel
        '
        Me.tsbEdit_Item_InventoryLevel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbEdit_Item_InventoryLevel, "tsbEdit_Item_InventoryLevel")
        Me.tsbEdit_Item_InventoryLevel.Name = "tsbEdit_Item_InventoryLevel"
        '
        'tsbEdit_Pricing_Batches
        '
        Me.tsbEdit_Pricing_Batches.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbEdit_Pricing_Batches, "tsbEdit_Pricing_Batches")
        Me.tsbEdit_Pricing_Batches.Name = "tsbEdit_Pricing_Batches"
        '
        'tsbEdit_Pricing_ReprintSigns
        '
        Me.tsbEdit_Pricing_ReprintSigns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbEdit_Pricing_ReprintSigns, "tsbEdit_Pricing_ReprintSigns")
        Me.tsbEdit_Pricing_ReprintSigns.Name = "tsbEdit_Pricing_ReprintSigns"
        '
        'tsbEdit_Pricing_PromotionalOffers
        '
        Me.tsbEdit_Pricing_PromotionalOffers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbEdit_Pricing_PromotionalOffers, "tsbEdit_Pricing_PromotionalOffers")
        Me.tsbEdit_Pricing_PromotionalOffers.Name = "tsbEdit_Pricing_PromotionalOffers"
        '
        'tsbReports_ReportManager
        '
        Me.tsbReports_ReportManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbReports_ReportManager, "tsbReports_ReportManager")
        Me.tsbReports_ReportManager.Name = "tsbReports_ReportManager"
        '
        'tsbEdit_SearchText
        '
        Me.tsbEdit_SearchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.tsbEdit_SearchText, "tsbEdit_SearchText")
        Me.tsbEdit_SearchText.Name = "tsbEdit_SearchText"
        '
        'tsbEdit_Search
        '
        Me.tsbEdit_Search.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbEdit_Search.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbEdit_Search_PO, Me.tsbEdit_Search_Identifier})
        resources.ApplyResources(Me.tsbEdit_Search, "tsbEdit_Search")
        Me.tsbEdit_Search.Name = "tsbEdit_Search"
        '
        'tsbEdit_Search_PO
        '
        Me.tsbEdit_Search_PO.Name = "tsbEdit_Search_PO"
        resources.ApplyResources(Me.tsbEdit_Search_PO, "tsbEdit_Search_PO")
        '
        'tsbEdit_Search_Identifier
        '
        Me.tsbEdit_Search_Identifier.Name = "tsbEdit_Search_Identifier"
        resources.ApplyResources(Me.tsbEdit_Search_Identifier, "tsbEdit_Search_Identifier")
        '
        'tsbOrchard
        '
        Me.tsbOrchard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbOrchard, "tsbOrchard")
        Me.tsbOrchard.Name = "tsbOrchard"
        '
        'ToolStrip_Inventory
        '
        resources.ApplyResources(Me.ToolStrip_Inventory, "ToolStrip_Inventory")
        Me.ToolStrip_Inventory.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip_Inventory.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbInventory_InventoryAdjustment})
        Me.ToolStrip_Inventory.Name = "ToolStrip_Inventory"
        '
        'tsbInventory_InventoryAdjustment
        '
        Me.tsbInventory_InventoryAdjustment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbInventory_InventoryAdjustment, "tsbInventory_InventoryAdjustment")
        Me.tsbInventory_InventoryAdjustment.Name = "tsbInventory_InventoryAdjustment"
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuEdit, Me.mnuReports, Me.mnuInventory, Me.mnuTools, Me.mnuData, Me.mnuAdministration, Me.mnuHelp})
        Me.MainMenu1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        resources.ApplyResources(Me.MainMenu1, "MainMenu1")
        Me.MainMenu1.Name = "MainMenu1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile_Export, Me.mnuFile_Import, Me.mnuFile_ProcessMonitor, Me.mnuFile_Exit})
        Me.mnuFile.Name = "mnuFile"
        resources.ApplyResources(Me.mnuFile, "mnuFile")
        '
        'mnuFile_Export
        '
        Me.mnuFile_Export.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile_Export_Planogram, Me.mnuFile_Export_RGIS})
        Me.mnuFile_Export.Name = "mnuFile_Export"
        resources.ApplyResources(Me.mnuFile_Export, "mnuFile_Export")
        '
        'mnuFile_Export_Planogram
        '
        Me.mnuFile_Export_Planogram.Name = "mnuFile_Export_Planogram"
        resources.ApplyResources(Me.mnuFile_Export_Planogram, "mnuFile_Export_Planogram")
        '
        'mnuFile_Export_RGIS
        '
        Me.mnuFile_Export_RGIS.Name = "mnuFile_Export_RGIS"
        resources.ApplyResources(Me.mnuFile_Export_RGIS, "mnuFile_Export_RGIS")
        '
        'mnuFile_Import
        '
        Me.mnuFile_Import.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile_Import_ExtendedItemMaintenance, Me.mnuFile_Import_ImportOrder, Me.mnuFile_Import_ItemMaintenanceBulkLoad, Me.mnuFile_Import_ItemStoreMaintenanceBulkLoad, Me.mnuFile_Import_ItemVendorMaintenanceBulkLoad, Me.mnuFile_Import_Planogram, Me.mnuFile_Import_RIPEOrder})
        Me.mnuFile_Import.Name = "mnuFile_Import"
        resources.ApplyResources(Me.mnuFile_Import, "mnuFile_Import")
        '
        'mnuFile_Import_ExtendedItemMaintenance
        '
        Me.mnuFile_Import_ExtendedItemMaintenance.Name = "mnuFile_Import_ExtendedItemMaintenance"
        resources.ApplyResources(Me.mnuFile_Import_ExtendedItemMaintenance, "mnuFile_Import_ExtendedItemMaintenance")
        '
        'mnuFile_Import_ImportOrder
        '
        Me.mnuFile_Import_ImportOrder.Name = "mnuFile_Import_ImportOrder"
        resources.ApplyResources(Me.mnuFile_Import_ImportOrder, "mnuFile_Import_ImportOrder")
        '
        'mnuFile_Import_ItemMaintenanceBulkLoad
        '
        Me.mnuFile_Import_ItemMaintenanceBulkLoad.Name = "mnuFile_Import_ItemMaintenanceBulkLoad"
        resources.ApplyResources(Me.mnuFile_Import_ItemMaintenanceBulkLoad, "mnuFile_Import_ItemMaintenanceBulkLoad")
        '
        'mnuFile_Import_ItemStoreMaintenanceBulkLoad
        '
        Me.mnuFile_Import_ItemStoreMaintenanceBulkLoad.Name = "mnuFile_Import_ItemStoreMaintenanceBulkLoad"
        resources.ApplyResources(Me.mnuFile_Import_ItemStoreMaintenanceBulkLoad, "mnuFile_Import_ItemStoreMaintenanceBulkLoad")
        '
        'mnuFile_Import_ItemVendorMaintenanceBulkLoad
        '
        Me.mnuFile_Import_ItemVendorMaintenanceBulkLoad.Name = "mnuFile_Import_ItemVendorMaintenanceBulkLoad"
        resources.ApplyResources(Me.mnuFile_Import_ItemVendorMaintenanceBulkLoad, "mnuFile_Import_ItemVendorMaintenanceBulkLoad")
        '
        'mnuFile_Import_Planogram
        '
        Me.mnuFile_Import_Planogram.Name = "mnuFile_Import_Planogram"
        resources.ApplyResources(Me.mnuFile_Import_Planogram, "mnuFile_Import_Planogram")
        '
        'mnuFile_Import_RIPEOrder
        '
        Me.mnuFile_Import_RIPEOrder.Name = "mnuFile_Import_RIPEOrder"
        resources.ApplyResources(Me.mnuFile_Import_RIPEOrder, "mnuFile_Import_RIPEOrder")
        '
        'mnuFile_ProcessMonitor
        '
        resources.ApplyResources(Me.mnuFile_ProcessMonitor, "mnuFile_ProcessMonitor")
        Me.mnuFile_ProcessMonitor.Name = "mnuFile_ProcessMonitor"
        '
        'mnuFile_Exit
        '
        Me.mnuFile_Exit.Name = "mnuFile_Exit"
        resources.ApplyResources(Me.mnuFile_Exit, "mnuFile_Exit")
        '
        'mnuEdit
        '
        Me.mnuEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEdit_CompetitorStore, Me.mnuEdit_Item, Me.mnuEdit_Orders, Me.mnuEdit_Pricing, Me.mnuEdit_TaxHosting, Me.mnuEdit_Vendors})
        Me.mnuEdit.Name = "mnuEdit"
        resources.ApplyResources(Me.mnuEdit, "mnuEdit")
        '
        'mnuEdit_CompetitorStore
        '
        Me.mnuEdit_CompetitorStore.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEdit_CompetitorStore_CompetitorPrices, Me.mnuEdit_CompetitorStore_CompetitorTrend, Me.mnuEdit_CompetitorStore_ImportCompetitorPrices, Me.mnuEdit_CompetitorStore_CompetitorMarginImpact, Me.mnuEdit_CompetitorStore_NationalPurchasingValues})
        Me.mnuEdit_CompetitorStore.Name = "mnuEdit_CompetitorStore"
        resources.ApplyResources(Me.mnuEdit_CompetitorStore, "mnuEdit_CompetitorStore")
        '
        'mnuEdit_CompetitorStore_CompetitorPrices
        '
        Me.mnuEdit_CompetitorStore_CompetitorPrices.Name = "mnuEdit_CompetitorStore_CompetitorPrices"
        resources.ApplyResources(Me.mnuEdit_CompetitorStore_CompetitorPrices, "mnuEdit_CompetitorStore_CompetitorPrices")
        '
        'mnuEdit_CompetitorStore_CompetitorTrend
        '
        Me.mnuEdit_CompetitorStore_CompetitorTrend.Name = "mnuEdit_CompetitorStore_CompetitorTrend"
        resources.ApplyResources(Me.mnuEdit_CompetitorStore_CompetitorTrend, "mnuEdit_CompetitorStore_CompetitorTrend")
        '
        'mnuEdit_CompetitorStore_ImportCompetitorPrices
        '
        Me.mnuEdit_CompetitorStore_ImportCompetitorPrices.Name = "mnuEdit_CompetitorStore_ImportCompetitorPrices"
        resources.ApplyResources(Me.mnuEdit_CompetitorStore_ImportCompetitorPrices, "mnuEdit_CompetitorStore_ImportCompetitorPrices")
        '
        'mnuEdit_CompetitorStore_CompetitorMarginImpact
        '
        Me.mnuEdit_CompetitorStore_CompetitorMarginImpact.Name = "mnuEdit_CompetitorStore_CompetitorMarginImpact"
        resources.ApplyResources(Me.mnuEdit_CompetitorStore_CompetitorMarginImpact, "mnuEdit_CompetitorStore_CompetitorMarginImpact")
        '
        'mnuEdit_CompetitorStore_NationalPurchasingValues
        '
        Me.mnuEdit_CompetitorStore_NationalPurchasingValues.Name = "mnuEdit_CompetitorStore_NationalPurchasingValues"
        resources.ApplyResources(Me.mnuEdit_CompetitorStore_NationalPurchasingValues, "mnuEdit_CompetitorStore_NationalPurchasingValues")
        '
        'mnuEdit_Item
        '
        Me.mnuEdit_Item.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEdit_Item_Add, Me.mnuEdit_Item_BulkLoadAuditHistory, Me.mnuEdit_Item_EditItem, Me.mnuEdit_Item_ItemChain})
        Me.mnuEdit_Item.Name = "mnuEdit_Item"
        resources.ApplyResources(Me.mnuEdit_Item, "mnuEdit_Item")
        '
        'mnuEdit_Item_Add
        '
        Me.mnuEdit_Item_Add.Name = "mnuEdit_Item_Add"
        resources.ApplyResources(Me.mnuEdit_Item_Add, "mnuEdit_Item_Add")
        '
        'mnuEdit_Item_BulkLoadAuditHistory
        '
        Me.mnuEdit_Item_BulkLoadAuditHistory.Name = "mnuEdit_Item_BulkLoadAuditHistory"
        resources.ApplyResources(Me.mnuEdit_Item_BulkLoadAuditHistory, "mnuEdit_Item_BulkLoadAuditHistory")
        '
        'mnuEdit_Item_EditItem
        '
        Me.mnuEdit_Item_EditItem.Name = "mnuEdit_Item_EditItem"
        resources.ApplyResources(Me.mnuEdit_Item_EditItem, "mnuEdit_Item_EditItem")
        '
        'mnuEdit_Item_ItemChain
        '
        Me.mnuEdit_Item_ItemChain.Name = "mnuEdit_Item_ItemChain"
        resources.ApplyResources(Me.mnuEdit_Item_ItemChain, "mnuEdit_Item_ItemChain")
        '
        'mnuEdit_Orders
        '
        Me.mnuEdit_Orders.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEdit_Orders_AddEdit, Me.mnuEdit_Orders_Search, Me.mnuEdit_Orders_Allocate, Me.mnuEdit_Orders_BatchReceiveClose, Me.mnuEdit_Orders_EInvoicing, Me.mnuEdit_Orders_InvoiceEntry, Me.mnuEdit_Orders_SuspendedPOTool, Me.mnuEdit_Orders_ItemQueue})
        Me.mnuEdit_Orders.Name = "mnuEdit_Orders"
        resources.ApplyResources(Me.mnuEdit_Orders, "mnuEdit_Orders")
        '
        'mnuEdit_Orders_AddEdit
        '
        Me.mnuEdit_Orders_AddEdit.Name = "mnuEdit_Orders_AddEdit"
        resources.ApplyResources(Me.mnuEdit_Orders_AddEdit, "mnuEdit_Orders_AddEdit")
        '
        'mnuEdit_Orders_Search
        '
        Me.mnuEdit_Orders_Search.Name = "mnuEdit_Orders_Search"
        resources.ApplyResources(Me.mnuEdit_Orders_Search, "mnuEdit_Orders_Search")
        '
        'mnuEdit_Orders_Allocate
        '
        Me.mnuEdit_Orders_Allocate.Name = "mnuEdit_Orders_Allocate"
        resources.ApplyResources(Me.mnuEdit_Orders_Allocate, "mnuEdit_Orders_Allocate")
        '
        'mnuEdit_Orders_BatchReceiveClose
        '
        Me.mnuEdit_Orders_BatchReceiveClose.Name = "mnuEdit_Orders_BatchReceiveClose"
        resources.ApplyResources(Me.mnuEdit_Orders_BatchReceiveClose, "mnuEdit_Orders_BatchReceiveClose")
        '
        'mnuEdit_Orders_EInvoicing
        '
        Me.mnuEdit_Orders_EInvoicing.Name = "mnuEdit_Orders_EInvoicing"
        resources.ApplyResources(Me.mnuEdit_Orders_EInvoicing, "mnuEdit_Orders_EInvoicing")
        '
        'mnuEdit_Orders_InvoiceEntry
        '
        Me.mnuEdit_Orders_InvoiceEntry.Name = "mnuEdit_Orders_InvoiceEntry"
        resources.ApplyResources(Me.mnuEdit_Orders_InvoiceEntry, "mnuEdit_Orders_InvoiceEntry")
        '
        'mnuEdit_Orders_SuspendedPOTool
        '
        Me.mnuEdit_Orders_SuspendedPOTool.Name = "mnuEdit_Orders_SuspendedPOTool"
        resources.ApplyResources(Me.mnuEdit_Orders_SuspendedPOTool, "mnuEdit_Orders_SuspendedPOTool")
        '
        'mnuEdit_Orders_ItemQueue
        '
        Me.mnuEdit_Orders_ItemQueue.Name = "mnuEdit_Orders_ItemQueue"
        resources.ApplyResources(Me.mnuEdit_Orders_ItemQueue, "mnuEdit_Orders_ItemQueue")
        '
        'mnuEdit_Pricing
        '
        Me.mnuEdit_Pricing.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEdit_Pricing_Batches, Me.mnuEdit_Pricing_LineDrive, Me.mnuEdit_Pricing_PriceChangeWizard, Me.mnuEdit_Pricing_PromotionalOffers, Me.mnuEdit_Pricing_ReprintSigns})
        Me.mnuEdit_Pricing.Name = "mnuEdit_Pricing"
        resources.ApplyResources(Me.mnuEdit_Pricing, "mnuEdit_Pricing")
        '
        'mnuEdit_Pricing_Batches
        '
        Me.mnuEdit_Pricing_Batches.Name = "mnuEdit_Pricing_Batches"
        resources.ApplyResources(Me.mnuEdit_Pricing_Batches, "mnuEdit_Pricing_Batches")
        '
        'mnuEdit_Pricing_LineDrive
        '
        Me.mnuEdit_Pricing_LineDrive.Name = "mnuEdit_Pricing_LineDrive"
        resources.ApplyResources(Me.mnuEdit_Pricing_LineDrive, "mnuEdit_Pricing_LineDrive")
        '
        'mnuEdit_Pricing_PriceChangeWizard
        '
        Me.mnuEdit_Pricing_PriceChangeWizard.Name = "mnuEdit_Pricing_PriceChangeWizard"
        resources.ApplyResources(Me.mnuEdit_Pricing_PriceChangeWizard, "mnuEdit_Pricing_PriceChangeWizard")
        '
        'mnuEdit_Pricing_PromotionalOffers
        '
        Me.mnuEdit_Pricing_PromotionalOffers.Name = "mnuEdit_Pricing_PromotionalOffers"
        resources.ApplyResources(Me.mnuEdit_Pricing_PromotionalOffers, "mnuEdit_Pricing_PromotionalOffers")
        '
        'mnuEdit_Pricing_ReprintSigns
        '
        Me.mnuEdit_Pricing_ReprintSigns.Name = "mnuEdit_Pricing_ReprintSigns"
        resources.ApplyResources(Me.mnuEdit_Pricing_ReprintSigns, "mnuEdit_Pricing_ReprintSigns")
        '
        'mnuEdit_TaxHosting
        '
        Me.mnuEdit_TaxHosting.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEdit_TaxHosting_TaxClassification, Me.mnuEdit_TaxHosting_TaxFlag, Me.mnuEdit_TaxHosting_TaxJurisdiction})
        Me.mnuEdit_TaxHosting.Name = "mnuEdit_TaxHosting"
        resources.ApplyResources(Me.mnuEdit_TaxHosting, "mnuEdit_TaxHosting")
        '
        'mnuEdit_TaxHosting_TaxClassification
        '
        Me.mnuEdit_TaxHosting_TaxClassification.Name = "mnuEdit_TaxHosting_TaxClassification"
        resources.ApplyResources(Me.mnuEdit_TaxHosting_TaxClassification, "mnuEdit_TaxHosting_TaxClassification")
        '
        'mnuEdit_TaxHosting_TaxFlag
        '
        Me.mnuEdit_TaxHosting_TaxFlag.Name = "mnuEdit_TaxHosting_TaxFlag"
        resources.ApplyResources(Me.mnuEdit_TaxHosting_TaxFlag, "mnuEdit_TaxHosting_TaxFlag")
        '
        'mnuEdit_TaxHosting_TaxJurisdiction
        '
        Me.mnuEdit_TaxHosting_TaxJurisdiction.Name = "mnuEdit_TaxHosting_TaxJurisdiction"
        resources.ApplyResources(Me.mnuEdit_TaxHosting_TaxJurisdiction, "mnuEdit_TaxHosting_TaxJurisdiction")
        '
        'mnuEdit_Vendors
        '
        Me.mnuEdit_Vendors.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEdit_Vendors_AddEdit, Me.mnuEdit_Vendors_CostImportExceptions})
        Me.mnuEdit_Vendors.Name = "mnuEdit_Vendors"
        resources.ApplyResources(Me.mnuEdit_Vendors, "mnuEdit_Vendors")
        '
        'mnuEdit_Vendors_AddEdit
        '
        Me.mnuEdit_Vendors_AddEdit.Name = "mnuEdit_Vendors_AddEdit"
        resources.ApplyResources(Me.mnuEdit_Vendors_AddEdit, "mnuEdit_Vendors_AddEdit")
        '
        'mnuEdit_Vendors_CostImportExceptions
        '
        Me.mnuEdit_Vendors_CostImportExceptions.Name = "mnuEdit_Vendors_CostImportExceptions"
        resources.ApplyResources(Me.mnuEdit_Vendors_CostImportExceptions, "mnuEdit_Vendors_CostImportExceptions")
        '
        'mnuReports
        '
        Me.mnuReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Accounting, Me.mnuReports_Audit, Me.mnuReports_Buyer, Me.mnuReports_Item, Me.mnuReports_Inventory, Me.mnuReports_Miscellaneous, Me.mnuReports_Order, Me.mnuReports_Price, Me.mnuReports_ReceivingDistribution, Me.mnuReports_Purchases, Me.mnuReports_ReportManager, Me.mnuReports_Tax, Me.mnuReports_3WayMatching})
        Me.mnuReports.Name = "mnuReports"
        resources.ApplyResources(Me.mnuReports, "mnuReports")
        '
        'mnuReports_Accounting
        '
        Me.mnuReports_Accounting.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Accounting_AdjustmentSummary, Me.mnuReports_Accounting_GLSummary, Me.mnuReports_Accounting_GLUploads, Me.mnuReports_Accounting_InvoiceDiscrepancies, Me.mnuReports_Accounting_InvoiceManifest, Me.mnuReports_Accounting_OutOfPeriodInvoiceReport, Me.mnuReports_Accounting_PJReconcile, Me.mnuReports_Accounting_PurchaseAccrualReport, Me.mnuReports_Accounting_PurchaseToSalesComp})
        Me.mnuReports_Accounting.Name = "mnuReports_Accounting"
        resources.ApplyResources(Me.mnuReports_Accounting, "mnuReports_Accounting")
        '
        'mnuReports_Accounting_AdjustmentSummary
        '
        Me.mnuReports_Accounting_AdjustmentSummary.Name = "mnuReports_Accounting_AdjustmentSummary"
        resources.ApplyResources(Me.mnuReports_Accounting_AdjustmentSummary, "mnuReports_Accounting_AdjustmentSummary")
        '
        'mnuReports_Accounting_GLSummary
        '
        Me.mnuReports_Accounting_GLSummary.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Accounting_GLSummary_GLDistributionCheck, Me.mnuReports_Accounting_GLSummary_GLSales, Me.mnuReports_Accounting_GLSummary_GLTransfers})
        Me.mnuReports_Accounting_GLSummary.Name = "mnuReports_Accounting_GLSummary"
        resources.ApplyResources(Me.mnuReports_Accounting_GLSummary, "mnuReports_Accounting_GLSummary")
        '
        'mnuReports_Accounting_GLSummary_GLDistributionCheck
        '
        Me.mnuReports_Accounting_GLSummary_GLDistributionCheck.Name = "mnuReports_Accounting_GLSummary_GLDistributionCheck"
        resources.ApplyResources(Me.mnuReports_Accounting_GLSummary_GLDistributionCheck, "mnuReports_Accounting_GLSummary_GLDistributionCheck")
        '
        'mnuReports_Accounting_GLSummary_GLSales
        '
        Me.mnuReports_Accounting_GLSummary_GLSales.Name = "mnuReports_Accounting_GLSummary_GLSales"
        resources.ApplyResources(Me.mnuReports_Accounting_GLSummary_GLSales, "mnuReports_Accounting_GLSummary_GLSales")
        '
        'mnuReports_Accounting_GLSummary_GLTransfers
        '
        Me.mnuReports_Accounting_GLSummary_GLTransfers.Name = "mnuReports_Accounting_GLSummary_GLTransfers"
        resources.ApplyResources(Me.mnuReports_Accounting_GLSummary_GLTransfers, "mnuReports_Accounting_GLSummary_GLTransfers")
        '
        'mnuReports_Accounting_GLUploads
        '
        Me.mnuReports_Accounting_GLUploads.Name = "mnuReports_Accounting_GLUploads"
        resources.ApplyResources(Me.mnuReports_Accounting_GLUploads, "mnuReports_Accounting_GLUploads")
        '
        'mnuReports_Accounting_InvoiceDiscrepancies
        '
        Me.mnuReports_Accounting_InvoiceDiscrepancies.Name = "mnuReports_Accounting_InvoiceDiscrepancies"
        resources.ApplyResources(Me.mnuReports_Accounting_InvoiceDiscrepancies, "mnuReports_Accounting_InvoiceDiscrepancies")
        '
        'mnuReports_Accounting_InvoiceManifest
        '
        Me.mnuReports_Accounting_InvoiceManifest.Name = "mnuReports_Accounting_InvoiceManifest"
        resources.ApplyResources(Me.mnuReports_Accounting_InvoiceManifest, "mnuReports_Accounting_InvoiceManifest")
        '
        'mnuReports_Accounting_OutOfPeriodInvoiceReport
        '
        Me.mnuReports_Accounting_OutOfPeriodInvoiceReport.Name = "mnuReports_Accounting_OutOfPeriodInvoiceReport"
        resources.ApplyResources(Me.mnuReports_Accounting_OutOfPeriodInvoiceReport, "mnuReports_Accounting_OutOfPeriodInvoiceReport")
        '
        'mnuReports_Accounting_PJReconcile
        '
        Me.mnuReports_Accounting_PJReconcile.Name = "mnuReports_Accounting_PJReconcile"
        resources.ApplyResources(Me.mnuReports_Accounting_PJReconcile, "mnuReports_Accounting_PJReconcile")
        '
        'mnuReports_Accounting_PurchaseAccrualReport
        '
        Me.mnuReports_Accounting_PurchaseAccrualReport.Name = "mnuReports_Accounting_PurchaseAccrualReport"
        resources.ApplyResources(Me.mnuReports_Accounting_PurchaseAccrualReport, "mnuReports_Accounting_PurchaseAccrualReport")
        '
        'mnuReports_Accounting_PurchaseToSalesComp
        '
        Me.mnuReports_Accounting_PurchaseToSalesComp.Name = "mnuReports_Accounting_PurchaseToSalesComp"
        resources.ApplyResources(Me.mnuReports_Accounting_PurchaseToSalesComp, "mnuReports_Accounting_PurchaseToSalesComp")
        '
        'mnuReports_Audit
        '
        Me.mnuReports_Audit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Audit_AVCIExceptions})
        Me.mnuReports_Audit.Name = "mnuReports_Audit"
        resources.ApplyResources(Me.mnuReports_Audit, "mnuReports_Audit")
        '
        'mnuReports_Audit_AVCIExceptions
        '
        Me.mnuReports_Audit_AVCIExceptions.Name = "mnuReports_Audit_AVCIExceptions"
        resources.ApplyResources(Me.mnuReports_Audit_AVCIExceptions, "mnuReports_Audit_AVCIExceptions")
        '
        'mnuReports_Buyer
        '
        Me.mnuReports_Buyer.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Buyer_CostException, Me.mnuReports_Buyer_InventoryBalance, Me.mnuReports_Buyer_Margin, Me.mnuReports_Buyer_StoreOrdersTotalBySKU, Me.mnuReports_Buyer_ZeroAverageCostItems, Me.mnuReports_Buyer_ZeroPriceItems, Me.mnuReports_Buyer_WarehouseMovement})
        Me.mnuReports_Buyer.Name = "mnuReports_Buyer"
        resources.ApplyResources(Me.mnuReports_Buyer, "mnuReports_Buyer")
        '
        'mnuReports_Buyer_CostException
        '
        Me.mnuReports_Buyer_CostException.Name = "mnuReports_Buyer_CostException"
        resources.ApplyResources(Me.mnuReports_Buyer_CostException, "mnuReports_Buyer_CostException")
        '
        'mnuReports_Buyer_InventoryBalance
        '
        Me.mnuReports_Buyer_InventoryBalance.Name = "mnuReports_Buyer_InventoryBalance"
        resources.ApplyResources(Me.mnuReports_Buyer_InventoryBalance, "mnuReports_Buyer_InventoryBalance")
        '
        'mnuReports_Buyer_Margin
        '
        Me.mnuReports_Buyer_Margin.Name = "mnuReports_Buyer_Margin"
        resources.ApplyResources(Me.mnuReports_Buyer_Margin, "mnuReports_Buyer_Margin")
        '
        'mnuReports_Buyer_StoreOrdersTotalBySKU
        '
        Me.mnuReports_Buyer_StoreOrdersTotalBySKU.Name = "mnuReports_Buyer_StoreOrdersTotalBySKU"
        resources.ApplyResources(Me.mnuReports_Buyer_StoreOrdersTotalBySKU, "mnuReports_Buyer_StoreOrdersTotalBySKU")
        '
        'mnuReports_Buyer_ZeroAverageCostItems
        '
        Me.mnuReports_Buyer_ZeroAverageCostItems.Name = "mnuReports_Buyer_ZeroAverageCostItems"
        resources.ApplyResources(Me.mnuReports_Buyer_ZeroAverageCostItems, "mnuReports_Buyer_ZeroAverageCostItems")
        '
        'mnuReports_Buyer_ZeroPriceItems
        '
        Me.mnuReports_Buyer_ZeroPriceItems.Name = "mnuReports_Buyer_ZeroPriceItems"
        resources.ApplyResources(Me.mnuReports_Buyer_ZeroPriceItems, "mnuReports_Buyer_ZeroPriceItems")
        '
        'mnuReports_Buyer_WarehouseMovement
        '
        Me.mnuReports_Buyer_WarehouseMovement.Name = "mnuReports_Buyer_WarehouseMovement"
        resources.ApplyResources(Me.mnuReports_Buyer_WarehouseMovement, "mnuReports_Buyer_WarehouseMovement")
        '
        'mnuReports_Item
        '
        Me.mnuReports_Item.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Item_ActiveItemList, Me.mnuReports_Item_DiscontinueItemsWithInventory, Me.mnuReports_Item_InventoryGuide, Me.mnuReports_Item_ItemList, Me.mnuReports_Item_ItemOnHandComparisonBetweenLocation, Me.mnuReports_Item_ItemOrderHistory, Me.mnuReports_Item_NotAvailableItems, Me.mnuReports_Item_OrderGuide, Me.mnuReports_Item_ReorderSummaryByVendor, Me.mnuReports_Item_ShipperItems, Me.mnuReports_Item_UPCLabels, Me.mnuReports_Item_VendorItems})
        Me.mnuReports_Item.Name = "mnuReports_Item"
        resources.ApplyResources(Me.mnuReports_Item, "mnuReports_Item")
        '
        'mnuReports_Item_ActiveItemList
        '
        Me.mnuReports_Item_ActiveItemList.Name = "mnuReports_Item_ActiveItemList"
        resources.ApplyResources(Me.mnuReports_Item_ActiveItemList, "mnuReports_Item_ActiveItemList")
        '
        'mnuReports_Item_DiscontinueItemsWithInventory
        '
        Me.mnuReports_Item_DiscontinueItemsWithInventory.Name = "mnuReports_Item_DiscontinueItemsWithInventory"
        resources.ApplyResources(Me.mnuReports_Item_DiscontinueItemsWithInventory, "mnuReports_Item_DiscontinueItemsWithInventory")
        '
        'mnuReports_Item_InventoryGuide
        '
        Me.mnuReports_Item_InventoryGuide.Name = "mnuReports_Item_InventoryGuide"
        resources.ApplyResources(Me.mnuReports_Item_InventoryGuide, "mnuReports_Item_InventoryGuide")
        '
        'mnuReports_Item_ItemList
        '
        Me.mnuReports_Item_ItemList.Name = "mnuReports_Item_ItemList"
        resources.ApplyResources(Me.mnuReports_Item_ItemList, "mnuReports_Item_ItemList")
        '
        'mnuReports_Item_ItemOnHandComparisonBetweenLocation
        '
        Me.mnuReports_Item_ItemOnHandComparisonBetweenLocation.Name = "mnuReports_Item_ItemOnHandComparisonBetweenLocation"
        resources.ApplyResources(Me.mnuReports_Item_ItemOnHandComparisonBetweenLocation, "mnuReports_Item_ItemOnHandComparisonBetweenLocation")
        '
        'mnuReports_Item_ItemOrderHistory
        '
        Me.mnuReports_Item_ItemOrderHistory.Name = "mnuReports_Item_ItemOrderHistory"
        resources.ApplyResources(Me.mnuReports_Item_ItemOrderHistory, "mnuReports_Item_ItemOrderHistory")
        '
        'mnuReports_Item_NotAvailableItems
        '
        Me.mnuReports_Item_NotAvailableItems.Name = "mnuReports_Item_NotAvailableItems"
        resources.ApplyResources(Me.mnuReports_Item_NotAvailableItems, "mnuReports_Item_NotAvailableItems")
        '
        'mnuReports_Item_OrderGuide
        '
        Me.mnuReports_Item_OrderGuide.Name = "mnuReports_Item_OrderGuide"
        resources.ApplyResources(Me.mnuReports_Item_OrderGuide, "mnuReports_Item_OrderGuide")
        '
        'mnuReports_Item_ReorderSummaryByVendor
        '
        Me.mnuReports_Item_ReorderSummaryByVendor.Name = "mnuReports_Item_ReorderSummaryByVendor"
        resources.ApplyResources(Me.mnuReports_Item_ReorderSummaryByVendor, "mnuReports_Item_ReorderSummaryByVendor")
        '
        'mnuReports_Item_ShipperItems
        '
        Me.mnuReports_Item_ShipperItems.Name = "mnuReports_Item_ShipperItems"
        resources.ApplyResources(Me.mnuReports_Item_ShipperItems, "mnuReports_Item_ShipperItems")
        '
        'mnuReports_Item_UPCLabels
        '
        Me.mnuReports_Item_UPCLabels.Name = "mnuReports_Item_UPCLabels"
        resources.ApplyResources(Me.mnuReports_Item_UPCLabels, "mnuReports_Item_UPCLabels")
        '
        'mnuReports_Item_VendorItems
        '
        Me.mnuReports_Item_VendorItems.Name = "mnuReports_Item_VendorItems"
        resources.ApplyResources(Me.mnuReports_Item_VendorItems, "mnuReports_Item_VendorItems")
        '
        'mnuReports_Inventory
        '
        Me.mnuReports_Inventory.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Inventory_BOHCompare, Me.mnuReports_Inventory_CycleCountReports, Me.mnuReports_Inventory_InventoryValue, Me.mnuReports_Inventory_InventoryWeeklyHistory, Me.mnuReports_Inventory_LocationItemsManulCountSheet, Me.mnuReports_Inventory_LocationItemsReport, Me.mnuReports_Inventory_LocationsReport})
        Me.mnuReports_Inventory.Name = "mnuReports_Inventory"
        resources.ApplyResources(Me.mnuReports_Inventory, "mnuReports_Inventory")
        '
        'mnuReports_Inventory_BOHCompare
        '
        Me.mnuReports_Inventory_BOHCompare.Name = "mnuReports_Inventory_BOHCompare"
        resources.ApplyResources(Me.mnuReports_Inventory_BOHCompare, "mnuReports_Inventory_BOHCompare")
        '
        'mnuReports_Inventory_CycleCountReports
        '
        Me.mnuReports_Inventory_CycleCountReports.Name = "mnuReports_Inventory_CycleCountReports"
        resources.ApplyResources(Me.mnuReports_Inventory_CycleCountReports, "mnuReports_Inventory_CycleCountReports")
        '
        'mnuReports_Inventory_InventoryValue
        '
        Me.mnuReports_Inventory_InventoryValue.Name = "mnuReports_Inventory_InventoryValue"
        resources.ApplyResources(Me.mnuReports_Inventory_InventoryValue, "mnuReports_Inventory_InventoryValue")
        '
        'mnuReports_Inventory_InventoryWeeklyHistory
        '
        Me.mnuReports_Inventory_InventoryWeeklyHistory.Name = "mnuReports_Inventory_InventoryWeeklyHistory"
        resources.ApplyResources(Me.mnuReports_Inventory_InventoryWeeklyHistory, "mnuReports_Inventory_InventoryWeeklyHistory")
        '
        'mnuReports_Inventory_LocationItemsManulCountSheet
        '
        Me.mnuReports_Inventory_LocationItemsManulCountSheet.Name = "mnuReports_Inventory_LocationItemsManulCountSheet"
        resources.ApplyResources(Me.mnuReports_Inventory_LocationItemsManulCountSheet, "mnuReports_Inventory_LocationItemsManulCountSheet")
        '
        'mnuReports_Inventory_LocationItemsReport
        '
        Me.mnuReports_Inventory_LocationItemsReport.Name = "mnuReports_Inventory_LocationItemsReport"
        resources.ApplyResources(Me.mnuReports_Inventory_LocationItemsReport, "mnuReports_Inventory_LocationItemsReport")
        '
        'mnuReports_Inventory_LocationsReport
        '
        Me.mnuReports_Inventory_LocationsReport.Name = "mnuReports_Inventory_LocationsReport"
        resources.ApplyResources(Me.mnuReports_Inventory_LocationsReport, "mnuReports_Inventory_LocationsReport")
        '
        'mnuReports_Miscellaneous
        '
        Me.mnuReports_Miscellaneous.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Miscellaneous_CasesBySubTeam, Me.mnuReports_Miscellaneous_CasesBySubTeamAudit, Me.mnuReports_Miscellaneous_LotNo, Me.mnuReports_Miscellaneous_VendorEfficiency})
        Me.mnuReports_Miscellaneous.Name = "mnuReports_Miscellaneous"
        resources.ApplyResources(Me.mnuReports_Miscellaneous, "mnuReports_Miscellaneous")
        '
        'mnuReports_Miscellaneous_CasesBySubTeam
        '
        Me.mnuReports_Miscellaneous_CasesBySubTeam.Name = "mnuReports_Miscellaneous_CasesBySubTeam"
        resources.ApplyResources(Me.mnuReports_Miscellaneous_CasesBySubTeam, "mnuReports_Miscellaneous_CasesBySubTeam")
        '
        'mnuReports_Miscellaneous_CasesBySubTeamAudit
        '
        Me.mnuReports_Miscellaneous_CasesBySubTeamAudit.Name = "mnuReports_Miscellaneous_CasesBySubTeamAudit"
        resources.ApplyResources(Me.mnuReports_Miscellaneous_CasesBySubTeamAudit, "mnuReports_Miscellaneous_CasesBySubTeamAudit")
        '
        'mnuReports_Miscellaneous_LotNo
        '
        Me.mnuReports_Miscellaneous_LotNo.Name = "mnuReports_Miscellaneous_LotNo"
        resources.ApplyResources(Me.mnuReports_Miscellaneous_LotNo, "mnuReports_Miscellaneous_LotNo")
        '
        'mnuReports_Miscellaneous_VendorEfficiency
        '
        Me.mnuReports_Miscellaneous_VendorEfficiency.Name = "mnuReports_Miscellaneous_VendorEfficiency"
        resources.ApplyResources(Me.mnuReports_Miscellaneous_VendorEfficiency, "mnuReports_Miscellaneous_VendorEfficiency")
        '
        'mnuReports_Order
        '
        Me.mnuReports_Order.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Order_APUpload, Me.mnuReports_Order_APUPAccrualsClosed, Me.mnuReports_Order_ClosedOrders, Me.mnuReports_Order_ClosedOrdersMissingInvoiceData, Me.mnuReports_Order_CreditReason, Me.mnuReports_Order_DeletedOrders, Me.mnuReports_Order_KitchenCaseTransfer, Me.mnuReports_Order_OpenPurchaseOrders, Me.mnuReports_Order_OrdersReceivedNotClosed, Me.mnuReports_Order_OutOfStock, Me.mnuReports_Order_POCostChanges, Me.mnuReports_Order_Short})
        Me.mnuReports_Order.Name = "mnuReports_Order"
        resources.ApplyResources(Me.mnuReports_Order, "mnuReports_Order")
        '
        'mnuReports_Order_APUpload
        '
        Me.mnuReports_Order_APUpload.Name = "mnuReports_Order_APUpload"
        resources.ApplyResources(Me.mnuReports_Order_APUpload, "mnuReports_Order_APUpload")
        '
        'mnuReports_Order_APUPAccrualsClosed
        '
        Me.mnuReports_Order_APUPAccrualsClosed.Name = "mnuReports_Order_APUPAccrualsClosed"
        resources.ApplyResources(Me.mnuReports_Order_APUPAccrualsClosed, "mnuReports_Order_APUPAccrualsClosed")
        '
        'mnuReports_Order_ClosedOrders
        '
        Me.mnuReports_Order_ClosedOrders.Name = "mnuReports_Order_ClosedOrders"
        resources.ApplyResources(Me.mnuReports_Order_ClosedOrders, "mnuReports_Order_ClosedOrders")
        '
        'mnuReports_Order_ClosedOrdersMissingInvoiceData
        '
        Me.mnuReports_Order_ClosedOrdersMissingInvoiceData.Name = "mnuReports_Order_ClosedOrdersMissingInvoiceData"
        resources.ApplyResources(Me.mnuReports_Order_ClosedOrdersMissingInvoiceData, "mnuReports_Order_ClosedOrdersMissingInvoiceData")
        '
        'mnuReports_Order_CreditReason
        '
        Me.mnuReports_Order_CreditReason.Name = "mnuReports_Order_CreditReason"
        resources.ApplyResources(Me.mnuReports_Order_CreditReason, "mnuReports_Order_CreditReason")
        '
        'mnuReports_Order_DeletedOrders
        '
        Me.mnuReports_Order_DeletedOrders.Name = "mnuReports_Order_DeletedOrders"
        resources.ApplyResources(Me.mnuReports_Order_DeletedOrders, "mnuReports_Order_DeletedOrders")
        '
        'mnuReports_Order_KitchenCaseTransfer
        '
        Me.mnuReports_Order_KitchenCaseTransfer.Name = "mnuReports_Order_KitchenCaseTransfer"
        resources.ApplyResources(Me.mnuReports_Order_KitchenCaseTransfer, "mnuReports_Order_KitchenCaseTransfer")
        '
        'mnuReports_Order_OpenPurchaseOrders
        '
        Me.mnuReports_Order_OpenPurchaseOrders.Name = "mnuReports_Order_OpenPurchaseOrders"
        resources.ApplyResources(Me.mnuReports_Order_OpenPurchaseOrders, "mnuReports_Order_OpenPurchaseOrders")
        '
        'mnuReports_Order_OrdersReceivedNotClosed
        '
        Me.mnuReports_Order_OrdersReceivedNotClosed.Name = "mnuReports_Order_OrdersReceivedNotClosed"
        resources.ApplyResources(Me.mnuReports_Order_OrdersReceivedNotClosed, "mnuReports_Order_OrdersReceivedNotClosed")
        '
        'mnuReports_Order_OutOfStock
        '
        Me.mnuReports_Order_OutOfStock.Name = "mnuReports_Order_OutOfStock"
        resources.ApplyResources(Me.mnuReports_Order_OutOfStock, "mnuReports_Order_OutOfStock")
        '
        'mnuReports_Order_POCostChanges
        '
        Me.mnuReports_Order_POCostChanges.Name = "mnuReports_Order_POCostChanges"
        resources.ApplyResources(Me.mnuReports_Order_POCostChanges, "mnuReports_Order_POCostChanges")
        '
        'mnuReports_Order_Short
        '
        Me.mnuReports_Order_Short.Name = "mnuReports_Order_Short"
        resources.ApplyResources(Me.mnuReports_Order_Short, "mnuReports_Order_Short")
        '
        'mnuReports_Price
        '
        Me.mnuReports_Price.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Price_Batches, Me.mnuReports_Price_ItemPrice, Me.mnuReports_Price_PriceChanges, Me.mnuReports_Price_Specials})
        Me.mnuReports_Price.Name = "mnuReports_Price"
        resources.ApplyResources(Me.mnuReports_Price, "mnuReports_Price")
        '
        'mnuReports_Price_Batches
        '
        Me.mnuReports_Price_Batches.Name = "mnuReports_Price_Batches"
        resources.ApplyResources(Me.mnuReports_Price_Batches, "mnuReports_Price_Batches")
        '
        'mnuReports_Price_ItemPrice
        '
        Me.mnuReports_Price_ItemPrice.Name = "mnuReports_Price_ItemPrice"
        resources.ApplyResources(Me.mnuReports_Price_ItemPrice, "mnuReports_Price_ItemPrice")
        '
        'mnuReports_Price_PriceChanges
        '
        Me.mnuReports_Price_PriceChanges.Name = "mnuReports_Price_PriceChanges"
        resources.ApplyResources(Me.mnuReports_Price_PriceChanges, "mnuReports_Price_PriceChanges")
        '
        'mnuReports_Price_Specials
        '
        Me.mnuReports_Price_Specials.Name = "mnuReports_Price_Specials"
        resources.ApplyResources(Me.mnuReports_Price_Specials, "mnuReports_Price_Specials")
        '
        'mnuReports_ReceivingDistribution
        '
        Me.mnuReports_ReceivingDistribution.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_ReceivingDistribution_AverageCost, Me.mnuReports_ReceivingDistribution_COOL, Me.mnuReports_ReceivingDistribution_DailyReceiving, Me.mnuReports_ReceivingDistribution_DCStoreRetailPriceReport, Me.mnuReports_ReceivingDistribution_OrderFulfillment, Me.mnuReports_ReceivingDistribution_ReceivingLog, Me.PartialShippmentToolStripMenuItem})
        Me.mnuReports_ReceivingDistribution.Name = "mnuReports_ReceivingDistribution"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution, "mnuReports_ReceivingDistribution")
        '
        'mnuReports_ReceivingDistribution_AverageCost
        '
        Me.mnuReports_ReceivingDistribution_AverageCost.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance, Me.mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport, Me.mnuReports_ReceivingDistribution_AverageCost_DistributionMargin, Me.mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge, Me.mnuReports_ReceivingDistribution_AverageCost_List})
        Me.mnuReports_ReceivingDistribution_AverageCost.Name = "mnuReports_ReceivingDistribution_AverageCost"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_AverageCost, "mnuReports_ReceivingDistribution_AverageCost")
        '
        'mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance
        '
        Me.mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance.Name = "mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance, "mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance")
        '
        'mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport
        '
        Me.mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport.Name = "mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport, "mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport")
        '
        'mnuReports_ReceivingDistribution_AverageCost_DistributionMargin
        '
        Me.mnuReports_ReceivingDistribution_AverageCost_DistributionMargin.Name = "mnuReports_ReceivingDistribution_AverageCost_DistributionMargin"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_AverageCost_DistributionMargin, "mnuReports_ReceivingDistribution_AverageCost_DistributionMargin")
        '
        'mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge
        '
        Me.mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge.Name = "mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge, "mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge")
        '
        'mnuReports_ReceivingDistribution_AverageCost_List
        '
        Me.mnuReports_ReceivingDistribution_AverageCost_List.Name = "mnuReports_ReceivingDistribution_AverageCost_List"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_AverageCost_List, "mnuReports_ReceivingDistribution_AverageCost_List")
        '
        'mnuReports_ReceivingDistribution_COOL
        '
        Me.mnuReports_ReceivingDistribution_COOL.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_ReceivingDistribution_COOL_ReceivingReport, Me.mnuReports_ReceivingDistribution_COOL_ShippingReport})
        Me.mnuReports_ReceivingDistribution_COOL.Name = "mnuReports_ReceivingDistribution_COOL"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_COOL, "mnuReports_ReceivingDistribution_COOL")
        '
        'mnuReports_ReceivingDistribution_COOL_ReceivingReport
        '
        Me.mnuReports_ReceivingDistribution_COOL_ReceivingReport.Name = "mnuReports_ReceivingDistribution_COOL_ReceivingReport"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_COOL_ReceivingReport, "mnuReports_ReceivingDistribution_COOL_ReceivingReport")
        '
        'mnuReports_ReceivingDistribution_COOL_ShippingReport
        '
        Me.mnuReports_ReceivingDistribution_COOL_ShippingReport.Name = "mnuReports_ReceivingDistribution_COOL_ShippingReport"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_COOL_ShippingReport, "mnuReports_ReceivingDistribution_COOL_ShippingReport")
        '
        'mnuReports_ReceivingDistribution_DailyReceiving
        '
        Me.mnuReports_ReceivingDistribution_DailyReceiving.Name = "mnuReports_ReceivingDistribution_DailyReceiving"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_DailyReceiving, "mnuReports_ReceivingDistribution_DailyReceiving")
        '
        'mnuReports_ReceivingDistribution_DCStoreRetailPriceReport
        '
        Me.mnuReports_ReceivingDistribution_DCStoreRetailPriceReport.Name = "mnuReports_ReceivingDistribution_DCStoreRetailPriceReport"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_DCStoreRetailPriceReport, "mnuReports_ReceivingDistribution_DCStoreRetailPriceReport")
        '
        'mnuReports_ReceivingDistribution_OrderFulfillment
        '
        Me.mnuReports_ReceivingDistribution_OrderFulfillment.Name = "mnuReports_ReceivingDistribution_OrderFulfillment"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_OrderFulfillment, "mnuReports_ReceivingDistribution_OrderFulfillment")
        '
        'mnuReports_ReceivingDistribution_ReceivingLog
        '
        Me.mnuReports_ReceivingDistribution_ReceivingLog.Name = "mnuReports_ReceivingDistribution_ReceivingLog"
        resources.ApplyResources(Me.mnuReports_ReceivingDistribution_ReceivingLog, "mnuReports_ReceivingDistribution_ReceivingLog")
        '
        'PartialShippmentToolStripMenuItem
        '
        Me.PartialShippmentToolStripMenuItem.Name = "PartialShippmentToolStripMenuItem"
        resources.ApplyResources(Me.PartialShippmentToolStripMenuItem, "PartialShippmentToolStripMenuItem")
        '
        'mnuReports_Purchases
        '
        Me.mnuReports_Purchases.Name = "mnuReports_Purchases"
        resources.ApplyResources(Me.mnuReports_Purchases, "mnuReports_Purchases")
        '
        'mnuReports_ReportManager
        '
        Me.mnuReports_ReportManager.Name = "mnuReports_ReportManager"
        resources.ApplyResources(Me.mnuReports_ReportManager, "mnuReports_ReportManager")
        '
        'mnuReports_Tax
        '
        Me.mnuReports_Tax.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_Tax_NewItemTaxClass, Me.mnuReports_Tax_ModifiedItemTaxClass})
        Me.mnuReports_Tax.Name = "mnuReports_Tax"
        resources.ApplyResources(Me.mnuReports_Tax, "mnuReports_Tax")
        '
        'mnuReports_Tax_NewItemTaxClass
        '
        Me.mnuReports_Tax_NewItemTaxClass.Name = "mnuReports_Tax_NewItemTaxClass"
        resources.ApplyResources(Me.mnuReports_Tax_NewItemTaxClass, "mnuReports_Tax_NewItemTaxClass")
        '
        'mnuReports_Tax_ModifiedItemTaxClass
        '
        Me.mnuReports_Tax_ModifiedItemTaxClass.Name = "mnuReports_Tax_ModifiedItemTaxClass"
        resources.ApplyResources(Me.mnuReports_Tax_ModifiedItemTaxClass, "mnuReports_Tax_ModifiedItemTaxClass")
        '
        'mnuReports_3WayMatching
        '
        Me.mnuReports_3WayMatching.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReports_3WayMatching_ControlGroupLog, Me.mnuReports_3WayMatching_ControlGroup3WayMatch})
        Me.mnuReports_3WayMatching.Name = "mnuReports_3WayMatching"
        resources.ApplyResources(Me.mnuReports_3WayMatching, "mnuReports_3WayMatching")
        '
        'mnuReports_3WayMatching_ControlGroupLog
        '
        Me.mnuReports_3WayMatching_ControlGroupLog.Name = "mnuReports_3WayMatching_ControlGroupLog"
        resources.ApplyResources(Me.mnuReports_3WayMatching_ControlGroupLog, "mnuReports_3WayMatching_ControlGroupLog")
        '
        'mnuReports_3WayMatching_ControlGroup3WayMatch
        '
        Me.mnuReports_3WayMatching_ControlGroup3WayMatch.Name = "mnuReports_3WayMatching_ControlGroup3WayMatch"
        resources.ApplyResources(Me.mnuReports_3WayMatching_ControlGroup3WayMatch, "mnuReports_3WayMatching_ControlGroup3WayMatch")
        '
        'mnuInventory
        '
        Me.mnuInventory.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuInventory_Adjustments, Me.mnuInventory_CycleCounts, Me.mnuInventory_InventoryCosting, Me.mnuInventory_Locations, Me.mnuInventory_ShrinkCorrections})
        Me.mnuInventory.Name = "mnuInventory"
        resources.ApplyResources(Me.mnuInventory, "mnuInventory")
        '
        'mnuInventory_Adjustments
        '
        Me.mnuInventory_Adjustments.Name = "mnuInventory_Adjustments"
        resources.ApplyResources(Me.mnuInventory_Adjustments, "mnuInventory_Adjustments")
        '
        'mnuInventory_CycleCounts
        '
        Me.mnuInventory_CycleCounts.Name = "mnuInventory_CycleCounts"
        resources.ApplyResources(Me.mnuInventory_CycleCounts, "mnuInventory_CycleCounts")
        '
        'mnuInventory_InventoryCosting
        '
        Me.mnuInventory_InventoryCosting.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuInventory_InventoryCosting_AverageCostAdjustment, Me.mnuInventory_InventoryCosting_AverageCostUpdate})
        Me.mnuInventory_InventoryCosting.Name = "mnuInventory_InventoryCosting"
        resources.ApplyResources(Me.mnuInventory_InventoryCosting, "mnuInventory_InventoryCosting")
        '
        'mnuInventory_InventoryCosting_AverageCostAdjustment
        '
        Me.mnuInventory_InventoryCosting_AverageCostAdjustment.Name = "mnuInventory_InventoryCosting_AverageCostAdjustment"
        resources.ApplyResources(Me.mnuInventory_InventoryCosting_AverageCostAdjustment, "mnuInventory_InventoryCosting_AverageCostAdjustment")
        '
        'mnuInventory_InventoryCosting_AverageCostUpdate
        '
        Me.mnuInventory_InventoryCosting_AverageCostUpdate.Name = "mnuInventory_InventoryCosting_AverageCostUpdate"
        resources.ApplyResources(Me.mnuInventory_InventoryCosting_AverageCostUpdate, "mnuInventory_InventoryCosting_AverageCostUpdate")
        '
        'mnuInventory_Locations
        '
        Me.mnuInventory_Locations.Name = "mnuInventory_Locations"
        resources.ApplyResources(Me.mnuInventory_Locations, "mnuInventory_Locations")
        '
        'mnuInventory_ShrinkCorrections
        '
        Me.mnuInventory_ShrinkCorrections.Name = "mnuInventory_ShrinkCorrections"
        resources.ApplyResources(Me.mnuInventory_ShrinkCorrections, "mnuInventory_ShrinkCorrections")
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTools_TGM, Me.mnuTools_VendorList, Me.mnuTools_UnitConverter})
        Me.mnuTools.Name = "mnuTools"
        resources.ApplyResources(Me.mnuTools, "mnuTools")
        '
        'mnuTools_TGM
        '
        Me.mnuTools_TGM.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTools_TGM_LoadTGMView, Me.mnuTools_TGM_NewTGMView})
        Me.mnuTools_TGM.Name = "mnuTools_TGM"
        resources.ApplyResources(Me.mnuTools_TGM, "mnuTools_TGM")
        '
        'mnuTools_TGM_LoadTGMView
        '
        Me.mnuTools_TGM_LoadTGMView.Name = "mnuTools_TGM_LoadTGMView"
        resources.ApplyResources(Me.mnuTools_TGM_LoadTGMView, "mnuTools_TGM_LoadTGMView")
        '
        'mnuTools_TGM_NewTGMView
        '
        Me.mnuTools_TGM_NewTGMView.Name = "mnuTools_TGM_NewTGMView"
        resources.ApplyResources(Me.mnuTools_TGM_NewTGMView, "mnuTools_TGM_NewTGMView")
        '
        'mnuTools_VendorList
        '
        Me.mnuTools_VendorList.Name = "mnuTools_VendorList"
        resources.ApplyResources(Me.mnuTools_VendorList, "mnuTools_VendorList")
        '
        'mnuTools_UnitConverter
        '
        resources.ApplyResources(Me.mnuTools_UnitConverter, "mnuTools_UnitConverter")
        Me.mnuTools_UnitConverter.Name = "mnuTools_UnitConverter"
        '
        'mnuData
        '
        Me.mnuData.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuData_BatchRollback, Me.IconItemRefreshMenuItem, Me.R10ItemRefreshToolStripMenuItem, Me.mnuData_RestoreDeletedItem, Me.mnuData_ScalePOSPush, Me.SlawItemRefreshMenuItem, Me.mnuData_UnprocessedPushFiles})
        Me.mnuData.Name = "mnuData"
        resources.ApplyResources(Me.mnuData, "mnuData")
        '
        'mnuData_BatchRollback
        '
        Me.mnuData_BatchRollback.Name = "mnuData_BatchRollback"
        resources.ApplyResources(Me.mnuData_BatchRollback, "mnuData_BatchRollback")
        '
        'IconItemRefreshMenuItem
        '
        Me.IconItemRefreshMenuItem.Name = "IconItemRefreshMenuItem"
        resources.ApplyResources(Me.IconItemRefreshMenuItem, "IconItemRefreshMenuItem")
        '
        'R10ItemRefreshToolStripMenuItem
        '
        Me.R10ItemRefreshToolStripMenuItem.Name = "R10ItemRefreshToolStripMenuItem"
        resources.ApplyResources(Me.R10ItemRefreshToolStripMenuItem, "R10ItemRefreshToolStripMenuItem")
        '
        'mnuData_RestoreDeletedItem
        '
        Me.mnuData_RestoreDeletedItem.Name = "mnuData_RestoreDeletedItem"
        resources.ApplyResources(Me.mnuData_RestoreDeletedItem, "mnuData_RestoreDeletedItem")
        '
        'mnuData_ScalePOSPush
        '
        Me.mnuData_ScalePOSPush.Name = "mnuData_ScalePOSPush"
        resources.ApplyResources(Me.mnuData_ScalePOSPush, "mnuData_ScalePOSPush")
        '
        'SlawItemRefreshMenuItem
        '
        Me.SlawItemRefreshMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SlawItemLocaleMenuItem, Me.SlawPriceMenuItem})
        Me.SlawItemRefreshMenuItem.Name = "SlawItemRefreshMenuItem"
        resources.ApplyResources(Me.SlawItemRefreshMenuItem, "SlawItemRefreshMenuItem")
        '
        'SlawItemLocaleMenuItem
        '
        Me.SlawItemLocaleMenuItem.Name = "SlawItemLocaleMenuItem"
        resources.ApplyResources(Me.SlawItemLocaleMenuItem, "SlawItemLocaleMenuItem")
        '
        'SlawPriceMenuItem
        '
        Me.SlawPriceMenuItem.Name = "SlawPriceMenuItem"
        resources.ApplyResources(Me.SlawPriceMenuItem, "SlawPriceMenuItem")
        '
        'mnuData_UnprocessedPushFiles
        '
        Me.mnuData_UnprocessedPushFiles.Name = "mnuData_UnprocessedPushFiles"
        resources.ApplyResources(Me.mnuData_UnprocessedPushFiles, "mnuData_UnprocessedPushFiles")
        '
        'mnuAdministration
        '
        Me.mnuAdministration.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdministration_IRMAConfiguration, Me.mnuAdministration_POSInterface, Me.mnuAdministration_ScheduledJobs, Me.mnuAdministration_Stores, Me.mnuAdministration_Users})
        Me.mnuAdministration.Name = "mnuAdministration"
        resources.ApplyResources(Me.mnuAdministration, "mnuAdministration")
        '
        'mnuAdministration_IRMAConfiguration
        '
        Me.mnuAdministration_IRMAConfiguration.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration, Me.mnuAdministration_IRMAConfiguration_SystemConfiguration})
        Me.mnuAdministration_IRMAConfiguration.Name = "mnuAdministration_IRMAConfiguration"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration, "mnuAdministration_IRMAConfiguration")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmentReasons, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementConfiguration, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingTolerance, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits, Me.LabelTypesToolStripMenuItem, Me.NoTagLogicToolStripMenuItem, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin, Me.PriceTypesToolStripMenuItem, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelationships, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance, Me.SubteamDiscountExceptionsToolStripMenuItem, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones})
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmentReasons
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmentReasons.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmen" &
    "tReasons"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmentReasons, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmen" &
        "tReasons")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeMana" &
    "gement"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeMana" &
        "gement")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementConfiguration
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementConfiguration.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementCon" &
    "figuration"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementConfiguration, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementCon" &
        "figuration")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentC" &
    "ode"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentC" &
        "ode")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingTolerance
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingTolerance.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingToler" &
    "ance"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingTolerance, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingToler" &
        "ance")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits")
        '
        'LabelTypesToolStripMenuItem
        '
        Me.LabelTypesToolStripMenuItem.Name = "LabelTypesToolStripMenuItem"
        resources.ApplyResources(Me.LabelTypesToolStripMenuItem, "LabelTypesToolStripMenuItem")
        '
        'NoTagLogicToolStripMenuItem
        '
        Me.NoTagLogicToolStripMenuItem.Name = "NoTagLogicToolStripMenuItem"
        resources.ApplyResources(Me.NoTagLogicToolStripMenuItem, "NoTagLogicToolStripMenuItem")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin")
        '
        'PriceTypesToolStripMenuItem
        '
        Me.PriceTypesToolStripMenuItem.Name = "PriceTypesToolStripMenuItem"
        resources.ApplyResources(Me.PriceTypesToolStripMenuItem, "PriceTypesToolStripMenuItem")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_EatBy, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_ExtraText, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Grade, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelFormat, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelStyle, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelType, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_NutriFact, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_RandomWeightType, Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tare})
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_EatBy
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_EatBy.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Eat" &
    "By"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_EatBy, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Eat" &
        "By")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_ExtraText
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_ExtraText.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Ext" &
    "raText"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_ExtraText, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Ext" &
        "raText")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Grade
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Grade.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Gra" &
    "de"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Grade, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Gra" &
        "de")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelFormat
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelFormat.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Lab" &
    "elFormat"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelFormat, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Lab" &
        "elFormat")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelStyle
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelStyle.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Lab" &
    "elStyle"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelStyle, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Lab" &
        "elStyle")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelType
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelType.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Lab" &
    "elType"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelType, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Lab" &
        "elType")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_NutriFact
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_NutriFact.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Nut" &
    "riFact"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_NutriFact, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Nut" &
        "riFact")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_RandomWeightType
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_RandomWeightType.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Ran" &
    "domWeightType"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_RandomWeightType, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Ran" &
        "domWeightType")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tare
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tare.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tar" &
    "e"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tare, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tar" &
        "e")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelationships
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelationships.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelation" &
    "ships"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelationships, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelation" &
        "ships")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance")
        '
        'SubteamDiscountExceptionsToolStripMenuItem
        '
        Me.SubteamDiscountExceptionsToolStripMenuItem.Name = "SubteamDiscountExceptionsToolStripMenuItem"
        resources.ApplyResources(Me.SubteamDiscountExceptionsToolStripMenuItem, "SubteamDiscountExceptionsToolStripMenuItem")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup")
        '
        'mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones
        '
        Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones.Name = "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones, "mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones")
        '
        'mnuAdministration_IRMAConfiguration_SystemConfiguration
        '
        Me.mnuAdministration_IRMAConfiguration_SystemConfiguration.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdministration_SystemConfiguration_BuildConfiguration, Me.mnuAdministration_SystemConfiguration_DataArchiving, Me.mnuAdministration_SystemConfiguration_InstanceData, Me.mnuAdministration_SystemConfiguration_InstanceDataFlags, Me.mnuAdministration_SystemConfiguration_ManageConfiguration, Me.mnuAdministration_SystemConfiguration_ResolutionCodes, Me.mnuAdministration_SystemConfiguration_ReasonCodeMaintenance, Me.mnuAdministration_SystemConfiguration_MenuAccess, Me.StaticStoreFTPToolStripMenuItem, Me.mnuAdministration_SystemConfiguration_ManageRetentionPolicies})
        Me.mnuAdministration_IRMAConfiguration_SystemConfiguration.Name = "mnuAdministration_IRMAConfiguration_SystemConfiguration"
        resources.ApplyResources(Me.mnuAdministration_IRMAConfiguration_SystemConfiguration, "mnuAdministration_IRMAConfiguration_SystemConfiguration")
        '
        'mnuAdministration_SystemConfiguration_BuildConfiguration
        '
        Me.mnuAdministration_SystemConfiguration_BuildConfiguration.Name = "mnuAdministration_SystemConfiguration_BuildConfiguration"
        resources.ApplyResources(Me.mnuAdministration_SystemConfiguration_BuildConfiguration, "mnuAdministration_SystemConfiguration_BuildConfiguration")
        '
        'mnuAdministration_SystemConfiguration_DataArchiving
        '
        Me.mnuAdministration_SystemConfiguration_DataArchiving.Name = "mnuAdministration_SystemConfiguration_DataArchiving"
        resources.ApplyResources(Me.mnuAdministration_SystemConfiguration_DataArchiving, "mnuAdministration_SystemConfiguration_DataArchiving")
        '
        'mnuAdministration_SystemConfiguration_InstanceData
        '
        Me.mnuAdministration_SystemConfiguration_InstanceData.Name = "mnuAdministration_SystemConfiguration_InstanceData"
        resources.ApplyResources(Me.mnuAdministration_SystemConfiguration_InstanceData, "mnuAdministration_SystemConfiguration_InstanceData")
        '
        'mnuAdministration_SystemConfiguration_InstanceDataFlags
        '
        Me.mnuAdministration_SystemConfiguration_InstanceDataFlags.Name = "mnuAdministration_SystemConfiguration_InstanceDataFlags"
        resources.ApplyResources(Me.mnuAdministration_SystemConfiguration_InstanceDataFlags, "mnuAdministration_SystemConfiguration_InstanceDataFlags")
        '
        'mnuAdministration_SystemConfiguration_ManageConfiguration
        '
        Me.mnuAdministration_SystemConfiguration_ManageConfiguration.Name = "mnuAdministration_SystemConfiguration_ManageConfiguration"
        resources.ApplyResources(Me.mnuAdministration_SystemConfiguration_ManageConfiguration, "mnuAdministration_SystemConfiguration_ManageConfiguration")
        '
        'mnuAdministration_SystemConfiguration_ResolutionCodes
        '
        Me.mnuAdministration_SystemConfiguration_ResolutionCodes.Name = "mnuAdministration_SystemConfiguration_ResolutionCodes"
        resources.ApplyResources(Me.mnuAdministration_SystemConfiguration_ResolutionCodes, "mnuAdministration_SystemConfiguration_ResolutionCodes")
        '
        'mnuAdministration_SystemConfiguration_ReasonCodeMaintenance
        '
        Me.mnuAdministration_SystemConfiguration_ReasonCodeMaintenance.Name = "mnuAdministration_SystemConfiguration_ReasonCodeMaintenance"
        resources.ApplyResources(Me.mnuAdministration_SystemConfiguration_ReasonCodeMaintenance, "mnuAdministration_SystemConfiguration_ReasonCodeMaintenance")
        '
        'mnuAdministration_SystemConfiguration_MenuAccess
        '
        Me.mnuAdministration_SystemConfiguration_MenuAccess.Name = "mnuAdministration_SystemConfiguration_MenuAccess"
        resources.ApplyResources(Me.mnuAdministration_SystemConfiguration_MenuAccess, "mnuAdministration_SystemConfiguration_MenuAccess")
        '
        'StaticStoreFTPToolStripMenuItem
        '
        Me.StaticStoreFTPToolStripMenuItem.Name = "StaticStoreFTPToolStripMenuItem"
        resources.ApplyResources(Me.StaticStoreFTPToolStripMenuItem, "StaticStoreFTPToolStripMenuItem")
        '
        'mnuAdministration_SystemConfiguration_ManageRetentionPolicies
        '
        Me.mnuAdministration_SystemConfiguration_ManageRetentionPolicies.Name = "mnuAdministration_SystemConfiguration_ManageRetentionPolicies"
        resources.ApplyResources(Me.mnuAdministration_SystemConfiguration_ManageRetentionPolicies, "mnuAdministration_SystemConfiguration_ManageRetentionPolicies")
        '
        'mnuAdministration_POSInterface
        '
        Me.mnuAdministration_POSInterface.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdministration_POSInterface_FileWriter, Me.mnuAdministration_POSInterface_PricingMethods, Me.mnuAdministration_POSInterface_StoreFTPConfiguration})
        Me.mnuAdministration_POSInterface.Name = "mnuAdministration_POSInterface"
        resources.ApplyResources(Me.mnuAdministration_POSInterface, "mnuAdministration_POSInterface")
        '
        'mnuAdministration_POSInterface_FileWriter
        '
        Me.mnuAdministration_POSInterface_FileWriter.Name = "mnuAdministration_POSInterface_FileWriter"
        resources.ApplyResources(Me.mnuAdministration_POSInterface_FileWriter, "mnuAdministration_POSInterface_FileWriter")
        '
        'mnuAdministration_POSInterface_PricingMethods
        '
        Me.mnuAdministration_POSInterface_PricingMethods.Name = "mnuAdministration_POSInterface_PricingMethods"
        resources.ApplyResources(Me.mnuAdministration_POSInterface_PricingMethods, "mnuAdministration_POSInterface_PricingMethods")
        '
        'mnuAdministration_POSInterface_StoreFTPConfiguration
        '
        Me.mnuAdministration_POSInterface_StoreFTPConfiguration.Name = "mnuAdministration_POSInterface_StoreFTPConfiguration"
        resources.ApplyResources(Me.mnuAdministration_POSInterface_StoreFTPConfiguration, "mnuAdministration_POSInterface_StoreFTPConfiguration")
        '
        'mnuAdministration_ScheduledJobs
        '
        Me.mnuAdministration_ScheduledJobs.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdministration_ScheduledJobs_APUpload, Me.mnuAdministration_ScheduledJobs_AuditExceptionsReport, Me.mnuAdministration_ScheduledJobs_AverageCostUpdate, Me.mnuAdministration_ScheduledJobs_CloseReceiving, Me.mnuAdministration_ScheduledJobs_PLUMHost, Me.mnuAdministration_ScheduledJobs_POSPull, Me.mnuAdministration_ScheduledJobs_SendOrders, Me.mnuAdministration_ScheduledJobs_TlogProcessing, Me.mnuAdministration_ScheduledJobs_ViewAppLogs, Me.mnuAdministration_ScheduledJobs_WeeklySalesRollup})
        Me.mnuAdministration_ScheduledJobs.Name = "mnuAdministration_ScheduledJobs"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs, "mnuAdministration_ScheduledJobs")
        '
        'mnuAdministration_ScheduledJobs_APUpload
        '
        Me.mnuAdministration_ScheduledJobs_APUpload.Name = "mnuAdministration_ScheduledJobs_APUpload"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_APUpload, "mnuAdministration_ScheduledJobs_APUpload")
        '
        'mnuAdministration_ScheduledJobs_AuditExceptionsReport
        '
        Me.mnuAdministration_ScheduledJobs_AuditExceptionsReport.Name = "mnuAdministration_ScheduledJobs_AuditExceptionsReport"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_AuditExceptionsReport, "mnuAdministration_ScheduledJobs_AuditExceptionsReport")
        '
        'mnuAdministration_ScheduledJobs_AverageCostUpdate
        '
        Me.mnuAdministration_ScheduledJobs_AverageCostUpdate.Name = "mnuAdministration_ScheduledJobs_AverageCostUpdate"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_AverageCostUpdate, "mnuAdministration_ScheduledJobs_AverageCostUpdate")
        '
        'mnuAdministration_ScheduledJobs_CloseReceiving
        '
        Me.mnuAdministration_ScheduledJobs_CloseReceiving.Name = "mnuAdministration_ScheduledJobs_CloseReceiving"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_CloseReceiving, "mnuAdministration_ScheduledJobs_CloseReceiving")
        '
        'mnuAdministration_ScheduledJobs_PLUMHost
        '
        Me.mnuAdministration_ScheduledJobs_PLUMHost.Name = "mnuAdministration_ScheduledJobs_PLUMHost"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_PLUMHost, "mnuAdministration_ScheduledJobs_PLUMHost")
        '
        'mnuAdministration_ScheduledJobs_POSPull
        '
        Me.mnuAdministration_ScheduledJobs_POSPull.Name = "mnuAdministration_ScheduledJobs_POSPull"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_POSPull, "mnuAdministration_ScheduledJobs_POSPull")
        '
        'mnuAdministration_ScheduledJobs_SendOrders
        '
        Me.mnuAdministration_ScheduledJobs_SendOrders.Name = "mnuAdministration_ScheduledJobs_SendOrders"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_SendOrders, "mnuAdministration_ScheduledJobs_SendOrders")
        '
        'mnuAdministration_ScheduledJobs_TlogProcessing
        '
        Me.mnuAdministration_ScheduledJobs_TlogProcessing.Name = "mnuAdministration_ScheduledJobs_TlogProcessing"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_TlogProcessing, "mnuAdministration_ScheduledJobs_TlogProcessing")
        '
        'mnuAdministration_ScheduledJobs_ViewAppLogs
        '
        Me.mnuAdministration_ScheduledJobs_ViewAppLogs.Name = "mnuAdministration_ScheduledJobs_ViewAppLogs"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_ViewAppLogs, "mnuAdministration_ScheduledJobs_ViewAppLogs")
        '
        'mnuAdministration_ScheduledJobs_WeeklySalesRollup
        '
        Me.mnuAdministration_ScheduledJobs_WeeklySalesRollup.Name = "mnuAdministration_ScheduledJobs_WeeklySalesRollup"
        resources.ApplyResources(Me.mnuAdministration_ScheduledJobs_WeeklySalesRollup, "mnuAdministration_ScheduledJobs_WeeklySalesRollup")
        '
        'mnuAdministration_Stores
        '
        Me.mnuAdministration_Stores.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdministration_Stores_CreateStore, Me.mnuAdministration_Stores_BuildPOSFile, Me.mnuAdministration_Stores_BuildScaleFile, Me.mnuAdministration_Stores_BuildESTFile, Me.mnuAdministration_Stores_SendStoreToMammoth})
        Me.mnuAdministration_Stores.Name = "mnuAdministration_Stores"
        resources.ApplyResources(Me.mnuAdministration_Stores, "mnuAdministration_Stores")
        '
        'mnuAdministration_Stores_CreateStore
        '
        Me.mnuAdministration_Stores_CreateStore.Name = "mnuAdministration_Stores_CreateStore"
        resources.ApplyResources(Me.mnuAdministration_Stores_CreateStore, "mnuAdministration_Stores_CreateStore")
        '
        'mnuAdministration_Stores_BuildPOSFile
        '
        Me.mnuAdministration_Stores_BuildPOSFile.Name = "mnuAdministration_Stores_BuildPOSFile"
        resources.ApplyResources(Me.mnuAdministration_Stores_BuildPOSFile, "mnuAdministration_Stores_BuildPOSFile")
        '
        'mnuAdministration_Stores_BuildScaleFile
        '
        Me.mnuAdministration_Stores_BuildScaleFile.Name = "mnuAdministration_Stores_BuildScaleFile"
        resources.ApplyResources(Me.mnuAdministration_Stores_BuildScaleFile, "mnuAdministration_Stores_BuildScaleFile")
        '
        'mnuAdministration_Stores_BuildESTFile
        '
        Me.mnuAdministration_Stores_BuildESTFile.Name = "mnuAdministration_Stores_BuildESTFile"
        resources.ApplyResources(Me.mnuAdministration_Stores_BuildESTFile, "mnuAdministration_Stores_BuildESTFile")
        '
        'mnuAdministration_Stores_SendStoreToMammoth
        '
        Me.mnuAdministration_Stores_SendStoreToMammoth.Name = "mnuAdministration_Stores_SendStoreToMammoth"
        resources.ApplyResources(Me.mnuAdministration_Stores_SendStoreToMammoth, "mnuAdministration_Stores_SendStoreToMammoth")
        '
        'mnuAdministration_Users
        '
        Me.mnuAdministration_Users.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdministration_Users_ManageTitles, Me.mnuAdministration_Users_ManageUsers})
        Me.mnuAdministration_Users.Name = "mnuAdministration_Users"
        resources.ApplyResources(Me.mnuAdministration_Users, "mnuAdministration_Users")
        '
        'mnuAdministration_Users_ManageTitles
        '
        Me.mnuAdministration_Users_ManageTitles.Name = "mnuAdministration_Users_ManageTitles"
        resources.ApplyResources(Me.mnuAdministration_Users_ManageTitles, "mnuAdministration_Users_ManageTitles")
        '
        'mnuAdministration_Users_ManageUsers
        '
        Me.mnuAdministration_Users_ManageUsers.Name = "mnuAdministration_Users_ManageUsers"
        resources.ApplyResources(Me.mnuAdministration_Users_ManageUsers, "mnuAdministration_Users_ManageUsers")
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelp_About, Me.ViewClientLogFileToolStripMenuItem})
        Me.mnuHelp.Name = "mnuHelp"
        resources.ApplyResources(Me.mnuHelp, "mnuHelp")
        '
        'mnuHelp_About
        '
        Me.mnuHelp_About.Name = "mnuHelp_About"
        resources.ApplyResources(Me.mnuHelp_About, "mnuHelp_About")
        '
        'ViewClientLogFileToolStripMenuItem
        '
        Me.ViewClientLogFileToolStripMenuItem.Name = "ViewClientLogFileToolStripMenuItem"
        resources.ApplyResources(Me.ViewClientLogFileToolStripMenuItem, "ViewClientLogFileToolStripMenuItem")
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel_Region, Me.ToolStripStatusLabel_Environment, Me.ToolStripStatusLabel_Version, Me.ToolStripStatusLabelRegionalSetting})
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Name = "StatusStrip1"
        '
        'ToolStripStatusLabel_Region
        '
        Me.ToolStripStatusLabel_Region.Name = "ToolStripStatusLabel_Region"
        resources.ApplyResources(Me.ToolStripStatusLabel_Region, "ToolStripStatusLabel_Region")
        '
        'ToolStripStatusLabel_Environment
        '
        Me.ToolStripStatusLabel_Environment.Name = "ToolStripStatusLabel_Environment"
        resources.ApplyResources(Me.ToolStripStatusLabel_Environment, "ToolStripStatusLabel_Environment")
        '
        'ToolStripStatusLabel_Version
        '
        Me.ToolStripStatusLabel_Version.Name = "ToolStripStatusLabel_Version"
        resources.ApplyResources(Me.ToolStripStatusLabel_Version, "ToolStripStatusLabel_Version")
        '
        'ToolStripStatusLabelRegionalSetting
        '
        Me.ToolStripStatusLabelRegionalSetting.Name = "ToolStripStatusLabelRegionalSetting"
        resources.ApplyResources(Me.ToolStripStatusLabelRegionalSetting, "ToolStripStatusLabelRegionalSetting")
        '
        'frmMain
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MainMenu1)
        Me.Controls.Add(Me.ToolStrip_Inventory)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Name = "frmMain"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.pnlMainMessage.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip_MenuItems.ResumeLayout(False)
        Me.ToolStrip_MenuItems.PerformLayout()
        Me.ToolStrip_Inventory.ResumeLayout(False)
        Me.ToolStrip_Inventory.PerformLayout()
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mnuReports_Audit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Audit_AVCIExceptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_TaxHosting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_TaxHosting_TaxClassification As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_TaxHosting_TaxFlag As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_TaxHosting_TaxJurisdiction As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Pricing_PromotionalOffers As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel_Region As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel_Environment As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel_Version As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents mnuFile_Import_ItemMaintenanceBulkLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFile_Import_ItemStoreMaintenanceBulkLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFile_Import_ItemVendorMaintenanceBulkLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Item_BulkLoadAuditHistory As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_ReportManager As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFile_Import_ExtendedItemMaintenance As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFile_Import_Planogram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFile_Export_Planogram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Item_EditItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Item_Add As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ToolStrip_MenuItems As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbFile_Export As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsbFile_Import As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsbFile_Import_Order As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbFile_Import_RipeOrder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbFile_Import_ItemMaintenanceBulkLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbFile_Import_ItemStoreMaintenanceBulkLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbFile_Import_ItemVendorMaintenanceBulkLoad As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Miscellaneous As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Miscellaneous_CasesBySubTeamAudit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Miscellaneous_CasesBySubTeam As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Miscellaneous_LotNo As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_Miscellaneous_VendorEfficiency As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbFile_Export_Planogram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbFile_Import_EIM As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbFile_Import_Planogram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Item_ItemChain As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Pricing_PriceChangeWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_CompetitorStore As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_CompetitorStore_CompetitorPrices As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_CompetitorStore_NationalPurchasingValues As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_CompetitorStore_ImportCompetitorPrices As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_CompetitorStore_CompetitorTrend As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_CompetitorStore_CompetitorMarginImpact As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Inventory_BOHCompare As System.Windows.Forms.ToolStripMenuItem
    'Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    'Friend WithEvents mnuInventoryValue As System.Windows.Forms.ToolStripMenuItem
    'Friend WithEvents mnuStoreOrdersTotalBySKU As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Buyer_StoreOrdersTotalBySKU As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Inventory_InventoryValue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Accounting_PurchaseAccrualReport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Order_APUPAccrualsClosed As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Order_KitchenCaseTransfer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_ReceivingDistribution_AverageCost_DailyItemAverageCostChangeReport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Order_Short As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Tax As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Tax_ModifiedItemTaxClass As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Tax_NewItemTaxClass As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_3WayMatching As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_3WayMatching_ControlGroupLog As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_3WayMatching_ControlGroup3WayMatch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Orders_InvoiceEntry As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Orders_SuspendedPOTool As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Purchases As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Inventory_InventoryWeeklyHistory As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports_ReceivingDistribution_AverageCost_DASHandlingCharge As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_ReceivingDistribution_COOL_ShippingReport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_ReceivingDistribution_COOL_ReceivingReport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_ReceivingDistribution_COOL As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_ReceivingDistribution_DCStoreRetailPriceReport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_ReceivingDistribution_AverageCost_AverageCostHistoryVariance As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Orders_BatchReceiveClose As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip_Inventory As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbInventory_InventoryAdjustment As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuReports_Accounting_InvoiceDiscrepancies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports_Order_ClosedOrdersMissingInvoiceData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFile_ProcessMonitor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_Stores As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_Stores_CreateStore As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_Users As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_Users_ManageUsers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_Users_ManageTitles As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_Stores_BuildPOSFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_Stores_BuildScaleFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_POSInterface As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuData_BatchRollback As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuData_RestoreDeletedItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuData_ScalePOSPush As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_POSInterface_FileWriter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_POSInterface_StoreFTPConfiguration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_POSInterface_PricingMethods As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_SystemConfiguration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_TaxJurisdictions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Zones As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_SystemConfiguration_BuildConfiguration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_SystemConfiguration_InstanceData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_SystemConfiguration_InstanceDataFlags As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_SystemConfiguration_ManageConfiguration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_APUpload As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_AuditExceptionsReport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_AverageCostUpdate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_CloseReceiving As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_PLUMHost As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_POSPull As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_SendOrders As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_TlogProcessing As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_WeeklySalesRollup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemAttributes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Currency As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_SystemConfiguration_MenuAccess As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AttributeDefaults As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_AverageCostAdjustmentReasons As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_BrandName As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ClassName As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Conversion As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InventoryAdjustmentCode As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_OrderWindow As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Origin As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ShelfLife As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_StoreSubTeamRelationships As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_SubTeamMaintenance As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_Team As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneDistribution As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ZoneMarkup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_EatBy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_ExtraText As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Grade As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelFormat As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelStyle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_LabelType As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_NutriFact As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_RandomWeightType As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ScaleMaintenance_Tare As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EditStore As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit_Orders_EInvoicing As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_EInvoicingElementConfiguration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuInventory_ShrinkCorrections As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_InvoiceMatchingTolerance As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFile_Export_RGIS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbEdit_Item_AddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbEdit_Item_EditExistingItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbEdit_Orders_AddEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbEdit_Pricing_Batches As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbEdit_Pricing_ReprintSigns As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbEdit_Pricing_PromotionalOffers As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbReports_ReportManager As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbEdit_Item_InventoryLevel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFile_Export_RGIS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_RoleConflicts As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_ScheduledJobs_ViewAppLogs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_SystemConfiguration_DataArchiving As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_ItemUnits As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_SystemConfiguration_ResolutionCodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuData_UnprocessedPushFiles As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_SystemConfiguration_ReasonCodeMaintenance As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbEdit_SearchText As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tsbEdit_Search As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsbEdit_Search_PO As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbEdit_Search_Identifier As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StaticStoreFTPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SubteamDiscountExceptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PriceTypesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LabelTypesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_Stores_BuildESTFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdministration_IRMAConfiguration_ApplicationConfiguration_DefaultAttributeManagement As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PartialShippmentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTools_UnitConverter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabelRegionalSetting As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents mnuEdit_Orders_Search As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbEdit_OrderSearch As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbOrchard As System.Windows.Forms.ToolStripButton
    Friend WithEvents IconItemRefreshMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlMainMessage As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents NoTagLogicToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewClientLogFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents mnuAdministration_Stores_SendStoreToMammoth As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents R10ItemRefreshToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SlawItemRefreshMenuItem As ToolStripMenuItem
    Friend WithEvents SlawItemLocaleMenuItem As ToolStripMenuItem
    Friend WithEvents SlawPriceMenuItem As ToolStripMenuItem
    Friend WithEvents mnuAdministration_SystemConfiguration_ManageRetentionPolicies As ToolStripMenuItem
#End Region
End Class
