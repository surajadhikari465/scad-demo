Option Strict Off
Option Explicit On

Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess

Module Global_Renamed
    ' Define the log4net logger for this class.
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public COLOR_ACTIVE As Color = SystemColors.Window
    Public COLOR_INACTIVE As Color = SystemColors.ControlLight

    Public Enum enumOrderType
        Purchase = 1
        Distribution = 2
        Transfer = 3
        Flowthru = 4
    End Enum

    Public Enum enumProductType
        Product = 1
        PackagingSupplies = 2
        OtherSupplies = 3
    End Enum

    Public Enum enumSubTeamType
        All = 1
        Store = 2
        Supplier = 3
        StoreMinusSupplier = 4
        StoreUser = 5
        Retail = 6
        StoreByVendorID = 7
        SupplierRetail = 9
        SupplierByVendor = 10
        Packaging = 11
        Supplies = 12
    End Enum

    Public Enum enumSubTeamContext
        OrderHeader_From
        OrderHeader_To
        Item_From
    End Enum

    Public Enum enumVendorType
        RegionalRetailWFM = 0
        RegionalRetailMega = 1
        RegionalFacilityDistCenter = 2
        RegionalFacilityManufacturer = 3
        OutSideRegionalWFM = 4
    End Enum

    Public Enum enumReasonCodeType
        'Task 2095 v 4.3: Adding ReasonCode Types Enumerations, an ENUM value should be added when a new
        'Reason Code Type needs to be defined. Once an enum is added here the LoadReasonCodes() or LoadReasonCodesUltraCombo() global
        'methods can be used throughout IRMA to retrieve ReasonCode Details associated with this Type- 
        'The ultraCombo method provides two columns on dropdown - code and description, the regular combobox method provides only code.
        'Format e.g.: LoadReasonCodes(cmbCostAdjustmentReason, enumReasonCodeType.CA)
        'Format e.g.: LoadReasonCodesUltraCombo(ucCostAdjustmentReason, enumReasonCodeType.CA)

        CA 'Cost Adjustment
        RR 'Receiving Refusal
        RD 'Receiving Discrepancy
        DR 'Discount Reason
        SP 'Suspended PO Resolutions
        DP 'Deleted PO Reason
        RI 'Refused Items
    End Enum

    Public Enum enumReturnState
        Active = -1
        Cancel = 0
        Apply = 1
    End Enum

    Public Enum enumAdminFormState
        Add = 0
        Edit = 1
    End Enum

    Public Enum enumShelfTagSortOrder
        None = 0
        Subteam = 1
        Identifier = 2
        Description = 3
        InsertDate = 4
        Brand = 5
    End Enum

    Private Enum enumTextByID
        BrandName
        CategoryName
        SignName
        StoreName
        SubTeamMargin
        SubTeamName
        UserFullName
        UserName
        VendorName
    End Enum

    Public Enum enumChkBoxValues
        CheckedDisabled = -1
        CheckedEnabled = -2
        UncheckedDisabled = 0
        UncheckedEnabled = 1
    End Enum

    ' These enums come from the OrderRefreshCostSource table used for tracking where UpdateOrderRefreshCost is called
    ' The only ones called by VB.NET code is OrderItemForm, ReceivingListForm, and SuspendedPOTool, all are included for transparency
    Public Enum enumRefreshCostSource
        ApplyNewVendorCost
        EInvoiceMatching
        UpdateOrderApproved
        UpdateOrderClosed
        OrderItemForm
        UpdateOrderSend
        ReceivingListForm
        SuspendedPOTool
    End Enum

    Public Enum TemperatureUnit
        Fahrenheit
        Celsius
    End Enum

    Structure tItemUnit
        Dim Unit_ID As Short
        Dim Weight_Unit As Boolean
        Dim IsPackageUnit As Boolean
        Dim SystemCode As String
    End Structure

    Structure VendorQuantityType
        Dim Vendor_ID As Integer
        Dim Quantity As Decimal
        Dim OrderItem_ID As Integer
    End Structure

    Structure TGMToolType
        Dim SubTeam_No As Integer
        Dim StartDate As Date
        Dim EndDate As Date
        Dim Discontinued As Byte
        Dim HIAH As Byte
        'given the marshaling rules for ByValTStr, which specifies that the size of the string buffer that will be marshaled must include a Null termination character.
        'therefore  The SizeConst param must be FixedString size + 1, to include the extra null terminator.
        <VBFixedString(10), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=11)> Public Query() As Char
        Dim value As Integer
        Dim FileName As String
    End Structure

    Structure TGMCalculateType
        'given the marshaling rules for ByValTStr, which specifies that the size of the string buffer that will be marshaled must include a Null termination character.
        'therefore  The SizeConst param must be FixedString size + 1, to include the extra null terminator.
        <VBFixedString(18), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=19)> Public Identifier() As Char
        <VBFixedString(60), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=61)> Public Item_Description() As Char
        Dim Retail As Decimal
        Dim Cost As Decimal
        Dim Margin As Decimal
    End Structure

    Public CurrencyCultureMapping As New Dictionary(Of String, String)
    Public PrimaryCurrency As New List(Of String)

    '-- General Constants
    Public Const bProfessional As Boolean = False

    '-- Regional Constants
    Public gsRegionName As String
    Public gsRegionCode As String
    Public gsPluDigitsSentToScale As String 'POSSIBLE VALUES: "ALWAYS 4", "ALWAYS 5", "VARIABLE BY ITEM"
    '20100215 - Dave Stacey - Add Culture to UltraGrid via config lookup value
    Public gsUG_Culture As String
    Public gsUG_CultureDisplayName As String
    Public gsUG_DateMask As String

    '-- Databound Constants
    Public Const LOCAL_DB As String = "INVENTORY.MDB"
    Public gsLocalDbDirectory As String = Application.StartupPath & "\Reports"
    Public glData_DefaultType As DAO.WorkspaceTypeEnum = DAO.WorkspaceTypeEnum.dbUseJet
    Public Const glWasteAdjustment As Short = 1
    Public Const glCountAdjustment As Short = 2
    Public Const glSalesAdjustment As Short = 3
    Public Const glReceivingAdjustment As Short = 5
    Public Const glDistributionAdjustment As Short = 6
    Public Const glTransferAdjustment As Short = 7
    Public Const glManualAdjustment As Short = 8

    '-- Search Constants
    Public Const iSearchVendorCompany As Short = 0
    Public Const iSearchContactContact As Short = 1
    Public Const iSearchUnit As Short = 2
    Public Const iSearchBrand As Short = 3
    Public Const iSearchCategory As Short = 4
    Public Const iSearchShelfLife As Short = 6
    Public Const iSearchOrigin As Short = 7
    Public Const isearchFromUnit As Short = 8
    Public Const iSearchToUnit As Short = 9
    Public Const iSearchOrganizationCompany As Short = 12
    Public Const iSearchFSCustomer As Short = 13
    Public Const iSearchAllVendors As Short = 14

    '-- Organization constants
    Public Const iOrganizationName As Short = 0
    Public Const iOrganizationAddress_Line_1 As Short = 1
    Public Const iOrganizationAddress_Line_2 As Short = 2
    Public Const iOrganizationCity As Short = 3
    Public Const iOrganizationState As Short = 4
    Public Const iOrganizationZip_Code As Short = 5
    Public Const iOrganizationPhone As Short = 6
    Public Const iOrganizationPhone_Ext As Short = 7
    Public Const iOrganizationFax As Short = 8
    Public Const iOrganizationContact As Short = 9
    Public Const iOrganizationEmail_Address As Short = 10
    Public Const iOrganizationComment As Short = 11

    Public Const iOrganizationActive As Short = 0

    '-- FS Customer constants
    Public Const iCustomerOrganizationName As Short = 0
    Public Const iCustomerName As Short = 1
    Public Const iCustomerAddress_Line_1 As Short = 2
    Public Const iCustomerAddress_Line_2 As Short = 3
    Public Const iCustomerCity As Short = 4
    Public Const iCustomerState As Short = 5
    Public Const iCustomerZip_Code As Short = 6
    Public Const iCustomerPhone As Short = 7
    Public Const iCustomerPhone_Ext As Short = 8
    Public Const iCustomerFax As Short = 9
    Public Const iCustomerEmail_Address As Short = 10
    Public Const iCustomerBirthday As Short = 11
    Public Const iCustomerCode As Short = 12
    Public Const iCustomerComment As Short = 13

    Public Const iCustomerActive As Short = 0


    '-- Vendor constants
    Public Const iVendorStore_No As Short = 0

    Public Const iVendor_ID As Short = 0
    Public Const iVendorCompanyName As Short = 1
    Public Const iVendorAddress_Line_1 As Short = 2
    Public Const iVendorAddress_Line_2 As Short = 3
    Public Const iVendorCity As Short = 4
    Public Const iVendorState As Short = 5
    Public Const iVendorZip_Code As Short = 6
    Public Const iVendorCountry As Short = 7
    Public Const iVendorPhone As Short = 8
    Public Const iVendorFax As Short = 9
    Public Const iVendorComments As Short = 10
    Public Const iVendorVendor_Key As Short = 11
    Public Const iVendorPayTo_Fax As Short = 12
    Public Const iVendorPayTo_Phone As Short = 13
    Public Const iVendorPayTo_Country As Short = 14
    Public Const iVendorPayTo_Zip_Code As Short = 15
    Public Const iVendorPayTo_State As Short = 16
    Public Const iVendorPayTo_City As Short = 17
    Public Const iVendorPayTo_Address_Line_2 As Short = 18
    Public Const iVendorPayTo_Address_Line_1 As Short = 19
    Public Const iVendorPayTo_CompanyName As Short = 20
    Public Const iVendorPayTo_Attention As Short = 21
    Public Const iVendorPS_Location_Code As Short = 22
    Public Const iVendorPS_Vendor_ID As Short = 23
    Public Const iVendorPS_Address_Sequence As Short = 24
    Public Const iVendorPhone_Ext As Short = 25
    Public Const iVendorPayTo_Phone_Ext As Short = 26
    Public Const iVendorDefault_Account_No As Short = 27
    Public Const iVendorEmail As Short = 28
    Public Const iVendorOtherName As Short = 29
    Public Const iVendorPONote As Short = 30
    Public Const iVendorReceivingAuthNote As Short = 31
    Public Const iCaseDistHandlingCharge As Short = 32
    Public Const iDefaultPOTransmission As Short = 33

    Public Const iVendorCustomer As Short = 0
    Public Const iVendorInternalCustomer As Short = 1
    Public Const iVendorWFM As Short = 2
    Public Const iVendorEFT As Short = 3

    Public Const iVendorPOFaxTransmission As Short = 0
    Public Const iVendorPOEmailTransmission As Short = 1
    Public Const iVendorPOManualTransmission As Short = 2
    Public Const iVendorPOElectronicTransmission As Short = 3

    '-- Contact constants
    Public Const iContactContact_Name As Short = 2
    Public Const iContactPhone As Short = 3
    Public Const iContactFax As Short = 4
    Public Const iContactPhone_Ext As Short = 5

    '-- Unit constants
    Public Const iUnitUnit_Abbreviation As Short = 0
    Public Const iUnitUnit_Name As Short = 1

    Public Const iUnitWeight_Unit As Short = 0

    '-- Brand Name constants
    Public Const iBrandBrand_ID As Short = 0
    Public Const iBrandBrand_Name As Short = 1

    '-- Department constants
    Public Const iDepartmentDepartment_ID As Short = 0
    Public Const iDepartmentDepartment_Name As Short = 1

    '-- Category constants
    Public Const iCategoryCategory_ID As Short = 0
    Public Const iCategoryCategory_Name As Short = 1

    '-- Shelf Life constants
    Public Const iShelfLifeShelfLife_ID As Short = 0
    Public Const iShelfLifeShelfLife_Name As Short = 1

    '-- Origin Name constants
    Public Const iOriginOrigin_ID As Short = 0
    Public Const iOriginOrigin_Name As Short = 1

    '-- Order Header Constants
    Public Const iOrderHeaderPurchaseLocation_ID As Short = 1
    Public Const iOrderHeaderReceiveLocation_ID As Short = 0
    Public Const iOrderHeaderDiscountType As Short = 2

    'Public Const iOrderHeaderFax_Order As Short = 0
    Public Const iOrderHeaderReturn_Order As Short = 1

    Public Const iOrderHeaderOrderHeader_ID As Short = 0
    Public Const iOrderHeaderCompanyName As Short = 1
    Public Const iOrderHeaderCreatedBy As Short = 2
    Public Const iOrderHeaderOrderDate As Short = 3
    Public Const iOrderHeaderSentDate As Short = 4
    Public Const iOrderHeaderCloseDate As Short = 5
    Public Const iOrderHeaderExpected_Date As Short = 6
    Public Const iOrderHeaderQuantityDiscount As Short = 7
    'BS-090707-Removed control (at least by name) in order to show all const fields/controls at once (no toggle).  See TFS Task # 10320 & 10321.
    'Public Const iOrderHeaderTotalCost As Short = 8
    Public Const iOrderHeaderOrgPO As Short = 9
    Public Const iOrderHeaderTotalFreight As Short = 10
    Public Const iOrderHeaderTemperature As Short = 11
    Public Const iOrderHeaderInvoiceNumber As Short = 12
    Public Const iOrderHeaderRecvLog_No As Short = 13
    ' Added by Rick Kelleher 12/2007
    Public Const iOrderHeader3rdPartyFreight As Short = 16

    Public Const iOrderHeaderPOFaxTransmission As Short = 0
    Public Const iOrderHeaderPOEmailTransmission As Short = 1
    Public Const iOrderHeaderPOManualTransmission As Short = 2


    '-- Order Item Constants
    Public Const iOrderItemQuantityUnit As Short = 0
    Public Const iOrderItemCostUnit As Short = 1
    Public Const iOrderItemDiscountType As Short = 2
    Public Const iOrderItemHandlingUnit As Short = 3
    Public Const iOrderItemFreightUnit As Short = 4
    Public Const iOrderItemLandingUnit As Short = 5
    Public Const iOrderItemMarkupUnit As Short = 6
    Public Const iOrderItemRetail_Unit_ID As Short = 7
    Public Const iOrderItemPackage_Unit_ID As Short = 8
    Public Const iOrderItemOrigin_ID As Short = 9
    Public Const iOrderItemCreditReason As Short = 10
    Public Const iOrderItemCountryProc_ID As Short = 11

    Public Const iOrderItemItem_Description As Short = 0
    Public Const iOrderItemIdentifier As Short = 1
    Public Const iOrderItemUnits_Per_Pallet As Short = 2
    Public Const iOrderItemPackage_Desc1 As Short = 3
    Public Const iOrderItemPackage_Desc2 As Short = 4
    Public Const iOrderItemQuantityOrdered As Short = 5
    Public Const iOrderItemNumber_Of_Pallets As Short = 6
    Public Const iOrderItemCost As Short = 7
    Public Const iOrderItemQuantityDiscount As Short = 8
    Public Const iOrderItemHandling As Short = 9
    Public Const iOrderItemFreight As Short = 10
    Public Const iOrderItemLandingCost As Short = 11
    Public Const iOrderItemMarkupPercent As Short = 12
    Public Const iOrderItemMarkupCost As Short = 13
    Public Const iOrderItemLineItemCost As Short = 14
    Public Const iOrderItemLineItemHandling As Short = 15
    Public Const iOrderItemLineItemFreight As Short = 16
    Public Const iOrderItemQuantityReceived As Short = 17
    Public Const iOrderItemTotal_Weight As Short = 18
    Public Const iOrderItemDateReceived As Short = 19
    Public Const iOrderItemExpirationDate As Short = 20
    Public Const iOrderItemReceivedItemCost As Short = 21
    Public Const iOrderItemReceivedItemHandling As Short = 22
    Public Const iOrderItemReceivedItemFreight As Short = 23
    Public Const iOrderItemAdjustedCost As Short = 24
    Public Const iOrderItemQuantityAllocated As Short = 25
    Public Const iOrderItemLotNo As Short = 26
    Public Const iOrderItemCOOL As Short = 27
    Public Const iOrderItemBIO As Short = 28
    Public Const iOrderItemCarrier As Short = 29

    '-- Conversion constants
    Public Const iConversionFromUnit_ID As Short = 0
    Public Const iConversionToUnit_ID As Short = 1
    Public Const iConversionConversionSymbol As Short = 2
    Public Const iConversionConversionFactor As Short = 3

    '-- Item Constants
    Public Const iItemSales_Account As Short = 0
    Public Const iItemIdentifier As Short = 1
    Public Const iItemItem_Description As Short = 2
    Public Const iItemPOS_Description As Short = 3
    Public Const iItemPackage_Desc1 As Short = 4
    Public Const iItemPackage_Desc2 As Short = 5
    Public Const iItemUnits_Per_Pallet As Short = 6
    Public Const iItemShelfLife_Length As Short = 7 'field removed
    Public Const iItemMin_Temperature As Short = 8
    Public Const iItemMax_Temperature As Short = 9
    Public Const iItemSign_Description As Short = 10
    Public Const iItemAverage_Unit_Weight As Short = 11
    Public Const iItemHigh As Short = 12
    Public Const iItemTie As Short = 13
    Public Const iItemYield As Short = 14
    Public Const iItemIngredients As Short = 15 'field removed
    Public Const iItemScaleDesc1 As Short = 16 'field removed
    Public Const iItemScaleDesc2 As Short = 17 'field removed
    Public Const iItemNotAvailNote As Short = 18
    Public Const iItemPurchaseThresholdCouponAmount As Short = 19
    Public Const iItemCurrentHandlingCharge As Short = 20
    Public Const iItemHandlingChargeOverride As Short = 21

    'Public Const iItemDiscountable As Short = 0        ' moved to pos info screen
    'Public Const iItemFood_Stamps As Short = 1         ' moved to pos info screen
    'Public Const iItemPrice_Required As Short = 2      ' moved to pos info screen
    'Public Const iItemQuantity_Required As Short = 3   ' moved to pos info screen

    Public Const iNoDistMarkup As Short = 4
    Public Const iItemFull_Pallet_Only As Short = 6
    Public Const iItemKeep_Frozen As Short = 7
    Public Const iItemNot_Available As Short = 8
    Public Const iItemOrganic As Short = 9
    Public Const iItemPre_Order As Short = 10
    Public Const iItemRefrigerated As Short = 11
    Public Const iItemRetail_Sale As Short = 12
    Public Const iItemShipper_Item As Short = 13
    Public Const iItemWFM_Item As Short = 14
    Public Const iItemHFM As Short = 15
    Public Const iItemEXEDist As Short = 16
    Public Const iItemCostedByWeight As Short = 17
    Public Const iItemRecallFlag As Short = 18
    Public Const iItemLockAuthFlag As Short = 19
    Public Const iItemPurchaseThresholdCouponSubTeam As Short = 20
    Public Const iItemCatchweightRequired As Short = 21
    Public Const iItemCOOL As Short = 22
    Public Const iItemBIO As Short = 23
    Public Const iItemIngredient As Short = 24
    Public Const iItemNot_Available_365 As Short = 25

    '----- orig code ----------------------------------
    'Public Const iItemCategory_ID As Short = 0
    'Public Const iItemSubTeam_No As Short = 1
    Public Const iItemBrand_ID As Short = 2
    Public Const iItemOrigin_ID As Short = 3
    Public Const iItemWholeSale_Unit_ID As Short = 4
    Public Const iItemRetail_Unit_ID As Short = 5
    Public Const iItemPackage_Unit_ID As Short = 6
    Public Const iItemShelfLife_ID As Short = 7  'field removed
    Public Const iItemVendor_Unit_ID As Short = 8
    Public Const iItemDistribution_Unit_ID As Short = 9
    Public Const iItemCost_Unit_ID As Short = 10
    Public Const iitemFreight_Unit_ID As Short = 11
    Public Const iitemItemType_ID As Short = 12
    Public Const iItemCountryProc_ID As Short = 13
    Public Const iItemManufacturing_Unit_ID As Short = 14
    Public Const iItemNatClassID As Short = 15
    Public Const iItemManagedBy As Short = 16

    '-- Item Quantity Constants
    Public Const iItemQuantityStore_Name As Short = 0
    Public Const iItemQuantitySupply_Days As Short = 1
    Public Const iItemQuantityReorder_Point As Short = 2
    Public Const iItemQuantitySafety_Stock As Short = 3
    Public Const iItemQuantityLead_Days As Short = 4
    Public Const iItemQuantityFacing As Short = 5
    Public Const iItemQuantityDepth As Short = 6
    Public Const iItemQuantityStack As Short = 7
    Public Const iItemQuantityFootPrint As Short = 8

    Public Const iItemQuantityNo_MarkDown_Safety As Short = 0
    Public Const iItemQuantityNo_LateSale_Safety As Short = 1

    Public Const iItemQuantityReplenish_By_FootPrint As Short = 0

    '-- Price Constants
    Public Const iPriceMultiple As Short = 0
    Public Const iPricePrice As Short = 1
    Public Const iPriceStartDate As Short = 2
    Public Const iPriceEndDate As Short = 3
    Public Const iPriceEarnedDiscount1 As Short = 4
    Public Const iPriceEarnedDiscount2 As Short = 5
    Public Const iPriceEarnedDiscount3 As Short = 6
    Public Const iPriceMixMatch As Short = 7

    '-- Grid Constants
    Public Const giGridScrollBarWidth As Short = 38

    ' ASCII Constants
    Public Const ASCII_NULL As Short = 0
    Public Const ASCII_BACKSPACE As Short = 8
    Public Const ASCII_ENTER As Short = 13
    Public Const ASCII_UPPERCASE_A As Short = 65
    Public Const ASCII_LOWERCASE_A As Short = 97
    Public Const ASCII_LOWERCASE_Z As Short = 122
    Public Const ASCII_PIPE As Short = 124

    '-- Passing variables
    Public gsWindowsCulture As String
    Public glVendorID As Integer
    Public glOrganizationID As Integer
    Public glCustomerID As Integer
    Public gsVendorName As String
    Public glSubTeamNo As Integer
    Public glContactID As Integer
    Public glUnitID As Integer
    Public glBrandID As Integer
    Public glCategoryID As Integer
    Public glCategoryIDSubTeam_No As Integer
    Public glLevel3ID As Integer
    Public glLevel3IDCategoryID As Integer
    Public glLevel4ID As Integer
    Public glLevel4IDLevel3ID As Integer
    Public glDepartmentID As Integer
    Public glShelfLifeID As Integer
    Public glOriginID As Integer
    Public glItemID As Integer
    Public gsItemDescription As String
    Public glItemSubTeam As Integer
    Public glStoreID As Integer
    Public gsStoreName As String
    Public glStoreNo As Integer
    Public glPriceLevelID As Integer
    Public glSignID As Integer
    Public glOrderHeaderID As Integer
    Public glOrderItemID As Integer
    Public glFromUnitID, glToUnitID As Integer
    Public glDistributionID As Integer
    Public glCycleHeaderID As Integer
    Public glCycleDetailID As Integer
    Public glCycleImportHeaderID As Integer
    Public glCycleImportDetailID As Integer
    Public glCycleImportItemID As Integer
    Public gdCycleImportEndDate As Date
    Public gdCycleEndDate As Date
    Public glItemQuantity As Integer
    Public gdItemWeight As Decimal
    Public gsTag As String
    Public gsZone As String
    Public gsDate As Object
    Public gDBInventory As DAO.Database
    Public gRSRecordset As DAO.Recordset
    Public gDBReport As ADODB.Connection
    Public gJetFlush As JRO.JetEngine
    Public giSearchType As Short
    Public gbValidationBroke As Boolean
    Public gbClosedOrder As Boolean
    Public gbClosedCount As Boolean
    Public gbClosedImportCount As Boolean
    Public gbOrderManifested As Boolean
    Public gbWFMItem As Boolean
    Public glInstance As Integer
    Public glTGMTool As TGMToolType
    Public gtTGMCalculator As TGMCalculateType
    Public glTransfer_To_SubTeam As Integer
    Public gbExcludeNot_Available As Boolean
    Public geOrderType As enumOrderType
    Public geProductType As enumProductType
    Public gsPOCreatorUserName As String
    Public glAllowBarcodePOReport As Boolean

    Public glRepCycleHeaderID As Integer
    Public glRepCycleDetailID As Integer
    Public glRepCycleItemKey As Integer
    Public gbRepCycleClosed As Boolean

    Public gbQuickSearch As Boolean
    Public glIdentifier As String

    'Database
    Public gsODBCConnect As String
    Public gsCrystal_Connect As String
    Public gsCrystalUser As String     ' commented out on 7/6/06 because they aren't being set anywhere
    Public gsCrystalPassword As String     ' enabled again on 7/20/06 because I will be setting them
    Public gdbADOInventory As ADODB.Connection
    Public grsADORecordset As ADODB.Recordset
    Private mLastActive As Date

    Public gbDistribution_Center As Boolean ' determines if the vendor is a distribution center or not

    'Client Roles
    Public gbAccountant As Boolean
    Public gbBatchBuildOnly As Boolean
    Public gbBuyer As Boolean
    Public gbCoordinator As Boolean
    Public gbCostAdmin As Boolean
    Public gbFacilityCreditProcessor As Boolean
    Public gbDCAdmin As Boolean
    Public gbDeletePO As Boolean
    Public gbDistributor As Boolean
    Public gbEInvoicing As Boolean
    Public gbInventoryAdministrator As Boolean
    Public gbItemAdministrator As Boolean
    Public gbLockAdministrator As Boolean
    Public gbManufacturer As Boolean
    Public gbPOAccountant As Boolean
    Public gbPOApprovalAdmin As Boolean
    Public gbPOEditor As Boolean
    Public gbPriceBatchProcessor As Boolean
    Public gbTaxAdministrator As Boolean
    Public gbUserShrink As Boolean
    Public gbUserShrinkAdmin As Boolean
    Public gbVendorAdministrator As Boolean
    Public gbVendorCostDiscrepancyAdmin As Boolean
    Public gbWarehouse As Boolean
    Public gbReceiver As Boolean
    Public gbCancelAllSales As Boolean

    Public gbApplicationConfigurationAdministrator As Boolean
    Public gbDataAdministrator As Boolean
    Public gbJobAdministrator As Boolean
    Public gbPOSInterfaceAdministrator As Boolean
    Public gbSecurityAdministrator As Boolean
    Public gbStoreAdministrator As Boolean
    Public gbSystemConfigurationAdministrator As Boolean
    Public gbUserMaintenance As Boolean
    Public gbSuperUser As Boolean
    Public gb365Administrator As Boolean
    Public gbSupportUser As Boolean

    Public gsUserName As String
    Public giUserID As Integer
    Public gsTitleDescription As String

    Public glStore_Limit As Integer
    Public aWeight_Unit() As tItemUnit
    Public grsDistSubTeams As ADODB.Recordset
    Public grsUnitConversion As ADODB.Recordset
    Public glRecvLog_Store_Limit As Integer
    Public glVendor_Limit As Integer
    Public giShipper, giCase, giUnit, giBox, giPound, giBag As Short

    Public giDate As Short
    Public gWidth As Short
    Public giType As Short
    Public gBorder As Short
    Public glBatchHeaderID As Integer
    Public glBatchItemID As Integer
    Public glMaxRows As Integer

    Public GridWidth As Short

    Public StoreType As Short
    Public bEnableAllStoresSelection As Nullable(Of Boolean)

    Public sDiscountType() As String
    Public bSpecificOrder As Boolean
    Public bSpecificVendor As Boolean
    Public bSpecificInvoice As Boolean
    Public bSpecificStoreItemVendorCost As Boolean
    Public sVersion As String
    Public gsReportingServicesURL As String
    Public gsReportDirectory As String
    Public gbUseLocalTime As Boolean
    Public stopwatch As New System.Diagnostics.Stopwatch

    Private m_dtDatabaseDateTime As Date = System.DateTime.MinValue
    Private isReceivingInProgress As Boolean = False

    Public Function ConvertQuotes(ByRef sIn As String) As String
        Return sIn.Replace("'", "''")
    End Function

    Public Sub CopyDAORecordToADORecord(ByRef rsIn As dao.Recordset, ByRef rsOut As ADODB.Recordset, Optional ByRef bAllowNulls As Boolean = True)

        logger.Debug("CopyDAORecordToADORecord Entry")

        Dim dFld As dao.Field

        rsOut.AddNew()
        For Each dFld In rsIn.Fields
            If Not IsDBNull(dFld.Value) Then
                rsOut.Fields(dFld.Name).Value = dFld.Value
            Else
                If bAllowNulls Then rsOut.Fields(dFld.Name).Value = System.DBNull.Value
            End If
        Next dFld
        rsOut.Update()

        logger.Debug("CopyDAORecordToADORecord Exit")


    End Sub

    Public Sub CreateEmptyADORS_FromDAO(ByRef rsIn As dao.Recordset, ByRef rsOut As ADODB.Recordset, Optional ByRef bAllowNulls As Boolean = True)


        logger.Debug("CreateEmptyADORS_FromDAO Entry")

        Dim dFld As dao.Field

        If Not (rsOut Is Nothing) Then
            If rsOut.State = ADODB.ObjectStateEnum.adStateOpen Then
                rsOut.Close()
                rsOut = Nothing
            End If
        End If
        rsOut = New ADODB.Recordset

        For Each dFld In rsIn.Fields
            Select Case dFld.Type
                Case dao.DataTypeEnum.dbLong, dao.DataTypeEnum.dbInteger, dao.DataTypeEnum.dbByte
                    If bAllowNulls Then
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adInteger, , ADODB.FieldAttributeEnum.adFldIsNullable + ADODB.FieldAttributeEnum.adFldMayBeNull)
                    Else
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adInteger)
                    End If
                Case dao.DataTypeEnum.dbDate
                    If bAllowNulls Then
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adDate, , ADODB.FieldAttributeEnum.adFldIsNullable + ADODB.FieldAttributeEnum.adFldMayBeNull)
                    Else
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adDate)
                    End If
                Case dao.DataTypeEnum.dbChar, dao.DataTypeEnum.dbText
                    If bAllowNulls Then
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adVarChar, IIf(dFld.Size = 0, 1, dFld.Size), ADODB.FieldAttributeEnum.adFldIsNullable + ADODB.FieldAttributeEnum.adFldMayBeNull)
                    Else
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adVarChar, IIf(dFld.Size = 0, 1, dFld.Size))
                    End If
                Case dao.DataTypeEnum.dbDecimal
                    If bAllowNulls Then
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adDecimal, , ADODB.FieldAttributeEnum.adFldIsNullable + ADODB.FieldAttributeEnum.adFldMayBeNull)
                    Else
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adDecimal)
                    End If
                Case dao.DataTypeEnum.dbCurrency
                    If bAllowNulls Then
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adCurrency, , ADODB.FieldAttributeEnum.adFldIsNullable + ADODB.FieldAttributeEnum.adFldMayBeNull)
                    Else
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adCurrency)
                    End If
                Case dao.DataTypeEnum.dbBoolean
                    If bAllowNulls Then
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adBoolean, , ADODB.FieldAttributeEnum.adFldIsNullable + ADODB.FieldAttributeEnum.adFldMayBeNull)
                    Else
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adBoolean)
                    End If
                Case dao.DataTypeEnum.dbDouble
                    If bAllowNulls Then
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adDouble, , ADODB.FieldAttributeEnum.adFldIsNullable + ADODB.FieldAttributeEnum.adFldMayBeNull)
                    Else
                        rsOut.Fields.Append(dFld.Name, ADODB.DataTypeEnum.adDouble)
                    End If
                Case Else
                    Err.Raise(ERR_DBTYPE_CONVERT_TO_ADTYPE, , ERR_DBTYPE_CONVERT_TO_ADTYPE_DESC & dFld.Type)
            End Select
        Next dFld

        logger.Debug("CreateEmptyADORS_FromDAO Exit")

    End Sub

    Public Function DoubleApostrophe(ByRef sIn As String) As String

        logger.Debug("DoubleApostrophe Entry")


        Dim sOut As String
        Dim chr_Renamed As String
        Dim i As Integer
        sOut = String.Empty

        For i = 1 To Len(sIn)
            chr_Renamed = Mid(sIn, i, 1)
            If chr_Renamed = "'" Then
                sOut = sOut & "''"
            Else
                sOut = sOut & chr_Renamed
            End If
        Next i

        DoubleApostrophe = sOut

        logger.Debug("DoubleApostrophe Exit")

    End Function

    Public Function ItemAdminSubTeam(ByRef lItem_Key As Integer) As Boolean

        logger.Debug("ItemAdminSubTeam Entry")


        Dim bUserSubTeam As Boolean = False


        If gbSuperUser Then
            bUserSubTeam = True
        Else
            Try
                Dim paramList As New DBParamList()

                paramList.Add(New DBParam("User_ID", DBParamType.Int, giUserID))
                paramList.Add(New DBParam("SubTeam_No", DBParamType.Int, DBNull.Value))
                paramList.Add(New DBParam("Item_Key", DBParamType.Int, lItem_Key))

                'Determine if user has access to this item's subteam
                If GetDataReader("GetUsersSubTeam", paramList).HasRows Then
                    bUserSubTeam = True
                End If

                paramList = Nothing

            Catch ex As Exception
                logger.Error(ex.Message)
                Throw ex

            End Try
        End If

        logger.Debug("ItemAdminSubTeam Exit")

        Return bUserSubTeam

    End Function

    ''' <summary>
    ''' Generic private method to populate a combobox from a stored procedure
    ''' </summary>
    ''' <param name="cbo">Reference to System.Windows.Forms.ComboBox</param>
    ''' <param name="StoredProcedure">Name of the stored procedure</param>
    ''' <param name="DataTextField">Field name in the result set which contains the text to display</param>
    ''' <returns>True if successful; False when an exception occurs</returns>
    ''' <remarks></remarks>
    Private Function LoadCombo(ByRef cbo As System.Windows.Forms.ComboBox, ByVal StoredProcedure As String, ByVal DataTextField As String) As Boolean

        Return LoadCombo(cbo, StoredProcedure, DataTextField, Nothing, Nothing)

    End Function

    ''' <summary>
    ''' Generic private method to populate a combobox from a stored procedure
    ''' </summary>
    ''' <param name="cbo">Reference to System.Windows.Forms.ComboBox</param>
    ''' <param name="StoredProcedure">Name of the stored procedure</param>
    ''' <param name="DataTextField">Field name in the result set which contains the text to display</param>
    ''' <param name="DataValueField">Field name in the result set which contains the data value associated with the displayed text</param>
    ''' <returns>True if successful; False when an exception occurs</returns>
    ''' <remarks></remarks>
    Private Function LoadCombo(ByRef cbo As System.Windows.Forms.ComboBox, ByVal StoredProcedure As String, ByVal DataTextField As String, ByVal DataValueField As String) As Boolean

        Return LoadCombo(cbo, StoredProcedure, DataTextField, DataValueField, Nothing)

    End Function

    ''' <summary>
    ''' Generic private method to populate a combobox from a stored procedure
    ''' </summary>
    ''' <param name="cbo">Reference to System.Windows.Forms.ComboBox</param>
    ''' <param name="StoredProcedure">Name of the stored procedure</param>
    ''' <param name="DataTextField">Field name in the result set which contains the text to display</param>
    ''' <param name="DataValueField">Field name in the result set which contains the data value associated with the displayed text</param>
    ''' <param name="paramList"></param>
    ''' <returns>True if successful; False when an exception occurs</returns>
    ''' <remarks></remarks>
    Private Function LoadCombo(ByRef cbo As System.Windows.Forms.ComboBox, ByVal StoredProcedure As String, ByVal DataTextField As String, ByVal DataValueField As String, ByVal paramList As DBParamList) As Boolean

        logger.Debug("LoadCombo Entry")

        Dim reader As SqlDataReader = Nothing
        Dim bStatus As Boolean = False

        Try
            'validate parameters
            If StoredProcedure Is Nothing OrElse StoredProcedure.Length = 0 Then
                Throw New ArgumentException("Invalid argument", "StoredProcedure")
            ElseIf DataTextField Is Nothing OrElse DataTextField.Length = 0 Then
                Throw New ArgumentException("Invalid argument", "DataTextField")
            ElseIf DataValueField IsNot Nothing AndAlso DataValueField.Length = 0 Then
                'DataValueField is optional for some stored procedures returning a single column
                Throw New ArgumentException("Invalid argument", "DataValueField")
            End If

            reader = GetDataReader(StoredProcedure, paramList)

            bStatus = LoadCombo(cbo, reader, DataTextField, DataValueField)

        Catch ex As Exception
            logger.Error(ex.Message)
            Throw ex

        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader = Nothing
            End If

        End Try

        logger.Debug("LoadCombo")


        Return bStatus

    End Function

    Private Function LoadCombo(ByRef cbo As System.Windows.Forms.ComboBox, ByVal reader As SqlClient.SqlDataReader, ByVal DataTextField As String, ByVal DataValueField As String) As Boolean
        Dim cboName As String = ""
        If cbo IsNot Nothing Then
            cboName = cbo.Name
        End If
        logger.DebugFormat("LoadCombo-SQLReader combo.Name={0} Entry", cboName)

        Dim NewIndex As Integer = 0
        Dim iTextFieldIndex As Integer = -1     'use default to distinguish from 0-based index
        Dim iValueFieldIndex As Integer = -1
        Dim DoOnce As Boolean = False
        Dim bStatus As Boolean = False

        Try
            cbo.Items.Clear()

            'validate parameters
            If reader Is Nothing Then
                Throw New ArgumentException("Invalid argument", "DataReader")
            ElseIf DataTextField Is Nothing OrElse DataTextField.Length = 0 Then
                Throw New ArgumentException("Invalid argument", "DataTextField")
            ElseIf DataValueField IsNot Nothing AndAlso DataValueField.Length = 0 Then
                'DataValueField is optional for some stored procedures returning a single column
                Throw New ArgumentException("Invalid argument", "DataValueField")
            End If

            With reader
                While .Read
                    If Not DoOnce Then
                        iTextFieldIndex = .GetOrdinal(DataTextField)
                        If .FieldCount > 1 Then
                            iValueFieldIndex = .GetOrdinal(DataValueField)
                        End If
                        DoOnce = True
                    End If

                    NewIndex = cbo.Items.Add(.GetString(iTextFieldIndex))
                    If iValueFieldIndex >= 0 Then
                        VB6.SetItemData(cbo, NewIndex, .GetValue(iValueFieldIndex))
                    End If
                End While
            End With

            bStatus = True

        Catch ex As Exception
            logger.Error(ex.Message)
            Throw ex

        End Try

        logger.Debug("LoadCombo-SQLReader Exit, Items loaded: " & cbo.Items.Count)

        Return bStatus

    End Function

    Private Function GetDataReader(ByVal StoredProcedure As String) As SqlClient.SqlDataReader

        Return GetDataReader(StoredProcedure, Nothing)

    End Function

    Private Function GetDataReader(ByVal StoredProcedure As String, ByVal paramList As DBParamList) As SqlClient.SqlDataReader

        logger.Debug("GetDataReader Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim reader As SqlDataReader = Nothing

        Try
            'validate parameters
            If StoredProcedure Is Nothing OrElse StoredProcedure.Length = 0 Then
                Throw New ArgumentException("Invalid argument", "StoredProcedure")
            End If

            If paramList Is Nothing Then
                reader = factory.GetStoredProcedureDataReader(StoredProcedure)
            Else
                reader = factory.GetStoredProcedureDataReader(StoredProcedure, paramList)
            End If

        Catch ex As Exception
            logger.Error(ex.Message)
            Throw ex

        Finally
            factory = Nothing

        End Try

        logger.Debug("GetDataReader Exit")

        Return reader

    End Function
#Region "Public methods to load combos"
    Public Sub LoadTeam(ByRef cmbComboBox As System.Windows.Forms.ComboBox)

        Call LoadCombo(cmbComboBox, "GetTeams", "Team_Name", "Team_No")

    End Sub

    Public Sub LoadAllSubTeams(ByRef cmbComboBox As System.Windows.Forms.ComboBox)

        Call LoadCombo(cmbComboBox, "GetAllSubTeams", "SubTeam_Name", "SubTeam_No")

    End Sub

    ' Procedure to fetch all related subteams corresponding to the selected Team from the dropdownbox.
    '  THIS SHOULD PROBABLY BE REPLACED WITH "LoadCombo" type stuff.  
    Public Sub LoadAllTeamSubTeams(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByVal intTeamNo As Int32)

        logger.Debug("LoadAllTeamSubTeams Entry")

        Dim NewIndex As Integer

        Try
            gRSRecordset = SQLOpenRecordSet("SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK) WHERE ST.Team_No = ISNULL(" & intTeamNo & ", ST.Team_No)", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                NewIndex = cmbComboBox.Items.Add(gRSRecordset.Fields("SubTeam_Name").Value)
                VB6.SetItemData(cmbComboBox, NewIndex, gRSRecordset.Fields("SubTeam_No").Value)
                gRSRecordset.MoveNext()
            Loop

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        logger.Debug("LoadAllTeamSubTeams Exit")

    End Sub

    Public Sub LoadSubTeam(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetSubTeams", "SubTeam_Name", "SubTeam_No")
    End Sub

    Public Sub LoadEXEDistSubTeam(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetEXEDistSubTeams", "SubTeam_Name", "SubTeam_No")
    End Sub

    Public Sub LoadSubTeam(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef sTeamList As String)

        logger.Debug("LoadSubTeam Entry")
        Dim paramList As New DBParamList()

        If sTeamList.Length = 0 Then
            Call LoadCombo(cmbComboBox, "GetSubTeams", "SubTeam_Name", "SubTeam_No")
        Else
            paramList.Add(New DBParam("TeamList", DBParamType.String, sTeamList))

            Call LoadCombo(cmbComboBox, "GetSubTeamsByTeam", "SubTeam_Name", "SubTeam_No")
        End If

        paramList = Nothing
        logger.Debug("LoadSubTeam Exit")

    End Sub

    Public Sub LoadSubTeamByType(ByRef eSubTeamType As enumSubTeamType, ByRef cmbSubTeam As System.Windows.Forms.ComboBox, Optional ByRef lNumber As Integer = -1, Optional ByRef lOrderHeader_SubTeamNo As Integer = -1)
        LoadSubTeamByType(eSubTeamType, cmbSubTeam, Nothing, lNumber, lOrderHeader_SubTeamNo)
    End Sub

  Public Sub LoadColumnsForTable(ByRef cmbPurgeJobName As System.Windows.Forms.ComboBox, ByVal Schema As String, ByVal Table As String)
    Dim paramList As New DBParamList()

    paramList.Add(New DBParam("Schema", DBParamType.String, Schema))
    paramList.Add(New DBParam("Table", DBParamType.String, Table))

    Call LoadCombo(cmbPurgeJobName, "GetColumnNamesBySchemaTable", "Column_Name", "Column_Name", paramList)

    paramList = Nothing
    logger.Debug("LoadLocations Exit")
  End Sub

  Public Sub LoadSubTeamByType(ByRef eSubTeamType As enumSubTeamType, ByRef cmbSubTeam As System.Windows.Forms.ComboBox, ByRef abSubTeamUnRestricted() As Boolean, Optional ByRef lNumber As Integer = -1, Optional ByRef lOrderHeader_SubTeamNo As Integer = -1)

        logger.Debug("LoadSubTeamByType Entry")

        Dim reader As SqlDataReader = Nothing
        Dim paramList As New DBParamList()
        Dim storedProcedure As String = String.Empty
        Dim iArraySize As Short

        Try
            cmbSubTeam.Items.Clear()
            iArraySize = -1

            'Load subteams into combo box.
            Select Case eSubTeamType
                Case enumSubTeamType.All

                    storedProcedure = "GetSubTeams"

                Case enumSubTeamType.Store

                    storedProcedure = "GetStoreSubTeam"
                    paramList.Add(New DBParam("Store_No", DBParamType.Int, lNumber))

                Case enumSubTeamType.Supplier

                    storedProcedure = "GetSupplierSubTeam"
                    paramList.Add(New DBParam("Store_No", DBParamType.Int, lNumber))
                    paramList.Add(New DBParam("ProductType_ID", DBParamType.SmallInt, geProductType))

                Case enumSubTeamType.SupplierByVendor

                    storedProcedure = "GetSupplierSubTeamByVendor"
                    paramList.Add(New DBParam("Vendor_ID", DBParamType.Int, lNumber))
                    paramList.Add(New DBParam("ProductType_ID", DBParamType.SmallInt, geProductType))

                Case enumSubTeamType.StoreMinusSupplier

                    storedProcedure = "GetStoreSubTeamMinusSupplier"
                    paramList.Add(New DBParam("Store_No", DBParamType.Int, lNumber))

                Case enumSubTeamType.StoreUser

                    storedProcedure = "GetUserStoreProductSubTeam"
                    paramList.Add(New DBParam("Store_No", DBParamType.Int, lNumber))
                    paramList.Add(New DBParam("User_ID", DBParamType.Int, giUserID))

                Case enumSubTeamType.Retail

                    storedProcedure = "GetRetailSubTeam"

                Case enumSubTeamType.StoreByVendorID

                    storedProcedure = "GetVendStoreSubTeam"
                    paramList.Add(New DBParam("Vendor_ID", DBParamType.Int, lNumber))

                Case enumSubTeamType.SupplierRetail

                    storedProcedure = "GetSupplierRetailSubTeam"
                    paramList.Add(New DBParam("Store_No", DBParamType.Int, lNumber))
                    paramList.Add(New DBParam("OrderHeader_SubTeamNo", DBParamType.Int, lOrderHeader_SubTeamNo))

                Case enumSubTeamType.Packaging

                    storedProcedure = "GetSubTeamByProductType"
                    paramList.Add(New DBParam("ProductType_ID", DBParamType.SmallInt, enumProductType.PackagingSupplies))

                Case enumSubTeamType.Supplies

                    storedProcedure = "GetSubTeamByProductType"
                    paramList.Add(New DBParam("ProductType_ID", DBParamType.SmallInt, enumProductType.OtherSupplies))

                Case Else
                    storedProcedure = "GetSubTeams"

            End Select

            reader = GetDataReader(storedProcedure, paramList)

            ' Iterate through the result set, populating the combo box with all subteams and
            ' setting the abSubTeamUnRestricted values for each of the subteams.
            Dim subTeamName As String
            Dim subTeamNo As Integer
            Dim subTeamUnrestricted As Boolean
            Dim currentIndex As Integer
            While (reader.Read)
                ' Read the current SubTeam_Name, SubTeam_No, and SubTeam_Unrestricted values
                subTeamName = reader.GetString(reader.GetOrdinal("SubTeam_Name"))
                subTeamNo = reader.GetInt32(reader.GetOrdinal("SubTeam_No"))
                subTeamUnrestricted = CBool(reader.GetInt32(reader.GetOrdinal("SubTeam_Unrestricted")))

                ' Add the subteam to the combo box
                currentIndex = cmbSubTeam.Items.Add(subTeamName)
                VB6.SetItemData(cmbSubTeam, currentIndex, subTeamNo)

                ' Set the restricted flag for the subteam
                ReDim Preserve abSubTeamUnRestricted(cmbSubTeam.Items.Count - 1)
                abSubTeamUnRestricted(currentIndex) = subTeamUnrestricted
            End While

        Catch ex As Exception
            logger.Error(ex.Message)
            Throw ex

        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader = Nothing
            End If

            paramList = Nothing

        End Try

        logger.Debug("LoadSubTeamByType Exit")

    End Sub

    Public Function GetDeletablePBHIds(ByVal pbhIds As String) As String

        logger.Debug("GetDeletablePBHIds Entry")

        Dim reader As SqlDataReader = Nothing
        Dim paramList As New DBParamList()
        Dim storedProcedure As String = String.Empty
        Dim deletablePBHIds As String = String.Empty

        Try
            storedProcedure = "GetDeletablePBHIds"
            paramList.Add(New DBParam("PriceBatchHeaderIds", DBParamType.String, pbhIds))

            reader = GetDataReader(storedProcedure, paramList)

            ' Iterate through the result set, populating the combo box with all subteams and
            ' setting the abSubTeamUnRestricted values for each of the subteams.

            While (reader.Read)
                ' Read the current SubTeam_Name, SubTeam_No, and SubTeam_Unrestricted values
                deletablePBHIds = deletablePBHIds + "," + reader.GetInt32(reader.GetOrdinal("PriceBatchHeaderId")).ToString()
            End While

            If (Len(deletablePBHIds) > 0) Then
                deletablePBHIds = deletablePBHIds.Substring(1)
            End If
        Catch ex As Exception

            logger.Error(ex.Message)
            Throw ex

        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader = Nothing
            End If

            paramList = Nothing

        End Try

        Return deletablePBHIds

        logger.Debug("GetDeletablePBHIds Exit")

    End Function
    Public Sub LoadLocations(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef lStore_No As Integer, Optional ByRef lSubTeamID As Integer = 0)

        logger.Debug("LoadLocations Entry")

        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("StoreID", DBParamType.Int, lStore_No))
        If lSubTeamID = 0 Then
            paramList.Add(New DBParam("SubTeamID", DBParamType.Int, DBNull.Value))
        Else
            paramList.Add(New DBParam("SubTeamID", DBParamType.Int, lSubTeamID))
        End If

        Call LoadCombo(cmbComboBox, "GetInventoryLocationsByStore", "InvLoc_Name", "InvLoc_ID", paramList)

        paramList = Nothing

        logger.Debug("LoadLocations Exit")


    End Sub

    Public Sub LoadSupplierSubteam(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef lStore_No As Integer, ByRef abSubTeamUnRestricted() As Boolean)

        Call LoadSubTeamByType(enumSubTeamType.Supplier, cmbComboBox, abSubTeamUnRestricted, lStore_No)

    End Sub

    Public Sub LoadBuyerVendor(ByRef cmbComboBox As System.Windows.Forms.ComboBox)

        Call LoadCombo(cmbComboBox, "BuyerVendor", "CompanyName", "Vendor_ID")

    End Sub

    Public Sub LoadBrand(ByRef cmbComboBox As System.Windows.Forms.ComboBox)

        Call LoadCombo(cmbComboBox, "GetBrandAndID", "Brand_Name", "Brand_ID")

    End Sub

    Public Sub LoadBrandNew(ByRef cmbComboBox As System.Windows.Forms.ComboBox)

        logger.Debug("Global.LoadBrandNew Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing

        ' Execute the stored procedure 
        results = factory.GetStoredProcedureDataTable("GetBrandAndID")

        With cmbComboBox
            .DataSource = results
            .DisplayMember = "Brand_Name"
            .ValueMember = "Brand_ID"
            .SelectedIndex = -1
        End With

        logger.Debug("Global.LoadBrandNew Exit")

    End Sub

    Public Sub LoadCategory(ByRef cmbComboBox As System.Windows.Forms.ComboBox)

        Call LoadCategory(cmbComboBox, -1)

    End Sub

    Public Sub LoadCategory(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByVal SubTeamID As Integer)

        logger.Debug("LoadCategory Entry")

        Dim NewIndex As Integer
        Dim procedureName As String = String.Empty
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As ArrayList = Nothing

        Dim ShowFullName As Boolean = InstanceDataDAO.IsFlagActive("ShowFullItemCategoryName")

        Try
            cmbComboBox.Items.Clear()
            If SubTeamID < 0 Then
                'all subteams
                procedureName = "GetCategoryAndID"
            Else
                procedureName = "GetCategoriesBySubTeam"
                paramList = New ArrayList(1)
                paramList.Add(New DBParam("SubTeam_No", DBParamType.Int, SubTeamID))
            End If

            'Load all Category into combo box
            With factory.GetStoredProcedureDataReader(procedureName, paramList)
                While .Read
                    If ShowFullName Then
                        NewIndex = cmbComboBox.Items.Add(.GetString(.GetOrdinal("Category_Name")))
                    Else
                        NewIndex = cmbComboBox.Items.Add(GetCategoryClass(.GetString(.GetOrdinal("Category_Name"))))
                    End If
                    VB6.SetItemData(cmbComboBox, NewIndex, .GetInt32(.GetOrdinal("Category_ID")))
                End While

                .Close()
            End With

        Finally
            paramList = Nothing
            factory = Nothing
        End Try

        logger.Debug("LoadCategory Exit")
    End Sub

    Public Sub LoadProdHierarchyLevel3s(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByVal CategoryID As Integer)

        logger.Debug("LoadProdHierarchyLevel3s Entry")
        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("Category_ID", DBParamType.Int, CategoryID))
        Call LoadCombo(cmbComboBox, "GetProdHierarchyLevel3sByCategory", "Description", "ProdHierarchyLevel3_ID", paramList)
        paramList = Nothing
        logger.Debug("LoadProdHierarchyLevel3s Exit")
    End Sub

    Public Sub LoadProdHierarchyLevel3s(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetAllProdHierarchyLevel3", "Description", "ProdHierarchyLevel3_ID")
    End Sub

    Public Sub LoadProdHierarchyLevel4s(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByVal Level3ID As Integer)
        logger.Debug("LoadProdHierarchyLevel4s Entry")

        Dim paramList As New DBParamList()
        paramList.Add(New DBParam("ProdHierarchyLevel3_ID", DBParamType.Int, Level3ID))
        Call LoadCombo(cmbComboBox, "GetProdHierarchyLevel4sByLevel3", "Description", "ProdHierarchyLevel4_ID", paramList)
        paramList = Nothing

        logger.Debug("LoadProdHierarchyLevel4s Exit")
    End Sub

    Public Sub LoadRipeZones(ByRef cmbComboBox As System.Windows.Forms.ComboBox, Optional ByVal locationID As Short = -1)
        Call LoadCombo(cmbComboBox, "GetRipeZones", "ZoneName", "ZoneID")
    End Sub

    Public Sub LoadRipeLocations(ByRef cmbComboBox As System.Windows.Forms.ComboBox, Optional ByVal sUserName As String = "")
        logger.Debug("LoadRipeLocations Entry")
        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("UserName", DBParamType.String, sUserName))
        Call LoadCombo(cmbComboBox, "GetRipeLocations", "LocationName", "LocationID", paramList)
        paramList = Nothing

        logger.Debug("LoadRipeLocations Exit")
    End Sub

    Public Sub LoadExType(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef AppID As Short)
        logger.Debug("LoadExType Entry")
        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("AppID", DBParamType.Int, AppID))
        Call LoadCombo(cmbComboBox, "GetExTypes", "RuleName", "RuleID", paramList)
        paramList = Nothing
        logger.Debug("LoadExType Exit")
    End Sub

    Public Sub LoadExSeverity(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef AppID As Short, ByRef RuleID As Short)

        logger.Debug("LoadExSeverity Entry")

        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("AppID", DBParamType.Int, AppID))
        paramList.Add(New DBParam("RuleID", DBParamType.Int, RuleID))

        Call LoadCombo(cmbComboBox, "GetExSeverity", "Severity", Nothing, paramList)

        paramList = Nothing

        logger.Debug("LoadExSeverity Exit")

    End Sub

    Public Sub LoadStates(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetRegionStates", "State")
    End Sub

    Public Sub LoadSubTeamCategory(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef lSubTeam_No As Integer)

        logger.Debug("LoadSubTeamCategory Entry")

        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("SubTeam_No", DBParamType.Int, lSubTeam_No))

        Call LoadCombo(cmbComboBox, "GetCategoriesBySubTeam", "Category_Name", "Category_ID", paramList)

        paramList = Nothing

        logger.Debug("LoadSubTeamCategory Exit")
    End Sub

    Public Sub LoadDepartment(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetDepartmentAndID", "Department_Name", "Department_ID")
    End Sub

    Public Sub LoadOrigin(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetOriginAndID", "Origin_Name", "Origin_ID")
    End Sub

    Public Sub LoadUnit(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetUnitAndID", "Unit_Name", "Unit_ID")
    End Sub
    Public Sub LoadUnitAbbreviation(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetUnitAndID", "Unit_Abbreviation", "Unit_ID")
    End Sub

    Public Sub LoadItemUnitsCost(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef WeightUnits As Boolean, Optional ByVal AllUnits As Boolean = False)
        logger.Debug("LoadItemUnitsCost Entry")
        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("WeightUnits", DBParamType.Bit, System.Math.Abs(CType(WeightUnits, Integer))))
        'Call LoadCombo(cmbComboBox, "GetItemUnitsCost", "Unit_Name", "Unit_ID", paramList)
        Call LoadCombo(cmbComboBox, "GetAllItemUnitsCost", "Unit_Name", "Unit_ID", Nothing)
        paramList = Nothing

        logger.Debug("LoadItemUnitsCost Exit")
    End Sub

    Public Sub LoadItemUnitsVendor(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef WeightUnits As Boolean, Optional ByVal AllUnits As Boolean = False)
        logger.Debug("LoadItemUnitsVendor Entry")

        'Rick Kelleher's fix for Bug 4311 is superseded by using the LoadCombo function
        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("WeightUnits", DBParamType.Bit, System.Math.Abs(CType(WeightUnits, Integer))))
        'paramList.Add(New DBParam("AllUnits", DBParamType.Bit, System.Math.Abs(CType(AllUnits, Integer))))
        Call LoadCombo(cmbComboBox, "GetItemUnitsVendor", "Unit_Name", "Unit_ID", paramList)
        paramList = Nothing

        logger.Debug("LoadItemUnitsVendor Exit")
    End Sub

    Public Sub LoadPackageDescriptionUnit(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef WeightsOnly As Boolean, Optional ByVal AllUnits As Boolean = False)
        logger.Debug("LoadPackageDescriptionUnit Entry")
        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("WeightsOnly", DBParamType.Bit, System.Math.Abs(CType(WeightsOnly, Integer))))
        'paramList.Add(New DBParam("AllUnits", DBParamType.Bit, System.Math.Abs(CType(AllUnits, Integer))))
        Call LoadCombo(cmbComboBox, "GetItemUnitsPDU", "Unit_Name", "Unit_ID", paramList)
        paramList = Nothing

        logger.Debug("LoadPackageDescriptionUnit Exit")
    End Sub

    Public Sub LoadInternalCustomer(ByRef cmbComboBox As System.Windows.Forms.ComboBox, Optional ByRef lException As Integer = 0)
        logger.Debug("LoadInternalCustomer Entry")
        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("Exception_ID", DBParamType.Int, lException))

        Call LoadCombo(cmbComboBox, "GetInternalCustomers", "CompanyName", "Vendor_ID", paramList)

        paramList = Nothing

        logger.Debug("LoadInternalCustomer Exit")
    End Sub

    Public Sub LoadCustomer(ByRef cmbComboBox As System.Windows.Forms.ComboBox, Optional ByVal bRegional As Boolean = False)
        logger.Debug("LoadCustomer Entry")

        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("Regional", DBParamType.Bit, System.Math.Abs(CType(bRegional, Integer))))

        Call LoadCombo(cmbComboBox, "GetCustomers", "CompanyName", "Vendor_ID", paramList)

        paramList = Nothing

        logger.Debug("LoadCustomer Exit")
    End Sub

    Public Sub LoadRegionCustomer(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetRegionCustomers", "CompanyName", "Vendor_ID")
    End Sub

    Public Sub LoadRegionCustomerAsDC(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetRegionCustomersAsDC", "CompanyName", "Vendor_ID")
    End Sub

    Public Sub LoadApps(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetApps", "AppName", "AppID")
    End Sub

    Public Sub LoadVendors(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetVendors", "CompanyName", "Vendor_ID")
    End Sub

    Public Sub LoadVendorItemStatuses(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetVendorItemStatuses", "StatusName", "StatusID")
    End Sub

    Public Sub LoadStoreCustomer(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef lStore_No As Integer)
        logger.Debug("LoadStoreCustomer Entry")

        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("Store_No", DBParamType.Int, lStore_No))
        paramList.Add(New DBParam("Vendor_ID", DBParamType.Int, DBNull.Value))

        Call LoadCombo(cmbComboBox, "GetStoreCustomer", "CompanyName", "Vendor_ID", paramList)

        paramList = Nothing

        logger.Debug("LoadStoreCustomer Exit")
    End Sub

    Public Sub LoadVendStoreSubteam(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef lVendor_ID As Integer, ByRef abSubTeamUnRestricted() As Boolean)
        Call LoadSubTeamByType(enumSubTeamType.StoreByVendorID, cmbComboBox, abSubTeamUnRestricted, lVendor_ID)
    End Sub

    Public Sub LoadVendStoreSubteam(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef lVendor_ID As Integer)
        Call LoadSubTeamByType(enumSubTeamType.StoreByVendorID, cmbComboBox, lVendor_ID)
    End Sub

    Public Sub LoadStoreSubteam(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef lStore_No As Integer)
        Call LoadSubTeamByType(enumSubTeamType.Store, cmbComboBox, lStore_No)
    End Sub

    Public Sub LoadReplenishment(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetReplenishments", "Replenishment_Date", "ReplenishmentHeader_ID")
    End Sub

    Public Sub LoadDistributionCenters(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetAllDistributionCenters", "Store_Name", "Store_No")
    End Sub

    Public Sub LoadStore(ByRef cmbComboBox As System.Windows.Forms.ComboBox, Optional ByVal bInclude_Dist As Boolean = False)
        If bInclude_Dist Then
            Call LoadCombo(cmbComboBox, "GetStoresAndDist", "Store_Name", "Store_No")
        Else
            Call LoadCombo(cmbComboBox, "GetAllStores", "Store_Name", "Store_No")
        End If
    End Sub

    Public Sub LoadRetailStore(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetRetailStores", "Store_Name", "Store_No")
    End Sub

    Public Sub LoadStores(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetAllStores", "Store_Name", "Store_No")
    End Sub

    Public Sub LoadDistMfg(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetDistAndMfg", "Store_Name", "Store_No")
    End Sub

    Public Sub LoadInventoryStore(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetStoresAndDist", "Store_Name", "Store_No")
    End Sub

    ' New code to fix the bug 5938. Adding only warehouse related stores to the combobox.
    Public Sub LoadWarehouseStore(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetWarehouses", "Store_Name", "Store_No")
    End Sub

    Public Sub LoadPricingUser(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetUsers", "UserName", "User_ID")
    End Sub

    Public Sub LoadNatClass(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetNatClass", "ClassName", "ClassID")
    End Sub

    Public Sub LoadItemType(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "ItemTypeList", "ItemType_Name", "ItemType_ID")
    End Sub

    Public Sub LoadPriceBatchStatus(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetPriceBatchStatusList", "PriceBatchStatusDesc", "PriceBatchStatusID")
    End Sub

    Public Sub LoadPricingMethod(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "PricingMethodList", "PricingMethod_Name", "PricingMethod_ID")
    End Sub

    Public Sub LoadCreditReasons(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetCreditReasons", "CreditReason", "CreditReason_ID")
    End Sub

    Public Sub LoadReasonCodes(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByVal enumRCT As enumReasonCodeType)
        'Generic Method to load reason codes into combo box
        logger.Debug("Global.LoadReasonCodes Exit")

        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("@ReasonCodeTypeAbbr", DBParamType.Char, enumRCT.ToString))

        Call LoadCombo(cmbComboBox, "ReasonCodes_GetDetailsForType", "ReasonCode", "ReasonCodeDetailID", paramList)

        paramList = Nothing

        logger.Debug("Global.LoadReasonCodes Exit")

    End Sub

    Public Sub LoadReasonCodesUltraCombo(ByRef ucReasonCode As Infragistics.Win.UltraWinGrid.UltraCombo, ByVal enumRCT As enumReasonCodeType)
        'Generic Method to load reason codes into ultra combo box
        logger.Debug("Global.LoadReasonCodesUltraCombo Exit")

        Dim paramList As New DBParamList()
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@ReasonCodeTypeAbbr"
        currentParam.Value = enumRCT.ToString
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)

        ' Execute the stored procedure 
        results = factory.GetStoredProcedureDataTable("ReasonCodes_GetDetailsForType", paramList)

        ucReasonCode.DataSource = results
        ucReasonCode.DisplayMember = "ReasonCode"
        ucReasonCode.ValueMember = "ReasonCodeDetailID"

        ucReasonCode.DisplayLayout.Bands(0).Columns("ReasonCode").Header.Caption = "Code"
        ucReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDesc").Header.Caption = "Description"
        ucReasonCode.DisplayLayout.Bands(0).Columns("ReasonCode").Width = 50
        ucReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDesc").Width = 200
        ucReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDetailID").Hidden = True
        ucReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeExtDesc").Hidden = True

        paramList = Nothing

        logger.Debug("Global.LoadReasonCodesUltraCombo Exit")

    End Sub

    Public Sub LoadUsers(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Call LoadCombo(cmbComboBox, "GetUsers", "UserName", "User_ID")
    End Sub

    Public Sub LoadStoreJurisdictions(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        logger.Debug("LoadStoreJurisdictions Entry")

        Dim NewIndex As Integer

        cmbComboBox.Items.Clear()

        Try
            'Load all Unit Abbr into combo box.
            gRSRecordset = SQLOpenRecordSet("EXEC GetStoreJurisdictions", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Do While Not gRSRecordset.EOF
                NewIndex = cmbComboBox.Items.Add(gRSRecordset.Fields("StoreJurisdictionDesc").Value)
                VB6.SetItemData(cmbComboBox, NewIndex, gRSRecordset.Fields("StoreJurisdictionID").Value)
                gRSRecordset.MoveNext()
            Loop
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try
        logger.Debug("LoadStoreJurisdictions Exit")

    End Sub

    Public Sub LoadInvAdjReason(ByRef cmbReason As System.Windows.Forms.ComboBox, ByRef bDistributionCenter As Boolean, ByRef bIsStore As Boolean)
        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("Distribution_Center", DBParamType.Bit, bDistributionCenter))
        paramList.Add(New DBParam("WFM_Store", DBParamType.Bit, bIsStore))

        Call LoadCombo(cmbReason, "GetInventoryAdjustmentCodeList", "LongDescription", "InventoryAdjustmentCode_ID", paramList)

    End Sub

    Public Sub LoadCurrency(ByRef cmbReason As System.Windows.Forms.ComboBox)

        Call LoadCombo(cmbReason, "GetCurrencies", "CurrencyCode", "CurrencyID")

    End Sub

    Public Sub LoadPaymentTerms(ByRef cmbPaymentTerms As System.Windows.Forms.ComboBox)

        Call LoadCombo(cmbPaymentTerms, "GetVendorPaymentTerms", "Description", "PaymentTermID")

    End Sub
#End Region

    Public Function GetAdoNetConnection() As System.Data.SqlClient.SqlConnection

        logger.Debug("GetAdoNetConnection Entry")

        Static oCon As System.Data.SqlClient.SqlConnection
        Dim sConStr As String

        Try
            If oCon Is Nothing Then
                sConStr = ConfigurationManager.ConnectionStrings(DataFactory.ItemCatalog).ConnectionString
                ' sConStr = ReadINI("DBMS", "ADONET", "Inventory.INI", True)
                oCon = New SqlClient.SqlConnection(sConStr)
                oCon.Open()
            ElseIf oCon.State = ConnectionState.Closed Then
                oCon.Open()
            End If
            Return oCon
        Catch ex As Exception
            logger.Error(ex.Message)
            Throw ex
        End Try

        logger.Debug("GetAdoNetConnection Exit")

    End Function

    Public Function InsertODBCError(ByVal dbCommand As String, ByVal errorNumber As Integer, ByVal errorMsg As String, ByVal startTime As Date, ByVal endTime As Date) As Boolean
        'log exception to ODBCErrorLog table
        Dim odbcError As New ODBCErrorLog()
        Dim subject As String = String.Empty
        Dim msg As String = String.Empty
        Dim status As Boolean = False
        Dim BULLET As Char = Chr(149)

        Try
            With odbcError
                .ODBCStart = startTime
                .ODBCEnd = endTime
                .ErrorNumber = errorNumber
                .ErrorDescription = errorMsg
                .ODBCCall = dbCommand

                .InsertODBCErrorLog()
            End With

            subject = "Logged ODBC error"
            msg = String.Format("A fatal database execution occurred and was automatically logged:{0}  {1}  {2}", Environment.NewLine, BULLET, dbCommand)
            errorMsg = String.Format("Error {0}: {1}", errorNumber, errorMsg)

            status = True

        Catch ex As Exception
            subject = "Unlogged ODBC error"
            msg = String.Format("A fatal database execution occurred and could not be logged:{0}  {1}  {2}", Environment.NewLine, BULLET, dbCommand)
            errorMsg = String.Format("Error {0}: {1}{2}{2}Exception: {3}", errorNumber, errorMsg, Environment.NewLine, ex.ToString)

        Finally
            ErrorDialog.HandleError(subject, msg, errorMsg, ErrorDialog.NotificationTypes.DialogAndEmail, String.Empty)
            logger.Error(String.Format("{0} ({1})", msg, errorMsg))

            odbcError = Nothing

        End Try

        Return status

    End Function

    Public Sub SQLExecute(ByRef sCall As String, ByRef Param1 As Integer, Optional ByRef bIgnoreError As Boolean = False, Optional ByRef bValidateLogon As Boolean = True)
        ' NOTE: The changes to use the DataFactory were removed because they did not allow for the transaction support as it was
        ' coded into the application code.  See the usage in the OrderStatus.SaveData function for an example.  When the COMMIT TRAN
        ' call is issued, it fails because of the difference in transaction handling between the DataFactory and the DAO calls.
        ' More work must be done throughout the application to switch to the DataFactory.
        logger.Debug("SQLExecute entry: sCall=" + sCall)
        Dim dStart, dEnd As Date
        Dim lNumber As Integer
        Dim sDescription As String
        Dim i As Integer

        If bValidateLogon Then ValidateLogon()

        On Error Resume Next

        dStart = Now
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        logger.DebugFormat("DAO => {0} ", sCall)
        gDBInventory.Execute(sCall, Param1)

        If Err.Number = 0 Then GoTo me_exit

        dEnd = Now
        If DAODBEngine_definst.Errors.Count > 0 Then
            lNumber = DAODBEngine_definst.Errors(0).Number
            sDescription = DAODBEngine_definst.Errors(0).Description
        Else
            lNumber = Err.Number
            sDescription = Err.Description
        End If

        Err.Clear()

        'If the caller started a transaction, must rollback
        Do
            gDBInventory.Execute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Loop Until Err.Number <> 0

        Err.Clear()

        If bIgnoreError Then GoTo me_exit

        logger.Error("SQLExecute error encounted.  An entry is being added to ODBCErrorLog." + Environment.NewLine + "SQL Call = " + sCall + Environment.NewLine + "Error Description = " + sDescription)
        Call InsertODBCError(sCall, lNumber, sDescription, dStart, dEnd)

        End
        logger.Debug("SQLExecute exit")
me_exit:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        GC.Collect()
        logger.Debug("SQLExecute exit from me_exit:")
    End Sub

    Public Sub SQLExecute2(ByRef sCall As String, ByRef Param1 As Integer, Optional ByRef bIgnoreError As Boolean = False, Optional ByRef bValidateLogon As Boolean = True)

        logger.Debug("SQLExecute2 entry: sCall=" + sCall)
        'NOTE: Unlike SQLExecute, bIgnoreError=True means raise back to the caller rather than do nothing
        'So the calling routine must handle the error if the bIgnoreError is set to true

        Dim dStart, dEnd As Date
        Dim lNumber As Integer
        Dim sDescription As String
        Dim i As Integer

        If bValidateLogon Then ValidateLogon()

        On Error Resume Next

        dStart = Now
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        logger.DebugFormat("DAO => {0} ", sCall)
        gDBInventory.Execute(sCall, Param1)

        If Err.Number = 0 Then GoTo me_exit

        dEnd = Now
        If DAODBEngine_definst.Errors.Count > 0 Then
            lNumber = DAODBEngine_definst.Errors(0).Number
            sDescription = DAODBEngine_definst.Errors(0).Description
        Else
            lNumber = Err.Number
            sDescription = Err.Description
        End If

        Err.Clear()

        If bIgnoreError Then
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            On Error GoTo 0
            Err.Raise(lNumber, , sDescription)
        Else

            'If the caller started a transaction, must rollback
            Do
                gDBInventory.Execute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Loop Until Err.Number <> 0

            Err.Clear()

            logger.Error("SQLExecute2 error encounted.  An entry is being added to ODBCErrorLog." + Environment.NewLine + "SQL Call = " + sCall + Environment.NewLine + "Error Description = " + sDescription)
            Call InsertODBCError(sCall, lNumber, sDescription, dStart, dEnd)
            'gDBInventory.Execute("EXEC InsertODBCError '" & VB6.Format(dStart, "MM/DD/YYYY HH:MM:SS") & "', '" & VB6.Format(dEnd, "MM/DD/YYYY HH:MM:SS") & "', " & lNumber & ", '" & DoubleApostrophe(sDescription) & "', '" & DoubleApostrophe(sCall) & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)

            'If Err.Number = 0 Then
            '    MsgBox("The last database execution failed and was automatically logged." & vbCrLf & vbCrLf & "Please contact the Helpdesk and ask them to review this log." & vbCrLf & vbCrLf & "Click ""OK"" and then restart IRMA.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, My.Application.Info.Title)
            '    logger.Error("The last database execution failed and was automatically logged." & vbCrLf & vbCrLf & "Please contact the Helpdesk and ask them to review this log." & vbCrLf & vbCrLf & "Click ""OK"" and then restart IRMA.")

            'Else
            '    MsgBox("Auto error logging failed.  Please call the Helpdesk and report the following information:" & vbCrLf & "Started = " & VB6.Format(dStart, "MM/DD/YYYY HH:MM:SS") & vbCrLf & "Ended = " & VB6.Format(dEnd, "MM/DD/YYYY HH:MM:SS") & vbCrLf & "Error = " & lNumber & " - " & sDescription & vbCrLf & "Call = " & sCall, MsgBoxStyle.SystemModal + MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, My.Application.Info.Title)
            '    logger.Error("Auto error logging failed.  Please call the Helpdesk and report the following information:" & vbCrLf & "Started = " & VB6.Format(dStart, "MM/DD/YYYY HH:MM:SS") & vbCrLf & "Ended = " & VB6.Format(dEnd, "MM/DD/YYYY HH:MM:SS") & vbCrLf & "Error = " & lNumber & " - " & sDescription & vbCrLf & "Call = " & sCall)
            'End If

            End

        End If

        logger.Debug("SQLExecute2 exit")
me_exit:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        GC.Collect()
        logger.Debug("SQLExecute2 exit(me_exit:")
    End Sub

    Public Sub SQLExecute3(ByRef sCall As String, ByRef Param1 As Integer, ByRef lIgnoreErrNum() As Integer, Optional ByRef bValidateLogon As Boolean = True)
        logger.Debug("SQLExecute3 entry: sCall=" + sCall)
        'NOTE: Unlike SQLExecute, if lIgnoreErrNum occurs, raise it back to the caller; otherwise, handle the error by ending the app

        Dim dStart, dEnd As Date
        Dim lNumber As Integer
        Dim sDescription As String
        Dim i As Integer

        If bValidateLogon Then ValidateLogon()

        On Error Resume Next

        dStart = Now
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        gDBInventory.Execute(sCall, Param1)
        logger.DebugFormat("DAO => {0} ", sCall)
        If Err.Number = 0 Then GoTo me_exit

        dEnd = Now
        If DAODBEngine_definst.Errors.Count > 0 Then
            lNumber = DAODBEngine_definst.Errors(0).Number
            sDescription = DAODBEngine_definst.Errors(0).Description
        Else
            lNumber = Err.Number
            sDescription = Err.Description
        End If

        Err.Clear()

        For i = LBound(lIgnoreErrNum) To UBound(lIgnoreErrNum)
            If lNumber = lIgnoreErrNum(i) Then
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                On Error GoTo 0
                Err.Raise(lNumber, , sDescription)
            End If
        Next

        'If the caller started a transaction, must rollback
        Do
            gDBInventory.Execute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Loop Until Err.Number <> 0

        Err.Clear()

        logger.Error("SQLExecute3 error encounted.  An entry is being added to ODBCErrorLog." + Environment.NewLine + "SQL Call = " + sCall + Environment.NewLine + "Error Description = " + sDescription)
        Call InsertODBCError(sCall, lNumber, sDescription, dStart, dEnd)

        End

        logger.Debug("SQLExecute3 exit")
me_exit:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        GC.Collect()
        logger.Debug("SQLExecute3 Exith with(me_exit:)")
    End Sub


    Public Sub CenterForm(ByRef frmForm As System.Windows.Forms.Form)


        frmForm.SetBounds(VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(frmForm.Width)) / 2), VB6.TwipsToPixelsY(((VB6.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6.PixelsToTwipsY(frmForm.Height)) / 2) - 300), 0, 0, Windows.Forms.BoundsSpecified.X Or Windows.Forms.BoundsSpecified.Y)


        frmForm.Icon = frmMain.Icon



    End Sub

    Private Function CreateDataTable(ByVal TableName As String, ByVal StoredProcedure As String, ByVal DataTextField As String, ByVal DataValueField As String, ByVal paramList As DBParamList) As DataTable


        logger.Debug("CreateDataTable Entry")

        Dim table As New DataTable(TableName)
        Dim reader As SqlDataReader = Nothing
        Dim Keys(0) As DataColumn

        Try
            'validate parameters
            If TableName Is Nothing OrElse TableName.Length = 0 Then
                Throw New ArgumentException("Invalid argument", "TableName")
            ElseIf StoredProcedure Is Nothing OrElse StoredProcedure.Length = 0 Then
                Throw New ArgumentException("Invalid argument", "StoredProcedure")
            ElseIf DataTextField Is Nothing OrElse DataTextField.Length = 0 Then
                Throw New ArgumentException("Invalid argument", "DataTextField")
            ElseIf DataValueField IsNot Nothing AndAlso DataValueField.Length = 0 Then
                'DataValueField is optional for some stored procedures returning a single column
                Throw New ArgumentException("Invalid argument", "DataValueField")
            End If

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

            With table
                .Columns.Add(New DataColumn(DataValueField, GetType(Integer)))
                .Columns.Add(New DataColumn(DataTextField, GetType(String)))

                Keys(0) = .Columns(DataValueField)

                .PrimaryKey = Keys

                reader = GetDataReader(StoredProcedure, paramList)

                .Load(reader)
            End With

        Catch ex As Exception
            logger.Error(ex.Message)
            Throw ex

        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader = Nothing
            End If

            System.Windows.Forms.Cursor.Current = Cursors.Default

        End Try

        logger.Debug("CreateDataTable Exit")

        Return table

    End Function

    Public Function GetBrandData() As DataTable

        Return CreateDataTable("ItemBrand", "GetBrandAndID", "Brand_Name", "Brand_ID", Nothing)

    End Function

    Public Function GetTaxClassificationData() As DataTable

        Return CreateDataTable("TaxClassifications", "TaxHosting_GetTaxClass", "TaxClassDesc", "TaxClassID", Nothing)

    End Function

    Public Function GetOriginData() As DataTable

        Return CreateDataTable("Origin", "GetOriginAndID", "Origin_Name", "Origin_ID", Nothing)

    End Function

    Public Sub PopulateAWeight()

        logger.Debug("PopulateAWeight Entry")


        Dim index As Short = -1

        ReDim aWeight_Unit(index)

        'Load all Unit Abbr into combo box.
        With GetDataReader("GetUnitAndID")
            While .Read
                index = aWeight_Unit.GetUpperBound(0) + 1

                ReDim Preserve aWeight_Unit(index)

                aWeight_Unit(index).Unit_ID = .GetInt32(.GetOrdinal("Unit_ID"))
                aWeight_Unit(index).Weight_Unit = .GetBoolean(.GetOrdinal("Weight_Unit"))
                aWeight_Unit(index).SystemCode = .GetString(.GetOrdinal("UnitSysCode"))
                aWeight_Unit(index).IsPackageUnit = .GetBoolean(.GetOrdinal("IsPackageUnit"))
            End While
            .Close()
        End With
        logger.Debug("PopulateAWeight Exit")

    End Sub

    Public Sub PopulateProductType(ByRef cmbComboBox As System.Windows.Forms.ComboBox)

        logger.Debug("PopulateProductType Entry")

        Dim NewIndex As Integer

        NewIndex = cmbComboBox.Items.Add(("Product"))
        VB6.SetItemData(cmbComboBox, NewIndex, CShort(enumProductType.Product))
        NewIndex = cmbComboBox.Items.Add(("Packaging Supplies"))
        VB6.SetItemData(cmbComboBox, NewIndex, CShort(enumProductType.PackagingSupplies))
        NewIndex = cmbComboBox.Items.Add(("Other Supplies"))
        VB6.SetItemData(cmbComboBox, NewIndex, CShort(enumProductType.OtherSupplies))

        ' Default the Product Type to Product
        cmbComboBox.SelectedIndex = 0
        ' Set the global product type to match
        geProductType = enumProductType.Product

        logger.Debug("PopulateProductType Exit")


    End Sub

    Public Function ReturnUnitID(ByVal sUnit_Name As String) As Integer

        logger.Debug("ReturnUnitID Entry with sUnit_Name=" + sUnit_Name)


        Dim iUnit_ID As Integer = -1

        sUnit_Name = sUnit_Name.ToUpper

        With GetDataReader("GetUnitAndID")
            While .Read
                If sUnit_Name.Equals(.GetString(.GetOrdinal("Unit_Name")).ToUpper) Then
                    iUnit_ID = .GetInt32(.GetOrdinal("Unit_ID"))
                    Exit While
                End If
            End While
            .Close()
        End With

        logger.Debug("ReturnUnitID Exit")

        Return iUnit_ID

    End Function

    Private Function GetTextByID(ByVal TextByIDType As enumTextByID, ByVal databaseID As Integer) As String


        logger.Debug("GetTextByID Entry with databaseID =" + databaseID.ToString())

        Dim paramList As New DBParamList()
        Dim storedProcedure As String = String.Empty
        Dim textValue As String = String.Empty
        Dim dataTextField As String = String.Empty
        Dim dataValueField As String = String.Empty

        Try
            Select Case TextByIDType
                Case enumTextByID.BrandName
                    storedProcedure = "GetBrandName"
                    dataTextField = "Brand_Name"
                    dataValueField = "Vendor_ID"

                Case enumTextByID.CategoryName
                    storedProcedure = "GetCategoryName"
                    dataTextField = "Category_Name"
                    dataValueField = "Vendor_ID"

                Case enumTextByID.SignName
                    storedProcedure = "GetSignName"
                    dataTextField = "Sign_Desc"
                    dataValueField = "Sign_ID"

                Case enumTextByID.StoreName
                    storedProcedure = "GetStoreName"
                    dataTextField = "Store_Name"
                    dataValueField = "Store_No"

                Case enumTextByID.SubTeamMargin
                    storedProcedure = "GetSubTeamMargin"
                    dataTextField = "Target_Margin"
                    dataValueField = "SubTeam_No"

                Case enumTextByID.SubTeamName
                    storedProcedure = "GetSubTeamName"
                    dataTextField = "SubTeam_Name"
                    dataValueField = "SubTeam_No"

                Case enumTextByID.UserFullName
                    storedProcedure = "GetUserFullName"
                    dataTextField = "FullName"
                    dataValueField = "User_ID"

                Case enumTextByID.UserName
                    storedProcedure = "GetUserName"
                    dataTextField = "UserName"
                    dataValueField = "User_ID"

                Case enumTextByID.VendorName
                    storedProcedure = "GetVendorName"
                    dataTextField = "CompanyName"
                    dataValueField = "Vendor_ID"

            End Select

            paramList.Add(New DBParam(dataValueField, DBParamType.Int, databaseID))

            With GetDataReader(storedProcedure, paramList)
                While .Read
                    textValue = .GetString(.GetOrdinal(dataTextField))
                End While
                .Close()
            End With

        Catch ex As Exception
            logger.Error(ex.Message)
            Throw ex

        Finally
            paramList = Nothing

        End Try

        logger.Debug("GetTextByID Exit")

        Return textValue

    End Function

    Public Function ReturnVendorName(ByRef lVendorID As Integer) As String
        Return GetTextByID(enumTextByID.VendorName, lVendorID)
    End Function

    Public Function ReturnCategoryName(ByRef lVendorID As Integer) As String
        Return GetTextByID(enumTextByID.CategoryName, lVendorID)
    End Function

    Public Function ReturnBrandName(ByRef lVendorID As Integer) As String
        Return GetTextByID(enumTextByID.BrandName, lVendorID)
    End Function

    Public Function GetInvUserName(ByVal iUserID As Integer) As String
        Return GetTextByID(enumTextByID.UserName, iUserID)
    End Function

    Public Function GetInvUserFullName(ByVal iUserID As Integer) As String
        Return GetTextByID(enumTextByID.UserFullName, iUserID)
    End Function

    Public Function GetStoreName(ByVal iStoreID As Integer) As String
        Return GetTextByID(enumTextByID.StoreName, iStoreID)
    End Function

    Public Function GetSignName(ByVal iSignID As Integer) As String
        Return GetTextByID(enumTextByID.SignName, iSignID)
    End Function

    Public Function GetSubTeamName(ByVal iSubTeam_No As Integer) As String
        Return GetTextByID(enumTextByID.SubTeamName, iSubTeam_No)
    End Function

    Public Function GetSubTeamMargin(ByVal iSubTeam_No As Integer) As Decimal
        Return GetTextByID(enumTextByID.SubTeamMargin, iSubTeam_No)
    End Function

    Public Sub LoadDiscount(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef bFreeItems As Boolean)
        logger.Debug("LoadDiscount Entry")

        Dim NewIndex As Integer
        Dim iLoop As Short

        cmbComboBox.Items.Clear()

        'Load all Unit Abbr into combo box.
        For iLoop = 0 To UBound(sDiscountType)
            If iLoop <> IIf(bFreeItems, 4, 3) Then
                NewIndex = cmbComboBox.Items.Add(sDiscountType(iLoop))
                VB6.SetItemData(cmbComboBox, NewIndex, iLoop)
            End If
        Next iLoop

        logger.Debug("LoadDiscount Exit")
    End Sub

    Public Function ItemInItemVendor(ByRef lItemId As Integer, ByRef lVendorID As Integer) As Boolean
        logger.Debug("ItemInItemVendor Entry with lItemId=" + lItemId.ToString() + ",lVendorID=" + lVendorID.ToString())

        Dim paramList As New DBParamList()

        paramList.Add(New DBParam("Item_Key", DBParamType.Int, lItemId))
        paramList.Add(New DBParam("Vendor_ID", DBParamType.Int, lVendorID))

        If GetDataReader("CheckItemInItemVendor", paramList).HasRows Then
            ItemInItemVendor = True
        Else
            ItemInItemVendor = False
        End If

        paramList = Nothing

        logger.Debug("ItemInItemVendor Exit")
    End Function

    Public Sub LoadDistSubTeam(ByRef cmbDistSubTeam As System.Windows.Forms.ComboBox)

        logger.Debug("LoadDistSubTeam Entry")

        cmbDistSubTeam.Items.Clear()

        'Load all Distribution subteams
        gRSRecordset = SQLOpenRecordSet("EXEC GetDistSubTeams ", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        CreateEmptyADORS_FromDAO(gRSRecordset, grsDistSubTeams)

        grsDistSubTeams.Open()

        Do While Not gRSRecordset.EOF
            CopyDAORecordToADORecord(gRSRecordset, grsDistSubTeams)
            gRSRecordset.MoveNext()
        Loop

        gRSRecordset.Close()
        gRSRecordset = Nothing

        Call PopulateDistSubTeams(cmbDistSubTeam)


        logger.Debug("LoadDistSubTeam Exit")

    End Sub

    Public Sub LoadItemChains(ByRef cmbItemChain As System.Windows.Forms.ComboBox)

        Call LoadCombo(cmbItemChain, "GetItemChains", "ItemChainDesc", "ItemChainID")

    End Sub

    Public Sub LoadItemUnitConversion()
        logger.Debug("LoadItemUnitConversion Entry")

        gRSRecordset = SQLOpenRecordSet("EXEC GetItemConversionAll ", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        CreateEmptyADORS_FromDAO(gRSRecordset, grsUnitConversion)

        grsUnitConversion.Open()

        Do While Not gRSRecordset.EOF
            CopyDAORecordToADORecord(gRSRecordset, grsUnitConversion)
            gRSRecordset.MoveNext()
        Loop

        gRSRecordset.Close()
        gRSRecordset = Nothing

        logger.Debug("LoadItemUnitConversion Exit")


    End Sub

    Public Sub PopulateDistSubTeams(ByRef cmbDistSubTeam As System.Windows.Forms.ComboBox)

        logger.Debug("PopulateDistSubTeams Entry")

        Dim lCurrentDist_No As Integer
        Dim NewIndex As Integer

        'Clear combo first
        cmbDistSubTeam.Items.Clear()

        ' Put distribution subteams into the combo
        If grsDistSubTeams.RecordCount > 0 Then
            NewIndex = cmbDistSubTeam.Items.Add("--All--")
            VB6.SetItemData(cmbDistSubTeam, NewIndex, -1)

            grsDistSubTeams.MoveFirst()
            lCurrentDist_No = grsDistSubTeams.Fields("DistSubTeam_No").Value
            NewIndex = cmbDistSubTeam.Items.Add(grsDistSubTeams.Fields("DistSubTeam_Name").Value)
            VB6.SetItemData(cmbDistSubTeam, NewIndex, grsDistSubTeams.Fields("DistSubTeam_No").Value)
            grsDistSubTeams.MoveNext()

            Do While Not grsDistSubTeams.EOF
                If Not (grsDistSubTeams.Fields("DistSubTeam_No").Value = lCurrentDist_No) Then
                    NewIndex = cmbDistSubTeam.Items.Add(grsDistSubTeams.Fields("DistSubTeam_Name").Value)
                    VB6.SetItemData(cmbDistSubTeam, NewIndex, grsDistSubTeams.Fields("DistSubTeam_No").Value)
                    lCurrentDist_No = grsDistSubTeams.Fields("DistSubTeam_No").Value
                End If
                grsDistSubTeams.MoveNext()

            Loop
        End If

        logger.Debug("PopulateDistSubTeams Exit")

    End Sub

    Public Sub PopulateDistSubTeamByRetailSubTeam(ByRef cmbSubTeam As System.Windows.Forms.ComboBox, ByRef lRetailSubTeam_No As Integer)

        logger.Debug("PopulateDistSubTeamByRetailSubTeam Entry")

        Dim NewIndex As Integer
        'Recordset is grouped by DistSubTeam_No

        'Clear combo first
        cmbSubTeam.Items.Clear()

        ' Put subteams into the combo
        If grsDistSubTeams.RecordCount > 0 Then
            grsDistSubTeams.MoveFirst()
            Do While Not grsDistSubTeams.EOF
                If (lRetailSubTeam_No = grsDistSubTeams.Fields("RetailSubTeam_No").Value) Then
                    NewIndex = cmbSubTeam.Items.Add(grsDistSubTeams.Fields("DistSubTeam_Name").Value)
                    VB6.SetItemData(cmbSubTeam, NewIndex, grsDistSubTeams.Fields("DistSubTeam_No").Value)
                End If

                grsDistSubTeams.MoveNext()
            Loop
        End If

        logger.Debug("PopulateDistSubTeamByRetailSubTeam exit")

    End Sub

    Public Function IsSubTeamDistributed(ByRef lRetailSubTeam_No As Integer) As Boolean

        logger.Debug("IsSubTeamDistributed Entry")

        Dim bIsDistributed As Boolean
        bIsDistributed = False

        If grsDistSubTeams.RecordCount > 0 Then
            grsDistSubTeams.MoveFirst()
            Do While Not grsDistSubTeams.EOF
                If (grsDistSubTeams.Fields("RetailSubTeam_No").Value = lRetailSubTeam_No) Then
                    bIsDistributed = True
                    Exit Do
                End If
                grsDistSubTeams.MoveNext()
            Loop
        End If

        IsSubTeamDistributed = bIsDistributed

        logger.Debug("IsSubTeamDistributed Exit")
    End Function

    Public Sub SetRetailSubTeam(ByRef lDistSubTeam_No As Integer, ByRef cmbRetailSubTeam As System.Windows.Forms.ComboBox)

        logger.Debug("SetRetailSubTeam Entry")

        If grsDistSubTeams.RecordCount > 0 Then
            grsDistSubTeams.MoveFirst()
            Do While Not grsDistSubTeams.EOF
                If (lDistSubTeam_No = grsDistSubTeams.Fields("DistSubTeam_No").Value) Then
                    cmbRetailSubTeam.Items.Add(New VB6.ListBoxItem(grsDistSubTeams.Fields("RetailSubTeam_Name").Value, grsDistSubTeams.Fields("RetailSubTeam_No").Value))
                    Exit Do
                End If
                grsDistSubTeams.MoveNext()
            Loop
        End If

        If (cmbRetailSubTeam.Items.Count = 1) Then cmbRetailSubTeam.SelectedIndex = 0

        logger.Debug("SetRetailSubTeam Exit")


    End Sub

    Public Sub LoadZone(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Dim NewIndex As Integer

        cmbComboBox.Items.Clear()

        'Load all Unit Abbr into combo box.
        gRSRecordset = SQLOpenRecordSet("EXEC GetZones", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Do While Not gRSRecordset.EOF
            NewIndex = cmbComboBox.Items.Add(gRSRecordset.Fields("Zone_Name").Value)
            VB6.SetItemData(cmbComboBox, NewIndex, gRSRecordset.Fields("Zone_ID").Value)
            gRSRecordset.MoveNext()
        Loop

        gRSRecordset.Close()
        gRSRecordset = Nothing

    End Sub
    Public Sub LoadJurisdiction(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        Dim NewIndex As Integer

        cmbComboBox.Items.Clear()

        'Load all Unit Abbr into combo box.
        gRSRecordset = SQLOpenRecordSet("EXEC GetStoreJurisdictions", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Do While Not gRSRecordset.EOF
            NewIndex = cmbComboBox.Items.Add(gRSRecordset.Fields("StoreJurisdictionDesc").Value)
            VB6.SetItemData(cmbComboBox, NewIndex, gRSRecordset.Fields("StoreJurisdictionID").Value)
            gRSRecordset.MoveNext()
        Loop

        gRSRecordset.Close()
        gRSRecordset = Nothing

    End Sub
    Public Sub LoadVendZones(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByVal lVendor_ID As Integer)
        Dim NewIndex As Integer
        logger.Debug("LoadVendZones Entry")
        cmbComboBox.Items.Clear()

        'Load all Unit Abbr into combo box.
        gRSRecordset = SQLOpenRecordSet("EXEC GetVendZones NULL," & CStr(lVendor_ID), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Do While Not gRSRecordset.EOF
            NewIndex = cmbComboBox.Items.Add(gRSRecordset.Fields("Zone_Name").Value)
            VB6.SetItemData(cmbComboBox, NewIndex, gRSRecordset.Fields("Zone_ID").Value)
            gRSRecordset.MoveNext()
        Loop

        gRSRecordset.Close()
        gRSRecordset = Nothing
        logger.Debug("LoadVendZones Exit")
    End Sub
    Public Sub LoadStoreZones(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByVal lStore_No As Integer)
        Dim NewIndex As Integer

        logger.Debug("LoadStoreZones Entry")

        cmbComboBox.Items.Clear()

        'Load all Unit Abbr into combo box.
        gRSRecordset = SQLOpenRecordSet("EXEC GetVendZones " & CStr(lStore_No) & ",NULL", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Do While Not gRSRecordset.EOF
            NewIndex = cmbComboBox.Items.Add(gRSRecordset.Fields("Zone_Name").Value)
            VB6.SetItemData(cmbComboBox, NewIndex, gRSRecordset.Fields("Zone_ID").Value)
            gRSRecordset.MoveNext()
        Loop

        gRSRecordset.Close()
        gRSRecordset = Nothing

        logger.Debug("LoadStoreZones Exit")

    End Sub

    Public Function CheckAllStoreSelectionEnabled() As Boolean

        If bEnableAllStoresSelection Is Nothing Then
            bEnableAllStoresSelection = InstanceDataDAO.IsFlagActive("EnableAllStoresSelection")
        End If

        Return bEnableAllStoresSelection

    End Function

    Public Sub GetPurchaseOrderHeader(ByRef lOrderHeaderID As Integer)

        logger.Debug("GetPurchaseOrderHeader Entry")


        Dim rsReport As ADODB.Recordset = Nothing
        Dim sCM As String
        sCM = String.Empty

        ' RE 10/4/2007: This sub does not work
        MsgBox("Global.vb GetPurchaseOrderHeader(): This Sub currently does not work and needs to be fixed. As of: 10/4/2007", MsgBoxStyle.Exclamation)

        gDBReport.BeginTrans()

        gDBReport.Execute("DELETE * FROM PURCHASEORDERHEADER")

        Try
            rsReport = New ADODB.Recordset
            rsReport.Open("PurchaseOrderHeader", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)

            Try
                gRSRecordset = SQLOpenRecordSet("EXEC GetPOHeader " & lOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                rsReport.AddNew()
                rsReport.Fields("PONumber").Value = lOrderHeaderID
                rsReport.Fields("OrderDate").Value = gRSRecordset.Fields("OrderDate").Value
                rsReport.Fields("VendorCompanyName").Value = gRSRecordset.Fields("CompanyName").Value
                rsReport.Fields("VendorAddress_Line_1").Value = gRSRecordset.Fields("Address_Line_1").Value
                rsReport.Fields("VendorAddress_Line_2").Value = gRSRecordset.Fields("Address_Line_2").Value
                rsReport.Fields("VendorCity").Value = gRSRecordset.Fields("City").Value
                rsReport.Fields("VendorState").Value = gRSRecordset.Fields("State").Value
                rsReport.Fields("VendorZip").Value = gRSRecordset.Fields("Zip_Code").Value
                rsReport.Fields("VendorCountry").Value = gRSRecordset.Fields("Country").Value
                rsReport.Fields("VendorPhone").Value = gRSRecordset.Fields("Phone").Value
                rsReport.Fields("VendorFax").Value = gRSRecordset.Fields("Fax").Value
                rsReport.Fields("VendorVendor_Key").Value = gRSRecordset.Fields("Vendor_Key").Value
                If Not IsDBNull(gRSRecordset.Fields("OriginalOrderHeader_ID").Value) Then
                    rsReport.Fields("OrderHeaderDesc").Value = "CM OF PO " & gRSRecordset.Fields("OriginalOrderHeader_ID").Value & vbCrLf & gRSRecordset.Fields("OrderHeaderDesc").Value
                Else
                    rsReport.Fields("OrderHeaderDesc").Value = gRSRecordset.Fields("OrderHeaderDesc").Value
                End If
                rsReport.Fields("QuantityDiscount").Value = gRSRecordset.Fields("QuantityDiscount").Value
                rsReport.Fields("DiscountType").Value = gRSRecordset.Fields("DiscountType").Value
                rsReport.Fields("UserName").Value = gRSRecordset.Fields("UserName").Value & ""
                rsReport.Fields("TransferDeptName").Value = gRSRecordset.Fields("TransferSubTeamName").Value & ""
                rsReport.Fields("TransferToDeptName").Value = gRSRecordset.Fields("TransferToSubTeamName").Value & ""
                rsReport.Fields("Return_Order").Value = gRSRecordset.Fields("Return_Order").Value

                If IsDBNull(gRSRecordset.Fields("Expected_Date").Value) Then
                    rsReport.Fields("Expected_Date").Value = ""
                Else
                    rsReport.Fields("Expected_Date").Value = String.Format(gRSRecordset.Fields("Expected_Date").Value, "MM/DD/YYYY")
                End If
                rsReport.Fields("ReceiveCompanyName").Value = gRSRecordset.Fields("RLCompanyName").Value
                rsReport.Fields("ReceiveAddress_Line_1").Value = gRSRecordset.Fields("RLAddress_Line_1").Value
                rsReport.Fields("ReceiveAddress_Line_2").Value = gRSRecordset.Fields("RLAddress_Line_2").Value
                rsReport.Fields("ReceiveCity").Value = gRSRecordset.Fields("RLCity").Value
                rsReport.Fields("ReceiveState").Value = gRSRecordset.Fields("RLState").Value
                rsReport.Fields("ReceiveZip").Value = gRSRecordset.Fields("RLZip_Code").Value
                rsReport.Fields("ReceiveCountry").Value = gRSRecordset.Fields("RLCountry").Value
                rsReport.Fields("ReceivePhone").Value = gRSRecordset.Fields("RLPhone").Value
                rsReport.Fields("ReceiveFax").Value = gRSRecordset.Fields("RLFax").Value

                rsReport.Fields("PurchaseCompanyName").Value = gRSRecordset.Fields("PLCompanyName").Value
                rsReport.Fields("PurchaseAddress_Line_1").Value = gRSRecordset.Fields("PLAddress_Line_1").Value
                rsReport.Fields("PurchaseAddress_Line_2").Value = gRSRecordset.Fields("PLAddress_Line_2").Value
                rsReport.Fields("PurchaseCity").Value = gRSRecordset.Fields("PLCity").Value
                rsReport.Fields("PurchaseState").Value = gRSRecordset.Fields("PLState").Value
                rsReport.Fields("PurchaseZip").Value = gRSRecordset.Fields("PLZip_Code").Value
                rsReport.Fields("PurchaseCountry").Value = gRSRecordset.Fields("PLCountry").Value
                rsReport.Fields("PurchasePhone").Value = gRSRecordset.Fields("PLPhone").Value
                rsReport.Fields("PurchaseFax").Value = gRSRecordset.Fields("PLFax").Value
                rsReport.Fields("OrderType_ID").Value = gRSRecordset.Fields("OrderType_ID").Value
                rsReport.Fields("ProductType_ID").Value = gRSRecordset.Fields("ProductType_ID").Value

            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                End If
            End Try

            '-- Add CM's to order
            Try
                gRSRecordset = SQLOpenRecordSet("EXEC GetCreditOrderList " & lOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If Not gRSRecordset.EOF Then

                    While Not gRSRecordset.EOF
                        sCM = sCM & gRSRecordset.Fields("ReturnOrderHeader_id").Value
                        gRSRecordset.MoveNext()
                        If Not gRSRecordset.EOF Then sCM = sCM & ", "
                    End While

                    rsReport.Fields("OrderHeaderDesc").Value = "CM ON PO(s) " & sCM & vbCrLf & rsReport.Fields("OrderHeaderDesc").Value

                End If
            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                End If
            End Try

            rsReport.Update()
        Finally
            If rsReport IsNot Nothing Then
                rsReport.Close()
                rsReport = Nothing
            End If
        End Try

        gDBReport.CommitTrans()
        If gJetFlush IsNot Nothing Then
            gJetFlush.RefreshCache(gDBReport)
        End If

        logger.Debug("GetPurchaseOrderHeader Exit")

    End Sub

    Public Function ValidateKeyPressEvent(ByRef AsciiCode As Short, ByRef ControlType As String, ByVal TB As TextBox, ByRef MinVal As Decimal, ByRef MaxVal As Decimal, ByRef DecimalPortionLength As Integer) As Integer

        logger.Debug("ValidateKeyPressEvent Entry")

        Dim i As Short
        Dim J As Short
        Dim iMonth As Short
        Dim iDay As Short
        Dim iYear As Short
        Dim sDate As String

        On Error GoTo ErrExit

        If TB.Text.Length = 0 AndAlso AsciiCode = 8 Then
            ValidateKeyPressEvent = 0
            Exit Function
        End If

        Select Case UCase(ControlType)

            Case "ALPHANUMERICSTRING"

                ' If the data in the textbox is highlighted,
                ' then set the text property to a blank string.
                If TB.SelectionStart = 0 AndAlso TB.SelectionLength = Len(TB.Text) Then
                    TB.Text = String.Empty
                End If

                ' Only allows characters in the following range.
                ' 0 - 9, A - Z and a - z.
                If AsciiCode <> 8 AndAlso AsciiCode <> 32 AndAlso (AsciiCode < 48 OrElse AsciiCode > 122) Then
                    ValidateKeyPressEvent = 0
                    Exit Function
                End If

                If AsciiCode <> 8 AndAlso AsciiCode <> 32 AndAlso ((AsciiCode > 57 AndAlso AsciiCode < 65) OrElse (AsciiCode > 90 AndAlso AsciiCode < 97)) Then
                    ValidateKeyPressEvent = 0
                    Exit Function
                End If

            Case "DATE", "DATETIME"

                On Error GoTo DateTimeErrExit

                ' Only numerical digits allowed.
                ' With the exception of Backspace/Delete which has a ascii value of 8.
                If (AsciiCode < 48 OrElse AsciiCode > 57) AndAlso AsciiCode <> 8 Then
                    ValidateKeyPressEvent = 0
                    Exit Function
                End If

                ' If the data in the textbox is highlighted,
                ' then set the text property toa blank string.
                If TB.SelectionStart = 0 AndAlso TB.SelectionLength = Len(TB.Text) Then
                    TB.Text = String.Empty
                End If

                If AsciiCode = 8 Then

                    If TB.Text.Length > 0 Then
                        TB.Text = Left(TB.Text, Len(TB.Text) - 1)

                        If Right(TB.Text, 1) = "/" OrElse Right(TB.Text, 1) = ":" Then
                            TB.Text = Left(TB.Text, Len(TB.Text) - 1)
                        End If
                    End If
                    TB.SelectionStart = Len(TB.Text)
                    TB.SelectionLength = 0

                    ValidateKeyPressEvent = 0

                    Exit Function

                End If
StartOver:

                sDate = CStr(TB.Text + Chr(AsciiCode))

                i = InStr(sDate, "/")

                If i > 0 Then

                    iMonth = CShort(Left(sDate, i - 1))

                    J = InStr(i + 1, sDate, "/")

                    If J > 0 Then

                        iDay = CShort(Mid(sDate, i + 1, J - (i + 1)))
                        iYear = CShort(Mid(sDate, J + 1, 4))

                    Else

                        iDay = CShort(Mid(sDate, i + 1))
                        iYear = -1

                    End If

                Else

                    iMonth = CShort(sDate)
                    iDay = -1
                    iYear = -1

                End If

                Select Case Len(sDate)

                    Case 1

                        If iMonth > 1 Then
                            sDate = "0" & sDate & "/" 'Example: 05/
                        End If

                    Case 2

                        If iMonth > 12 OrElse iMonth = 0 Then
                            sDate = TB.Text 'Example: sDate = 15/ or sDate = 00/
                        Else
                            sDate = sDate & "/" 'Example: 12/
                        End If

                    Case 3

                        TB.Text = TB.Text + "/"
                        GoTo StartOver

                    Case 4

                        If iMonth = 2 Then

                            If iDay > 2 Then
                                sDate = CStr(TB.Text + "0" + Chr(AsciiCode) + "/")
                            End If

                        Else

                            If iDay > 3 Then
                                sDate = CStr(TB.Text + "0" + Chr(AsciiCode) + "/")
                            End If

                        End If

                    Case 5

                        Select Case iMonth

                            Case 1, 3, 5, 7, 8, 10, 12

                                If iDay = 0 OrElse iDay > 31 Then
                                    sDate = TB.Text
                                Else
                                    sDate = sDate & "/"
                                End If

                            Case 2

                                If iDay = 0 OrElse iDay > 29 Then
                                    sDate = TB.Text
                                Else
                                    sDate = sDate & "/"
                                End If

                            Case 4, 6, 9, 11

                                If iDay = 0 OrElse iDay > 30 Then
                                    sDate = TB.Text
                                Else
                                    sDate = sDate & "/"
                                End If

                        End Select

                    Case 6 ' Slash entered by program.

                        TB.Text = TB.Text + "/"
                        GoTo StartOver

                    Case 7

                        Select Case iYear

                            Case 0
                                sDate = CStr(TB.Text + "200")

                            Case 1
                                sDate = CStr(TB.Text + "201")

                            Case 2
                                sDate = CStr(TB.Text + "202")

                            Case 3
                                sDate = CStr(TB.Text + "193")

                            Case 4
                                sDate = CStr(TB.Text + "194")

                            Case 5
                                sDate = CStr(TB.Text + "195")

                            Case 6
                                sDate = CStr(TB.Text + "196")

                            Case 7
                                sDate = CStr(TB.Text + "197")

                            Case 8
                                sDate = CStr(TB.Text + "198")

                            Case 9
                                sDate = CStr(TB.Text + "199")

                        End Select

                    Case 8

                        If (AsciiCode) < 48 OrElse (AsciiCode) > 57 Then
                            sDate = TB.Text
                        End If

                    Case 9

                        If (AsciiCode) < 48 OrElse (AsciiCode) > 57 Then
                            sDate = TB.Text
                        End If

                    Case 10

                        If iMonth = 2 AndAlso iDay = 29 Then
                            If iYear Mod 4 <> 0 Then
                                sDate = TB.Text
                            End If
                        End If

                        If ControlType = "DateTime" Then
                            sDate = sDate & " "
                        End If

                    Case 11

                        If AsciiCode < 48 OrElse AsciiCode > 50 Then
                            sDate = TB.Text
                        Else
                            sDate = TB.Text & " " & Chr(AsciiCode)
                        End If

                    Case 12

                        If AsciiCode < 48 OrElse AsciiCode > 50 Then
                            sDate = TB.Text
                        End If

                    Case 13

                        If AsciiCode < 48 OrElse AsciiCode > 57 Then
                            sDate = TB.Text
                        Else
                            If CShort(Right(sDate, 2)) > 23 Then
                                sDate = TB.Text
                            Else
                                sDate = sDate & ":"
                            End If
                        End If

                    Case 14

                        If CShort(Mid(sDate, Len(sDate) - 3, 2)) > 23 Then
                            sDate = TB.Text
                        Else
                            sDate = TB.Text & ":"
                        End If

                    Case 15

                        If AsciiCode < 48 OrElse AsciiCode > 53 Then
                            sDate = TB.Text
                        End If

                    Case 16

                        If AsciiCode < 48 OrElse AsciiCode > 57 Then
                            sDate = TB.Text
                        End If

                End Select

                TB.Text = sDate

                'Do not allow them to exceed maxlength of the field.
                If Len(TB.Text) > TB.MaxLength Then TB.Text = TB.Text.Substring(0, TB.MaxLength)

                TB.SelectionStart = Len(TB.Text)
                TB.SelectionLength = 0

                ValidateKeyPressEvent = 0

                Exit Function

DateTimeErrExit:

                TB.Text = ""
                Exit Function

            Case "LIMITEDSTRING"

                ' Only allows characters in the following range.
                ' A-Z, 0-9, /, \, ., -, ", , %, (, ), $, £, &, #, ;, :, +, =, ?, `, {, }, !, *, ’, _, ^
                ' backspace (BS), space (32), Del, Ctrl (for ctrl+v)

                If AsciiCode <= 7 OrElse (AsciiCode >= 9 AndAlso AsciiCode <= 21) OrElse (AsciiCode >= 23 AndAlso AsciiCode <= 31) OrElse AsciiCode = 39 OrElse AsciiCode = 60 OrElse AsciiCode = 62 OrElse AsciiCode = 64 OrElse AsciiCode = 91 _
                      OrElse AsciiCode = 93 OrElse AsciiCode = 124 OrElse (AsciiCode >= 126 AndAlso AsciiCode <= 155) OrElse AsciiCode >= 157 Then
                    ValidateKeyPressEvent = 0
                    Exit Function
                End If

            Case "NUMBER", "CURRENCY", "EXTCURRENCY", "-NUMBER", "-CURRENCY", "-EXTCURRENCY"

                If Mid(ControlType, 1, 1) = "-" Then
                    ControlType = Mid(ControlType, 2)
                    If AsciiCode = Asc("-") AndAlso TB.Text = "" Then
                        TB.Text = "-"
                        TB.SelectionStart = Len(TB.Text)
                        TB.SelectionLength = 0
                        ValidateKeyPressEvent = 0
                        Exit Function
                    End If
                End If

                Select Case UCase(ControlType)
                    Case "CURRENCY" : DecimalPortionLength = 2
                    Case "EXTCURRENCY" : DecimalPortionLength = 4
                End Select


                ' Only numerical digits allowed.  With the exception of Backspace/Delete
                ' which has a ascii value of 8, and decimal point which has a ascii value of 46.
                If (AsciiCode < 48 OrElse AsciiCode > 57) AndAlso AsciiCode <> 8 AndAlso AsciiCode <> 46 AndAlso AsciiCode <> 22 AndAlso AsciiCode <> 3 Then

                    ValidateKeyPressEvent = 0
                    Exit Function

                End If



                ' If no decimal portion is specified, then DO NOT allow the user
                ' to input a decimal point.
                If DecimalPortionLength = 0 AndAlso AsciiCode = 46 Then
                    ValidateKeyPressEvent = 0
                    Exit Function
                End If

                ' If the data in the textbox is highlighted,
                ' then set the text property to a blank string.
                If TB.SelectionStart = 0 AndAlso TB.SelectionLength = Len(TB.Text) Then
                    TB.Text = ""
                End If

                ' Only a decimal point allowed.
                If AsciiCode = 46 Then

                    If InStr(TB.Text, ".") > 0 Then
                        ValidateKeyPressEvent = 0
                        Exit Function
                    End If

                    If Len(TB.Text) = 0 Then
                        TB.Text = "0."
                        TB.SelectionStart = Len(TB.Text)
                        TB.SelectionLength = 0
                        ValidateKeyPressEvent = 0
                        Exit Function
                    End If

                End If

                ' Make sure that the value does not exceeds the maximum value allowed and
                ' not exceeds the minimum value allowed, as long as the asciicode was not
                ' a backspace key.
                If MinVal <> MaxVal AndAlso AsciiCode > 8 AndAlso Len(TB.Text) > 0 Then
                    If CDec(TB.Text & Chr(AsciiCode)) < MinVal OrElse CDec(TB.Text & Chr(AsciiCode)) > MaxVal Then
                        ValidateKeyPressEvent = 0
                        Exit Function
                    End If
                End If

                ' Make sure that the number of digits pass the decimal point
                ' does not exceeds the maximum number of digits allowed.
                ' DecimalPortionLength lets the system know the maximum of digits
                ' to allows for the decimal portion.
                i = InStr(TB.Text, ".")
                If i > 0 Then
                    If Len(TB.Text) - i + 1 > DecimalPortionLength Then
                        If AsciiCode <> 8 Then
                            ValidateKeyPressEvent = 0
                            Exit Function
                        End If
                    End If
                End If

            Case "PHONENUMBER"

                ' Only numerical digits allowed.
                ' With the exception of Backspace/Delete which has a ascii value of 8.
                If (AsciiCode < 48 OrElse AsciiCode > 57) AndAlso AsciiCode <> 8 Then
                    ValidateKeyPressEvent = 0
                    Exit Function
                End If

                If AsciiCode = 8 Then

                    ' '(000) 000-' is exactly ten characters in length.
                    ' Remove one more extra character which will delete the hyphen character.
                    If Len(TB.Text) = 10 Then
                        TB.Text = Left(TB.Text, Len(TB.Text) - 1)
                    End If

                    ' '(000) 0' is exactly six characters in lenght.
                    ' Remove two more extra characters which will delete the
                    ' space and open paraenthsis.
                    If Len(TB.Text) = 6 Then
                        TB.Text = Left(TB.Text, Len(TB.Text) - 2)
                    End If

                    ' '(0' is exactly two characters in length.'
                    ' Remove the numerical digit as well as the open paraenthsis.
                    If Len(TB.Text) = 2 Then
                        TB.Text = ""
                    End If

                Else

                    ' If the data in the textbox is highlighted,
                    ' then set the text property to a blank string.
                    If TB.SelectionStart = 0 AndAlso TB.SelectionLength = Len(TB.Text) Then
                        TB.Text = "("
                    End If

                    ' '(000' is exactly four characters in length.
                    ' Add the extra ')' and a space to the end of the text.
                    If Len(TB.Text) = 4 Then
                        TB.Text = TB.Text + ") "
                    End If

                    ''(000) 000' is exactly nine characters in length.
                    ' Add the extra '-' to the end of the text.
                    If Len(TB.Text) = 9 Then
                        TB.Text = TB.Text + "-"
                    End If

                End If

                'Do not allow them to exceed maxlength of the field.
                If Len(TB.Text) > TB.MaxLength Then TB.Text = TB.Text.Substring(0, TB.MaxLength)

                TB.SelectionStart = Len(TB.Text)
                TB.SelectionLength = 0

            Case "STRING", "POSSTRING"

                Select Case True
                    Case AsciiCode = 39
                        ' Currency-Quotation Mark NOT ALLOWED.
                        ' Replace the character with this '`'.
                        ValidateKeyPressEvent = 96
                        Exit Function
                    Case (AsciiCode = 44 AndAlso UCase(ControlType) = "POSTRING"), AsciiCode = 124
                        'Don't allow comma for the POS
                        'Don't allow | for any string
                        ValidateKeyPressEvent = 0
                        Exit Function
                End Select


                ' #### Commented out for Bug Fix: 989 ####################

                '' If the data in the textbox is highlighted,
                '' then set the text property toa blank string.
                'If TB.SelectionStart = 0 And TB.SelectionLength = Len(TB.Text) Then
                '    TB.Text = String.Empty
                'End If

                ' ########################################################
        End Select

        ValidateKeyPressEvent = AsciiCode

        logger.Debug("ValidateKeyPressEvent Exit")


        Exit Function

ErrExit:

        ValidateKeyPressEvent = 0
        logger.Debug("ValidateKeyPressEvent Exit(ErrExit:)")
    End Function
    Public Sub HighlightText(ByRef TB As TextBox)
        TB.SelectionStart = 0
        TB.SelectionLength = Len(TB.Text)
    End Sub

    Public Sub HighLightText(ByRef TB As MaskedTextBox)
        TB.SelectionStart = 0
        TB.SelectionLength = Len(TB.Text)
    End Sub

    Public Sub SetActive(ByRef txtTextbox As TextBox, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        txtTextbox.Visible = Not bBlank
        txtTextbox.ReadOnly = Not bActive
        If bActive Then
            txtTextbox.BackColor = SystemColors.Window
        Else
            txtTextbox.BackColor = SystemColors.ControlLight
        End If
    End Sub

    Public Sub SetActive(ByRef mskdTextbox As MaskedTextBox, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        mskdTextbox.Visible = Not bBlank
        mskdTextbox.ReadOnly = Not bActive
        If bActive Then
            mskdTextbox.BackColor = SystemColors.Window
        Else
            mskdTextbox.BackColor = SystemColors.ControlLight
        End If
    End Sub

    Public Sub SetActive(ByRef txtTextbox As UltraWinEditors.UltraNumericEditor, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        txtTextbox.Visible = Not bBlank
        txtTextbox.ReadOnly = Not bActive
        If bActive Then
            txtTextbox.BackColor = SystemColors.Window
        Else
            txtTextbox.BackColor = SystemColors.ControlLight
        End If
    End Sub

    Public Sub SetActive(ByRef numNumericUpDwn As NumericUpDown, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        numNumericUpDwn.Visible = Not bBlank
        numNumericUpDwn.Enabled = bActive
    End Sub

    Public Sub SetActive(ByRef cmbComboBox As ComboBox, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        cmbComboBox.Visible = Not bBlank
        cmbComboBox.BackColor = SystemColors.Window 'Make sure the backcolor is not controllight in all cases
        cmbComboBox.Enabled = bActive
    End Sub
    Public Sub SetActive(ByRef ucmbComboBox As Infragistics.Win.UltraWinGrid.UltraCombo, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        ucmbComboBox.Visible = Not bBlank
        ucmbComboBox.BackColor = SystemColors.Window 'Make sure the backcolor is not controllight in all cases
        ucmbComboBox.Enabled = bActive
    End Sub
    Public Sub SetActive(ByRef btnButton As Button, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        btnButton.Visible = Not bBlank
        btnButton.Enabled = bActive
    End Sub
    Public Sub SetActive(ByRef chkCheckbox As CheckBox, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        chkCheckbox.Visible = Not bBlank
        chkCheckbox.Enabled = bActive
    End Sub
    Public Sub SetActive(ByRef optRadioButton As RadioButton, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        optRadioButton.Visible = Not bBlank
        optRadioButton.Enabled = bActive
    End Sub
    Public Sub SetActive(ByRef fraGroupBox As GroupBox, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        fraGroupBox.Visible = Not bBlank
        fraGroupBox.Enabled = bActive
    End Sub
    Public Sub SetActive(ByRef lblLabel As Label, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        lblLabel.Visible = Not bBlank
        lblLabel.Enabled = bActive
    End Sub
    Public Sub SetActive(ByRef dtpDateTimePicker As DateTimePicker, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        dtpDateTimePicker.Visible = Not bBlank
        dtpDateTimePicker.Enabled = (bActive)
    End Sub
    Public Sub SetActive(ByRef dtpDateTimeEditor As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
        dtpDateTimeEditor.Visible = Not bBlank
        dtpDateTimeEditor.Enabled = (bActive)
    End Sub
    'Public Sub SetActive(ByRef cmbComboBox As Infragistics.Win.UltraWinGrid.UltraCombo, ByVal bActive As Boolean, Optional ByRef bBlank As Boolean = False)
    '    cmbComboBox.Visible = Not bBlank
    '    cmbComboBox.Enabled = (bActive)
    'End Sub

    Sub SetCombo(ByRef cmbField As System.Windows.Forms.ComboBox, ByRef sValue As String)


        logger.Debug("SetCombo Entry")

        Dim Locked As Boolean
        Dim iLoop As Integer

        If IsNothing(sValue) Then sValue = String.Empty

        'Save the state of the combo (Locked or not).
        Locked = Not cmbField.Enabled
        cmbField.Enabled = True

        If sValue = String.Empty Then
            cmbField.SelectedIndex = -1
        Else
            For iLoop = 0 To cmbField.Items.Count - 1
                '-- See if its the right data
                If VB6.GetItemData(cmbField, iLoop) = sValue Then
                    '-- if so then set and exit
                    cmbField.SelectedIndex = iLoop
                    Exit For
                End If
            Next iLoop
        End If

        'Set the state back.
        cmbField.Enabled = Not Locked

        logger.Debug("SetCombo Exit")


    End Sub
    Sub SetCombo(ByRef cmbField As System.Windows.Forms.ComboBox, ByRef iValue As Integer)

        logger.Debug("SetCombo Entry")

        Dim Locked As Boolean
        Dim iLoop As Integer
        Dim retval As Integer


        'Save the state of the combo (Locked or not).
        Locked = Not cmbField.Enabled
        cmbField.Enabled = True

        For iLoop = 0 To cmbField.Items.Count - 1
            '-- See if its the right data
            retval = VB6.GetItemData(cmbField, iLoop)
            If retval = iValue Then
                '-- if so then set and exit
                cmbField.SelectedIndex = iLoop
                Exit For
            End If
        Next iLoop

        'Set the state back.
        cmbField.Enabled = Not Locked

        logger.Debug("SetCombo Exit")


    End Sub

    Sub SetCombo(ByRef cmbField As System.Windows.Forms.ComboBox, ByRef lValue As Long)

        logger.Debug("SetCombo Entry")

        Dim Locked As Boolean
        Dim iLoop As Integer

        'Save the state of the combo (Locked or not).
        Locked = Not cmbField.Enabled
        cmbField.Enabled = True

        For iLoop = 0 To cmbField.Items.Count - 1
            '-- See if its the right data
            If VB6.GetItemData(cmbField, iLoop) = lValue Then
                '-- if so then set and exit
                cmbField.SelectedIndex = iLoop
                Exit For
            End If
        Next iLoop

        'Set the state back.
        cmbField.Enabled = Not Locked

        logger.Debug("SetCombo Exit")


    End Sub
    Sub SetCombo(ByRef cmbField As System.Windows.Forms.ComboBox, ByRef hValue As Short)

        logger.Debug("SetCombo Entry")

        Dim Locked As Boolean
        Dim iLoop As Integer

        'Save the state of the combo (Locked or not).
        Locked = Not cmbField.Enabled
        cmbField.Enabled = True

        For iLoop = 0 To cmbField.Items.Count - 1
            '-- See if its the right data
            If VB6.GetItemData(cmbField, iLoop) = hValue Then
                '-- if so then set and exit
                cmbField.SelectedIndex = iLoop
                Exit For
            End If
        Next iLoop

        'Set the state back.
        cmbField.Enabled = Not Locked

        logger.Debug("SetCombo Exit")


    End Sub
    Sub SetCombo(ByRef cmbField As System.Windows.Forms.ComboBox, ByRef dValue As Double)

        logger.Debug("SetCombo Entry")
        Dim Locked As Boolean
        Dim iLoop As Integer

        'Save the state of the combo (Locked or not).
        Locked = Not cmbField.Enabled
        cmbField.Enabled = True

        For iLoop = 0 To cmbField.Items.Count - 1
            '-- See if its the right data
            If VB6.GetItemData(cmbField, iLoop) = dValue Then
                '-- if so then set and exit
                cmbField.SelectedIndex = iLoop
                Exit For
            End If
        Next iLoop

        'Set the state back.
        cmbField.Enabled = Not Locked

        logger.Debug("SetCombo Exit")

    End Sub
    Sub SetComboNew(ByRef cmbField As System.Windows.Forms.ComboBox, ByRef dValue As Double)

        logger.Debug("SetCombo Entry")
        Dim Locked As Boolean
        'Dim iLoop As Integer

        'Save the state of the combo (Locked or not).
        Locked = Not cmbField.Enabled
        cmbField.Enabled = True

        If dValue > 0 Then
            cmbField.SelectedValue = dValue
        Else
            cmbField.SelectedIndex = -1
        End If

        'Set the state back.
        cmbField.Enabled = Not Locked

        logger.Debug("SetCombo Exit")

    End Sub
    Sub SetCombo(ByRef cmbField As System.Windows.Forms.ComboBox, ByVal obj As Object)

        logger.Debug("SetCombo Entry")
        Dim Locked As Boolean
        Dim iLoop As Integer

        'Save the state of the combo (Locked or not).
        Locked = Not cmbField.Enabled
        cmbField.Enabled = True

        If IsDBNull(obj) Then
            cmbField.SelectedIndex = -1
        Else
            For iLoop = 0 To cmbField.Items.Count - 1
                '-- See if its the right data
                If VB6.GetItemData(cmbField, iLoop) = CInt(obj) Then
                    '-- if so then set and exit
                    cmbField.SelectedIndex = iLoop
                    Exit For
                End If
            Next iLoop
        End If

        'Set the state back.
        cmbField.Enabled = Not Locked

        logger.Debug("SetCombo Exit")

    End Sub
    Sub SetCombo(ByRef cmbField As Infragistics.Win.UltraWinGrid.UltraCombo, ByRef iValue As Integer)

        logger.Debug("SetCombo Entry")

        Dim Locked As Boolean

        'Save the state of the combo (Locked or not).
        Locked = Not cmbField.Enabled
        cmbField.Enabled = True

        For Each r As UltraGridRow In cmbField.Rows
            If r.Cells("Store_No").Value.ToString() = iValue.ToString() Then
                cmbField.Text = r.Cells("StoreAbbr").Value.ToString()
                Exit For
            End If
        Next

        'Set the state back.
        cmbField.Enabled = Not Locked

        logger.Debug("SetCombo Exit")


    End Sub
    Function ComboEnabledValue(ByRef cmbCombo As System.Windows.Forms.ComboBox) As String


        logger.Debug("ComboEnabledValue Entry")

        If (cmbCombo.SelectedIndex = -1) Or (cmbCombo.Enabled = False) Then
            ComboEnabledValue = "NULL"
        Else
            ComboEnabledValue = CStr(VB6.GetItemData(cmbCombo, cmbCombo.SelectedIndex))
        End If

        logger.Debug("ComboEnabledValue Exit")

    End Function
    Function ComboValue(ByRef cmbField As System.Windows.Forms.ComboBox) As String

        logger.Debug("ComboValue Entry")

        If cmbField.SelectedIndex = -1 Then
            ComboValue = "NULL"
        Else
            ComboValue = CStr(VB6.GetItemData(cmbField, cmbField.SelectedIndex))
        End If

        logger.Debug("ComboValue Exit")


    End Function

    Function ComboValue(ByRef cmbField As Infragistics.Win.UltraWinGrid.UltraCombo) As String

        logger.Debug("ComboValue Entry")

        ComboValue = String.Empty

        If cmbField.CheckedRows.Count <= 0 Then
            ComboValue = "NULL"
        Else
            For Each r As UltraGridRow In cmbField.CheckedRows
                ComboValue += r.Cells(cmbField.ValueMember).Value.ToString() + "|"
            Next
            ComboValue = Left(ComboValue, Len(ComboValue) - 1)
        End If

        logger.Debug("ComboValue Exit")


    End Function

    Function ComboVal(ByRef cmbField As System.Windows.Forms.ComboBox) As Integer

        logger.Debug("ComboVal Entry")

        If cmbField.SelectedIndex > -1 Then
            ComboVal = VB6.GetItemData(cmbField, cmbField.SelectedIndex)
        End If

        logger.Debug("ComboVal Exit")


    End Function

    Function CrystalValue(ByRef cmbField As System.Windows.Forms.ComboBox) As String

        logger.Debug("CrystalValue Entry")

        If cmbField.SelectedIndex = -1 Then
            CrystalValue = "crwnull"
        Else
            CrystalValue = CStr(VB6.GetItemData(cmbField, cmbField.SelectedIndex))
        End If

        logger.Debug("CrystalValue Exit")


    End Function

    Sub ReplicateCombo(ByVal cmbSource As System.Windows.Forms.ComboBox, ByRef cmbTarget As System.Windows.Forms.ComboBox)

        logger.Debug("ReplicateCombo Entry")

        Dim iLoop As Short

        cmbTarget.Items.Clear()
        For iLoop = 0 To cmbSource.Items.Count - 1
            cmbTarget.Items.Add(New VB6.ListBoxItem(VB6.GetItemString(cmbSource, iLoop), VB6.GetItemData(cmbSource, iLoop)))
        Next iLoop

        logger.Debug("ReplicateCombo Exit")


    End Sub

    Sub ReplicateUltraCombo(ByVal cmbSource As Infragistics.Win.UltraWinGrid.UltraCombo, ByRef cmbTarget As Infragistics.Win.UltraWinGrid.UltraCombo)

        logger.Debug("ReplicateUltraCombo Entry")

        cmbTarget.DataSource = cmbSource.DataSource
        cmbTarget.ValueMember = cmbSource.ValueMember
        cmbTarget.DisplayMember = cmbSource.DisplayMember

        logger.Debug("ReplicateUltraCombo Exit")


    End Sub

    Function TextValue(ByRef txtField As String) As String

        logger.Debug("TextValue Entry")

        If txtField = "" Or txtField = "''" Then
            TextValue = "NULL"
        Else
            TextValue = txtField
        End If

        logger.Debug("TextValue Exit")


    End Function

    Function GetWeight_Unit(ByVal lUnit_ID As Integer) As Boolean

        logger.Debug("GetWeight_Unit Entry")

        Dim iLoop As Integer

        GetWeight_Unit = False

        For iLoop = 1 To UBound(aWeight_Unit)
            If aWeight_Unit(iLoop).Unit_ID = lUnit_ID Then
                GetWeight_Unit = aWeight_Unit(iLoop).Weight_Unit
                Exit Function
            End If
        Next iLoop

        logger.Debug("GetWeight_Unit Exit")


    End Function

    Function GetItemUnit(ByVal lUnit_ID As Integer) As tItemUnit

        logger.Debug("GetItemUnit Entry")

        Dim iLoop As Integer
        Dim result As tItemUnit = Nothing

        For iLoop = 1 To UBound(aWeight_Unit)
            If aWeight_Unit(iLoop).Unit_ID = lUnit_ID Then
                result = aWeight_Unit(iLoop)
                Exit For
            End If
        Next iLoop

        GetItemUnit = result

        logger.Debug("GetItemUnit Exit")

    End Function

    Public Function CostConversion(ByVal Amount As Decimal, ByVal FromUnit As Integer, ByVal ToUnit As Integer, ByVal PD1 As Decimal, ByVal PD2 As Decimal, ByVal PDU As Integer, ByRef Total_Weight As Decimal, ByRef Received As Decimal) As Decimal

        logger.Debug("CostConversion Entry")

        Dim fu As tItemUnit
        Dim tu As tItemUnit
        Dim result As Decimal

        ' TODO Documentation needed: What does PD1, PD2. PDU mean? Product descriptions?
        ' TODID:
        ' PD1 = VendorCostHistory.Package_Desc1 = Case Size
        ' PD2 = Item.Package_Desc2 = Item Size (12 part of "12 OZ")
        ' PDU = Item.Package_Unit_Id = Item Size UOM ( "OZ" part of "12 OZ")

        fu = GetItemUnit(FromUnit)
        tu = GetItemUnit(ToUnit)

        '-- Adjust PD1 based on the weight received if dealing with packages
        If Total_Weight <> 0 And Received <> 0 And (fu.IsPackageUnit Or tu.IsPackageUnit) Then PD1 = Total_Weight / (Received * PD2)

        If FromUnit <> ToUnit Then
            If fu.Weight_Unit Then
                If tu.IsPackageUnit Then
                    ' Convert A per weight cost, to a case cost (LB cost * Case Size * Item Size)
                    result = Amount * PD1 * PD2
                Else
                    result = Amount
                End If
            Else
                If Not fu.IsPackageUnit Then
                    If tu.IsPackageUnit Then
                        ' Convert A unit cost, to a case cost (unit cost * Case Size)
                        result = Amount * PD1
                    Else
                        result = Amount
                    End If
                Else 'FromUnit is a package
                    ' Added logic to check for ToUnit also being a package unit, for CASE to BOX conversions. RS 10/26 TFS: 11332
                    If tu.IsPackageUnit Then
                        result = Amount
                    Else
                        ' Convert A case cost, to a unit cost (case cost / (Item Size if it's a weight unit, or 1)
                        result = Amount / (PD1 * IIf(tu.Weight_Unit, PD2, 1))
                    End If
                End If
            End If
        Else
            ' From and To units are the same, cost stays the same
            result = Amount
        End If

        CostConversion = result

        logger.Debug("CostConversion Exit")


    End Function

    Public Function SQLOpenRecordSet(ByRef sCall As String, ByRef Param1 As Integer, ByRef Param2 As Integer, Optional ByRef bValidateLogon As Boolean = True) As DAO.Recordset


        logger.Debug("SQLOpenRecordSet entry: sCall=" + sCall)
        Dim dStart, dEnd As Date
        Dim lNumber As Integer
        Dim sDescription As String
        Dim i As Integer
        Dim rs As DAO.Recordset

        If bValidateLogon Then ValidateLogon()

        On Error Resume Next

        dStart = Now
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        logger.DebugFormat("DAO => {0} ", sCall)
        rs = gDBInventory.OpenRecordset(sCall, Param1, Param2)

        If Err.Number = 0 Then
            GoTo me_exit
        Else
            If DAODBEngine_definst.Errors.Count > 0 Then
                lNumber = DAODBEngine_definst.Errors(0).Number
            Else
                lNumber = Err.Number
            End If

            If lNumber = 10054 Then 'Connection glitch
                'try once again
                Err.Clear()
                rs = gDBInventory.OpenRecordset(sCall, Param1, Param2)
                If Err.Number = 0 Then GoTo me_exit
            End If
        End If

        dEnd = Now
        If DAODBEngine_definst.Errors.Count > 0 Then
            lNumber = DAODBEngine_definst.Errors(0).Number
            sDescription = DAODBEngine_definst.Errors(0).Description
        Else
            lNumber = Err.Number
            sDescription = Err.Description
        End If

        Err.Clear()

        'If the caller started a transaction, must rollback
        Do
            gDBInventory.Execute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Loop Until Err.Number <> 0

        Err.Clear()

        logger.Error("SQLOpenRecordSet error encounted.  An entry is being added to ODBCErrorLog." + Environment.NewLine + "SQL Call = " + sCall + Environment.NewLine + "Error Description = " + sDescription)
        Call InsertODBCError(sCall, lNumber, sDescription, dStart, dEnd)

        End
        logger.Debug("SQLOpenRecordSet exit")
me_exit:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        SQLOpenRecordSet = rs

        GC.Collect()

        logger.Debug("SQLOpenRecordSet Exit with(me_exit:)")

    End Function

    Public Sub SQLOpenRS(ByRef rs As DAO.Recordset, ByRef sCall As String, ByRef Param1 As Integer, ByRef Param2 As Integer, Optional ByRef bValidateLogon As Boolean = True)
        logger.Debug("SQLOpenRS entry: sCall=" + sCall)

        Dim dStart, dEnd As Date
        Dim lNumber As Integer
        Dim sDescription As String
        Dim i As Integer

        If bValidateLogon Then ValidateLogon()

        On Error Resume Next

        dStart = Now
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        logger.DebugFormat("DAO => {0} ", sCall)
        rs = gDBInventory.OpenRecordset(sCall, Param1, Param2)

        If Err.Number = 0 Then
            GoTo me_exit
        Else
            If DAODBEngine_definst.Errors.Count > 0 Then
                lNumber = DAODBEngine_definst.Errors(0).Number
            Else
                lNumber = Err.Number
            End If

            If lNumber = 10054 Then 'Connection glitch
                'try once again
                Err.Clear()
                rs = gDBInventory.OpenRecordset(sCall, Param1, Param2)
                If Err.Number = 0 Then GoTo me_exit
            End If
        End If

        dEnd = Now
        If DAODBEngine_definst.Errors.Count > 0 Then
            lNumber = DAODBEngine_definst.Errors(0).Number
            sDescription = DAODBEngine_definst.Errors(0).Description
        Else
            lNumber = Err.Number
            sDescription = Err.Description
        End If

        Err.Clear()

        'If the caller started a transaction, must rollback
        Do
            gDBInventory.Execute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Loop Until Err.Number <> 0

        Err.Clear()

        logger.Error("SQLOpenRS error encounted.  An entry is being added to ODBCErrorLog." + Environment.NewLine + "SQL Call = " + sCall + Environment.NewLine + "Error Description = " + sDescription)
        Call InsertODBCError(sCall, lNumber, sDescription, dStart, dEnd)

        End
        logger.Debug("SQLOpenRS exit")
me_exit:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        GC.Collect()
        logger.Debug("SQLOpenRS exit from me_exit:")
    End Sub

    Public Sub SQLOpenRS2(ByRef rs As DAO.Recordset, ByRef sCall As String, ByRef Param1 As Integer, ByRef Param2 As Integer, Optional ByRef bIgnoreError As Boolean = False, Optional ByRef bValidateLogon As Boolean = True)
        logger.DebugFormat("=> SQLOpenRS2: [ {0} ]", sCall)
        'NOTE: Unlike SQLOpenRS, bIgnoreError=True means raise back to the caller rather than do nothing
        'So the calling routine must handle the error if the bIgnoreError is set to true

        Dim dStart, dEnd As Date
        Dim lNumber As Integer
        Dim sDescription As String
        Dim i As Integer

        If bValidateLogon Then ValidateLogon()

        On Error Resume Next

        dStart = Now
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        logger.DebugFormat("DAO => {0} ", sCall)
        rs = gDBInventory.OpenRecordset(sCall, Param1, Param2)

        If Err.Number = 0 Then
            GoTo me_exit
        Else
            If DAODBEngine_definst.Errors.Count > 0 Then
                lNumber = DAODBEngine_definst.Errors(0).Number
            Else
                lNumber = Err.Number
            End If

            If lNumber = 10054 Then 'Connection glitch
                'try once again
                Err.Clear()
                rs = gDBInventory.OpenRecordset(sCall, Param1, Param2)
                If Err.Number = 0 Then GoTo me_exit
            End If
        End If

        dEnd = Now
        If DAODBEngine_definst.Errors.Count > 0 Then
            lNumber = DAODBEngine_definst.Errors(0).Number
            sDescription = DAODBEngine_definst.Errors(0).Description
        Else
            lNumber = Err.Number
            sDescription = Err.Description
        End If

        Err.Clear()

        If bIgnoreError Then
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            On Error GoTo 0
            Err.Raise(lNumber, , sDescription)
        Else

            'If the caller started a transaction, must rollback
            Do
                gDBInventory.Execute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Loop Until Err.Number <> 0

            Err.Clear()

            logger.Error("! SQLOpenRS2 error encounted.  An entry is being added to ODBCErrorLog." + Environment.NewLine + "SQL Call = " + sCall + Environment.NewLine + "Error Description = " + sDescription)
            Call InsertODBCError(sCall, lNumber, sDescription, dStart, dEnd)

            End
        End If
        logger.Debug("<= SQLOpenRS2")
me_exit:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        GC.Collect()
        logger.Debug("SQLOpenRS2 exit with me_exit:")
    End Sub

    Public Function SQLOpenRS3(ByRef sCall As String, ByRef Param1 As Integer, ByRef Param2 As Integer, ByRef lIgnoreErrNum() As Integer, Optional ByRef bValidateLogon As Boolean = True) As DAO.Recordset
        logger.Debug("SQLOpenRS3 entry: sCall=" + sCall)
        'NOTE: Unlike SQLOpenRS, if lIgnoreErrNum occurs, raise it back to the caller;
        '       otherwise, handle the error by ending the app

        Dim dStart, dEnd As Date
        Dim lNumber As Integer
        Dim sDescription As String
        Dim i As Integer
        Dim rs As DAO.Recordset

        If bValidateLogon Then ValidateLogon()

        On Error Resume Next

        dStart = Now
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        logger.DebugFormat("DAO => {0} ", sCall)
        rs = gDBInventory.OpenRecordset(sCall, Param1, Param2)

        If Err.Number = 0 Then
            GoTo me_exit
        Else
            If DAODBEngine_definst.Errors.Count > 0 Then
                lNumber = DAODBEngine_definst.Errors(0).Number
            Else
                lNumber = Err.Number
            End If

            If lNumber = 10054 Then 'Connection glitch
                'try once again
                Err.Clear()
                rs = gDBInventory.OpenRecordset(sCall, Param1, Param2)
                If Err.Number = 0 Then GoTo me_exit
            End If
        End If

        dEnd = Now
        If DAODBEngine_definst.Errors.Count > 0 Then
            lNumber = DAODBEngine_definst.Errors(0).Number
            sDescription = DAODBEngine_definst.Errors(0).Description
        Else
            lNumber = Err.Number
            sDescription = Err.Description
        End If

        Err.Clear()

        For i = LBound(lIgnoreErrNum) To UBound(lIgnoreErrNum)
            If lNumber = lIgnoreErrNum(i) Then
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                On Error GoTo 0
                Err.Raise(lNumber, , sDescription)
            End If
        Next

        'If the caller started a transaction, must rollback
        Do
            gDBInventory.Execute("ROLLBACK TRAN", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Loop Until Err.Number <> 0

        Err.Clear()

        logger.Error("SQLOpenRS3 error encounted.  An entry is being added to ODBCErrorLog." + Environment.NewLine + "SQL Call = " + sCall + Environment.NewLine + "Error Description = " + sDescription)
        Call InsertODBCError(sCall, lNumber, sDescription, dStart, dEnd)

        End
        logger.Debug("SQLOpenRS3 exit")
me_exit:
        SQLOpenRS3 = rs
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        GC.Collect()
        logger.Debug("SQLOpenRS3 exit from me_exit:")
    End Function

    Public Sub RetrieveTGMData(Optional ByRef pb As System.Windows.Forms.ProgressBar = Nothing)


        logger.Debug("RetrieveTGMData Entry")

        Dim rsReport As ADODB.Recordset = Nothing
        Dim lQueryTimeout As Integer

        lQueryTimeout = gDBInventory.QueryTimeout
        gDBInventory.QueryTimeout = 1800 '30 minutes

        gDBReport.BeginTrans()

        gDBReport.Execute("DELETE FROM TGMTool WHERE Instance = " & glInstance, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adExecuteNoRecords)
        Try
            With glTGMTool
                gRSRecordset = SQLOpenRecordSet("EXEC TGMToolGetData" & Trim(.Query) & " " & .SubTeam_No & ", '" & VB6.Format(.StartDate, "yyyy-MM-dd") & "', '" & VB6.Format(System.DateTime.FromOADate(.EndDate.ToOADate + 1), "yyyy-MM-dd") & "', " & .Discontinued & ", " & .HIAH & IIf(Trim(UCase(.Query)) = "ALL", "", ", " & .value), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End With

            rsReport = New ADODB.Recordset
            rsReport.Open("TGMTool", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)

            If Not IsNothing(pb) Then pb.Value = 1
            While Not gRSRecordset.EOF

                rsReport.AddNew()
                rsReport.Fields("Instance").Value = glInstance
                rsReport.Fields("Item_Key").Value = gRSRecordset.Fields("Item_Key").Value
                rsReport.Fields("Store_No").Value = gRSRecordset.Fields("Store_No").Value
                rsReport.Fields("Identifier").Value = gRSRecordset.Fields("Identifier").Value
                rsReport.Fields("Item_Description").Value = gRSRecordset.Fields("Item_Description").Value
                rsReport.Fields("Package_Desc").Value = gRSRecordset.Fields("Package_Desc").Value
                rsReport.Fields("Category_ID").Value = gRSRecordset.Fields("Category_ID").Value
                rsReport.Fields("CurrentCost").Value = gRSRecordset.Fields("CurrentCost").Value
                rsReport.Fields("CurrentExtCost").Value = gRSRecordset.Fields("CurrentExtCost").Value
                rsReport.Fields("CurrentRetail").Value = gRSRecordset.Fields("CurrentRetail").Value
                rsReport.Fields("Sold_By_weight").Value = gRSRecordset.Fields("Sold_By_weight").Value
                rsReport.Fields("TotalQuantity").Value = gRSRecordset.Fields("TotalQuantity").Value
                rsReport.Fields("TotalActualRetail").Value = IIf(IsDBNull(gRSRecordset.Fields("TotalActualRetail").Value), 0, gRSRecordset.Fields("TotalActualRetail").Value)
                rsReport.Fields("TotalRetail").Value = gRSRecordset.Fields("TotalRetail").Value
                rsReport.Fields("TotalCost").Value = gRSRecordset.Fields("TotalCost").Value
                rsReport.Fields("TotalExtCost").Value = gRSRecordset.Fields("TotalExtCost").Value
                rsReport.Update()

                gRSRecordset.MoveNext()

            End While

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
            End If
            If rsReport IsNot Nothing Then
                rsReport.Close()
            End If

        End Try

        If Not IsNothing(pb) Then pb.Value = 2

        '-- Populate the totals for the SubTeam
        gDBReport.Execute("DELETE FROM TGMToolHeader WHERE Instance = " & glInstance, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        Try
            With glTGMTool
                gRSRecordset = SQLOpenRecordSet("EXEC TGMToolGetAcctTotals " & .SubTeam_No & ", '" & VB6.Format(.StartDate, "yyyy-MM-dd") & "', '" & VB6.Format(System.DateTime.FromOADate(.EndDate.ToOADate + 1), "yyyy-MM-dd") & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End With

            rsReport.Open("TGMToolHeader", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)

            While Not gRSRecordset.EOF

                rsReport.AddNew()
                rsReport.Fields("Instance").Value = glInstance
                rsReport.Fields("Store_No").Value = gRSRecordset.Fields("Store_No").Value
                rsReport.Fields("TotalActualRetail").Value = gRSRecordset.Fields("ActualRetail").Value
                rsReport.Fields("TotalRetail").Value = gRSRecordset.Fields("TotalRetail").Value
                rsReport.Fields("TotalCost").Value = gRSRecordset.Fields("TotalCost").Value
                rsReport.Fields("TotalExtCost").Value = gRSRecordset.Fields("TotalExtCost").Value
                rsReport.Update()

                gRSRecordset.MoveNext()

            End While

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
            End If
            If rsReport IsNot Nothing Then
                rsReport.Close()
            End If
        End Try

        If Not IsNothing(pb) Then pb.Value = 3

        '-- Populate the totals for the SubTeam
        gDBReport.Execute("DELETE FROM TGMStore WHERE Instance = " & glInstance, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetStores", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            rsReport.Open("TGMStore", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)

            While Not gRSRecordset.EOF

                rsReport.AddNew()
                rsReport.Fields("Instance").Value = glInstance
                rsReport.Fields("Store_Name").Value = gRSRecordset.Fields("Store_Name").Value
                rsReport.Fields("Store_No").Value = gRSRecordset.Fields("Store_No").Value
                rsReport.Fields("Mega_Store").Value = gRSRecordset.Fields("Mega_Store").Value
                rsReport.Fields("WFM_Store").Value = gRSRecordset.Fields("WFM_Store").Value
                rsReport.Fields("Zone_ID").Value = gRSRecordset.Fields("Zone_ID").Value
                rsReport.Update()

                gRSRecordset.MoveNext()

            End While

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
            If rsReport IsNot Nothing Then
                rsReport.Close()
                rsReport = Nothing
            End If
        End Try

        gDBReport.CommitTrans()
        If gJetFlush IsNot Nothing Then
            gJetFlush.RefreshCache(gDBReport)
        End If

        If Not IsNothing(pb) Then pb.Value = 4

        gDBInventory.QueryTimeout = lQueryTimeout

        logger.Debug("RetrieveTGMData Exit")

    End Sub

    ''' <summary>
    ''' Convert a null value to an empty string
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function FixNull(ByVal obj As Object) As String

        If IsNothing(obj) Then
            FixNull = String.Empty
        Else
            FixNull = obj
        End If

    End Function

    Public Function FixValue(ByVal s As String) As String

        If Trim(s) = "" Or Trim(s) = "-" Then
            FixValue = "0"
        Else
            FixValue = s
        End If

    End Function

    Public Function FixNumber(ByVal s As String) As String

        s = Trim(s)
        If s = "" Then
            s = "0"
        Else
            If Mid(s, 1, 1) <> "-" Then
                s = "0" & s
            End If
        End If

        FixNumber = s

    End Function
    Public Function GrossMargin(ByRef cPrice As Decimal, ByRef cExtCost As Decimal) As Single


        logger.Debug("GrossMargin Entry")

        On Error GoTo me_err

        If cPrice > 0 Then

            GrossMargin = (cPrice - cExtCost) / cPrice

        Else
            GrossMargin = 0

        End If

        logger.Debug("GrossMargin Exit")

        Exit Function

me_err:
        Err.Raise(Err.Number, Err.Source, "GrossMargin error: " & Err.Description)

        logger.Debug("GrossMargin Exit from me_err:")

    End Function
    Public Function IsValidDate(ByRef sDate As String) As Boolean

        logger.Debug("IsValidDate Entry")

        Dim dDate As Date

        '-- Turn off error handling
        On Error Resume Next

        '-- See if VB had a problem with it
        dDate = CDate(sDate)
        If Err.Number = 0 Then
            IsValidDate = (dDate >= #1/1/1995# And dDate <= #6/6/2079#)
        Else
            IsValidDate = False
        End If

        '-- Reset error handling
        On Error GoTo 0
        Err.Clear()

        logger.Debug("IsValidDate Exit")


    End Function

    Public Sub RightJustControls(ByRef frmForm As System.Windows.Forms.Form, ByVal ParamArray oControls() As Control)
        'If the form hasn't been shown, then by default its controls will be visible = false
        'be sure to explicitly set your controls to visible = true\false in form load.


        logger.Debug("RightJustControls Entry")


        Dim tControl As Control
        Dim i As Short
        'get first left position
        For Each tControl In oControls
            If tControl.Visible = True Then Exit For
        Next tControl

        i = frmForm.Width - 100

        For Each tControl In oControls
            If tControl.Visible = True Then
                i = i - tControl.Width - 50
                tControl.Left = i
            End If
        Next tControl

        logger.Debug("RightJustControls Exit")


    End Sub

    Public Sub CenterControls(ByRef frmForm As System.Windows.Forms.Form, ByVal ParamArray oControls() As Control)


        logger.Debug("CenterControls Entry")

        Dim tControl As Control
        Dim i As Short
        'get first left position
        i = 0
        For Each tControl In oControls
            i = i + tControl.Width + 50
        Next tControl
        i = (VB6.PixelsToTwipsX(frmForm.Width) - i) / 2


        For Each tControl In oControls
            tControl.Left = i
            i = tControl.Left + tControl.Width + 50
        Next tControl

        logger.Debug("CenterControls Exit")


    End Sub

    Public Sub ValidateLogon()

        logger.Debug("ValidateLogon Entry")


        If DateDiff(Microsoft.VisualBasic.DateInterval.Minute, mLastActive, Now) >= 30 Then

            If Len(gsUserName) > 0 Then
                frmLogin.UserName = gsUserName
                frmLogin.SetUserInfo = False
            Else
                frmLogin.SetUserInfo = True
            End If

            SetActive((frmLogin.txtUserName), (mLastActive = System.DateTime.MinValue))

            frmLogin.ShowDialog()

            If Not frmLogin.Validated_Renamed Then
                End
            Else
                gsUserName = frmLogin.UserName
            End If

            frmLogin.Close()
            frmLogin.Dispose()

        End If

        mLastActive = Now

        logger.Debug("ValidateLogon Exit")


    End Sub

    ' SubTeamTypes:  All, Store, Supplier, StoreMinusSupplier, Retail, StoreUser
    Public Function DetermineSubTeamType(ByRef eSubTeamContext As enumSubTeamContext, Optional ByRef bIsVendorAFacility As Boolean = False, Optional ByRef bIsVendorSame As Boolean = False, Optional ByRef bIsPurchaserDistribution As Boolean = False) As enumSubTeamType


        logger.Debug("DetermineSubTeamType Entry")

        Dim eSubTeamType As enumSubTeamType

        If (eSubTeamContext <> enumSubTeamContext.OrderHeader_To) Then
            If (geProductType <> enumProductType.Product) And (eSubTeamContext = enumSubTeamContext.Item_From) Then
                If (geProductType = enumProductType.PackagingSupplies) Then
                    eSubTeamType = enumSubTeamType.Packaging
                ElseIf (geProductType = enumProductType.OtherSupplies) Then
                    eSubTeamType = enumSubTeamType.Supplies
                End If
            Else
                ' Check the order type
                Select Case geOrderType
                    Case enumOrderType.Distribution
                        eSubTeamType = enumSubTeamType.Supplier
                        '                If (eSubTeamContext = OrderHeader_From) Then
                        '                    eSubTeamType = enumSubTeamType.Supplier
                        '                ElseIf (eSubTeamContext = Item_From) Then
                        '                    eSubTeamType = enumSubTeamType.SupplierRetail
                        '                End If

                    Case enumOrderType.Purchase
                        If bIsPurchaserDistribution Then
                            eSubTeamType = enumSubTeamType.All
                        Else
                            eSubTeamType = enumSubTeamType.Retail
                        End If

                    Case enumOrderType.Transfer
                        If (bIsVendorAFacility) Then
                            If (bIsVendorSame) Then
                                eSubTeamType = enumSubTeamType.Store
                            Else
                                eSubTeamType = enumSubTeamType.StoreMinusSupplier
                            End If
                        Else
                            eSubTeamType = enumSubTeamType.Store
                        End If

                End Select
            End If
        Else 'SubTeamTo
            ' The list is always the same (storeuser) for OrderHeader_To context
            eSubTeamType = enumSubTeamType.StoreUser
        End If

        ' return the final choice
        DetermineSubTeamType = eSubTeamType

        ' SUBTEAMS -----------------------------------------------------------------

        ' Store Rule:
        ' All store subteams, limited by store.
        ' Used for From-SubTeam list for OrderType: Transfer

        ' Supplier Rule:
        ' All zone subteams, limited by Vendor (distributor/manufacturer)
        ' Used for From-SubTeam list for OrderType: Distribution

        ' StoreMinusSupplier Rule:
        ' All store subteams that are not supplier zone subteams, limited by Supplier
        ' Used for From-SubTeam list for OrderType: Transfer (when Vendor is distributor)
        '   Unless Vendor is distributor and transfer is within the same facility, then
        '   the From-SubTeam is opened up to enumSubTeamType.Store

        ' Retail Rule:
        ' All store subteams, limited by retail = 1.
        ' Used for From-SubTeam list for OrderType: Purchasing

        ' StoreUser Rule:
        ' All store subteams, limited by store and limited by user.
        ' Used for To-SubTeam list for OrderTypes: Purchasing, Distribution,
        '   and Transfers

        ' ORDERS -----------------------------------------------------------------

        ' Purchase Order...
        ' From-SubTeam Type = enumSubTeamType.Retail
        ' Item-SubTeam Type = enumSubTeamType.Retail
        ' To-SubTeam Type = enumSubTeamType.StoreUser

        ' Distribution Order...
        ' From-SubTeam Type = enumSubTeamType.Supplier
        ' Item-SubTeam Type = enumSubTeamType.Supplier
        ' To-SubTeam Type = enumSubTeamType.StoreUser

        ' Transfer Order, Vendor is Distributor/Manufacturer...
        ' From-SubTeam Type = enumSubTeamType.StoreMinusSupplier
        ' Item-SubTeam Type = enumSubTeamType.All
        ' To-SubTeam Type = enumSubTeamType.StoreUser

        ' Transfer Order, Vendor is Store...
        '   OR Vendor is Distributor/Manufacturer and transfer is within Same Facility...
        ' From-SubTeam Type = enumSubTeamType.Store
        ' Item-SubTeam Type = enumSubTeamType.All
        ' To-SubTeam Type = enumSubTeamType.StoreUser

        logger.Debug("DetermineSubTeamType Exit")


    End Function

    Public Function DetermineVendorInternal() As Boolean

        logger.Debug("DetermineVendorInternal Entry")

        Dim bIsInternal As Boolean

        Try
            ' Determine type of vendor
            gRSRecordset = SQLOpenRecordSet("EXEC GetVendorStore " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If Not gRSRecordset.EOF Then
                glStoreNo = gRSRecordset.Fields("Store_No").Value
                gbDistribution_Center = gRSRecordset.Fields("Distribution_Center").Value
                gbManufacturer = gRSRecordset.Fields("Manufacturer").Value
                bIsInternal = True
            Else
                glStoreNo = -1
                gbDistribution_Center = False
                gbManufacturer = False
                bIsInternal = False
            End If

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        DetermineVendorInternal = bIsInternal

        logger.Debug("DetermineVendorInternal Exit with bIsInternal =" + bIsInternal.ToString())
    End Function


    Public Function DeterminePurchaserFacility(ByRef lVendorID As Integer) As Boolean

        logger.Debug("DeterminePurchaserFacility Entry")

        Dim bIsInternal As Boolean
        bIsInternal = False

        Try
            ' Determine type of purchaser
            gRSRecordset = SQLOpenRecordSet("EXEC GetVendorStore " & lVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If Not gRSRecordset.EOF Then
                If (gRSRecordset.Fields("Distribution_Center")).Value Or (gRSRecordset.Fields("Manufacturer")).Value Then
                    bIsInternal = True
                End If
            End If

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        DeterminePurchaserFacility = bIsInternal

        logger.Debug("DeterminePurchaserFacility Exit")
    End Function

    Public Function DeterminePurchaserStore(ByRef lVendorID As Integer) As Boolean

        logger.Debug("DeterminePurchaserStore Entry")


        Dim bIsRetailStore As Boolean
        bIsRetailStore = False

        Try
            ' Determine type of purchaser
            gRSRecordset = SQLOpenRecordSet("EXEC GetVendorStore " & lVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If Not gRSRecordset.EOF Then
                If Not (gRSRecordset.Fields("Distribution_Center")).Value And Not (gRSRecordset.Fields("Manufacturer")).Value Then
                    bIsRetailStore = True
                End If
            End If

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        DeterminePurchaserStore = bIsRetailStore

        logger.Debug("DeterminePurchaserStore Exit")

    End Function

    Public Function GetEndOfPeriodDate(Optional ByRef dtDateInPeriod As Date = #12:00:00 AM#) As Date
        '------------------------------------------------------------------------------------------
        ' Purpose:  Returns the End of Period Date (date only, no time).
        ' Inputs:   dtDateInPeriod - If passed in, function returns end of period for this date.
        '                                     If not passed in, function returns the CURRENT end of period date.
        '------------------------------------------------------------------------------------------

        logger.Debug("GetEndOfPeriodDate Entry")

        Dim dtEOPDate As Date
        Dim sDate As String

        sDate = "null"
        If dtDateInPeriod <> CDate("12:00:00 AM") Then
            sDate = "'" & dtDateInPeriod & "'"
        End If

        Try
            'Get the begin date of period.
            gRSRecordset = SQLOpenRecordSet("EXEC GetBeginPeriodDateRS " & sDate, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If Not gRSRecordset.EOF Then
                dtEOPDate = gRSRecordset.Fields("PeriodBeginDate").Value
                dtEOPDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, 27, dtEOPDate)
            Else
                'SP should fail before reaching this. Returning "Now" may cause erroneous program behavior.
                dtEOPDate = SystemDateTime(True)
            End If

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        GetEndOfPeriodDate = dtEOPDate

        logger.Debug("GetEndOfPeriodDate Exit")

    End Function

    ''' <summary>
    ''' Get the current Date from the database server (with no time information)
    ''' </summary>
    ''' <returns>Date only</returns>
    ''' <remarks></remarks>
    Public Function GetDBDate() As Date
        Return GetDatabaseDateTime(False)
    End Function

    ''' <summary>
    ''' Get the current Date and Time from the database server
    ''' </summary>
    ''' <returns>Date/Time</returns>
    ''' <remarks></remarks>
    Public Function GetDBDateTime() As Date
        Return GetDatabaseDateTime(True)
    End Function

    Private Function GetDatabaseDateTime(ByVal IncludeTime As Boolean, Optional ByVal ForceGet As Boolean = False) As Date
        '-----------------------------------------------------
        ' Purpose: Returns system date and time from database.
        '-----------------------------------------------------

        logger.Debug("GetDatabaseDateTime Entry")

        Dim cmd As SqlClient.SqlCommand
        Dim prm As SqlClient.SqlParameter

        If IncludeTime OrElse m_dtDatabaseDateTime = System.DateTime.MinValue Then
            cmd = New SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetSystemDateTime"

            'add input parameter
            prm = New SqlClient.SqlParameter("@IncludeTime", SqlDbType.Bit)
            prm.Direction = ParameterDirection.Input
            prm.Value = System.Math.Abs(CInt(IncludeTime))
            cmd.Parameters.Add(prm)

            'add output parameter
            prm = New SqlClient.SqlParameter("@SystemDateTime", SqlDbType.DateTime)
            prm.Direction = ParameterDirection.Output
            cmd.Parameters.Add(prm)

            'set the database connection
            cmd.Connection = GetAdoNetConnection()

            Try
                cmd.ExecuteNonQuery()

                m_dtDatabaseDateTime = cmd.Parameters("@SystemDateTime").Value
            Catch ex As Exception
                'use the date/time from the computer as the fallback
                m_dtDatabaseDateTime = System.DateTime.Now
                logger.Error(ex.Message)
            Finally
                prm = Nothing
                cmd.Dispose()
            End Try
        End If

        logger.Debug("GetDatabaseDateTime Exit")

        If IncludeTime Then
            Return m_dtDatabaseDateTime
        Else
            Return FormatDateTime(m_dtDatabaseDateTime, DateFormat.ShortDate)
        End If
    End Function

    Public Function CheckReceivingInProgress(ByVal StoreNumber As Int64, ByVal UserId As Int64) As Boolean

        logger.Debug("CheckReceivingInProgress Entry")

        Dim cmd As SqlClient.SqlCommand
        Dim prm As SqlClient.SqlParameter

        cmd = New SqlClient.SqlCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "CheckReceivingInProgress"

        'add input parameter
        prm = New SqlClient.SqlParameter("@Store_No", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        prm.Value = System.Math.Abs(CInt(StoreNumber))
        cmd.Parameters.Add(prm)

        prm = New SqlClient.SqlParameter("@User_Id", SqlDbType.Int)
        prm.Direction = ParameterDirection.Input
        prm.Value = System.Math.Abs(CInt(UserId))
        cmd.Parameters.Add(prm)

        'set the database connection
        cmd.Connection = GetAdoNetConnection()

        Try
            isReceivingInProgress = cmd.ExecuteScalar()

        Catch ex As Exception
            'use the date/time from the computer as the fallback
            logger.Error(ex.Message)
        Finally
            prm = Nothing
            cmd.Dispose()
        End Try

        logger.Debug("CheckReceivingInProgress Exit")

        Return isReceivingInProgress
    End Function

    Public Function SystemDateTime(Optional ByRef bDateOnly As Boolean = False) As Date
        '-----------------------------------------------------
        ' Purpose: Returns system date and time from database.
        '-----------------------------------------------------

        If gbUseLocalTime Then
            'This would be used for testing purposes only.
            'It is activated by starting the applicaiton and passing "uselocaltime" as a command line parameter.
            SystemDateTime = Now
        Else
            Try
                gRSRecordset = SQLOpenRecordSet("EXEC GetSystemDate", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                SystemDateTime = gRSRecordset.Fields("SystemDate").Value
            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                End If
            End Try
        End If

        If bDateOnly Then
            SystemDateTime = FormatDateTime(SystemDateTime, DateFormat.ShortDate)
        End If

    End Function

    Public Sub ReportingServicesReport(ByRef sReportURL As String)
        '-------------------------------------------
        ' Purpose: Runs a Reporting Services report.
        '-------------------------------------------

        'Doesn't work... fix later...
        'Browser.Width = Screen.PrimaryScreen.Bounds.Width
        'Browser.Height = Screen.PrimaryScreen.Bounds.Height

        ' this creates a new instance of the webbrowser class on the same thread
        ' as the IRMA client - this is causing hang-ups of the client and is not the proper
        ' way to open a new browser window in its own process.
        'Dim Browser As New WebBrowser
        'Browser.Navigate(gsReportingServicesURL & sReportURL, True)
        'Browser.Visible = True

        ' this starts a new process
        System.Diagnostics.Process.Start(gsReportingServicesURL & sReportURL)

    End Sub

    Public Sub GoToURL(ByRef url As String)
        ' Opens a browser and navigates to the URL provided.
        ' Similar to Global.ReportingServicesReport except no string concatenation is 
        ' required between the reporting services URL and the report name/parameter string.

        ' this creates a new instance of the webbrowser class on the same thread
        ' as the IRMA client - this is causing hang-ups of the client and is not the proper
        ' way to open a new browser window in its own process.
        'Dim Browser As New WebBrowser
        'Browser.Navigate(url, True)
        'Browser.Visible = True

        ' this starts a new process
        System.Diagnostics.Process.Start(url)

    End Sub

    Public Function ExternalCountMissing(ByRef lMasterCountID As Integer) As Boolean
        '----------------------------------------------------------------------------------------
        ' Purpose: For Cycle Counting... determines if an external count is required and present.
        '----------------------------------------------------------------------------------------

        logger.Debug("ExternalCountMissing Entry")

        Dim rs As DAO.Recordset = Nothing

        ExternalCountMissing = False

        Try
            rs = SQLOpenRecordSet("EXEC CheckForExternalCycleCount " & lMasterCountID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            ExternalCountMissing = IIf(rs.Fields("Found").Value = 0, True, False)

        Finally
            If rs IsNot Nothing Then
                rs.Close()
                rs = Nothing
            End If
        End Try

        logger.Debug("ExternalCountMissing Exit")


    End Function

    Public Function AllCountsClosed(ByRef lMasterCountID As Integer) As Boolean
        '-----------------------------------------------------------------------------------
        ' Purpose: For Cycle Counting... determines if all counts for the Master are closed.
        '-----------------------------------------------------------------------------------

        logger.Debug("AllCountsClosed Entry")


        Dim rsClosed As DAO.Recordset = Nothing

        AllCountsClosed = False

        Try
            rsClosed = SQLOpenRecordSet("EXEC CheckForOpenCycleCounts " & lMasterCountID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If rsClosed.Fields("OpenCounts").Value = 0 Then AllCountsClosed = True

        Finally
            If rsClosed IsNot Nothing Then
                rsClosed.Close()
                rsClosed = Nothing
            End If
        End Try

        logger.Debug("AllCountsClosed Exit")


    End Function

    Public Sub InitializeComboIndexArray(ByRef indexArray As Integer(), ByVal arraysize As Integer)
        Dim i As Integer
        ReDim indexArray(arraysize)
        For i = 0 To arraysize - 1
            indexArray(i) = -1
        Next i
    End Sub

    Public Sub AddCategory(ByRef cmbCat As System.Windows.Forms.ComboBox, ByVal lCurSubTeamNo As Integer)
        '-----------------------------------------------------------------------------------
        'Used to be a part of the ComboSubTeamCat control in the VB6 version.
        '-----------------------------------------------------------------------------------

        logger.Debug("AddCategory Entry")

        Dim lLoop As Integer
        Dim lMax As Integer
        Dim lMaxValue As Integer

        glCategoryID = 0

        '-- Call the adding form.
        Dim frmCatAdd As New frmCategoryAdd
        frmCatAdd.plSubTeam_No = lCurSubTeamNo
        frmCatAdd.ShowDialog()
        frmCatAdd.Dispose()

        '-- a new Category was added
        If glCategoryID = -2 Then

            Call LoadCategory(cmbCat, lCurSubTeamNo)
            For lLoop = 0 To cmbCat.Items.Count - 1

                If VB6.GetItemData(cmbCat, lLoop) > lMaxValue Then
                    lMaxValue = VB6.GetItemData(cmbCat, lLoop)
                    lMax = lLoop
                End If

            Next lLoop
            cmbCat.SelectedIndex = lMax

        End If

        logger.Debug("AddCategory Exit")

    End Sub

    Public Sub AddLevel3(ByRef cmbLevel3 As System.Windows.Forms.ComboBox, ByVal lCurCategoryId As Integer, ByVal lCurSubTeamNo As Integer)


        logger.Debug("AddLevel3 Entry")

        Dim lLoop As Integer
        Dim lMax As Integer
        Dim lMaxValue As Integer

        glLevel3ID = 0

        '-- Call the adding form.
        Dim frmLevel3Add As New Level3Add
        frmLevel3Add.SubTeamId = lCurSubTeamNo
        frmLevel3Add.CategoryId = lCurCategoryId
        frmLevel3Add.ShowDialog()
        frmLevel3Add.Dispose()

        '-- a new Level 3 was added
        If glLevel3ID = -2 Then

            Call LoadProdHierarchyLevel3s(cmbLevel3, lCurCategoryId)
            For lLoop = 0 To cmbLevel3.Items.Count - 1

                If VB6.GetItemData(cmbLevel3, lLoop) > lMaxValue Then
                    lMaxValue = VB6.GetItemData(cmbLevel3, lLoop)
                    lMax = lLoop
                End If

            Next lLoop
            cmbLevel3.SelectedIndex = lMax

        End If

        logger.Debug("AddLevel3 Exit")
    End Sub

    Public Sub AddLevel4(ByRef cmbLevel4 As System.Windows.Forms.ComboBox, ByVal lCurLevel3Id As Integer, ByVal lCurCategoryId As Integer)


        logger.Debug("AddLevel4 Entry")

        Dim lLoop As Integer
        Dim lMax As Integer
        Dim lMaxValue As Integer

        glLevel3ID = 0

        '-- Call the adding form.
        Dim frmLevel4Add As New Level4Add
        frmLevel4Add.Level3Id = lCurLevel3Id
        frmLevel4Add.CategoryId = lCurCategoryId
        frmLevel4Add.ShowDialog()
        frmLevel4Add.Dispose()

        '-- a new Level 4 was added
        If glLevel4ID = -2 Then

            Call LoadProdHierarchyLevel4s(cmbLevel4, lCurLevel3Id)
            For lLoop = 0 To cmbLevel4.Items.Count - 1

                If VB6.GetItemData(cmbLevel4, lLoop) > lMaxValue Then
                    lMaxValue = VB6.GetItemData(cmbLevel4, lLoop)
                    lMax = lLoop
                End If

            Next lLoop
            cmbLevel4.SelectedIndex = lMax

        End If

        logger.Debug("AddLevel4 Exit")

    End Sub

    Public Sub StoreListGridCreateLoad(ByRef rsStores As ADODB.Recordset, ByRef dtStores As DataTable, ByRef ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid)

        logger.Debug("StoreListGridCreateLoad Entry")

        dtStores = New DataTable("StoreList")
        dtStores.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
        dtStores.Columns.Add(New DataColumn("Store_Name", GetType(String)))
        dtStores.Columns.Add(New DataColumn("Zone_ID", GetType(Integer)))
        dtStores.Columns.Add(New DataColumn("State", GetType(String)))
        dtStores.Columns.Add(New DataColumn("WFM_Store", GetType(Boolean)))
        dtStores.Columns.Add(New DataColumn("Mega_Store", GetType(Boolean)))
        dtStores.Columns.Add(New DataColumn("CustomerType", GetType(Integer)))

        '-- Fill out the store list
        Dim row As DataRow
        While Not rsStores.EOF
            row = dtStores.NewRow
            row("Store_No") = rsStores.Fields("Store_No").Value
            row("Store_Name") = rsStores.Fields("Store_Name").Value
            row("Zone_ID") = rsStores.Fields("Zone_ID").Value
            row("State") = rsStores.Fields("State").Value
            row("WFM_Store") = rsStores.Fields("WFM_Store").Value
            row("Mega_Store") = rsStores.Fields("Mega_Store").Value
            row("CustomerType") = rsStores.Fields("CustomerType").Value

            dtStores.Rows.Add(row)

            rsStores.MoveNext()
        End While

        dtStores.AcceptChanges()
        ugrdStoreList.DataSource = dtStores

        logger.Debug("StoreListGridCreateLoad Exit")


    End Sub

    Public Sub StoreListGridCreateLoadWithTax(ByRef rsStores As ADODB.Recordset, ByRef dtStores As DataTable, ByRef ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid)

        logger.Debug("StoreListGridCreateLoadWithTax Entry")

        dtStores = New DataTable("StoreList")
        dtStores.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
        dtStores.Columns.Add(New DataColumn("Store_Name", GetType(String)))
        dtStores.Columns.Add(New DataColumn("Price", GetType(String)))
        dtStores.Columns.Add(New DataColumn("Zone_ID", GetType(Integer)))
        dtStores.Columns.Add(New DataColumn("State", GetType(String)))
        dtStores.Columns.Add(New DataColumn("WFM_Store", GetType(Boolean)))
        dtStores.Columns.Add(New DataColumn("Mega_Store", GetType(Boolean)))
        dtStores.Columns.Add(New DataColumn("CustomerType", GetType(Integer)))
        dtStores.Columns.Add(New DataColumn("TaxPercent", GetType(Double)))

        '-- Fill out the store list
        Dim row As DataRow
        While Not rsStores.EOF
            row = dtStores.NewRow
            row("Store_No") = rsStores.Fields("Store_No").Value
            row("Store_Name") = rsStores.Fields("Store_Name").Value
            row("Price") = "0" 'rsStores.Fields("Price").Value
            row("Zone_ID") = rsStores.Fields("Zone_ID").Value
            row("State") = rsStores.Fields("State").Value
            row("WFM_Store") = rsStores.Fields("WFM_Store").Value
            row("Mega_Store") = rsStores.Fields("Mega_Store").Value
            row("CustomerType") = rsStores.Fields("CustomerType").Value
            row("TaxPercent") = "0" 'rsStores.Fields("TaxPercent").Value

            dtStores.Rows.Add(row)

            rsStores.MoveNext()
        End While

        dtStores.AcceptChanges()
        ugrdStoreList.DataSource = dtStores

        logger.Debug("StoreListGridCreateLoadWithTax Exit")


    End Sub

    Public Sub StoreListGridLoadStatesCombo(ByRef dtStoreList As DataTable, ByRef cmbStates As ComboBox)


        logger.Debug("StoreListGridLoadStatesCombo Entry")

        Dim dv As DataView = New System.Data.DataView(dtStoreList)
        Dim drv As DataRowView
        Dim currState As String = ""
        Dim i As Integer

        dv.Sort = "State"

        For i = 0 To dv.Count - 1
            drv = dv.Item(i)
            If Not (IsDBNull(drv("State"))) AndAlso drv("State") <> currState Then
                cmbStates.Items.Add(drv("State"))
                currState = drv("State")
            End If
        Next

        drv = Nothing
        dv.Dispose()

        logger.Debug("StoreListGridLoadStatesCombo Exit")


    End Sub

    Public Sub StoreListGridLoadStatesCombo(ByRef udsStores As UltraDataSource, ByRef cmbStates As ComboBox)


        logger.Debug("StoreListGridLoadStatesCombo Entry")

        Dim row As UltraDataRow
        Dim rowNdx As Integer

        With udsStores
            For rowNdx = 0 To udsStores.Rows.Count - 1
                row = udsStores.Rows(rowNdx)
                If Not (IsDBNull(row("State"))) AndAlso row("State") <> "" AndAlso Not StateAlreadyInList(cmbStates, row("State")) Then
                    cmbStates.Items.Add(row("State"))
                End If
            Next

            row = Nothing
        End With

        logger.Debug("StoreListGridLoadStatesCombo Exit")


    End Sub

    Private Function StateAlreadyInList(ByVal cmbStates As ComboBox, ByVal state As String) As Boolean

        logger.Debug("StateAlreadyInList Entry with state=" + state)

        Dim i As Integer

        For i = 0 To cmbStates.Items.Count - 1
            If cmbStates.Items(i) = state Then
                logger.Debug("StateAlreadyInList exit True")
                Return True
            End If
        Next

        logger.Debug("StateAlreadyInList exit False")

        Return False

    End Function

    Public Sub StoreListGridSelectAll(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal IsSelected As Boolean)

        logger.Debug("StoreListGridSelectAll Entry")

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrd.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            row.Selected = IsSelected
        Next row

        If ugrd.Selected.Rows.Count > 0 Then ugrd.ActiveRow = ugrd.Selected.Rows(0)

        logger.Debug("StoreListGridSelectAll Exit")


    End Sub

    Public Sub StoreListGridSelectAllRegion(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid)

        logger.Debug("StoreListGridSelectAllRegion Entry")

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrd.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            If row.Cells("CustomerType").Value = "3" Then
                row.Selected = True
            Else
                row.Selected = False
            End If
        Next row

        If ugrd.Selected.Rows.Count > 0 Then ugrd.ActiveRow = ugrd.Selected.Rows(0)

        logger.Debug("StoreListGridSelectAllRegion Exit")


    End Sub

    Public Sub StoreListGridSelectRetailOnly(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid)

        logger.Debug("StoreListGridSelectRetailOnly Entry")

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrd.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            If row.Cells("CustomerType").Value = "3" And (row.Cells("WFM_Store").Value Or row.Cells("Mega_Store").Value) Then
                row.Selected = True
            Else
                row.Selected = False
            End If
        Next row

        If ugrd.Selected.Rows.Count > 0 Then ugrd.ActiveRow = ugrd.Selected.Rows(0)

        logger.Debug("StoreListGridSelectRetailOnly Exit")


    End Sub

    Public Function StoreListGridGetStoreList(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid) As String

        logger.Debug("StoreListGridGetStoreList Entry")

        Dim sStores As New System.Text.StringBuilder(String.Empty)
        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow

        For Each row In ugrd.Selected.Rows
            If sStores.Length > 0 Then
                sStores.Append("|" & row.Cells("Store_No").Value.ToString)
            Else
                sStores.Append(row.Cells("Store_No").Value.ToString)
            End If
        Next

        logger.Debug("StoreListGridGetStoreList exit with" + sStores.ToString)

        Return sStores.ToString

    End Function

    Public Sub StoreListGridSelectStore(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal Store_No As Integer)

        logger.Debug("StoreListGridSelectStore Entry with Store_No=" + Store_No.ToString())

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrd.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            row.Selected = False
            If Not (row.Cells("Store_No").Value Is System.DBNull.Value) Then
                If row.Cells("Store_No").Value = Store_No Then
                    row.Selected = True
                End If
            End If
        Next row

        If ugrd.Selected.Rows.Count > 0 Then ugrd.ActiveRow = ugrd.Selected.Rows(0)

        logger.Debug("StoreListGridSelectStore Exit")


    End Sub

    Public Sub StoreListGridSelectByZone(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal Zone_ID As Integer)

        logger.Debug("StoreListGridSelectByZone Entry with Zone_ID=" + Zone_ID.ToString)

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrd.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            row.Selected = False
            If Not (row.Cells("Zone_ID").Value Is System.DBNull.Value) Then
                If row.Cells("Zone_ID").Value = Zone_ID Then
                    row.Selected = True
                End If
            End If
        Next row

        If ugrd.Selected.Rows.Count > 0 Then ugrd.ActiveRow = ugrd.Selected.Rows(0)

        logger.Debug("StoreListGridSelectByZone Exit")


    End Sub

    Public Sub StoreListGridSelectByJurisdiction(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal StoreJurisdictionID As Integer)

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrd.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            row.Selected = False
            If Not (row.Cells("StoreJurisdictionID").Value Is System.DBNull.Value) Then
                If row.Cells("StoreJurisdictionID").Value = StoreJurisdictionID Then
                    row.Selected = True
                End If
            End If
        Next row

        If ugrd.Selected.Rows.Count > 0 Then ugrd.ActiveRow = ugrd.Selected.Rows(0)

    End Sub

    Public Sub StoreListGridSelectByState(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal State As String)

        logger.Debug("StoreListGridSelectByState entry with state=" + State)
        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrd.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            row.Selected = False
            If Not (row.Cells("State").Value Is System.DBNull.Value) Then
                If row.Cells("State").Value = State Then
                    row.Selected = True
                End If
            End If
        Next row

        If ugrd.Selected.Rows.Count > 0 Then ugrd.ActiveRow = ugrd.Selected.Rows(0)

        logger.Debug("StoreListGridSelectByState Exit")


    End Sub

    Public Sub StoreListGridSelectAllWFM(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid)

        logger.Debug("StoreListGridSelectAllWFM Entry")

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrd.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            row.Selected = False
            If Not (row.Cells("WFM_Store").Value Is System.DBNull.Value) Then
                If row.Cells("WFM_Store").Value Then
                    row.Selected = True
                End If
            End If
        Next row

        If ugrd.Selected.Rows.Count > 0 Then ugrd.ActiveRow = ugrd.Selected.Rows(0)

        logger.Debug("StoreListGridSelectAllWFM Exit")


    End Sub

    Public Sub StoreListGridSelectAll365(ByRef ugrd As Infragistics.Win.UltraWinGrid.UltraGrid)

        logger.Debug("StoreListGridSelectAll365 Entry")

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrd.DisplayLayout.Bands(0).GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow)
            If row.Cells("Mega_Store").Value Then
                row.Selected = True
            End If
        Next row

        If ugrd.Selected.Rows.Count > 0 Then ugrd.ActiveRow = ugrd.Selected.Rows(0)

        logger.Debug("StoreListGridSelectAll365 Exit")


    End Sub


    Public Sub WriteAppEventLog(ByVal msg As String)

        If Not EventLog.SourceExists("IRMA") Then EventLog.CreateEventSource("IRMA", "Application")

        EventLog.WriteEntry("IRMA", msg, EventLogEntryType.Error)

    End Sub

    Public Sub WriteAppEventLog(ByVal msg As String, ByVal type As EventLogEntryType)

        If Not EventLog.SourceExists("IRMA") Then EventLog.CreateEventSource("IRMA", "Application")

        EventLog.WriteEntry("IRMA", msg, type)

    End Sub

    Public Function GetPriceWithoutVAT(ByVal POSPriceWithVAT As Double, ByVal VATRate As Double) As Double

        Dim price As Double
        price = POSPriceWithVAT / ((VATRate / 100) + 1)
        GetPriceWithoutVAT = Math.Round(price, 2)

    End Function

    Public Function GetCategoryClass(ByVal CategoryName As String) As String

        logger.Debug("GetCategoryClass Entry with CategoryName =" + CategoryName)

        Dim catClass As String = ""

        CategoryName = CategoryName.TrimEnd(CType(" ", Char))
        If CategoryName.Length > 4 Then
            catClass = CategoryName.Substring(CategoryName.Length - 4)
            catClass = catClass.TrimStart(CType(" ", Char))
        Else
            catClass = CategoryName
        End If

        catClass = catClass.TrimStart(CType(" ", Char))

        logger.Debug("CategoryName Exit with GetCategoryClass=" & catClass)
        Return catClass

    End Function

    Public Function DetermineStoreVendorCurrencyMatch(ByRef lVendorID As Integer, ByVal iVendorID As Integer, ByVal sDirection As String) As Boolean
        logger.Debug("DetermineStoreVendorCurrencyMatch Entry")

        Dim bIsMatch As Boolean
        bIsMatch = False

        Try
            ' Determine type of purchaser
            gRSRecordset = SQLOpenRecordSet("EXEC GetVendorStoreCurrencyMatch " & lVendorID & ", " & iVendorID & ", '" & sDirection & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If Not gRSRecordset.EOF Then
                If (gRSRecordset.Fields("Match")).Value = True Then
                    bIsMatch = True
                End If
            End If

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        DetermineStoreVendorCurrencyMatch = bIsMatch

        logger.Debug("DetermineStoreVendorCurrencyMatch Exit")
    End Function

    Public Function DetermineOrderCurrency(ByVal iOrderID As Integer) As String
        logger.Debug("DetermineOrderCurrency Entry")

        Dim sCurrencyCode As String = String.Empty

        Try
            ' Determine type of purchaser
            gRSRecordset = SQLOpenRecordSet("EXEC GetOrderCurrency " & iOrderID & ", '" & gsRegionCode & "' ", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If Not gRSRecordset.EOF Then
                If Not (gRSRecordset.Fields("CurrencyCode")) Is Nothing Then
                    sCurrencyCode = (gRSRecordset.Fields("CurrencyCode")).Value
                End If
            End If

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        DetermineOrderCurrency = sCurrencyCode

        logger.Debug("DetermineOrderCurrency Exit")
    End Function

    Public Sub LoadSustainabilityRankings(ByRef cmbComboBox As System.Windows.Forms.ComboBox)
        logger.Debug("LoadSustainabilityRankings Entry")

        'Load all Origin into combo box.
        gRSRecordset = SQLOpenRecordSet("EXEC GetSustainabilityRankings", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Dim l As New Collection
        l.Add("", -1)

        Do While Not gRSRecordset.EOF
            l.Add(gRSRecordset.Fields("RankingDescription").Value, gRSRecordset.Fields("ID").Value)
            gRSRecordset.MoveNext()
        Loop

        cmbComboBox.DataSource = l
        gRSRecordset.Close()
        gRSRecordset = Nothing

        logger.Debug("LoadSustainabilityRankings Exit")
    End Sub

    'TFS 8325, 02/28/2013, Faisal Ahmed, Refusal functionality
    Public Function IsRefusalAllowed(ByVal iOrderHeader_ID As Integer) As Boolean
        logger.Debug("IsRefusalAllowed Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim returnVal As Boolean = False

        ' Execute the function
        returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_IsRefusalAllowed(" & CStr(iOrderHeader_ID) & ")"), Boolean)

        logger.Debug("IsRefusalAllowed Exit")
        Return returnVal
    End Function

    Public Function ConvertTemperature(ByVal temperature As Integer, ByVal unitId As Integer) As Integer

        Select Case unitId
            Case TemperatureUnit.Celsius
                Return (temperature - 32) * 5 / 9
            Case TemperatureUnit.Fahrenheit
                Return (temperature * 9 / 5) + 32
        End Select

    End Function

    Public Sub InitializeCurrencyCultureMapping()

        ' The CurrencyCultureMapping Dictionary keeps track of which currency maps to which culture.
        CurrencyCultureMapping.Add("USD", "en-US")
        CurrencyCultureMapping.Add("CAD", "en-CA")
        CurrencyCultureMapping.Add("GBP", "en-GB")

        ' Primary Currencies are those currencies which represent the default currency for any of our regions (for example, in the
        ' PN region, USD would be the primary currency, while CAD would be secondary).
        PrimaryCurrency.Add("USD")
        PrimaryCurrency.Add("GBP")

    End Sub


    'TFS 8325, 03/30/2013, Faisal Ahmed, Check whether invoice costs and refused quantities are populated for all refused items
    Public Function IsValidRefusedItemList(ByVal iOrderHeader_ID As Integer) As Boolean
        logger.Debug("IsValidRefusedItemList Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim returnVal As Boolean = False

        ' Execute the function
        returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_IsValidRefusedItemList(" & CStr(iOrderHeader_ID) & ")"), Boolean)

        logger.Debug("IsValidRefusedItemList Exit")
        Return returnVal
    End Function
End Module
