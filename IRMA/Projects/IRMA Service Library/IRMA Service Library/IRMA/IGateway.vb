Namespace IRMA

    ''' <summary>
    ''' The primary service contact for IRMA services.
    ''' </summary>
    ''' <remarks></remarks>
    <ServiceContract()>
    Public Interface IGateway

#Region " Read Operation Contracts"
        ''' <summary>
        ''' Contract for retrieving a list of user roles for the current user.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetUserRole(ByVal UserName As String) As List(Of Security.UserRole)

        <OperationContract()>
        Function GetServerName() As String

        <OperationContract()>
        Function GetApplicationConfig(ByVal EnvironmentGUID As String, ByVal ApplicationGUID As String) As String

        <OperationContract()>
        Function GetStoreFtpConfigByStoreAndWriterType(ByVal Store_No As Integer, ByVal FileWriterType As String) As List(Of Lists.StoreFTPConfig)

        <OperationContract()>
        Function GetStoreFtpConfigDataForWriterType(ByVal fileWriterType As String) As List(Of Lists.StoreFTPConfig)

        ''' <summary>
        ''' Contract for retrieving list of ItemUnits
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetItemUnits() As List(Of Lists.ItemUnit)

        ''' <summary>
        ''' Contract for retrieving a list of Shrink Adjustment Reasons from the database.
        ''' </summary>
        ''' <returns>Generic List Collection of Adjustment Reasons</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetShrinkAdjustmentReasons() As List(Of Lists.ShrinkAdjustmentReason)

        ''' <summary>
        ''' Contract for retrieving a list of Shrink Types
        ''' </summary>
        ''' <returns></returns>
        <OperationContract()>
        Function GetShrinkSubTypes() As List(Of ShrinkSubType)

        ''' <summary>
        ''' Contract for retrieving a list of Item Information from the database.
        ''' </summary>
        ''' <param name="Item_Key">ItemKey, this can be null</param>
        ''' <param name="Identifier">Item Identifier</param>
        ''' <returns>Generic List Collection of Item Information</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetItem(ByVal Item_Key As Integer, ByVal Identifier As String) As List(Of Lists.GetItem)

        ''' <summary>
        ''' Contract for retrieving a instance data flags from the database.
        ''' </summary>
        ''' <param name="FlagKey">Name of the Instance Data Flag</param>
        ''' <param name="Store_No">Store Number to check Overrides, can be null</param>
        ''' <returns>Generic List Collection of Item Information</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetInstanceDataFlag(ByVal FlagKey As String, ByVal Store_No As Integer) As Boolean

        ''' <summary>
        ''' Contract for retrieving a list of discount reason codes from the database.
        ''' </summary>
        ''' <returns>Generic List Collection of of discount reason codes for the region</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetRefusedItemsReasonCodes() As List(Of Lists.ReasonCode)

        ''' <summary>
        ''' Contract for retrieving a list of refused items reason codes from the database.
        ''' </summary>
        ''' <returns>Generic List Collection of of refused items reason codes for the region</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetDiscountReasonCodes() As List(Of Lists.ReasonCode)

        ''' <summary>
        ''' Contract for retrieving a list of Subteams from the database.
        ''' </summary>
        ''' <returns>Generic List Collection of subteams for the region</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetSubteams() As List(Of Lists.Subteam)

        ''' <summary>
        ''' Contract for retrieving a list of Subteams By Product Type from the database.
        ''' </summary>
        ''' <returns>Generic List Collection of subteams for the region</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetSubteamByProductType(ByVal ProductType_ID As Integer) As List(Of Lists.Subteam)

        ''' <summary>
        ''' Contract for retrieving a list of Stores from the database.
        ''' </summary>
        ''' <returns>Generic List Collection of stores for the region</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetStores(Optional ByVal isVendorIDUsed As Boolean = False) As List(Of Lists.Store)

        <OperationContract()>
        Function GetInventorylocations(ByVal lStoreNo As Long, ByVal lSubTeamNo As Long) As List(Of InventoryLocation)

        ''' <summary>
        ''' Contract for Contract for retrieving list of ItemMovement
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetItemMovement(ByVal lstoreNo As Integer,
                                 ByVal iTransferToSubteam_To As Integer, _
                                 ByVal sIdentifier As String, _
                                 ByVal adjustmentID As Integer) As List(Of Lists.ItemMovement)

        ''' <summary>
        ''' Contract for get the quantity of billed, bot not received quantity
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetItemBilledQuantity(ByVal lstoreNo As Integer,
                                 ByVal iTransferToSubteam_To As Integer, _
                                 ByVal sIdentifier As String) As List(Of Lists.ItemBilledQty)

        ''' <summary>
        ''' Contract for get vendor package
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetVendorPackage(ByVal lstoreNo As Integer,
                                 ByVal vendorID As Integer, _
                                 ByVal sIdentifier As String) As Decimal

        ''' <summary>
        ''' Contract for retrieving a list of DSD Vendors By Store No. from the database.
        ''' <param name="Store_No">Store Number to filter DSD vendors, cannot be null</param>
        ''' </summary>
        ''' <returns>Generic List of DSD vendors for the store</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetDSDVendors(ByVal iStoreNo As Integer) As List(Of DSDVendor)

        ''' <summary>
        ''' Contract for retrieving a list of reason codes by type.
        ''' <param name="reasonCodeType">The two-character reason code type.</param>
        ''' </summary>
        ''' <returns>List of reason codes of a specified type.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetReasonCodesByType(ByVal reasonCodeType As String) As List(Of ReasonCode)

        ''' <summary>
        ''' Contract for retrieving a list of subteams with GL Account numbers.reason codes by type.
        ''' <param name="orderHeaderID">The OrderHeader_ID.</param>
        ''' </summary>
        ''' <returns>List of subteams with GL accounts.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetGLAccountSubteams(ByVal orderHeaderID As Integer) As List(Of Lists.Subteam)

        ''' <summary>
        ''' Contract for retrieving a list of allocated charges.
        ''' <param></param>
        ''' </summary>
        ''' <returns>List of allocated charges.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetAllocatedCharges() As List(Of InvoiceCharge)

        ''' <summary>
        ''' Contract for reparsing an eInvoice.
        ''' <param name="eInvoiceID">The eInvoiceID.</param>
        ''' </summary>
        ''' <returns>Result object.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function ReparseEinvoice(ByVal eInvoiceID As Integer) As Result

        ''' <summary>
        ''' Contract for getting information about available currencies.
        ''' <param></param>
        ''' </summary>
        ''' <returns>List of Currency.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetCurrencies() As List(Of Currency)

        ''' <summary>
        ''' Contract for counting the OrderItems which need a sustainability ranking.
        ''' <param name="orderHeaderID">The OrderHeader_ID.</param>
        ''' </summary>
        ''' <returns>Integer count of OrderItems.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function CountSustainabilityRankingRequiredItems(ByVal orderHeaderID As Integer) As Integer

        ''' <summary>
        ''' Contract for getting the system time from the database.
        ''' <param></param>
        ''' </summary>
        ''' <returns>A DateTime object representing the current server date.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetSystemDate() As DateTime

        ''' <summary>
        ''' Contract for calculating the result of the Item Unit of Measure conversion
        ''' <param></param>
        ''' </summary>
        ''' <returns>A Decimal of the calculation.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function CalculateConversion(ByVal InUnit As String, ByVal OutUnit As String, ByVal Amount As Decimal) As Decimal

        ''' <summary>
        ''' Contract for calculating the result of the Item Unit of Measure conversion
        ''' <param></param>
        ''' </summary>
        ''' <returns>A Decimal of the calculation.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetInUnits() As String()

        ''' <summary>
        ''' Contract for calculating the result of the Item Unit of Measure conversion
        ''' <param></param>
        ''' </summary>
        ''' <returns>A Decimal of the calculation.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetOutUnits(ByVal InUnit As String) As String()

        ''' <summary>
        ''' Contract for retrieving the currency of an order.
        ''' <param></param>
        ''' </summary>
        ''' <returns>The currency code for the order.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function GetOrderCurrency(ByVal orderHeaderId As Integer, ByVal regionCode As String) As String

        ''' <summary>
        ''' Contract for checking for duplicate invoice numbers on receiving documents.
        ''' <param></param>
        ''' </summary>
        ''' <returns>True if duplicates found, otherwise false.</returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function IsDuplicateReceivingDocumentInvoiceNumber(ByVal InvoiceNumber As String, ByVal VendorId As Integer) As Boolean

#End Region

#Region " Write Operation Contracts"

#Region " Inventory/Shrink"

        ''' <summary>
        ''' Contract for inserting a new Shrink Adjustment into the database.
        ''' </summary>
        ''' <param name="Adjustment"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <OperationContract()>
        Function AddShrinkAdjustment(ByVal Adjustment As Inventory.Shrink) As Boolean

#End Region
#Region " Ordering and Receiving"

        <OperationContract()>
        Function GetStoreItem(ByVal iStoreNo As Integer, _
                                     ByVal iTransferToSubteam_To As Integer, _
                                     ByVal iUser_ID As Integer, _
                                     ByVal iItem_Key As Integer, _
                                     ByVal sIdentifier As String) As StoreItem

        <OperationContract()>
        Function AddToReprintSignQueue(ByVal lUser_ID As Long, _
                                         ByVal iSourceType As Integer, _
                                         ByVal sItemList As String,
                                         ByVal sItemListSeperator As String, _
                                         ByVal iStoreNo As Integer) As Boolean
        <OperationContract()>
        Function AddToOrderQueue(ByVal IsTransfer As Boolean, _
                                ByVal IsCredit As Boolean, _
                                ByVal Quantity As Decimal, _
                                ByVal UnitID As Integer, _
                                ByVal iUser_ID As Integer, _
                                ByRef si As StoreItem) As Boolean

        <OperationContract()>
        Function ReceiveOrderItem(ByVal dQuantity As Decimal, _
                                    ByVal dWeight As Decimal, _
                                    ByVal dtDate As DateTime, _
                                    ByVal bCorrection As Boolean, _
                                    ByVal iOrderItem_ID As Integer, _
                                    ByVal reasonCodeID As Integer, _
                                    Optional ByVal dPackSize As Decimal = 0, _
                                    Optional ByVal UserID As Long = 0) As Result

        <OperationContract()>
        Function UpdateRefusedQuantity(ByVal orderHeaderID As Integer, ByVal Identifier As String, ByVal Quantity As Decimal) As Result

        <OperationContract()>
        Function GetRefusedQuantity(ByVal orderHeaderID As Integer, ByVal Identifier As String) As Decimal

        <OperationContract()>
        Function InsertOrderItemRefused( _
                        ByVal iOrderHeader_ID As Integer, _
                        ByVal iOrderItem_ID As Integer, _
                        ByVal sIdentifier As String, _
                        ByVal sVIN As String, _
                        ByVal sDescription As String, _
                        ByVal sUnit As String, _
                        ByVal dInvoiceQuantity As Decimal, _
                        ByVal dInvoiceCost As Decimal, _
                        ByVal reasonCodeID As Integer) As Result

        <OperationContract()>
        Function UpdateOrderItemRefused( _
                        ByVal iOrderItemRefused_ID As Integer, _
                        ByVal sIdentifier As String, _
                        ByVal sVIN As String, _
                        ByVal sDescription As String, _
                        ByVal sUnit As String, _
                        ByVal dInvoiceQuantity As Decimal, _
                        ByVal dInvoiceCost As Decimal, _
                        ByVal reasonCodeID As Integer, _
                        ByVal userAddedEntry As Integer) As Result

        <OperationContract()>
        Function UpdateRefusedItemsList(ByVal columnValuesList As String, ByVal iSeparator1 As String, ByVal iSeparator2 As String) As Result

        <OperationContract()>
        Function DeleteOrderItemRefused(ByVal iOrderItemRefused_ID As Integer) As Result

        <OperationContract()>
        Function GetRefusedTotal(ByVal iOrderHeader_ID As Integer) As Decimal

        <OperationContract()>
        Function GetOrder(ByVal lOrderID As Long) As Order

        <OperationContract()>
        Function GetExternalOrders(ByVal lExternalSourceOrderID As Integer, iStore_No As Integer) As List(Of Lists.ExternalOrder)

        <OperationContract()>
        Function GetStoreItemOrderInfo(ByVal iStoreNo As Integer, _
                                       ByVal iTransferToSubteamNo As Integer, _
                                       ByVal iItemKey As Integer) As StoreItemOrderInfo

        <OperationContract()>
        Function GetStoreItemCycleCountInfo(ByVal si As StoreItem, _
                                           Optional ByVal lInventoryLocationID As Long = 0) As CycleCountInfo

        <OperationContract()>
        Function GetTransferItem(ByVal iItem_Key As Integer, _
                             ByVal sIdentifier As String, _
                             ByVal iProductType_ID As Integer, _
                             ByVal iVendorStore_No As Integer, _
                             ByVal iVendor_ID As Integer, _
                             ByVal iTransfer_SubTeam As Integer, _
                             ByVal iSupplySubTeam_No As Integer) As StoreItem

        <OperationContract()>
        Function CreateDSDOrder(ByVal O As Order) As Result

        <OperationContract()>
        Function CreateTransferOrder(ByVal O As Order) As Result

        <OperationContract()>
        Function CreateOrder(ByVal O As Order) As Result

        <OperationContract()>
        Function SendOrder(ByVal O As Order) As Result

        <OperationContract()>
        Function ReceiveOrder(ByVal O As Order) As Result

        <OperationContract()>
        Function CloseOrder(ByVal iOrderHeader_ID As Integer, ByVal iUser_ID As Integer) As Result

        <OperationContract()>
        Function DeleteOrder(ByVal O As Order) As Boolean

        <OperationContract()>
        Function IsValidRefusedItemList(ByVal orderHeaderID As Integer) As Boolean

        <OperationContract()>
        Function IsRefusalAllowed(ByVal orderHeaderID As Integer) As Boolean

        <OperationContract()>
        Function StoreSubTeamRelationshipExists(ByVal sStore_No As String, ByVal sTransfer_To_SubTeam As String) As Boolean

        <OperationContract()>
        Function GetOrderHeaderByIdentifier(ByVal UPC As String, ByVal storeNumber As Integer) As List(Of Order)

        <OperationContract()>
        Function GetReceivingListEinvoiceExceptions(ByVal iOrderHeader_ID As Integer) As List(Of OrderItem)

        <OperationContract()>
        Function GetOrderItemsRefused(ByVal iOrderHeader_ID As Integer) As List(Of OrderItemRefused)

        <OperationContract()>
        Function UpdateReceivingDiscrepancyCode(ByVal iReasonCodeList As String, ByVal iSeparator1 As String, ByVal iSeparator2 As String) As Result

        <OperationContract()>
        Function GetOrderInvoiceCharges(ByVal orderHeaderID As Integer) As List(Of InvoiceCharge)

        <OperationContract()>
        Function IsDSDStoreVendorByUPC(ByVal UPC As String, ByVal Store_No As Integer) As Boolean

        <OperationContract()>
        Function UpdateOrderBeforeClose(ByVal OrderHeader_ID As Integer, _
                                        ByVal InvoiceNumber As String, _
                                        ByVal InvoiceDate As Date, _
                                        ByVal InvoiceCost As Decimal, _
                                        ByVal VendorDoc_ID As String, _
                                        ByVal VendorDocDate As Date, _
                                        ByVal SubTeam_No As Integer, _
                                        ByVal PartialShipment As Boolean) As Result

        <OperationContract()>
        Function AddInvoiceCharge(ByVal orderHeaderID As Integer, ByVal SACTypeID As Integer, ByVal description As String, _
                                  ByVal subteamNumber As Integer, ByVal allowance As Boolean, ByVal amount As Decimal) As Result

        <OperationContract()>
        Function RemoveInvoiceCharge(ByVal chargeID As Integer) As Result

        <OperationContract()>
        Function CheckInvoiceNumber(ByVal iVendor_ID As Integer, ByVal iInvoiceNumber As String, ByVal iOrderHeader_ID As Integer) As Result

        <OperationContract()>
        Function ReOpenOrder(ByVal OrderHeader_ID As Integer) As Result

        <OperationContract()>
        Function RefuseReceiving(ByVal orderHeaderID As Integer, ByVal userID As Integer, refuseReceivingReasonCodeID As Integer) As Result

        <OperationContract()>
        Function UpdateOrderHeaderCosts(ByVal orderHeaderID As Integer) As Result

#End Region
#Region " Cycle Count"

        <OperationContract()>
        Function GetCycleCount(ByVal lStoreNo As Long, ByVal lSubTeamNo As Long) As CycleCount

        <OperationContract()>
        Function CreateCycleCountHeader(ByVal lMasterCountID As Long, _
                                               ByVal dStartScan As DateTime, _
                                               ByVal lInventoryLocationId As Long, _
                                               ByVal bExternal As Boolean) As Object

        <OperationContract()>
        Sub AddCycleCountItem(ByVal lItemKey As Long, _
                                     ByVal dQuantity As Decimal, _
                                     ByVal dWeight As Decimal, _
                                     ByVal dPackSize As Decimal, _
                                     ByVal bIsCaseCnt As Boolean, _
                                     ByVal lCycleCountID As Long, _
                                     Optional ByVal lInvLocID As Long = Nothing)
#End Region
#Region " POS Push"
        <OperationContract()>
        Function RunPOSPush(ByVal sAppPath As String, ByVal sRegion As String, ByVal sConnectionString As String, ByVal sEmailAddress As String) As Boolean
#End Region
#Region "Mars"
        <OperationContract()>
        Function SetStopSaleForItem(ByVal storeNumber As Integer, ByVal itemIdentifier As String, ByVal stopSale As Boolean) As Boolean
#End Region

#End Region

#Region " Debug Operation Contracts"

        <OperationContract()>
        Function ReturnServiceConnectionString() As String

        <OperationContract()>
        Function LoggerTest() As String

#End Region

    End Interface

End Namespace