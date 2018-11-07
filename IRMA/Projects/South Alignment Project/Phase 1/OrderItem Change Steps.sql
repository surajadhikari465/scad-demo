/******************************************************************************
	SO OrderItem - Rebuild
	Change Steps
******************************************************************************/
PRINT N'Status: BEGIN SO OrderItem - Rebuild (takes about 3 hours in TEST): --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 180, SYSDATETIME()), 9)
GO
USE ItemCatalog
--USE ItemCatalog_Test
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderItem] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated): --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_AdjustedCostUserID]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_AdjustedCostReason]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Order__611F22F4]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Order__6213472D]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_OrigReceivedItemCost]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF_OrderItem_OrigReceivedItemUnit]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Catch__0918D10C]
GO
ALTER TABLE [OrderItem] DROP CONSTRAINT [DF__OrderItem__Recei__4329A18D]
GO
/******************************************************************************
			3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'MarkupDollars'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'MarkupDollars'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'AdjustedCostReason'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'AdjustedCostReason'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Origin_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Origin_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'LandedCost'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'LandedCost'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'ReceivedItemFreight'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'ReceivedItemFreight'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'ReceivedItemHandling'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'ReceivedItemHandling'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'ReceivedItemCost'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'ReceivedItemCost'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'LineItemFreight'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'LineItemFreight'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'LineItemHandling'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'LineItemHandling'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'LineItemCost'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'LineItemCost'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Comments'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Comments'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'OriginalDateReceived'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'OriginalDateReceived'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'DateReceived'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'DateReceived'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'FreightUnit'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'FreightUnit'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Freight'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Freight'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'HandlingUnit'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'HandlingUnit'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Handling'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Handling'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'AdjustedCost'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'AdjustedCost'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'DiscountType'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'DiscountType'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'QuantityDiscount'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'QuantityDiscount'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'CostUnit'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'CostUnit'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'UnitExtCost'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'UnitExtCost'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'UnitCost'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'UnitCost'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Cost'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Cost'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Units_per_Pallet'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Units_per_Pallet'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Total_Weight'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Total_Weight'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'QuantityReceived'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'QuantityReceived'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'QuantityUnit'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'QuantityUnit'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'QuantityOrdered'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'QuantityOrdered'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'ExpirationDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'ExpirationDate'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Item_Key'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Item_Key'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'OrderHeader_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'OrderHeader_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'OrderItem_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'OrderItem_ID'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderItemAddUpdDel]'))
DROP TRIGGER [dbo].[OrderItemAddUpdDel]
GO
/******************************************************************************
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Users_AdjustedCostUserID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Users_AdjustedCostUserID]
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
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__OrderHeader_ID]
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
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem__OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID')
DROP INDEX [_dta_IX_OrderItem__OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID] ON [dbo].[OrderItem]
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[PK__OrderItem__6D0D32F4]', N'PK__OrderItem_Unaligned';
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[OrderItem]', N'OrderItem_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OrderItem](
	[OrderItem_ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderHeader_ID] [int] NOT NULL,
	[Item_Key] [int] NOT NULL,
	[ExpirationDate] [smalldatetime] NULL,
	[QuantityOrdered] [decimal](18, 4) NOT NULL CONSTRAINT [DF__OrderItem__Quant__38AF44A5]  DEFAULT ((0)),
	[QuantityUnit] [int] NOT NULL,
	[QuantityReceived] [decimal](18, 4) NULL,
	[Total_Weight] [decimal](18, 4) NOT NULL CONSTRAINT [DF__OrderItem__Total__3C7FD589]  DEFAULT ((0)),
	[Units_per_Pallet] [smallint] NOT NULL CONSTRAINT [DF__OrderItem__Units__3D73F9C2]  DEFAULT ((0)),
	[Cost] [money] NOT NULL CONSTRAINT [DF__OrderItem__Cost__3E681DFB]  DEFAULT ((0)),
	[UnitCost] [money] NOT NULL CONSTRAINT [DF__OrderItem__UnitC__3F5C4234]  DEFAULT ((0)),
	[UnitExtCost] [money] NOT NULL CONSTRAINT [DF__OrderItem__UnitE__4050666D]  DEFAULT ((0)),
	[CostUnit] [int] NULL,
	[QuantityDiscount] [decimal](18, 4) NOT NULL CONSTRAINT [DF__OrderItem__Quant__4238AEDF]  DEFAULT ((0)),
	[DiscountType] [int] NOT NULL CONSTRAINT [DF__OrderItem__Disco__432CD318]  DEFAULT ((0)),
	[AdjustedCost] [money] NOT NULL CONSTRAINT [DF__OrderItem__Adjus__4420F751]  DEFAULT ((0)),
	[Handling] [money] NOT NULL CONSTRAINT [DF__OrderItem__Handl__45151B8A]  DEFAULT ((0)),
	[HandlingUnit] [int] NULL,
	[Freight] [money] NOT NULL CONSTRAINT [DF__OrderItem__Freig__46FD63FC]  DEFAULT ((0)),
	[FreightUnit] [int] NULL,
	[DateReceived] [datetime] NULL,
	[OriginalDateReceived] [datetime] NULL,
	[Comments] [varchar](255) NULL,
	[LineItemCost] [money] NOT NULL CONSTRAINT [DF__OrderItem__LineI__61B15A38]  DEFAULT ((0)),
	[LineItemHandling] [money] NOT NULL CONSTRAINT [DF__OrderItem__LineI__62A57E71]  DEFAULT ((0)),
	[LineItemFreight] [money] NOT NULL CONSTRAINT [DF__OrderItem__LineI__6399A2AA]  DEFAULT ((0)),
	[ReceivedItemCost] [money] NOT NULL CONSTRAINT [DF__OrderItem__Recei__648DC6E3]  DEFAULT ((0)),
	[ReceivedItemHandling] [money] NOT NULL CONSTRAINT [DF__OrderItem__Recei__6581EB1C]  DEFAULT ((0)),
	[ReceivedItemFreight] [money] NOT NULL CONSTRAINT [DF__OrderItem__Recei__66760F55]  DEFAULT ((0)),
	[LandedCost] [money] NOT NULL CONSTRAINT [DF__OrderItem__Lande__676A338E]  DEFAULT ((0)),
	[MarkupPercent] [decimal](18, 4) NOT NULL CONSTRAINT [DF_OrderItem_MarkupPercent_1]  DEFAULT ((0)),
	[MarkupCost] [money] NOT NULL CONSTRAINT [DF_OrderItem_MarkupCost_1]  DEFAULT ((0)),
	[Package_Desc1] [decimal](9, 4) NOT NULL CONSTRAINT [DF_OrderItem_Package_Desc1_1]  DEFAULT ((0)),
	[Package_Desc2] [decimal](9, 4) NOT NULL CONSTRAINT [DF_OrderItem_Package_Desc2_1]  DEFAULT ((0)),
	[Package_Unit_ID] [int] NULL,
	[Retail_Unit_ID] [int] NULL,
	[Origin_ID] [int] NULL,
	[ReceivedFreight] [money] NOT NULL CONSTRAINT [DF__OrderItem__Recei__7F4817D7]  DEFAULT ((0)),
	[UnitsReceived] [decimal](18, 4) NOT NULL CONSTRAINT [DF__OrderItem__Units__003C3C10]  DEFAULT ((0)),
	[CreditReason_ID] [int] NULL,
	[QuantityAllocated] [decimal](18, 4) NULL,
	[CountryProc_ID] [int] NULL,
	[Lot_No] [varchar](12) NULL,
	[NetVendorItemDiscount] [money] NULL,
	[CostAdjustmentReason_ID] [int] NULL,
	[Freight3Party] [smallmoney] NULL,
	[LineItemFreight3Party] [smallmoney] NULL,
	[HandlingCharge] [smallmoney] NULL,
	[eInvoiceQuantity] [decimal](18, 4) NULL,
	[SACCost] [smallmoney] NULL,
	[OrderItemCOOL] [bit] NOT NULL DEFAULT ((0)),
	[OrderItemBIO] [bit] NOT NULL DEFAULT ((0)),
	[Carrier] [varchar](99) NULL,
	[InvoiceQuantityUnit] [int] NULL,
	[InvoiceCost] [money] NULL,
	[InvoiceExtendedCost] [money] NULL,
	[InvoiceExtendedFreight] [money] NULL,
	[InvoiceTotalWeight] [decimal](18, 4) NULL,
	[VendorCostHistoryID] [int] NULL,
	[OrigReceivedItemCost] [money] NULL CONSTRAINT [DF_OrderItem_OrigReceivedItemCost]  DEFAULT ((0.00)),
	[OrigReceivedItemUnit] [int] NULL CONSTRAINT [DF_OrderItem_OrigReceivedItemUnit]  DEFAULT ((0)),
	[CatchWeightCostPerWeight] [money] NULL DEFAULT ((0)),
	[QuantityShipped] [decimal](18, 4) NULL,
	[WeightShipped] [decimal](18, 4) NULL,
	[OHOrderDate] [smalldatetime] NULL,
	[SustainabilityRankingID] [int] NULL,
	[eInvoiceWeight] [decimal](18, 4) NULL,
	[ReasonCodeDetailID] [int] NULL,
	[ReceivingDiscrepancyReasonCodeID] [int] NULL,
	[PaidCost] [money] NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedByUserId] [int] NULL,
	[AdminNotes] [varchar](5000) NULL,
	[ResolutionCodeID] [int] NULL,
	[PaymentTypeID] [int] NULL,
	[LineItemSuspended] [bit] NULL,
	[ReceivedViaGun] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK__OrderItem__6D0D32F4] PRIMARY KEY CLUSTERED 
(
	[OrderItem_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/******************************************************************************
		10. Populate FL Table in Batches
******************************************************************************/
PRINT N'Status: 10. Populate FL Table in Batches of 10000 (takes about 90 minutes in TEST for batches of 10000): --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for this Step to Complete: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 90, SYSDATETIME()), 9)
GO
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SET XACT_ABORT ON;
IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[OrderItem_Unaligned])
    BEGIN
		SET IDENTITY_INSERT [dbo].[OrderItem] ON;
        DECLARE @RowsToLoad BIGINT;
		DECLARE @RowsPerBatch INT = 10000;
		DECLARE @LeftBoundary BIGINT = 0;
		DECLARE @RightBoundary BIGINT = @RowsPerBatch;

		SELECT @RowsToLoad = MAX([OrderItem_ID]) FROM [dbo].[OrderItem_Unaligned]

		WHILE @LeftBoundary < @RowsToLoad
		BEGIN		
			INSERT INTO [dbo].[OrderItem] ([OrderItem_ID], [OrderHeader_ID], [Item_Key], [ExpirationDate], [QuantityOrdered], [QuantityUnit], [QuantityReceived], [Total_Weight], [Units_per_Pallet], [Cost], [UnitCost], [UnitExtCost], [CostUnit], [QuantityDiscount], [DiscountType], [AdjustedCost], [Handling], [HandlingUnit], [Freight], [FreightUnit], [DateReceived], [OriginalDateReceived], [Comments], [LineItemCost], [LineItemHandling], [LineItemFreight], [ReceivedItemCost], [ReceivedItemHandling], [ReceivedItemFreight], [LandedCost], [MarkupPercent], [MarkupCost], [Package_Desc1], [Package_Desc2], [Package_Unit_ID], [Retail_Unit_ID], [Origin_ID], [ReceivedFreight], [UnitsReceived], [CreditReason_ID], [QuantityAllocated], [CountryProc_ID], [Lot_No], [VendorCostHistoryID], [OHOrderDate], [SustainabilityRankingID], [NetVendorItemDiscount], [CostAdjustmentReason_ID], [Freight3Party], [LineItemFreight3Party], [HandlingCharge], [eInvoiceQuantity], [SACCost], [OrderItemCOOL], [OrderItemBIO], [Carrier], [InvoiceQuantityUnit], [InvoiceCost], [InvoiceExtendedCost], [InvoiceExtendedFreight], [InvoiceTotalWeight], [OrigReceivedItemCost], [OrigReceivedItemUnit], [CatchWeightCostPerWeight], [QuantityShipped], [WeightShipped], [eInvoiceWeight], [ReasonCodeDetailID], [ReceivingDiscrepancyReasonCodeID], [PaidCost], [ApprovedDate], [ApprovedByUserId], [AdminNotes], [ResolutionCodeID], [PaymentTypeID], [LineItemSuspended], [ReceivedViaGun])
			SELECT   src.[OrderItem_ID],
					 src.[OrderHeader_ID],
					 src.[Item_Key],
					 src.[ExpirationDate],
					 src.[QuantityOrdered],
					 src.[QuantityUnit],
					 src.[QuantityReceived],
					 src.[Total_Weight],
					 src.[Units_per_Pallet],
					 src.[Cost],
					 src.[UnitCost],
					 src.[UnitExtCost],
					 src.[CostUnit],
					 src.[QuantityDiscount],
					 src.[DiscountType],
					 src.[AdjustedCost],
					 src.[Handling],
					 src.[HandlingUnit],
					 src.[Freight],
					 src.[FreightUnit],
					 src.[DateReceived],
					 src.[OriginalDateReceived],
					 src.[Comments],
					 src.[LineItemCost],
					 src.[LineItemHandling],
					 src.[LineItemFreight],
					 src.[ReceivedItemCost],
					 src.[ReceivedItemHandling],
					 src.[ReceivedItemFreight],
					 src.[LandedCost],
					 src.[MarkupPercent],
					 src.[MarkupCost],
					 src.[Package_Desc1],
					 src.[Package_Desc2],
					 src.[Package_Unit_ID],
					 src.[Retail_Unit_ID],
					 src.[Origin_ID],
					 src.[ReceivedFreight],
					 src.[UnitsReceived],
					 src.[CreditReason_ID],
					 src.[QuantityAllocated],
					 src.[CountryProc_ID],
					 src.[Lot_No],
					 src.[VendorCostHistoryID],
					 src.[OHOrderDate],
					 src.[SustainabilityRankingID],
					 src.[NetVendorItemDiscount],
					 src.[CostAdjustmentReason_ID],
					 src.[Freight3Party],
					 src.[LineItemFreight3Party],
					 src.[HandlingCharge],
					 src.[eInvoiceQuantity],
					 src.[SACCost],
					 src.[OrderItemCOOL],
					 src.[OrderItemBIO],
					 src.[Carrier],
					 src.[InvoiceQuantityUnit],
					 src.[InvoiceCost],
					 src.[InvoiceExtendedCost],
					 src.[InvoiceExtendedFreight],
					 src.[InvoiceTotalWeight],
					 src.[OrigReceivedItemCost],
					 src.[OrigReceivedItemUnit],
					 src.[CatchWeightCostPerWeight],
					 src.[QuantityShipped],
					 src.[WeightShipped],
					 src.[eInvoiceWeight],
					 src.[ReasonCodeDetailID],
					 src.[ReceivingDiscrepancyReasonCodeID],
					 src.[PaidCost],
					 src.[ApprovedDate],
					 src.[ApprovedByUserId],
					 src.[AdminNotes],
					 src.[ResolutionCodeID],
					 src.[PaymentTypeID],
					 src.[LineItemSuspended],
					 src.[ReceivedViaGun]
			FROM     [dbo].[OrderItem_Unaligned] src
			WHERE
				src.[OrderItem_ID] > @LeftBoundary
				AND src.[OrderItem_ID] <= @RightBoundary
			ORDER BY src.[OrderItem_ID]
			
			SET @LeftBoundary = @LeftBoundary + @RowsPerBatch;
			SET @RightBoundary = @RightBoundary + @RowsPerBatch;
		END
		SET IDENTITY_INSERT [dbo].[OrderItem] OFF;
    END
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
GO
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderItem] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
SET ANSI_PADDING ON
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes (takes about 40 minutes in TEST): --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_DiscountType_NetVendorItemDiscount_OrderItem_ID] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[DiscountType] ASC,
	[NetVendorItemDiscount] ASC,
	[OrderItem_ID] ASC
)
INCLUDE ( 	[LineItemCost],
	[ReceivedItemCost],
	[ReceivedItemFreight]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_Item_Key_OrderItem_ID')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_Item_Key_OrderItem_ID] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[Item_Key] ASC,
	[OrderItem_ID] ASC
)
INCLUDE ( 	[UnitsReceived]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
	[LineItemFreight]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
	[Package_Desc2]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
	[Lot_No]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'_dta_IX_OrderItem_OrderHeader_ID_QuantityReceived_Origin_ID_OrderItem_ID_Item_Key')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderItem_OrderHeader_ID_QuantityReceived_Origin_ID_OrderItem_ID_Item_Key] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[QuantityReceived] ASC,
	[Origin_ID] ASC,
	[OrderItem_ID] ASC,
	[Item_Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'idxDiscountType')
CREATE NONCLUSTERED INDEX [idxDiscountType] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC
)
INCLUDE ( 	[Item_Key],
	[DiscountType]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'idxOrderItemIDHeaderID')
CREATE UNIQUE NONCLUSTERED INDEX [idxOrderItemIDHeaderID] ON [dbo].[OrderItem]
(
	[OrderHeader_ID] ASC,
	[OrderItem_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name = N'idxVendorCostHistoryID')
CREATE NONCLUSTERED INDEX [idxVendorCostHistoryID] ON [dbo].[OrderItem]
(
	[VendorCostHistoryID] ASC
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
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__CostU__1A34DF26] FOREIGN KEY([CostUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK__OrderItem__CostU__1A34DF26]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK__OrderItem__Freig__1B29035F] FOREIGN KEY([FreightUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK__OrderItem__Freig__1B29035F]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK__OrderItem__Handl__1C1D2798] FOREIGN KEY([HandlingUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK__OrderItem__Handl__1C1D2798]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Order__1FEDB87C]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Order__1FEDB87C] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Order__1FEDB87C]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK__OrderItem__Order__1FEDB87C]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Origi__20E1DCB5]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK__OrderItem__Origi__20E1DCB5] FOREIGN KEY([Origin_ID])
REFERENCES [dbo].[ItemOrigin] ([Origin_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Origi__20E1DCB5]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK__OrderItem__Origi__20E1DCB5]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Quant__21D600EE] FOREIGN KEY([QuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK__OrderItem__Quant__21D600EE]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_CostAdjustmentReason]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_CostAdjustmentReason] FOREIGN KEY([CostAdjustmentReason_ID])
REFERENCES [dbo].[CostAdjustmentReason] ([CostAdjustmentReason_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_CostAdjustmentReason]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_CostAdjustmentReason]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Item]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemOrigin_CountryProc_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_ItemOrigin_CountryProc_ID] FOREIGN KEY([CountryProc_ID])
REFERENCES [dbo].[ItemOrigin] ([Origin_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemOrigin_CountryProc_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_ItemOrigin_CountryProc_ID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_ItemUnit]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit1] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_ItemUnit1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit2] FOREIGN KEY([InvoiceQuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_ItemUnit2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_ReasonCodeDetail] FOREIGN KEY([ReasonCodeDetailID])
REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_ReasonCodeDetail]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail] FOREIGN KEY([ReceivingDiscrepancyReasonCodeID])
REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_ReceivingDiscrepancyReasonCodeDetail]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_SustainabilityRanking]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_SustainabilityRanking] FOREIGN KEY([SustainabilityRankingID])
REFERENCES [dbo].[SustainabilityRanking] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_SustainabilityRanking]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_SustainabilityRanking]
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		16. Check FL Checks (manually generated)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks (takes about 30 minutes in TEST): --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables (takes about 5 minutes in TEST): --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM orderitem_unaligned(NOLOCK)

SELECT @new = count(*)
FROM orderitem(NOLOCK)

PRINT N'OrderItem_Unaligned Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'OrderItem Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count. --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count. --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END