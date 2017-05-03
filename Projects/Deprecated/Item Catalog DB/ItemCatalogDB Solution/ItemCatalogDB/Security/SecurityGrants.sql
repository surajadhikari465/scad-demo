----------------------------------------------------
-- This file contains 3 sections:
--- Functions
--    * Functions that return a value
--    * Functions that return a table
--ici
--- Stored Procedures
--
--- Table Grants
----------------------------------------------------

SET NOCOUNT ON
GO

--*********************
--     Functions
--*********************

grant exec on dbo.fn_AvgCostHistory to IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.fn_CalcBarcodeCheckDigit to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_CompPrice to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_CompRegPrice to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_ConvertItemUnit to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_ConvertVarBinaryToHex to IRMAClientRole, IRMAAdminRole, IRMASupportRole
grant exec on dbo.fn_CostConversion to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_CurAvgCostHistory to IRMAClientRole, IRMAReportsRole
grant exec on dbo.fn_CurOnHoldQtyAvgCostHistory to IRMAClientRole, IRMAReportsRole
grant exec on dbo.fn_EDLP_Price to IRMAClientRole
grant exec on dbo.fn_EDLP_UnitPrice to IRMAClientRole
grant exec on dbo.fn_EDLP_UnitsPerPrice to IRMAClientRole
grant exec on dbo.fn_EIM_GetListOfItemChains TO IRMAClientRole
grant exec on dbo.fn_EIM_GetListOfUploadTypes to IRMAClientRole
grant exec on dbo.fn_FiscalYearBeginDate to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.fn_FormatSmartXScaleTares to IRMAAdminRole, IRMAClientRole
grant exec on dbo.fn_GET_DRLUOM_for_item to IRMAClientRole
grant exec on dbo.fn_GetAlternateUPC to IRMAReportsRole
grant exec on dbo.fn_GetAverageCostDateRange to IRMAReportsRole
grant exec on dbo.fn_GetAvgNetCostDateRange to IRMAReportsRole
grant exec on dbo.fn_GetCurrentAvgCost TO IRMAAdminRole, IRMAReportsRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_IsFixedSpoilageSubTeam TO IRMAAdminRole, IRMAReportsRole, IRMAClientRole
grant exec on dbo.fn_ItemSalesQty2 to IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_HasScaleIdentifier to IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_CaseUpchargePct to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_GetDistributionUnitAbbreviation to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_LoadExternalCycleCountEOP to IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_GetLastItemPOID to IRMAAdminRole, IRMAReportsRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant select on dbo.fn_GetEnumSubTeamTypes to IRMAReportsRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_GetRegFacID to IRMAReportsRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_IsUserAssignedToTeam to IRMAClientRole, IRMASupportRole
grant exec on dbo.fn_GetSystemDateTime to IRMAReportsRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant select on dbo.fn_GetOrderAllocItems to IRMAAdminRole
grant select on dbo.fn_GetOrderAllocItems to IRMAClientRole
grant select on dbo.fn_GetOrderAllocItems to IRMASchedJobsRole
grant select on dbo.fn_GetOrderAllocItems to public
grant exec on dbo.fn_GetJobStatus TO  IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.fn_IsRegionWithMutipleJurisdiction to IRMAClientRole
grant exec on dbo.fn_ReceiveUPCPLUUpdateFromIcon to IRMAClientRole, IRSUser
grant exec on dbo.fn_IsItemValidated to IRMAClientRole, IRSUser
grant select on dbo.fn_ItemIdentifiers TO IRMAClientRole, IRSUser
grant select on dbo.fn_GetItemIdentifiers TO IRMAClientRole, IRSUser
grant exec on dbo.fn_ValidatedScanCodeExists TO IRSUser, IRMAClientRole
grant execute on dbo.GetNonAlignedSubteamNames to IRMAClientRole
grant execute on dbo.fn_HasAlignedSubteam to IRMAClientRole
grant select on dbo.fn_GetSalesSumByItem to IRSUser, IRMAClientRole

-- tables
grant update, insert, delete on dbo.LabelType to IRMAAdminRole
grant update, insert, delete on dbo.MenuAccess to IRMAAdminRole
grant update, insert, delete on dbo.Region to IRMAAdminRole
grant update, insert, delete on dbo.Zone to IRMAAdminRole
grant update, insert, delete on dbo.Store to IRMAAdminRole
grant update, insert, delete on dbo.Team to IRMAAdminRole
grant update, insert, delete on dbo.SubTeam to IRMAAdminRole
grant update, insert, delete on dbo.Users to IRMAAdminRole
grant update, insert, delete on dbo.StoreSubTeam to IRMAAdminRole
grant update, insert, delete on dbo.ItemCategory to IRMAAdminRole
grant update, insert, delete on dbo.ItemUnit to IRMAAdminRole
grant update, insert, delete on dbo.ItemCategory to IRMAAdminRole
grant update, insert, delete on dbo.UserStoreTeamTitle to IRMAAdminRole
grant update, insert, delete on dbo.RuleDef to IRMAAdminRole
grant update, insert, delete on dbo.ExRule_VendCostPackSize to IRMAAdminRole
grant update, insert, delete on dbo.ExRule_VendCostDiff to IRMAAdminRole
grant update, insert, delete on dbo.ItemType to IRMAAdminRole
grant update, insert, delete on dbo.TaxJurisdiction to IRMAAdminRole
grant update, insert, delete on dbo.TaxDefinition to IRMAAdminRole
grant update, insert, delete on dbo.TaxFlag to IRMAAdminRole
grant update, insert, delete on dbo.tlog_cmcard to IRMAAdminRole
grant update, insert, delete on dbo.tlog_cmreserve to IRMAAdminRole
grant update, insert, delete on dbo.tlog_cmreward to IRMAAdminRole
grant update, insert, delete on dbo.tlog_discnt to IRMAAdminRole
grant update, insert, delete on dbo.tlog_item to IRMAAdminRole
grant update, insert, delete on dbo.tlog_mrkdwn to IRMAAdminRole
grant update, insert, delete on dbo.tlog_stores to IRMAAdminRole
grant update, insert, delete on dbo.tlog_taxrec to IRMAAdminRole
grant update, insert, delete on dbo.tlog_tender to IRMAAdminRole
grant update, insert, delete on dbo.POSWriterEscapeChars to IRMAAdminRole
grant update, insert, delete on dbo.POSWriterDictionary to IRMAAdminRole
grant update, insert, delete on dbo.POSItem to IRMAAdminRole, IRMAClientRole
grant update, insert, delete on dbo.Temp_PriceAudit to IRMAAdminRole
grant update, insert, delete on dbo.StoreFTPConfig to IRMAAdminRole
grant insert, update, delete on dbo.UsersSubTeam to IRMAAdminRole
grant insert, update, delete on dbo.VendorRequest to IRMAAdminRole
grant insert, update, delete on dbo.ItemRequest to IRMAAdminRole
grant insert, update, delete on dbo.SLIMAccess to IRMAAdminRole
grant select, insert, update, delete on dbo.Reporting_PIRIS_Audit to IRMAAdminRole
grant select, insert, update, delete on dbo.POSAuditException to IRMAAdminRole
grant select on dbo.AttributeIdentifier to IRMAAdminRole
grant select on dbo.Scale_Tare to IRMAAdminRole
grant select on dbo.Scale_EatBy to IRMAReportsRole
grant select on dbo.Scale_Grade to IRMAReportsRole
grant select on dbo.Scale_ExtraText to IRMAReportsRole
grant select on dbo.Nutrifacts to IRMAAdminRole
grant select on dbo.POSAuditExceptionType to IRMAAdminRole
grant select on dbo.ItemIdentifier to IRMAAdminRole
grant select on dbo.ItemOverride to IRMAAdminRole
grant select on dbo.ItemScaleOverride to IRMAAdminRole
grant select on dbo.StoreItemVendor to IRMAAdminRole
grant select on dbo.Vendor to IRMAAdminRole
grant select on dbo.ItemVendor to IRMAAdminRole
grant select on dbo.ItemScale to IRMAAdminRole
grant select on dbo.Scale_ExtraText to IRMAAdminRole
grant select on dbo.Scale_Grade to IRMAAdminRole
grant select on dbo.ItemUnit to IRMAAdminRole
grant select on dbo.ItemBrand to IRMAAdminRole
grant select on dbo.PriceChgType to IRMAAdminRole
grant select on dbo.OrderTransmissionOverride to IRMAReportsRole
grant select on dbo.PayOrderedCost to IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant select on dbo.ResolutionCodes to IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant select, alter on dbo.PosPushStagingPriceBatchDetail to IRMAClientRole, IRSUser

grant update, insert, select, delete, alter on dbo.Warehouse_Inventory to IRMAAdminRole
grant update, insert, select, delete, alter on dbo.Warehouse_Inventory to IRMAClientRole
grant update, insert, select, delete, alter on dbo.Warehouse_Inventory to IRMASchedJobsRole

grant update, insert, select, delete, alter on dbo.ConfigurationData to IRMAAdminRole
grant update, insert, select, delete, alter on dbo.ConfigurationData to IRMAClientRole
grant update, insert, select, delete, alter on dbo.ConfigurationData to IRMASchedJobsRole

grant update, insert, select, delete, alter on dbo.ValidatedScanCode to IRSUser, IRMAClientRole
grant update, insert, select, delete, alter on dbo.ValidatedBrand to IRSUser, IRMAClientRole
grant update, insert, select, delete, alter on dbo.ItemSignAttribute to IRSUser, IRMAClientRole
grant select on dbo.ItemSignAttribute to iCONReportingRole

--functions
grant select on dbo.fn_VendorCostAll to IRMAAdminRole

go

---- IRMAClientRole ----

--stored procedures
grant exec on dbo.IsRetailSaleItem to IRSUser, IRMAClientRole
grant exec on dbo.IsValidatedItemInIcon to IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_CheckTlogExists to IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPush_GetRegionFTPConfig to IRMAClientRole, IRMASchedJobsRole
grant exec on [dbo].[GetFixedSpoilageFlag] to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on [dbo].[GetItemIdentifersForItem]  to IRMAClientRole
grant exec on dbo.UpdateOrderItemQuantityShipped to IRMAClientRole
grant exec on dbo.GetAllOrderExternalSource to IRMAClientRole
grant exec on dbo.rptCycleCountMaster to IRMAClientRole, IRMAReportsRole
grant exec on dbo.CheckForItemChangeDifferences to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateDiscountRecord to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateItemRecord to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreatePaymentRecord to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateTransactionRecord to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_UpdateSalesAggregates to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateOfferRecord to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateVoidRecord to IRMAClientRole
grant exec on dbo.EInvoicing_GetConfigInfo to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.EPromotions_GetOfferIdFromPriceBatchDetailId to IRMAClientRole
grant exec on dbo.EPromotions_ShowLockedPromotions to IRMAClientRole
grant exec on dbo.EPromotions_ShowLockedGroups to IRMAClientRole
grant exec on dbo.EPromotions_UnlockGroup to IRMAClientRole
grant exec on dbo.EPromotions_IsGroupInCurrentPromotion to IRMAClientRole
grant exec on dbo.EPromotions_GetPriceBatchDetailsByOfferItem  to IRMAClientRole
grant exec on dbo.EPromotions_DeletePromotionalOffer to IRMAClientRole
grant exec on dbo.EPromotions_SetGroupEditStatus to IRMAClientRole
grant exec on dbo.EPromotions_GetGroupEditStatus to IRMAClientRole
grant exec on dbo.EPromotions_PromotionalOffer_SetEditFlag  to IRMAClientRole
grant exec on dbo.EPromotions_ReturnStoreActiveFlag to IRMAClientRole
grant exec on dbo.EPromotions_DeleteUnbatchedPriceBatchDetails to IRMAClientRole
grant exec on dbo.EPromotions_ReturnUnbatchedPriceBatchDetailCount to IRMAClientRole
grant exec on dbo.EPromotions_GetPromotionPriceBatchDetail to IRMAClientRole
grant exec on dbo.EPromotions_GetOffersByGroupID to IRMAClientRole
grant exec on dbo.EPromotions_ValidatePromotionName to IRMAClientRole
grant exec on dbo.GetPricingMethodInfo to IRMAClientRole
grant exec on dbo.GetOrderItemLines to IRMAClientRole
grant exec on dbo.UpdateOrderHeaderFreight3Party to IRMAClientRole
grant exec on dbo.UpdateOrderItemFreight3PartyAll to IRMAClientRole
grant exec on dbo.UpdateOrderItemFreight3PartyOne to IRMAClientRole
grant exec on dbo.UpdateOrderRefreshCosts to IRMAClientRole
grant exec on dbo.CostConversion  to IRMAClientRole
grant exec on dbo.UpdateOrdersApplyNewVendorCost to IRMAClientRole
grant exec on dbo.GetItemManagers to IRMAClientRole
grant exec on dbo.GetItemUnitsCost to IRMAClientRole
grant exec on dbo.GetStoreTlogFtpInfo to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetPOSWriterDictionary to IRMAClientRole
grant exec on dbo.DeptComp to IRMAClientRole
grant exec on dbo.Takings to IRMAClientRole
grant exec on dbo.ItemHosting_GetLabelType to IRMAClientRole
grant exec on dbo.EPromotions_GetStoresByPromotionId to IRMAClientRole
grant exec on dbo.GetSubTeamVendors to IRMAClientRole
grant exec on dbo.GetItemUnitsPDU to IRMAClientRole
GRANT EXEC on dbo.Administration_UserAdmin_GetAllUsers to IRMAClientRole
grant exec on dbo.DeptKeyUsage to IRMAClientRole
grant exec on dbo.TaxReport1 to IRMAClientRole
grant exec on dbo.ItemHosting_UpdateStore to IRMAClientRole
grant exec on dbo.GetSupplierRetailSubteam to IRMAClientRole
grant exec on dbo.GetItemUnitsVendor to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetPOSWriterFileConfigRowCount to IRMAClientRole
grant exec on dbo.DiscontinuedItemsWithInventory to IRMAClientRole
grant exec on dbo.TaxReport2 to IRMAClientRole
grant exec on dbo.GetSupplierSubteam to IRMAClientRole
grant exec on dbo.GetItemVendors to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetPOSWriterFileConfigurationForEdit to IRMAClientRole
grant exec on dbo.DistInvFrght to IRMAClientRole
grant exec on dbo.TaxReport3 to IRMAClientRole
grant exec on dbo.ItemListReport to IRMAClientRole
grant exec on dbo.GetSupplierSubteamByVendor to IRMAClientRole
grant exec on dbo.GetItemVendorStores to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetPOSWriters to IRMAClientRole
grant exec on dbo.TestOrgPO to IRMAClientRole
grant exec on dbo.ItemOnHandComparisonBetweenLocation to IRMAClientRole
grant exec on dbo.GetSystemDate to IRMAClientRole
grant exec on dbo.GetItemVideo to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetStoresAvailableForAdd to IRMAClientRole
grant exec on dbo.ESSBaseQuery to IRMAClientRole
grant exec on dbo.TGMToolGetAcctTotals to IRMAClientRole
grant exec on dbo.ItemPriceReport to IRMAClientRole
grant exec on dbo.GetSystemDateCmd to IRMAClientRole
grant exec on dbo.GetItemVideoLastID to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetStoresByWriter to IRMAClientRole
grant exec on dbo.ExReport to IRMAClientRole
grant exec on dbo.TGMToolGetDataAll to IRMAClientRole
grant exec on dbo.ItemSalesByStoreComp to IRMAClientRole
grant exec on dbo.GetTeamByStoreSubTeam to IRMAClientRole
grant exec on dbo.GetItemVideoList to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetTaxFlagKeys to IRMAClientRole
grant exec on dbo.FindOpenOrders to IRMAClientRole
grant exec on dbo.TGMToolGetDataBrand to IRMAClientRole
grant exec on dbo.ItemTypeList to IRMAClientRole
grant exec on dbo.GetTeamBySubTeam to IRMAClientRole
grant exec on dbo.Administration_POSPush_InsertPOSWriter to IRMAClientRole
grant exec on dbo.FiscalCompSales to IRMAClientRole
grant exec on dbo.TGMToolGetDataCategory to IRMAClientRole
grant exec on dbo.ItemVendorCostCurrent to IRMAClientRole
grant exec on dbo.GetTeams to IRMAClientRole
grant exec on dbo.GetLineDrivePreUpdate to IRMAClientRole
grant exec on dbo.Administration_POSPush_InsertPOSWriterFileConfig to IRMAClientRole
grant exec on dbo.FiscalMonthAllStores to IRMAClientRole
grant exec on dbo.TGMToolGetDataVendor to IRMAClientRole
grant exec on dbo.ItemVendorCostSearch to IRMAClientRole
grant exec on dbo.GetTranCount to IRMAClientRole
grant exec on dbo.GetNatClass to IRMAClientRole
grant exec on dbo.Administration_POSPush_InsertStorePOSConfig to IRMAClientRole
grant exec on dbo.FiscalMonthAllStoresLastYear to IRMAClientRole
grant exec on dbo.LoadCheck to IRMAClientRole
grant exec on dbo.GetUnAvailStores to IRMAClientRole
grant exec on dbo.GetOpenDistributionOrders to IRMAClientRole
grant exec on dbo.Administration_POSPush_PopulatePIRUSConfigData to IRMAClientRole
grant exec on dbo.FiscalMonthSales to IRMAClientRole
grant exec on dbo.ThirteenWeekMovementReportDist to IRMAClientRole
grant exec on dbo.LoadCycleCountExternal to IRMAClientRole
grant exec on dbo.GetUNFIOrder to IRMAClientRole
grant exec on dbo.InsertPromoPlannerFromEIM to IRMAClientRole
grant exec on dbo.GetOrderAllocItems to IRMAClientRole
grant exec on dbo.Administration_POSPush_UpdatePOSWriter to IRMAClientRole
grant exec on dbo.GetAdjustmentInfo to IRMAClientRole
grant exec on dbo.GetInventoryAdjustmentCodeList to IRMAClientRole
grant exec on dbo.GetInventoryAdjustmentIDFromCode to IRMAClientRole
grant exec on dbo.GetInventoryAdjustmentAllows to IRMAClientRole
grant exec on dbo.TopMovers to IRMAClientRole
grant exec on dbo.LoadPMPriceChange to IRMAClientRole
grant exec on dbo.GetUnitAndID to IRMAClientRole
grant exec on dbo.GetKitchenRoutes to IRMAClientRole
grant exec on dbo.GetOrderAllocOrderItems to IRMAClientRole
grant exec on dbo.Administration_POSPush_UpdatePOSWriterFileConfig to IRMAClientRole
grant exec on dbo.GetAdjustmentInfoFirst to IRMAClientRole
grant exec on dbo.TopMoversList to IRMAClientRole
grant exec on dbo.LoadSalesData2 to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateItemRecord to IRMAClientRole
grant exec on dbo.GetUnitInfo to IRMAClientRole
grant exec on dbo.GetOrderEmail to IRMAClientRole
grant exec on dbo.Administration_POSPush_UpdatePOSWriterFileConfigOrder to IRMAClientRole
grant exec on dbo.GetAllDistributionCenters to IRMAClientRole
grant exec on dbo.TopMoversSummary to IRMAClientRole
grant exec on dbo.LockBrand to IRMAClientRole
grant exec on dbo.GetUnitInfoFirst to IRMAClientRole
grant exec on dbo.GetOrderHeaderDesc to IRMAClientRole
grant exec on dbo.Administration_POSPush_UpdateStorePOSConfig to IRMAClientRole
grant exec on dbo.GetAllOnHand to IRMAClientRole
grant exec on dbo.TYDeptComp to IRMAClientRole
grant exec on dbo.LockCategory to IRMAClientRole
grant exec on dbo.EPromotions_RemoveStoreFromPromotion to IRMAClientRole
grant exec on dbo.GetUnitInfoLast to IRMAClientRole
grant exec on dbo.GetPriceBatchItemSearch to IRMAClientRole
grant exec on dbo.GetOrderHeaderLockStatus to IRMAClientRole
grant exec on dbo.GetAllStores to IRMAClientRole
grant exec on dbo.UnitCount7Day to IRMAClientRole
grant exec on dbo.LockFSCustomer to IRMAClientRole
grant exec on dbo.GetUnitLockStatus to IRMAClientRole
grant exec on dbo.GetOrderInfo to IRMAClientRole
grant exec on dbo.GetAllStoreUsers to IRMAClientRole
grant exec on dbo.UnitCount7DayByStore to IRMAClientRole
grant exec on dbo.LockFSOrganization to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_GetIdentifierDeletes to IRMAClientRole
grant exec on dbo.GetUnitWeight to IRMAClientRole
grant exec on dbo.GetAllSubTeams to IRMAClientRole
grant exec on dbo.UnitCountTotalAllStores_CrossTab to IRMAClientRole
grant exec on dbo.Acct4YrComp to IRMAClientRole
grant exec on dbo.LockItem to IRMAClientRole
grant exec on dbo.GetUrgentVCAI_Exceptions to IRMAClientRole
grant exec on dbo.GetANSOrderHeader to IRMAClientRole
grant exec on dbo.UnlockBrand to IRMAClientRole
grant exec on dbo.AcctDeptComp to IRMAClientRole
grant exec on dbo.LockOrderHeader to IRMAClientRole
grant exec on dbo.GetUser to IRMAClientRole
grant exec on dbo.GetUserID to IRMAClientRole
grant exec on dbo.GetOrderInvoice to IRMAClientRole
grant exec on dbo.GetANSOrderItems to IRMAClientRole
grant exec on dbo.UnlockCategory to IRMAClientRole
grant exec on dbo.ActiveItemList to IRMAClientRole
grant exec on dbo.LockOrigin to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_GetIdentifierAdds to IRMAClientRole
grant exec on dbo.GetUserEmail to IRMAClientRole
grant exec on dbo.GetOrderItemComments to IRMAClientRole
grant exec on dbo.GetApps to IRMAClientRole
grant exec on dbo.UnlockFSCustomer to IRMAClientRole
grant exec on dbo.AllocationReport to IRMAClientRole
grant exec on dbo.LockShelfLife to IRMAClientRole
grant exec on dbo.GetUserName to IRMAClientRole
grant exec on dbo.GetUserFullName to IRMAClientRole
grant exec on dbo.GetOrderItemInfo to IRMAClientRole
grant exec on dbo.GetAPUpAccruals to IRMAClientRole
grant exec on dbo.UnlockFSOrganization to IRMAClientRole
grant exec on dbo.ApplyPMPriceChange to IRMAClientRole
grant exec on dbo.LockUnit to IRMAClientRole
grant exec on dbo.GetUsers to IRMAClientRole
grant exec on dbo.GetAPUpAppDailySum to IRMAClientRole
grant exec on dbo.UnlockItem to IRMAClientRole
grant exec on dbo.AutoCloseReceiving to IRMAClientRole
grant exec on dbo.LockVendor to IRMAClientRole
grant exec on dbo.GetUsersLastOrderHeaderID to IRMAClientRole
grant exec on dbo.GetAPUpExceptions to IRMAClientRole
grant exec on dbo.UnlockOrderHeader to IRMAClientRole
grant exec on dbo.AutomaticOrderItemInfo to IRMAClientRole
grant exec on dbo.LYDeptComp to IRMAClientRole
grant exec on dbo.GetUsersSubteam to IRMAClientRole
grant exec on dbo.GetAPUpNoInvoice to IRMAClientRole
grant exec on dbo.UnlockOrigin to IRMAClientRole
grant exec on dbo.AutomaticOrderList to IRMAClientRole
grant exec on dbo.ManufacturingOrders to IRMAClientRole
grant exec on dbo.EPromotions_UpdatePromotionalOffer to IRMAClientRole
grant exec on dbo.GetUsersSubTeamList to IRMAClientRole
grant exec on dbo.GetOrderItemItemInfo to IRMAClientRole
grant exec on dbo.GetAPUpNoPSInfo to IRMAClientRole
grant exec on dbo.UnlockShelfLife to IRMAClientRole
grant exec on dbo.AutomaticOrderOriginUpdate to IRMAClientRole
grant exec on dbo.MarginBySubTeamReport to IRMAClientRole
grant exec on dbo.GrossMarginExceptionReport to IRMAClientRole
grant exec on dbo.GrossMarginExceptionReportVsMvmt to IRMAClientRole
grant exec on dbo.DailySales to IRMAClientRole
grant exec on dbo.GetUserStoreProductSubTeam to IRMAClientRole
grant exec on dbo.GetOrderItemList to IRMAClientRole
grant exec on dbo.GetAPUpNotApproved to IRMAClientRole
grant exec on dbo.UnlockUnit to IRMAClientRole
grant exec on dbo.AutoOrderInfoReport to IRMAClientRole
grant exec on dbo.MarginByVendorReport to IRMAClientRole
grant exec on dbo.GetRetailStoresAndTaxRates to IRMAClientRole
grant exec on dbo.UpdatePriceTaxChange to IRMAClientRole
grant exec on dbo.GetRetailStoresPOSPriceTax to IRMAClientRole
grant exec on dbo.GetUserStoreTeam_ByUserTitle to IRMAClientRole
grant exec on dbo.GetOrderItemQueueCount to IRMAClientRole
grant exec on dbo.GetAPUpUploaded to IRMAClientRole
grant exec on dbo.UnlockVendor to IRMAClientRole
grant exec on dbo.AutoSendDistOrders to IRMAClientRole
grant exec on dbo.MarkDownByCashier to IRMAClientRole
grant exec on dbo.GetVCAI_Exceptions to IRMAClientRole
grant exec on dbo.GetOrderItemQueueSearch to IRMAClientRole
grant exec on dbo.GetAvailItemVendorStores to IRMAClientRole
grant exec on dbo.UpdateAverageCost to IRMAClientRole
grant exec on dbo.AvgHourlySales to IRMAClientRole
grant exec on dbo.MovementCompSales to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateTransactionRecord to IRMAClientRole
grant exec on dbo.GetVCAI_ExceptionsCount to IRMAClientRole
grant exec on dbo.GetOrderItemQueueView to IRMAClientRole
grant exec on dbo.GetAvailPrimVend to IRMAClientRole
grant exec on dbo.UpdateBrandInfo to IRMAClientRole
grant exec on dbo.BuggyCount to IRMAClientRole
grant exec on dbo.MovementCompSalesNew to IRMAClientRole
grant exec on dbo.EPromotions_GetStoresByPricingMethod to IRMAClientRole
grant exec on dbo.GetVendCostData to IRMAClientRole
grant exec on dbo.GetOrderItemReceivedList to IRMAClientRole
grant exec on dbo.GetAvailPrimVendDetail to IRMAClientRole
grant exec on dbo.UpdateCategoryInfo to IRMAClientRole
grant exec on dbo.BuggySalesByItem to IRMAClientRole
grant exec on dbo.MovementCompSalesOrg to IRMAClientRole
grant exec on dbo.GetVendor1099AndID to IRMAClientRole
grant exec on dbo.GetOrderItems to IRMAClientRole
grant exec on dbo.GetAvgCost to IRMAClientRole
grant exec on dbo.UpdateContactInfo to IRMAClientRole
grant exec on dbo.BuggySalesGetItem to IRMAClientRole
grant exec on dbo.msqAlpharettaProduceInventory to IRMAClientRole
grant exec on dbo.GetTaxRateForStore to IRMAClientRole
grant exec on dbo.GetAvgCostDist to IRMAClientRole
grant exec on dbo.UpdateConversionInfo to IRMAClientRole
grant exec on dbo.BuggySalesSelectItem to IRMAClientRole
grant exec on dbo.GetVendorByPSVendorID to IRMAClientRole
grant exec on dbo.GetOrderListUnitID to IRMAClientRole
grant exec on dbo.GetAvgCostVendor to IRMAClientRole
grant exec on dbo.UpdateCustomer to IRMAClientRole
grant exec on dbo.BuyerVendor to IRMAClientRole
grant exec on dbo.Convertion_AddItemsFromTemp to IRMAClientRole
grant exec on dbo.GetVendorCost to IRMAClientRole
grant exec on dbo.GetBackOrderItems to IRMAClientRole
grant exec on dbo.UpdateCustReturnItem to IRMAClientRole
grant exec on dbo.CasesBySubTeam to IRMAClientRole
grant exec on dbo.GetVendorCostHistory to IRMAClientRole
grant exec on dbo.GetOrderOriginUpdates to IRMAClientRole
grant exec on dbo.GetBeginPeriodDate to IRMAClientRole
grant exec on dbo.UpdateCycleCountHistory to IRMAClientRole
grant exec on dbo.CasesBySubTeamAudit to IRMAClientRole
grant exec on dbo.GetVendorElectronic_Transfer to IRMAClientRole
grant exec on dbo.GetBeginPeriodDateRS to IRMAClientRole
grant exec on dbo.UpdateCycleCountMaster to IRMAClientRole
grant exec on dbo.CharcuterieInventoryItems to IRMAClientRole
grant exec on dbo.GetOrderSearch to IRMAClientRole
grant exec on dbo.GetBrandAndID to IRMAClientRole
grant exec on dbo.UpdateCycleCountMasterClosed to IRMAClientRole
grant exec on dbo.CheckCostChanges to IRMAClientRole
grant exec on dbo.GetVendorInfo to IRMAClientRole
grant exec on dbo.GetVendorItemStatuses to IRMAClientRole
grant exec on dbo.GetOrderStatus to IRMAClientRole
grant exec on dbo.EPromotions_UpdatePromotionalOfferMember to IRMAClientRole
grant exec on dbo.GetBrandInfo to IRMAClientRole
grant exec on dbo.UpdateFSCustomerInfo to IRMAClientRole
grant exec on dbo.CheckForDuplicateBrands to IRMAClientRole
grant exec on dbo.GetVendorInfoFirst to IRMAClientRole
grant exec on dbo.GetOrganizationInfoLast to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_AddIdentifier to IRMAClientRole
grant exec on dbo.GetBrandInfoFirst to IRMAClientRole
grant exec on dbo.UpdateInventoryLocation to IRMAClientRole
grant exec on dbo.CheckForDuplicateCardNumbers to IRMAClientRole
grant exec on dbo.GetVendorInfoLast to IRMAClientRole
grant exec on dbo.GetOriginAndID to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_DeleteIdentifier to IRMAClientRole
grant exec on dbo.GetBrandInfoLast to IRMAClientRole
grant exec on dbo.UpdateItemHistoryFromSales to IRMAClientRole
grant exec on dbo.CheckForDuplicateCategories to IRMAClientRole
grant exec on dbo.GetVendorItems to IRMAClientRole
grant exec on dbo.GetOriginInfo to IRMAClientRole
grant exec on dbo.GetBrandLockStatus to IRMAClientRole
grant exec on dbo.UpdateItemID to IRMAClientRole
grant exec on dbo.CheckForDuplicateContacts to IRMAClientRole
grant exec on dbo.GetVendorLinks to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_GetPriceBatchOffers to IRMAClientRole
grant exec on dbo.GetOriginInfoFirst to IRMAClientRole
grant exec on dbo.GetBrandName to IRMAClientRole
grant exec on dbo.UpdateItemIdentifier to IRMAClientRole
grant exec on dbo.UpdateItemRestore to IRMAClientRole
grant exec on dbo.CheckForDuplicateConversions to IRMAClientRole
grant exec on dbo.GetVendorLockStatus to IRMAClientRole
grant exec on dbo.GetOriginInfoLast to IRMAClientRole
grant exec on dbo.GetCategoriesBySubTeam to IRMAClientRole
grant exec on dbo.UpdateItemIdentifierDefault to IRMAClientRole
grant exec on dbo.CheckForDuplicateCustomers to IRMAClientRole
grant exec on dbo.msqGAMeatOrderGuide to IRMAClientRole
grant exec on dbo.GetVendorName to IRMAClientRole
grant exec on dbo.GetOriginLockStatus to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_GetPOSWriterFileConfig to IRMAClientRole
grant exec on dbo.GetCategoryAndID to IRMAClientRole
grant exec on dbo.UpdateItemInfo to IRMAClientRole
grant exec on dbo.CheckForDuplicateCycleCountMaster to IRMAClientRole
grant exec on dbo.GetVendors to IRMAClientRole, IRMAAdminRole
grant exec on dbo.GetPeriodDates to IRMAClientRole
grant exec on dbo.GetCategoryInfo to IRMAClientRole
grant exec on dbo.UpdateItemOrigin to IRMAClientRole
grant exec on dbo.CheckForDuplicateIdentifier to IRMAClientRole
grant exec on dbo.GetVendorStore to IRMAClientRole
grant exec on dbo.EPromotions_DeletePromotionalOfferMembers to IRMAClientRole
grant exec on dbo.GetPLUMCorpChg to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_GetStoreWriterConfigurations to IRMAClientRole
grant exec on dbo.GetCategoryInfoFirst to IRMAClientRole
grant exec on dbo.UpdateItemVendor to IRMAClientRole
grant exec on dbo.CheckForDuplicateInvLocation to IRMAClientRole
grant exec on dbo.GetVendStoreSubTeam to IRMAClientRole
grant exec on dbo.GetPLUMDeptMap to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_UpdatePriceBatchProcessedChg to IRMAClientRole
grant exec on dbo.GetCategoryInfoLast to IRMAClientRole
grant exec on dbo.UpdateLineDrive to IRMAClientRole
grant exec on dbo.CheckForDuplicateInvLocationItem to IRMAClientRole
grant exec on dbo.GetVendZones to IRMAClientRole
grant exec on dbo.EPromotions_DeletePromotionalOffer to IRMAClientRole
grant exec on dbo.GetPLUMInterface to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_UpdatePriceBatchProcessedDel to IRMAClientRole
grant exec on dbo.GetCategoryLockStatus to IRMAClientRole
grant exec on dbo.UpdateOrderApproved to IRMAClientRole
grant exec on dbo.CheckForDuplicateItemQuantity to IRMAClientRole
grant exec on dbo.msqGetBakeryVendorInventoryItems to IRMAClientRole
grant exec on dbo.GetWarehouse to IRMAClientRole
grant exec on dbo.EPromotions_GetAvailableItemGroups to IRMAClientRole
grant exec on dbo.GetPLUMStoreMap to IRMAClientRole
grant exec on dbo.GetCategoryName to IRMAClientRole
grant exec on dbo.UpdateOrderBackdate to IRMAClientRole
grant exec on dbo.CheckForDuplicateItemSign to IRMAClientRole
grant exec on dbo.msqGetCoffeeBarVendorInventoryItems to IRMAClientRole
grant exec on dbo.GetWarehouseCustomerOrders to IRMAClientRole
grant exec on dbo.EPromotions_GetPricingMethods to IRMAClientRole
grant exec on dbo.GetPMSalesHistory to IRMAClientRole
grant exec on dbo.TaxHosting_DeleteTaxFlag to IRMAClientRole
grant exec on dbo.TaxHosting_DeleteTaxClass to IRMAClientRole
grant exec on dbo.GetContactInfo to IRMAClientRole
grant exec on dbo.UpdateOrderCancelSend to IRMAClientRole
grant exec on dbo.CheckForDuplicateItemVendors to IRMAClientRole
grant exec on dbo.GetWarehouseItemChanges to IRMAClientRole
grant exec on dbo.GetPMSalesHistoryLoad to IRMAClientRole
grant exec on dbo.TaxHosting_DeleteTaxOverride to IRMAClientRole
grant exec on dbo.TaxHosting_DeleteTaxOverrideForItem to IRMAClientRole
grant exec on dbo.GetContactInfoFirst to IRMAClientRole
grant exec on dbo.UpdateOrderClosed to IRMAClientRole
grant exec on dbo.CheckForDuplicateManifest to IRMAClientRole
grant exec on dbo.msqGetIRMAItems to IRMAClientRole
grant exec on dbo.GetWarehousePurchaseOrders to IRMAClientRole
grant exec on dbo.EPromotions_GetItemGroups to IRMAClientRole
grant exec on dbo.GetPOCostDifAuto to IRMAClientRole
grant exec on dbo.TaxHosting_GetTaxClass to IRMAClientRole
grant exec on dbo.IRMA_Main_GetMenuAccess to IRMAClientRole
grant exec on dbo.Reporting_VendorItems to IRMAClientRole
grant exec on dbo.POExceptionReport to IRMAClientRole
grant exec on dbo.DCStoreRetailPriceReport to IRMAClientRole
grant exec on dbo.Reporting_ItemList to IRMAClientRole
grant exec on dbo.GetTaxClasses to IRMAClientRole
grant exec on dbo.GetLabelTypes to IRMAClientRole
grant exec on dbo.GetContactInfoLast to IRMAClientRole
grant exec on dbo.UpdateOrderHeaderDesc to IRMAClientRole
grant exec on dbo.CheckForDuplicateOrganizations to IRMAClientRole
grant exec on dbo.GetWarehouses to IRMAClientRole
grant exec on dbo.EPromotions_GetPriceBatchDetailCountByOfferID to IRMAClientRole
grant exec on dbo.GetPOHeader to IRMAClientRole
grant exec on dbo.GetPurchaseOrderHeader to IRMAClientRole
grant exec on dbo.TaxHosting_GetTaxFlag to IRMAClientRole
grant exec on dbo.GetConversionInfo to IRMAClientRole
grant exec on dbo.UpdateOrderInfo to IRMAClientRole
grant exec on dbo.CheckForDuplicateOrigins to IRMAClientRole
grant exec on dbo.GetWarehouseVendChanges to IRMAClientRole
grant exec on dbo.EPromotions_GetPromotionalOfferMembers to IRMAClientRole
grant exec on dbo.GetPOInfo to IRMAClientRole
grant exec on dbo.TaxHosting_GetTaxFlagActiveCount to IRMAClientRole
grant exec on dbo.GetConversionInfoFirst to IRMAClientRole
grant exec on dbo.UpdateOrderItemAlloc to IRMAClientRole
grant exec on dbo.CheckForDuplicateShelfLives to IRMAClientRole
grant exec on dbo.msqGetProduceAllStoresRetailPrices to IRMAClientRole
grant exec on dbo.GetZones to IRMAClientRole
grant exec on dbo.EPromotions_GetPromotionalOffersByPricingMethod to IRMAClientRole
grant exec on dbo.GetPosChangesAggregated to IRMAClientRole
grant exec on dbo.TaxHosting_GetTaxJurisdictions to IRMAClientRole
grant exec on dbo.GetCreditOrderList to IRMAClientRole
grant exec on dbo.UpdateOrderItemComments to IRMAClientRole
grant exec on dbo.msqGetPSVendorID to IRMAClientRole
grant exec on dbo.GetZoneSubTeams to IRMAClientRole
grant exec on dbo.EPromotions_GetPromotionalOffers to IRMAClientRole
grant exec on dbo.GetPosChangesGL_Pushed to IRMAClientRole
grant exec on dbo.TaxHosting_GetTaxOverride to IRMAClientRole
grant exec on dbo.GetCreditReasons to IRMAClientRole
grant exec on dbo.UpdateOrderItemInfo to IRMAClientRole
grant exec on dbo.CheckForDuplicateUnits to IRMAClientRole
grant exec on dbo.msqGetWFMGAMeatInventory to IRMAClientRole
grant exec on dbo.GetZoneSupply to IRMAClientRole
grant exec on dbo.EPromotions_InsertGroupData to IRMAClientRole
grant exec on dbo.GetPosChangesInQueue to IRMAClientRole
grant exec on dbo.TaxHosting_InsertTaxFlag to IRMAClientRole
grant exec on dbo.GetCurrentOnHand to IRMAClientRole
grant exec on dbo.UpdateOrderItemQueue to IRMAClientRole
grant exec on dbo.msqGetWFMNCMeatInventory to IRMAClientRole
grant exec on dbo.GLDistributionCheckDetailsReport to IRMAClientRole
grant exec on dbo.EPromotions_UpdateGroupData to IRMAClientRole
grant exec on dbo.GetPOSStores to IRMAClientRole
grant exec on dbo.TaxHosting_InsertTaxOverride to IRMAClientRole
grant exec on dbo.GetCurrentPrices to IRMAClientRole
grant exec on dbo.UpdateOrderItemReceivingInfo to IRMAClientRole
grant exec on dbo.CheckForDuplicateVendors to IRMAClientRole
grant exec on dbo.msqGetWFMSCMeatInventory to IRMAClientRole
grant exec on dbo.GLDistributionCheckFromSubReport to IRMAClientRole
grant exec on dbo.EPromotions_InsertPromotionalOffer to IRMAClientRole
grant exec on dbo.GetPOUsersSubteam to IRMAClientRole
grant exec on dbo.TaxHosting_UpdateTaxFlag to IRMAClientRole
grant exec on dbo.GetCurrentVendorStores to IRMAClientRole
grant exec on dbo.UpdateOrderItemUnitsReceived to IRMAClientRole
grant exec on dbo.CheckForExternalCycleCount to IRMAClientRole
grant exec on dbo.msqNCMeatOrderGuide to IRMAClientRole
grant exec on dbo.GLDistributionCheckReport to IRMAClientRole
grant exec on dbo.EPromotions_InsertPromotionalOfferMember to IRMAClientRole
grant exec on dbo.GetPriceBatchDetail to IRMAClientRole
grant exec on dbo.TaxHosting_UpdateTaxOverride to IRMAClientRole
grant exec on dbo.GetCustomer to IRMAClientRole
grant exec on dbo.UpdateOrderNotSent to IRMAClientRole
grant exec on dbo.CheckForOpenCycleCountMaster to IRMAClientRole
grant exec on dbo.msqProduceRetailPriceGuide to IRMAClientRole
grant exec on dbo.GLDistributionCheckSubReport to IRMAClientRole
grant exec on dbo.EPromotions_PromotionExistence to IRMAClientRole
grant exec on dbo.GetPriceBatchDetailDetailReport to IRMAClientRole
grant exec on dbo.GetCustomerInfoLast to IRMAClientRole
grant exec on dbo.UpdateOrderOpen to IRMAClientRole
grant exec on dbo.CheckForOpenCycleCounts to IRMAClientRole
grant exec on dbo.msqSeafoodInventory to IRMAClientRole
grant exec on dbo.GLDistributionsReport to IRMAClientRole
grant exec on dbo.EPromotions_RemoveItemGroup to IRMAClientRole
grant exec on dbo.GetPriceBatchDetailSumReport to IRMAClientRole
grant exec on dbo.GetCustomerReturn to IRMAClientRole
grant exec on dbo.UpdateOrderResetWarehouseSent to IRMAClientRole
grant exec on dbo.CheckIdentifierInItemVendor to IRMAClientRole
grant exec on dbo.NoSells to IRMAClientRole
grant exec on dbo.GLSalesReport to IRMAClientRole
grant exec on dbo.Administration_POSPush_DeletePOSWriterFileConfig to IRMAClientRole
grant exec on dbo.GetPriceBatchHeaderReport to IRMAClientRole
grant exec on dbo.GetPriceBatchHeaderDetailReport to IRMAClientRole
grant exec on dbo.GetCustomerReturnHistory to IRMAClientRole
grant exec on dbo.CheckIfPrimVendCanSwap to IRMAClientRole
grant exec on dbo.OpenOrdersDetailReport to IRMAClientRole
grant exec on dbo.OpenOrdersReportNOIDNORD to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GLTransfersReport to IRMAClientRole
grant exec on dbo.GetCustomerReturnItems to IRMAClientRole
grant exec on dbo.UpdateOrderSentDate to IRMAClientRole
grant exec on dbo.CheckIfWarehouseItem to IRMAClientRole
grant exec on dbo.OpenOrdersReport to IRMAClientRole
grant exec on dbo.HourlySales to IRMAClientRole
grant exec on dbo.GetPriceBatchPrintItems to IRMAClientRole
grant exec on dbo.GetCustomers to IRMAClientRole
grant exec on dbo.UpdateOrderSentToFaxDate to IRMAClientRole
grant exec on dbo.UpdateOrderSentToEmailDate to IRMAClientRole
grant exec on dbo.CheckItemInItemVendor to IRMAClientRole
grant exec on dbo.OrderItemQueueReport to IRMAClientRole
grant exec on dbo.IBMCatalogDump to IRMAClientRole
grant exec on dbo.GetCustomerSearch to IRMAClientRole
grant exec on dbo.UpdateOrderStatus to IRMAClientRole
grant exec on dbo.CheckOrderReceived to IRMAClientRole
grant exec on dbo.OrdersReceivedNotClosedReport to IRMAClientRole
grant exec on dbo.IBMIncrementRecords to IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreatePaymentRecord to IRMAClientRole
grant exec on dbo.GetPriceBatchSign to IRMAClientRole
grant exec on dbo.GetCustReturnReasons to IRMAClientRole
grant exec on dbo.UpdateOrderVendor to IRMAClientRole
grant exec on dbo.CheckPriceExist to IRMAClientRole
grant exec on dbo.OutOfPeriodInvoiceReport to IRMAClientRole
grant exec on dbo.IdentifyItem to IRMAClientRole
grant exec on dbo.GetPriceBatchStatusList to IRMAClientRole
grant exec on dbo.GetCustReturns to IRMAClientRole
grant exec on dbo.UpdateOrderWarehouseSend to IRMAClientRole
grant exec on dbo.CheckReceiveLog to IRMAClientRole
grant exec on dbo.OutOfStockReport to IRMAClientRole
grant exec on dbo.InsertAccountingIn to IRMAClientRole
grant exec on dbo.EPromotions_GetRewardTypes to IRMAClientRole
grant exec on dbo.GetPrimVendItmCnt to IRMAClientRole
grant exec on dbo.GetCycleCountHistoryItem to IRMAClientRole
grant exec on dbo.UpdateOrderWarehouseSent to IRMAClientRole
grant exec on dbo.CheckTransferDistributions to IRMAClientRole
grant exec on dbo.POSDeleteItem to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetPOSWriterEscapeChars to IRMAClientRole
grant exec on dbo.InsertContact to IRMAClientRole
grant exec on dbo.GetPSVendors to IRMAClientRole
grant exec on dbo.GetCycleCountHistoryList to IRMAClientRole
grant exec on dbo.UpdateOrganizationInfo to IRMAClientRole
grant exec on dbo.CheckVendorCostHistoryOverlap to IRMAClientRole
grant exec on dbo.POSGetFTPStores to IRMAClientRole
grant exec on dbo.InsertConversion to IRMAClientRole
grant exec on dbo.GetReceiveLogDate to IRMAClientRole
grant exec on dbo.GetCycleCountItemList to IRMAClientRole
grant exec on dbo.UpdateOriginInfo to IRMAClientRole
grant exec on dbo.POSGetStoreInfo to IRMAClientRole
grant exec on dbo.InsertCustomer to IRMAClientRole
grant exec on dbo.GetReceivingList to IRMAClientRole
grant exec on dbo.GetCycleCountList to IRMAClientRole
grant exec on dbo.CheeseInventoryItems to IRMAClientRole
grant exec on dbo.PosQueueGLClose to IRMAClientRole
grant exec on dbo.InsertCustomerReturn to IRMAClientRole
grant exec on dbo.GetReceivingLog to IRMAClientRole
grant exec on dbo.GetCycleCountMaster to IRMAClientRole
grant exec on dbo.UpdatePOSChangesAggregated to IRMAClientRole
grant exec on dbo.CloseCycleCountHeader to IRMAClientRole
grant exec on dbo.PosQueueGLPush to IRMAClientRole
grant exec on dbo.InsertCustomerReturnItem to IRMAClientRole
grant exec on dbo.GetReceivingStores to IRMAClientRole
grant exec on dbo.GetCycleCountMasterList to IRMAClientRole
grant exec on dbo.UpdatePOSChangesGL_Pushed to IRMAClientRole
grant exec on dbo.ClosedOrdersReport to IRMAClientRole
grant exec on dbo.PostStoreItemChange to IRMAClientRole
grant exec on dbo.InsertCycleCountHeader to IRMAClientRole
grant exec on dbo.GetRecvLogSumApprv to IRMAClientRole
grant exec on dbo.GetDaysInFiscalMonth to IRMAClientRole
grant exec on dbo.CloseReceiving to IRMAClientRole
grant exec on dbo.PricingMethodList to IRMAClientRole
grant exec on dbo.InsertCycleCountItem to IRMAClientRole
grant exec on dbo.GetRecvLogSumExcp to IRMAClientRole
grant exec on dbo.GetDistAndMfg to IRMAClientRole
grant exec on dbo.CostConversion to IRMAClientRole
grant exec on dbo.PricingPrintSignsSearch to IRMAClientRole
grant exec on dbo.InsertCycleCountItem2 to IRMAClientRole
grant exec on dbo.GetRegionCustomers to IRMAClientRole
grant exec on dbo.GetDistManZones to IRMAClientRole
grant exec on dbo.CostExceptionReport to IRMAClientRole
grant exec on dbo.ProductInventoryGuide to IRMAClientRole
grant exec on dbo.InsertCycleCountItemsAll to IRMAClientRole
grant exec on dbo.GetRegions to IRMAClientRole
grant exec on dbo.GetDistOrderWindowsClosed to IRMAClientRole
grant exec on dbo.CountOpenImportCounts to IRMAClientRole
grant exec on dbo.ProductNotAvaiable to IRMAClientRole
grant exec on dbo.InsertCycleCountItemsBrand to IRMAClientRole
grant exec on dbo.GetRegionStates to IRMAClientRole
grant exec on dbo.GetDistributionHeader to IRMAClientRole
grant exec on dbo.UpdatePriceBatchDetailCutHeader to IRMAClientRole
grant exec on dbo.CountOrderItems to IRMAClientRole
grant exec on dbo.CountReceivedOrderItems to IRMAClientRole
grant exec on dbo.fn_ReceivedItemsExist to IRMAClientRole 
grant exec on dbo.ProductOrderGuide to IRMAClientRole
grant exec on dbo.InsertCycleCountItemsCategory to IRMAClientRole
grant exec on dbo.GetRetailStoreCommInfo to IRMAClientRole
grant exec on dbo.GetDistributionMarkup to IRMAClientRole
grant exec on dbo.UpdatePriceBatchDetailHeader to IRMAClientRole
grant exec on dbo.UpdatePriceBatchHeader to IRMAClientRole
grant exec on dbo.CreditReasonReport to IRMAClientRole
grant exec on dbo.PromoSalesExport to IRMAClientRole
grant exec on dbo.InsertCycleCountItemsMostExpensive to IRMAClientRole
grant exec on dbo.GetRetailStores to IRMAClientRole
grant exec on dbo.GetDistSubTeams to IRMAClientRole
grant exec on dbo.UpdatePriceBatchDetailPromo to IRMAClientRole
grant exec on dbo.DailyReceiving to IRMAClientRole
grant exec on dbo.PSIItem to IRMAClientRole
grant exec on dbo.InsertCycleCountItemsVendor to IRMAClientRole
grant exec on dbo.GetEndPeriodDate to IRMAClientRole
grant exec on dbo.UpdatePriceBatchDetailReg to IRMAClientRole
grant exec on dbo.PSIMovement to IRMAClientRole
grant exec on dbo.InsertCycleCountItemsZeroCount to IRMAClientRole
grant exec on dbo.GetRetailSubTeam to IRMAClientRole
grant exec on dbo.GetExSeverity to IRMAClientRole
grant exec on dbo.UpdatePriceBatchPackage to IRMAClientRole
grant exec on dbo.DailySalesComp to IRMAClientRole
grant exec on dbo.PSIStore to IRMAClientRole
grant exec on dbo.InsertGLPushHistory to IRMAClientRole
grant exec on dbo.GetRetailZones to IRMAClientRole
grant exec on dbo.GetExTypes to IRMAClientRole
grant exec on dbo.UpdatePriceBatchStatus to IRMAClientRole
grant exec on dbo.DailyTax to IRMAClientRole
grant exec on dbo.PSISubTeam to IRMAClientRole
grant exec on dbo.InsertGLPushQueue to IRMAClientRole
grant exec on dbo.GetReturnOrderList to IRMAClientRole
grant exec on dbo.GetDistributionCreditOrderList to IRMAClientRole
grant exec on dbo.GetFaxOrderItemList to IRMAClientRole
grant exec on dbo.UpdatePriceBatchUnpackage to IRMAClientRole
grant exec on dbo.DailyTaxSales to IRMAClientRole
grant exec on dbo.PSIVendor to IRMAClientRole
grant exec on dbo.InsertImportCycleCount to IRMAClientRole
grant exec on dbo.GetRipeCustomerByRipeZoneLocation to IRMAClientRole
grant exec on dbo.GetFirstItem to IRMAClientRole
grant exec on dbo.UpdateSalesAggregates to IRMAClientRole
grant exec on dbo.DaySales to IRMAClientRole
grant exec on dbo.InsertInventoryLocationItem to IRMAClientRole
grant exec on dbo.GetRipeCustomerByRipeZoneLocationDistDate to IRMAClientRole
grant exec on dbo.GetFirstVendor to IRMAClientRole
grant exec on dbo.UpdateShelfLifeInfo to IRMAClientRole
grant exec on dbo.DeleteBrand to IRMAClientRole
grant exec on dbo.PurchasesSummaryReport to IRMAClientRole
grant exec on dbo.InsertItem to IRMAClientRole
grant exec on dbo.GetRipeCustomerStoreNo to IRMAClientRole
grant exec on dbo.GetFQSOrganizationInfo to IRMAClientRole
grant exec on dbo.DeleteCategory to IRMAClientRole
grant exec on dbo.PurchToSalesComp to IRMAClientRole
grant exec on dbo.InsertItemBrand to IRMAClientRole
grant exec on dbo.GetRipeLocations to IRMAClientRole
grant exec on dbo.GetFQSOrganizationInfoFirst to IRMAClientRole
grant exec on dbo.DeleteContact to IRMAClientRole
grant exec on dbo.QueuePMOrganizationChg to IRMAClientRole
grant exec on dbo.InsertItemCategory to IRMAClientRole
grant exec on dbo.GetRipeLocationStoreNo to IRMAClientRole
grant exec on dbo.GetFreightMarkUps to IRMAClientRole
grant exec on dbo.UpdateSignQueuePrinted to IRMAClientRole
grant exec on dbo.DeleteContacts to IRMAClientRole
grant exec on dbo.QueuePMProductChg to IRMAClientRole
grant exec on dbo.InsertItemHistory to IRMAClientRole
grant exec on dbo.InsertItemHistoryShrink to IRMAClientRole
grant exec on dbo.GetWasteTypes to IRMAClientRole
grant exec on dbo.GetRipeZones to IRMAClientRole
grant exec on dbo.GetFSCustomerInfoFirst to IRMAClientRole
grant exec on dbo.UpdateStoresSignListPrinted to IRMAClientRole
grant exec on dbo.DeleteConversion to IRMAClientRole
grant exec on dbo.InsertItemHistory2 to IRMAClientRole
grant exec on dbo.GetRuleDef to IRMAClientRole
grant exec on dbo.GetFSCustomerInfoLast to IRMAClientRole
grant exec on dbo.UpdateUnitInfo to IRMAClientRole
grant exec on dbo.DeleteCustomerReturn to IRMAClientRole
grant exec on dbo.InsertItemHistory3 to IRMAClientRole
grant exec on dbo.InsertItemHistory4 to IRMAClientRole
grant exec on dbo.GetShelfLifeAndID to IRMAClientRole
grant exec on dbo.GetFSCustomerInformation to IRMAClientRole
grant exec on dbo.UpdateUnreceivedOrders to IRMAClientRole
grant exec on dbo.DeleteCustReturnItem to IRMAClientRole
grant exec on dbo.ReceiveOrderItem3 to IRMAClientRole
grant exec on dbo.InsertItemHistoryCycleCount to IRMAClientRole
grant exec on dbo.GetShelfLifeInfo to IRMAClientRole
grant exec on dbo.GetFSCustomerLinks to IRMAClientRole
grant exec on dbo.UpdateVCAI_Exception to IRMAClientRole
grant exec on dbo.DeleteCycleCount to IRMAClientRole
grant exec on dbo.ReceiveOrderItem4 to IRMAClientRole
grant exec on dbo.InsertItemHistoryCycleCountCursor to IRMAClientRole
grant exec on dbo.GetShelfLifeInfoFirst to IRMAClientRole
grant exec on dbo.GetFSOrganizationInfo to IRMAClientRole
grant exec on dbo.UpdateVendorCostHistory to IRMAClientRole
grant exec on dbo.DeleteCycleCountHistoryItem to IRMAClientRole
grant exec on dbo.InsertItemIdentifier to IRMAClientRole
grant exec on dbo.GetShelfLifeInfoLast to IRMAClientRole
grant exec on dbo.GetFSOrganizationInfoFirst to IRMAClientRole
grant exec on dbo.UpdateVendorInfo to IRMAClientRole
grant exec on dbo.DeleteCycleCountItem to IRMAClientRole
grant exec on dbo.ReceivingCheckList to IRMAClientRole
grant exec on dbo.GetReceivingCheckList to IRMAClientRole
grant exec on dbo.InsertItemOrigin to IRMAClientRole
grant exec on dbo.GetShelfLifeLockStatus to IRMAClientRole
grant exec on dbo.GetFSOrganizationInformation to IRMAClientRole
grant exec on dbo.UpdateZoneSubTeam to IRMAClientRole
grant exec on dbo.DeleteCycleCountMaster to IRMAClientRole
grant exec on dbo.RegionSalesCompByDayReport to IRMAClientRole
grant exec on dbo.InsertItemShelfLife to IRMAClientRole
grant exec on dbo.GetFSOrganizationLinks to IRMAClientRole
grant exec on dbo.UpdateZoneSupply to IRMAClientRole
grant exec on dbo.DeletedOrderReport to IRMAClientRole
grant exec on dbo.RegionSalesCompByWeekReport to IRMAClientRole
grant exec on dbo.InsertItemSign to IRMAClientRole
grant exec on dbo.GetGLQueue to IRMAClientRole
grant exec on dbo.usp_ImportMultipleFiles to IRMAClientRole
grant exec on dbo.DeleteFSCustomer to IRMAClientRole
grant exec on dbo.RegionSalesCompTrendReport to IRMAClientRole
grant exec on dbo.InsertItemUnit to IRMAClientRole
grant exec on dbo.GetGLUploadDistributions to IRMAClientRole
grant exec on dbo.GetGLUploadInventoryAdjustment to IRMAClientRole
grant exec on dbo.ValidateLogin to IRMAClientRole
grant exec on dbo.DeleteFSCustomers to IRMAClientRole
grant exec on dbo.RemoveGLQueue to IRMAClientRole
grant exec on dbo.InsertItemUPCs to IRMAClientRole
grant exec on dbo.GetGLUploadTransfers to IRMAClientRole
grant exec on dbo.GetGLUploadTransfersByGroup to IRMAClientRole
grant exec on dbo.ValidateMonth to IRMAClientRole
grant exec on dbo.DeleteFSOrganization to IRMAClientRole
grant exec on dbo.RemoveItemOnHand to IRMAClientRole
grant exec on dbo.InsertItemVendor to IRMAClientRole
grant exec on dbo.GetSignName to IRMAClientRole
grant exec on dbo.GetHandPrinterLabels to IRMAClientRole
grant exec on dbo.ValidateTeams to IRMAClientRole
grant exec on dbo.DeleteInventoryLocationItems to IRMAClientRole
grant exec on dbo.RemoveItemUPCInventory to IRMAClientRole
grant exec on dbo.InsertODBCError to IRMAClientRole
grant exec on dbo.GetSignQueue to IRMAClientRole
grant exec on dbo.GetInternalCustomers to IRMAClientRole
grant exec on dbo.ValidateWeek to IRMAClientRole
grant exec on dbo.DeleteInventoryLocations to IRMAClientRole
grant exec on dbo.RepInventoryLocationItems to IRMAClientRole
grant exec on dbo.InsertOrder to IRMAClientRole
grant exec on dbo.GetSignType to IRMAClientRole
grant exec on dbo.GetInventoryLocation to IRMAClientRole
grant exec on dbo.VendorEfficiency to IRMAClientRole
grant exec on dbo.DeleteItemIdentifier to IRMAClientRole
grant exec on dbo.RepInventoryLocationItemsCount to IRMAClientRole
grant exec on dbo.InsertOrder2 to IRMAClientRole
grant exec on dbo.GetSpecificUnitAndID to IRMAClientRole
grant exec on dbo.GetInventoryLocationItems to IRMAClientRole
grant exec on dbo.VendorItemReport to IRMAClientRole
grant exec on dbo.DeleteItemInventory to IRMAClientRole
grant exec on dbo.RepInventoryLocations to IRMAClientRole
grant exec on dbo.InsertOrderInvoice to IRMAClientRole
grant exec on dbo.GetStore to IRMAClientRole
grant exec on dbo.GetInventoryLocations to IRMAClientRole
grant exec on dbo.VendorSearch to IRMAClientRole
grant exec on dbo.DeleteItemSign to IRMAClientRole
grant exec on dbo.RIPECheckExistingDistributions to IRMAClientRole
grant exec on dbo.GetStoreCustomer to IRMAClientRole
grant exec on dbo.GetInventoryLocationsByStore to IRMAClientRole
grant exec on dbo.VIMAuthorizationStatusFile to IRMAClientRole
grant exec on dbo.DeleteItemUPCInventory to IRMAClientRole
grant exec on dbo.RIPEDeleteIRSOrderHistory to IRMAClientRole
grant exec on dbo.InsertOrderItemCredit to IRMAClientRole
grant exec on dbo.EPromotions_DeleteItemFromGroup to IRMAClientRole
grant exec on dbo.GetStoreFromOrder to IRMAClientRole
grant exec on dbo.GetInventoryServiceExport to IRMAClientRole
grant exec on dbo.VIMItemRegionFile to IRMAClientRole
grant exec on dbo.DeleteItemVendor to IRMAClientRole
grant exec on dbo.RIPEGetDistributions to IRMAClientRole
grant exec on dbo.GetStoreIsDistribution to IRMAClientRole
grant exec on dbo.GetInventoryStores to IRMAClientRole
grant exec on dbo.VIMItemStatusFile to IRMAClientRole
grant exec on dbo.DeleteItemVideo to IRMAClientRole
grant exec on dbo.RIPEGetImportedOrders to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_GetPriceBatchSent to IRMAClientRole
grant exec on dbo.GetStoreItem to IRMAClientRole
grant exec on dbo.GetInvoiceNumberUse to IRMAClientRole
grant exec on dbo.VIMPriceTypeFile to IRMAClientRole
grant exec on dbo.DeleteOldVCAI_Exception to IRMAClientRole
grant exec on dbo.RIPEImportErrors to IRMAClientRole
grant exec on dbo.InsertOrderItemRtnID to IRMAClientRole
grant exec on dbo.EPromotions_ReturnPendingPriceBatchDetailCount to IRMAClientRole
grant exec on dbo.GetStoreItemCycleCountInfo to IRMAClientRole
grant exec on dbo.GetItem to IRMAClientRole
grant exec on dbo.VIMPriceZoneFile to IRMAClientRole
grant exec on dbo.DeleteOrderHeader to IRMAClientRole
grant exec on dbo.RIPEInsertImportData to IRMAClientRole
grant exec on dbo.InsertOrganization to IRMAClientRole
grant exec on dbo.GetStoreItemOrderInfo to IRMAClientRole
grant exec on dbo.GetItemConversion to IRMAClientRole
grant exec on dbo.VIMPSVendorRefFile to IRMAClientRole
grant exec on dbo.DeleteOrderHeaderOrderItems to IRMAClientRole
grant exec on dbo.rptLot_NoByIdentifier to IRMAClientRole
grant exec on dbo.InsertOrgPO to IRMAClientRole
grant exec on dbo.GetStoreItemSearch to IRMAClientRole
grant exec on dbo.GetItemConversionAll to IRMAClientRole
grant exec on dbo.VIMRegionalDepartmentFile to IRMAClientRole
grant exec on dbo.DeleteOrderInvoice to IRMAClientRole
grant exec on dbo.rptLot_NoByLot_No to IRMAClientRole
grant exec on dbo.InsertPOSChanges to IRMAClientRole
grant exec on dbo.GetStoreItemVendors to IRMAClientRole
grant exec on dbo.GetItemData to IRMAClientRole
grant exec on dbo.VIMRetailFuturePriceFile to IRMAClientRole
grant exec on dbo.DeleteOrderItem to IRMAClientRole
grant exec on dbo.SalesAggregation to IRMAClientRole
grant exec on dbo.InsertPOSItem to IRMAClientRole
grant exec on dbo.EPromotions_AddItemToGroup to IRMAClientRole
grant exec on dbo.GetStoreItemVendorStores to IRMAClientRole
grant exec on dbo.GetItemDataInventory to IRMAClientRole
grant exec on dbo.VIMRetailPriceFile to IRMAClientRole
grant exec on dbo.DeleteOrderItemQueue to IRMAClientRole
grant exec on dbo.SalesByItemByDayCrossTab to IRMAClientRole
grant exec on dbo.EPromotions_GetGroupItems to IRMAClientRole
grant exec on dbo.GetStoreMobilePrinter to IRMAClientRole
grant exec on dbo.GetItemHistory to IRMAClientRole
grant exec on dbo.VIMStoreFile to IRMAClientRole
grant exec on dbo.DeleteOrderReceiving to IRMAClientRole
grant exec on dbo.SalesPercentage to IRMAClientRole
grant exec on dbo.GetStoreName to IRMAClientRole
grant exec on dbo.GetItemIdentifiers to IRMAClientRole
grant exec on dbo.VIMVendorCostFile to IRMAClientRole
grant exec on dbo.DeleteOrigin to IRMAClientRole
grant exec on dbo.SalesPercentageCrossTab to IRMAClientRole
grant exec on dbo.InsertReceivingItemHistory to IRMAClientRole
grant exec on dbo.GetStores to IRMAClientRole
grant exec on dbo.GetStoreOnHand to IRMAClientRole
grant exec on dbo.GetStoreOnHandDetail to IRMAClientRole
grant exec on dbo.GetItemIDInfo to IRMAClientRole
grant exec on dbo.VIMVendorStoreItemFile to IRMAClientRole
grant exec on dbo.DeletePLUMCorpChgQueueTmp to IRMAClientRole
grant exec on dbo.SalesSummary to IRMAClientRole
grant exec on dbo.InsertReturnOrderHeader to IRMAClientRole
grant exec on dbo.GetStoresAndDist to IRMAClientRole
grant exec on dbo.GetItemInfo to IRMAClientRole
grant exec on dbo.GetItemChangeInfo to IRMAClientRole
grant exec on dbo.WarehouseMovement to IRMAClientRole
grant exec on dbo.StoreOrders to IRMAClientRole
grant exec on dbo.DeletePriceBatch to IRMAClientRole
grant exec on dbo.ScanReport to IRMAClientRole
grant exec on dbo.InsertSalesExportQueue to IRMAClientRole
grant exec on dbo.GetStoresAndDistAdjustments to IRMAClientRole
grant exec on dbo.GetItemInfoByIdentifier to IRMAClientRole
grant exec on dbo.Reporting_AdjustmentSummary to IRMAClientRole
grant exec on dbo.DeletePriceBatchCutDetail to IRMAClientRole
grant exec on dbo.InsertSASIItem to IRMAClientRole
grant exec on dbo.GetStoreSubTeam to IRMAClientRole
grant exec on dbo.GetItemLockStatus to IRMAClientRole
grant exec on dbo.WasteToSales to IRMAClientRole
grant exec on dbo.DeletePriceBatchDetail to IRMAClientRole
grant exec on dbo.SetFQSCusModifiedCreatedTime to IRMAClientRole
grant exec on dbo.GetStoreSubTeamMinusSupplier to IRMAClientRole
grant exec on dbo.GetItemMultipleUPCs to IRMAClientRole
grant exec on dbo.xl_OffSale to IRMAClientRole
grant exec on dbo.DeleteReceiving to IRMAClientRole
grant exec on dbo.SetFQSOrgModifiedCreatedTime to IRMAClientRole
grant exec on dbo.InsertStoreItemVendor to IRMAClientRole
grant exec on dbo.GetStoreUserSubTeam to IRMAClientRole
grant exec on dbo.GetItemName to IRMAClientRole
grant exec on dbo.xl_OffSaleStores to IRMAClientRole
grant exec on dbo.DeleteShelfLife to IRMAClientRole
grant exec on dbo.SetOrderSentDate to IRMAClientRole
grant exec on dbo.InsertVendor to IRMAClientRole
grant exec on dbo.GetStoreVendor to IRMAClientRole
grant exec on dbo.GetItemOrder to IRMAClientRole
grant exec on dbo.xl_OffSaleSubteams to IRMAClientRole
grant exec on dbo.SetPrimaryVendor to IRMAClientRole
grant exec on dbo.InsertVendorCostHistory to IRMAClientRole
grant exec on dbo.EPromotions_ValidateGroupName to IRMAClientRole
grant exec on dbo.GetSubTeam to IRMAClientRole
grant exec on dbo.GetItemPendPrice to IRMAClientRole
grant exec on dbo.YearlyBuggyCount1 to IRMAClientRole
grant exec on dbo.DeleteStoreItemVendor to IRMAClientRole
grant exec on dbo.EPromotions_CreateNewItemGroup to IRMAClientRole
grant exec on dbo.GetSubTeamBrand to IRMAClientRole
grant exec on dbo.GetItemSearch to IRMAClientRole
grant exec on dbo.YearlyBuggyCount2 to IRMAClientRole
grant exec on dbo.DeleteUnit to IRMAClientRole
grant exec on dbo.InsertVendorCostHistory3 to IRMAClientRole
grant exec on dbo.GetSubTeamByProductType to IRMAClientRole
grant exec on dbo.ZeroCostPriceReport to IRMAClientRole
grant exec on dbo.DeleteVendor to IRMAClientRole
grant exec on dbo.SpecialsByEndDate to IRMAClientRole
grant exec on dbo.InsertVendorCostHistoryException to IRMAClientRole
grant exec on dbo.GetSubTeamCategory to IRMAClientRole
grant exec on dbo.GetItemsInfo to IRMAClientRole
grant exec on dbo.Administration_POSPush_DeletePOSWriter to IRMAClientRole
grant exec on dbo.DeleteVendorCostHistory to IRMAClientRole
grant exec on dbo.StoreOpsOrdersExport to IRMAClientRole
grant exec on dbo.InsertVendorDealHistory to IRMAClientRole
grant exec on dbo.EPromotions_AddStoreToPromotion to IRMAClientRole
grant exec on dbo.GetSubTeamMargin to IRMAClientRole
grant exec on dbo.GetItemStoreVendorsCost to IRMAClientRole
grant exec on dbo.DeleteVendorItems to IRMAClientRole
grant exec on dbo.StoreOpsSalesExport to IRMAClientRole
grant exec on dbo.InsertZoneSubTeam to IRMAClientRole
grant exec on dbo.GetSubTeamName to IRMAClientRole
grant exec on dbo.GetPriceBatchSearch to IRMAClientRole
grant exec on dbo.GetItemUnitID to IRMAClientRole
grant exec on dbo.Administration_POSPush_DeleteStorePOSConfig to IRMAClientRole
grant exec on dbo.DeleteWarehouseItemChange to IRMAClientRole
grant exec on dbo.StoreOpsVendorExport to IRMAClientRole
grant exec on dbo.InventoryBalance to IRMAClientRole
grant exec on dbo.Administration_POSPush_DeletePOSWriterFileConfigRow to IRMAClientRole
grant exec on dbo.GetPriceTypes to IRMAClientRole
grant exec on dbo.GetSubTeams to IRMAClientRole
grant exec on dbo.GetSubTeamBySubTeamNo to IRSUser, IRMAClientRole
grant exec on dbo.GetItemUnitInfo to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetPOSChangeTypes to IRMAClientRole
grant exec on dbo.DeleteWarehouseVendorChange to IRMAClientRole
grant exec on dbo.SubTeamSales to IRMAClientRole
grant exec on dbo.InvoiceManifestReport to IRMAClientRole
grant exec on dbo.GetSubTeamsByTeam to IRMAClientRole
grant exec on dbo.InsertPriceBatchHeader to IRMAClientRole
grant exec on dbo.GetItemUnits to IRMAClientRole
grant exec on dbo.Administration_POSPush_GetPOSDataElement to IRMAClientRole
grant exec on dbo.DeleteZoneSubTeam to IRMAClientRole
grant exec on dbo.SwitchPrimaryVendor to IRMAClientRole
grant exec on dbo.EPromotions_CreatePriceBatchDetail to IRMAClientRole
grant exec on dbo.GetSubTeamTotSalesCost to IRMAClientRole
grant exec on dbo.GetInstanceData to IRMAClientRole
grant exec on dbo.GetInstanceDataFlags to IRMAClientRole
grant exec on dbo.TaxHosting_GetTaxJurisdictionsForTaxClass to IRMAClientRole
grant exec on dbo.TaxHosting_GetTaxOverrideForItem to IRMAClientRole
grant exec on dbo.TaxHosting_GetAvailableTaxFlagsForItem to IRMAClientRole
grant exec on dbo.UpdateItemScaleData to IRMAClientRole
grant exec on dbo.UpdateItemInfoForBulkLoad to IRMAClientRole
grant exec on dbo.GetPriceBatchDetailIDs to IRMAClientRole
grant exec on dbo.GetItemUploadTypes to IRMAClientRole
grant exec on dbo.InsertItemUploadHeader to IRMAClientRole
grant exec on dbo.InsertItemUploadDetail to IRMAClientRole
grant exec on dbo.GetItemUploadSearch to IRMAClientRole
grant exec on dbo.GetItemAdminUsers to IRMAClientRole
grant exec on dbo.UpdateItemUploadHeader to IRMAClientRole
grant exec on dbo.GetItemUploadDetails to IRMAClientRole
grant exec on dbo.UpdateItemUploadDetail to IRMAClientRole
grant exec on dbo.DeleteItemUpload to IRMAClientRole
grant exec on dbo.CheckIfVendorIsPrimaryForAnyItems to IRMAClientRole
grant exec on dbo.TaxHosting_IsExistingTaxFlagForJurisdiction to IRMAClientRole
grant exec on dbo.TaxHosting_ConfirmDeleteTaxFlag to IRMAClientRole
grant exec on dbo.VendorMovement to IRMAClientRole
grant exec on dbo.MarginBySubTeamReport_UseLastCost to IRMAClientRole
grant exec on dbo.MarginByVendorReport_UseLastCost to IRMAClientRole
grant exec on dbo.GetVendor_ByCompanyName to IRMAClientRole
grant exec on dbo.GetVendor_ByPSVendorID to IRMAClientRole
grant exec on dbo.GetVendor_ByVendorID to IRMAClientRole
grant exec on dbo.GetAllStores_ByStoreName to IRMAClientRole
grant exec on dbo.TopMovers_UseLastCost to IRMAClientRole
grant exec on dbo.TopMoversSummary_UseLastCost to IRMAClientRole
grant exec on dbo.ZeroMovementReport to IRMAClientRole
grant exec on dbo.Reporting_ItemList_UseLastCost to IRMAClientRole
grant exec on dbo.Reporting_SpecialsInProgress to IRMAClientRole
grant exec on dbo.GetInstanceDataAvailableStoreOverrides to IRMAClientRole
grant exec on dbo.GetInstanceDataFlagsStoreOverrideList to IRMAClientRole
grant exec on dbo.GetInstanceDataFlagValue to IRMAClientRole
grant exec on dbo.GetStoresWithNoVendorForItem to IRMAClientRole
grant exec on dbo.GetPriceBatchHeader_LabelSummary to IRMAClientRole
grant exec on dbo.GetItem_LabelSummary to IRMAClientRole
grant exec on dbo.Reporting_Movement_Full to IRMAClientRole
grant exec on dbo.Reporting_Movement_Full_UK to IRMAClientRole
grant exec on dbo.Reporting_SpecialsByDateRange to IRMAClientRole
grant exec on dbo.GetVendorDealTypes to IRMAClientRole
grant exec on dbo.GetCostPromoCodeTypes to IRMAClientRole
grant exec on dbo.GetVendorDealHistory to IRMAClientRole
grant exec on dbo.UpdateVendorDealHistory to IRMAClientRole
grant exec on dbo.DeleteVendorDealHistory to IRMAClientRole
grant exec on dbo.fn_IsScaleIdentifier to IRMAClientRole
grant exec on dbo.GetScalePLUConflicts to IRMAClientRole
grant exec on dbo.Teams_LoadTeams to IRMAClientRole
grant exec on dbo.Teams_GetTeam to IRMAClientRole
grant exec on dbo.Teams_CreateNew to IRMAClientRole
grant exec on dbo.Teams_SaveChanges to IRMAClientRole
grant exec on dbo.Teams_Validate_TeamAbbr to IRMAClientRole
grant exec on dbo.Teams_Validate_Teamname to IRMAClientRole
grant exec on dbo.GetDefaultPOSBatchId to IRMAClientRole
grant exec on dbo.GetDefaultPOSBatchIdRangeByStore to IRMAClientRole
grant exec on dbo.SubTeams_LoadSubTeams to IRMAClientRole
grant exec on dbo.SubTeams_GetSubTeam to IRMAClientRole
grant exec on dbo.SubTeams_SaveSubteam to IRMAClientRole
grant exec on dbo.SubTeams_CreateSubTeam to IRMAClientRole
grant exec on dbo.SubTeams_GetSubTeamsByStore to IRMAClientRole
grant exec on dbo.SubTeams_GetTeamSubTeamRelationshipsByStore to IRMAClientRole
grant exec on dbo.SubTeams_ValidateSubTeamToTeamRelationships to IRMAClientRole
grant exec on dbo.SubTeams_CreateSubTeamToTeamRelationship to IRMAClientRole
grant exec on dbo.SubTeams_UpdateSubTeamToTeamRelationship to IRMAClientRole
grant exec on dbo.GetProposedNetCostValues to IRMAClientRole
grant exec on dbo.GetScalePLUConflicts to IRMAClientRole
grant exec on dbo.InsertCostPromoCodeType to IRMAClientRole
grant exec on dbo.GetProposedNetCost_CostChange to IRMAClientRole
grant exec on dbo.fn_IsCaseItemUnit to IRMAClientRole
grant exec on dbo.StoreItemAttribute_InsertUpdateAttribute to IRMAClientRole
grant exec on dbo.GetCurrentProcessedSaleBatches to IRMAClientRole
grant exec on dbo.Scale_GetEatBy to IRMAClientRole
grant exec on dbo.Scale_GetGrades to IRMAClientRole
grant exec on dbo.Scale_GetLabelFormats to IRMAClientRole
grant exec on dbo.Scale_GetLabelStyles to IRMAClientRole, IRMAAdminRole, IRMASupportRole
grant exec on dbo.Scale_GetLabelTypes to IRMAClientRole
grant exec on dbo.Scale_GetNutriFacts to IRMAClientRole
grant exec on dbo.Scale_GetExtraTexts to IRMAClientRole
grant exec on dbo.Scale_GetRandomWeightTypes to IRMAClientRole
grant exec on dbo.Scale_GetTares to IRMAClientRole
grant exec on dbo.Scale_GetScaleUOMs to IRMAClientRole
grant exec on dbo.Scale_GetScaleDetailCombos to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateItemScaleDetails to IRMAClientRole
grant exec on dbo.Scale_GetItemScaleDetails to IRMAClientRole
grant exec on dbo.Scale_CheckForDuplicateEatBy to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateEatBy to IRMAClientRole
grant exec on dbo.Scale_DeleteEatBy to IRMAClientRole
grant exec on dbo.Scale_CheckForDuplicateGrade to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateGrade to IRMAClientRole
grant exec on dbo.Scale_DeleteGrade to IRMAClientRole
grant exec on dbo.Scale_CheckForDuplicateLabelFormat to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateLabelFormat to IRMAClientRole
grant exec on dbo.Scale_DeleteLabelFormat to IRMAClientRole
grant exec on dbo.Scale_CheckForDuplicateLabelStyle to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateLabelStyle to IRMAClientRole
grant exec on dbo.Scale_DeleteLabelStyle to IRMAClientRole
grant exec on dbo.Scale_CheckForDuplicateLabelType to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateLabelType to IRMAClientRole
grant exec on dbo.Scale_DeleteLabelType to IRMAClientRole
grant exec on dbo.Scale_CheckForDuplicateRandomWeightType to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateRandomWeightType to IRMAClientRole
grant exec on dbo.Scale_DeleteRandomWeightType to IRMAClientRole
grant exec on dbo.Scale_CheckForDuplicateTare to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateTare to IRMAClientRole
grant exec on dbo.Scale_DeleteTare to IRMAClientRole
grant exec on dbo.Scale_CheckForDuplicateExtraText to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateExtraText to IRMAClientRole
grant exec on dbo.Scale_GetExtraTextCombo to IRMAClientRole
grant exec on dbo.Scale_CheckForDuplicateNutriFact to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateNutriFact to IRMAClientRole
grant exec on dbo.Scale_GetNutriFact to IRMAClientRole
grant exec on dbo.InsertOrUpdateItemNutrifact to IRMAClientRole
grant exec on dbo.DeleteItemNutrifact to IRMAClientRole
grant exec on dbo.GetVendorDealHistoryStackableConflicts to IRMAClientRole
grant exec on dbo.Scale_GetNutriFactByItem to IRMAClientRole
grant exec on dbo.Scale_GetExtraTextByItem to IRMAClientRole
grant exec on dbo.GetRegPriceExists to IRMAClientRole
grant exec on dbo.EIM_PriceChangeValidation to IRMAClientRole
grant exec on dbo.Planogram_GetSetNumbers to IRMAClientRole
grant exec on dbo.Planogram_InsertPlanogramItem to IRMAClientRole
grant exec on dbo.Planogram_GetRegPlanogramItems to IRMAClientRole
grant exec on dbo.Planogram_GetNonRegPlanogramItems to IRMAClientRole
grant exec on dbo.Planogram_GetPrintLabSetRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetPrintLabNonRegTagFile to IRMAClientRole
grant exec on dbo.Replenishment_POSPull_GetIdentifier to IRMAClientRole
grant exec on dbo.DepartmentSalesAnalysis to IRMAClientRole
grant exec on dbo.BERTReport to IRMAClientRole
grant exec on dbo.fn_ValidateDescriptionCharacters to IRMAClientRole
grant exec on dbo.ItemMasterReport to IRMAClientRole
grant exec on dbo.GetIsBatchedByStatus to IRMAClientRole
grant exec on dbo.CancelAllSales to IRMAClientRole
grant exec on dbo.Reporting_CostAudit to IRMAClientRole
grant exec on dbo.Reporting_Movement_BySubteam to IRMAClientRole
grant exec on dbo.EndSaleEarly to IRMAClientRole, IRMASLIMRole
grant exec on dbo.UpdateStoreItem to IRMAClientRole
grant exec on dbo.GetValidationCodeDetails to IRMAClientRole
grant exec on dbo.GetStoresWithPrimaryVendorThatCanSwap to IRMAClientRole
grant exec on dbo.GetPricingMethodMappings to IRMAClientRole
grant exec on dbo.InsertPricingMethodMapping to IRMAClientRole
grant exec on dbo.DeletePricingMethodMapping to IRMAClientRole
grant exec on dbo.GetStoreJurisdictions to IRMAClientRole
grant exec on dbo.GetIsBatchedByStatusForStore to IRMAClientRole
grant exec on dbo.GetItemOverride to IRMAClientRole
grant exec on dbo.InsertUpdateItemOverride to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateItemScaleOverride to IRMAClientRole
grant exec on dbo.Scale_GetItemScaleOverride to IRMAClientRole
grant exec on dbo.GetIsItemOnSaleOrSalePendingForStore to IRMAClientRole
grant exec on dbo.OrderInvoice_3PartyFreightInvoice_Get to IRMAClientRole
grant exec on dbo.OrderInvoice_3PartyFreightInvoice_Update to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetFileMakerReprintTagFile to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetFXReprintTagFile to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetReprintTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetFileMakerNonRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetFXNonRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetFXSetRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetFileMakerSetRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetSetRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetAccessViaExtendedSetRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetAccessViaSetRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetNonRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetAccessViaNonRegTagFile to IRMAClientRole
grant exec on dbo.Planogram_GetAccessViaExtendedNonRegTagFile to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetFXBatchTagFile to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetFileMakerBatchTagFile to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetBatchTagFile to IRMAClientRole
grant exec on dbo.GetBatchesInSentState to IRMAClientRole
grant exec on dbo.UpdateBatchesInSentState to IRMAClientRole
grant exec on dbo.GetAllOnHandDetail to IRMAClientRole
grant exec on dbo.GetRegionCustomersAsDC to IRMAClientRole
grant exec on dbo.RollbackElectronicOrder to IRMAClientRole
grant exec on dbo.GetElectronicOrderHeaderInfo to IRMAClientRole
grant exec on dbo.fn_GetSubTeamUserName to IRMAClientRole
grant exec on dbo.fn_CountAllocationItems to IRMAClientRole
grant exec on dbo.GetSustainabilityRankings to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole, IRMASupportRole
grant exec on dbo.PurchasesToSalesCompSubTeamDetail TO IRMAReportsRole, IRMAClientRole
grant exec on dbo.PurchasesToSalesCompSubTeamSummary TO IRMAReportsRole, IRMAClientRole
grant exec on dbo.Moveotherdatatohist TO  IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.MoveAppLogDatatoArchive TO  IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.DeleteAppLogDataArchive TO  IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.PurgeLog TO  IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.GetDeletedOrderItemList TO  IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.GetAgeCode TO IRMAClientRole, IRSUser
grant exec on dbo.GetAppConfigKeys TO IRSUser, IRMAClientRole, IconInterface

-- for refusal functionality
grant exec on dbo.fn_IsRefusalAllowed to IRMAClientRole, IRMAReportsRole
grant exec on dbo.fn_IsValidRefusedItemList to IRMAClientRole, IRMAReportsRole
grant exec on dbo.InsertOrderItemRefused to IRMAClientRole
grant exec on dbo.UpdateOrderItemRefused to IRMAClientRole
grant exec on dbo.DeleteOrderItemRefused to IRMAClientRole
grant exec on dbo.GetTotalRefused to IRMAClientRole, IRMAReportsRole
grant exec on dbo.GetOrderItemsRefused to IRMAClientRole, IRMAReportsRole
grant exec on dbo.UpdateRefusedItemsList to IRMAClientRole, IRMAReportsRole
grant exec on dbo.GetFiscalCalendarInfo to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on fn_GetRefusedQuantityByIdentifier to IRMAClientRole, IRMAAdminRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on UpdateRefusedQuantityByIdentifier to IRMAClientRole, IRMAAdminRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.AddOrderItemRefusedForEInvoiceExceptionItems to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole

-- for default attributes and four level hierarchy
grant exec on dbo.GetAllProdHierarchyLevel3 to IRMAClientRole
grant exec on dbo.GetAllProdHierarchyLevel4 to IRMAClientRole
grant exec on dbo.GetProdHierarchyLevel3sByCategory to IRMAClientRole
grant exec on dbo.GetProdHierarchyLevel4sByLevel3 to IRMAClientRole
grant exec on dbo.GetItemDefaultAttributes to IRMAClientRole
grant exec on dbo.GetItemDefaultValues to IRMAClientRole
grant exec on dbo.GetItemDefaultValuesByItem to IRMAClientRole
grant exec on dbo.SaveItemDefaultValue to IRMAClientRole
grant exec on dbo.DeleteItemDefaultValue to IRMAClientRole
grant exec on dbo.InsertProdHierarchyLevel3 to IRMAClientRole
grant exec on dbo.InsertProdHierarchyLevel4 to IRMAClientRole
grant exec on dbo.CheckForDuplicateProdHierarchyLevel3 to IRMAClientRole
grant exec on dbo.CheckForDuplicateProdHierarchyLevel4 to IRMAClientRole
grant exec on dbo.fn_IsScaleIdentifier	to IRMAClientRole
grant exec on dbo.StoreItemAttribute_GetAttribute to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetStoreWriterConfigurations to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_UpdatePBDWithTagID to IRMAClientRole
grant exec on dbo.GetRegularPriceChgTypeData to IRMAClientRole
grant exec on dbo.ImportBOHFile to IRMAClientRole
grant exec on dbo.InventoryValueReport to IRMAClientRole

grant exec on dbo.fn_QuarterBeginDate to IRMAClientRole
grant exec on dbo.GetBeginQuarterDate to IRMAClientRole
grant exec on dbo.fn_FiscalYearBeginDate to IRMAClientRole
grant exec on dbo.GetBeginFiscalYearDate to IRMAClientRole
grant exec on dbo.Pricebook_Report to IRMAClientRole
grant exec on dbo.InStoreSpecials_Report to IRMAClientRole
grant exec on GetBeginQuarterDateRS to IRMAClientRole
grant exec on GetBeginFiscalYearDateRS to IRMAClientRole
grant exec on dbo.fn_GetDiscountAllowanceDateRange to IRMAClientRole
grant exec on dbo.fn_GetCurrentSumAllowances to IRMAClientRole
grant exec on dbo.fn_GetCurrentSumDiscounts to IRMAClientRole

grant exec on dbo.GetWIMPExtract_COSTDATA to IRMAClientRole
grant exec on dbo.GetWIMPExtract_ITEMDATA to IRMAClientRole
grant exec on dbo.GetWIMPExtract_PLANOSTATUS to IRMAClientRole

grant exec on dbo.GetItemTypes to IRMAClientRole
grant exec on dbo.GetOrderItemsCostData to IRMAClientRole
grant exec on dbo.UpdateOrderItemCostData to IRMAClientRole
GRANT EXEC ON dbo.DeleteReturnOrderUserRecords TO IRMAClientRole
GRANT EXEC ON dbo.CheckForReturnOrderChanges TO IRMAClientRole
GRANT EXEC ON dbo.GetReturnOrderChanges TO IRMAClientRole
GRANT EXEC ON dbo.GetReturnOrderRecords TO IRMAClientRole
GRANT EXEC ON dbo.UpdateReturnOrderRecord TO IRMAClientRole

grant exec on dbo.OrderInvoice_CloseControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_GetControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_GetControlGroupInvoices to IRMAClientRole
grant exec on dbo.OrderInvoice_GetControlGroupSearch to IRMAClientRole
grant exec on dbo.OrderInvoice_InsertControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_InsertControlGroupInvoice to IRMAClientRole
grant exec on dbo.OrderInvoice_UpdateControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_UpdateControlGroupInvoice to IRMAClientRole
grant exec on dbo.OrderInvoice_ValidateControlGroupInvoice to IRMAClientRole
grant exec on dbo.OrderInvoice_DeleteControlGroupInvoice to IRMAClientRole
grant exec on dbo.GetThirdPartyInvoiceNumberUse to IRMAClientRole
grant exec on dbo.InsertThirdPartyFreightInvoice to IRMAClientRole
grant exec on dbo.CheckVendorIdAndVendorKey to IRMAClientRole
grant exec on dbo.GetVendorCountByVendorKey to IRMAClientRole
grant exec on dbo.OrderInvoice_CheckInvoiceExistsInOpenControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_CheckInvoiceExistsInCurrentControlGroup to IRMAClientRole
grant exec on dbo.UpdateLineItemApproved to IRMAClientRole
grant exec on dbo.InventoryValueDetail to IRMAClientRole
grant exec on dbo.InventoryValueSummary  to IRMAClientRole
grant select on dbo.fn_InventoryValue to IRMAClientRole
grant exec on dbo.fn_GetRetailUnitAbbreviation to IRMAClientRole
grant exec on dbo.fn_IsDistributionCenter to IRMAClientRole
grant exec on dbo.CopyExistingPO to IRMAClientRole
grant exec on dbo.ValidateCopyPOItems to IRMAClientRole
grant exec on dbo.AddRoleConflict to IRMAClientRole
grant exec on dbo.DeleteRoleConflict to IRMAClientRole
grant exec on dbo.GetRoleConflicts to IRMAClientRole
grant exec on dbo.GetRoleConflictReason to IRMAClientRole
grant exec on dbo.InsertRoleConflictReason to IRMAClientRole
grant exec on dbo.UpdateStoreItemVendorDiscontinue to IRMAClientRole

grant exec on dbo.fn_GetGLSuppliesAccountNumber TO IRMAReportsRole, IRMAClientRole
grant exec on dbo.fn_GetGLPackagingAccountNumber TO IRMAReportsRole, IRMAClientRole
grant exec on dbo.fn_GetDiscontinueStatus to IRMAReportsRole, IRMAClientRole

grant exec on dbo.GetAllItemOverrides to IRMAClientRole
-- Begin EIM

-- UDF Grants
grant exec on dbo.fn_EIM_GetListOfUploadTypes to IRMAClientRole

-- Stored Proc Grants
grant exec on dbo.EIM_Regen_GetAllUploadAttributes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadAttributeByPK to IRMAClientRole

grant exec on dbo.EIM_ValidateCostedByWeightUnit to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadAttribute to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllUploadRows to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadRowByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadRowsByItemKey to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadRowsByUploadSessionID to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadRow to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadRow to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadRow to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllUploadSessions to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionByPK to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadSession to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadSession to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadSession to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllUploadSessionUploadTypes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypeByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypesByUploadSessionID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypesByUploadTypeCODE to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadSessionUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadSessionUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadSessionUploadType to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllUploadSessionUploadTypeStores to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypeStoreByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypeStoresByStoreNo to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadSessionUploadTypeStore to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadSessionUploadTypeStore to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadSessionUploadTypeStore to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllUploadTypes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeByPK to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadType to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllUploadTypeAttributes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeAttributeByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeAttributesByUploadTypeCode to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeAttributesByUploadAttributeID to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadTypeAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadTypeAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadTypeAttribute to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllUploadTypeTemplates to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplateByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplatesByUploadTypeCode to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadTypeTemplate to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadTypeTemplate to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadTypeTemplate to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllUploadTypeTemplateAttributes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplateAttributeByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplateAttributesByUploadTypeTemplateID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplateAttributesByUploadTypeAttributeID to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadTypeTemplateAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadTypeTemplateAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadTypeTemplateAttribute to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllUploadValues to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadValueByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadValuesByUploadAttributeID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadValuesByUploadRowID to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertUploadValue to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadValue to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadValue to IRMAClientRole

grant exec on dbo.EIM_Regen_GetAllPriceChgTypes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetPriceChgTypeByPK to IRMAClientRole

grant exec on dbo.EIM_Regen_InsertPriceChgType to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdatePriceChgType to IRMAClientRole
grant exec on dbo.EIM_Regen_DeletePriceChgType to IRMAClientRole

grant exec on dbo.Replenishment_TagPush_GetPrintLabBatchTagFile to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetPrintLabReprintTagFile to IRMAClientRole

-- custom store procs for EIM
grant exec on dbo.EIM_GetAllUploadValues to IRMAClientRole
grant exec on dbo.EIM_GetUploadValueByPK to IRMAClientRole
grant exec on dbo.EIM_GetUploadValuesByUploadAttributeID to IRMAClientRole
grant exec on dbo.EIM_GetUploadValuesByUploadRowID to IRMAClientRole

grant exec on dbo.EIM_GetAllUploadAttributes to IRMAClientRole
grant exec on dbo.EIM_GetUploadAttributeByPK to IRMAClientRole

grant exec on dbo.EIM_GetAllUploadTypeAttributes to IRMAClientRole
grant exec on dbo.EIM_GetUploadTypeAttributeByPK to IRMAClientRole
grant exec on dbo.EIM_GetUploadTypeAttributesByUploadTypeCode to IRMAClientRole
grant exec on dbo.EIM_GetUploadTypeAttributesByUploadAttributeID to IRMAClientRole

grant exec on dbo.EIM_SearchUploadSessions to IRMAClientRole
grant exec on dbo.EIM_CascadeDeleteUploadSession to IRMAClientRole
grant exec on dbo.EIM_ItemLoadSearch to IRMAClientRole
grant exec on dbo.EIM_UploadSession to IRMAClientRole
grant exec on dbo.EIM_UploadSession_DeleteItems to IRMAClientRole
grant exec on dbo.EIM_DeleteItemValidation to IRMAClientRole
grant exec on dbo.GetAllPriceTypes TO IRMAClientRole

grant exec on dbo.EIM_OptimizedInsertUploadRow TO IRMAClientRole
grant exec on dbo.EIM_OptimizedUpdateUploadRow TO IRMAClientRole

-- this allows EIM trace logging to be toggled by the user
grant exec on dbo.Administration_UpdateInstanceDataFlags to IRMAClientRole

-- various look up procs for value lists in grid cells
-- and other utility procs
grant exec on dbo.GetIdentifierTypes TO IRMAClientRole
grant exec on dbo.GetItemHierarchyByIdentifier TO IRMAClientRole
grant exec on dbo.GetItemChains TO IRMAClientRole
grant exec on dbo.fn_EIM_GetListOfItemChains TO IRMAClientRole
grant exec on dbo.EIM_AddChainNamesToIdList TO IRMAClientRole
grant exec on dbo.EIM_GetChainIdsFromNameList TO IRMAClientRole
grant exec on dbo.IsScaleIdentifier to IRMAClientRole
grant exec on dbo.EIM_LookUpHierarchyIds to IRMAClientRole
grant exec on dbo.EIM_HasNoPrimaryVendor to IRMAClientRole
grant exec on dbo.CheckItemAuthLock to IRMAClientRole
grant exec on dbo.EIM_JurisdictionValidation to IRMAClientRole
grant exec on dbo.EIM_ValidateVendorUOM to IRMAClientRole
grant exec on dbo.GetAllFacilities to IRMAClientRole
grant exec on dbo.KitchenCaseTransferRpt to IRMAClientRole
grant exec on dbo.UpdateOrderItemFreight to IRMAClientRole
grant exec on dbo.UpdateOrderCurrency to IRMAClientRole
grant exec on dbo.GetOrderItemSumQty to IRMAClientRole
grant exec on dbo.StoreOrdersTotBySKUReport to IRMAClientRole
grant exec on dbo.UpdateItemPOSData to IRMAClientRole
grant exec on dbo.GetItemByVIN_VendorID to IRMAClientRole
grant exec on dbo.EIM_ValidateDeleteVendor to IRMAClientRole
grant exec on dbo.EIM_ValidateForPriceUploadCollision  to IRMAClientRole

-- needed due to accessing tables and views using dynamic SQL
grant select on dbo.ItemScale TO IRMAClientRole
grant select on dbo.ItemScaleOverride TO IRMAClientRole

grant select on dbo.SLIM_ItemView TO IRMAClientRole
grant select on dbo.SLIM_ItemIdentifierView TO IRMAClientRole
grant select on dbo.SLIM_ItemScaleView TO IRMAClientRole
grant select on dbo.SLIM_ItemVendorView TO IRMAClientRole
grant select on dbo.SLIM_StoreItemVendorView TO IRMAClientRole
grant select on dbo.SLIM_StoreItemView TO IRMAClientRole
grant select on dbo.SLIM_PriceView TO IRMAClientRole
grant select on dbo.SLIM_VendorCostHistoryView TO IRMAClientRole
grant select on dbo.SLIM_Scale_ExtraTextView TO IRMAClientRole
grant select on dbo.SLIM_ItemAttributeView TO IRMAClientRole
grant select on dbo.SLIM_VendorDealView TO IRMAClientRole

grant exec on dbo.GetStoreItemECommerce to IRMASLIMRole
grant exec on dbo.UpdateStoreItemECommerce to IRMASLIMRole

grant select on dbo.EIM_Jurisdiction_ItemView to IRMAClientRole
grant select on dbo.EIM_Jurisdiction_ItemScaleView to IRMAClientRole

grant select on dbo.StoreJurisdiction to IRMAClientRole

-- End EIM

-- item atribute store procs
grant exec on dbo.ItemAttributes_Regen_GetAllAttributeIdentifiers to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_GetAttributeIdentifierByPK to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_InsertAttributeIdentifier to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_UpdateAttributeIdentifier to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_DeleteAttributeIdentifier to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_GetAllItemAttributes to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_GetItemAttributeByPK to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_InsertItemAttribute to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_UpdateItemAttribute to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_DeleteItemAttribute to IRMAClientRole
grant exec on dbo.ItemAttributes_GetItemAttributeByItemKey to IRMAClientRole
grant exec on dbo.ItemAttributes_GetAttributeIdentifiersByItemKey to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_GetFTPConfigForStoreAndWriterType to IRMAClientRole
grant exec on dbo.Replenishment_POSPush_GetFTPConfigForWriterType to IRMAClientRole

grant exec on dbo.AutoCloseIntraStoreTransfer to IRMAClientRole

-- Item Chaining
grant exec on dbo.DeleteItemChain TO IRMAClientRole
grant exec on dbo.InsertItemChain TO IRMAClientRole
grant exec on dbo.InsertItemChainItem TO IRMAClientRole
grant exec on dbo.GetVendor_ByCompanyNameStartsWith TO IRMAClientRole
grant exec on dbo.GetSubTeams_BySubTeam_NameStartsWith TO IRMAClientRole
grant exec on dbo.PendingRegularPriceChange TO IRMAClientRole
grant exec on dbo.GetStatesWithStores TO IRMAClientRole
grant exec on dbo.GetProdHierarchyLevel4s_ByDescriptionStartsWith TO IRMAClientRole
grant exec on dbo.GetProdHierarchyLevel3s_ByDescriptionStartsWith TO IRMAClientRole
grant exec on dbo.ItemPriceListByItemAndStore TO IRMAClientRole
grant exec on dbo.GetItemChains_ByItemKey TO IRMAClientRole
grant exec on dbo.GetStoresByZoneName TO IRMAClientRole
grant exec on dbo.GetStoresByState TO IRMAClientRole
grant exec on dbo.GetCategory_ByNameStartsWith TO IRMAClientRole
grant exec on dbo.CheckREGPriceDifference TO IRMAClientRole
grant exec on dbo.CheckBatchedPriceChange TO IRMAClientRole
grant exec on dbo.GetItemChainItems_ByItemChainID TO IRMAClientRole
grant exec on dbo.GetItemChains_ByDescriptionStartsWith TO IRMAClientRole
grant exec on dbo.GetItemChain_ByItemChainID TO IRMAClientRole
grant exec on dbo.GetBrands_ByNameStartsWith TO IRMAClientRole
grant exec on dbo.GetESRSItemList TO IRMAClientRole
grant exec on dbo.GetESRSItemSearch TO IRMAClientRole
grant exec on dbo.GetESRSBatchList TO IRMAClientRole
grant exec on dbo.GetESRSBatchDetail TO IRMAClientRole
grant exec on dbo.GetESRSPriceCheck TO IRMAClientRole
grant exec on dbo.GetESRSSubTeams TO IRMAClientRole
grant exec on dbo.GetESRSItemsByDateRange TO IRMAClientRole
grant exec on dbo.GetSubTeams TO IRMAClientRole
grant exec on dbo.GetVendors TO IRMAClientRole
grant exec on dbo.GetCategoriesAndSubTeams TO IRMAClientRole

-- Competitor Store
grant exec on dbo.DeleteCompetitorImportSession TO IRMAClientRole
grant exec on dbo.DeleteCompetitorImportInfo TO IRMAClientRole
grant exec on dbo.DeleteCompetitorPrice TO IRMAClientRole
grant exec on dbo.DeleteStoreCompetitorStore TO IRMAClientRole
grant exec on dbo.InsertCompetitorImportInfo TO IRMAClientRole
grant exec on dbo.InsertCompetitorImportSession TO IRMAClientRole
grant exec on dbo.InsertCompetitorPrice TO IRMAClientRole
grant exec on dbo.InsertCompetitorPriceFromImportSession TO IRMAClientRole
grant exec on dbo.InsertStoreCompetitorStore TO IRMAClientRole
grant exec on dbo.GetCompetitivePriceTypes TO IRMAClientRole
grant exec on dbo.GetCompetitorImportInfoExistingCount TO IRMAClientRole
grant exec on dbo.GetCompetitorImportSessionByUser_ID TO IRMAClientRole
grant exec on dbo.GetCompetitorImportSessionByCompetitorImportSessionID TO IRMAClientRole
grant exec on dbo.GetCompetitorLocations TO IRMAClientRole
grant exec on dbo.GetCompetitorPriceSearch TO IRMAClientRole
grant exec on dbo.GetCompetitors TO IRMAClientRole
grant exec on dbo.GetCompetitorStoreByName TO IRMAClientRole
grant exec on dbo.GetCompetitorStoreSearch TO IRMAClientRole
grant exec on dbo.GetFiscalWeeks TO IRMAClientRole
grant exec on dbo.GetPriceSearch TO IRMAClientRole
grant exec on dbo.GetStoreCompetitorStoresByStore_No TO IRMAClientRole
grant exec on dbo.Reporting_CompetitorTrend to IRMAClientRole
grant exec on dbo.UpdateCompetitivePriceInfo TO IRMAClientRole
grant exec on dbo.UpdateCompetitorImportInfo TO IRMAClientRole
grant exec on dbo.UpdateCompetitorImportInfoWithIdentifiers TO IRMAClientRole
grant exec on dbo.UpdateCompetitorPrice TO IRMAClientRole
grant exec on dbo.UpdateStoreCompetitorStore TO IRMAClientRole


--Ordering
grant exec on dbo.GetOrderVendorConfig to IRMAClientRole
grant exec on dbo.GetCostAdjustmentReasons TO IRMAClientRole
grant exec on dbo.InsertCostAdjustmentReason TO IRMAClientRole
grant exec on dbo.UpdateOrderItemAdjustedCost TO IRMAClientRole
grant exec on dbo.GetSuspendedPONotes TO IRMAClientRole
grant exec on dbo.UpdateSuspendedPONotes TO IRMAClientRole
grant exec on dbo.UpdateOrderHeaderCosts TO IRMAClientRole

-- Scan Gun App
grant exec on dbo.GetScanGunStoreSubTeam TO IRMAClientRole
grant exec on dbo.InsertScanGunStoreSubTeam TO IRMAClientRole

grant exec on dbo.GetEXEDistSubTeams TO IRMAClientRole
grant exec on dbo.DeleteOrderWindowEntry TO IRMAClientRole
grant exec on dbo.GetOrderWindow TO IRMAClientRole
grant exec on dbo.InsertUpdateOrderWindow TO IRMAClientRole
grant exec on dbo.GetElectronicOrderItemInfo TO IRMAClientRole

--tables
grant select on dbo.KitchenRoute to IRMAClientRole
grant select on dbo.MenuAccess to IRMAClientRole
grant select on dbo.TaxOverride to IRMAClientRole
grant select on dbo.StoreItemVendor to IRMAClientRole
grant select on dbo.BulkPromoPush to IRMAClientRole
grant select on dbo.StoreSubTeam to IRMAClientRole
grant select on dbo.CreditReasons to IRMAClientRole
grant select on dbo.TLog_UK_Transaction to IRMAClientRole
grant select on dbo.PromotionalOfferStore to IRMAClientRole
grant select on dbo.Store_MobilePrinter to IRMAClientRole
grant select on dbo.CycleCountVendor to IRMAClientRole
grant select on dbo.Users to IRMAClientRole
grant select on dbo.Date to IRMAClientRole
grant select on dbo.UsersSubTeam to IRMAClientRole
grant select on dbo.DeletedOrder to IRMAClientRole
grant select on dbo.TLog_UK_Payment to IRMAClientRole
grant select on dbo.TaxFlagChgQueue to IRMAClientRole
grant select on dbo.Vendor to IRMAClientRole
grant select on dbo.ExRule_AutoOrdersNoTitle to IRMAClientRole
grant select on dbo.VendorCostHistory to IRMAClientRole
grant select on dbo.ExRule_VendCostDIff to IRMAClientRole
grant select on dbo.VendorDealHistory to IRMAClientRole
grant select on dbo.ExRule_VendCostPackSize to IRMAClientRole
grant select on dbo.TLog_UK_Item to IRMAClientRole
grant select on dbo.WarehouseItemChange to IRMAClientRole
grant select on dbo.GLPushHistory to IRMAClientRole
grant select on dbo.WarehouseVendorChange to IRMAClientRole
grant select on dbo.GLPushQueue to IRMAClientRole
grant select on dbo.ZoneSubTeam to IRMAClientRole
grant select on dbo.ItemChangeHistory to IRMAClientRole
grant select on dbo.TLog_UK_Offers to IRMAClientRole
grant select on dbo.AICS to IRMAClientRole
grant select on dbo.ItemChgType to IRMAClientRole
grant select on dbo.AICSErrors to IRMAClientRole
grant select on dbo.ItemType to IRMAClientRole
grant select on dbo.CostImports to IRMAClientRole
grant select on dbo.MobilePrinter to IRMAClientRole
grant select on dbo.Customer to IRMAClientRole
grant select on dbo.NatItemCat to IRMAClientRole
grant select on dbo.POSWriterEscapeChars to IRMAClientRole
grant select on dbo.NatItemClass to IRMAClientRole
grant select on dbo.NatItemFamily to IRMAClientRole
grant select on dbo.NewMrkDwn to IRMAClientRole
grant select on dbo.POSWriterPricingMethods to IRMAClientRole
grant select on dbo.ODBCErrorLog to IRMAClientRole
grant select on dbo.OnHand to IRMAClientRole
grant select on dbo.OrderExportDeletedQueue to IRMAClientRole
grant select on dbo.OrderExportQueue to IRMAClientRole
grant select on dbo.OrderHeaderHistory to IRMAClientRole
grant select on dbo.PLUMCorpChgQueue to IRMAClientRole
grant select on dbo.PromotionalOffer to IRMAClientRole
grant select on dbo.CustomerReturnReason to IRMAClientRole
grant select on dbo.PLUMCorpChgQueueTmp to IRMAClientRole
grant select on dbo.PromotionalOfferMembers to IRMAClientRole
grant select on dbo.PMOrganizationChg to IRMAClientRole
grant select on dbo.PMOrganizationChgQueue to IRMAClientRole
grant select on dbo.ItemGroupMembers to IRMAClientRole
grant select on dbo.PMPriceChange to IRMAClientRole
grant select on dbo.PMProductChg to IRMAClientRole
grant select on dbo.PMProductChgQueue to IRMAClientRole
grant select on dbo.PMSalesHistory_Temp to IRMAClientRole
grant select on dbo.PaymentGroup to IRMAClientRole
grant select on dbo.NatHier_Category to IRMAClientRole
grant select on dbo.PriceBatchStatus to IRMAClientRole
grant select on dbo.NatHier_Class to IRMAClientRole
grant select on dbo.PriceChgType to IRMAClientRole
grant select on dbo.NatHier_Family to IRMAClientRole
grant select on dbo.PriceHistory to IRMAClientRole
grant select on dbo.POSChangesSave to IRMAClientRole
grant select on dbo.PricingMethod to IRMAClientRole
grant select on dbo.ProduceAvgCostFix to IRMAClientRole
grant select on dbo.Purchases to IRMAClientRole
grant select on dbo.Region to IRMAClientRole
grant select on dbo.Sales to IRMAClientRole
grant select on dbo.SalesExportQueue to IRMAClientRole
grant select on dbo.SubTeam to IRMAClientRole
grant select on dbo.Team to IRMAClientRole
grant select on dbo.TempDeliAvgCost to IRMAClientRole
grant select on dbo.TempID to IRMAClientRole
grant select on dbo.TempLastVendor to IRMAClientRole
grant select on dbo.TempNewItemList to IRMAClientRole
grant select on dbo.TempSignQueue to IRMAClientRole
grant select on dbo.Temp_PLUMIngredients to IRMAClientRole
grant select on dbo.Temp_PriceAudit to IRMAClientRole
grant select on dbo.Time to IRMAClientRole
grant select on dbo.Title to IRMAClientRole
grant select on dbo.TmpTestVendorCostImport to IRMAClientRole
grant select on dbo.UserStoreTeamTitle to IRMAClientRole
grant select on dbo.UsersHistory to IRMAClientRole
grant select on dbo.Buggy_Load to IRMAClientRole
grant select on dbo.VIMItemRegion to IRMAClientRole
grant select on dbo.PMPriceChangeLoad to IRMAClientRole
grant select on dbo.VIMRetailPriceLoad to IRMAClientRole
grant select on dbo.POSItem to IRMAClientRole
grant select on dbo.VIMVendorCostFileLoad to IRMAClientRole
grant select on dbo.POSScan_Load to IRMAClientRole
grant select on dbo.VendorCostHistoryExceptions to IRMAClientRole
grant select on dbo.Payment_Load to IRMAClientRole
grant select on dbo.VendorDealType to IRMAClientRole
grant select on dbo.Sales_Load to IRMAClientRole
grant select on dbo.VendorExportQueue to IRMAClientRole
grant select on dbo.VendorStoreFreight to IRMAClientRole
grant select on dbo.Version to IRMAClientRole
grant select on dbo.ddl_log to IRMAClientRole
grant select on dbo.Z_SQLGUARD_Reaction to IRMAClientRole
grant select on dbo.IRISKeyToIRMAKey to IRMAClientRole
grant select on dbo.tmpAllergens to IRMAClientRole
grant select on dbo.tmpBulkOrderItemFix to IRMAClientRole
grant select on dbo.tmpCentralVendorUploadList to IRMAClientRole
grant select on dbo.tmpCostList to IRMAClientRole
grant select on dbo.tmpDistSubTeam_Mapping  to IRMAClientRole
grant select on dbo.RewardType to IRMAClientRole
grant select on dbo.tmpFloralItemFix to IRMAClientRole
grant select on dbo.tmpIndexDefragList to IRMAClientRole
grant select on dbo.tmpOrderExport to IRMAClientRole
grant select on dbo.tmpOrderExportDeleted to IRMAClientRole
grant select on dbo.tmpPOSRePush to IRMAClientRole
grant select on dbo.PromotionalOfferHistory to IRMAClientRole
grant select on dbo.tmpPurchases to IRMAClientRole
grant select on dbo.tmpScanAccuracyCount to IRMAClientRole
grant select on dbo.tmpScanAccuracyList to IRMAClientRole
grant select on dbo.PromotionalOfferMembersHistory to IRMAClientRole
grant select on dbo.tmpVendExport to IRMAClientRole
grant select on dbo.tmpWholeBodyRetails to IRMAClientRole
grant select on dbo.ItemGroupHistory to IRMAClientRole
grant select on dbo.CycleCountExternalLoad to IRMAClientRole
grant select on dbo.NewItemsLoad to IRMAClientRole
grant select on dbo.DistSubTeam to IRMAClientRole
grant select on dbo.POSDataTypes to IRMAClientRole
grant select on dbo.PMSubTeamInclude to IRMAClientRole
grant select on dbo.ItemGroupMembersHistory to IRMAClientRole
grant select on dbo.Payment to IRMAClientRole
grant select on dbo.POSWriter to IRMAClientRole
grant select on dbo.PriceBatchHeader to IRMAClientRole
grant select on dbo.RuleDef to IRMAClientRole
grant select on dbo.PromotionalOfferStoreHistory to IRMAClientRole
grant select on dbo.Sales_Fact to IRMAClientRole
grant select on dbo.POSChangeType to IRMAClientRole
grant select on dbo.Zone to IRMAClientRole
grant select on dbo.ExSeverityDef to IRMAClientRole
grant select on dbo.ZoneSupply to IRMAClientRole
grant select on dbo.Buggy_Fact to IRMAClientRole
grant select on dbo.InventoryServiceImportLoad to IRMAClientRole
grant select on dbo.Buggy_SumByCashier to IRMAClientRole
grant select on dbo.Buggy_SumByRegister to IRMAClientRole
grant select on dbo.Contact to IRMAClientRole
grant select on dbo.InstanceData to IRMAClientRole
grant select on dbo.InstanceDataFlags to IRMAClientRole
grant select on dbo.InstanceDataFlagsStoreOverride to IRMAClientRole
grant select on dbo.CycleCountHeader to IRMAClientRole
grant select on dbo.CycleCountHistory to IRMAClientRole
grant select on dbo.CycleCountItems to IRMAClientRole
grant select on dbo.POSDataElement to IRMAClientRole
grant select on dbo.CycleCountMaster to IRMAClientRole
grant select on dbo.FSCustomer to IRMAClientRole
grant select on dbo.FSOrganization to IRMAClientRole
grant select on dbo.POSWriterDictionary to IRMAClientRole
grant select on dbo.InventoryLocation to IRMAClientRole
grant select on dbo.InventoryLocationItems to IRMAClientRole
grant select on dbo.Item to IRMAClientRole
grant select on dbo.ItemAdjustment to IRMAClientRole
grant select on dbo.ItemManager to IRMAClientRole
grant select on dbo.POSWriterFileConfig to IRMAClientRole
grant select on dbo.ItemBrand to IRMAClientRole
grant select on dbo.ItemCategory to IRMAClientRole
grant select on dbo.ItemConversion to IRMAClientRole
grant select on dbo.ItemHistory to IRMAClientRole
grant select on dbo.ItemIdentifier to IRMAClientRole
grant select on dbo.ItemOnOrder to IRMAClientRole
grant select on dbo.ItemOrigin to IRMAClientRole
grant select on dbo.ItemShelfLife to IRMAClientRole
grant select on dbo.ItemUnit to IRMAClientRole
grant select,update on dbo.ItemVendor to IRMAClientRole
grant select on dbo.KitchenRoute to IRMAClientRole
grant select on dbo.LastVendor to IRMAClientRole
grant select on dbo.InventoryServiceExportLoad to IRMAClientRole
grant select on dbo.StorePOSConfig to IRMAClientRole
grant select on dbo.OrderHeader to IRMAClientRole
grant select on dbo.OrderInvoice to IRMAClientRole
grant select on dbo.ItemGroup to IRMAClientRole
grant select on dbo.OrderItem to IRMAClientRole
grant select on dbo.OrderItemQueue to IRMAClientRole
grant select on dbo.PMExcludedItem to IRMAClientRole
grant select on dbo.POSChanges to IRMAClientRole
grant select on dbo.TaxJurisdiction to IRMAClientRole
grant select on dbo.POSScan to IRMAClientRole
grant select on dbo.Payment_Fact to IRMAClientRole
grant select on dbo.TaxDefinition to IRMAClientRole
grant select on dbo.Payment_SumByRegister to IRMAClientRole
grant select on dbo.Price to IRMAClientRole
grant select on dbo.LabelType to IRMAClientRole
grant select on dbo.PriceBatchDetail to IRMAClientRole
grant select on dbo.TaxFlag to IRMAClientRole
grant select on dbo.ReturnOrderList to IRMAClientRole
grant select on dbo.Sales_SumByItem to IRMAClientRole
grant select on dbo.App to IRMAClientRole
grant select on dbo.Sales_SumBySubDept to IRMAClientRole
grant select on dbo.AvgCostComps to IRMAClientRole
grant select on dbo.Shipper to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole, IRMASupportRole
grant select on dbo.AvgCostFixProduce to IRMAClientRole
grant select on dbo.SignQueue to IRMAClientRole
grant select on dbo.AvgCostHistory to IRMAClientRole
grant select on dbo.Store to IRMAClientRole
grant select on dbo.StoreItem to IRMAClientRole
grant select on dbo.AvgCostQueue to IRMAClientRole
grant select on dbo.OfferChgType to IRMAClientRole
grant select on dbo.ProdHierarchyLevel4 to IRMAClientRole
grant insert on dbo.CycleCountItems to IRMAClientRole
grant insert on dbo.InventoryLocationItems to IRMAClientRole
grant insert on dbo.TaxJurisdiction to IRMAClientRole
grant update on dbo.TaxJurisdiction to IRMAClientRole
grant delete on dbo.TaxJurisdiction to IRMAClientRole

grant select on dbo.StoreItem to IRMAClientRole
grant select on dbo.ProdHierarchyLevel3 to IRMAClientRole
grant select on dbo.ProdHierarchyLevel4 to IRMAClientRole
grant select on dbo.ItemDefaultAttribute to IRMAClientRole
grant select on dbo.ItemDefaultValue to IRMAClientRole
grant select on dbo.UOM_Conversion to IRMAClientRole
grant exec on dbo.GetJobStatus to IRMAClientRole
grant exec on dbo.InsertJobStatus to IRMAClientRole
grant exec on dbo.UpdateJobStatus to IRMAClientRole
grant exec on dbo.InsertJobError to IRMAClientRole
grant select, insert, update, delete on dbo.ReturnOrder to IRMAClientRole
grant select on dbo.ItemOverride to IRMAClientRole
grant select on dbo.StoreShelfTagConfig to IRMAClientRole
grant select on dbo.OrderExternalSource to IRMAClientRole
grant select on dbo.OrderExternalSource to IRMAReportsRole
grant select on dbo.VendorItemStatuses to IRMAClientRole
grant select, insert, update, delete on dbo.IconItemLastChange to IRMAClientRole
--Planogram
grant select, insert on dbo.Planogram to IRMAClientRole

--Batch Receive and Close
grant exec on dbo.GetOrderReceivingDisplayInfo to IRMAClientRole
grant exec on dbo.BRC_GetFacilities to IRMAClientRole
grant exec on dbo.BRC_GetOrders to IRMAClientRole
grant exec on dbo.BRC_ReceiveOrder to IRMAClientRole

-- Begin EIM
-- Table Grants
grant select on dbo.UploadAttribute to IRMAClientRole
grant select on dbo.UploadRow to IRMAClientRole
grant select on dbo.UploadSession to IRMAClientRole
grant select on dbo.UploadSessionUploadType to IRMAClientRole
grant select on dbo.UploadSessionUploadTypeStore to IRMAClientRole
grant select on dbo.UploadType to IRMAClientRole
grant select on dbo.UploadTypeAttribute to IRMAClientRole
grant select on dbo.UploadTypeTemplate to IRMAClientRole
grant select on dbo.UploadTypeTemplateAttribute to IRMAClientRole
grant select on dbo.UploadValue to IRMAClientRole

-- END EIM

-- Item Atribute Tables
grant select on dbo.AttributeIdentifier to IRMAClientRole
grant select, insert, update on dbo.ItemAttribute to IRMAClientRole
grant update on dbo.Item to IRMAClientRole
grant update on dbo.Price to IRMAClientRole
grant select on dbo.Scale_ExtraText to IRMAClientRole

-- Item Chaining
grant select ON dbo.ItemChainItem to IRMAClientRole
grant select ON dbo.ItemChain to IRMAClientRole

-- Competitor Store Data
grant select ON dbo.FiscalWeek to IRMAClientRole
grant select ON dbo.Competitor to IRMAClientRole
grant select ON dbo.CompetitorLocation to IRMAClientRole
grant select ON dbo.CompetitorStore to IRMAClientRole
grant select ON dbo.CompetitorStoreIdentifier to IRMAClientRole
grant select ON dbo.CompetitorStoreItemIdentifier to IRMAClientRole
grant select ON dbo.CompetitorPrice to IRMAClientRole
grant select ON dbo.CompetitorImportSession to IRMAClientRole
grant select ON dbo.CompetitorImportInfo to IRMAClientRole
grant select ON dbo.StoreCompetitorStore to IRMAClientRole
grant select ON dbo.CompetitivePriceType to IRMAClientRole


--functions

grant exec on dbo.fn_PeriodBeginDate to IRMAClientRole
grant exec on dbo.fn_Price to IRMAClientRole
grant exec on dbo.fn_PriceBatchDetailCount to IRMAClientRole
grant exec on dbo.fn_PriceHistoryPrice to IRMAClientRole
grant exec on dbo.fn_PriceHistoryRegPrice to IRMAClientRole
grant exec on dbo.fn_CompPrice to IRMAClientRole
grant exec on dbo.fn_CompRegPrice to IRMAClientRole
grant exec on dbo.fn_VendorType to IRMAClientRole
grant exec on dbo.fn_GetTorexJulianDate to IRMAClientRole
grant exec on dbo.fn_JulianDate to IRMAClientRole
grant exec on dbo.fn_AvgCostHistory to IRMAClientRole
grant exec on dbo.fn_CalcBarcodeCheckDigit to IRMAClientRole
grant exec on dbo.fn_ConvertItemUnit to IRMAClientRole
grant exec on dbo.fn_CostConversion to IRMAClientRole
grant exec on dbo.fn_GetCustomerType to IRMAClientRole, IRMAReportsRole
grant exec on dbo.fn_IsEXEDistributed to IRMAClientRole
grant exec on dbo.fn_IsScaleItem to IRMAClientRole
grant exec on dbo.fn_ItemSalesQty to IRMAClientRole
grant exec on dbo.fn_UpdatePriceBatchDetailOffer to IRMAClientRole
grant exec on dbo.fn_GetStoreItemVendorID to IRMAClientRole
grant exec on dbo.fn_GetLastCost to IRMAClientRole
grant exec on dbo.fn_GetCurrentCost to IRMAClientRole, IRMAReportsRole
grant exec on dbo.fn_GetMargin to IRMAClientRole
grant exec on dbo.fn_ItemSalesQty_NotSO to IRMAClientRole
grant exec on dbo.fn_InstanceDataValue to IRMAClientRole, IRMAReports
grant exec on dbo.fn_PricingMethodMoney to IRMAClientRole
grant exec on dbo.fn_PricingMethodInt to IRMAClientRole
grant exec on dbo.fn_GetCurrentNetCost to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_GetCurrentNetUnitCost to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_GetCurrentVendorPackage_Desc1 to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole, IRMASLIMRole
grant exec on dbo.fn_GetCurrentVendorPackInfo to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole, IRMASLIMRole
grant exec on dbo.fn_GetCurrentSumAllowances to IRMAClientRole
grant exec on dbo.fn_GetCurrentSumDiscounts to IRMAClientRole
grant exec on dbo.fn_IsItemAuthorizedForStore to IRMAClientRole
grant exec on dbo.fn_HasPrimaryVendor to IRMAClientRole
grant exec on dbo.fn_ValidatePromoPriceChange to IRMAClientRole
grant exec on dbo.fn_ValidateRegularPriceChange to IRMAClientRole
grant exec on dbo.fn_IsSameDayPromoChgConflict to IRMAClientRole
grant exec on dbo.fn_GetIsBatched to IRMAClientRole
grant exec on dbo.fn_GetLastCost to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_GetLastWeekMovement to IRMAReportsRole
grant exec on dbo.fn_GetMargin to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_GetMargin to IRMASLIMRole
grant exec on dbo.fn_GetPreviousAvgCost TO IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on dbo.fn_GetPreviousAvgCostEffDate TO IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on dbo.fn_GetPreviousAvgCostSource TO IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on dbo.fn_GetRetailUnitAbbreviation to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_GetScalePLU to IRMAAdminRole, IRMAClientRole
grant exec on dbo.fn_GetScaleUPC to IRMAAdminRole, IRMAClientRole
grant exec on dbo.fn_GetStoreItemVendorID to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_GetSumSalesDollars to IRMASLIMRole
grant exec on dbo.fn_GetSumSalesQuantity to IRMASLIMRole
grant exec on dbo.fn_GetTorexJulianDate to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_GetUOMConversionFactor to IRMAClientRole
grant exec on dbo.fn_GetVendorPack to IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMASchedJobsRole, IRMAAdminRole
grant exec on dbo.fn_GetVendorPackSize to IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMASchedJobsRole, IRMAAdminRole
grant exec on dbo.fn_GetVendorPSVendorId to IRMAClientRole
grant exec on dbo.fn_GetVendorTransmissionType to IRMAClientRole
grant exec on dbo.fn_GetWeightOrQtySold to IRMAReportsRole
grant exec on dbo.fn_GLAcctIncludesTeamSubteam to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on dbo.fn_HasPrimaryVendor to IRMAClientRole
grant exec on dbo.fn_InstanceDataValue to IRMAClientRole, IRMASupportRole
grant exec on dbo.fn_IsAvgCostAdjReasonActive TO IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole 
grant exec on dbo.fn_IsCaseItemUnit to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_IsDistributionCenter to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.fn_isEInvoice to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on dbo.fn_isEmpDiscountException to IRMAAdminRole, IRMAClientRole
grant exec on dbo.fn_IsEXEDistributed to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_IsInternalVendor to IRMAAdminRole, IRMAClientRole
grant exec on dbo.fn_IsItemAuthorizedForStore to IRMAClientRole
grant exec on dbo.fn_IsItemPrimaryVendor to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.fn_IsOrderWindowOpen to IRMAClientRole
grant exec on dbo.fn_IsPayByAgreedCostStoreVendor to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_IsRetailLocation TO IRMAAdminRole, IRMAClientRole
grant exec on dbo.fn_IsSameDayPromoChgConflict to IRMAClientRole
grant exec on dbo.fn_IsScaleIdentifier to IRMAClientRole, IRMASLIMRole, IRMASupportRole
grant exec on dbo.fn_IsScaleIdentifier to IRMASLIMRole
grant exec on dbo.fn_isScaleItem to ExtractRole
grant exec on dbo.fn_IsScaleItem to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole, spice_user, [WFM\IRMA.developers]
grant exec on dbo.fn_IsUserInSLIM to IRMAAdminRole, IRMAClientRole
grant exec on dbo.fn_IsVendorEInvoice to IRMAClientRole, IRMAAdminRole
grant exec on dbo.fn_IsWarningValidationCode to IRMAAdminRole, IRMAClientRole
grant exec on dbo.fn_IsWarningValidationCode to IRMASLIMRole
grant exec on dbo.fn_ItemNetDiscountDateRange to IRMAReportsRole
grant exec on dbo.fn_ItemSalesQty to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_ItemSalesQty_NotSO to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_JulianDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_MSRP_Price to IRMAClientRole
grant exec on dbo.fn_MSRP_UnitsPerPrice to IRMAClientRole
grant exec on dbo.fn_NonPreOrderItemsExist to IRMAClientRole
grant exec on dbo.fn_OnSale to IMHARole
grant exec on dbo.fn_OnSale to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.fn_PeriodBeginDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_PreOrderItemsExist to IRMAClientRole
grant exec on dbo.fn_Price to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_PriceBatchDetailCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_PriceHistoryPrice to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_PriceHistoryRegPrice to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_PrvAvgCostHistory to IRMAClientRole, IRMAReportsRole
grant exec on dbo.fn_PrvOnHoldQtyAvgCostHistory to IRMAClientRole, IRMAReportsRole
grant exec on dbo.fn_QuarterBeginDate to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.fn_RegularCurrentPrice to IRMAClientRole
grant exec on dbo.fn_SaleItemUnitsPerPrice to IRMAClientRole
grant exec on dbo.fn_SalePrice to IRMAClientRole
grant exec on dbo.fn_SellingUnitWeight to IRMAClientRole
grant exec on dbo.fn_PricingMethodMoney to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_PricingMethodInt to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_StoreSubTeamExists to IRMAClientRole
grant exec on dbo.fn_UnitPrice to IRMAClientRole
grant exec on dbo.fn_UnitsPerPrice to IRMAClientRole
grant exec on dbo.fn_UpdatePriceBatchDetailOffer to IRMAClientRole
grant exec on dbo.fn_ValidateDescriptionCharacters to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.fn_ValidatePromoPriceChange to IRMAClientRole
grant exec on dbo.fn_ValidateRegularPriceChange to IRMAClientRole
grant exec on dbo.fn_VendorExists to IRMAClientRole
grant exec on dbo.fn_VendorKeyExists TO IRMAReportsRole, IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IMHARole, IRMASLIMRole
grant exec on dbo.fn_VendorType to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.fn_IsMultipleMatchedVendors to IRMAClientRole
grant exec on dbo.fn_GetAppConfigValue to IRMAClientRole



--***************************
-- Functions Returning Tables
--***************************
grant select on dbo.fn_GetMatchingTolerance to IRMAClientRole
grant select on dbo.fn_3WayMatchDetails to IRMAClientRole
grant select on dbo.fn_3WayMatchDetails to IRMAReportsRole
grant select on dbo.fn_3WayMatchDetails to IRMASchedJobsRole
grant select on dbo.fn_GetNetCost to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant select on dbo.fn_GetPLUMCorpOrScalePriceChangeKeys to IRMAAdminRole
grant select on dbo.fn_GetPLUMCorpOrScalePriceChangeKeys to IRMASchedJobsRole
grant select on dbo.fn_getShelfTagType to IRMAClientRole
grant select on dbo.fn_GetUnsentOrders to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant select on dbo.fn_GetVendor_SubteamsItemCount to IRMAReportsRole
grant select on dbo.fn_InventoryDetails to IRMAClientRole
grant select on dbo.fn_InventoryDetails to IRMAReportsRole
grant select on dbo.fn_InventoryValue to IRMAAdminRole
grant select on dbo.fn_InventoryValue to IRMAClientRole
grant select on dbo.fn_InventoryValue to IRMAExcelRole
grant select on dbo.fn_InventoryValue to IRMASchedJobsRole
grant select on dbo.fn_ItemIdentifiers      to IRMAClientRole
grant select on dbo.fn_MVParam to IRMAReportsRole
grant select on dbo.fn_Parse_List to IRMAAVCIRole
grant select on dbo.fn_Parse_List to IRMAClientRole
grant select on dbo.fn_Parse_List to IRMAReportsRole
grant select on dbo.fn_Parse_List to IRMASchedJobsRole
grant select on dbo.fn_Parse_List to IRMASLIMRole
grant select on dbo.fn_Parse_List to IRMASupportRole
grant select on dbo.fn_ParseIntStringList to IRMAAVCIRole
grant select on dbo.fn_ParseIntStringList to IRMAClientRole
grant select on dbo.fn_ParseIntStringList to IRMAReportsRole
grant select on dbo.fn_ParseIntStringList to IRMASchedJobsRole
grant select on dbo.fn_ParseIntStringList to IRMASLIMRole
grant select on dbo.fn_ParseIntStringList to IRMASupportRole
grant select on dbo.fn_Parse_List_Two to IRMAAdminRole
grant select on dbo.fn_Parse_List_Four to IRMAClientRole, IRMAAdminRole
grant select on dbo.fn_ParseStringList to IRMAReportsRole
grant select on dbo.fn_TaxFlagData to IRMAClientRole
grant select on dbo.fn_TaxFlagData to IRMAReportsRole
grant select on dbo.fn_TaxFlagData to IRMASchedJobsRole
grant select on dbo.fn_TaxFlagData to IRMASupportRole
grant select on dbo.fn_VendorCost to IRMAReportsRole, IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IMHARole, IRMASupportRole
grant select on dbo.fn_VendorCostAll to ExtractRole
grant select on dbo.fn_VendorCostAll to IMHARole
grant select on dbo.fn_VendorCostAll to IRMAAdminRole
grant select on dbo.fn_VendorCostAll to IRMAClientRole
grant select on dbo.fn_VendorCostAll to IRMAReportsRole
grant select on dbo.fn_VendorCostAll to IRMASchedJobsRole
grant select on dbo.fn_VendorCostAll to IRMASupportRole
grant select on dbo.fn_VendorCostAllOrderItemInfoPackSizes to IRMAClientRole
grant select on dbo.fn_VendorCostAllpackSizes to IRMAClientRole
grant select on dbo.fn_VendorCostByPromo to IRMAAVCIRole, IRMAAdminRole, IRMAClientRole, IRMAExcelRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant select on dbo.fn_VendorCostItems to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant select on dbo.fn_VendorCostItemsStores to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant select on dbo.fn_VendorCostStores to IRMAAVCIRole
grant select on dbo.fn_VendorCostStores to IRMAClientRole
grant select on dbo.fn_VendorCostStores to IRMAReportsRole
grant select on dbo.fn_VendorCostStores to IRMASchedJobsRole
grant select on dbo.fn_VendorCostStores to IRMASupportRole
grant select on dbo.fn_VendorCostStoresValidation to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant select on dbo.fn_VendorsCost to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant select on dbo.fn_TaxFlagData to IRMAReportsRole
grant select on dbo.fn_ParseStringList to IRMAReportsRole
grant select on dbo.fn_MVParam to IRMAReportsRole
grant exec on dbo.fn_GetCurrentHandlingCharge to IRMAReportsRole
grant exec on dbo.fn_GetFacilityHandlingCharge to IRMAReportsRole
grant exec on dbo.fn_GetFacilityHandlingChargeOverride to IRMAReportsRole

grant execute on dbo.Reporting_DASHandlingCharge to IRMAReportsRole

go


---- IRMAExcelRole ----

--stored procedures
grant exec on dbo.GetAllFacilities to IRMAExcelRole
grant exec on dbo.KitchenCaseTransferRpt to IRMAExcelRole
grant exec on dbo.UpdateOrderItemFreight to IRMAExcelRole
grant exec on dbo.GetOrderItemSumQty to IRMAExcelRole
grant exec on dbo.StoreOrdersTotBySKUReport to IRMAExcelRole
grant exec on dbo.CheckForDuplicateIdentifier to IRMAExcelRole
grant exec on dbo.CheckIdentifierInItemVendor to IRMAExcelRole
grant exec on dbo.GetBrandAndID to IRMAExcelRole
grant exec on dbo.GetCategoriesBySubTeam to IRMAExcelRole
grant exec on dbo.GetCategoriesAndSubTeams to IRMAExcelRole
grant exec on dbo.GetDistSubTeams to IRMAExcelRole
grant exec on dbo.GetItemInfoByIdentifier to IRMAExcelRole
grant exec on dbo.GetItemManagers to IRMAExcelRole
grant exec on dbo.GetItemUnitsCost to IRMAExcelRole
grant exec on dbo.GetItemUnitsPDU to IRMAExcelRole
grant exec on dbo.GetItemUnitsVendor to IRMAExcelRole
grant exec on dbo.GetNatClass to IRMAExcelRole
grant exec on dbo.GetRipeZones to IRMAExcelRole
grant exec on dbo.GetStores to IRMAExcelRole
grant exec on dbo.GetStoreOnHand to IRMAExcelRole
grant exec on dbo.GetStoreOnHandDetail to IRMAExcelRole
grant exec on dbo.GetSubTeamByProductType to IRMAExcelRole
grant exec on dbo.GetPriceTypes to IRMAExcelRole
grant exec on dbo.GetSubTeams to IRMAExcelRole
grant exec on dbo.GetUnitAndID to IRMAExcelRole
grant exec on dbo.GetVendors to IRMAExcelRole
grant exec on dbo.GetZones to IRMAExcelRole
grant exec on dbo.GetCategoryAndID to IRMAExcelRole
grant exec on dbo.GetLabelTypes to IRMAExcelRole
grant exec on dbo.GetTaxClasses to IRMAExcelRole
grant exec on dbo.GetVendorItems to IRMAExcelRole
grant exec on dbo.GetVendorByPSVendorID to IRMAExcelRole
grant exec on dbo.GetVendorCost to IRMAExcelRole
grant exec on dbo.IdentifyItem to IRMAExcelRole
grant exec on dbo.GetInstanceData to IRMAExcelRole
grant exec on dbo.GetInstanceDataFlags to IRMAExcelRole
grant exec on dbo.InventoryValueDetail to IRMAExcelRole
grant exec on dbo.InventoryValueSummary  to IRMAExcelRole
grant select on dbo.fn_InventoryValue to IRMAExcelRole

--tables
grant references on dbo.Item to IRMAExcelRole
grant select on dbo.Item to IRMAExcelRole
grant insert on dbo.Item to IRMAExcelRole
grant delete on dbo.Item to IRMAExcelRole
grant update on dbo.Item to IRMAExcelRole
grant references on dbo.ItemIdentifier to IRMAExcelRole
grant select on dbo.ItemIdentifier to IRMAExcelRole
grant insert on dbo.ItemIdentifier to IRMAExcelRole
grant delete on dbo.ItemIdentifier to IRMAExcelRole
grant update on dbo.ItemIdentifier to IRMAExcelRole
grant references on dbo.ItemVendor to IRMAExcelRole
grant select on dbo.ItemVendor to IRMAExcelRole
grant insert on dbo.ItemVendor to IRMAExcelRole
grant delete on dbo.ItemVendor to IRMAExcelRole
grant update on dbo.ItemVendor to IRMAExcelRole
grant references on dbo.Price to IRMAExcelRole
grant select on dbo.Price to IRMAExcelRole
grant insert on dbo.Price to IRMAExcelRole
grant delete on dbo.Price to IRMAExcelRole
grant update on dbo.Price to IRMAExcelRole
grant references on dbo.StoreItemVendor to IRMAExcelRole
grant select on dbo.StoreItemVendor to IRMAExcelRole
grant insert on dbo.StoreItemVendor to IRMAExcelRole
grant delete on dbo.StoreItemVendor to IRMAExcelRole
grant update on dbo.StoreItemVendor to IRMAExcelRole
grant select on dbo.NewItemsLoad to IRMAExcelRole
grant insert on dbo.NewItemsLoad to IRMAExcelRole
grant delete on dbo.NewItemsLoad to IRMAExcelRole
grant update on dbo.NewItemsLoad to IRMAExcelRole
grant select on dbo.ItemCategory to IRMAExcelRole
grant select on dbo.Vendor to IRMAExcelRole
grant select on dbo.Store to IRMAExcelRole
grant select on dbo.StoreItem to IRMAExcelRole
grant select on dbo.Zone to IRMAExcelRole
grant select on dbo.InstanceData to IRMAExcelRole
grant select on dbo.InstanceDataFlags to IRMAExcelRole
grant select on dbo.InstanceDataFlagsStoreOverride to IRMAExcelRole


--functions
grant exec on dbo.Fn_GetCustomerType to IRMAExcelRole
go

--IRMASLIMRole Table Permissions--
grant select on dbo.SubTeam to IRMASLIMRole
grant select on dbo.Team to IRMASLIMRole
grant select on dbo.Store to IRMASLIMRole
grant select on dbo.StoreItem to IRMASLIMRole
grant select on dbo.Title to IRMASLIMRole
grant select on dbo.Sales_SumByItem to IRMASLIMRole
grant select on dbo.NatItemClass to IRMASLIMRole
grant select on dbo.NatItemCat to IRMASLIMRole
grant select on dbo.NatItemFamily to IRMASLIMRole
grant select, insert, update, delete on dbo.UserStoreTeamTitle to IRMASLIMRole
grant select, insert, update, delete on dbo.UsersSubTeam to IRMASLIMRole
grant select on dbo.Users to IRMASLIMRole
grant select, insert, update,delete on dbo.Item to IRMASLIMRole
grant select, insert, update,delete on dbo.ItemManager to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemBrand to IRMASLIMRole, IRSUser
grant select on dbo.ItemCategory to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemIdentifier to IRMASLIMRole
grant select on dbo.ItemUnit to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemVendor to IRMASLIMRole
grant select, insert, update, delete on dbo.Price to IRMASLIMRole
grant select,insert, update, delete on dbo.PriceBatchDetail to IRMASLIMRole
grant select,insert,update,delete on dbo.ItemRequest to IRMASLIMRole
grant select,insert,update,delete on dbo.VendorRequest to IRMASLIMRole
grant select,insert,update,delete on dbo.ItemRequest_Status to IRMASLIMRole
grant select,insert,update,delete on dbo.ItemRequestIdentifier_Type to IRMASLIMRole
grant select,insert,update,delete on dbo.VendorRequest_Status to IRMASLIMRole
grant select,insert,update,delete on dbo.UserAccess to IRMASLIMRole
grant select,insert,update,delete on dbo.SLIMAccess to IRMASLIMRole
grant select,insert,update,delete on dbo.SLIMEmail to IRMASLIMRole
grant select,insert,update,delete on dbo.ItemRequest to IRMASLIMRole
grant select, insert, update, delete on dbo.VendorCostHistory to IRMASLIMRole
grant select, insert, update, delete on dbo.VendorDealHistory to IRMASLIMRole
grant select, insert, update, delete on dbo.Vendor to IRMASLIMRole
grant select, insert, update, delete on dbo.StoreItemVendor to IRMASLIMRole
grant select, insert, update, delete on dbo.ODBCErrorLOg to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemScale to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemAttribute to IRMASLIMRole
grant select, insert, update, delete on dbo.Scale_ExtraText to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemScaleRequest to IRMASLIMRole
grant select, insert, update, delete on dbo.StoreJurisdiction to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemOverride to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemScaleOverride to IRMASLIMRole
grant select on dbo.Scale_RandomWeightType to IRMASLIMRole
grant select on dbo.PriceChgType to IRMASLIMRole
grant select on dbo.Scale_Tare to IRMASLIMRole
grant select,insert,update,delete on dbo.SLIM_InStoreSpecials to IRMASLIMRole
grant select,insert,update,delete on dbo.SLIM_StatusTypes to IRMASLIMRole
grant select on dbo.StoreSubTeam to IRMASLIMRole
grant select on dbo.ItemOrigin to IRMASLIMRole
grant select on dbo.LabelType to IRMASLIMRole
grant select on dbo.DistSubTeam to IRMASLIMRole
grant select on dbo.Scale_LabelType to IRMASLIMRole
grant select on dbo.Scale_LabelStyle to IRMASLIMRole
grant select on dbo.LabelType to IRMASLIMRole
grant select on dbo.AttributeIdentifier to IRMASLIMRole
grant select on dbo.version to IRMASLIMRole
grant select on dbo.region to IRMASLIMRole
go


--IRMASLIMRole Stored Procedure Permissions--
grant exec on dbo.GetInstanceDataFlagValue to IRMASLIMRole
grant exec on dbo.GetAllFacilities to IRMASLIMRole
grant exec on dbo.KitchenCaseTransferRpt to IRMASLIMRole
grant exec on dbo.UpdateOrderItemFreight to IRMASLIMRole
grant exec on dbo.GetOrderItemSumQty to IRMASLIMRole
grant exec on dbo.GetItemWebQuery to IRMASLIMRole
grant exec on dbo.InsertSLIMVendor to IRMASLIMRole
grant exec on dbo.GetItemWebQueryStore to IRMASLIMRole
grant exec on dbo.ItemWebQueryDetail to IRMASLIMRole
grant exec on dbo.ItemWebQueryStoreDetail to IRMASLIMRole
grant exec on dbo.ItemWebQueryDetailMovement to IRMASLIMRole
grant exec on dbo.GetSubTeamBrand to IRMASLIMRole
grant exec on dbo.GetAllSubTeams to IRMASLIMRole
grant exec on dbo.GetStoresByUser to IRMASLIMRole
grant exec on dbo.CheckForDuplicateIdentifier to IRMASLIMRole
grant exec on dbo.GetVendors to IRMASLIMRole
grant exec on dbo.GetStores to IRMASLIMRole
grant exec on dbo.InsertItemRequest to IRMASLIMRole
grant exec on dbo.GetItemAuthorization to IRMASLIMRole
grant exec on dbo.InsertItem to IRMASLIMROLE
grant exec on dbo.InsertVendor to IRMASLIMROLE
grant exec on dbo.InsertStoreItemVendor to IRMASLIMROLE
grant exec on dbo.SetPrimaryVendor to IRMASLIMROLE
grant exec on dbo.InsertItemVendor to IRMASLIMROLE
grant exec on dbo.UpdateItemVendor to IRMASLIMROLE
grant exec on dbo.UpdatePriceBatchDetailReg to IRMASLIMROLE
grant exec on dbo.UpdatePriceBatchDetailPromo to IRMASLIMROLE
grant exec on dbo.InsertVendorCostHistory to IRMASLIMROLE
grant exec on dbo.UpdateVendorInfo to IRMASLIMROLE
grant exec on dbo.DeleteStoreItemVendor to IRMASLIMROLE
grant exec on dbo.InsertODBCError to IRMASLIMRole
grant exec on dbo.GetBrandAndID to IRMASLIMRole
grant exec on dbo.GetItemPendPrice to IRMASLIMRole
grant exec on dbo.UpdateSLIMItemAttribute to IRMASLIMRole
grant exec on dbo.Scale_GetScaleUOMs to IRMASLIMRole
grant exec on dbo.Scale_InsertUpdateItemScaleDetails to IRMASLIMRole
grant exec on dbo.Scale_GetRandomWeightTypes to IRMASLIMRole
grant exec on dbo.Scale_InsertUpdateExtraText to IRMASLIMRole
grant exec on dbo.Scale_GetExtraTextCombo to IRMASLIMRole
grant exec on dbo.GetTeams to IRMASLIMRole
grant exec on dbo.GetItemUnitsCost to IRMASLIMRole
grant exec on dbo.GetStoreItemAuths to IRMASLIMRole
grant exec on dbo.GetStoreItemECommerce to IRMASLIMRole
grant exec on dbo.SLIM_RejectStoreSpecial to IRMASLIMRole
grant exec on dbo.SLIM_ProcessStoreSpecial to IRMASLIMRole
grant exec on dbo.SLIM_GetInStoreSpecials to IRMASLIMRole
grant exec on dbo.SLIM_StoreSpecialsStatus to IRMASLIMRole
grant exec on dbo.SLIM_CreateStoreSpecial to IRMASLIMRole
grant exec on dbo.SLIM_ReProcessStoreSpecial to IRMASLIMRole
grant exec on dbo.SLIM_ReProcessItemRequest to IRMASLIMRole
grant exec on dbo.SLIM_RejectItemRequest to IRMASLIMRole
grant exec on dbo.SLIM_DeleteItemRequest to IRMASLIMRole
grant exec on dbo.SLIM_GetItemRejectInfo to IRMASLIMRole
grant exec on dbo.GetCategoriesBySubTeam to IRMASLIMRole
grant exec on dbo.GetProdHierarchyLevel3sByCategory to IRMASLIMRole
grant exec on dbo.GetItemPrimVend to IRMASLIMRole
grant exec on dbo.UpdateStoreItem to IRMASLIMRole
grant exec on dbo.GetIdentifierTypes to IRMASLIMRole
grant exec on dbo.GetItemOverride to IRMASLIMRole
grant exec on dbo.InsertUpdateItemOverride to IRMASLIMRole
grant exec on dbo.Scale_GetItemScaleOverride to IRMASLIMRole
grant exec on dbo.Scale_InsertUpdateItemScaleOverride to IRMASLIMRole
grant exec on dbo.GetValidationCodeDetails to IRMASLIMRole
grant exec on dbo.CheckItemAuthLock to IRMASLIMRole
grant exec on dbo.GetBrands_ByNameStartsWith to IRMASLIMRole
grant exec on dbo.GetVendor_ByCompanyNameStartsWith to IRMASLIMRole
grant exec on dbo.GetBrand_ByNameExact to IRMASLIMRole
grant exec on dbo.GetVendor_ByCompanyNameExact to IRMASLIMRole
grant exec on dbo.GetTaxClass_ByTaxClassDescStartsWith to IRMASLIMRole
grant exec on dbo.GetTaxClass_ByDescExact to IRMASLIMRole
grant exec on dbo.GetUserStoreTeamTitles_ByUser_ID to IRMASLIMRole
grant exec on dbo.GetInstanceData to IRMASLIMRole
grant exec on dbo.SLIM_GetItemOverride to IRMASLIMRole
grant exec on dbo.ItemWebQueryDetailShort to IRMASLIMRole
grant exec on dbo.ItemWebQueryScaleDetail to IRMASLIMRole
grant exec on dbo.GetItemUnitsCost to IRMASLIMRole
grant select on dbo.CostPromoCodeType to IRMAReportsRole
grant exec on dbo.fn_VendorKeyExists TO IRMAReportsRole, IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IMHARole, IRMASLIMRole

go

--IRMASLIMRole Functions Permissions--

grant exec on dbo.Fn_GetCustomerType to IRMASLIMRole
grant exec on dbo.Fn_GetMargin to IRMASLIMRole
grant exec on dbo.fn_GetCurrentNetCost to IRMASLIMRole
grant exec on dbo.fn_GetCurrentNetUnitCost to IRMASLIMRole
grant exec on dbo.fn_GetCurrentSumAllowances to IRMASLIMRole
grant exec on dbo.fn_GetCurrentSumDiscounts to IRMASLIMRole
grant exec on dbo.fn_GetSumSalesDollars to IRMASLIMRole
grant exec on dbo.fn_GetSumSalesQuantity to IRMASLIMRole
grant exec on dbo.fn_IsScaleIdentifier to IRMASLIMRole
grant exec on dbo.fn_IsWarningValidationCode to IRMASLIMRole
grant select on dbo.fn_Parse_List to IRMASLIMRole

go


--IRMAPromoRole Table Permissions--
grant select on dbo.SubTeam to IRMAPromoRole
grant select on dbo.Team to IRMAPromoRole
grant select on dbo.Store to IRMAPromoRole
grant select on dbo.StoreItem to IRMAPromoRole
grant select on dbo.Title to IRMAPromoRole
grant select on dbo.UserStoreTeamTitle to IRMAPromoRole
grant select on dbo.UsersSubTeam to IRMAPromoRole
grant select on dbo.Users to IRMAPromoRole
grant select on dbo.Item to IRMAPromoRole
grant select on dbo.ItemManager to IRMAPromoRole
grant select on dbo.ItemBrand to IRMAPromoRole
grant select on dbo.ItemCategory to IRMAPromoRole
grant select on dbo.ItemIdentifier to IRMAPromoRole
grant select on dbo.ItemUnit to IRMAPromoRole
grant select on dbo.ItemVendor to IRMAPromoRole
grant select on dbo.Price to IRMAPromoRole
grant select,insert on dbo.PriceBatchDetail to IRMAPromoRole
grant select,insert,update,delete on dbo.PriceBatchPromo to IRMAPromoRole
grant select,insert,update,delete on dbo.PromoPreOrders to IRMAPromoRole
grant select,insert,update,delete on dbo.PriceBatchDetail to IRMAPromoRole
grant select on dbo.VendorCostHistory to IRMAPromoRole
grant select on dbo.StoreItemVendor to IRMAPromoRole
grant select on dbo.Vendor to IRMAPromoRole
grant select on dbo.StoreItemVendor to IRMAPromoRole
go


--IRMAPromoRole Stored Procedure Permissions--
grant exec on dbo.UpdateOrderItemFreight to IRMAPromoRole
grant exec on dbo.GetOrderItemSumQty to IRMAPromoRole
grant exec on dbo.GetPromo to IRMAPromoRole
grant exec on dbo.GetPromoOrders to IRMAPromoRole
grant exec on dbo.GetPromoPreOrder to IRMAPromoRole
grant exec on dbo.UpdatePromoPreOrder to IRMAPromoRole
grant exec on dbo.InsertPromoPlanner to IRMAPromoRole
grant exec on dbo.UpdatePromoItem to IRMAPromoRole
grant exec on dbo.UpdatePromoItemRev to IRMAPromoRole
grant exec on dbo.UpdatePriceBatchDetailPromoPlanner to IRMAPromoRole
grant exec on dbo.InsertPromoPlannerFromEIM to IRMAPromoRole
grant exec on dbo.PromoPivotTable to IRMAPromoRole
grant exec on dbo.InsertODBCError to IRMAPromoRole

go

---- IMHARole ----

--stored procedure
grant exec on dbo.IMHA_Update_Costs to IMHARole
grant exec on dbo.IMHA_Update_Promo to IMHARole
grant exec on dbo.IMHA_Update_VIN to IMHARole
grant exec on dbo.InsertCostPromoCodeType to IMHARole
grant exec on dbo.InsertODBCError to IMHARole

--tables
grant select on dbo.Item to IMHARole
grant select on dbo.ItemIdentifier to IMHARole
grant select on dbo.ItemBrand to IMHARole
grant select on dbo.ItemUnit to IMHARole
grant select on dbo.ItemManager to IMHARole
grant select on dbo.Price to IMHARole
grant select on dbo.Store to IMHARole
grant select on dbo.StoreItem to IMHARole
grant select on dbo.SubTeam to IMHARole
grant select on dbo.VendorDealType to IMHARole
grant select on dbo.CostPromoCodeType to IMHARole
grant select on dbo.VendorCostHistory to IMHARole
grant select on dbo.VendorDealHistory to IMHARole
grant select on dbo.StoreItemVendor to IMHARole
grant select on dbo.Vendor to IMHARole
grant select,update on dbo.ItemVendor to IMHARole
grant select on dbo.Zone to IMHARole
grant select on dbo.NatItemFamily to IMHARole
grant select on dbo.NatItemCat to IMHARole
grant select on dbo.NatItemClass to IMHARole
grant select on dbo.PriceChgType to IMHARole
grant select on dbo.PriceBatchDetail to IMHARole
grant select on Sales_SumByItem to IMHARole
grant select on StoreRegionMapping to IMHARole
grant select on StoreSubTeam to IMHARole
grant select,insert on VendorItemStatuses to IMHARole

--functions
grant exec on dbo.fn_OnSale to IMHARole
grant select on dbo.fn_VendorCostAll to IMHARole
grant select on dbo.fn_VendorCost to IMHARole
go

---- IRMARSTRole ----
grant exec on dbo.Replenishment_POSPull_GetIdentifier to IRMARSTRole
grant exec on dbo.Replenishment_POSPush_GetFTPConfigForWriterType to IRMARSTRole
grant exec on dbo.UpdateSignQueuePrinted to IRMARSTRole
grant exec on dbo.InsertODBCError to IRMARSTRole
grant exec on dbo.GetStores to IRMARSTRole
go

---- IRMAAVCIRole ----

--stored procedures
grant exec on dbo.GetRuleDef to IRMAAVCIRole
grant exec on dbo.GetTeamBySubTeam to IRMAAVCIRole
grant exec on dbo.GetUserEmail to IRMAAVCIRole
grant exec on dbo.GetVendorcost to IRMAAVCIRole
grant exec on dbo.GetFreightMarkUps to IRMAAVCIRole
grant exec on dbo.GetExTypes to IRMAAVCIRole
grant exec on dbo.GetUrgentVCAI_Exceptions to IRMAAVCIRole
grant exec on dbo.DeleteOldVCAI_Exception to IRMAAVCIRole
grant exec on dbo.IdentifyItem to IRMAAVCIRole
grant exec on dbo.UpdateItemID to IRMAAVCIRole
grant exec on dbo.InsertVendorCostHistory3 to IRMAAVCIRole
grant exec on dbo.InsertVendorDealHistory to IRMAAVCIRole
grant exec on dbo.GetCurrentVendorStores to IRMAAVCIRole
grant exec on dbo.GetSubTeamName to IRMAAVCIRole
grant exec on dbo.InsertVendorCostHistoryException to IRMAAVCIRole
grant exec on dbo.InsertODBCError to IRMAAVCIRole


--tables
grant select on dbo.sysobjects to IRMAAVCIRole
grant select on dbo.RuleDef to IRMAAVCIRole
grant select on dbo.ExRule_AutoOrdersNoTitle to IRMAAVCIRole
grant select on dbo.ExRule_VendCostDIff to IRMAAVCIRole
grant select on dbo.ExRule_VendCostPackSize to IRMAAVCIRole
grant select on dbo.Subteam to IRMAAVCIRole
grant select on dbo.Team to IRMAAVCIRole
grant select on dbo.UserStoreTeamTitle to IRMAAVCIRole
grant select on dbo.StoreSubTeam to IRMAAVCIRole
grant select on dbo.Users to IRMAAVCIRole
grant select on dbo.VendorStoreFreight to IRMAAVCIRole
grant select on dbo.vendor to IRMAAVCIRole
grant select on dbo.ItemIdentifier to IRMAAVCIRole
grant select on dbo.Item to IRMAAVCIRole
grant select on dbo.ItemManager to IRMAAVCIRole
grant select on dbo.StoreItemVendor to IRMAAVCIRole

grant insert on dbo.VendorCostHistory to IRMAAVCIRole
grant insert on dbo.VendorDealHistory to IRMAAVCIRole

grant select, insert, delete on dbo.VendorCostHistoryExceptions to IRMAAVCIRole
grant select, update on dbo.ItemVendor to IRMAAVCIRole

--functions
grant select on dbo.fn_VendorCostStores to IRMAAVCIRole
grant select on dbo.fn_Parse_List to IRMAAVCIRole
go


---- ExtractRole ----

--tables
grant select on dbo.Item to ExtractRole
grant select on dbo.ItemUnit to ExtractRole
grant select on dbo.ItemBrand to ExtractRole
grant select on dbo.ItemVendor to ExtractRole
grant select on dbo.ItemIdentifier to ExtractRole
grant select on dbo.NatItemCat to ExtractRole
grant select on dbo.NatItemClass to ExtractRole
grant select on dbo.NatItemFamily to ExtractRole
grant select on dbo.Price to ExtractRole
grant select on dbo.PriceHistory to ExtractRole
grant select on dbo.Store to ExtractRole
grant select on dbo.StoreItem to ExtractRole
grant select on dbo.Sales_SumByItem to ExtractRole
grant select on dbo.StoreItemVendor to ExtractRole
grant select on dbo.SubTeam to ExtractRole
grant select on dbo.Vendor to ExtractRole
grant select on dbo.Zone to ExtractRole

--functions
grant exec on dbo.fn_GetCurrentCost to ExtractRole
grant exec on dbo.fn_isScaleItem to ExtractRole

grant select on dbo.fn_VendorCostAll to ExtractRole

-- stored procedures
grant exec on dbo.Reporting_Movement_BySubteam to ExtractRole
grant exec on dbo.InsertODBCError to ExtractRole

go

---- IRMA_Farm_Role ----
if exists (select 1 from sysusers where name = 'IRMA_Farm_Role')
	begin
	grant exec on dbo.GetFARMItemData to IRMA_Farm_Role
	grant exec on dbo.GetFARMStoreItemData to IRMA_Farm_Role
	end
go

---- IRMASupportRole ----
--tables

--functions

-- stored procedures
grant exec on dbo.Administration_GetPriceBatchHeaderSearch to IRMASupportRole
grant exec on dbo.Administration_GetPriceBatchHeaderStatus to IRMASupportRole
grant exec on dbo.UpdateOrderCurrency to IRMASupportRole

GO

------------ Database-level permissions ------------

grant create table to IRMASchedJobsRole
grant alter on schema :: dbo to IRMASchedJobsRole

exec sp_addrolemember 'db_datareader',   'IRMASupportRole'
exec sp_addrolemember 'db_datareader',   'IRMAAdminRole'
exec sp_addrolemember 'db_datareader',	 'WFM\zingonet'
exec sp_addrolemember 'db_datareader',	 'WFM\conns'
go


------------ Instance-level permissions ------------

--exec sp_addsrvrolemember 'SQLExcel', 'bulkadmin'
--exec sp_addsrvrolemember 'IRMAAdmin', 'bulkadmin'
--exec sp_addsrvrolemember 'IRMASchedJobs', 'bulkadmin'
--exec sp_addsrvrolemember 'IMHA_User', 'bulkadmin'
--exec sp_addsrvrolemember 'IRSUser', 'bulkadmin'
--go

grant exec on dbo.fn_GetCustomerType to IRMAAdminRole
go

--tables
grant select, update, insert, delete on dbo.ShelfTag to IRMAAdminRole
grant select, update, insert, delete on dbo.ShelfTag to IRMAClientRole

grant select, update, insert, delete on dbo.ShelfTagAttribute to IRMAAdminRole
grant select, update, insert, delete on dbo.ShelfTagAttribute to IRMAClientRole

grant select, update, insert, delete on dbo.ShelfTagRule to IRMAAdminRole
grant select, update, insert, delete on dbo.ShelfTagRule to IRMAClientRole

grant select, update, insert, delete on dbo.ShelfTagRuleType to IRMAAdminRole
grant select, update, insert, delete on dbo.ShelfTagRuleType to IRMAClientRole

grant select, update, insert, delete on dbo.StoreItemAttribute to IRMAAdminRole
grant select, update, insert, delete on dbo.StoreItemAttribute to IRMAClientRole


go

-- Public stored procedures and functions
grant execute on dbo.pivot_table to public
grant execute on dbo.pivot_query to public
grant execute on dbo.pivot_example to public
go

-- Data Migration Conditional grants.  
if exists (select 1 from sysusers where name = 'DataMigration')
	begin
	grant exec on dbo.CleanUpTablesForConversion  to DataMigration
	grant exec on dbo.UpdatePriceBatchDetailPromo to DataMigration
	grant exec on dbo.Conversion_RegionAddToItemTemp to DataMigration
	grant exec on dbo.Conversion_SoPacAddToItemTemp to DataMigration
	grant exec on dbo.Conversion_SaleEnd		  to DataMigration
	grant exec on dbo.Conversion_SaleEnd to DataMigration

	grant execute on dbo.fn_conv_getunitid to datamigration
	grant execute on dbo.fn_conv_isonpromotion to datamigration
	grant execute on dbo.fn_conv_isy to datamigration
	grant execute on dbo.fn_conv_upctoid to datamigration

	grant execute on dbo.Conversion_AddItemsFromPrep to datamigration
	grant execute on fn_IsScaleIdentifier to DataMigration
	grant execute on fn_CalcCheckDigit to DataMigration
	grant execute on fn_OnSale to DataMigration
	grant execute on fn_Conv_ForcePackWeight to DataMigration

	grant exec on dbo.fn_IsScaleItem to DataMigration
	grant exec on dbo.fn_OnSale to DataMigration

	end

GO

--*********************
--  Stored Procedures
--*********************

grant exec on dbo.GetConfigurationData to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetConfigurationValue to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateConfigurationData to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on dbo.UpdateItemRestore to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on EInvoicing_CreateEInvoiceRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_GetPOSSubTeams to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_GetSalePriceChgType to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_GetSubStores to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_GetZones to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_InsertZone to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UpdateZone to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_ArchiveEInvoice to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_getPOsByPSVendor to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_UpdAllocLineItemCharge to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_UpdateInvoicePOInformation to IRMAAdminRole, IRMAClientRole
grant exec on dbo.OrderInvoice_DelOrderInvoiceSpecCharge to IRMAClientRole
grant exec on dbo.OrderInvoice_GetGLAcctSubteams to IRMAClientRole
grant exec on dbo.OrderInvoice_GetOrderInvoiceSACTotal to IRMAClientRole
grant exec on dbo.OrderInvoice_GetOrderInvoiceSpecCharges to IRMAClientRole
grant exec on dbo.OrderInvoice_InsOrderInvoiceSpecCharge to IRMAClientRole
grant exec on dbo.Acct4YrComp to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.AcctDeptComp to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ActiveItemList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.AddTitle to IRMAClientRole, IRMAAdminRole
grant exec on dbo.Administration_DeleteZone to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_GetPriceBatchHeaderSearch to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.Administration_GetPriceBatchHeaderStatus to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.Administration_GetRegionalStore to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_GetSourceStores to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_MenuAccess_GetMenuAccessRecords to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_MenuAccess_GetMenuItemToEdit to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_MenuAccess_GetMenuNamesList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_MenuAccess_UpdateMenuAccessRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_MenuAccess_DeleteMenuAccessRecord to IRMAClientRole
grant exec on dbo.Administration_MenuAccess_AddMenuAccessRecord to IRMAClientRole
grant exec on dbo.Administration_POSPush_DeletePOSWriter to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_DeletePOSWriterFileConfig to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_DeletePOSWriterFileConfigRow to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_DeleteStoreFTPConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_DeleteStorePOSConfig to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_GetAvailableFileWriterTypeForStore to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_GetDefaultBatchIdByChangeType to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_GetDefaultBatchIdByItemChgType to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_GetDefaultBatchIdByPriceChgType to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_GetFileWriterClass to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_GetFileWriterTypes to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_GetPOSChangeTypes to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Administration_POSPush_GetPOSDataElement to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Administration_POSPush_GetPOSWriterDictionary to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Administration_POSPush_GetPOSWriterEscapeChars to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Administration_POSPush_GetPOSWriterFileConfigRowCount to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Administration_POSPush_GetPOSWriterFileConfigurationForEdit to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Administration_POSPush_GetPOSWriters to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Administration_POSPush_GetScaleWriterTypes to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_GetStorePOSWriterConfigurations to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_GetStoresAvailableForAdd to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Administration_POSPush_GetStoresByWriter to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_GetStoreScaleWriterConfigurations to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_GetTaxFlagKeys to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Administration_POSPush_InsertPOSWriter to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_InsertPOSWriterFileConfig to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_InsertStoreFTPConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_InsertStorePOSConfig to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_PopulatePIRUSConfigData to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_UpdateDefaultBatchIdByChangeType to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_UpdateDefaultBatchIdByItemChgType to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_UpdateDefaultBatchIdByPriceChgType to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_UpdatePOSWriter to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_UpdatePOSWriterFileConfig to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_UpdatePOSWriterFileConfigOrder to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_UpdateStoreFTPConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_UpdateStoreFTPPassword to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_POSPush_UpdateStorePOSConfig to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Administration_POSPush_UpdateStoreScaleConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_Scale_DeleteStoreScaleConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_Scale_InsertStoreScaleConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_Scale_UpdateStoreScaleWriterConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_TagPush_DeleteStoreShelfTagConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_TagPush_GetStoreShelfTagWriterConfigurations to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_TagPush_InsertStoreShelfTagConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_TagPush_DeleteStoreElectronicShelfTagConfig to IRMAClientRole, IRMAAdminRole
grant exec on dbo.Administration_TagPush_InsertStoreElectronicShelfTagConfig to IRMAClientRole, IRMAAdminRole
grant exec on dbo.Administration_TagPush_UpdateStoreElectronicShelfTagWriterConfig to IRMAClientRole, IRMAAdminRole
grant exec on dbo.Administration_TagPush_GetStoreElectronicShelfTagWriterConfigurations to IRMAClientRole, IRMAAdminRole
grant exec on dbo.Replenishment_TagPush_GetElectronicShelfTagBatchFile to IRMAClientRole, IRMAAdminRole
grant exec on dbo.Replenishment_TagPush_GetFullElectronicShelfTagFile to IRMAClientRole, IRMAAdminRole
grant exec on dbo.Administration_TagPush_UpdateStoreShelfTagWriterConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UpdateInstanceDataFlags to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UpdateStore to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UserAdmin_DeleteUser to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UserAdmin_GetAllUsers to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UserAdmin_GetUserList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UserAdmin_InsertUser to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UserAdmin_UpdateUser to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UserStoreTeamTitle to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Administration_UserSubteam to IRMAAdminRole, IRMAClientRole
grant exec on dbo.BuildStorePosFileForR10 to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AllocationReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.AppConfig_AddApp to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_AddEnv to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_AddKey to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_AddKeyValue to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_AppList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_EnvList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_GetConfigDoc to IRMAAdminRole, IRMAClientRole, IRMASchedJobs, IRMAReportsRole
grant exec on dbo.AppConfig_GetConfigKeyList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_GetConfigList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_ImportKey to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_KeyList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_RemoveApp to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_RemoveEnv to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_RemoveKey to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_SaveBuildConfig to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_TypeList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.AppConfig_UpdateKeyValue to IRMAAdminRole, IRMAClientRole
grant exec on dbo.ApplyPMPriceChange to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.APUpClosedReport to IRMAClientRole, IRMAReportsRole
grant exec on dbo.AutoCloseReceiving to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.AutomaticOrderItemInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.AutomaticOrderList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.AutomaticOrderOriginUpdate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.AutoOrderInfoReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.AutoSendDistOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.AvgHourlySales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.BERTReport to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.BRC_GetFacilities to IRMAClientRole
grant exec on dbo.BRC_GetOrders to IRMAClientRole
grant exec on dbo.BRC_ReceiveOrder to IRMAClientRole
grant exec on dbo.BuggyCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.BuggySalesByItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.BuggySalesGetItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.BuggySalesSelectItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.BuyerVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CancelAllSales to IRMAClientRole
grant exec on dbo.CasesBySubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CasesBySubTeamAudit to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CharcuterieInventoryItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckBatchedPriceChange to IRMAClientRole
grant exec on dbo.CheckCostChanges to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateBrands to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateCardNumbers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateCategories to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateContacts to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateConversions to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateCustomers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateCycleCountMaster to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateIdentifier to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateIdentifier to IRMAExcelRole
grant exec on dbo.CheckForDuplicateIdentifier to IRMASLIMRole
grant exec on dbo.CheckForDuplicateInvLocation to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateInvLocationItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateItemQuantity to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateItemSign to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateItemVendors to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateManifest to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateOrganizations to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateOrigins to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateProdHierarchyLevel3 to IRMAClientRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateProdHierarchyLevel4 to IRMAClientRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateShelfLives to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateUnits to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForDuplicateVendors to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForExternalCycleCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForItemChangeDifferences to IRMAClientRole
grant exec on dbo.CheckForOpenCycleCountMaster to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForOpenCycleCounts to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckForReturnOrderChanges to IRMAClientRole
grant exec on dbo.CheckIdentifierInItemVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckIdentifierInItemVendor to IRMAExcelRole
grant exec on dbo.CheckIfPrimVendCanSwap to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckIfVendorIsPrimaryForAnyItems to IRMAClientRole, IRMASupportRole
grant exec on dbo.CheckIfWarehouseItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckInventoryAdjustmentAbbreviation to IRMAClientRole
grant exec on dbo.CheckItemAuthLock to IRMAClientRole
grant exec on dbo.CheckItemAuthLock to IRMASLIMRole
grant exec on dbo.CheckItemInItemVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckOrderReceived to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckPriceExist to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckReceiveLog to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckREGPriceDifference to IRMAClientRole
grant exec on dbo.CheckTransferDistributions to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckVendorCostHistoryOverlap to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CheckVendorIdAndVendorKey to IRMAClientRole
grant exec on dbo.CheeseInventoryItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CloseCycleCountHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ClosedOrdersReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CloseReceiving to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CommitAllGLUploads to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.CommitGLUploadDistributions to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.CommitGLUploadInventoryAdjustment to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.CommitGLUploadTransfers to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.CommodityCodeMovement to IRMAReportsRole
grant exec on dbo.CommodityCodeMovementDatesCompare to IRMAReportsRole
grant exec on dbo.Configuration_Getvalue to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Configuration_Setvalue to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ControlGroup3WayMatchLogReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Convertion_AddItemsFromTemp to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CopyExistingPO to IRMAClientRole
grant exec on dbo.CostChangeEventReport to IRMAReportsRole
grant exec on dbo.CostConversion to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CostExceptionReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CountOpenImportCounts to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CountOrderItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CountReceivedOrderItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CountUnrankedSustainabilityRequiredOrderItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.CreditReasonReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DailyItemAvgCostChgRpt to IRMAClientRole, IRMAReportsRole
grant exec on dbo.DailyReceiving to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DailySales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DailySalesComp to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DailyTax to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DailyTaxSales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DaySales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DCStoreRetailPriceReport to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DeleteBrand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteCategory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteCompetitorImportInfo to IRMAClientRole
grant exec on dbo.DeleteCompetitorImportSession to IRMAClientRole
grant exec on dbo.DeleteCompetitorPrice to IRMAClientRole
grant exec on dbo.DeleteContact to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteContacts to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteConversion to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteCurrency to IRMAAdminRole, IRMAClientRole
grant exec on dbo.DeleteCustomerReturn to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteCustReturnItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteCycleCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteCycleCountHistoryItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteCycleCountItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteCycleCountMaster to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeletedOrderReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteFSCustomer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteFSCustomers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteFSOrganization to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteInstanceDataFlagsStoreOverride to IRMAAdminRole, IRMAClientRole
grant exec on dbo.DeleteInventoryLocationItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteInventoryLocations to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteInventoryServiceImportLoad to IRMAClientRole, IRMASchedJobsRole, IRMASchedJobsRole
grant exec on dbo.DeleteItemChain to IRMAClientRole
grant exec on dbo.DeleteItemDefaultValue to IRMAClientRole
grant exec on dbo.DeleteItemIdentifier to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteItemInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteItemSign to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteItemUPCInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteItemUpload to IRMAClientRole
grant exec on dbo.DeleteItemVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteItemVideo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteOldVCAI_Exception to IRMAAVCIRole
grant exec on dbo.DeleteOldVCAI_Exception to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteOrderHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteOrderHeaderOrderItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteOrderInvoice to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteOrderItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteOrderItemQueue to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteOrderReceiving to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteOrderWindowEntry to IRMAClientRole
grant exec on dbo.DeleteOrigin to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeletePLUMCorpChgQueueTmp to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeletePriceBatch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeletePriceBatchCutDetail to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeletePriceBatchDetail to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeletePricingMethodMapping to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteReceiving to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteReturnOrderUserRecords to IRMAClientRole
grant exec on dbo.DeleteShelfLife to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteStoreCompetitorStore to IRMAClientRole
grant exec on dbo.DeleteStoreItemVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteStoreItemVendor to IRMASLIMROLE
grant exec on dbo.DeleteTitle to IRMAClientRole, IRMAAdminRole
grant exec on dbo.DeleteUnit to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteVendorCostHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteVendorDealHistory to IRMAClientRole
grant exec on dbo.DeleteVendorItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteWarehouseItemChange to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteWarehouseVendorChange to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeleteZoneSubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DepartmentSalesAnalysis to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DeptComp to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DeptKeyUsage to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.DiscontinuedItemsWithInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DistInvFrght to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DVO_ImportOrderDetail to IRMASchedJobsRole;
grant exec on dbo.DVO_ImportOrderHeader to IRMASchedJobsRole;
grant exec on dbo.DVO_ProcessBulkOrders to IRMASchedJobsRole;
grant exec on dbo.DoFSAWarehouseSend to IRMAClientRole
grant exec on dbo.DoFSASubstitution to IRMAClientRole
grant exec on dbo.DoFSAAutoAllocate to IRMAClientRole
grant exec on dbo.UpdateFSAStoreOnOrder to IRMAClientRole
grant exec on dbo.Dynamic_POSSearchForNonBatchedChanges to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.DynamicMovement to IRMAReportsRole
grant exec on dbo.EIM_AddChainNamesToIdList to IRMAClientRole
grant exec on dbo.EIM_CascadeDeleteUploadSession to IRMAClientRole
grant exec on dbo.EIM_DeleteItemValidation to IRMAClientRole
grant exec on dbo.EIM_GetAllUploadAttributes to IRMAClientRole
grant exec on dbo.EIM_GetAllUploadTypeAttributes to IRMAClientRole
grant exec on dbo.EIM_GetAllUploadValues to IRMAClientRole
grant exec on dbo.EIM_GetChainIdsFromNameList to IRMAClientRole
grant exec on dbo.EIM_GetUploadAttributeByPK to IRMAClientRole
grant exec on dbo.EIM_GetUploadTypeAttributeByPK to IRMAClientRole
grant exec on dbo.EIM_GetUploadTypeAttributesByUploadAttributeID to IRMAClientRole
grant exec on dbo.EIM_GetUploadTypeAttributesByUploadTypeCode to IRMAClientRole
grant exec on dbo.EIM_GetUploadValueByPK to IRMAClientRole
grant exec on dbo.EIM_GetUploadValuesByUploadAttributeID to IRMAClientRole
grant exec on dbo.EIM_GetUploadValuesByUploadRowID to IRMAClientRole
grant exec on dbo.EIM_HasNoPrimaryVendor to IRMAClientRole
grant exec on dbo.EIM_ItemLoadSearch to IRMAClientRole
grant exec on dbo.EIM_JurisdictionValidation to IRMAClientRole
grant exec on dbo.EIM_LookUpHierarchyIds to IRMAClientRole
grant exec on dbo.EIM_OptimizedInsertUploadRow to IRMAClientRole
grant exec on dbo.EIM_OptimizedUpdateUploadRow to IRMAClientRole
grant exec on dbo.EIM_PriceChangeValidation to IRMAClientRole, IRMASupportRole
grant exec on dbo.EIM_Regen_DeletePriceChgType to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadRow to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadSession to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadSessionUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadSessionUploadTypeStore to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadTypeAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadTypeTemplate to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadTypeTemplateAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_DeleteUploadValue to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllPriceChgTypes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadAttributes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadRows to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadSessions to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadSessionUploadTypes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadSessionUploadTypeStores to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadTypeAttributes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadTypes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadTypeTemplateAttributes to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadTypeTemplates to IRMAClientRole
grant exec on dbo.EIM_Regen_GetAllUploadValues to IRMAClientRole
grant exec on dbo.EIM_Regen_GetPriceChgTypeByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadAttributeByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadRowByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadRowsByItemKey to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadRowsByUploadSessionID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypeByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypesByUploadSessionID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypesByUploadTypeCODE to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypeStoreByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypeStoresByStoreNo to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeAttributeByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeAttributesByUploadAttributeID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeAttributesByUploadTypeCode to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplateAttributeByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplateAttributesByUploadTypeAttributeID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplateAttributesByUploadTypeTemplateID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplateByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadTypeTemplatesByUploadTypeCode to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadValueByPK to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadValuesByUploadAttributeID to IRMAClientRole
grant exec on dbo.EIM_Regen_GetUploadValuesByUploadRowID to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertPriceChgType to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadRow to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadSession to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadSessionUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadSessionUploadTypeStore to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadTypeAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadTypeTemplate to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadTypeTemplateAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_InsertUploadValue to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdatePriceChgType to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadRow to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadSession to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadSessionUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadSessionUploadTypeStore to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadType to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadTypeAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadTypeTemplate to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadTypeTemplateAttribute to IRMAClientRole
grant exec on dbo.EIM_Regen_UpdateUploadValue to IRMAClientRole
grant exec on dbo.EIM_SearchUploadSessions to IRMAClientRole
grant exec on dbo.EIM_UploadSession to IRMAClientRole
grant exec on dbo.EIM_UploadSession_DeleteItems to IRMAClientRole
grant exec on dbo.EIM_ValidateCostedByWeightUnit to IRMAClientRole
grant exec on dbo.EIM_ValidateVendorUOM to IRMAClientRole
grant exec on dbo.EInvoicing_GetEInvoiceDisplay_HeaderInfo to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole, IRMASupportRole
grant exec on dbo.EInvoicing_GetEInvoiceDisplay_Items to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole, IRMASupportRole
grant exec on dbo.EInvoicing_GetEInvoiceDisplay_ItemCharges to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole, IRMASupportRole
grant exec on dbo.EInvoicing_GetEInvoiceDisplay_SummaryCharges to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole, IRMASupportRole
grant exec on dbo.Einvoicing_GetEInvoiceDisplay_InvoiceMessage  to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole, IRMASupportRole
grant exec on dbo.EInvoicing_getErrorHistory to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_GetKnownElements to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_GetKnownHeaderElements to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_GetKnownItemElements to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_GetKnownSACCodes to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_getSACTypes to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_GetOrderHeaderIDForDSDOrder to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertConfigValue to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertErrorHistory to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertHeaderElement to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertInvoiceHeaderRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertLineItemElement to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertLineItemRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertSummaryElement to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_MatchEInvoiceToPurchaseOrder to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_RemoveConfigValue to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_SetInvoiceStatus to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_ValidateDataElements to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EndSaleEarly to IRMAClientRole, IRMASLIMRole
grant exec on dbo.EPromotions_AddItemToGroup to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_AddStoreToPromotion to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_CreateNewItemGroup to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_CreatePriceBatchDetail to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_DeleteItemFromGroup to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_DeletePromotionalOffer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_DeletePromotionalOfferMembers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_DeleteUnbatchedPriceBatchDetails to IRMAClientRole
grant exec on dbo.EPromotions_GetAvailableItemGroups to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetGroupEditStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetGroupItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetItemGroups to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetPricingMethodInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetOfferIdFromPriceBatchDetailId to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetOffersByGroupID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetPriceBatchDetailCountByOfferID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetPriceBatchDetailsByOfferItem to IRMAClientRole
grant exec on dbo.EPromotions_GetPricingMethods to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetPromotionalOfferMembers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetPromotionalOffers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetPromotionalOffersByPricingMethod to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetPromotionPriceBatchDetail to IRMAClientRole
grant exec on dbo.EPromotions_GetRewardTypes to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetStoresByPricingMethod to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_GetStoresByPromotionId to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_InsertGroupData to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_InsertPromotionalOffer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_InsertPromotionalOfferMember to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_IsGroupInCurrentPromotion to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_PromotionalOffer_SetEditFlag to IRMAClientRole
grant exec on dbo.EPromotions_PromotionExistence to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_RemoveItemGroup to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_RemoveStoreFromPromotion to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_ReturnPendingPriceBatchDetailCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_ReturnStoreActiveFlag to IRMAClientRole
grant exec on dbo.EPromotions_ReturnUnbatchedPriceBatchDetailCount to IRMAClientRole
grant exec on dbo.EPromotions_SetGroupEditStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_ShowLockedGroups to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_ShowLockedPromotions to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_UnlockGroup to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_UpdateGroupData to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_UpdatePromotionalOffer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_UpdatePromotionalOfferMember to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_ValidateGroupName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.EPromotions_ValidatePromotionName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.ESSBaseQuery to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ExReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
GRANT EXEC ON FacilityAvgCostListWeightedItems TO IRMAClientRole, IRMAReportsRole, IrmaReportsRole
grant exec on dbo.FacilityOrderReport to IRMAClientRole, IRMAReportsRole
grant exec on dbo.FillRate4WeekDetailReport to IRMAClientRole, IRMAReportsRole
grant exec on dbo.FillRate7DayDetailReport to IRMAClientRole, IRMAReportsRole
grant exec on dbo.FillRateFPtoDateDetailReport to IRMAClientRole, IRMAReportsRole
grant exec on dbo.FillRateSummaryReport to IRMAClientRole, IRMAReportsRole
grant exec on dbo.FindOpenOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.FiscalCompSales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.FiscalMonthAllStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.FiscalMonthAllStoresLastYear to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.FiscalMonthSales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAdjustmentInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAdjustmentInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAdminSubTeamList to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on dbo.GetAdPlanAuditReport to IRMAReportsRole
grant exec on dbo.GetAllClosedControlGroups to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetAllControlGroups to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetAllControlGroupStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetAllDistributionCenters to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAllFacilities to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetAllFacilities to IRMAExcelRole
grant exec on dbo.GetAllFacilities to IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetAllFacilities to IRMASLIMRole
grant exec on dbo.GetAllGLUpload to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.GetAllItemAuthorization to IRMAReportsRole
grant exec on dbo.GetAllItemOverrides to IRMAClientRole
grant exec on dbo.GetAllOnHand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetAllOnHandDetail to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetAllPriceTypes to IRMAClientRole
grant exec on dbo.GetAllProdHierarchyLevel3 to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetAllProdHierarchyLevel4 to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetAllStores to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAllStores_ByStoreName to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetAllStoreUsers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAllSubTeams to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAllSubTeams to IRMASLIMRole
grant exec on dbo.GetANSOrderHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetANSOrderItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetApps to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAPUpAccruals to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAPUpAppDailySum to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAPUpExceptions to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAPUploads to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.GetAPUpNoInvoice to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAPUpNoPSInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAPUpNotApproved to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAPUpUploaded to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAvailItemVendorStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAvailPrimVend to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAvailPrimVendDetail to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAvgCost to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAvgCostAdjustmentReasons to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on dbo.GetAvgCostAllFacilities to IRMAClientRole, IRMAReportsRole
grant exec on dbo.GetAvgCostDist to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetAvgCostHistory to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on dbo.GetAvgCostForStores to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on dbo.GetAvgCostVariance to IRMAReportsRole
grant exec on dbo.GetAvgCostVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBackOrderItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBatchesInSentState to IRMAClientRole
grant exec on dbo.GetBeginFiscalYearDate to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetBeginFiscalYearDateRS to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetBeginPeriodDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBeginPeriodDateRS to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBeginQuarterDate to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetBeginQuarterDateRS to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetBrand_ByNameExact to IRMASLIMRole
grant exec on dbo.GetBrandAndID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBrandAndID to IRMAExcelRole
grant exec on dbo.GetBrandAndID to IRMASLIMRole
grant exec on dbo.GetBrandInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBrandInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBrandInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBrandLockStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBrandName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetBrands_ByNameStartsWith to IRMAClientRole
grant exec on dbo.GetBrands_ByNameStartsWith to IRMASLIMRole
grant exec on dbo.GetCategoriesAndSubTeams to IRMAClientRole
grant exec on dbo.GetCategoriesAndSubTeams to IRMAExcelRole
grant exec on dbo.GetCategoriesByMultiSubTeam to IRMAReportsRole
grant exec on dbo.GetCategoriesBySubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCategoriesBySubTeam to IRMAExcelRole
grant exec on dbo.GetCategoriesBySubTeam to IRMASLIMRole
grant exec on dbo.GetCategory_ByNameStartsWith to IRMAClientRole
grant exec on dbo.GetCategoryAndID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCategoryAndID to IRMAExcelRole
grant exec on dbo.GetCategoryInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCategoryInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCategoryInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCategoryLockStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCategoryName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCompetitivePriceTypes to IRMAClientRole
grant exec on dbo.GetCompetitorImportInfoExistingCount to IRMAClientRole
grant exec on dbo.GetCompetitorImportSessionByCompetitorImportSessionID to IRMAClientRole
grant exec on dbo.GetCompetitorImportSessionByUser_ID to IRMAClientRole
grant exec on dbo.GetCompetitorLocations to IRMAClientRole
grant exec on dbo.GetCompetitorPriceSearch to IRMAClientRole
grant exec on dbo.GetCompetitors to IRMAClientRole
grant exec on dbo.GetCompetitorStoreByName to IRMAClientRole
grant exec on dbo.GetCompetitorStoreSearch to IRMAClientRole
grant exec on dbo.GetContactInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetContactInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetContactInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetConversionInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetConversionInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCostAdjustmentReasons to IRMAClientRole
grant exec on dbo.GetCostPromoCodeTypes to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetCreditOrderList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCreditReasons to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCurrencies to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetCurrentOnHand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCurrentPrices to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCurrentProcessedSaleBatches to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetCurrentVendorStores to IRMAAVCIRole
grant exec on dbo.GetCurrentVendorStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCustomer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCustomerInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCustomerReturn to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCustomerReturnHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCustomerReturnItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCustomers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCustomerSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCustReturnReasons to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCustReturns to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCycleCountHistoryItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCycleCountHistoryList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCycleCountItemList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCycleCountList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCycleCountMaster to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetCycleCountMasterList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetDateRangeForCurrentWeek to IRMAClientRole, IRMAAdminRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetDaysInFiscalMonth to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetDefaultPOSBatchId to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetDefaultPOSBatchIdRangeByStore to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetDefaultPOSWriterBatchId to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.GetDistAndMfg to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetDistManZones to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetDistOrderWindowsClosed to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetDistributionCreditOrderList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetDistributionHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetDistributionMarkup to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetDistSubTeams to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetDistSubTeams to IRMAExcelRole
grant exec on dbo.GetElectronicOrderHeaderInfo to IRMAClientRole
grant exec on dbo.GetElectronicOrderItemInfo to IRMAClientRole
grant exec on dbo.GetEndPeriodDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetESRSBatchDetail to IRMAClientRole
grant exec on dbo.GetESRSBatchList to IRMAClientRole
grant exec on dbo.GetESRSItemList to IRMAClientRole
grant exec on dbo.GetESRSItemsByDateRange to IRMAClientRole
grant exec on dbo.GetESRSItemSearch to IRMAClientRole
grant exec on dbo.GetESRSPriceCheck to IRMAClientRole
grant exec on dbo.GetESRSSubTeams to IRMAClientRole
grant exec on dbo.GetEXEDistSubTeams to IRMAClientRole
grant exec on dbo.GetExSeverity to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetExTypes to IRMAAVCIRole
grant exec on dbo.GetExTypes to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFaxOrderItemList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFirstItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFirstVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFiscalWeeks to IRMAClientRole
grant exec on dbo.GetFQSOrganizationInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFQSOrganizationInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFreightMarkUps to IRMAAVCIRole
grant exec on dbo.GetFreightMarkUps to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFreightUploads to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.GetFSCustomerInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFSCustomerInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFSCustomerInformation to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFSCustomerLinks to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFSOrganizationInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFSOrganizationInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFSOrganizationInformation to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetFSOrganizationLinks to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetGLQueue to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetGLUploadDistributions to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetGLUploadInventoryAdjustment to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetGLUploadTransfers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetGLUploadTransfersByGroup to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetHandPrinterLabels to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetIdentifierTypes to IRMAClientRole
grant exec on dbo.GetIdentifierTypes to IRMASLIMRole
grant exec on dbo.GetInstanceData to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.GetInstanceData to IRMAExcelRole
grant exec on dbo.GetInstanceData to IRMASLIMRole
grant exec on dbo.GetInstanceDataAvailableStoreOverrides to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInstanceDataFlags to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.GetInstanceDataFlags to IRMAExcelRole
grant exec on dbo.GetInstanceDataFlagsStoreOverrideList to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInstanceDataFlagValue to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInstanceDataFlagValue to IRMASLIMRole
grant exec on dbo.GetInternalCustomers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInventoryAdjustmentAllows to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInventoryAdjustmentCode to IRMAClientRole
grant exec on dbo.GetInventoryAdjustmentCodeList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInventoryAdjustmentIDFromCode to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInventoryLocation to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInventoryLocationItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInventoryLocations to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInventoryLocationsByStore to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInventoryServiceExport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInventoryStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetInvoiceNumberUse to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetIsBatched to IRMASupportRole
grant exec on dbo.GetIsBatchedByStatus to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetIsBatchedByStatusForStore to IRMAClientRole
grant exec on dbo.GetIsItemOnSaleOrSalePendingForStore to IRMAClientRole
grant exec on dbo.GetIsPayAgreedCostVendorAllStores to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetDSDVendorAllStores to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.UpdateDSDVendorStore to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItem_LabelSummary to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetItemAdminUsers to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetItemAttributeText_1 to IRMAReportsRole
grant exec on dbo.GetItemAuthorization to IRMASLIMRole
grant exec on dbo.GetItemByVIN_VendorID to IRMAClientRole
grant exec on dbo.GetItemChain_ByItemChainID to IRMAClientRole
grant exec on dbo.GetItemChainItems_ByItemChainID to IRMAClientRole
grant exec on dbo.GetItemChains to IRMAClientRole
grant exec on dbo.GetItemChains_ByDescriptionStartsWith to IRMAClientRole
grant exec on dbo.GetItemChains_ByItemKey to IRMAClientRole
grant exec on dbo.GetItemChangeInfo to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetItemConversion to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemConversionAll to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemData to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemDataInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemDefaultAttributes to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetItemDefaultValues to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetItemDefaultValuesByItem to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetItemHierarchyByIdentifier to IRMAClientRole
grant exec on dbo.GetItemHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemIdentifiers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemIDInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemUnitInfoStore to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemInfoByIdentifier to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemInfoByIdentifier to IRMAExcelRole
grant exec on dbo.GetItemLockStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemManagers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetItemManagers to IRMAExcelRole
grant exec on dbo.GetItemMultipleUPCs to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemOrder to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemOverride to IRMAClientRole
grant exec on dbo.GetItemOverride to IRMASLIMRole
grant exec on dbo.GetItemPendPrice to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemPendPrice to IRMASLIMRole
grant exec on dbo.GetItemPrimVend to IRMASLIMRole
grant exec on dbo.GetItemSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemsInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemStoreVendorsCost to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemTypes to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetItemUnitID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemUnitInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemUnits to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemUnitsCost to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetItemUnitsCost to IRMAExcelRole
grant exec on dbo.GetItemUnitsCost to IRMASLIMRole
grant exec on dbo.GetItemUnitsPDU to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetItemUnitsPDU to IRMAExcelRole
grant exec on dbo.GetItemUnitsVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemUnitsVendor to IRMAExcelRole
grant exec on dbo.GetItemUploadDetails to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetItemUploadSearch to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetItemUploadTypes to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetItemVendors to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemVendorStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemVideo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemVideoLastID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemVideoList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetItemWebQuery to IRMASLIMRole
grant exec on dbo.GetItemWebQueryStore to IRMASLIMRole
grant exec on dbo.GetJobStatus to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.GetJobStatusList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetKitchenRoutes to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetLabelTypes to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetLabelTypes to IRMAExcelRole
grant exec on dbo.GetLineDrivePreUpdate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetMarginInfo to IRMAClientRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetNatClass to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetNatClass to IRMAExcelRole
grant exec on dbo.GetOpenDistributionOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderAllocItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderAllocOrderItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderCurrency to IRMAClientRole, IRMAReportsRole
grant exec on dbo.GetOrderEmail to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderHeaderDesc to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderHeaderLockStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderID_for_ExternalSourceOrderID to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderInvoice to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemComments to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemItemInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemLines to IRMAClientRole, IRMAReportsRole
grant exec on dbo.GetOrderItemList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemListReport TO IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemQueueCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemQueueSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemQueueView to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemReceivedList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemsCostData to IRMAClientRole
grant exec on dbo.GetOrderItemSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetTransferOrderItemSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderItemSumQty to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetOrderItemSumQty to IRMAExcelRole
grant exec on dbo.GetOrderItemSumQty to IRMAPromoRole
grant exec on dbo.GetOrderItemSumQty to IRMASLIMRole
grant exec on dbo.GetOrderListUnitID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderOriginUpdates to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetOrderReceivingDisplayInfo to IRMAClientRole
grant exec on dbo.GetOrderSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderSendInfo to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetOrdersNoSubstitutionItem to IRMAClientRole, IRMAAdminRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetOrderStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOrderVendorConfig to IRMAClientRole
grant exec on dbo.GetOrderWindow to IRMAClientRole
grant exec on dbo.GetOrganizationInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOriginAndID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOriginInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOriginInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOriginInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetOriginLockStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPeriodDates to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPLUMCorpChg to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPLUMDeptMap to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPLUMInterface to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPLUMStoreMap to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPMSalesHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPMSalesHistoryLoad to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPOCostDifAuto to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPOHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPOInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPosChangesAggregated to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPosChangesGL_Pushed to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPosChangesInQueue to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPOSStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPOSSystemTypes to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetPOUsersSubteam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchDetail to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchDetailDetailReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchDetailIDs to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetPriceBatchDetailSumReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchHeader_LabelSummary to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchHeaderDetailReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchHeaderReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchItemSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchPrintItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchSign to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceBatchStatusList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceSearch to IRMAClientRole, IRMAReportsRole
grant exec on dbo.GetPriceTypes to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPriceTypes to IRMAExcelRole
grant exec on dbo.GetPricingMethodMappings to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetPrimVendItmCnt to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetProdHierarchyLevel3s_ByDescriptionStartsWith to IRMAClientRole
grant exec on dbo.GetProdHierarchyLevel3sByCategory to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetProdHierarchyLevel3sByCategory to IRMASLIMRole
grant exec on dbo.GetProdHierarchyLevel4s_ByDescriptionStartsWith to IRMAClientRole
grant exec on dbo.GetProdHierarchyLevel4sByLevel3 to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetPromo to IRMAPromoRole
grant exec on dbo.GetPromoOrders to IRMAPromoRole
grant exec on dbo.GetPromoPreOrder to IRMAPromoRole
grant exec on dbo.GetProposedNetCost_CostChange to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetProposedNetCostValues to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetPSVendors to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPurchaseOrderHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetPurchaseOrderReportDetails to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRandomWeightScalePLUs to IRMAReportsRole
grant exec on dbo.GetReceiveLogDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetReceivingCheckList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetReceivingList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetReceivingLog to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetReceivingStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRecvLogSumApprv to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRecvLogSumExcp to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRegionCustomers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRegionCustomersAsDC to IRMAClientRole
grant exec on dbo.GetRegionList to IRMAAdminRole, IRMASchedJobsRole, IRMAClientRole
grant exec on dbo.GetRegions to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRegionStates to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRegPriceExists to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetRegularPriceChgTypeData to IRMAClientRole
grant exec on dbo.GetRetailStoreCommInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRetailStores to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRetailStoresAndTaxRates to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRetailStoresPOSPriceTax to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetRetailSubteam to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRetailSubteamsByTeamNo to IRMAAdminRole
grant exec on dbo.GetRetailTeams to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetReturnOrderChanges to IRMAClientRole
grant exec on dbo.GetReturnOrderList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetReturnOrderRecords to IRMAClientRole
grant exec on dbo.GetRipeCustomerByRipeZoneLocation to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRipeCustomerByRipeZoneLocationDistDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRipeCustomerStoreNo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRipeLocations to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRipeLocationStoreNo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRipeZones to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetRipeZones to IRMAExcelRole
grant exec on dbo.GetRuleDef to IRMAAVCIRole
grant exec on dbo.GetRuleDef to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetScalePLUConflicts to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetScanGunStoreSubTeam to IRMAClientRole
grant exec on dbo.GetScheduledJobClasses to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.GetScheduledJobErrors to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.GetShelfLifeAndID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetShelfLifeInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetShelfLifeInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetShelfLifeInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetShelfLifeLockStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSignName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSignQueue to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSignType to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSpecificUnitAndID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStatesWithStores to IRMAClientRole
grant exec on dbo.GetStore to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreCompetitorStoresByStore_No to IRMAClientRole
grant exec on dbo.GetStoreCustomer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreFromOrder to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreIsDistribution to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreItemAuths to IRMASLIMRole
grant exec on dbo.GetStoreItemCycleCountInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreItemOrderInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreItemSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreItemVendors to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreItemVendorStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreJurisdictions to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetStoreMobilePrinter to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreOnHand to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetStoreOnHand to IRMAExcelRole
grant exec on dbo.GetStoreOnHandDetail to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetStoreOnHandDetail to IRMAExcelRole
grant exec on dbo.GetStores to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStores to IRMAExcelRole
grant exec on dbo.GetStores to IRMARSTRole
grant exec on dbo.GetStores to IRMASLIMRole
grant exec on dbo.GetStoresAndDist to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoresAndDistAdjustments to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoresByState to IRMAClientRole
grant exec on dbo.GetStoresByUser to IRMASLIMRole
grant exec on dbo.GetStoresByZoneName to IRMAClientRole
grant exec on dbo.GetStoreSubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreSubTeamMinusSupplier to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoresWithNoVendorForItem to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetStoresWithPrimaryVendorThatCanSwap to IRMAClientRole
grant exec on dbo.GetStoreTlogFtpInfo to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetStoreUserSubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetStoreVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeamBrand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeamBrand to IRMASLIMRole
grant exec on dbo.GetSubTeamByProductType to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeamByProductType to IRMAExcelRole
grant exec on dbo.GetSubTeamCategory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeamMargin to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeamName to IRMAAVCIRole
grant exec on dbo.GetSubTeamName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeams to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeams to IRMAExcelRole
grant exec on dbo.GetSubTeams_BySubTeam_NameStartsWith to IRMAClientRole
grant exec on dbo.GetSubTeamsByTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeamTotSalesCost to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSubTeamVendors to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSupplierRetailSubteam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSupplierSubteam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSupplierSubteamByVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSystemDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetSystemDateCmd to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetTaxClass_ByDescExact to IRMASLIMRole
grant exec on dbo.GetTaxClass_ByTaxClassDescStartsWith to IRMASLIMRole
grant exec on dbo.GetTaxClasses to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetTaxClasses to IRMAExcelRole
grant exec on dbo.GetTeamByStoreSubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetTeamBySubTeam to IRMAAVCIRole
grant exec on dbo.GetTeamBySubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetTeams to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetTeams to IRMASLIMRole
grant exec on dbo.GetThirdPartyInvoiceNumberUse to IRMAClientRole
grant exec on dbo.GetTitleConflicts to IRMAClientRole, IRMAAdminRole
grant exec on dbo.GetTitlePermissions to IRMAClientRole, IRMAAdminRole
grant exec on dbo.GetTop20Items_AllSubTeams to IRMAReportsRole
grant exec on dbo.GetTranCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUnAvailStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUNFIOrder to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUnitAndID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUnitAndID to IRMAExcelRole
grant exec on dbo.GetUnitInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUnitInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUnitInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUnitLockStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetUnitWeight to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUrgentVCAI_Exceptions to IRMAAVCIRole
grant exec on dbo.GetUrgentVCAI_Exceptions to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUser to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUserID to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUserEmail to IRMAAVCIRole
grant exec on dbo.GetUserEmail to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUserFullName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUserName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUsers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUsersLastOrderHeaderID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUsersSubteam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUsersSubteamAssignments to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetUsersSubTeamList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUserStoreProductSubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUserStoreTeam_ByUserTitle to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetUserStoreTeamTitleByUser to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetUserStoreTeamTitles_ByUser_ID to IRMASLIMRole
grant exec on dbo.GetUsersWithTitle to IRMAClientRole, IRMAAdminRole
grant exec on dbo.GetValidationCodeDetails to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetValidationCodeDetails to IRMASLIMRole
grant exec on dbo.GetVCAI_Exceptions to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVCAI_ExceptionsCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendCostData to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendor_ByCompanyName to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetVendor_ByCompanyNameExact to IRMASLIMRole
grant exec on dbo.GetVendor_ByCompanyNameStartsWith to IRMAClientRole
grant exec on dbo.GetVendor_ByCompanyNameStartsWith to IRMASLIMRole
grant exec on dbo.GetVendor_ByPSVendorID to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetVendor_ByVendorID to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetVendor_Listing to IRMAReportsRole
grant exec on dbo.GetVendor1099AndID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetVendorByPSVendorID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorByPSVendorID to IRMAExcelRole
grant exec on dbo.GetVendorcost to IRMAAVCIRole
grant exec on dbo.GetVendorCost to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorCost to IRMAExcelRole
grant exec on dbo.GetVendorCostHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorCountByVendorKey to IRMAClientRole
grant exec on dbo.GetVendorDealHistory to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetVendorDealHistoryStackableConflicts to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetVendorDealTypes to IRMAClientRole, IRMASupportRole
grant exec on dbo.GetVendorElectronic_Transfer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorInfoFirst to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorInfoLast to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorItems to IRMAExcelRole
grant exec on dbo.GetVendorItemsInStores_All to IRMAReportsRole
grant exec on dbo.GetVendorLinks to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorLockStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorName to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendors to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendors to IRMAExcelRole
grant exec on dbo.GetVendors to IRMASLIMRole
grant exec on dbo.GetVendors_Detail to IRMAReportsRole
grant exec on dbo.GetVendorStore to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendorStoreCurrencyMatch to IRMAClientRole, IRMAReportsRole
grant exec on dbo.GetVendStoreSubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVendZones to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetVersion to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole
grant exec on dbo.GetWarehouse to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetWarehouseCustomerOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetWarehouseItemChanges to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetWarehousePurchaseOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetWarehouses to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetWarehouseVendChanges to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetWIMPExtract_COSTDATA to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetWIMPExtract_ITEMDATA to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetWIMPExtract_PLANOSTATUS to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetZones to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetZones to IRMAExcelRole
grant exec on dbo.GetZoneSubTeams to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GetZoneSupply to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GLDistributionCheckDetailsReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GLDistributionCheckFromSubReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GLDistributionCheckReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GLDistributionCheckSubReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GLDistributionsReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GLSalesReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GLTransfersReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.GrossMarginExceptionReport to IRMAClientRole, IRMAReportsRole
grant exec on dbo.GrossMarginExceptionReportVsMvmt to IRMAClientRole, IRMAReportsRole
grant exec on dbo.HourlySales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.IBMCatalogDump to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.IBMIncrementRecords to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.IdentifyItem to IRMAAVCIRole
grant exec on dbo.IdentifyItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.IdentifyItem to IRMAExcelRole
grant exec on dbo.IMHA_Update_Costs to IMHARole
grant exec on dbo.IMHA_Update_Promo to IMHARole
grant exec on dbo.IMHA_Update_VIN to IMHARole
grant exec on dbo.ImportBOHFile to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertAccountingIn to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertAvgCostAdjustment to IRMAClientRole
grant exec on dbo.InsertAvgCostAdjustmentReason to IRMAClientRole
grant exec on dbo.InsertCloneStore to IRMAAdminRole, IRMAClientRole
grant exec on dbo.InsertCloneTaxJurisdiction to IRMAAdminRole, IRMAClientRole
grant exec on dbo.InsertCompetitorImportInfo to IRMAClientRole
grant exec on dbo.InsertCompetitorImportSession to IRMAClientRole
grant exec on dbo.InsertCompetitorPrice to IRMAClientRole
grant exec on dbo.InsertCompetitorPriceFromImportSession to IRMAClientRole
grant exec on dbo.InsertContact to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertConversion to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCostAdjustmentReason to IRMAClientRole
grant exec on dbo.InsertCostPromoCodeType to IMHARole
grant exec on dbo.InsertCostPromoCodeType to IRMAClientRole
grant exec on dbo.InsertCurrency to IRMAAdminRole, IRMAClientRole
grant exec on dbo.InsertCustomer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCustomerReturn to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCustomerReturnItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCycleCountHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCycleCountItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCycleCountItem2 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCycleCountItemsAll to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCycleCountItemsBrand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCycleCountItemsCategory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCycleCountItemsMostExpensive to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCycleCountItemsVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertCycleCountItemsZeroCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertGLPushHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertGLPushQueue to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertImportCycleCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertInstanceDataFlagsStoreOverride to IRMAAdminRole, IRMAClientRole
grant exec on dbo.InsertInventoryAdjustmentCode to IRMAClientRole
grant exec on dbo.InsertInventoryLocationItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItem to IRMASLIMROLE
grant exec on dbo.InsertItemBrand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemCategory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemChain to IRMAClientRole
grant exec on dbo.InsertItemChainItem to IRMAClientRole
grant exec on dbo.GetWasteTypes to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemHistoryShrink to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemHistoryShrinkUpdate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemHistory2 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemHistory3 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemHistory4 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemHistoryCycleCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemHistoryCycleCountCursor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemIdentifier to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemOrigin to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemRequest to IRMASLIMRole
grant exec on dbo.InsertItemShelfLife to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemSign to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemUnit to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemUPCs to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemUploadDetail to IRMAClientRole
grant exec on dbo.InsertItemUploadHeader to IRMAClientRole
grant exec on dbo.InsertItemVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertItemVendor to IRMASLIMROLE
grant exec on dbo.InsertJobError to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.InsertJobStatus to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.InsertODBCError to ExtractRole
grant exec on dbo.InsertODBCError to IMHARole
grant exec on dbo.InsertODBCError to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertODBCError to IRMAAVCIRole
grant exec on dbo.InsertODBCError to IRMAPromoRole
grant exec on dbo.InsertODBCError to IRMARSTRole
grant exec on dbo.InsertODBCError to IRMASLIMRole
grant exec on dbo.InsertOrder to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertOrder2 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertOrderInvoice to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertOrderItemCredit to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertOrderItemRtnID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertOrganization to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertOrgPO to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertPOSChanges to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertPOSItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertPriceBatchHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertPricingMethodMapping to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertProdHierarchyLevel3 to IRMAClientRole
grant exec on dbo.InsertProdHierarchyLevel4 to IRMAClientRole
grant exec on dbo.InsertPromoPlanner to IRMAPromoRole
grant exec on dbo.InsertPromoPlannerFromEIM to IRMAClientRole
grant exec on dbo.InsertPromoPlannerFromEIM to IRMAPromoRole
grant exec on dbo.InsertReceivingItemHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertReturnOrderHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertSalesExportQueue to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertSASIItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertScanGunStoreSubTeam to IRMAClientRole
grant exec on dbo.InsertSLIMVendor to IRMASLIMRole
grant exec on dbo.InsertStoreCompetitorStore to IRMAClientRole
grant exec on dbo.InsertStoreItemVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertStoreItemVendor to IRMASLIMROLE
grant exec on dbo.InsertThirdPartyFreightInvoice to IRMAClientRole
grant exec on dbo.InsertUpdateItemOverride to IRMAClientRole
grant exec on dbo.InsertUpdateItemOverride to IRMASLIMRole
grant exec on dbo.InsertUpdateOrderWindow to IRMAClientRole
grant exec on dbo.InsertVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertVendor to IRMASLIMROLE
grant exec on dbo.InsertVendorCostHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertVendorCostHistory to IRMASLIMROLE
grant exec on dbo.InsertVendorCostHistory3 to IRMAAVCIRole
grant exec on dbo.InsertVendorCostHistory3 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertVendorCostHistoryException to IRMAAVCIRole
grant exec on dbo.InsertVendorCostHistoryException to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertVendorDealHistory to IRMAAVCIRole
grant exec on dbo.InsertVendorDealHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InsertZoneSubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InStoreSpecials_Report to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InventoryBalance to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.InventoryValueDetail to IRMAAdminRole, IRMAClientRole
grant exec on dbo.InventoryValueDetail to IRMAExcelRole
grant exec on dbo.InventoryValueDetail to IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InventoryValueReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InventoryValueSummary to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.InventoryValueSummary to IRMAExcelRole
grant exec on dbo.InvoiceDiscrepanciesReport to IRMAReportsRole
grant exec on dbo.InvoiceManifestReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.IRMA_Main_GetMenuAccess to IRMAClientRole, IRMASupportRole
grant exec on dbo.IsSameDayPromoChgConflict to IRMASupportRole
grant exec on dbo.IsScaleIdentifier to IRMAClientRole
grant exec on dbo.ItemAttributes_GetAttributeIdentifiersByItemKey to IRMAClientRole
grant exec on dbo.ItemAttributes_GetItemAttributeByItemKey to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_DeleteAttributeIdentifier to IRMAAdminRole, IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_DeleteItemAttribute to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_GetAllAttributeIdentifiers to IRMAAdminRole, IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_GetAllItemAttributes to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_GetAttributeIdentifierByPK to IRMAAdminRole, IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_GetItemAttributeByPK to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_InsertAttributeIdentifier to IRMAAdminRole, IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_InsertItemAttribute to IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_UpdateAttributeIdentifier to IRMAAdminRole, IRMAClientRole
grant exec on dbo.ItemAttributes_Regen_UpdateItemAttribute to IRMAClientRole
grant exec on dbo.ItemHosting_GetLabelType to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ItemHosting_UpdateStore to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.ItemListReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ItemMasterReport to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.ItemOnHandComparisonBetweenLocation to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ItemPriceListByItemAndStore to IRMAClientRole
grant exec on dbo.ItemPriceReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ItemSalesByStoreComp to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ItemStatusList to IRMAReportsRole
grant exec on dbo.ItemTypeList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ItemVendorCostCurrent to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ItemVendorCostSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ItemWebQueryDetail to IRMASLIMRole
grant exec on dbo.ItemWebQueryDetailMovement to IRMASLIMRole
grant exec on dbo.ItemWebQueryDetailShort to IRMASLIMRole
grant exec on dbo.ItemWebQueryScaleDetail to IRMASLIMRole
grant exec on dbo.ItemWebQueryStoreDetail to IRMASLIMRole
grant exec on dbo.KitchenCaseTransferRpt to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.KitchenCaseTransferRpt to IRMAExcelRole
grant exec on dbo.KitchenCaseTransferRpt to IRMASLIMRole
grant exec on dbo.LoadCheck to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LoadCycleCountExternal to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.LoadInventoryServiceImport to IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.LoadInventoryServiceExport to IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.LoadPMPriceChange to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.LoadSalesData2 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockBrand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockCategory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockFSCustomer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockFSOrganization to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockOrderHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockOrigin to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockShelfLife to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockUnit to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LockVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.LYDeptComp to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ManufacturingOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.MarginBySubTeamReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.MarginBySubTeamReport_UseLastCost to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.MarginByVendorReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.MarginByVendorReport_UseLastCost to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.MarkDownByCashier to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.MovementByDay to IRMAReportsRole
grant exec on dbo.MovementCompSales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.MovementCompSalesNew to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.MovementCompSalesOrg to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.MovementSingleStore to IRMAReportsRole
grant exec on dbo.msqAlpharettaProduceInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqGAMeatOrderGuide to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqGetBakeryVendorInventoryItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqGetCoffeeBarVendorInventoryItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqGetIRMAItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqGetProduceAllStoresRetailPrices to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqGetPSVendorID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqGetWFMGAMeatInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqGetWFMNCMeatInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqGetWFMSCMeatInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqNCMeatOrderGuide to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqProduceRetailPriceGuide to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.msqSeafoodInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.NoSells to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.NumOfInvoicesInControlGroupReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.NumOfInvoicesMatchedInControlGroupReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.NumOfInvoicesMisMatchedInControlGroupReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.OpenOrdersDetailReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.OpenOrdersReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.OpenOrdersReportNOIDNORD to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.OrderInvoice_3PartyFreightInvoice_Get to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.OrderInvoice_3PartyFreightInvoice_Update to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.OrderInvoice_CheckInvoiceExistsInCurrentControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_CheckInvoiceExistsInOpenControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_CloseControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_DeleteControlGroupInvoice to IRMAClientRole
grant exec on dbo.OrderInvoice_GetControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_GetControlGroupInvoices to IRMAClientRole
grant exec on dbo.OrderInvoice_GetControlGroupSearch to IRMAClientRole
grant exec on dbo.OrderInvoice_InsertControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_InsertControlGroupInvoice to IRMAClientRole
grant exec on dbo.OrderInvoice_UpdateControlGroup to IRMAClientRole
grant exec on dbo.OrderInvoice_UpdateControlGroupInvoice to IRMAClientRole
grant exec on dbo.OrderInvoice_ValidateControlGroupInvoice to IRMAClientRole
grant exec on dbo.OrderItemQueueReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.OrderLink_ImportOrder to IRMASchedJobsRole;
grant exec on dbo.OrdersReceivedNotClosedReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.OutOfPeriodInvoiceReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.OutOfStockReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PendingRegularPriceChange to IRMAClientRole
grant exec on dbo.Planogram_GetFileMakerNonRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_GetFileMakerSetRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_GetFXNonRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_GetFXSetRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_GetNonRegPlanogramItems to IRMAClientRole, IRMASupportRole
grant exec on dbo.Planogram_GetNonRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_GetPrintLabNonRegTagFile to IRMAClientRole, IRMASupportRole
grant exec on dbo.Planogram_GetPrintLabSetRegTagFile to IRMAClientRole, IRMASupportRole
grant exec on dbo.Planogram_GetRegPlanogramItems to IRMAClientRole, IRMASupportRole
grant exec on dbo.Planogram_GetSetNumbers to IRMAClientRole, IRMASupportRole
grant exec on dbo.Planogram_GetSetRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_GetAccessViaExtendedSetRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_GetAccessViaSetRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_GetAccessViaNonRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_GetAccessViaExtendedNonRegTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Planogram_InsertPlanogramItem to IRMAClientRole
grant exec on dbo.POExceptionReport to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.POSDeleteItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.POSGetFTPStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.POSGetStoreInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PosQueueGLClose to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.PosQueueGLPush to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.PostStoreItemChange to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Pricebook_Report to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.PriceChangeEventReport to IRMAReportsRole
grant exec on dbo.PricingMethodList to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PricingPrintSignsSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ProductInventoryGuide to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ProductNotAvaiable to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ProductOrderGuide to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PromoPivotTable to IRMAPromoRole
grant exec on dbo.PromoSalesExport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PSIItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PSIMovement to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PSIStore to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PSISubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PSIVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PurchaseAccrualReport to IRMAClientRole, IRMAReportsRole
grant exec on dbo.PurchasesSummaryReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.PurchToSalesComp to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.QueuePMOrganizationChg to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.QueuePMProductChg to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ReceiveOrderItem3 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ReceiveOrderItem4 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ReceivingCheckList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.RegionSalesCompByDayReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.RegionSalesCompByWeekReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.RegionSalesCompTrendReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.RemoveGLQueue to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.RemoveItemOnHand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.RemoveItemUPCInventory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.RepInventoryLocationItems to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.RepInventoryLocationItemsCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.RepInventoryLocations to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_GetIdentifierAddsForAudits to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSAudit_FindExceptions to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSAudit_GetNoOfExceptions to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPull_DeptPriceAuditReport to IRMAReportsRole
grant exec on dbo.Replenishment_POSPull_GetIdentifier to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.Replenishment_POSPull_GetIdentifier to IRMARSTRole
grant exec on dbo.Replenishment_POSPush_GetIdentifierRefreshes to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_POSPush_UpdateRefreshSent to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_POSPull_InsertPOSItem to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_POSPull_InsertTempPriceAudit to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_POSPull_PriceAuditReport to IRMAReportsRole
grant exec on dbo.Replenishment_POSPull_StorePriceAuditReport to IRMAReportsRole
grant exec on dbo.Replenishment_POSPush_AddIdentifier to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_POSPush_DeAuthorizeItem to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPush_DeleteIdentifier to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_POSPush_GetAllFTPConfigData to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPush_GetFTPConfigForStoreAndWriterType to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPush_GetFTPConfigForWriterType to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPush_GetFTPConfigForWriterType to IRMARSTRole
grant exec on dbo.Replenishment_POSPush_GetIdentifierAdds to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_POSPush_GetIdentifierDeletes to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_POSPush_GetPOSWriterFileConfig to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_POSPush_GetPriceBatchOffers to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_POSPush_GetPriceBatchSent to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_POSPush_GetStoreWriterConfigurations to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_POSPush_GetVendorAdds to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPush_UpdateOffPromoCostRecords to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPush_UpdatePriceBatchProcessedChg to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPush_UpdatePriceBatchProcessedDel to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_POSPush_UpdatePromoOffersProcessed to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_POSPush_UpdateVendorAddsProcessed to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_AuthorizeItem to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_DeAuthorizeItem to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_DeleteExtraTextChgQueueTmp to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_DeleteNutriFactsChgQueueTmp to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_InsertScaleExtraTextChgQueue to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_InsertNutrifactsChgQueue to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_GetExtraTextChanges to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_GetNutriFactChanges to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_GetSmartXPrices to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_ScalePush_GetStoreWriterConfigurations to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetAccessViaBatchTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetAccessViaReprintTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetAccessViaExtendedBatchTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetAccessViaExtendedReprintTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetBatchTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetFileMakerBatchTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetFileMakerReprintTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetFXBatchTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetFXReprintTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetPrintLabBatchTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetPrintLabReprintTagFile to IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetReprintTagFile to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TagPush_GetStoreWriterConfigurations to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.Replenishment_TagPush_UpdatePBDWithTagID to IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_ClearLoadTables to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_GetStores to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_LoadDWCMCard to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_LoadDWCMReserve to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_LoadDWCMReward to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_LoadDWCMVar to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_LoadDWDiscnt to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_LoadDWItem to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_LoadDWMrkDwn to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_LoadDWTaxRec to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_LoadDWTender to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_Tlog_House_UpdateAggregates to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_ClearLoadingTables to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateDiscountRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateItemRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreateItemRecord to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Replenishment_TLog_UK_CreateOfferRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_CreatePaymentRecord to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_TLog_UK_CreateTransactionRecord to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Replenishment_TLog_UK_CreateVoidRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Replenishment_TLog_UK_UpdateSalesAggregates to IRMAAdminRole, IRMAClientRole
grant exec on dbo.Reporting_AdjustmentSummary to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Reporting_BOHCompare to IRMAClientRole, IRMASupportRole, IRMAReportsRole
grant exec on dbo.Reporting_CompetitorTrend to IRMAClientRole, IRMAReportsRole
grant exec on dbo.Reporting_COOLReceiving to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_COOLShipping to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_CostAudit to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.Reporting_GeneralMarginAudit to IRMAReportsRole
grant exec on dbo.Reporting_GetAverageCostVariance to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_GetOrderReceivingStore to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_GetOrderTransferToSubTeams to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_GetPaidByAgreedCostSavings to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_GetPOAdminReasonCodesInSPOT to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_GetPORefusalCodes to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_GetPOItemLevelRefusalCodes to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_GetPostPaymentDiscrepancyResolution to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_GetStores to IRMAReportsRole
grant exec on dbo.Reporting_GetSubTeams to IRMAReportsRole
grant exec on dbo.Reporting_GetVendors to IRMAReportsRole
grant exec on dbo.Reporting_ItemList to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.Reporting_ItemList_UseLastCost to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Reporting_MarginImpact to IRMAReportsRole
grant exec on dbo.Reporting_MarginImpact_Helper to IRMAReportsRole
grant exec on dbo.Reporting_Movement_BySubteam to ExtractRole
grant exec on dbo.Reporting_Movement_BySubteam to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Reporting_Movement_Full to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.Reporting_Movement_Full_UK to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.Reporting_Movement_Promo to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on dbo.Reporting_NA_MassExemptList to IRMAReportsRole
grant exec on dbo.Reporting_NotOnFile to IRMAReportsRole
grant exec on dbo.Reporting_PIRIS_ImportFile to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on dbo.Reporting_PIRIS_RunAudit to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on dbo.Reporting_RGIS_Extract to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on dbo.Reporting_SpecialsByDateRange to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.Reporting_SpecialsInProgress to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Reporting_VendorItems to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.Reporting_VendorItemsList to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on dbo.PurchasesToSalesCompSubTeamDetail TO IRMAReportsRole
grant exec on dbo.PurchasesToSalesCompSubTeamSummary TO IRMAReportsRole
grant exec on dbo.ResetFailedScheduledJob to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.RIPECheckExistingDistributions to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.RIPEDeleteIRSOrderHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.RIPEGetDistributions to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.RIPEGetImportedOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.RIPEImportErrors to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.RIPEInsertImportData to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.RollbackElectronicOrder to IRMAClientRole
grant exec on dbo.rptLot_NoByIdentifier to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.rptLot_NoByLot_No to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.SalesAggregation to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.SalesByItemByDayCrossTab to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.SalesPercentage to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.SalesPercentageCrossTab to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.SalesSummary to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.SaveItemDefaultValue to IRMAClientRole
grant exec on dbo.SaveTitlePermissions to IRMAClientRole, IRMAAdminRole
grant exec on dbo.Scale_CheckForDuplicateEatBy to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_CheckForDuplicateExtraText to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_CheckForDuplicateGrade to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_CheckForDuplicateLabelFormat to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_CheckForDuplicateLabelStyle to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_CheckForDuplicateLabelType to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_CheckForDuplicateNutriFact to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_CheckForDuplicateRandomWeightType to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_CheckForDuplicateTare to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_DeleteEatBy to IRMAClientRole
grant exec on dbo.Scale_DeleteGrade to IRMAClientRole
grant exec on dbo.Scale_DeleteLabelFormat to IRMAClientRole
grant exec on dbo.Scale_DeleteLabelStyle to IRMAClientRole
grant exec on dbo.Scale_DeleteLabelType to IRMAClientRole
grant exec on dbo.Scale_DeleteRandomWeightType to IRMAClientRole
grant exec on dbo.Scale_DeleteTare to IRMAClientRole
grant exec on dbo.Scale_GetEatBy to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetExtraText to IRMAReportsRole, IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.Scale_GetExtraTextByItem to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetExtraTextCombo to IRMAClientRole
grant exec on dbo.Scale_GetExtraTextCombo to IRMAReportsRole, IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.Scale_GetExtraTextCombo to IRMASLIMRole
grant exec on dbo.Scale_GetExtraTexts to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetGrades to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetItemScaleDetails to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetItemScaleOverride to IRMAClientRole
grant exec on dbo.Scale_GetItemScaleOverride to IRMASLIMRole
grant exec on dbo.Scale_GetLabelFormats to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetLabelStyles to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetLabelTypes to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetNutriFact to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetNutriFactByItem to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetNutriFacts to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetRandomWeightTypes to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetRandomWeightTypes to IRMASLIMRole
grant exec on dbo.Scale_GetScaleDetailCombos to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetScaleUOMs to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_GetScaleUOMs to IRMASLIMRole
grant exec on dbo.Scale_GetTares to IRMAClientRole, IRMASupportRole
grant exec on dbo.Scale_InsertUpdateEatBy to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateExtraText to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateExtraText to IRMASLIMRole
grant exec on dbo.Scale_InsertUpdateGrade to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateItemScaleDetails to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateItemScaleDetails to IRMASLIMRole
grant exec on dbo.Scale_InsertUpdateItemScaleOverride to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateItemScaleOverride to IRMASLIMRole
grant exec on dbo.Scale_InsertUpdateLabelFormat to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateLabelStyle to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateLabelType to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateNutriFact to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateRandomWeightType to IRMAClientRole
grant exec on dbo.Scale_InsertUpdateTare to IRMAClientRole
grant exec on dbo.ScanReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.SecurityGetApplications to IRMAAdminRole, IRMAClientRole
grant exec on dbo.SecurityGetTitles to IRMAAdminRole, IRMAClientRole
grant exec on dbo.SetAPUploadsUploaded to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.SetFQSCusModifiedCreatedTime to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.SetFQSOrgModifiedCreatedTime to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.SetOrderSentDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.SetPrimaryVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.SetPrimaryVendor to IRMASLIMROLE
grant exec on dbo.ShortsReport to IRMAClientRole, IRMAReportsRole
grant exec on dbo.SLIM_CreateStoreSpecial to IRMASLIMRole
grant exec on dbo.SLIM_DeleteItemRequest to IRMASLIMRole
grant exec on dbo.SLIM_GetInStoreSpecials to IRMASLIMRole
grant exec on dbo.SLIM_GetItemOverride to IRMASLIMRole
grant exec on dbo.SLIM_ProcessStoreSpecial to IRMASLIMRole
grant exec on dbo.SLIM_RejectItemRequest to IRMASLIMRole
grant exec on dbo.SLIM_RejectStoreSpecial to IRMASLIMRole
grant exec on dbo.SLIM_ReProcessItemRequest to IRMASLIMRole
grant exec on dbo.SLIM_ReProcessStoreSpecial to IRMASLIMRole
grant exec on dbo.SLIM_StoreSpecialsStatus to IRMASLIMRole
grant exec on dbo.SLIM_GetPriceBatchDetailInfo to IRMASLIMRole
grant exec on dbo.SOG_AddAdminSetting to IRMASLIMRole
grant exec on dbo.SOG_AddCatalog to IRMASLIMRole
grant exec on dbo.SOG_AddCatalogItem to IRMASLIMRole
grant exec on dbo.SOG_AddCatalogSchedule to IRMASLIMRole
grant exec on dbo.SOG_AddCatalogStore to IRMASLIMRole
grant exec on dbo.SOG_AddError to IRMASLIMRole
grant exec on dbo.SOG_AddOrder to IRMASLIMRole
grant exec on dbo.SOG_AddOrderItem to IRMASLIMRole
grant exec on dbo.SOG_DelAdminSetting to IRMASLIMRole
grant exec on dbo.SOG_DelCatalog to IRMASLIMRole
grant exec on dbo.SOG_DelCatalogItem to IRMASLIMRole
grant exec on dbo.SOG_DelCatalogSchedule to IRMASLIMRole
grant exec on dbo.SOG_DelCatalogStore to IRMASLIMRole
grant exec on dbo.SOG_GetAdminSetting to IRMASLIMRole
grant exec on dbo.SOG_GetAdminSettings to IRMASLIMRole
grant exec on dbo.SOG_GetBrandList to IRMASLIMRole
grant exec on dbo.SOG_GetCatalogItems to IRMASLIMRole
grant exec on dbo.SOG_GetCatalogs to IRMASLIMRole
grant exec on dbo.SOG_GetCatalogSchedules to IRMASLIMRole
grant exec on dbo.SOG_GetCatalogStores to IRMASLIMRole
grant exec on dbo.SOG_GetClassList to IRMASLIMRole
grant exec on dbo.SOG_GetItemList to IRMASLIMRole
grant exec on dbo.SOG_GetLevel3List to IRMASLIMRole
grant exec on dbo.SOG_GetManagedByList to IRMASLIMRole
grant exec on dbo.SOG_GetStoreList to IRMASLIMRole
grant exec on dbo.SOG_GetSubTeamList to IRMASLIMRole
grant exec on dbo.SOG_GetUserDetails to IRMASLIMRole
grant exec on dbo.SOG_GetVendorList to IRMASLIMRole
grant exec on dbo.SOG_GetZoneList to IRMASLIMRole
grant exec on dbo.SOG_PrintCatalog to IRMASLIMRole, IRMAReportsRole
grant exec on dbo.SOG_PrintCatalogs to IRMASLIMRole, IRMAReportsRole
grant exec on dbo.SOG_SetAdminSetting to IRMASLIMRole
grant exec on dbo.SOG_SetCatalog to IRMASLIMRole
grant exec on dbo.SOG_MassPublishCatalogs to IRMASLIMRole
grant exec on dbo.SOG_SetCatalogItem to IRMASLIMRole
grant exec on dbo.SOG_SetCatalogSchedule to IRMASLIMRole
grant exec on dbo.SOG_SetOrder to IRMASLIMRole
grant exec on dbo.SpecialsByEndDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.StoreItemAttribute_GetAttribute to IRMAClientRole, IRMASupportRole
grant exec on dbo.StoreItemAttribute_InsertUpdateAttribute to IRMAClientRole
grant exec on dbo.StoreOpsOrdersExport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.StoreOpsSalesExport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.StoreOpsVendorExport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.StoreOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.StoreOrdersTotBySKUReport to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on dbo.StoreOrdersTotBySKUReport to IRMAExcelRole
grant exec on dbo.SubTeams_CreateSubTeam to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.SubTeams_CreateSubTeamToTeamRelationship to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.SubTeams_GetSubTeam to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.SubTeams_GetSubTeamsByStore to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.SubTeams_GetTeamSubTeamRelationshipsByStore to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.SubTeams_LoadSubTeams to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.SubTeams_SaveSubteam to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.SubTeams_UpdateSubTeamToTeamRelationship to IRMAAdminRole, IRMAClientRole
grant exec on dbo.SubTeams_ValidateSubTeamToTeamRelationships to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.SubTeamSales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.SwitchPrimaryVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.Takings to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TaxHosting_ConfirmDeleteTaxFlag to IRMAClientRole
grant exec on dbo.TaxHosting_DeleteTaxFlag to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.TaxHosting_DeleteTaxJurisdiction to IRMAAdminRole, IRMAClientRole
grant exec on dbo.TaxHosting_DeleteTaxOverride to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.TaxHosting_DeleteTaxOverrideForItem to IRMAClientRole
grant exec on dbo.TaxHosting_GetAvailableTaxFlagsForItem to IRMAClientRole, IRMASupportRole
grant exec on dbo.TaxHosting_GetTaxClass to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TaxHosting_GetTaxFlag to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TaxHosting_GetTaxFlagActiveCount to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TaxHosting_GetTaxJurisdictions to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TaxHosting_GetTaxJurisdictionsForTaxClass to IRMAClientRole, IRMASupportRole
grant exec on dbo.TaxHosting_GetTaxOverride to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TaxHosting_GetTaxOverrideForItem to IRMAClientRole, IRMASupportRole
grant exec on dbo.TaxHosting_InsertDefaultTaxFlags to IRMAClientRole
grant exec on dbo.TaxHosting_InsertTaxFlag to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.TaxHosting_InsertTaxOverride to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.TaxHosting_IsExistingTaxFlagForJurisdiction to IRMAClientRole, IRMASupportRole
grant exec on dbo.TaxHosting_UpdateTaxFlag to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.TaxHosting_UpdateTaxJurisdiction to IRMAAdminRole, IRMAClientRole
grant exec on dbo.TaxHosting_UpdateTaxOverride to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.TaxReport1 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TaxReport2 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TaxReport3 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Teams_CreateNew to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.Teams_GetTeam to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.Teams_LoadTeams to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.Teams_SaveChanges to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.Teams_Validate_TeamAbbr to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.Teams_Validate_Teamname to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.TestOrgPO to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TGMToolGetAcctTotals to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TGMToolGetDataAll to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TGMToolGetDataBrand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TGMToolGetDataCategory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TGMToolGetDataVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ThirteenWeekMovementReportDist to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ThreeWayMatchDetailSummaryReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.ThreeWayMisMatchedDetailSummaryReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.TopMovers to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TopMovers_UseLastCost to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.TopMoversList to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TopMoversSummary to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.TopMoversSummary_UseLastCost to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.TYDeptComp to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnitCount7Day to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnitCount7DayByStore to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnitCountTotalAllStores_CrossTab to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.UnlockBrand to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnlockCategory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnlockFSCustomer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnlockFSOrganization to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnlockItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnlockOrderHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnlockOrigin to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnlockShelfLife to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnlockUnit to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UnlockVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateAverageCost to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateAvgCostAdjReasonStatus to IRMAClientRole
grant exec on dbo.UpdateBatchesInSentState to IRMAClientRole
grant exec on dbo.UpdateBrandInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateCategoryInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateCompetitivePriceInfo to IRMAClientRole
grant exec on dbo.UpdateCompetitorImportInfo to IRMAClientRole
grant exec on dbo.UpdateCompetitorImportInfoWithIdentifiers to IRMAClientRole
grant exec on dbo.UpdateCompetitorPrice to IRMAClientRole
grant exec on dbo.UpdateContactInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateConversionInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateCurrency to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateCustomer to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateCustReturnItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateCycleCountHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateCycleCountMaster to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateCycleCountMasterClosed to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateFSCustomerInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateInstanceData to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateInstanceDataFlagsStoreOverride to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateInstanceDataFlagsValues to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateInventoryAdjustmentCode to IRMAClientRole
grant exec on dbo.UpdateInventoryLocation to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemHistoryFromSales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemID to IRMAAVCIRole
grant exec on dbo.UpdateItemID to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemIdentifier to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemIdentifierDefault to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemInfoForBulkLoad to IRMAClientRole
grant exec on dbo.UpdateItemOrigin to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemPOSData to IRMAClientRole
grant exec on dbo.UpdateItemRestore to IRMAClientRole
grant exec on dbo.UpdateItemScaleData to IRMAClientRole
grant exec on dbo.UpdateItemUploadDetail to IRMAClientRole
grant exec on dbo.UpdateItemUploadHeader to IRMAClientRole
grant exec on dbo.UpdateItemVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemVendor to IRMASLIMROLE
grant exec on dbo.UpdateJobStatus to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.UpdateJobStatusList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateLineDrive to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderApproved to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderBackdate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderBeforeClose to IRMAClientRole
grant exec on dbo.UpdateOrderCancelSend to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderClosed to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderCurrency to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.UpdateOrderHeaderDesc to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderHeaderFreight3Party to IRMAClientRole, IRMAReportsRole
grant exec on dbo.UpdateOrderInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderItemAdjustedCost to IRMAClientRole
grant exec on dbo.UpdateOrderItemAlloc to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderItemComments to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderItemCostData to IRMAClientRole
grant exec on dbo.UpdateOrderItemFreight to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderItemFreight to IRMAExcelRole
grant exec on dbo.UpdateOrderItemFreight to IRMAPromoRole
grant exec on dbo.UpdateOrderItemFreight to IRMASLIMRole 
grant exec on dbo.UpdateOrderItemFreight3PartyAll to IRMAClientRole, IRMAReportsRole
grant exec on dbo.UpdateOrderItemFreight3PartyOne to IRMAClientRole, IRMAReportsRole
grant exec on dbo.UpdateOrderItemInfo to IRMAClientRole, IRMAAdminRole, IRMAReportsRole, IRMASupportRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderItemQueue to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderItemReceivingInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderItemUnitsReceived to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderNotSent to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderOpen to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderRefreshCosts to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrderResetWarehouseSent to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrdersApplyNewVendorCost to IRMAClientRole
grant exec on dbo.UpdateOrderSend to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderSentDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderSentToEmailDate to IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderSentToFaxDate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderStatus to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderVendor to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderWarehouseSend to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrderWarehouseSent to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOrganizationInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateOriginInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePOSBatchInfo to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.UpdatePOSChangesAggregated to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePOSChangesGL_Pushed to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePriceBatchDetailCutHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePriceBatchDetailHeader to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePriceBatchDetailPromo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePriceBatchDetailPromo to IRMASLIMROLE
grant exec on dbo.UpdatePriceBatchDetailPromoPlanner to IRMAPromoRole
grant exec on dbo.UpdatePriceBatchDetailReg to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePriceBatchDetailReg to IRMASLIMROLE
grant exec on dbo.UpdatePriceBatchHeader to IRMAClientRole
grant exec on dbo.UpdatePriceBatchPackage to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePriceBatchStatus to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePriceBatchUnpackage to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdatePriceTaxChange to IRMAClientRole
grant exec on dbo.UpdatePromoItem to IRMAPromoRole
grant exec on dbo.UpdatePromoItemRev to IRMAPromoRole
grant exec on dbo.UpdatePromoPreOrder to IRMAPromoRole
grant exec on dbo.UpdateReturnOrderRecord to IRMAClientRole
grant exec on dbo.UpdateSalesAggregates to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateShelfLifeInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateSignQueuePrinted to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateSignQueuePrinted to IRMARSTRole
grant exec on dbo.UpdateSLIMItemAttribute to IRMASLIMRole
grant exec on dbo.UpdateStoreCompetitorStore to IRMAClientRole
grant exec on dbo.UpdateStoreItem to IRMAClientRole
grant exec on dbo.UpdateStoreItem to IRMASLIMRole
grant exec on dbo.UpdateStoresSignListPrinted to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateTitle to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateTitleConflicts to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateUnitInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateUnreceivedOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateVCAI_Exception to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateVendorCostHistory to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateVendorDealHistory to IRMAClientRole
grant exec on dbo.UpdateVendorInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateVendorInfo to IRMASLIMROLE
grant exec on dbo.UpdateVendorStorePayAgreedCostSetup to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.UpdateZoneSubTeam to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateZoneSupply to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.usp_ImportMultipleFiles to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ValidateCopyPOItems to IRMAClientRole
grant exec on dbo.ValidateLogin to IRMAAdminRole, IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.ValidateMonth to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ValidateTeams to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ValidateWeek to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.Vendor52WeekByDept to IRMAReportsRole
grant exec on dbo.VendorEfficiency to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VendorItemCount to IRMAReportsRole
grant exec on dbo.VendorItemReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VendorMovement to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VendorSearch to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMAuthorizationStatusFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMItemRegionFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMItemStatusFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMItemStoreExceptionFile to IRMASchedJobsRole
grant exec on dbo.VIMPriceTypeFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMPriceZoneFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMPSVendorRefFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMRegHierarchyFile to IRMASchedJobsRole
grant exec on dbo.VIMRegionalDepartmentFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMRetailFuturePriceFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMRetailPriceFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMStoreFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMVendorCostFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.VIMVendorStoreItemFile to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.WarehouseMovement to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.WarehouseOutOfStock to IRMAClientRole, IRMAReportsRole
grant exec on dbo.WasteReport to IRMASupportRole
grant exec on dbo.WasteToSales to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.WeeklyRollUp_SalesSumByItem to IRMAAdminRole, IRMAClientRole
grant exec on dbo.WeeklyRollUp_SalesSumByItem to IRMAReports
grant exec on dbo.xl_OffSale to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.xl_OffSaleStores to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.xl_OffSaleSubteams to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.YearlyBuggyCount1 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.YearlyBuggyCount2 to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ZeroCostItemsReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ZeroMovementReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant exec on dbo.ZeroMovers52Week to IRMAReportsRole
grant exec on EInvoicing_CreateInvoiceRecord to IRMAAdminRole, IRMAClientRole
grant exec on EInvoicing_getEInvoices to IRMAAdminRole, IRMAClientRole
grant exec on fn_GetItemDescription to IRMAClientRole, IRMAAdminRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on GetBeginFiscalYearDateRS to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on GetBeginQuarterDateRS to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on GetNetCostByDate to IRMASchedJobs, IRMAAdminRole, IRMAClientRole
grant exec on PostAlocInventoryWeeklyHistoryReport to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on PreAlocInventoryWeeklyHistoryReport to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on WareHouseWeeklyAllocatedQuantities to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on WareHouseWeeklyOrderedQuantities to IRMAAdminRole, IRMAClientRole, IRMAReportsRole
grant exec on dbo.AutomaticOrderItemInfoPackSize to IRMAClientRole
grant exec on dbo.AutomaticOrderListPackSizes to IRMAClientRole
grant exec on dbo.EInvoicing_CreateEInvoiceRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_ClearEInvoiceData to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_CreateInvoiceRecord to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertHeaderElement to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertLineItemElement to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_InsertSummaryElement to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_IsDuplicateEInvoice to IRMAAdminRole, IRMAClientRole
grant exec on dbo.EInvoicing_ValidateDataElements to IRMAAdminRole, IRMAClientRole
grant exec on dbo.getPackSizesByOrderItem to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.GetP2PUnsentInvoiceDiscrepancies to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrdersApplyNewVendorCost to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.UpdateP2PInvoiceDiscrepancySentDate to IRMAAdminRole, IRMAClientRole
grant exec on DBO.AlertBuyerAbout_ED_SS_OOS to IRMAAdminRole, IRMAClientRole
grant exec on dbo.CalculateOrderDiscountForLineItem to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.CalculateOrderItemCosts to IRMAAdminRole, IRMAClientRole, IRMASupportRole
grant exec on dbo.FindOpenOrders to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetANSOrderHeader to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetANSOrderItems to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetFaxOrderItemList to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetOrderEmail to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetOrderItemLines to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetPOHeader to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetPurchaseOrderHeader to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetSystemDate to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetUNFIOrder to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetWarehouseCustomerOrders to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetWarehousePurchaseOrders to IRMAAdminRole, IRMAClientRole
grant exec on dbo.MatchOrderInvoiceCosts to IRMAAdminRole , IRMAClientRole
grant exec on dbo.GetAllInvoiceTolerances to IRMAAdminRole , IRMAClientRole
grant exec on dbo.UpdateInvoiceTolerance to IRMAAdminRole , IRMAClientRole
grant exec on dbo.UpdateInvoiceToleranceVendor to IRMAAdminRole , IRMAClientRole
grant exec on dbo.UpdateInvoiceToleranceStore to IRMAAdminRole , IRMAClientRole
grant exec on dbo.InsertInvoiceToleranceVendor to IRMAAdminRole , IRMAClientRole
grant exec on dbo.InsertInvoiceToleranceStore to IRMAAdminRole , IRMAClientRole
grant exec on dbo.DeleteInvoiceToleranceVendor to IRMAAdminRole , IRMAClientRole
grant exec on dbo.DeleteInvoiceToleranceStore to IRMAAdminRole , IRMAClientRole
grant exec on dbo.pivot_example to public
grant exec on dbo.pivot_query to public
grant exec on dbo.pivot_table to public
grant exec on dbo.Reporting_DASHandlingCharge to IRMAReportsRole
grant exec on dbo.UpdateOrderCancelSend to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrderHeaderFreight3Party to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrderItemFreight3PartyAll to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrderItemFreight3PartyOne to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrderNotSent to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrdersApplyNewVendorCost to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrderSentDate to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrderSentToEmailDate to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrderSentToFaxDate to IRMAAdminRole, IRMAClientRole
grant exec on dbo.UpdateOrderWarehouseSent to IRMAAdminRole, IRMAClientRole
grant exec on dbo.GetOrderAllocateItemsQty to IRMAClientRole
grant exec on dbo.DeleteTempFSARecords to IRMAClientRole
grant exec on dbo.GetAllocationItemPackSizes to IRMAClientRole
grant exec on dbo.GetAllocationItems to IRMAClientRole
grant exec on dbo.UpdateAllocationItemPackSize to IRMAClientRole
grant exec on dbo.GetOrderAllocationItems to IRMAClientRole
grant exec on dbo.UpdateConfigurationData  TO IRMAClientRole
grant exec on dbo.GetConfigurationValue  TO IRMAClientRole
grant exec on dbo.GetConfigurationData  TO IRMAClientRole
grant exec on dbo.rptFacilityOpenCustomerOrders to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.PerpetualMovementReport to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemHistoryFromSales to IRMAClientRole, IRMASchedJobsRole, IRMASupportRole, IRMAAdminRole
grant exec on dbo.RIPE_GetIrmaItemForRecipe to IRMAClientRole, IRMASchedJobsRole, IRMASupportRole, IRMAAdminRole
grant exec on dbo.RIPE_UpdateRecipeCost to IRMAClientRole, IRMASchedJobsRole, IRMASupportRole, IRMAAdminRole
grant exec on dbo.UpdateReceivingDiscrepancyReasonCodeID to IRMAClientRole, IRMASchedJobsRole, IRMASupportRole, IRMAAdminRole

GO

--*********************
--	  Table Grants
--*********************

grant select on dbo.AICS to IRMAClientRole
grant select on dbo.AICS to IRMAReportsRole
grant select on dbo.AICS to IRMASchedJobsRole
grant select on dbo.AICSErrors to IRMAClientRole
grant select on dbo.AICSErrors to IRMAReportsRole
grant select on dbo.AICSErrors to IRMASchedJobsRole
grant select on dbo.App to IRMAClientRole
grant select on dbo.App to IRMAReportsRole
grant select on dbo.App to IRMASchedJobsRole
grant select on dbo.AppConfigApp to IRMAReportsRole
grant select on dbo.AppConfigEnv to IRMAReportsRole
grant select on dbo.AppConfigKey to IRMAReportsRole
grant select on dbo.AppConfigValue to IRMAReportsRole
grant select on dbo.AttributeIdentifier to IRMAAdminRole
grant select on dbo.AttributeIdentifier to IRMAClientRole
grant select on dbo.AttributeIdentifier to IRMASchedJobsRole
grant select on dbo.AttributeIdentifier to IRMASLIMRole
grant select on dbo.AttributeIdentifier to IRMAReportsRole
grant select on dbo.AvgCostAdjReason TO IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole 
grant select on dbo.AvgCostComps to IRMAClientRole
grant select on dbo.AvgCostComps to IRMAReportsRole
grant select on dbo.AvgCostComps to IRMASchedJobsRole
grant select on dbo.AvgCostFixProduce to IRMAClientRole
grant select on dbo.AvgCostFixProduce to IRMAReportsRole
grant select on dbo.AvgCostFixProduce to IRMASchedJobsRole
grant select on dbo.AvgCostHistory to IRMAClientRole
grant select on dbo.AvgCostHistory to IRMAReportsRole
grant select on dbo.AvgCostHistory to IRMASchedJobsRole
grant select on dbo.AvgCostQueue to IRMAClientRole
grant select on dbo.AvgCostQueue to IRMAReportsRole
grant select on dbo.AvgCostQueue to IRMASchedJobsRole
grant select on dbo.Buggy_Fact to IRMAClientRole
grant select on dbo.Buggy_Fact to IRMAReportsRole
grant select on dbo.Buggy_Fact to IRMASchedJobsRole
grant select on dbo.Buggy_Load to IRMAClientRole
grant select on dbo.Buggy_Load to IRMAReportsRole
grant select on dbo.Buggy_Load to IRMASchedJobsRole
grant select on dbo.Buggy_SumByCashier to IRMAClientRole
grant select on dbo.Buggy_SumByCashier to IRMAReportsRole
grant select on dbo.Buggy_SumByCashier to IRMASchedJobsRole
grant select on dbo.Buggy_SumByRegister to IRMAClientRole
grant select on dbo.Buggy_SumByRegister to IRMAReportsRole
grant select on dbo.Buggy_SumByRegister to IRMASchedJobsRole
grant select on dbo.BulkPromoPush to IRMAClientRole
grant select on dbo.BulkPromoPush to IRMAReportsRole
grant select on dbo.BulkPromoPush to IRMASchedJobsRole
grant select on dbo.CompetitivePriceType to IRMAClientRole
grant select on dbo.CompetitivePriceType to IRMAReportsRole
grant select on dbo.Competitor to IRMAClientRole
grant select on dbo.Competitor to IRMAReportsRole
grant select on dbo.CompetitorImportInfo to IRMAClientRole
grant select on dbo.CompetitorImportInfo to IRMAReportsRole
grant select on dbo.CompetitorImportSession to IRMAClientRole
grant select on dbo.CompetitorImportSession to IRMAReportsRole
grant select on dbo.CompetitorLocation to IRMAClientRole
grant select on dbo.CompetitorLocation to IRMAReportsRole
grant select on dbo.CompetitorPrice to IRMAClientRole
grant select on dbo.CompetitorPrice to IRMAReportsRole
grant select on dbo.CompetitorStore to IRMAClientRole
grant select on dbo.CompetitorStore to IRMAReportsRole
grant select on dbo.CompetitorStoreIdentifier to IRMAClientRole
grant select on dbo.CompetitorStoreIdentifier to IRMAReportsRole
grant select on dbo.CompetitorStoreItemIdentifier to IRMAClientRole
grant select on dbo.CompetitorStoreItemIdentifier to IRMAReportsRole
grant select on dbo.Contact to IRMAClientRole
grant select on dbo.Contact to IRMAReportsRole
grant select on dbo.Contact to IRMASchedJobsRole
grant select on dbo.conversion_runmode to IRMAReportsRole, IMHARole
grant select on dbo.CostImports to IRMAClientRole
grant select on dbo.CostImports to IRMAReportsRole
grant select on dbo.CostImports to IRMASchedJobsRole
grant select on dbo.CostPromoCodeType to IMHARole
grant select on dbo.CostPromoCodeType to IRMAReportsRole
grant select on dbo.CreditReasons to IRMAClientRole
grant select on dbo.CreditReasons to IRMAReportsRole
grant select on dbo.CreditReasons to IRMASchedJobsRole
grant select on dbo.Customer to IRMAClientRole
grant select on dbo.Customer to IRMAReportsRole
grant select on dbo.Customer to IRMASchedJobsRole
grant select on dbo.CustomerReturnReason to IRMAClientRole
grant select on dbo.CustomerReturnReason to IRMAReportsRole
grant select on dbo.CustomerReturnReason to IRMASchedJobsRole
grant select on dbo.CycleCountExternalLoad to IRMAClientRole
grant select on dbo.CycleCountExternalLoad to IRMAReportsRole
grant select on dbo.CycleCountExternalLoad to IRMASchedJobsRole
grant select on dbo.CycleCountHeader to IRMAClientRole
grant select on dbo.CycleCountHeader to IRMAReportsRole
grant select on dbo.CycleCountHeader to IRMASchedJobsRole
grant select on dbo.CycleCountHistory to IRMAClientRole
grant select on dbo.CycleCountHistory to IRMAReportsRole
grant select on dbo.CycleCountHistory to IRMASchedJobsRole
grant select on dbo.CycleCountItems to IRMAClientRole
grant select on dbo.CycleCountItems to IRMAReportsRole
grant select on dbo.CycleCountItems to IRMASchedJobsRole
grant select on dbo.CycleCountMaster to IRMAClientRole
grant select on dbo.CycleCountMaster to IRMAReportsRole
grant select on dbo.CycleCountMaster to IRMASchedJobsRole
grant select on dbo.CycleCountVendor to IRMAClientRole
grant select on dbo.CycleCountVendor to IRMAReportsRole
grant select on dbo.CycleCountVendor to IRMASchedJobsRole
grant select on dbo.Date to IRMAClientRole
grant select on dbo.Date to IRMAReportsRole
grant select on dbo.Date to IRMASchedJobsRole
grant select on dbo.DC_AVGCOST TO IRMADCAnalysisRole
grant select on dbo.DC_AVGCOST TO IRMAReportsRole
grant select on dbo.DC_EXE_PROCESS_STAGING TO IRMADCAnalysisRole
grant select on dbo.DC_EXE_PROCESS_STAGING TO IRMAReportsRole
grant select on dbo.DC_ITEM TO IRMADCAnalysisRole
grant select on dbo.DC_ITEM TO IRMAReportsRole
grant select on dbo.DC_ITEMHISTORY TO IRMADCAnalysisRole
grant select on dbo.DC_ITEMHISTORY TO IRMAReportsRole
grant select on dbo.DC_ONHAND TO IRMADCAnalysisRole
grant select on dbo.DC_ONHAND TO IRMAReportsRole
grant select on dbo.DC_VENDOR_COST TO IRMADCAnalysisRole
grant select on dbo.DC_VENDOR_COST TO IRMAReportsRole
grant select on dbo.ddl_log to IRMAClientRole
grant select on dbo.ddl_log to IRMASchedJobsRole
grant select on dbo.DeletedOrder to IRMAClientRole
grant select on dbo.DeletedOrder to IRMAReportsRole
grant select on dbo.DeletedOrder to IRMASchedJobsRole
grant select on dbo.DistSubTeam to IRMAClientRole
grant select on dbo.DistSubTeam to IRMAReportsRole
grant select on dbo.DistSubTeam to IRMASchedJobsRole
grant select on dbo.DistSubTeam to IRMASLIMRole
grant select on dbo.DSDVendorStore to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EIM_Jurisdiction_ItemScaleView to IRMAClientRole
grant select on dbo.EIM_Jurisdiction_ItemView to IRMAClientRole
grant select on dbo.EInvoicing_Item to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.Einvoicing_Header to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EInvoicing_Logging to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EInvoicing_Config to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EInvoicing_ErrorHistory to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EInvoicing_ErrorCodes to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EInvoicing_SummaryData to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EInvoicing_SACTypes to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EInvoicing_Invoices to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EInvoicing_HeaderData to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.EInvoicing_ItemData to IRMAReportsRole,IRMASchedJobsRole
grant select on dbo.ExRule_AutoOrdersNoTitle to IRMAAVCIRole
grant select on dbo.ExRule_AutoOrdersNoTitle to IRMAClientRole
grant select on dbo.ExRule_AutoOrdersNoTitle to IRMAReportsRole
grant select on dbo.ExRule_AutoOrdersNoTitle to IRMASchedJobsRole
grant select on dbo.ExRule_VendCostDIff to IRMAAVCIRole
grant select on dbo.ExRule_VendCostDIff to IRMAClientRole
grant select on dbo.ExRule_VendCostDIff to IRMAReportsRole
grant select on dbo.ExRule_VendCostDIff to IRMASchedJobsRole
grant select on dbo.ExRule_VendCostPackSize to IRMAAVCIRole
grant select on dbo.ExRule_VendCostPackSize to IRMAClientRole
grant select on dbo.ExRule_VendCostPackSize to IRMAReportsRole
grant select on dbo.ExRule_VendCostPackSize to IRMASchedJobsRole
grant select on dbo.ExSeverityDef to IRMAClientRole
grant select on dbo.ExSeverityDef to IRMAReportsRole
grant select on dbo.ExSeverityDef to IRMASchedJobsRole
grant select on dbo.FiscalWeek to IRMAClientRole
grant select on dbo.FiscalWeek to IRMAReportsRole
grant select on dbo.FSCustomer to IRMAClientRole
grant select on dbo.FSCustomer to IRMAReportsRole
grant select on dbo.FSCustomer to IRMASchedJobsRole
grant select on dbo.FSOrganization to IRMAClientRole
grant select on dbo.FSOrganization to IRMAReportsRole
grant select on dbo.FSOrganization to IRMASchedJobsRole
grant select on dbo.GLPushHistory to IRMAClientRole
grant select on dbo.GLPushHistory to IRMAReportsRole
grant select on dbo.GLPushHistory to IRMASchedJobsRole
grant select on dbo.GLPushQueue to IRMAClientRole
grant select on dbo.GLPushQueue to IRMAReportsRole
grant select on dbo.GLPushQueue to IRMASchedJobsRole
grant select on dbo.InstanceData to IRMAClientRole
grant select on dbo.InstanceData to IRMAExcelRole
grant select on dbo.InstanceData to IRMAReportsRole
grant select on dbo.InstanceData to IRMASchedJobsRole
grant select on dbo.InstanceDataFlags to IRMAClientRole
grant select on dbo.InstanceDataFlags to IRMAExcelRole
grant select on dbo.InstanceDataFlags to IRMAReportsRole
grant select on dbo.InstanceDataFlags to IRMASchedJobsRole
grant select on dbo.InstanceDataFlagsStoreOverride to IRMAClientRole
grant select on dbo.InstanceDataFlagsStoreOverride to IRMAExcelRole
grant select on dbo.InstanceDataFlagsStoreOverride to IRMAReportsRole
grant select on dbo.InstanceDataFlagsStoreOverride to IRMASchedJobsRole
grant select on dbo.InventoryLocation to IRMAClientRole
grant select on dbo.InventoryLocation to IRMAReportsRole
grant select on dbo.InventoryLocation to IRMASchedJobsRole
grant select on dbo.InventoryLocationItems to IRMAClientRole
grant select on dbo.InventoryLocationItems to IRMAReportsRole
grant select on dbo.InventoryLocationItems to IRMASchedJobsRole
grant select on dbo.InventoryServiceExportLoad to IRMAClientRole
grant select on dbo.InventoryServiceExportLoad to IRMAReportsRole
grant select on dbo.InventoryServiceExportLoad to IRMASchedJobsRole
grant select on dbo.InventoryServiceImportLoad to IRMAClientRole
grant select on dbo.InventoryServiceImportLoad to IRMAReportsRole
grant select on dbo.InventoryServiceImportLoad to IRMASchedJobsRole
grant select on dbo.IRISKeyToIRMAKey to IRMAClientRole
grant select on dbo.IRISKeyToIRMAKey to IRMASchedJobsRole
grant select on dbo.Item to ExtractRole
grant select on dbo.Item to IMHARole
grant select on dbo.Item to IRMAAVCIRole
grant select on dbo.Item to IRMAClientRole
grant select on dbo.Item to IRMAExcelRole
grant select on dbo.Item to IRMAPromoRole
grant select on dbo.Item to IRMAReportsRole
grant select on dbo.Item to IRMASchedJobsRole
grant select on dbo.ItemAdjustment to IRMAClientRole
grant select on dbo.ItemAdjustment to IRMAReportsRole
grant select on dbo.ItemAdjustment to IRMASchedJobsRole
grant select on dbo.ItemAttribute to IRMAReportsRole
grant select on dbo.ItemAttribute to IRMASchedJobsRole
grant select on dbo.ItemBrand to ExtractRole
grant select on dbo.ItemBrand to IMHARole
grant select on dbo.ItemBrand to IRMAAdminRole
grant select on dbo.ItemBrand to IRMAClientRole
grant select on dbo.ItemBrand to IRMAPromoRole
grant select on dbo.ItemBrand to IRMAReportsRole
grant select on dbo.ItemBrand to IRMASchedJobsRole
grant select on dbo.ItemCategory to IRMAClientRole
grant select on dbo.ItemCategory to IRMAExcelRole
grant select on dbo.ItemCategory to IRMAPromoRole
grant select on dbo.ItemCategory to IRMAReportsRole
grant select on dbo.ItemCategory to IRMASchedJobsRole
grant select on dbo.ItemCategory to IRMASLIMRole
grant select on dbo.ItemChain to IRMAClientRole
grant select on dbo.ItemChain to IRMAReportsRole
grant select on dbo.ItemChainItem to IRMAClientRole
grant select on dbo.ItemChainItem to IRMAReportsRole
grant select on dbo.ItemChangeHistory to IRMAClientRole
grant select on dbo.ItemChangeHistory to IRMAReportsRole
grant select on dbo.ItemChangeHistory to IRMASchedJobsRole
grant select on dbo.ItemChgType to IRMAClientRole
grant select on dbo.ItemChgType to IRMAReportsRole
grant select on dbo.ItemChgType to IRMASchedJobsRole
grant select on dbo.ItemConversion to IRMAClientRole
grant select on dbo.ItemConversion to IRMAReportsRole
grant select on dbo.ItemConversion to IRMASchedJobsRole
grant select on dbo.ItemDefaultAttribute to IRMAClientRole
grant select on dbo.ItemDefaultValue to IRMAClientRole
grant select on dbo.ItemGroup to IRMAClientRole
grant select on dbo.ItemGroup to IRMAReportsRole
grant select on dbo.ItemGroup to IRMASchedJobsRole
grant select on dbo.ItemGroupHistory to IRMAClientRole
grant select on dbo.ItemGroupHistory to IRMAReportsRole
grant select on dbo.ItemGroupHistory to IRMASchedJobsRole
grant select on dbo.ItemGroupMembers to IRMAClientRole
grant select on dbo.ItemGroupMembers to IRMAReportsRole
grant select on dbo.ItemGroupMembers to IRMASchedJobsRole
grant select on dbo.ItemGroupMembersHistory to IRMAClientRole
grant select on dbo.ItemGroupMembersHistory to IRMAReportsRole
grant select on dbo.ItemGroupMembersHistory to IRMASchedJobsRole
grant select on dbo.ItemHistory to IRMAClientRole
grant select on dbo.ItemHistory to IRMAReportsRole
grant select on dbo.ItemHistory to IRMASchedJobsRole
grant select on dbo.ItemIdentifier to ExtractRole
grant select on dbo.ItemIdentifier to IMHARole
grant select on dbo.ItemIdentifier to IRMAAdminRole
grant select on dbo.ItemIdentifier to IRMAAVCIRole
grant select on dbo.ItemIdentifier to IRMAClientRole
grant select on dbo.ItemIdentifier to IRMAExcelRole
grant select on dbo.ItemIdentifier to IRMAPromoRole
grant select on dbo.ItemIdentifier to IRMAReportsRole
grant select on dbo.ItemIdentifier to IRMASchedJobsRole
grant select on dbo.ItemManager to IMHARole
grant select on dbo.ItemManager to IRMAAVCIRole
grant select on dbo.ItemManager to IRMAClientRole
grant select on dbo.ItemManager to IRMAPromoRole
grant select on dbo.ItemManager to IRMAReportsRole
grant select on dbo.ItemManager to IRMASchedJobsRole
grant select on dbo.ItemOnOrder to IRMAClientRole
grant select on dbo.ItemOnOrder to IRMAReportsRole
grant select on dbo.ItemOnOrder to IRMASchedJobsRole
grant select on dbo.ItemOrigin to IRMAClientRole
grant select on dbo.ItemOrigin to IRMAReportsRole
grant select on dbo.ItemOrigin to IRMASchedJobsRole
grant select on dbo.ItemOrigin to IRMASLIMRole
grant select on dbo.ItemOverride to IRMAAdminRole
grant select on dbo.ItemOverride to IRMAClientRole
grant select on dbo.ItemOverride to IRMAReportsRole
grant select on dbo.ItemOverride to IRMASchedJobsRole
grant select on dbo.ItemScale to IRMAAdminRole
grant select on dbo.ItemScale TO IRMAClientRole
grant select on dbo.ItemScale to IRMAReportsRole
grant select on dbo.ItemScale to IRMASchedJobsRole
grant select on dbo.ItemScaleOverride to IRMAAdminRole
grant select on dbo.ItemScaleOverride TO IRMAClientRole
grant select on dbo.ItemScaleOverride to IRMAReportsRole
grant select on dbo.ItemScaleOverride to IRMASchedJobsRole
grant select on dbo.ItemShelfLife to IRMAClientRole
grant select on dbo.ItemShelfLife to IRMAReportsRole
grant select on dbo.ItemShelfLife to IRMASchedJobsRole
grant select on dbo.ItemType to IRMAClientRole
grant select on dbo.ItemType to IRMAReportsRole
grant select on dbo.ItemType to IRMASchedJobsRole
grant select on dbo.ItemUnit to ExtractRole
grant select on dbo.ItemUnit to IMHARole
grant select on dbo.ItemUnit to IRMAAdminRole
grant select on dbo.ItemUnit to IRMAClientRole
grant select on dbo.ItemUnit to IRMAPromoRole
grant select on dbo.ItemUnit to IRMAReportsRole
grant select on dbo.ItemUnit to IRMASchedJobsRole
grant select on dbo.ItemUnit to IRMASLIMRole
grant select on dbo.ItemUnit to IConInterface
grant select on dbo.ItemUomOverride to IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole, IRSUser
grant select on dbo.ItemVendor to ExtractRole
grant select on dbo.ItemVendor to IMHARole
grant select on dbo.ItemVendor to IRMAAdminRole
grant select on dbo.ItemVendor to IRMAClientRole
grant select on dbo.ItemVendor to IRMAExcelRole
grant select on dbo.ItemVendor to IRMAPromoRole
grant select on dbo.ItemVendor to IRMAReportsRole
grant select on dbo.ItemVendor to IRMASchedJobsRole
grant select on dbo.JobStatus to IRMAReportsRole, IconInterface
grant select on dbo.KitchenRoute to IRMAClientRole
grant select on dbo.KitchenRoute to IRMAClientRole
grant select on dbo.LabelType to IRMAClientRole
grant select on dbo.LabelType to IRMAReportsRole
grant select on dbo.LabelType to IRMASchedJobsRole
grant select on dbo.LabelType to IRMASLIMRole
grant select on dbo.LabelType to IRMASLIMRole
grant select on dbo.LastVendor to IRMAClientRole
grant select on dbo.LastVendor to IRMAReportsRole
grant select on dbo.LastVendor to IRMASchedJobsRole
grant select on dbo.MenuAccess to IRMAClientRole
grant select on dbo.MobilePrinter to IRMAClientRole
grant select on dbo.MobilePrinter to IRMAReportsRole
grant select on dbo.MobilePrinter to IRMASchedJobsRole
grant select on dbo.NatHier_Category to IRMAClientRole
grant select on dbo.NatHier_Category to IRMAReportsRole
grant select on dbo.NatHier_Category to IRMASchedJobsRole
grant select on dbo.NatHier_Class to IRMAClientRole
grant select on dbo.NatHier_Class to IRMAReportsRole
grant select on dbo.NatHier_Class to IRMASchedJobsRole
grant select on dbo.NatHier_Family to IRMAClientRole
grant select on dbo.NatHier_Family to IRMAReportsRole
grant select on dbo.NatHier_Family to IRMASchedJobsRole
grant select on dbo.NatItemCat to ExtractRole
grant select on dbo.NatItemCat to IMHARole
grant select on dbo.NatItemCat to IRMAClientRole
grant select on dbo.NatItemCat to IRMAReportsRole
grant select on dbo.NatItemCat to IRMASchedJobsRole
grant select on dbo.NatItemCat to IRMASLIMRole
grant select on dbo.NatItemClass to ExtractRole
grant select on dbo.NatItemClass to IMHARole
grant select on dbo.NatItemClass to IRMAClientRole
grant select on dbo.NatItemClass to IRMAReportsRole
grant select on dbo.NatItemClass to IRMASchedJobsRole
grant select on dbo.NatItemClass to IRMASLIMRole
grant select on dbo.NatItemFamily to ExtractRole
grant select on dbo.NatItemFamily to IMHARole
grant select on dbo.NatItemFamily to IRMAClientRole
grant select on dbo.NatItemFamily to IRMAReportsRole
grant select on dbo.NatItemFamily to IRMASchedJobsRole
grant select on dbo.NatItemFamily to IRMASLIMRole
grant select on dbo.NewItemsLoad to IRMAClientRole
grant select on dbo.NewItemsLoad to IRMAExcelRole
grant select on dbo.NewItemsLoad to IRMAReportsRole
grant select on dbo.NewItemsLoad to IRMASchedJobsRole
grant select on dbo.NewMrkDwn to IRMAClientRole
grant select on dbo.NewMrkDwn to IRMAReportsRole
grant select on dbo.NewMrkDwn to IRMASchedJobsRole
grant select on dbo.Nutrifacts to IRMAAdminRole
grant select on dbo.Nutrifacts to IRMAClientRole
grant select on dbo.Nutrifacts to IRMAReportsRole
grant select, update on dbo.Nutrifacts to IRMASchedJobsRole
grant select on dbo.ODBCErrorLog to IRMAClientRole
grant select on dbo.ODBCErrorLog to IRMAReportsRole
grant select on dbo.ODBCErrorLog to IRMASchedJobsRole
grant select on dbo.OfferChgType to IRMAClientRole
grant select on dbo.OfferChgType to IRMAReportsRole
grant select on dbo.OfferChgType to IRMASchedJobsRole
grant select on dbo.OnHand to IRMAClientRole
grant select on dbo.OnHand to IRMAReportsRole
grant select on dbo.OnHand to IRMASchedJobsRole
grant select on dbo.OrderExportDeletedQueue to IRMAClientRole
grant select on dbo.OrderExportDeletedQueue to IRMAReportsRole
grant select on dbo.OrderExportDeletedQueue to IRMASchedJobsRole
grant select on dbo.OrderExportQueue to IRMAClientRole
grant select on dbo.OrderExportQueue to IRMAReportsRole
grant select on dbo.OrderExportQueue to IRMASchedJobsRole
grant select on dbo.OrderExportQueue to IRMASchedJobsRole
grant select on dbo.OrderHeader to IRMAClientRole
grant select on dbo.OrderHeader to IRMAReportsRole
grant select on dbo.OrderHeader to IRMASchedJobsRole
grant select on dbo.OrderHeaderHistory to IRMAClientRole
grant select on dbo.OrderHeaderHistory to IRMAReportsRole
grant select on dbo.OrderHeaderHistory to IRMASchedJobsRole
grant select on dbo.OrderInvoice to IRMAClientRole
grant select on dbo.OrderInvoice to IRMAReportsRole
grant select on dbo.OrderInvoice to IRMASchedJobsRole
grant select on dbo.OrderItem to IRMAClientRole
grant select on dbo.OrderItem to IRMAReportsRole
grant select on dbo.OrderItem to IRMASchedJobsRole
grant select on dbo.OrderItemRefused to IRMASchedJobsRole, IRMAReportsRole
grant select on dbo.OrderItemQueue to IRMAClientRole
grant select on dbo.OrderItemQueue to IRMAReportsRole
grant select on dbo.OrderItemQueue to IRMASchedJobsRole
grant select on dbo.Payment to IRMAClientRole
grant select on dbo.Payment to IRMAReportsRole
grant select on dbo.Payment to IRMASchedJobsRole
grant select on dbo.Payment_Fact to IRMAClientRole
grant select on dbo.Payment_Fact to IRMAReportsRole
grant select on dbo.Payment_Fact to IRMASchedJobsRole
grant select on dbo.Payment_Load to IRMAClientRole
grant select on dbo.Payment_Load to IRMAReportsRole
grant select on dbo.Payment_Load to IRMASchedJobsRole
grant select on dbo.Payment_SumByRegister to IRMAClientRole
grant select on dbo.Payment_SumByRegister to IRMAReportsRole
grant select on dbo.Payment_SumByRegister to IRMASchedJobsRole
grant select on dbo.PaymentGroup to IRMAClientRole
grant select on dbo.PaymentGroup to IRMAReportsRole
grant select on dbo.PaymentGroup to IRMASchedJobsRole
grant select on dbo.PLUMCorpChgQueue to IRMAClientRole
grant select on dbo.PLUMCorpChgQueue to IRMAReportsRole
grant select on dbo.PLUMCorpChgQueue to IRMASchedJobsRole
grant select on dbo.PLUMCorpChgQueueTmp to IRMAClientRole
grant select on dbo.PLUMCorpChgQueueTmp to IRMAReportsRole
grant select on dbo.PLUMCorpChgQueueTmp to IRMASchedJobsRole
grant select on dbo.PMExcludedItem to IRMAClientRole
grant select on dbo.PMExcludedItem to IRMAReportsRole
grant select on dbo.PMExcludedItem to IRMASchedJobsRole
grant select on dbo.PMOrganizationChg to IRMAClientRole
grant select on dbo.PMOrganizationChg to IRMAReportsRole
grant select on dbo.PMOrganizationChg to IRMASchedJobsRole
grant select on dbo.PMOrganizationChgQueue to IRMAClientRole
grant select on dbo.PMOrganizationChgQueue to IRMAReportsRole
grant select on dbo.PMOrganizationChgQueue to IRMASchedJobsRole
grant select on dbo.PMPriceChange to IRMAClientRole
grant select on dbo.PMPriceChange to IRMAReportsRole
grant select on dbo.PMPriceChange to IRMASchedJobsRole
grant select on dbo.PMPriceChangeLoad to IRMAClientRole
grant select on dbo.PMPriceChangeLoad to IRMAReportsRole
grant select on dbo.PMPriceChangeLoad to IRMASchedJobsRole
grant select on dbo.PMProductChg to IRMAClientRole
grant select on dbo.PMProductChg to IRMAReportsRole
grant select on dbo.PMProductChg to IRMASchedJobsRole
grant select on dbo.PMProductChgQueue to IRMAClientRole
grant select on dbo.PMProductChgQueue to IRMAReportsRole
grant select on dbo.PMProductChgQueue to IRMASchedJobsRole
grant select on dbo.PMSalesHistory_Temp to IRMAClientRole
grant select on dbo.PMSalesHistory_Temp to IRMAReportsRole
grant select on dbo.PMSalesHistory_Temp to IRMASchedJobsRole
grant select on dbo.PMSubTeamInclude to IRMAClientRole
grant select on dbo.PMSubTeamInclude to IRMAReportsRole
grant select on dbo.PMSubTeamInclude to IRMASchedJobsRole
grant select on dbo.POSAuditException to IRMAReportsRole
grant select on dbo.POSAuditException to IRMAAdminRole
grant select on dbo.POSAuditException to IRMASchedJobsRole
grant select on dbo.POSAuditExceptionType to IRMAAdminRole
grant select on dbo.POSAuditExceptionType to IRMASchedJobsRole
grant select on dbo.POSAuditExceptionType to IRMAReportsRole
grant select on dbo.POSChanges to IRMAClientRole
grant select on dbo.POSChanges to IRMAReportsRole
grant select on dbo.POSChanges to IRMASchedJobsRole
grant select on dbo.POSChangesSave to IRMAClientRole
grant select on dbo.POSChangesSave to IRMAReportsRole
grant select on dbo.POSChangesSave to IRMASchedJobsRole
grant select on dbo.POSChangeType to IRMAClientRole
grant select on dbo.POSChangeType to IRMAReportsRole
grant select on dbo.POSChangeType to IRMASchedJobsRole
grant select on dbo.POSDataElement to IRMAClientRole
grant select on dbo.POSDataElement to IRMAReportsRole
grant select on dbo.POSDataElement to IRMASchedJobsRole
grant select on dbo.POSDataTypes to IRMAClientRole
grant select on dbo.POSDataTypes to IRMAReportsRole
grant select on dbo.POSDataTypes to IRMASchedJobsRole
grant select on dbo.POSItem to IRMAClientRole
grant select on dbo.POSItem to IRMAReportsRole
grant select on dbo.POSItem to IRMASchedJobsRole
grant select on dbo.POSScan to IRMAClientRole
grant select on dbo.POSScan to IRMAReportsRole
grant select on dbo.POSScan to IRMASchedJobsRole
grant select on dbo.POSScan_Load to IRMAClientRole
grant select on dbo.POSScan_Load to IRMAReportsRole
grant select on dbo.POSScan_Load to IRMASchedJobsRole
grant select on dbo.POSSystemTypes to IRMASchedJobsRole
grant select on dbo.POSSystemTypes to IRMAReportsRole
grant select on dbo.POSWriter to IRMAClientRole
grant select on dbo.POSWriter to IRMAReportsRole
grant select on dbo.POSWriter to IRMASchedJobsRole
grant select on dbo.POSWriterDictionary to IRMAClientRole
grant select on dbo.POSWriterDictionary to IRMAReportsRole
grant select on dbo.POSWriterDictionary to IRMASchedJobsRole
grant select on dbo.POSWriterEscapeChars to IRMAClientRole
grant select on dbo.POSWriterEscapeChars to IRMAReportsRole
grant select on dbo.POSWriterEscapeChars to IRMASchedJobsRole
grant select on dbo.POSWriterFileConfig to IRMAClientRole
grant select on dbo.POSWriterFileConfig to IRMAReportsRole
grant select on dbo.POSWriterFileConfig to IRMASchedJobsRole
grant select on dbo.POSWriterPricingMethods to IRMAClientRole
grant select on dbo.POSWriterPricingMethods to IRMAReportsRole
grant select on dbo.POSWriterPricingMethods to IRMASchedJobsRole
grant select on dbo.Price to ExtractRole
grant select on dbo.Price to IMHARole
grant select on dbo.Price to IRMAClientRole
grant select on dbo.Price to IRMAExcelRole
grant select on dbo.Price to IRMAPromoRole
grant select on dbo.Price to IRMAReportsRole
grant select on dbo.Price to IRMASchedJobsRole
grant select on dbo.PriceBatchDetail to IMHARole
grant select on dbo.PriceBatchDetail to IRMAClientRole
grant select on dbo.PriceBatchDetail to IRMAReportsRole
grant select on dbo.PriceBatchDetail to IRMASchedJobsRole
grant select on dbo.PriceBatchHeader to IRMAClientRole
grant select on dbo.PriceBatchHeader to IRMAReportsRole
grant select on dbo.PriceBatchHeader to IRMASchedJobsRole
grant select on dbo.PriceBatchStatus to IRMAClientRole
grant select on dbo.PriceBatchStatus to IRMAReportsRole
grant select on dbo.PriceBatchStatus to IRMASchedJobsRole
grant select on dbo.PriceChgType to IMHARole
grant select on dbo.PriceChgType to IRMAAdminRole
grant select on dbo.PriceChgType to IRMAClientRole
grant select on dbo.PriceChgType to IRMAReportsRole
grant select on dbo.PriceChgType to IRMASchedJobsRole
grant select on dbo.PriceChgType to IRMASLIMRole
grant select on dbo.PriceHistory to ExtractRole
grant select on dbo.PriceHistory to IRMAClientRole
grant select on dbo.PriceHistory to IRMAReportsRole
grant select on dbo.PriceHistory to IRMASchedJobsRole
grant select on dbo.PricingMethod to IRMAClientRole
grant select on dbo.PricingMethod to IRMAReportsRole
grant select on dbo.PricingMethod to IRMASchedJobsRole
grant select on dbo.ProdHierarchyLevel3 to IRMAClientRole
grant select on dbo.ProdHierarchyLevel3 to IRMAReportsRole
grant select on dbo.ProdHierarchyLevel4 to IRMAClientRole
grant select on dbo.ProdHierarchyLevel4 to IRMAClientRole
grant select on dbo.ProdHierarchyLevel4 to IRMAReportsRole
grant select on dbo.ProduceAvgCostFix to IRMAClientRole
grant select on dbo.ProduceAvgCostFix to IRMAReportsRole
grant select on dbo.ProduceAvgCostFix to IRMASchedJobsRole
grant select on dbo.PromotionalOffer to IRMAClientRole
grant select on dbo.PromotionalOffer to IRMAReportsRole
grant select on dbo.PromotionalOffer to IRMASchedJobsRole
grant select on dbo.PromotionalOfferHistory to IRMAClientRole
grant select on dbo.PromotionalOfferHistory to IRMAReportsRole
grant select on dbo.PromotionalOfferHistory to IRMASchedJobsRole
grant select on dbo.PromotionalOfferMembers to IRMAClientRole
grant select on dbo.PromotionalOfferMembers to IRMAReportsRole
grant select on dbo.PromotionalOfferMembers to IRMASchedJobsRole
grant select on dbo.PromotionalOfferMembersHistory to IRMAClientRole
grant select on dbo.PromotionalOfferMembersHistory to IRMAReportsRole
grant select on dbo.PromotionalOfferMembersHistory to IRMASchedJobsRole
grant select on dbo.PromotionalOfferStore to IRMAClientRole
grant select on dbo.PromotionalOfferStore to IRMAReportsRole
grant select on dbo.PromotionalOfferStore to IRMASchedJobsRole
grant select on dbo.PromotionalOfferStoreHistory to IRMAClientRole
grant select on dbo.PromotionalOfferStoreHistory to IRMAReportsRole
grant select on dbo.PromotionalOfferStoreHistory to IRMASchedJobsRole
grant select on dbo.Purchases to IRMAClientRole
grant select on dbo.Purchases to IRMAReportsRole
grant select on dbo.Purchases to IRMASchedJobsRole
grant select on dbo.Region to IRMAClientRole
grant select on dbo.Region to IRMAReportsRole
grant select on dbo.Region to IRMASchedJobsRole
grant select on dbo.region to IRMASLIMRole
grant select on dbo.region to IRMASchedJobsRole
grant select on dbo.ReturnOrderList to IRMAClientRole
grant select on dbo.ReturnOrderList to IRMAReportsRole
grant select on dbo.ReturnOrderList to IRMASchedJobsRole
grant select on dbo.RewardType to IRMAClientRole
grant select on dbo.RewardType to IRMAReportsRole
grant select on dbo.RewardType to IRMASchedJobsRole
GRANT SELECT on dbo.RoleConflictReason to IRMAReportsRole
grant select on dbo.RuleDef to IRMAAVCIRole
grant select on dbo.RuleDef to IRMAClientRole
grant select on dbo.RuleDef to IRMAReportsRole
grant select on dbo.RuleDef to IRMASchedJobsRole
grant select on dbo.Sales to IRMAClientRole
grant select on dbo.Sales to IRMAReportsRole
grant select on dbo.Sales to IRMASchedJobsRole
grant select on dbo.Sales_Fact to IRMAClientRole
grant select on dbo.Sales_Fact to IRMAReportsRole
grant select on dbo.Sales_Fact to IRMASchedJobsRole
grant select on dbo.Sales_Load to IRMAClientRole
grant select on dbo.Sales_Load to IRMAReportsRole
grant select on dbo.Sales_Load to IRMASchedJobsRole
grant select on dbo.Sales_SumByItem to ExtractRole
grant select on dbo.Sales_SumByItem to IRMAClientRole
grant select on dbo.Sales_SumByItem to IRMAReportsRole
grant select on dbo.Sales_SumByItem to IRMASchedJobsRole
grant select on dbo.Sales_SumByItem to IRMASLIMRole
grant select on dbo.Sales_SumByItemWkly to IRMAReportsRole
grant select on dbo.Sales_SumBySubDept to IRMAClientRole
grant select on dbo.Sales_SumBySubDept to IRMAReportsRole
grant select on dbo.Sales_SumBySubDept to IRMASchedJobsRole
grant select on dbo.SalesExportQueue to IRMAClientRole
grant select on dbo.SalesExportQueue to IRMAReportsRole
grant select on dbo.SalesExportQueue to IRMASchedJobsRole
grant select on dbo.Scale_ExtraText to IRMAAdminRole
grant select on dbo.Scale_ExtraText to IRMAClientRole
grant select on dbo.Scale_ExtraText to IRMAReportsRole
grant select,update on dbo.Scale_ExtraText to IRMASchedJobsRole
grant select on dbo.Scale_Grade to IRMAAdminRole
grant select on dbo.Scale_Grade to IRMAClientRole
grant select on dbo.Scale_Grade to IRMAReportsRole
grant select on dbo.Scale_Grade to IRMASchedJobsRole
grant select on dbo.Scale_LabelStyle to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant select on dbo.Scale_LabelStyle to IRMASLIMRole
grant select on dbo.Scale_LabelType to IRMASLIMRole
grant select on dbo.Scale_RandomWeightType to IRMASLIMRole
grant select on dbo.Scale_Tare to IRMAAdminRole
grant select on dbo.Scale_Tare to IRMAClientRole
grant select on dbo.Scale_Tare to IRMAReportsRole
grant select on dbo.Scale_Tare to IRMASchedJobsRole
grant select on dbo.Scale_Tare to IRMASLIMRole
grant select on dbo.SignQueue to IRMAClientRole
grant select on dbo.SignQueue to IRMAReportsRole
grant select on dbo.SignQueue to IRMASchedJobsRole
grant select on dbo.SLIM_ItemAttributeView TO IRMAClientRole
grant select on dbo.SLIM_ItemIdentifierView TO IRMAClientRole
grant select on dbo.SLIM_ItemScaleView TO IRMAClientRole
grant select on dbo.SLIM_ItemVendorView TO IRMAClientRole
grant select on dbo.SLIM_ItemView TO IRMAClientRole
grant select on dbo.SLIM_PriceView TO IRMAClientRole
grant select on dbo.SLIM_Scale_ExtraTextView TO IRMAClientRole
grant select on dbo.SLIM_StoreItemVendorView TO IRMAClientRole
grant select on dbo.SLIM_StoreItemView TO IRMAClientRole
grant select on dbo.SLIM_VendorCostHistoryView TO IRMAClientRole
grant select on dbo.SLIM_VendorDealView TO IRMAClientRole
grant select on dbo.Store to ExtractRole
grant select on dbo.Store to IMHARole
grant select on dbo.Store to IRMAClientRole
grant select on dbo.Store to IRMAExcelRole
grant select on dbo.Store to IRMAPromoRole
grant select on dbo.Store to IRMAReportsRole
grant select on dbo.Store to IRMASchedJobsRole
grant select on dbo.Store to IRMASLIMRole
grant select on dbo.Store_MobilePrinter to IRMAClientRole
grant select on dbo.Store_MobilePrinter to IRMAReportsRole
grant select on dbo.Store_MobilePrinter to IRMASchedJobsRole
grant select on dbo.StoreCompetitorStore to IRMAClientRole
grant select on dbo.StoreCompetitorStore to IRMAReportsRole
grant select on dbo.StoreItem to ExtractRole
grant select on dbo.StoreItem to IMHARole
grant select on dbo.StoreItem to IRMAClientRole
grant select on dbo.StoreItem to IRMAClientRole
grant select on dbo.StoreItem to IRMAExcelRole
grant select on dbo.StoreItem to IRMAPromoRole
grant select on dbo.StoreItem to IRMAReportsRole
grant select on dbo.StoreItem to IRMAReportsRole
grant select on dbo.StoreItem to IRMASchedJobsRole
grant select on dbo.StoreItem to IRMASLIMRole
grant select on dbo.StoreItemAttribute to IRMAReportsRole
grant select on dbo.StoreItemVendor to ExtractRole
grant select on dbo.StoreItemVendor to IMHARole
grant select on dbo.StoreItemVendor to IRMAAdminRole
grant select on dbo.StoreItemVendor to IRMAAVCIRole
grant select on dbo.StoreItemVendor to IRMAClientRole
grant select on dbo.StoreItemVendor to IRMAExcelRole
grant select on dbo.StoreItemVendor to IRMAPromoRole
grant select on dbo.StoreItemVendor to IRMAPromoRole
grant select on dbo.StoreItemVendor to IRMAReportsRole
grant select on dbo.StoreItemVendor to IRMASchedJobsRole
grant select on dbo.StoreJurisdiction to IRMAClientRole
grant select on dbo.StorePOSConfig to IRMAClientRole
grant select on dbo.StorePOSConfig to IRMAReportsRole
grant select on dbo.StorePOSConfig to IRMASchedJobsRole
grant select on dbo.StoreRegionMapping to IRMAReportsRole, IMHARole, IRMASchedJobsRole
grant select on dbo.StoreShelfTagConfig to IRMAClientRole
grant select on dbo.StoreSubTeam to IRMAAVCIRole
grant select on dbo.StoreSubTeam to IMHARole
grant select on dbo.StoreSubTeam to IRMAClientRole
grant select on dbo.StoreSubTeam to IRMAReportsRole
grant select on dbo.StoreSubTeam to IRMASchedJobsRole
grant select on dbo.StoreSubTeam to IRMASLIMRole
grant select on dbo.SubTeam to ExtractRole
grant select on dbo.SubTeam to IMHARole
grant select on dbo.Subteam to IRMAAVCIRole
grant select on dbo.SubTeam to IRMAClientRole
grant select on dbo.SubTeam to IRMAPromoRole
grant select on dbo.SubTeam to IRMAReportsRole
grant select on dbo.SubTeam to IRMASchedJobsRole
grant select on dbo.SubTeam to IRMASLIMRole
grant select on dbo.SustainabilityRanking to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole, IRMASupportRole
grant select on dbo.sysobjects to IRMAAVCIRole
grant select on dbo.TaxDefinition to IRMAClientRole
grant select on dbo.TaxDefinition to IRMAReportsRole
grant select on dbo.TaxDefinition to IRMASchedJobsRole
grant select on dbo.TaxFlag to IRMAClientRole
grant select on dbo.TaxFlag to IRMAReportsRole
grant select on dbo.TaxFlag to IRMASchedJobsRole
grant select on dbo.TaxFlagChgQueue to IRMAClientRole
grant select on dbo.TaxFlagChgQueue to IRMAReportsRole
grant select on dbo.TaxFlagChgQueue to IRMASchedJobsRole
grant select on dbo.TaxJurisdiction to IRMAClientRole
grant select on dbo.TaxJurisdiction to IRMAReportsRole
grant select on dbo.TaxJurisdiction to IRMASchedJobsRole
grant select on dbo.TaxOverride to IRMAClientRole
grant select on dbo.TaxOverride to IRMAReportsRole
grant select on dbo.TaxOverride to IRMASchedJobsRole
grant select on dbo.Team to IRMAAVCIRole
grant select on dbo.Team to IRMAClientRole
grant select on dbo.Team to IRMAPromoRole
grant select on dbo.Team to IRMAReportsRole
grant select on dbo.Team to IRMASchedJobsRole
grant select on dbo.Team to IRMASLIMRole
grant select on dbo.Temp_PLUMIngredients to IRMAClientRole
grant select on dbo.Temp_PLUMIngredients to IRMASchedJobsRole
grant select on dbo.Temp_PriceAudit to IRMAClientRole
grant select on dbo.Temp_PriceAudit to IRMASchedJobsRole
grant select on dbo.TempDeliAvgCost to IRMAClientRole
grant select on dbo.TempDeliAvgCost to IRMASchedJobsRole
grant select on dbo.TempID to IRMAClientRole
grant select on dbo.TempID to IRMASchedJobsRole
grant select on dbo.TempLastVendor to IRMAClientRole
grant select on dbo.TempLastVendor to IRMASchedJobsRole
grant select on dbo.TempNewItemList to IRMAClientRole
grant select on dbo.TempNewItemList to IRMASchedJobsRole
grant select on dbo.TempSignQueue to IRMAClientRole
grant select on dbo.TempSignQueue to IRMASchedJobsRole
grant select on dbo.Time to IRMAClientRole
grant select on dbo.Time to IRMAReportsRole
grant select on dbo.Time to IRMASchedJobsRole
grant select on dbo.Title to IRMAClientRole
grant select on dbo.Title to IRMAPromoRole
grant select on dbo.Title to IRMAReportsRole
grant select on dbo.Title to IRMASchedJobsRole
grant select on dbo.Title to IRMASLIMRole
grant select on dbo.TLog_UK_Item to IRMAClientRole
grant select on dbo.TLog_UK_Item to IRMAReportsRole
grant select on dbo.TLog_UK_Item to IRMASchedJobsRole
grant select on dbo.TLog_UK_Offers to IRMAClientRole
grant select on dbo.TLog_UK_Offers to IRMAReportsRole
grant select on dbo.TLog_UK_Offers to IRMASchedJobsRole
grant select on dbo.TLog_UK_Payment to IRMAClientRole
grant select on dbo.TLog_UK_Payment to IRMAReportsRole
grant select on dbo.TLog_UK_Payment to IRMASchedJobsRole
grant select on dbo.TLog_UK_Transaction to IRMAClientRole
grant select on dbo.TLog_UK_Transaction to IRMAReportsRole
grant select on dbo.TLog_UK_Transaction to IRMASchedJobsRole
grant select on dbo.tmpAllergens to IRMAClientRole
grant select on dbo.tmpAllergens to IRMASchedJobsRole
grant select on dbo.tmpBulkOrderItemFix to IRMAClientRole
grant select on dbo.tmpBulkOrderItemFix to IRMASchedJobsRole
grant select on dbo.tmpCentralVendorUploadList to IRMAClientRole
grant select on dbo.tmpCentralVendorUploadList to IRMASchedJobsRole
grant select on dbo.tmpCostList to IRMAClientRole
grant select on dbo.tmpCostList to IRMASchedJobsRole
grant select on dbo.tmpDistSubTeam_Mapping  to IRMAClientRole
grant select on dbo.tmpDistSubTeam_Mapping  to IRMASchedJobsRole
grant select on dbo.tmpFloralItemFix to IRMAClientRole
grant select on dbo.tmpFloralItemFix to IRMASchedJobsRole
grant select on dbo.tmpIndexDefragList to IRMAClientRole
grant select on dbo.tmpIndexDefragList to IRMASchedJobsRole
grant select on dbo.tmpOrderExport to IRMAClientRole
grant select on dbo.tmpOrderExport to IRMASchedJobsRole
grant select on dbo.tmpOrderExportDeleted to IRMAClientRole
grant select on dbo.tmpOrderExportDeleted to IRMASchedJobsRole
grant select on dbo.tmpPOSRePush to IRMAClientRole
grant select on dbo.tmpPOSRePush to IRMASchedJobsRole
grant select on dbo.tmpPurchases to IRMAClientRole
grant select on dbo.tmpPurchases to IRMASchedJobsRole
grant select on dbo.tmpScanAccuracyCount to IRMAClientRole
grant select on dbo.tmpScanAccuracyCount to IRMASchedJobsRole
grant select on dbo.tmpScanAccuracyList to IRMAClientRole
grant select on dbo.tmpScanAccuracyList to IRMASchedJobsRole
grant select on dbo.TmpTestVendorCostImport to IRMAClientRole
grant select on dbo.TmpTestVendorCostImport to IRMASchedJobsRole
grant select on dbo.tmpVendExport to IRMAClientRole
grant select on dbo.tmpVendExport to IRMASchedJobsRole
grant select on dbo.tmpWholeBodyRetails to IRMAClientRole
grant select on dbo.tmpWholeBodyRetails to IRMASchedJobsRole
grant select on dbo.UOM_Conversion to IRMAClientRole
grant select on dbo.UploadAttribute to IRMAClientRole
grant select on dbo.UploadRow to IRMAClientRole
grant select on dbo.UploadSession to IRMAClientRole
grant select on dbo.UploadSessionUploadType to IRMAClientRole
grant select on dbo.UploadSessionUploadTypeStore to IRMAClientRole
grant select on dbo.UploadType to IRMAClientRole
grant select on dbo.UploadTypeAttribute to IRMAClientRole
grant select on dbo.UploadTypeTemplate to IRMAClientRole
grant select on dbo.UploadTypeTemplateAttribute to IRMAClientRole
grant select on dbo.UploadValue to IRMAClientRole
grant select on dbo.Users to IRMAAVCIRole
grant select on dbo.Users to IRMAClientRole
grant select on dbo.Users to IRMAPromoRole
grant select on dbo.Users to IRMAReportsRole
grant select on dbo.Users to IRMASchedJobsRole
grant select on dbo.Users to IRMASLIMRole
grant select on dbo.UsersHistory to IRMAClientRole
grant select on dbo.UsersHistory to IRMAReportsRole
grant select on dbo.UsersHistory to IRMASchedJobsRole
grant select on dbo.UsersSubTeam to IRMAClientRole
grant select on dbo.UsersSubTeam to IRMAPromoRole
grant select on dbo.UsersSubTeam to IRMAReportsRole
grant select on dbo.UsersSubTeam to IRMASchedJobsRole
grant select on dbo.UserStoreTeamTitle to IRMAAVCIRole
grant select on dbo.UserStoreTeamTitle to IRMAClientRole
grant select on dbo.UserStoreTeamTitle to IRMAPromoRole
grant select on dbo.UserStoreTeamTitle to IRMAReportsRole
grant select on dbo.UserStoreTeamTitle to IRMASchedJobsRole
grant select on dbo.Vendor to ExtractRole
grant select on dbo.Vendor to IMHARole
grant select on dbo.Vendor to IRMAAdminRole
grant select on dbo.vendor to IRMAAVCIRole
grant select on dbo.Vendor to IRMAClientRole
grant select on dbo.Vendor to IRMAExcelRole
grant select on dbo.Vendor to IRMAPromoRole
grant select on dbo.Vendor to IRMAReportsRole
grant select on dbo.Vendor to IRMASchedJobsRole
grant select on dbo.VendorCostHistory to IMHARole
grant select on dbo.VendorCostHistory to IRMAClientRole
grant select on dbo.VendorCostHistory to IRMAPromoRole
grant select on dbo.VendorCostHistory to IRMAReportsRole
grant select on dbo.VendorCostHistory to IRMASchedJobsRole
grant select on dbo.VendorCostHistoryExceptions to IRMAClientRole
grant select on dbo.VendorCostHistoryExceptions to IRMAReportsRole
grant select on dbo.VendorCostHistoryExceptions to IRMASchedJobsRole
grant select on dbo.VendorDealHistory to IMHARole
grant select on dbo.VendorDealHistory to IRMAClientRole
grant select on dbo.VendorDealHistory to IRMAReportsRole
grant select on dbo.VendorDealHistory to IRMASchedJobsRole
grant select on dbo.VendorDealType to IMHARole
grant select on dbo.VendorDealType to IRMAClientRole
grant select on dbo.VendorDealType to IRMAReportsRole
grant select on dbo.VendorDealType to IRMASchedJobsRole
grant select on dbo.VendorExportQueue to IRMAClientRole
grant select on dbo.VendorExportQueue to IRMAReportsRole
grant select on dbo.VendorExportQueue to IRMASchedJobsRole
grant select on dbo.VendorStoreFreight to IRMAAVCIRole
grant select on dbo.VendorStoreFreight to IRMAClientRole
grant select on dbo.VendorStoreFreight to IRMAReportsRole
grant select on dbo.VendorStoreFreight to IRMASchedJobsRole
grant select on dbo.Version to IRMAClientRole
grant select on dbo.Version to IRMAReportsRole
grant select on dbo.Version to IRMASchedJobsRole
grant select on dbo.version to IRMASLIMRole
grant select on dbo.VIMItemRegion to IRMAClientRole
grant select on dbo.VIMItemRegion to IRMAReportsRole
grant select on dbo.VIMItemRegion to IRMASchedJobsRole
grant select on dbo.VIMRetailPriceLoad to IRMAClientRole
grant select on dbo.VIMRetailPriceLoad to IRMAReportsRole
grant select on dbo.VIMRetailPriceLoad to IRMASchedJobsRole
grant select on dbo.VIMVendorCostFileLoad to IRMAClientRole
grant select on dbo.VIMVendorCostFileLoad to IRMAReportsRole
grant select on dbo.VIMVendorCostFileLoad to IRMASchedJobsRole
grant select on dbo.WarehouseItemChange to IRMAClientRole
grant select on dbo.WarehouseItemChange to IRMAReportsRole
grant select on dbo.WarehouseItemChange to IRMASchedJobsRole
grant select on dbo.WarehouseVendorChange to IRMAClientRole
grant select on dbo.WarehouseVendorChange to IRMAReportsRole
grant select on dbo.WarehouseVendorChange to IRMASchedJobsRole
grant select on dbo.Z_SQLGUARD_Reaction to IRMAClientRole
grant select on dbo.Z_SQLGUARD_Reaction to IRMASchedJobsRole
grant select on dbo.Zone to ExtractRole
grant select on dbo.Zone to IMHARole
grant select on dbo.Zone to IRMAClientRole
grant select on dbo.Zone to IRMAExcelRole
grant select on dbo.Zone to IRMAReportsRole
grant select on dbo.Zone to IRMASchedJobsRole
grant select on dbo.ZoneSubTeam to IRMAClientRole
grant select on dbo.ZoneSubTeam to IRMAReportsRole
grant select on dbo.ZoneSubTeam to IRMASchedJobsRole
grant select on dbo.ZoneSupply to IRMAClientRole
grant select on dbo.ZoneSupply to IRMAReportsRole
grant select on dbo.ZoneSupply to IRMASchedJobsRole
grant SELECT on AppConfigActive to IRMAReportsRole                                                                                                                  
grant SELECT on AppConfigActiveKey to IRMAReportsRole                                                                                                               
grant SELECT on ConfigurationData to IRMAReportsRole                                                                                                                
grant SELECT on EIM_Jurisdiction_ItemScaleView to IRMAReportsRole                                                                                                   
grant SELECT on EIM_Jurisdiction_ItemView to IRMAReportsRole                                                                                                        
grant SELECT on EXEInterfaces_ZeroShippedOrdersValidationWorkspace to IRMAReportsRole                                                                               
grant SELECT on InventoryAdjustmentCode to IRMAReportsRole                                                                                                          
grant SELECT on ItemDefaultAttribute to IRMAReportsRole                                                                                                             
grant SELECT on ItemDefaultValue to IRMAReportsRole                                                                                                                 
grant SELECT on ItemRequest to IRMAReportsRole                                                                                                                      
grant SELECT on ItemRequest_Status to IRMAReportsRole                                                                                                               
grant SELECT on ItemRequestIdentifier_Type to IRMAReportsRole                                                                                                       
grant SELECT on ItemScaleRequest to IRMAReportsRole                                                                                                                 
grant SELECT on KitchenRoute to IRMAReportsRole                                                                                                                     
grant SELECT on MenuAccess to IRMAReportsRole                                                                                                                       
grant SELECT on Planogram to IRMAReportsRole                                                                                                                        
grant SELECT on PriceBatchPromo to IRMAReportsRole                                                                                                                  
grant SELECT on PromoPreOrders to IRMAReportsRole                                                                                                                   
grant SELECT on ReturnOrder to IRMAReportsRole                                                                                                                      
grant SELECT on Scale_LabelType to IRMAReportsRole                                                                                                                  
grant SELECT on Scale_RandomWeightType to IRMAReportsRole                                                                                                           
grant SELECT on ShelfTag to IRMAReportsRole                                                                                                                         
grant SELECT on ShelfTagAttribute to IRMAReportsRole                                                                                                                
grant SELECT on ShelfTagRule to IRMAReportsRole                                                                                                                     
grant SELECT on ShelfTagRuleType to IRMAReportsRole                                                                                                                 
grant SELECT on SLIM_InStoreSpecials to IRMAReportsRole                                                                                                             
grant SELECT on SLIM_ItemAttributeView to IRMAReportsRole                                                                                                           
grant SELECT on SLIM_ItemIdentifierView to IRMAReportsRole                                                                                                          
grant SELECT on SLIM_ItemScaleView to IRMAReportsRole                                                                                                               
grant SELECT on SLIM_ItemVendorView to IRMAReportsRole                                                                                                              
grant SELECT on SLIM_ItemView to IRMAReportsRole                                                                                                                    
grant SELECT on SLIM_PriceView to IRMAReportsRole                                                                                                                   
grant SELECT on SLIM_Scale_ExtraTextView to IRMAReportsRole                                                                                                         
grant SELECT on SLIM_StatusTypes to IRMAReportsRole                                                                                                                 
grant SELECT on SLIM_StoreItemVendorView to IRMAReportsRole                                                                                                         
grant SELECT on SLIM_StoreItemView to IRMAReportsRole                                                                                                               
grant SELECT on SLIM_VendorCostHistoryView to IRMAReportsRole                                                                                                       
grant SELECT on SLIM_VendorDealView to IRMAReportsRole                                                                                                              
grant SELECT on SlimAccess to IRMAReportsRole                                                                                                                       
grant SELECT on SlimEmail to IRMAReportsRole                                                                                                                        
grant SELECT on StoreFTPConfig to IRMAReportsRole                                                                                                                   
grant SELECT on StoreItemRefresh to IRMAReportsRole                                                                                                                 
grant SELECT on StoreItemRefreshReason to IRMAReportsRole                                                                                                           
grant SELECT on StoreShelfTagConfig to IRMAReportsRole                                                                                                              
grant SELECT on tlog_cmcard to IRMAReportsRole                                                                                                                      
grant SELECT on tlog_cmreserve to IRMAReportsRole                                                                                                                   
grant SELECT on tlog_cmreward to IRMAReportsRole                                                                                                                    
grant SELECT on tlog_discnt to IRMAReportsRole                                                                                                                      
grant SELECT on tlog_item to IRMAReportsRole                                                                                                                        
grant SELECT on tlog_mrkdwn to IRMAReportsRole                                                                                                                      
grant SELECT on tlog_stores to IRMAReportsRole                                                                                                                      
grant SELECT on tlog_taxrec to IRMAReportsRole                                                                                                                      
grant SELECT on tlog_tender to IRMAReportsRole                                                                                                                      
grant SELECT on tmpOrdersAllocateItems to IRMAReportsRole                                                                                                           
grant SELECT on tmpOrdersAllocateOrderItems to IRMAReportsRole                                                                                                      
grant SELECT on UOM_Conversion to IRMAReportsRole                                                                                                                   
grant SELECT on UploadAttribute to IRMAReportsRole                                                                                                                  
grant SELECT on UploadAttributeView to IRMAReportsRole                                                                                                              
grant SELECT on UploadRow to IRMAReportsRole                                                                                                                        
grant SELECT on UploadSession to IRMAReportsRole                                                                                                                    
grant SELECT on UploadSessionUploadType to IRMAReportsRole                                                                                                          
grant SELECT on UploadSessionUploadTypeStore to IRMAReportsRole                                                                                                     
grant SELECT on UploadType to IRMAReportsRole                                                                                                                       
grant SELECT on UploadTypeAttribute to IRMAReportsRole                                                                                                              
grant SELECT on UploadTypeAttributeView to IRMAReportsRole                                                                                                          
grant SELECT on UploadTypeTemplate to IRMAReportsRole                                                                                                               
grant SELECT on UploadTypeTemplateAttribute to IRMAReportsRole                                                                                                      
grant SELECT on UploadValue to IRMAReportsRole                                                                                                                      
grant SELECT on UserAccess to IRMAReportsRole                                                                                                                       
grant SELECT on VendorRequest to IRMAReportsRole                                                                                                                    
grant SELECT on VendorRequest_Status to IRMAReportsRole                                                                                                             
grant SELECT on Warehouse_Inventory to IRMAReportsRole                                                                                                              

grant select, insert on dbo.Planogram to IRMAClientRole
grant select, insert, delete on dbo.VendorCostHistoryExceptions to IRMAAVCIRole
grant select, INSERT, DELETE, UPDATE on dbo.EInvoicing_ErrorHistory TO IRMAAdminRole
grant select, INSERT, DELETE, UPDATE on dbo.EInvoicing_ErrorHistory TO IRMAClientRole 
grant select, insert, delete, update on dbo.tmpOrderExport to IRMASchedJobs
grant select, insert, delete, update on dbo.tmpVendExport to IRMASchedJobs
grant select, insert, update on dbo.ItemAttribute to IRMAClientRole
grant select, insert, update, delete on dbo.conversion_runmode to IRMASchedJobsRole
grant select, insert, update, delete on dbo.ItemAttribute to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemBrand to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemIdentifier to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemOverride to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemScale to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemScaleOverride to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemScaleRequest to IRMASLIMRole
grant select, insert, update, delete on dbo.ItemVendor to IRMASLIMRole
grant select, insert, update, delete on dbo.ODBCErrorLOg to IRMASLIMRole
grant select, insert, update, delete on dbo.POSAuditException to IRMAAdminRole
grant select, insert, update, delete on dbo.POSAuditException to IRMASchedJobsRole
grant select, insert, update, delete on dbo.Price to IRMASLIMRole
grant select, insert, update, delete on dbo.Reporting_PIRIS_Audit to IRMAAdminRole
grant select, insert, update, delete on dbo.Reporting_PIRIS_Audit to IRMAReportsRole
grant select, insert, update, delete on dbo.ReturnOrder to IRMAClientRole
grant select, insert, update, delete on dbo.Scale_ExtraText to IRMASLIMRole
grant select, insert, update, delete on dbo.StoreItemVendor to IRMASLIMRole
grant select, insert, update, delete on dbo.StoreJurisdiction to IRMASLIMRole
grant select, insert, update, delete on dbo.UsersSubTeam to IRMASLIMRole
grant select, insert, update, delete on dbo.UserStoreTeamTitle to IRMASLIMRole
grant select, insert, update, delete on dbo.Vendor to IRMASLIMRole
grant select, insert, update, delete on dbo.VendorCostHistory to IRMASLIMRole
grant select, insert, update, delete on dbo.VendorDealHistory to IRMASLIMRole
grant select, insert, update,delete on dbo.Item to IRMASLIMRole
grant select, insert, update,delete on dbo.ItemManager to IRMASLIMRole
grant select, update on dbo.ItemVendor to IRMAAVCIRole
grant select, update on dbo.OrderExportDeletedQueue to IRMASchedJobs
grant select, update on dbo.OrderExportQueue to IRMASchedJobs
grant select, update on dbo.VendorExportQueue to IRMASchedJobs
grant select, update, insert, delete on dbo.ApplicationConfiguration to IRMAAdminRole
grant select, update, insert, delete on dbo.ApplicationConfiguration to IRMAAdminRole
grant select, update, insert, delete on dbo.ApplicationConfiguration to IRMAClientRole
grant select, update, insert, delete on dbo.ApplicationConfiguration to IRMAClientRole
grant select, update, insert, delete on dbo.ApplicationConfiguration to IRMAReportsRole
grant select, update, insert, delete on dbo.ApplicationConfiguration to IRMAReportsRole
grant select, update, insert, delete on dbo.ApplicationConfiguration to IRMASchedJobsRole
grant select, update, insert, delete on dbo.ApplicationConfiguration to IRMASchedJobsRole
grant select, update, insert, delete on dbo.EInvoicing_Item to IRMAAdminRole
grant select, update, insert, delete on dbo.EInvoicing_Item to IRMAClientRole
grant select, update, insert, delete on dbo.ShelfTag to IRMAAdminRole
grant select, update, insert, delete on dbo.ShelfTag to IRMAClientRole
grant select, update, insert, delete on dbo.ShelfTagAttribute to IRMAAdminRole
grant select, update, insert, delete on dbo.ShelfTagAttribute to IRMAClientRole
grant select, update, insert, delete on dbo.ShelfTagRule to IRMAAdminRole
grant select, update, insert, delete on dbo.ShelfTagRule to IRMAClientRole
grant select, update, insert, delete on dbo.ShelfTagRuleType to IRMAAdminRole
grant select, update, insert, delete on dbo.ShelfTagRuleType to IRMAClientRole
grant select, update, insert, delete on dbo.StoreItemAttribute to IRMAAdminRole
grant select, update, insert, delete on dbo.StoreItemAttribute to IRMAClientRole
grant select, update, insert, delete on Einvoicing_Header to IRMAAdminRole
grant select, update, insert, delete on Einvoicing_Header to IRMAClientRole
grant select,insert on dbo.PriceBatchDetail to IRMAPromoRole
grant select,insert, update, delete on dbo.PriceBatchDetail to IRMASLIMRole
grant select,insert,update,delete on dbo.ItemRequest to IRMASLIMRole
grant select,insert,update,delete on dbo.ItemRequest to IRMASLIMRole
grant select,insert,update,delete on dbo.ItemRequest_Status to IRMASLIMRole
grant select,insert,update,delete on dbo.ItemRequestIdentifier_Type to IRMASLIMRole
grant select,insert,update,delete on dbo.PriceBatchDetail to IRMAPromoRole
grant select,insert,update,delete on dbo.PriceBatchPromo to IRMAPromoRole
grant select,insert,update,delete on dbo.PromoPreOrders to IRMAPromoRole
grant select,insert,update,delete on dbo.SLIM_InStoreSpecials to IRMASLIMRole
grant select,insert,update,delete on dbo.SLIM_StatusTypes to IRMASLIMRole
grant select,insert,update,delete on dbo.SLIMAccess to IRMASLIMRole
grant select,insert,update,delete on dbo.SLIMEmail to IRMASLIMRole
grant select,insert,update,delete on dbo.UserAccess to IRMASLIMRole
grant select,insert,update,delete on dbo.VendorRequest to IRMASLIMRole
grant select,insert,update,delete on dbo.VendorRequest_Status to IRMASLIMRole
grant update on dbo.Item to IRMAClientRole
grant update on dbo.Item to IRMAExcelRole
grant update on dbo.ItemIdentifier to IRMAExcelRole
grant update on dbo.ItemVendor to IRMAExcelRole
grant update on dbo.NewItemsLoad to IRMAExcelRole
grant update on dbo.Price to IRMAClientRole
grant update on dbo.Price to IRMAExcelRole
grant update on dbo.StoreItemVendor to IRMAExcelRole
grant update on dbo.TaxJurisdiction to IRMAClientRole
grant update, insert, delete on dbo.ExRule_VendCostDiff to IRMAAdminRole
grant update, insert, delete on dbo.ExRule_VendCostPackSize to IRMAAdminRole
grant update, insert, delete on dbo.ItemCategory to IRMAAdminRole
grant update, insert, delete on dbo.ItemCategory to IRMAAdminRole
grant update, insert, delete on dbo.ItemType to IRMAAdminRole
grant update, insert, delete on dbo.ItemUnit to IRMAAdminRole
grant update, insert, delete on dbo.LabelType to IRMAAdminRole
grant update, insert, delete on dbo.MenuAccess to IRMAAdminRole
grant update, insert, delete on dbo.POSItem to IRMAAdminRole
grant update, insert, delete on dbo.POSWriterDictionary to IRMAAdminRole
grant update, insert, delete on dbo.POSWriterEscapeChars to IRMAAdminRole, IRMAClientRole
grant update, insert, delete on dbo.Region to IRMAAdminRole
grant update, insert, delete on dbo.RuleDef to IRMAAdminRole
grant update, insert, delete on dbo.Store to IRMAAdminRole
grant update, insert, delete on dbo.StoreFTPConfig to IRMAAdminRole
grant update, insert, delete on dbo.StoreSubTeam to IRMAAdminRole
grant update, insert, delete on dbo.SubTeam to IRMAAdminRole
grant update, insert, delete on dbo.TaxDefinition to IRMAAdminRole
grant update, insert, delete on dbo.TaxFlag to IRMAAdminRole
grant update, insert, delete on dbo.TaxJurisdiction to IRMAAdminRole
grant update, insert, delete on dbo.Team to IRMAAdminRole
grant update, insert, delete on dbo.Temp_PriceAudit to IRMAAdminRole
grant update, insert, delete on dbo.tlog_cmcard to IRMAAdminRole
grant update, insert, delete on dbo.tlog_cmreserve to IRMAAdminRole
grant update, insert, delete on dbo.tlog_cmreward to IRMAAdminRole
grant update, insert, delete on dbo.tlog_discnt to IRMAAdminRole
grant update, insert, delete on dbo.tlog_item to IRMAAdminRole
grant update, insert, delete on dbo.tlog_mrkdwn to IRMAAdminRole
grant update, insert, delete on dbo.tlog_stores to IRMAAdminRole
grant update, insert, delete on dbo.tlog_taxrec to IRMAAdminRole
grant update, insert, delete on dbo.tlog_tender to IRMAAdminRole
grant update, insert, delete on dbo.Users to IRMAAdminRole
grant update, insert, delete on dbo.UserStoreTeamTitle to IRMAAdminRole
grant update, insert, delete on dbo.Zone to IRMAAdminRole
grant update, insert, select, delete, alter on dbo.ConfigurationData to IRMAAdminRole
grant update, insert, select, delete, alter on dbo.ConfigurationData to IRMAClientRole
grant update, insert, select, delete, alter on dbo.ConfigurationData to IRMASchedJobsRole
grant update, insert, select, delete, alter on dbo.Warehouse_Inventory to IRMAAdminRole
grant update, insert, select, delete, alter on dbo.Warehouse_Inventory to IRMAClientRole
grant update, insert, select, delete, alter on dbo.Warehouse_Inventory to IRMASchedJobsRole
grant alter on schema :: dbo to IRMASchedJobsRole
grant create table to IRMASchedJobsRole
grant delete on dbo.Item to IRMAExcelRole
grant delete on dbo.ItemIdentifier to IRMAExcelRole
grant delete on dbo.ItemVendor to IRMAExcelRole
grant delete on dbo.NewItemsLoad to IRMAExcelRole
grant delete on dbo.Price to IRMAExcelRole
grant delete on dbo.StoreItemVendor to IRMAExcelRole
grant delete on dbo.TaxJurisdiction to IRMAClientRole
grant insert on dbo.CycleCountItems to IRMAClientRole
grant insert on dbo.CycleCountItems to IRMAReportsRole
grant insert on dbo.CycleCountItems to IRMASchedJobsRole
grant insert on dbo.ddl_log to IRMASchedJobsRole
grant insert on dbo.InventoryLocationItems to IRMAClientRole
grant insert on dbo.InventoryLocationItems to IRMAReportsRole
grant insert on dbo.InventoryLocationItems to IRMASchedJobsRole
grant insert on dbo.Item to IRMAExcelRole
grant insert on dbo.ItemIdentifier to IRMAExcelRole
grant insert on dbo.ItemVendor to IRMAExcelRole
grant insert on dbo.NewItemsLoad to IRMAExcelRole
grant insert on dbo.Price to IRMAExcelRole
grant insert on dbo.StoreItemVendor to IRMAExcelRole
grant insert on dbo.TaxJurisdiction to IRMAClientRole
grant insert on dbo.VendorCostHistory to IRMAAVCIRole
grant insert on dbo.VendorDealHistory to IRMAAVCIRole
grant insert, update on dbo.OrderHeader to IRMAReportsRole
grant insert, update on OrderHeader to IRMASchedJobs
grant insert, update on OrderItem to IRMASchedJobs
grant insert, update, delete on dbo.ItemRequest to IRMAAdminRole, IRMAClientRole
grant insert, update, delete on dbo.SLIMAccess to IRMAAdminRole, IRMAClientRole
grant insert, update, delete on dbo.UsersSubTeam to IRMAAdminRole, IRMAClientRole
grant insert, update, delete on dbo.VendorRequest to IRMAAdminRole, IRMAClientRole
grant insert,select,delete,update on dbo.EInvoicing_Config to IRMAAdminRole, IRMAClientRole
grant insert,select,delete,update on dbo.EInvoicing_Config to IRMAClientRole
grant insert,select,delete,update on dbo.EInvoicing_ErrorCodes to IRMAAdminRole, IRMAClientRole
grant insert,select,delete,update on dbo.EInvoicing_ErrorCodes to IRMAClientRole
grant insert,select,delete,update on dbo.EInvoicing_Invoices to IRMAAdminRole, IRMAClientRole
grant insert,select,delete,update on dbo.EInvoicing_Invoices to IRMAClientRole
grant insert,select,delete,update on dbo.EInvoicing_Logging to IRMAAdminRole, IRMAClientRole
grant insert,select,delete,update on dbo.SuspendedAvgCost to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant references on dbo.Item to IRMAExcelRole
grant references on dbo.ItemIdentifier to IRMAExcelRole
grant references on dbo.ItemVendor to IRMAExcelRole
grant references on dbo.Price to IRMAExcelRole
grant references on dbo.StoreItemVendor to IRMAExcelRole
grant select on dbo.AppConfigApp to IRMAClientRole, IRMASchedJobsRole
grant exec on dbo.UpdateItemUomOverride to IRMAClientRole
grant select, insert, update, delete on dbo.ItemOverride365 to IRSUser


--DN for EInvoicing WIs  11364,11365,11366
grant exec on dbo.GetReceivingListForNOIDNORD to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
--DN for 11492
grant exec on dbo.FN_GetExePack to IRMAClientRole, IRMAReportsRole

-- Cost function for reports and support.
grant exec on dbo.fn_ItemNetDiscount to IRMAReportsRole, IRMASupportRole

grant select on dbo.StoreRegionMapping to IRMAReportsRole
grant select on dbo.StoreJurisdiction to IRMAReportsRole

-- IRMA Waste 
grant exec on dbo.WasteReport to IRMAReportsRole, IRMAAdminRole, IRMAClientRole
grant exec on dbo.ShrinkReport to IRMAReportsRole, IRMAAdminRole, IRMAClientRole
grant exec on dbo.fn_GetUnitCostForSpoilage to IRMASchedJobs, IRMAReportsRole, IRMAAdminRole, IRMAClientRole
grant select on InventoryAdjustmentCode to IRMASchedJobs
grant exec on dbo.GetWasteCorrections to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetWasteCorrectionRecords to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
GO

grant select on dbo.AppConfigActive to IRMASupportRole
grant select on dbo.AppConfigActiveKey to IRMASupportRole

--tfs 11425
grant exec on [dbo].[CheckIfItemInOpenOrder] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on [dbo].[GetDCOnHand] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
GO

--SLIM POS Item Refresh
GRANT EXEC ON dbo.GetStoreItemRefresh TO IRMASLIMRole
GRANT EXEC ON UpdateStoreItemRefresh TO IRMASLIMRole
GRANT INSERT,SELECT,DELETE,UPDATE ON dbo.StoreItemRefresh TO IRMASLIMRole
GRANT INSERT,SELECT,DELETE,UPDATE ON dbo.StoreItemRefreshReason TO IRMASLIMRole
GO

--Web App Config
grant exec on [dbo].[AppConfig_GetAppConfigSetting] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole


-- ####################################################
-- IRMA Service Library 
-- ####################################################
GRANT EXEC ON dbo.GetStorelist TO IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
GRANT EXEC ON dbo.GetAvailableWasteTypes to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
GRANT EXEC ON dbo.GetUserPermissions to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
GRANT EXEC ON dbo.GetDSDVendors to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
GRANT EXEC ON dbo.GetItem to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
GRANT EXEC ON dbo.GetInstanceDataFlagValue to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
GRANT EXEC ON dbo.InsertItemHistoryShrink to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
GRANT EXEC ON dbo.IsDSDStoreVendorByUPC to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole
grant exec on [dbo].[AppConfig_GetAppConfigSetting] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole

-- TFS 11428, for DB-based application logging.
grant select, insert, delete, update on [dbo].[AppLog] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole
grant select on [dbo].[AppLog] to IRMASupportRole, IRMAReportsRole
grant exec on [dbo].[AppLogPurgeHistory] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASupportRole -- Reports shouldn't need.
grant exec on [dbo].[AppLogInsertEntry] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASupportRole -- Reports shouldn't need.
grant exec on [dbo].[AppLogGetEntries] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASupportRole, IRMAReportsRole

GRANT SELECT ON [dbo].[AppConfigEnv] TO IRMASchedJobs
GRANT SELECT ON [dbo].[AppConfigValue] TO IRMASchedJobs
GRANT SELECT ON [dbo].[AppConfigKey] TO IRMASchedJobs
GRANT SELECT ON [dbo].[AppConfigApp] TO IRMASchedJobs
GRANT SELECT ON [dbo].[Version] TO IRMASchedJobs

--##### ItemUnit UI ####
grant exec on dbo.GetItemUnits to IRMAADminRole, IRMAClientRole
grant exec on dbo.ItemUnitSave to IRMAAdminRole, IRMAClientRole
GO

--### EXEInterfaces ####
grant select, insert, update, delete on dbo.EXEInterfaces_ZeroShippedOrdersValidationWorkspace to IRMAClientRole,IRMAAdminRole, IRMASchedJobsRole

GRANT EXECUTE ON dbo.EInvoicing_UpdateConfigValue TO IRMAAdminRole, IRMAClientRole
GO

------------------------------------------------------------------
-- Shipper SPs for 4.0 (table grant is with table section)
-- Write (insert/update/delete)
grant exec on dbo.ShipperDeleteItem to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASLIMRole, IRMAPromoRole
grant exec on dbo.ShipperInsertItem to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASLIMRole, IRMAPromoRole
grant exec on dbo.ShipperItemUpdateInfo to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASLIMRole, IRMAPromoRole
grant exec on dbo.ShipperUpdatePackInfo to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASLIMRole, IRMAPromoRole
-- Read Only
grant exec on dbo.ShipperGetAllContainingItem to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole, IRMASupportRole
grant exec on dbo.ShipperGetInfo to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole, IRMASupportRole
grant exec on dbo.ShipperCheck to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole, IRMASupportRole
------------------------------------------------------------------

-- TFS 12790 for eInvoice Error Summary Report
GRANT Update ON [dbo].EInvoicing_Invoices TO IRMAReportsRole

grant select on dbo.fn_GetCompetitivePricingInfo to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASLIMRole, IRMAPromoRole
grant exec on dbo.GetCompetitiveBatchItemDateChecked to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASLIMRole, IRMAPromoRole
grant exec on dbo.GetCompetitivePriceChgTypeStatus to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASLIMRole, IRMAPromoRole

-- TFS 13138, Tom Lux, For 'restore deleted item' functionality.
grant exec on dbo.ItemDeletePendingGetInfo to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole, IRMASupportRole
grant exec on dbo.fn_IsItemActive to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASLIMRole, IRMAPromoRole, IRMASupportRole
go

grant insert, update, delete, select on dbo.OrderImportExceptionLog to IRMAClientRole, IRMASchedJobsRole, IRMAReportsRole
go

-- in SQL 2008 these permissions are needed to truncate if the user is not the table owner
grant alter on dbo.tmpVendExport to IRMASchedJobsRole
grant alter on dbo.tmpOrderExport to IRMASchedJobsRole
grant exec on dbo.fn_IsInternalVendor to IRMASchedJobsRole

GO

grant exec on dbo.fn_IsLeadTimeVendor to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.fn_GetLeadTimeDays to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
grant select on VendorHistory to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole
go


-- PO Resolution code
grant exec on dbo.GetResolutionCodeList to IRMAClientRole
grant exec on dbo.GetResolutionCode to IRMAClientRole
grant exec on dbo.UpdateResolutionCode to IRMAClientRole
grant exec on dbo.AddResolutionCode to IRMAClientRole

grant execute on GetVendorPaymentTerms   to IRMAClientRole

grant exec on dbo.GetSuspendedOrderSearch to IRMAClientRole, IRMAReportsRole, IRMASupportRole
grant exec on dbo.UpdateSuspendedPOAdminNotes to IRMAClientRole
grant exec on dbo.UpdateSuspendedPOAdminNotesAndResolution to IRMAClientRole
grant exec on dbo.GetLineItemDetails to IRMAClientRole
grant exec on GetAllItemUnitsCost to IRMAClientRole


grant exec on dbo.fn_IsWeightUnit to IRMAClientRole
grant exec on dbo.fn_IsSoldAsEachCostedByWeightItem to IRMAClientRole
grant exec on dbo.fn_GetAverageUnitWeight to IRMAClientRole
grant exec on dbo.fn_IsRetailUnitNotCostedByWeight to IRMAClientRole
grant exec on dbo.fn_GetAverageUnitWeightByItemKey to IRMAClientRole
grant exec on dbo.fn_IsQuantityMismatch to IRMAClientRole
grant exec on dbo.fn_TrimLeadingZeros to IRMAClientRole


grant exec on dbo.GetShrinkCorrections to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant select ,insert, update, delete on dbo.VendorItemStatuses to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole

-- 13800
grant exec on dbo.AutomaticOrderListNoCost to IRMAClientRole

-- WFM Mobile SPs
grant exec on dbo.CheckForDuplicateReceivingDocumentInvoiceNumber to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.WFMM_GetItem to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.WFMM_GetStoreItemOrderInfo to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.WFMM_GetItemBilledQuantity to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.WFMM_GetItemMovement to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.WFMM_GetVendorPackage to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.GetItemScanData to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateInReviewStatus to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.WFMM_GetTransferItem to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.GetOrderHeaderByIdentifier to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.EInvoicing_GetErrorCodes to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
GO

--MD Reason Code Framework TFS 2095

grant exec on dbo.ReasonCodes_AddMapping to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_CheckIfDetailExists to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_CheckIfTypeExists to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_CreateDetail to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_CreateType to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_DisableMapping to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_GetDetails to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_GetDetailsForType to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_GetTypes to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_UpdateDetails to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
grant exec on dbo.ReasonCodes_UpdateType to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole

grant select on dbo.ReasonCodeType to IRMASchedJobsRole, IRMAreportsRole
grant select on dbo.ReasonCodeDetail to IRMASchedJobsRole, IRMAreportsRole
grant select on dbo.ReasonCodeMappings to IRMASchedJobsRole, IRMAreportsRole
GO

--MD Refuse Receiving stored procedure TFS 2460

grant exec on dbo.UpdateOrderHeaderRefuseReceivingAndClose to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
GO

--MD DSD Vendor enhancement IRMA 4.3 TFS 2455

grant exec on dbo.ClosedPendingEinvoices to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
GO

-- For DataWarehouse team: dynamically grant view to all table where change tracking is enabled
DECLARE @teradataGrantSQL NVARCHAR(MAX) = N'';
SELECT @teradataGrantSQL += CHAR(13) + CHAR(10) + 'GRANT VIEW CHANGE TRACKING ON ' + t.name + ' TO [IRMA_Teradata] AS [dbo];'
FROM sys.tables t, sys.change_tracking_tables ctt 
WHERE t.object_id = ctt.object_id;
EXEC sp_executesql @teradataGrantSQL;

-- Extra tracking grant.
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderItem] TO [IRMASchedJobsRole]

GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUomOverride] TO [spice_user]

GRANT exec on dbo.HARTGetInventoryStoreOpsExport to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
GRANT exec on dbo.LoadESTFileUpdate to IRMAClientRole, IRMAAdminRole, IRMASchedjobsRole, IRMAreportsRole
GO

--IRMA Data Surgery grants
GRANT EXECUTE ON dbo.LabelType_Update TO IRMAClientRole AS dbo
grant execute on [dbo].[GetInventoryCountVendors] to IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMASchedJobsRole
grant execute on [dbo].[SubTeams_RemoveSubTeamToTeamRelationship] to IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMASchedJobsRole
grant execute on [dbo].[SubTeams_AddDiscountExceptions] to IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMASchedJobsRole
grant execute on [dbo].[SubTeams_GetDiscountExceptions] to IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMASchedJobsRole
grant execute on [dbo].[SubTeams_RemoveDiscountExceptions] to IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMASchedJobsRole
GRANT EXECUTE ON [dbo].[fn_CheckUniquePLUMStoreNo] TO [IRMAClientRole], [IRMAReportsRole], [IRMASchedJobsRole] AS [dbo]
GRANT EXECUTE ON dbo.LabelType_Add TO IRMAClientRole AS dbo
GRANT EXECUTE ON dbo.LabelType_Delete TO IRMAClientRole AS dbo
GO

-- item default attribute manager
GRANT EXECUTE ON [dbo].[ItemDefaultAttributes_GetAll] TO IRMAClientRole AS dbo
GRANT EXECUTE ON [dbo].[ItemDefaultAttributes_UpdateItemDefaultAttribute] TO IRMAClientRole AS dbo
GO

-- TFS 8256: DSD et. al.
grant exec on [dbo].[CheckVendorIsDSDForStore] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole AS dbo
grant exec on [dbo].[CheckDSDVendorWithPurchasingStore] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMAReportsRole, IRMASupportRole AS dbo
GO
grant exec on dbo.fn_GetCurrencyConversionRate to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
grant select, update, insert, delete on dbo.CurrencyExchangeRate to IRMASchedJobs
GO

GRANT INSERT, SELECT ON dbo.ExternalOrderInformation TO IRMASchedJobsRole, IRMAAdminRole, IRMAClientRole, IRMAReportsRole
GRANT INSERT, SELECT ON dbo.OrderExternalSource TO IRMASchedJobsRole, IRMAAdminRole, IRMAClientRole, IRMAReportsRole

-- SO Reporting Stuff
grant select, insert, update, delete on dbo.CountDateSchedule to IRMASchedJobs
grant select on dbo.CountDateSchedule to IRMAReports

GO

-- Read access for report user.
declare @rptUserMap table(
	LoginName nvarchar(max),
	DBname nvarchar(max),
	Username nvarchar(max), 
	AliasName nvarchar(max)
)

insert into @rptUserMap
	EXEC master..sp_msloginmappings 'irmareports'

if exists (
	select * from @rptUserMap where dbname like db_name() -- We should be executing against an IRMA DB, so we check for report user mapping in DB where this script is being run.
)
begin
	print 'Adding IRMAReports to DB_DataReader role...'
	exec sp_addrolemember N'db_datareader', N'IRMAReports'
end
go

-- TFS 13594: Click and Collect
IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = 'WFM\ESB_ClickAndCollect')
BEGIN

	DECLARE @Env	VARCHAR(20)
	SET		@Env = (SELECT Environment FROM Version WHERE ApplicationName = 'IRMA Client')

	IF @Env = 'TEST'
		CREATE LOGIN [WFM\ESB_ClickAndCollect] FROM WINDOWS WITH DEFAULT_DATABASE=[ItemCatalog_test], DEFAULT_LANGUAGE=[us_english]
	ELSE
		CREATE LOGIN [WFM\ESB_ClickAndCollect] FROM WINDOWS WITH DEFAULT_DATABASE=[ItemCatalog], DEFAULT_LANGUAGE=[us_english]
END
GO

grant select, insert, update, delete on dbo.P_PriceBatchDenorm to [WFM\ESB_ClickAndCollect]

grant select, insert, update, delete on dbo.PriceBatchDenorm to IRMAClientRole, IRMASchedJobsRole
grant select on dbo.PriceBatchDenorm to IRMAReportsRole, [WFM\ESB_ClickAndCollect]

grant exec on type::dbo.PriceBatchDetailType to IRSUSER

grant exec on dbo.Replenishment_POSPush_PopulatePriceBatchDenorm to IRSUser, IRMAClientRole

grant exec on dbo.Replenishment_POSPush_PublishPriceBatchDenorm to IRSUser, IRMAClientRole

go

GRANT SELECT, INSERT, UPDATE, DELETE  ON dbo.ICONPOSPushStaging TO IRSUser, IRMAClientRole, IRMASchedJobsRole
GRANT SELECT ON dbo.ICONPOSPushStaging TO IRMAReportsRole

GRANT SELECT, INSERT, UPDATE, DELETE  ON dbo.ICONPOSPushPublish TO IRSUser, IRMAClientRole, IRMASchedJobsRole
GRANT SELECT ON dbo.ICONPOSPushPublish TO IRMAReportsRole

GRANT EXEC ON dbo.Replenishment_POSPush_PopulateIConPOSPushPublish TO IRSUser, IRMAClientRole
GRANT EXEC ON dbo.Replenishment_POSPush_PopulateIConPOSPushStaging TO IRSUser, IRMAClientRole
GRANT EXEC ON dbo.BuildStorePosFileForR10 TO IRSUser, IRMAClientRole
GRANT EXEC ON dbo.Replenishment_POSPush_GetPOSFileWriterClass TO IRSUser, IRMAClientRole
GRANT EXEC ON type::dbo.IconPOSPushStagingType to IRSUSER, IRMAClientRole

go

grant exec on dbo.fn_IsRetailSaleItem to IRSUser, IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMASchedJobsRole
grant exec on dbo.fn_HasIngredientIdentifier to IRSUser, IRMAClientRole, IRMAReportsRole, IRMASupportRole, IRMAAdminRole, IRMASchedJobsRole

-- TFS15543
GRANT EXEC ON type::dbo.TlogReprocessRequestType TO IRSUSER, IRMAClientRole
GRANT EXEC ON type::dbo.ItemMovementType TO IRSUSER, IRMAClientRole
GRANT EXEC ON dbo.Replenishment_Tlog_House_InsertTlogReprocessRequest TO IRSUSER, IRMAClientRole
GRANT EXEC ON dbo.Replenishment_Tlog_House_UpdateSalesSumByItem TO IRSUSER, IRMAClientRole

-- TFS15460
grant select on dbo.AppConfigValue to IconInterface
grant select on dbo.AppConfigApp to IconInterface
grant select on dbo.AppConfigEnv to IconInterface
grant select on dbo.AppConfigKey to IconInterface
grant select on dbo.Version to IconInterface

GRANT EXEC ON type::dbo.IconItemChangeQueueIdType to IConInterface
GRANT EXEC ON dbo.MassUpdateIconItemChangeQueue to IConInterface
GRANT EXEC ON dbo.MarkIconItemChangeQueueEntriesInProcess to IConInterface
GRANT EXEC ON dbo.MarkPublishTableEntriesAsInProcess to IConInterface
GRANT EXEC ON dbo.UpdatePublishTableDates to IConInterface

-- TFS 13361 FINTECH
grant select on VendorPaymentTerms to IRMASchedJobs
go

-- TFS 13402
grant exec on dbo.PostStoreItemChangeECom to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
grant exec on dbo.UpdateStoreItemECom to IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole
go

-- TFS 14930

grant execute on Set_Stop_Sale_For_Item to IRSUser
go

--TFS 16196
grant execute on dbo.IconItemRefreshValidateIdentifiers to IRSUser, IRMAClientRole
grant execute on dbo.IconItemRefresh to IRSUser, IRMAClientRole

--TFS 19152
grant execute on dbo.IconPosItemRefresh to IRSUser, IRMAClientRole

--TFS 16201
grant execute on dbo.Scale_GetIngredientsByItem to IRSUser, IRMAClientRole
grant execute on dbo.Scale_GetAllergensByItem to IRSUser, IRMAClientRole
grant execute on dbo.Scale_AddIngredientsToItem to IRSUser, IRMAClientRole
grant execute on dbo.Scale_AddAllergensToItem to IRSUser, IRMAClientRole
grant execute on dbo.Scale_UpdateIngredients to IRSUser, IRMAClientRole
grant execute on dbo.Scale_UpdateAllergens to IRSUser, IRMAClientRole

-- TFS 14921 (Track item-adds from IRMA Client and EIM to send to iCon) - Grant perms to event table for IRMA users.
grant select, insert, update, delete on dbo.IconItemChangeQueue to IRMAClientRole, IRMASchedJobsRole, IRMAAdminRole
grant select on dbo.IconItemChangeQueue to IRMAReportsRole

-- Consolidated all grants for dbo.taxclass (removed dups, added IConInterface).
grant select, insert, update, delete on dbo.TaxClass to IRMAClientRole, IRMASLIMRole, IRMAAdminRole, IConInterface
grant select on dbo.TaxClass to IRMAReportsRole, IRMASchedJobsRole
go

-- Security grants for Nutrifact job
GRANT SELECT ON dbo.itemidentifier TO NutriChefDataWriter
GRANT SELECT ON dbo.item TO NutriChefDataWriter
GRANT SELECT, UPDATE ON dbo.scale_extratext TO NutriChefDataWriter
GRANT SELECT, UPDATE ON dbo.itemscale TO NutriChefDataWriter  
GRANT SELECT, UPDATE, INSERT ON dbo.nutrifacts TO NutriChefDataWriter
Go

-- Icon / Global Controller
GRANT EXEC ON TYPE::IconUpdateItemType TO IConInterface
GRANT EXEC ON dbo.IconItemAddBrand TO IConInterface
GRANT EXEC ON dbo.IconItemAddTax TO IConInterface
GRANT EXEC ON dbo.IconItemAddUpdateLastChange TO IConInterface
GRANT EXEC ON dbo.IconItemAddValidatedScanCode TO IConInterface
GRANT EXEC ON dbo.IconItemUpdateItem TO IConInterface
GRANT EXEC ON dbo.GetIconItemWithTax TO IConInterface
GRANT SELECT, UPDATE ON dbo.ItemOverride TO IconInterface
GRANT EXEC ON TYPE::IconItemNutriFactsType TO IConInterface
GRANT EXEC ON dbo.UpdateNutriFactsFromIcon TO IConInterface
GRANT SELECT, UPDATE, INSERT ON dbo.NutriFacts TO IConInterface
GRANT SELECT, UPDATE, INSERT ON dbo.ItemNutrition TO IConInterface
GRANT SELECT, UPDATE, INSERT ON dbo.ItemScale TO IConInterface
GRANT SELECT, UPDATE ON dbo.ItemAttribute TO IConInterface
GRANT EXEC ON TYPE::dbo.IconLastChangedItemType TO IConInterface
GRANT SELECT, UPDATE, INSERT ON dbo. Scale_Ingredient TO IConInterface
GRANT SELECT, UPDATE, INSERT ON dbo.Scale_Allergen TO IConInterface
GRANT SELECT ON dbo.Scale_ExtraText TO IConInterface
GRANT exec ON [dbo].[fn_IsScaleItem] TO IConInterface
GRANT SELECT ON dbo.Store TO IConInterface
GRANT EXEC ON TYPE::dbo.IdentifiersType TO IConInterface
GRANT EXEC ON dbo.IconUpdateItemSignAttributes TO IConInterface
GRANT EXEC ON dbo.GetIconItemsWithNoNatlClass TO IconInterface
GRANT EXEC ON dbo.GetIconItemsWithNoRetailUOM TO IconInterface

--Icon / SubTeam Controller 
grant select, update on dbo.SubTeam to IConInterface 
grant select, insert on dbo.ItemCategory to IConInterface
grant execute on type::dbo.IconItemWithSubteamType to IConInterface
grant execute on dbo.IconItemSubTeamUpdate to IConInterface
grant execute on dbo.IsItemSubTeamAligned to IConInterface

--Icon / Item Movement
GRANT EXEC ON dbo.IconInsertTlogReprocessRequest TO IRSUser, IRMAClientRole, IconInterface
GRANT EXEC ON dbo.IconUpdateSalesSumByItem TO IRSUser, IRMAClientRole, IconInterface
GRANT EXEC ON dbo.UpdateItemHistoryFromSalesSumByItem TO IRSUser, IRMAClientRole
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.TlogReprocessRequest TO IRSUser, IRMAClientRole, IconInterface
GRANT EXEC ON TYPE::dbo.ItemMovementType TO IRSUser, IRMAClientRole, IconInterface
GRANT EXEC ON TYPE::dbo.TlogReprocessRequestType TO IRSUser, IRMAClientRole, IconInterface
GRANT SELECT ON fn_UpdateItemHistory_GetSalesSummary TO IRSUser, IRMAClientRole

--Icon / Regional Controller
grant select on dbo.NatItemClass to IConInterface
 
-- NutriFacts
GRANT SELECT, INSERT, UPDATE, DELETE ON Scale_Allergen TO IRSUser, IRMAClientRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Scale_Ingredient TO IRSUser, IRMAClientRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Scale_Allergen TO IRSUser, IRMAClientRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Scale_Ingredient TO IRSUser, IRMAClientRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Item_ExtraText TO IRSUser, IRMAClientRole
GRANT EXEC ON Item_GetExtraTextByItem TO IRSUser, IRMAClientRole
GRANT EXEC ON InsertOrUpdateItemExtraText TO IRSUser, IRMAClientRole
GRANT EXEC ON Item_GetIngredientsByItem TO IRSUser, IRMAClientRole
GRANT EXEC ON InsertOrUpdateItemIngredient TO IRSUser, IRMAClientRole
GRANT EXEC ON Item_GetAllergensByItem TO IRSUser, IRMAClientRole
GRANT EXEC ON InsertOrUpdateItemAllergen TO IRSUser, IRMAClientRole

-- Mammoth
GRANT SELECT on [mammoth].[ItemLocaleChangeQueue] to IRMAReportsRole, IRMASchedJobsRole, IRMAAdminRole
GRANT SELECT on [mammoth].[ItemChangeEventType] to IRMAClientRole, IRMASchedJobsRole, IRMAAdminRole, MammothRole
GRANT SELECT, INSERT, UPDATE, DELETE ON mammoth.PriceChangeQueue TO IRSUser, IRMAClientRole, IconInterface
GRANT EXEC ON mammoth.InsertPriceChangeQueue TO IRSUser, IRMAClientRole
GRANT EXEC ON mammoth.IconGenerateMammothEvents TO IConInterface
GRANT EXEC ON [mammoth].[InsertItemLocaleChangeQueueAndPriceChangeQueueByStore] TO IRSUser, IRMAClientRole
GRANT EXEC ON [mammoth].[InsertItemLocaleChangeQueueByBatchHeaderAndStore]		TO IRMAClientrole
GRANT EXEC on [mammoth].[InsertItemLocaleChangeQueue]							to IRMAClientRole
GRANT EXEC on [mammoth].[SlawItemLocaleRefresh]									to IRMAClientRole
GRANT EXEC on [mammoth].[SlawPriceRefresh]										to IRMAClientRole

GRANT SELECT, INSERT, UPDATE, DELETE on [mammoth].[PriceChangeQueue]		to [MammothRole];
GRANT SELECT, INSERT, UPDATE, DELETE on [mammoth].[ItemLocaleChangeQueue]	to [MammothRole];
GRANT SELECT on SCHEMA::[mammoth]											to [MammothRole];
GRANT SELECT on SCHEMA::[dbo]												to [MammothRole];
GRANT SELECT on [dbo].[fn_Parse_List]										to [MammothRole];
GRANT EXEC on [dbo].[fn_GetAppConfigValue]									to [MammothRole];
GRANT EXEC on [mammoth].[UpdateItemLocaleChangeQueueAsInProcess]			to [MammothRole];
GRANT EXEC on [mammoth].[GetItemLocaleChanges]								to [MammothRole];
GRANT EXEC on [mammoth].[GenerateEvents]									to [MammothRole];
GRANT EXEC on [mammoth].[ValidateScanCodesExist]							to [MammothRole];
GRANT EXEC on [mammoth].[GetItemLocaleChanges]								to [MammothRole];
GRANT EXEC on [mammoth].[UpdateItemLocaleChangeQueueAsInProcess]			to [MammothRole];
GRANT EXEC ON TYPE::dbo.IdentifiersType										to [MammothRole];

-- No-tag
grant select, update, delete, insert, alter on dbo.NoTagItemExclusion to IRSUser, IRMAClientRole, IRMAReports
grant execute on dbo.InsertNoTagItemExclusion to IRSUser, IRMAClientRole
grant execute on dbo.GetNoTagMovementHistoryExclusions to IRSUser, IRMAClientRole
grant execute on dbo.GetNoTagOrderingHistoryExclusions to IRSUser, IRMAClientRole
grant execute on dbo.GetNoTagReceivingHistoryExclusions to IRSUser, IRMAClientRole
grant execute on type::dbo.IntType to IRSUser, IRMAClientRole
grant execute on type::dbo.[NoTagRuleThresholdType] to IRSUser, IRMAClientRole
grant execute on type::dbo.[NoTagSubteamOverrideType] to IRSUser, IRMAClientRole
grant select, update, insert, delete, alter on dbo.[NoTagRuleThreshold] to IRSUser, IRMAClientRole
grant select, update, insert, delete, alter on dbo.[NoTagThresholdSubteamOverride] to IRSUser, IRMAClientRole
grant execute on dbo.[GetNoTagRuleThreshold] to IRSUser, IRMAClientRole
grant execute on dbo.[GetNoTagThresholdSubteamOverride] to IRSUser, IRMAClientRole
grant execute on dbo.[GetAlignedSubteams] to IRSUser, IRMAClientRole
grant execute on dbo.[UpdateNoTagRuleThresholds] to IRSUser, IRMAClientRole
grant execute on dbo.[UpdateNoTagSubteamOverrides] to IRSUser, IRMAClientRole
grant execute on dbo.GetNoTagLabelTypeExclusions to IRSUser, IRMAClientRole
grant execute on dbo.GetNoTagOffSaleExclusions to IRMAClientRole, IRSUser

-- SLAW
grant exec on dbo.Reporting_NoTag to IRMAReportsRole
grant exec on dbo.GetPriceBatchDetailForItemKeys to IRMAClientRole
grant exec on [dbo].[GetItemSignAttributeByItemKey] to IRMAClientRole
grant exec on [dbo].[InsertOrUpdateItemSignAttribute] to IRMAClientRole
grant execute on dbo.GetValidPlanogramIdentifiers to IRSUser, IRMAClientRole
grant execute on dbo.GetStoreNumberToBusinessUnitCollection to IRSUser, IRMAClientRole
grant execute on type::dbo.IdentifiersType to IRSUser, IRMAClientRole
grant exec on dbo.GetValidTagReprintIdentifiers to IRSUser, IRMAClientRole
grant execute on dbo.GetDeletablePBHIds to IRSUser, IRMAClientRole

-- POS Push Monitoring Job
GRANT EXEC ON dbo.SendPOSPushFailureNotification TO IRSUser, IRMAClientRole

-- Item Non Batchable Changes
grant exec on dbo.Replenishment_POSPush_DeleteItemNonBatchableChanges to IRMAClientRole, IRMASchedJobsRole
grant SELECT, DELETE, UPDATE on dbo.ItemNonBatchableChanges to IRMAClientRole, IRMASchedJobsRole

-- 365 VIM Extract
GRANT EXEC ON dbo.VIM365AuthorizationStatusFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365ItemRegionFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365ItemStatusFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365ItemStoreExceptionFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365PriceTypeFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365PriceZoneFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365PSVendorRefFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365RegHierarchyFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365RegionalDepartmentFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365RetailFuturePriceFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365RetailPriceFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365StoreFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365VendorCostFile to IRMASchedJobsRole
GRANT EXEC ON dbo.VIM365VendorStoreItemFile to IRMASchedJobsRole
GRANT SELECT, UPDATE ON dbo.StoreRegionMapping to IRMAClientRole

-- 365 customer facing scales
grant select, update, insert, delete, alter on dbo.ItemCustomerFacingScale to IRSUser, IRMAClientRole
grant execute on dbo.GenerateCustomerFacingScaleMaintenance to IRMAClientRole, IRSUser
grant execute on dbo.fn_IsCustomerFacingScaleItem to IRMAClientRole, IRSUser
grant execute on dbo.fn_IsPosPlu to IRMAClientRole, IRSUser

-- UNFI Discontinued item flag update project - RS
grant update on dbo.StoreItemVendor to IRMASchedJobsRole

-- PDX Extracts
USE [master]
GO

CREATE LOGIN [WFM\PDXExtractUserDev] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]
GO

Use [ItemCatalog]
GO

CREATE ROLE [IRMAPDXExtractRole]

GRANT SELECT ON [date] TO  [IRMAPDXExtractRole];
GRANT SELECT ON itemhistory								  TO  [IRMAPDXExtractRole]
GRANT SELECT ON Store                                     TO  [IRMAPDXExtractRole]
GRANT SELECT ON StoreItem								  TO  [IRMAPDXExtractRole]
GRANT SELECT ON StoreItemVendor							  TO  [IRMAPDXExtractRole]
GRANT SELECT ON ItemIdentifier							  TO  [IRMAPDXExtractRole]
GRANT SELECT ON Item									  TO  [IRMAPDXExtractRole]
GRANT SELECT ON ItemAttribute							  TO  [IRMAPDXExtractRole]
GRANT SELECT ON ValidatedScanCode						  TO  [IRMAPDXExtractRole]
GRANT SELECT ON SubTeam									  TO  [IRMAPDXExtractRole]
GRANT SELECT ON ItemUnit								  TO  [IRMAPDXExtractRole]
GRANT SELECT ON Vendor									  TO  [IRMAPDXExtractRole]
GRANT SELECT ON VendorCostHistory						  TO  [IRMAPDXExtractRole]
GRANT SELECT ON OrderHeader								  TO  [IRMAPDXExtractRole]
GRANT SELECT ON OrderItem								  TO  [IRMAPDXExtractRole]
GRANT SELECT ON ExternalOrderInformation				  TO  [IRMAPDXExtractRole]
GRANT SELECT ON VendorCostHistory				          TO  [IRMAPDXExtractRole]
GRANT SELECT ON StoreItemVendor				              TO  [IRMAPDXExtractRole]
GRANT SELECT ON AppConfigValue  				          TO  [IRMAPDXExtractRole]
GRANT SELECT ON AppConfigEnv				              TO  [IRMAPDXExtractRole]
GRANT SELECT ON AppConfigApp    				          TO  [IRMAPDXExtractRole]
GRANT SELECT ON AppConfigKey				              TO  [IRMAPDXExtractRole]

GRANT EXECUTE ON [dbo].[PDX_CalendarHierarchyFile]        TO [IRMAPDXExtractRole]
GRANT EXECUTE ON [dbo].[PDX_ItemSubscriptionFile]         TO [IRMAPDXExtractRole]
GRANT EXECUTE ON [dbo].[PDX_InventoryDataSpoilageFile]    TO [IRMAPDXExtractRole]
GRANT EXECUTE ON [dbo].[PDX_ItemVendorLaneFile]           TO [IRMAPDXExtractRole]
GRANT EXECUTE ON [dbo].[PDX_PurchaseOrdersFile]           TO [IRMAPDXExtractRole]
GRANT EXECUTE ON [dbo].[PDX_ReceiptOrdersFile]            TO [IRMAPDXExtractRole]
GRANT EXECUTE ON [dbo].[PDX_TransferOrdersFile]           TO [IRMAPDXExtractRole]
GRANT EXECUTE ON [dbo].[PDX_DeletedOrdersFile]            TO [IRMAPDXExtractRole]
GRANT EXECUTE ON [dbo].[PDX_FutureCostFile]               TO [IRMAPDXExtractRole]
GRANT EXECUTE ON [dbo].[fn_GetAppConfigValue]             TO [IRMAPDXExtractRole]

-- Infor
grant execute on type::infor.NewItemEventType to IconInterface
grant execute on infor.FinalizeNewItemEvents to IconInterface
grant execute on infor.FailUnprocessedEvents to IconInterface
grant execute on infor.GetNewItems to IconInterface

-- Icon interfaces should be able to read all IRMA data.
execute sp_addrolemember @rolename = N'db_datareader', @membername = N'IConInterface'

--POS Push version 2.0
GRANT EXEC ON dbo.Replenishment_ScalePush_GetIdentifierDeletes TO IRSUser, IRMAClientRole
GRANT EXEC ON dbo.Replenishment_ScalePush_GetPriceBatchSent TO IRSUser, IRMAClientRole
GRANT EXEC ON dbo.Replenishment_POSPush_GetBatchDataForStores TO IRSUser, IRMAClientRole
GRANT EXEC ON dbo.Replenishment_POSPush_GetAllTaxflagData TO IRSUser, IRMAClientRole

--POS Push Version 2.1
GRANT EXEC ON dbo.Replenishment_POSPush_PopulateNonBatchIconPOSPushStaging TO IRSUser, IRMAClientRole

--POS Push Version 2.2
GRANT EXEC ON dbo.Replenishment_POSPush_PopulateBatchIconPOSPushStaging TO IRSUser, IRMAClientRole

--POS Push Version 3.0
grant execute on type::dbo.BatchIdsType to IRSUser, IRMAClientRole
-- For Rentention Policy UI
GRANT EXECUTE ON [dbo].DeleteRetentionPolicy TO [IRMAClientRole]
GRANT EXECUTE ON [dbo].GetRetentionPoliciesByTableDailyPurge TO [IRMAClientRole]
GRANT EXECUTE ON [dbo].GetRetentionPolicyById TO [IRMAClientRole]
GRANT EXECUTE ON [dbo].UpdateRetentionPolicy TO [IRMAClientRole]
GRANT EXECUTE ON [dbo].GetPurgeJobNamesForRetentionPolicy TO [IRMAClientRole]
GRANT EXECUTE ON [dbo].GetTablesWithRetentionPolicy TO [IRMAClientRole]
GRANT EXECUTE ON [dbo].GetColumnNamesBySchemaTable TO [IRMAClientRole] 
GRANT EXECUTE ON [dbo].ValidateIfTableExists TO [IRMAClientRole]
--giving read permission to IRMA user so that we can validate if table is valid or not on retention policy screen
exec sp_addrolemember 'db_datareader', 'IRSUser' 
GRANT UPDATE, INSERT, DELETE on dbo.RetentionPolicy to IRSUser