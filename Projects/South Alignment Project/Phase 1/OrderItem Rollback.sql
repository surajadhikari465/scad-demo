/******************************************************************************
	FL OrderItem - RollBack
	Change Steps
******************************************************************************/
PRINT N'Status: BEGIN FL OrderItem - RollBack (takes about 1.5 hours in TEST): --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Run: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 90, SYSDATETIME()), 9)
GO
USE ItemCatalog
--USE ItemCatalog_Test
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderItem] disable change_tracking
GO
/******************************************************************************
			2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated): --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Quant__38AF44A5]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Total__3C7FD589]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Units__3D73F9C2]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Cost__3E681DFB]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__UnitC__3F5C4234]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__UnitE__4050666D]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Quant__4238AEDF]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Disco__432CD318]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Adjus__4420F751]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Handl__45151B8A]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Freig__46FD63FC]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__LineI__61B15A38]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__LineI__62A57E71]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__LineI__6399A2AA]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Recei__648DC6E3]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Recei__6581EB1C]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Recei__66760F55]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Lande__676A338E]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_MarkupPercent_1]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_MarkupCost_1]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_Package_Desc1_1]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_Package_Desc2_1]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Recei__7F4817D7]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Units__003C3C10]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_OrigReceivedItemCost]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_OrigReceivedItemUnit]
GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderItemAddUpdDel]'))
DROP TRIGGER [dbo].[OrderItemAddUpdDel]
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_SustainabilityRanking]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_SustainabilityRanking]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ReasonCodeDetail]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemUnit2]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemUnit1]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemUnit]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemOrigin_CountryProc_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemOrigin_CountryProc_ID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Item]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_CostAdjustmentReason]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_CostAdjustmentReason]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Quant__21D600EE]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Origi__20E1DCB5]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Origi__20E1DCB5]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Order__1FEDB87C]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Order__1FEDB87C]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Handl__1C1D2798]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Freig__1B29035F]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__CostU__1A34DF26]
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'IX_OrderItem_Item_Key')
DROP INDEX [IX_OrderItem_Item_Key] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'IX_OrderItem_DateReceived')
DROP INDEX [IX_OrderItem_DateReceived] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'idxVendorCostHistoryID')
DROP INDEX [idxVendorCostHistoryID] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'idxOrderItemIDHeaderID')
DROP INDEX [idxOrderItemIDHeaderID] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'idxDiscountType')
DROP INDEX [idxDiscountType] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_QuantityReceived_Origin_ID_OrderItem_ID_Item_Key')
DROP INDEX [_dta_IX_OrderItem_OrderHeader_ID_QuantityReceived_Origin_ID_OrderItem_ID_Item_Key] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_Package_Unit_ID_OrderItem_ID_Item_Key_QuantityUnit')
DROP INDEX [_dta_IX_OrderItem_OrderHeader_ID_Package_Unit_ID_OrderItem_ID_Item_Key_QuantityUnit] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_QuantityUnit_Item_Key')
DROP INDEX [_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_QuantityUnit_Item_Key] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_Item_Key')
DROP INDEX [_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_Item_Key] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_Item_Key_OrderItem_ID')
DROP INDEX [_dta_IX_OrderItem_OrderHeader_ID_Item_Key_OrderItem_ID] ON [dbo].[OrderItem]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID')
DROP INDEX [_dta_IX_OrderItem_OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID] ON [dbo].[OrderItem]
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[PK__OrderItem__6D0D32F4]', N'PK__OrderItem_Rollback';
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[PK__OrderItem_Unaligned]', N'PK__OrderItem__6D0D32F4';
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[OrderItem]', N'OrderItem_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[OrderItem_Unaligned]', N'OrderItem';
GO
/******************************************************************************
			10. Create SO Defaults
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Quant__38AF44A5] DEFAULT ((0)) FOR [QuantityOrdered]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Total__3C7FD589] DEFAULT ((0)) FOR [Total_Weight]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Units__3D73F9C2] DEFAULT ((0)) FOR [Units_per_Pallet]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Cost__3E681DFB] DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__UnitC__3F5C4234] DEFAULT ((0)) FOR [UnitCost]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__UnitE__4050666D] DEFAULT ((0)) FOR [UnitExtCost]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Quant__4238AEDF] DEFAULT ((0)) FOR [QuantityDiscount]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Disco__432CD318] DEFAULT ((0)) FOR [DiscountType]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Adjus__4420F751] DEFAULT ((0)) FOR [AdjustedCost]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Handl__45151B8A] DEFAULT ((0)) FOR [Handling]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Freig__46FD63FC] DEFAULT ((0)) FOR [Freight]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__LineI__61B15A38] DEFAULT ((0)) FOR [LineItemCost]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__LineI__62A57E71] DEFAULT ((0)) FOR [LineItemHandling]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__LineI__6399A2AA] DEFAULT ((0)) FOR [LineItemFreight]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Recei__648DC6E3] DEFAULT ((0)) FOR [ReceivedItemCost]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Recei__6581EB1C] DEFAULT ((0)) FOR [ReceivedItemHandling]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Recei__66760F55] DEFAULT ((0)) FOR [ReceivedItemFreight]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Lande__676A338E] DEFAULT ((0)) FOR [LandedCost]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF_OrderItem_MarkupPercent_1] DEFAULT ((0)) FOR [MarkupPercent]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF_OrderItem_MarkupCost_1] DEFAULT ((0)) FOR [MarkupCost]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF_OrderItem_Package_Desc1_1] DEFAULT ((0)) FOR [Package_Desc1]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF_OrderItem_Package_Desc2_1] DEFAULT ((0)) FOR [Package_Desc2]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Recei__7F4817D7] DEFAULT ((0)) FOR [ReceivedFreight]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Units__003C3C10] DEFAULT ((0)) FOR [UnitsReceived]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF_OrderItem_AdjustedCostUserID] DEFAULT ((0)) FOR [AdjustedCostUserID]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF_OrderItem_AdjustedCostReason] DEFAULT ((0)) FOR [AdjustedCostReason]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Order__611F22F4] DEFAULT ((0)) FOR [OrderItemCOOL]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Order__6213472D] DEFAULT ((0)) FOR [OrderItemBIO]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF_OrderItem_OrigReceivedItemCost] DEFAULT ((0.00)) FOR [OrigReceivedItemCost]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF_OrderItem_OrigReceivedItemUnit] DEFAULT ((0)) FOR [OrigReceivedItemUnit]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Catch__0918D10C] DEFAULT ((0)) FOR [CatchWeightCostPerWeight]
GO
ALTER TABLE [OrderItem]WITH NOCHECK ADD CONSTRAINT [DF__OrderItem__Recei__4329A18D] DEFAULT ((0)) FOR [ReceivedViaGun]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderItem] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
SET ANSI_PADDING ON
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderItem] TO [IConInterface] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderItem] TO [iCONReportingRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderItem] TO [IRMA_Teradata] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderItem] TO [IRMAAdminRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderItem] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderItem] TO [IRMAPDXExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderItem] TO [IRMAReportsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderItem] TO [IRMAReportsRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[OrderItem] TO [IRMASchedJobs] AS [dbo]
GO
GRANT UPDATE ON [dbo].[OrderItem] TO [IRMASchedJobs] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderItem] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderItem] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderItem] TO [IRMASupportRole] AS [dbo]
GO
/******************************************************************************
			13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes (takes about 40 minutes in TEST): --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem__OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem__OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[DiscountType] ASC,
	[NetVendorItemDiscount] ASC,
	[OrderItem_ID] ASC
)
INCLUDE ( 	[LineItemCost],
	[ReceivedItemCost],
	[ReceivedItemFreight]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_Item_Key_OrderItem_ID')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_Item_Key_OrderItem_ID] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[Item_Key] ASC,
	[OrderItem_ID] ASC
)
INCLUDE ( 	[UnitsReceived]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_Item_Key')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_Item_Key] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[OrderItem_ID] ASC,
	[Item_Key] ASC
)
INCLUDE ( 	[DateReceived],
	[LineItemHandling],
	[LineItemFreight]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_QuantityUnit_Item_Key')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_OrderItem_ID_QuantityUnit_Item_Key] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[OrderItem_ID] ASC,
	[QuantityUnit] ASC,
	[Item_Key] ASC
)
INCLUDE ( 	[QuantityOrdered],
	[QuantityReceived],
	[Total_Weight],
	[QuantityDiscount],
	[DiscountType],
	[Package_Desc1],
	[Package_Desc2]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_Package_Unit_ID_OrderItem_ID_Item_Key_QuantityUnit')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_Package_Unit_ID_OrderItem_ID_Item_Key_QuantityUnit] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[Package_Unit_ID] ASC,
	[OrderItem_ID] ASC,
	[Item_Key] ASC,
	[QuantityUnit] ASC
)
INCLUDE ( 	[QuantityOrdered],
	[QuantityReceived],
	[Total_Weight],
	[Cost],
	[UnitCost],
	[UnitExtCost],
	[QuantityDiscount],
	[DiscountType],
	[AdjustedCost],
	[LineItemCost],
	[LineItemHandling],
	[LineItemFreight],
	[Package_Desc1],
	[Package_Desc2],
	[Origin_ID],
	[CountryProc_ID],
	[Lot_No]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_QuantityReceived_Origin_ID_OrderItem_ID_Item_Key')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_QuantityReceived_Origin_ID_OrderItem_ID_Item_Key] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[QuantityReceived] ASC,
	[Origin_ID] ASC,
	[OrderItem_ID] ASC,
	[Item_Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'idxDiscountType')
CREATE NONCLUSTERED INDEX [idxDiscountType] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC
)
INCLUDE ( 	[Item_Key],
	[DiscountType]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'idxOrderItemIDHeaderID')
CREATE UNIQUE NONCLUSTERED INDEX [idxOrderItemIDHeaderID] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[OrderItem_ID] ASC,
	[OHOrderDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'idxVendorCostHistoryID')
CREATE NONCLUSTERED INDEX [idxVendorCostHistoryID] ON [dbo].[OrderItem]
(
	[VendorCostHistoryID] ASC,
	[OHOrderDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER INDEX [idxVendorCostHistoryID] ON [dbo].[OrderItem] DISABLE
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'IX_OrderItem_DateReceived')
CREATE NONCLUSTERED INDEX [IX_OrderItem_DateReceived] ON [dbo].[OrderItem]
(
	[DateReceived] ASC
)
INCLUDE ( 	[Item_Key]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'IX_OrderItem_Item_Key')
CREATE NONCLUSTERED INDEX [IX_OrderItem_Item_Key] ON [dbo].[OrderItem]
(
	[Item_Key] ASC
)
INCLUDE ( 	[OrderHeader_ID],
	[DateReceived]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
			14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__CostU__1A34DF26] FOREIGN KEY([CostUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__CostU__1A34DF26]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Freig__1B29035F] FOREIGN KEY([FreightUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__Freig__1B29035F]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Handl__1C1D2798] FOREIGN KEY([HandlingUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__Handl__1C1D2798]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__OrderHeader_ID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Origi__20E1DCB5]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Origi__20E1DCB5] FOREIGN KEY([Origin_ID])
REFERENCES [dbo].[ItemOrigin] ([Origin_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Origi__20E1DCB5]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__Origi__20E1DCB5]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Quant__21D600EE] FOREIGN KEY([QuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__Quant__21D600EE]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_CostAdjustmentReason]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_CostAdjustmentReason] FOREIGN KEY([CostAdjustmentReason_ID])
REFERENCES [dbo].[CostAdjustmentReason] ([CostAdjustmentReason_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_CostAdjustmentReason]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_CostAdjustmentReason]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_Item]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemOrigin_CountryProc_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemOrigin_CountryProc_ID] FOREIGN KEY([CountryProc_ID])
REFERENCES [dbo].[ItemOrigin] ([Origin_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemOrigin_CountryProc_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_ItemOrigin_CountryProc_ID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_ItemUnit]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit1] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_ItemUnit1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit2] FOREIGN KEY([InvoiceQuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_ItemUnit2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ReasonCodeDetail] FOREIGN KEY([ReasonCodeDetailID])
REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_ReasonCodeDetail]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail] FOREIGN KEY([ReceivingDiscrepancyReasonCodeID])
REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_SustainabilityRanking]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_SustainabilityRanking] FOREIGN KEY([SustainabilityRankingID])
REFERENCES [dbo].[SustainabilityRanking] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_SustainabilityRanking]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_SustainabilityRanking]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Users_AdjustedCostUserID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_Users_AdjustedCostUserID] FOREIGN KEY([AdjustedCostUserID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Users_AdjustedCostUserID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_Users_AdjustedCostUserID]
GO
/******************************************************************************
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderItemAddUpdDel]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[OrderItemAddUpdDel]
ON [dbo].[OrderItem]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
    BEGIN TRY
    
    
    -- StoreOps Export 
	UPDATE OrderExportQueue
	SET QueueInsertedDate = GetDate(), DeliveredToStoreOpsDate = null
	WHERE OrderHeader_ID in (
	    SELECT DISTINCT OH.OrderHeader_ID
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Inserted
			 UNION
			 SELECT OrderHeader_ID FROM Deleted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
		 	
	)
	IF @@ROWCOUNT=0
	BEGIN
		INSERT INTO OrderExportQueue
		SELECT DISTINCT OH.OrderHeader_ID, GetDate(), null
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Inserted
			 UNION
			 SELECT OrderHeader_ID FROM Deleted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
	END
    --Copy the Cost and CostUnit value in OrigReceivedItemCost and OrigReceivedItemUnit
    --when data is inserted into OrderItem table
    UPDATE OI
    SET OI.OrigReceivedItemCost = I.Cost, OI.OrigReceivedItemUnit = I.CostUnit
    FROM OrderItem OI
    INNER JOIN INSERTED I ON
		I.OrderItem_ID  = OI.OrderItem_ID 
    WHERE NOT EXISTS (SELECT * FROM DELETED D WHERE D.OrderItem_ID  = I.OrderItem_ID)    	 
    
    --Copy the OrderDate from OrderHeader table insert into OHOrderDate when data is inserted into OrderItem table
    UPDATE OI
    SET OI.OHOrderDate = OH.OrderDate 
    FROM OrderItem OI
    INNER JOIN INSERTED I ON
            I.OrderItem_ID  = OI.OrderItem_ID 
    INNER JOIN OrderHeader OH ON
            OI.OrderHeader_ID = OH.OrderHeader_ID
    
    
            -- Capture for the update avgcost/onhand process 
        INSERT INTO ItemHistoryInsertedQueue (Store_No, Item_Key, DateStamp, SubTeam_No, ItemHistoryID, Adjustment_ID)
        SELECT IH.Store_No, IH.Item_Key, IH.DateStamp, IH.SubTeam_No, IH.ItemHistoryID, IH.Adjustment_ID
        FROM Inserted
        INNER JOIN Deleted ON Inserted.OrderItem_ID = Deleted.OrderItem_ID
        INNER JOIN ItemHistory IH (nolock) ON IH.OrderItem_ID = Inserted.OrderItem_ID
        INNER JOIN Item (nolock) ON Item.Item_Key = Inserted.Item_Key
        INNER JOIN Store (nolock) ON Store.Store_No = IH.Store_No
        -- exclude ingredient items unless the affected store is a Distribution Center
        WHERE (((Ingredient = 0 AND ISNULL(UseLastReceivedCost, 0) = 0) AND (Item.Subteam_No = IH.Subteam_No)) OR Store.Distribution_Center = 1)
            AND IH.Adjustment_ID = 5
            AND (
                ((Inserted.ReceivedItemCost + Inserted.ReceivedItemFreight) <> (Deleted.ReceivedItemCost + Deleted.ReceivedItemFreight))
                OR Inserted.UnitsReceived <> Deleted.UnitsReceived
                )
                        
        -- For updates, keep receiving ItemHistory in synch
        -- Use a table variable and a while loop instead of a cursor
        DECLARE @ReceivedList TABLE (OrderItem_ID int PRIMARY KEY)
        DECLARE @OrderItem_ID int
        
        INSERT INTO @ReceivedList
        SELECT Inserted.OrderItem_ID
        FROM Inserted
        INNER JOIN Deleted ON Inserted.OrderItem_ID = Deleted.OrderItem_ID
        WHERE (Inserted.UnitsReceived <> Deleted.UnitsReceived)
        
        WHILE EXISTS (SELECT * FROM @ReceivedList)
        BEGIN
            SET @OrderItem_ID = (SELECT TOP 1 OrderItem_ID FROM @ReceivedList)
            
            EXEC InsertReceivingItemHistory @OrderItem_ID, 0  -- Does not matter who the user is as far as ItemHistory since there is a link to OrderItem.  The receiver should be recorded in the OrderHeader record.
            
            DELETE @ReceivedList WHERE OrderItem_ID = @OrderItem_ID
        END
                                           
    END TRY
    BEGIN CATCH
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
        IF @@TranCount > 0 
          begin
            ROLLBACK TRAN
          end
        
        RAISERROR (''OrderItemAddUpdDel trigger failed with @@ERROR: %d - %s'', @err_sev, 1, @err_no, @err_msg)
    END CATCH	
    
END
' 
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'OrderItem_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order item id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'OrderItem_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'OrderHeader_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order header id refer to order header table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'OrderHeader_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Item_Key'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The item that was ordered refer to item table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Item_Key'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'ExpirationDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Expiration date for the order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'ExpirationDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'QuantityOrdered'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount ordered' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'QuantityOrdered'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'QuantityUnit'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of units ordered' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'QuantityUnit'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'QuantityReceived'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount received' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'QuantityReceived'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Total_Weight'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total weight of the order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Total_Weight'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Units_per_Pallet'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Units per pallet' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Units_per_Pallet'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Cost'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cost of the order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Cost'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'UnitCost'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unit cost of the order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'UnitCost'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'UnitExtCost'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unit extended cost' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'UnitExtCost'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'CostUnit'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cost per unit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'CostUnit'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'QuantityDiscount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quantitiy discount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'QuantityDiscount'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'DiscountType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=No Discount;1=Cash Discount;2=Percent Discount;3=Free Items;4=Landed Percent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'DiscountType'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'AdjustedCost'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The amount the cost was adjusted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'AdjustedCost'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Handling'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'How much it cost for handling' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Handling'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'HandlingUnit'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'How much it cost per unit to handle the item' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'HandlingUnit'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Freight'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total frieght cost' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Freight'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'FreightUnit'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cost of freight per unit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'FreightUnit'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'DateReceived'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date item was received' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'DateReceived'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'OriginalDateReceived'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Original date item was received' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'OriginalDateReceived'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Comments'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order comments' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Comments'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'LineItemCost'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line item cost' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'LineItemCost'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'LineItemHandling'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lint item handling cost' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'LineItemHandling'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'LineItemFreight'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line item freight cost' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'LineItemFreight'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'ReceivedItemCost'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Received item cost' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'ReceivedItemCost'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'ReceivedItemHandling'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Received item handling cost' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'ReceivedItemHandling'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'ReceivedItemFreight'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Received item freight cost' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'ReceivedItemFreight'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'LandedCost'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Landed cost' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'LandedCost'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Origin_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Origin id refer to item origin table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Origin_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'AdjustedCostReason'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 = None, 1 = Special Deal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'AdjustedCostReason'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'MarkupDollars'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The monetary amount of distribution center markup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'MarkupDollars'
GO
/******************************************************************************
		17. Check SO Checks
******************************************************************************/
PRINT N'Status: 17. Check SO Checks (takes about 30 minutes in TEST): --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_Item];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK__OrderItem__Freig__1B29035F];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK__OrderItem__Handl__1C1D2798];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK__OrderItem__Origi__20E1DCB5];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_CostAdjustmentReason];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_ItemOrigin_CountryProc_ID];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_ItemUnit2];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_ReasonCodeDetail];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_SustainabilityRanking];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_CostAdjustmentReason];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_ItemOrigin_CountryProc_ID];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_ItemUnit2];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_ReasonCodeDetail];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail];
GO
ALTER TABLE [dbo].[OrderItem] WITH CHECK CHECK CONSTRAINT [FK_OrderItem_SustainabilityRanking];
GO
PRINT N'Status: **** Operation Complete ****: --- [dbo].[OrderItem] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO