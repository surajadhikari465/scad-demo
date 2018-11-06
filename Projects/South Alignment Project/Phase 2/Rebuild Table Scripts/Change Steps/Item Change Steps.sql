/******************************************************************************
		SO [dbo].[Item]
		Change Steps
******************************************************************************/
PRINT N'Status: Begin [dbo].[Item]  Rebuild --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 5, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[Item] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Package_De__0CA1479E]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Package_De__0D956BD7]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Min_Temper__0F7DB449]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Max_Temper__1071D882]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Units_Per___1165FCBB]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Tie__125A20F4]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__High__134E452D]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Yield__14426966]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__ShelfLife___19FB42BC]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Deleted_It__219C6484]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__HIAH_Item__2478D12F]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Not_Availa__266119A1]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Pre_Order__27553DDA]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Remove_Ite__2B25CEBE]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Average_Co__2FEA83DB]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Organic__31D2CC4D]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Refrigerat__32C6F086]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Keep_Froze__33BB14BF]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Shipper_It__35A35D31]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Full_Palle__3697816A]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Retail_Sal__3973EE15]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Food_Stamp__3A68124E]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Discountab__3B5C3687]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Price_Requ__41150FDD]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Quantity_R__42093416]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__ItemType_I__43F17C88]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__item__HFM_Item__32695FD8]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_Insert_Date]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_EXEDistributed]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_CostedByWeight]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__CatchWtReq__0B627115]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_Ingredient]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_InnerPack]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_COOL]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_BIO]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__Sustainabi__76C81239]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_ITEM_Case_Discount]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_ITEM_Coupon_Multipler]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_Recall_Flag]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_LockAuth]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_ITEM_PurchaseThresholdCouponSubTeam]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_CatchweightRequired]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF__Item__FSA_Eligib__4BD7BD10]
GO
ALTER TABLE [Item] DROP CONSTRAINT [DF_Item_GiftCard]
GO
/******************************************************************************
		3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'CaseDistMarkupDollars'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'CaseDistMarkupDollars'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'ClassID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'ClassID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'EXEDistributed'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'EXEDistributed'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Manufacturing_Unit_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Manufacturing_Unit_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Insert_Date'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Insert_Date'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'CountryProc_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'CountryProc_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Not_AvailableNote'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Not_AvailableNote'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'ScaleDesc2'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'ScaleDesc2'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'ScaleDesc1'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'ScaleDesc1'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'HFM_Item'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'HFM_Item'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'ItemType_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'ItemType_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Quantity_Required'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Quantity_Required'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Price_Required'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Price_Required'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Discountable'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Discountable'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Food_Stamps'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Food_Stamps'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Retail_Sale'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Retail_Sale'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'POS_Description'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'POS_Description'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'User_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'User_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Full_Pallet_Only'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Full_Pallet_Only'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Shipper_Item'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Shipper_Item'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Keep_Frozen'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Keep_Frozen'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Refrigerated'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Refrigerated'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Organic'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Organic'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'NoDistMarkup'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'NoDistMarkup'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Remove_Item'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Remove_Item'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Pre_Order'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Pre_Order'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Not_Available'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Not_Available'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'WFM_Item'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'WFM_Item'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Deleted_Item'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Deleted_Item'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Freight_Unit_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Freight_Unit_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Cost_Unit_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Cost_Unit_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Distribution_Unit_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Distribution_Unit_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Vendor_Unit_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Vendor_Unit_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Retail_Unit_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Retail_Unit_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'ShelfLife_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'ShelfLife_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'ShelfLife_Length'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'ShelfLife_Length'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Origin_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Origin_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Category_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Category_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Brand_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Brand_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Yield'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Yield'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'High'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'High'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Tie'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Tie'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Average_Unit_Weight'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Average_Unit_Weight'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Units_Per_Pallet'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Units_Per_Pallet'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Max_Temperature'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Max_Temperature'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Min_Temperature'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Min_Temperature'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Package_Unit_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Package_Unit_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Package_Desc2'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Package_Desc2'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Package_Desc1'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Package_Desc1'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Sales_Account'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Sales_Account'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'SubTeam_No'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'SubTeam_No'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Ingredients'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Ingredients'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Sign_Description'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Sign_Description'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Item_Description'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Item_Description'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Item', N'COLUMN',N'Item_Key'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Item', @level2type=N'COLUMN',@level2name=N'Item_Key'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemUpdate]'))
DROP TRIGGER [dbo].[ItemUpdate]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemDelete]'))
DROP TRIGGER [dbo].[ItemDelete]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemAdd]'))
DROP TRIGGER [dbo].[ItemAdd]
GO
/******************************************************************************
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_TaxClass1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_TaxClass1]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SustainabilityRanking]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_SustainabilityRanking]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_SubTeam]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_StoreJurisdictionID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_StoreJurisdictionID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_ProdHierarchyLevel4]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_ProdHierarchyLevel4]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_LabelType_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_LabelType_ID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_ItemOrigin_CountryProc_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_ItemOrigin_CountryProc_ID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Vendor_Uni__1DCBD3A0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__User_ID__378BA5A3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__User_ID__378BA5A3]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__SubTeam_No__0BAD2365]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__SubTeam_No__0BAD2365]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__ShelfLife___1AEF66F5]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__ShelfLife___1AEF66F5]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Retail_Uni__1BE38B2E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Package_Un__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Package_Un__0E899010]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Origin_ID__1812FA4A]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Origin_ID__1812FA4A]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Manager__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Manager__0E899010]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Freight_Un__20A8404B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Freight_Un__20A8404B]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Distributi__1EBFF7D9]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Distributi__1EBFF7D9]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Cost_Unit___1FB41C12]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Cost_Unit___1FB41C12]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Category_I__162AB1D8]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Category_I__162AB1D8]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Brand_ID__15368D9F]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Brand_ID__15368D9F]
GO
/******************************************************************************
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemUserLock')
DROP INDEX [idxItemUserLock] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemUserID')
DROP INDEX [idxItemUserID] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemSubTeamNo')
DROP INDEX [idxItemSubTeamNo] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemShelfLife')
DROP INDEX [idxItemShelfLife] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemRetailUnit')
DROP INDEX [idxItemRetailUnit] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemRemoveItem')
DROP INDEX [idxItemRemoveItem] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemPOSDescription')
DROP INDEX [idxItemPOSDescription] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemPackageUnit')
DROP INDEX [idxItemPackageUnit] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemOrigin')
DROP INDEX [idxItemOrigin] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemOrderingUnit')
DROP INDEX [idxItemOrderingUnit] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemOrderAlloc2')
DROP INDEX [idxItemOrderAlloc2] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemOrderAlloc1')
DROP INDEX [idxItemOrderAlloc1] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemMicrostrategyReporting2')
DROP INDEX [idxItemMicrostrategyReporting2] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemMicrostrategyReporting1')
DROP INDEX [idxItemMicrostrategyReporting1] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemIngredient2')
DROP INDEX [idxItemIngredient2] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemIngredient1')
DROP INDEX [idxItemIngredient1] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemFreightUnitID')
DROP INDEX [idxItemFreightUnitID] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemEXEDistributed2')
DROP INDEX [idxItemEXEDistributed2] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemEXEDistributed')
DROP INDEX [idxItemEXEDistributed] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemDistributionUnit')
DROP INDEX [idxItemDistributionUnit] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemDescription')
DROP INDEX [idxItemDescription] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemDeleted')
DROP INDEX [idxItemDeleted] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemCycleCountReport')
DROP INDEX [idxItemCycleCountReport] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemCountryProc_ID')
DROP INDEX [idxItemCountryProc_ID] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemCostUnitID')
DROP INDEX [idxItemCostUnitID] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemCategory')
DROP INDEX [idxItemCategory] ON [dbo].[Item]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemBrand')
DROP INDEX [idxItemBrand] ON [dbo].[Item]
GO
/******************************************************************************
		7. Rename SO PK 
******************************************************************************/
PRINT N'Status: 7. Rename SO PK  --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[PK_Item_ItemKey]', N'PK_Item_ItemKey_Unaligned';
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[Item]', N'Item_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Item](
	[Item_Key] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Item_Description] [varchar](60) NOT NULL,
	[Sign_Description] [varchar](60) NOT NULL,
	[Ingredients] [varchar](3500) NULL,
	[SubTeam_No] [int] NOT NULL,
	[Sales_Account] [varchar](6) NULL,
	[Package_Desc1] [decimal](9, 4) NOT NULL CONSTRAINT [DF__Item__Package_De__0CA1479E]  DEFAULT ((0)),
	[Package_Desc2] [decimal](9, 4) NOT NULL CONSTRAINT [DF__Item__Package_De__0D956BD7]  DEFAULT ((0)),
	[Package_Unit_ID] [int] NOT NULL,
	[Min_Temperature] [smallint] NOT NULL CONSTRAINT [DF__Item__Min_Temper__0F7DB449]  DEFAULT ((0)),
	[Max_Temperature] [smallint] NOT NULL CONSTRAINT [DF__Item__Max_Temper__1071D882]  DEFAULT ((0)),
	[Units_Per_Pallet] [smallint] NOT NULL CONSTRAINT [DF__Item__Units_Per___1165FCBB]  DEFAULT ((0)),
	[Average_Unit_Weight] [decimal](9, 4) NULL,
	[Tie] [tinyint] NOT NULL CONSTRAINT [DF__Item__Tie__125A20F4]  DEFAULT ((0)),
	[High] [tinyint] NOT NULL CONSTRAINT [DF__Item__High__134E452D]  DEFAULT ((0)),
	[Yield] [decimal](9, 4) NOT NULL CONSTRAINT [DF__Item__Yield__14426966]  DEFAULT ((100)),
	[Brand_ID] [int] NULL,
	[Category_ID] [int] NULL,
	[Origin_ID] [int] NULL,
	[ShelfLife_Length] [smallint] NOT NULL CONSTRAINT [DF__Item__ShelfLife___19FB42BC]  DEFAULT ((0)),
	[ShelfLife_ID] [int] NULL,
	[Retail_Unit_ID] [int] NOT NULL,
	[Vendor_Unit_ID] [int] NOT NULL,
	[Distribution_Unit_ID] [int] NOT NULL,
	[Cost_Unit_ID] [int] NULL,
	[Freight_Unit_ID] [int] NULL,
	[Deleted_Item] [bit] NOT NULL CONSTRAINT [DF__Item__Deleted_It__219C6484]  DEFAULT ((0)),
	[WFM_Item] [bit] NOT NULL CONSTRAINT [DF__Item__HIAH_Item__2478D12F]  DEFAULT ((1)),
	[Not_Available] [bit] NOT NULL CONSTRAINT [DF__Item__Not_Availa__266119A1]  DEFAULT ((0)),
	[Pre_Order] [bit] NOT NULL CONSTRAINT [DF__Item__Pre_Order__27553DDA]  DEFAULT ((0)),
	[Remove_Item] [tinyint] NOT NULL CONSTRAINT [DF__Item__Remove_Ite__2B25CEBE]  DEFAULT ((0)),
	[NoDistMarkup] [bit] NOT NULL CONSTRAINT [DF__Item__Average_Co__2FEA83DB]  DEFAULT ((0)),
	[Organic] [bit] NOT NULL CONSTRAINT [DF__Item__Organic__31D2CC4D]  DEFAULT ((0)),
	[Refrigerated] [bit] NOT NULL CONSTRAINT [DF__Item__Refrigerat__32C6F086]  DEFAULT ((0)),
	[Keep_Frozen] [bit] NOT NULL CONSTRAINT [DF__Item__Keep_Froze__33BB14BF]  DEFAULT ((0)),
	[Shipper_Item] [bit] NOT NULL CONSTRAINT [DF__Item__Shipper_It__35A35D31]  DEFAULT ((0)),
	[Full_Pallet_Only] [bit] NOT NULL CONSTRAINT [DF__Item__Full_Palle__3697816A]  DEFAULT ((0)),
	[User_ID] [int] NULL,
	[POS_Description] [varchar](26) NOT NULL,
	[Retail_Sale] [bit] NOT NULL CONSTRAINT [DF__Item__Retail_Sal__3973EE15]  DEFAULT ((0)),
	[Food_Stamps] [bit] NOT NULL CONSTRAINT [DF__Item__Food_Stamp__3A68124E]  DEFAULT ((0)),
	[Discountable] [bit] NOT NULL CONSTRAINT [DF__Item__Discountab__3B5C3687]  DEFAULT ((0)),
	[Price_Required] [bit] NOT NULL CONSTRAINT [DF__Item__Price_Requ__41150FDD]  DEFAULT ((0)),
	[Quantity_Required] [bit] NOT NULL CONSTRAINT [DF__Item__Quantity_R__42093416]  DEFAULT ((0)),
	[ItemType_ID] [int] NOT NULL CONSTRAINT [DF__Item__ItemType_I__43F17C88]  DEFAULT ((0)),
	[HFM_Item] [bit] NOT NULL CONSTRAINT [DF__item__HFM_Item__32695FD8]  DEFAULT ((0)),
	[ScaleDesc1] [varchar](64) NULL,
	[ScaleDesc2] [varchar](64) NULL,
	[Not_AvailableNote] [varchar](255) NULL,
	[CountryProc_ID] [int] NULL,
	[Insert_Date] [datetime] NOT NULL CONSTRAINT [DF_Item_Insert_Date]  DEFAULT (getdate()),
	[Manufacturing_Unit_ID] [int] NULL,
	[EXEDistributed] [bit] NOT NULL CONSTRAINT [DF_Item_EXEDistributed]  DEFAULT ((0)),
	[ClassID] [int] NULL,
	[User_ID_Date] [datetime] NULL,
	[DistSubTeam_No] [int] NULL,
	[CostedByWeight] [bit] NOT NULL CONSTRAINT [DF_Item_CostedByWeight]  DEFAULT ((0)),
	[TaxClassID] [int] NULL,
	[LabelType_ID] [int] NULL,
	[ScaleDesc3] [varchar](64) NULL,
	[ScaleDesc4] [varchar](64) NULL,
	[ScaleTare] [int] NULL,
	[ScaleUseBy] [int] NULL,
	[ScaleForcedTare] [char](1) NULL,
	[QtyProhibit] [bit] NULL,
	[GroupList] [int] NULL,
	[ProdHierarchyLevel4_ID] [int] NULL,
	[Case_Discount] [bit] NULL,
	[Coupon_Multiplier] [bit] NULL,
	[Misc_Transaction_Sale] [smallint] NULL,
	[Misc_Transaction_Refund] [smallint] NULL,
	[Recall_Flag] [bit] NULL CONSTRAINT [DF_Item_Recall_Flag]  DEFAULT ((0)),
	[Manager_ID] [tinyint] NULL,
	[Ice_Tare] [int] NULL,
	[LockAuth] [bit] NULL CONSTRAINT [DF_Item_LockAuth]  DEFAULT ((0)),
	[PurchaseThresholdCouponAmount] [smallmoney] NULL,
	[PurchaseThresholdCouponSubTeam] [bit] NULL,
	[Product_Code] [varchar](15) NULL,
	[Unit_Price_Category] [int] NULL,
	[StoreJurisdictionID] [int] NULL,
	[CatchweightRequired] [bit] NOT NULL CONSTRAINT [DF_Item_CatchweightRequired]  DEFAULT ((0)),
	[COOL] [bit] NOT NULL DEFAULT ((0)),
	[BIO] [bit] NOT NULL DEFAULT ((0)),
	[LastModifiedUser_ID] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CatchWtReq] [bit] NOT NULL CONSTRAINT [DF_Item_CatchWtReq]  DEFAULT ((0)),
	[SustainabilityRankingRequired] [bit] NULL DEFAULT ((0)),
	[SustainabilityRankingID] [int] NULL,
	[Ingredient] [bit] NOT NULL DEFAULT ((0)),
	[FSA_Eligible] [bit] NOT NULL DEFAULT ((0)),
	[TaxClassModifiedDate] [datetime] NULL,
	[UseLastReceivedCost] [bit] NULL,
	[GiftCard] [bit] NOT NULL CONSTRAINT [DF_Item_GiftCard]  DEFAULT ((0)),
 CONSTRAINT [PK_Item_ItemKey] PRIMARY KEY CLUSTERED 
(
	[Item_Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/******************************************************************************
		10. Populate FL Table in Batches
******************************************************************************/
PRINT N'Status: 10. Populate FL Table in Batches (takes about 15 seconds in TEST for batches of 5000): --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for this Step to Complete: --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 1, SYSDATETIME()), 9)
GO
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SET XACT_ABORT ON;
IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Item_Unaligned])
    BEGIN
		SET IDENTITY_INSERT [dbo].[Item] ON;
        DECLARE @RowsToLoad BIGINT;
		DECLARE @RowsPerBatch INT = 5000;
		DECLARE @LeftBoundary BIGINT = 0;
		DECLARE @RightBoundary BIGINT = @RowsPerBatch;

		SELECT @RowsToLoad = MAX([Item_Key]) FROM [dbo].[Item_Unaligned]

		WHILE @LeftBoundary < @RowsToLoad
		BEGIN		
			SET IDENTITY_INSERT [dbo].[Item] ON;
			INSERT INTO [dbo].[Item] ([Item_Key], [Item_Description], [Sign_Description], [Ingredients], [SubTeam_No], [Sales_Account], [Package_Desc1], [Package_Desc2], [Package_Unit_ID], [Min_Temperature], [Max_Temperature], [Units_Per_Pallet], [Average_Unit_Weight], [Tie], [High], [Yield], [Brand_ID], [Category_ID], [Origin_ID], [ShelfLife_Length], [ShelfLife_ID], [Retail_Unit_ID], [Vendor_Unit_ID], [Distribution_Unit_ID], [Cost_Unit_ID], [Freight_Unit_ID], [Deleted_Item], [WFM_Item], [Not_Available], [Pre_Order], [Remove_Item], [NoDistMarkup], [Organic], [Refrigerated], [Keep_Frozen], [Shipper_Item], [Full_Pallet_Only], [User_ID], [POS_Description], [Retail_Sale], [Food_Stamps], [Discountable], [Price_Required], [Quantity_Required], [ItemType_ID], [HFM_Item], [ScaleDesc1], [ScaleDesc2], [Not_AvailableNote], [CountryProc_ID], [Insert_Date], [Manufacturing_Unit_ID], [EXEDistributed], [ClassID], [User_ID_Date], [DistSubTeam_No], [CostedByWeight], [CatchWtReq], [Ingredient], [COOL], [BIO], [SustainabilityRankingRequired], [SustainabilityRankingID], [TaxClassID], [LabelType_ID], [ScaleDesc3], [ScaleDesc4], [ScaleTare], [ScaleUseBy], [ScaleForcedTare], [QtyProhibit], [GroupList], [ProdHierarchyLevel4_ID], [Case_Discount], [Coupon_Multiplier], [Misc_Transaction_Sale], [Misc_Transaction_Refund], [Recall_Flag], [Manager_ID], [Ice_Tare], [LockAuth], [PurchaseThresholdCouponAmount], [PurchaseThresholdCouponSubTeam], [Product_Code], [Unit_Price_Category], [StoreJurisdictionID], [CatchweightRequired], [LastModifiedUser_ID], [LastModifiedDate], [FSA_Eligible], [TaxClassModifiedDate], [UseLastReceivedCost], [GiftCard])
			SELECT   src.[Item_Key],
					 src.[Item_Description],
					 src.[Sign_Description],
					 src.[Ingredients],
					 src.[SubTeam_No],
					 src.[Sales_Account],
					 src.[Package_Desc1],
					 src.[Package_Desc2],
					 src.[Package_Unit_ID],
					 src.[Min_Temperature],
					 src.[Max_Temperature],
					 src.[Units_Per_Pallet],
					 src.[Average_Unit_Weight],
					 src.[Tie],
					 src.[High],
					 src.[Yield],
					 src.[Brand_ID],
					 src.[Category_ID],
					 src.[Origin_ID],
					 src.[ShelfLife_Length],
					 src.[ShelfLife_ID],
					 src.[Retail_Unit_ID],
					 src.[Vendor_Unit_ID],
					 src.[Distribution_Unit_ID],
					 src.[Cost_Unit_ID],
					 src.[Freight_Unit_ID],
					 src.[Deleted_Item],
					 src.[WFM_Item],
					 src.[Not_Available],
					 src.[Pre_Order],
					 src.[Remove_Item],
					 src.[NoDistMarkup],
					 src.[Organic],
					 src.[Refrigerated],
					 src.[Keep_Frozen],
					 src.[Shipper_Item],
					 src.[Full_Pallet_Only],
					 src.[User_ID],
					 src.[POS_Description],
					 src.[Retail_Sale],
					 src.[Food_Stamps],
					 src.[Discountable],
					 src.[Price_Required],
					 src.[Quantity_Required],
					 src.[ItemType_ID],
					 src.[HFM_Item],
					 src.[ScaleDesc1],
					 src.[ScaleDesc2],
					 src.[Not_AvailableNote],
					 src.[CountryProc_ID],
					 src.[Insert_Date],
					 src.[Manufacturing_Unit_ID],
					 src.[EXEDistributed],
					 src.[ClassID],
					 src.[User_ID_Date],
					 src.[DistSubTeam_No],
					 src.[CostedByWeight],
					 src.[CatchWtReq],
					 src.[Ingredient],
					 src.[COOL],
					 src.[BIO],
					 src.[SustainabilityRankingRequired],
					 src.[SustainabilityRankingID],
					 src.[TaxClassID],
					 src.[LabelType_ID],
					 src.[ScaleDesc3],
					 src.[ScaleDesc4],
					 src.[ScaleTare],
					 src.[ScaleUseBy],
					 src.[ScaleForcedTare],
					 src.[QtyProhibit],
					 src.[GroupList],
					 src.[ProdHierarchyLevel4_ID],
					 src.[Case_Discount],
					 src.[Coupon_Multiplier],
					 src.[Misc_Transaction_Sale],
					 src.[Misc_Transaction_Refund],
					 src.[Recall_Flag],
					 src.[Manager_ID],
					 src.[Ice_Tare],
					 src.[LockAuth],
					 src.[PurchaseThresholdCouponAmount],
					 src.[PurchaseThresholdCouponSubTeam],
					 src.[Product_Code],
					 src.[Unit_Price_Category],
					 src.[StoreJurisdictionID],
					 src.[CatchweightRequired],
					 src.[LastModifiedUser_ID],
					 src.[LastModifiedDate],
					 src.[FSA_Eligible],
					 src.[TaxClassModifiedDate],
					 src.[UseLastReceivedCost],
					 src.[GiftCard]
			FROM     [dbo].[Item_Unaligned] src
			WHERE
				src.[Item_Key] > @LeftBoundary
				AND src.[Item_Key] <= @RightBoundary
			ORDER BY src.[Item_Key] ASC;
			
			SET @LeftBoundary = @LeftBoundary + @RowsPerBatch;
			SET @RightBoundary = @RightBoundary + @RowsPerBatch;
		END
		SET IDENTITY_INSERT [dbo].[Item] OFF;
    END
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
GO
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[Item] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemBrand')
CREATE NONCLUSTERED INDEX [idxItemBrand] ON [dbo].[Item]
(
	[Brand_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemCategory')
CREATE NONCLUSTERED INDEX [idxItemCategory] ON [dbo].[Item]
(
	[Category_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemCostUnitID')
CREATE NONCLUSTERED INDEX [idxItemCostUnitID] ON [dbo].[Item]
(
	[Cost_Unit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemCountryProc_ID')
CREATE NONCLUSTERED INDEX [idxItemCountryProc_ID] ON [dbo].[Item]
(
	[CountryProc_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemDescription')
CREATE NONCLUSTERED INDEX [idxItemDescription] ON [dbo].[Item]
(
	[Item_Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemDistributionUnit')
CREATE NONCLUSTERED INDEX [idxItemDistributionUnit] ON [dbo].[Item]
(
	[Distribution_Unit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemFreightUnitID')
CREATE NONCLUSTERED INDEX [idxItemFreightUnitID] ON [dbo].[Item]
(
	[Freight_Unit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemOrderingUnit')
CREATE NONCLUSTERED INDEX [idxItemOrderingUnit] ON [dbo].[Item]
(
	[Vendor_Unit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemOrigin')
CREATE NONCLUSTERED INDEX [idxItemOrigin] ON [dbo].[Item]
(
	[Origin_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemPackageUnit')
CREATE NONCLUSTERED INDEX [idxItemPackageUnit] ON [dbo].[Item]
(
	[Package_Unit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemPOSDescription')
CREATE NONCLUSTERED INDEX [idxItemPOSDescription] ON [dbo].[Item]
(
	[POS_Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemRemoveItem')
CREATE NONCLUSTERED INDEX [idxItemRemoveItem] ON [dbo].[Item]
(
	[Remove_Item] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemRetailUnit')
CREATE NONCLUSTERED INDEX [idxItemRetailUnit] ON [dbo].[Item]
(
	[Retail_Unit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemShelfLife')
CREATE NONCLUSTERED INDEX [idxItemShelfLife] ON [dbo].[Item]
(
	[ShelfLife_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemSubTeamNo')
CREATE NONCLUSTERED INDEX [idxItemSubTeamNo] ON [dbo].[Item]
(
	[SubTeam_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND name = N'idxItemUserID')
CREATE NONCLUSTERED INDEX [idxItemUserID] ON [dbo].[Item]
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Brand_ID__15368D9F]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Brand_ID__15368D9F] FOREIGN KEY([Brand_ID])
REFERENCES [dbo].[ItemBrand] ([Brand_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Brand_ID__15368D9F]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Brand_ID__15368D9F]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Category_I__162AB1D8]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Category_I__162AB1D8] FOREIGN KEY([Category_ID])
REFERENCES [dbo].[ItemCategory] ([Category_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Category_I__162AB1D8]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Category_I__162AB1D8]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Cost_Unit___1FB41C12]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Cost_Unit___1FB41C12] FOREIGN KEY([Cost_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Cost_Unit___1FB41C12]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Cost_Unit___1FB41C12]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Distributi__1EBFF7D9]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Distributi__1EBFF7D9] FOREIGN KEY([Distribution_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Distributi__1EBFF7D9]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Distributi__1EBFF7D9]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Freight_Un__20A8404B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Freight_Un__20A8404B] FOREIGN KEY([Freight_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Freight_Un__20A8404B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Freight_Un__20A8404B]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Manager__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Manager__0E899010] FOREIGN KEY([Manager_ID])
REFERENCES [dbo].[ItemManager] ([Manager_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Manager__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Manager__0E899010]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Origin_ID__1812FA4A]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Origin_ID__1812FA4A] FOREIGN KEY([Origin_ID])
REFERENCES [dbo].[ItemOrigin] ([Origin_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Origin_ID__1812FA4A]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Origin_ID__1812FA4A]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Package_Un__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Package_Un__0E899010] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Package_Un__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Package_Un__0E899010]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Retail_Uni__1BE38B2E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Retail_Uni__1BE38B2E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__ShelfLife___1AEF66F5]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__ShelfLife___1AEF66F5] FOREIGN KEY([ShelfLife_ID])
REFERENCES [dbo].[ItemShelfLife] ([ShelfLife_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__ShelfLife___1AEF66F5]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__ShelfLife___1AEF66F5]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__SubTeam_No__0BAD2365]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__SubTeam_No__0BAD2365] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__SubTeam_No__0BAD2365]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__SubTeam_No__0BAD2365]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__User_ID__378BA5A3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__User_ID__378BA5A3] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__User_ID__378BA5A3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__User_ID__378BA5A3]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Vendor_Uni__1DCBD3A0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0] FOREIGN KEY([Vendor_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Vendor_Uni__1DCBD3A0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_ItemOrigin_CountryProc_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemOrigin_CountryProc_ID] FOREIGN KEY([CountryProc_ID])
REFERENCES [dbo].[ItemOrigin] ([Origin_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_ItemOrigin_CountryProc_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_ItemOrigin_CountryProc_ID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_LabelType_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_LabelType_ID] FOREIGN KEY([LabelType_ID])
REFERENCES [dbo].[LabelType] ([LabelType_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_LabelType_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_LabelType_ID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_ProdHierarchyLevel4]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ProdHierarchyLevel4] FOREIGN KEY([ProdHierarchyLevel4_ID])
REFERENCES [dbo].[ProdHierarchyLevel4] ([ProdHierarchyLevel4_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_ProdHierarchyLevel4]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_ProdHierarchyLevel4]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_StoreJurisdictionID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_StoreJurisdictionID] FOREIGN KEY([StoreJurisdictionID])
REFERENCES [dbo].[StoreJurisdiction] ([StoreJurisdictionID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_StoreJurisdictionID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_StoreJurisdictionID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_SubTeam] FOREIGN KEY([DistSubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_SubTeam]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SustainabilityRanking]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH NOCHECK ADD  CONSTRAINT [FK_Item_SustainabilityRanking] FOREIGN KEY([SustainabilityRankingID])
REFERENCES [dbo].[SustainabilityRanking] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SustainabilityRanking]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK_Item_SustainabilityRanking]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_TaxClass1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_TaxClass1] FOREIGN KEY([TaxClassID])
REFERENCES [dbo].[TaxClass] ([TaxClassID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_TaxClass1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_TaxClass1]
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemAdd]'))
EXEC dbo.sp_executesql @statement = N'CREATE Trigger [dbo].[ItemAdd] 
ON [dbo].[Item]
FOR INSERT
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
    -- Queue for Price Modeling if necessary
    INSERT INTO PMProductChg (HierLevel, Item_Key, ItemDescription, ParentID, ParentDescription, ActionID, Status)
    SELECT ''Product'', Inserted.Item_Key, Inserted.Item_Description, 
           ISNULL(ItemCategory.Category_ID, CONVERT(varchar(255), Inserted.SubTeam_No) + ''1''), ISNULL(Category_Name, ''NO CATEGORY''), 
           ''ADD'', ''ACTIVE''
    FROM Inserted
    LEFT JOIN
        ItemCategory
        ON Inserted.Category_ID = ItemCategory.Category_ID
    WHERE Inserted.Retail_Sale = 1
          AND (Inserted.SubTeam_No IN (SELECT SubTeam_No FROM PMSubTeamInclude (nolock)))
    SELECT @Error_No = @@ERROR
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR (''ItemAdd trigger failed with @@ERROR: %d'', @Severity, 1, @Error_No)
    END
END' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemDelete]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[ItemDelete]
ON [dbo].[Item]
FOR DELETE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
	INSERT INTO PMProductChg (HierLevel, Item_Key, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT ''Product'', Deleted.Item_Key, Identifier, Item_Description, 
           ISNULL(ItemCategory.Category_ID, CONVERT(varchar(255), Deleted.SubTeam_No) + ''1''), ISNULL(Category_Name, ''NO CATEGORY''), 
           ''DELETE''
    FROM Deleted
        LEFT JOIN
            ItemIdentifier II
            ON Deleted.Item_Key = II.Item_Key AND Default_Identifier = 1
        LEFT JOIN
            ItemCategory
            ON Deleted.Category_ID = ItemCategory.Category_ID
    WHERE Deleted.Retail_Sale = 1
          AND Deleted.SubTeam_No IN (SELECT SubTeam_No FROM PMSubTeamInclude (nolock))
          AND NOT EXISTS (SELECT * FROM PMExcludedItem WHERE Item_Key = Deleted.Item_Key)
    SELECT @Error_No = @@ERROR
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR (''ItemDelete trigger failed with @@ERROR: %d'', @Severity, 1, @Error_No)
    END
END' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemUpdate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[ItemUpdate]
ON [dbo].[Item] FOR UPDATE
AS
BEGIN
	IF 0 = (SELECT COUNT(*) FROM dbo.SupportRestoreDeletedItemsItemKeys WHERE Item_Key IN (SELECT Item_Key FROM INSERTED))
	BEGIN
		DECLARE 
			@error_no int = 0, 	
			@IconControllerUserId int = (SELECT User_ID FROM Users WHERE UserName = ''iconcontrolleruser'')
		DECLARE @Identifiers TABLE (Item_Key int, Identifier varchar(13), IdentifierType varchar(3));
		
		-- Add item to EXE item change queue table if it is supplied by a warehouse with the EXE system installed
		INSERT INTO WarehouseItemChange
		(
			Store_No,
			Item_Key,
			ChangeType
		)
		SELECT 
			Supplier_Store_No,
			INSERTED.Item_Key,
			CASE 
				WHEN (INSERTED.Deleted_Item = 1) OR (INSERTED.EXEDistributed = 0) THEN 
						''D''
				ELSE CASE 
							WHEN INSERTED.EXEDistributed <> DELETED.EXEDistributed THEN 
								''A''
							ELSE ''M''
						END
			END
		FROM   
			INSERTED
			INNER JOIN DELETED
				ON  DELETED.Item_Key = INSERTED.Item_Key
			INNER JOIN (
					SELECT Supplier_Store_No,
							SubTeam_No
					FROM   ZoneSubTeam Z(NOLOCK)
							INNER JOIN Store(NOLOCK)
								ON  Store.Store_No = Z.Supplier_Store_No
					WHERE  EXEWarehouse IS NOT NULL
					GROUP BY
							Supplier_Store_No,
							SubTeam_No
				) ZS
				ON  ZS.SubTeam_No = INSERTED.SubTeam_No
		WHERE  
			(
				INSERTED.Deleted_Item <> DELETED.Deleted_Item
				OR INSERTED.Not_Available <> DELETED.Not_Available
				OR INSERTED.Item_Description <> DELETED.Item_Description
				OR INSERTED.POS_Description <> DELETED.POS_Description
				OR INSERTED.Package_Desc2 <> DELETED.Package_Desc2
				OR INSERTED.Package_Desc1 <> DELETED.Package_Desc1
				OR INSERTED.EXEDistributed <> DELETED.EXEDistributed
				OR INSERTED.Cool <> DELETED.Cool
				OR INSERTED.Bio <> DELETED.Bio
				OR INSERTED.[CatchweightRequired] <> DELETED.[CatchweightRequired]
			)
			AND (INSERTED.EXEDistributed = 1 OR DELETED.EXEDistributed = 1)
			AND INSERTED.SubTeam_No = DELETED.SubTeam_No
		UNION
		SELECT 
			Supplier_Store_No,
			INSERTED.Item_Key,
			''A''
		FROM   
			INSERTED
			INNER JOIN DELETED
				ON  DELETED.Item_Key = INSERTED.Item_Key
			INNER JOIN (
					SELECT Supplier_Store_No,
							SubTeam_No
					FROM   ZoneSubTeam Z(NOLOCK)
							INNER JOIN Store(NOLOCK)
								ON  Store.Store_No = Z.Supplier_Store_No
					WHERE  EXEWarehouse IS NOT NULL
					GROUP BY
							Supplier_Store_No,
							SubTeam_No
				) ZS
				ON  ZS.SubTeam_No = INSERTED.SubTeam_No
		WHERE  
			INSERTED.SubTeam_No <> DELETED.SubTeam_No
			AND INSERTED.Deleted_Item = 0
			AND INSERTED.EXEDistributed = 1
		UNION
		SELECT 
			Supplier_Store_No,
			INSERTED.Item_Key,
			''D''
		FROM   
			INSERTED
			INNER JOIN DELETED
				ON  DELETED.Item_Key = INSERTED.Item_Key
			INNER JOIN (
					SELECT Supplier_Store_No,
							SubTeam_No
					FROM   ZoneSubTeam Z(NOLOCK)
							INNER JOIN Store(NOLOCK)
								ON  Store.Store_No = Z.Supplier_Store_No
					WHERE  EXEWarehouse IS NOT NULL
					GROUP BY
							Supplier_Store_No,
							SubTeam_No
				) ZS
				ON  ZS.SubTeam_No = DELETED.SubTeam_No
		WHERE  
			INSERTED.SubTeam_No <> DELETED.SubTeam_No
			AND DELETED.EXEDistributed = 1
	
		SELECT @error_no = @@ERROR
	
		IF @error_no = 0
			BEGIN -- Queue for Price Modeling if necessary			
				INSERT INTO PMProductChg
				(
					HierLevel,
					Item_Key,
					ItemID,
					ItemDescription,
					ParentID,
					ParentDescription,
					ActionID,
					STATUS
				)
				SELECT 
					''Product'',
					INSERTED.Item_Key,
					Identifier,
					INSERTED.Item_Description,
					ISNULL(
						ItemCategory.Category_ID,
						CONVERT(VARCHAR(255), INSERTED.SubTeam_No) + ''1''
					),
					ISNULL(Category_Name, ''NO CATEGORY''),
					CASE 
						WHEN INSERTED.Deleted_Item = 1 THEN ''DELETE''
						ELSE ''CHANGE''
					END,
					CASE 
						WHEN dbo.fn_GetDiscontinueStatus(INSERTED.Item_Key, NULL, NULL) = 1 THEN ''DISCONTINUED''
						ELSE ''ACTIVE''
					END
				FROM   
					INSERTED
					INNER JOIN DELETED
						ON  DELETED.Item_Key = INSERTED.Item_Key
					INNER JOIN ItemIdentifier II
						ON  II.Item_Key = INSERTED.Item_Key
						AND Default_Identifier = 1
					LEFT JOIN ItemCategory
						ON  INSERTED.Category_ID = ItemCategory.Category_ID
				WHERE  
					INSERTED.Retail_Sale = 1
					AND (
							(INSERTED.Deleted_Item <> DELETED.Deleted_Item)
							OR (DELETED.Retail_Sale <> INSERTED.Retail_Sale)
							OR (DELETED.Item_Description <> INSERTED.Item_Description)
							OR (DELETED.Deleted_Item <> INSERTED.Deleted_Item)
							OR (
									ISNULL(DELETED.ProdHierarchyLevel4_ID, 0) <> 
									ISNULL(INSERTED.ProdHierarchyLevel4_ID, 0)
								)
							OR (
									ISNULL(DELETED.Category_ID, 0) <> ISNULL(INSERTED.Category_ID, 0)
								)
						)
					AND (
							INSERTED.SubTeam_No IN (SELECT SubTeam_No
													FROM   PMSubTeamInclude(NOLOCK))
							OR DELETED.SubTeam_No IN (SELECT SubTeam_No
														FROM   PMSubTeamInclude(NOLOCK))
						)
					AND NOT EXISTS (
							SELECT *
							FROM   PMExcludedItem
							WHERE  Item_Key = INSERTED.Item_Key
						)
	    
				SELECT @error_no = @@ERROR
			END
	
		IF @error_no = 0
			BEGIN -- Insert ItemChangeHistory
				INSERT INTO ItemChangeHistory
				(
					Item_Key,
					Item_Description,
					Sign_Description,
					Ingredients,
					SubTeam_No,
					Sales_Account,
					Package_Desc1,
					Package_Desc2,
					Package_Unit_ID,
					Min_Temperature,
					Max_Temperature,
					Units_Per_Pallet,
					Average_Unit_Weight,
					Tie,
					HIGH,
					Yield,
					Brand_ID,
					Category_ID,
					Origin_ID,
					ShelfLife_Length,
					ShelfLife_ID,
					Retail_Unit_ID,
					Vendor_Unit_ID,
					Distribution_Unit_ID,
					WFM_Item,
					Not_Available,
					Pre_Order,
					NoDistMarkup,
					Organic,
					Refrigerated,
					Keep_Frozen,
					Shipper_Item,
					Full_Pallet_Only,
					POS_Description,
					Retail_Sale,
					Food_Stamps,
					Price_Required,
					Quantity_Required,
					ItemType_ID,
					HFM_Item,
					ScaleDesc1,
					ScaleDesc2,
					Not_AvailableNote,
					CountryProc_ID,
					Manufacturing_Unit_ID,
					EXEDistributed,
					DistSubTeam_No,
					CostedByWeight,
					TaxClassID,
					USER_ID,
					User_ID_Date,
					LabelType_ID,
					QtyProhibit,
					GroupList,
					Case_Discount,
					Coupon_Multiplier,
					Misc_Transaction_Sale,
					Misc_Transaction_Refund,
					Recall_Flag,
					Manager_ID,
					Ice_Tare,
					PurchaseThresholdCouponAmount,
					PurchaseThresholdCouponSubTeam,
					Product_Code,
					Unit_Price_Category,
					StoreJurisdictionID,
					CatchweightRequired,
					Cost_Unit_ID,
					Freight_Unit_ID,
					Discountable,
					ClassID,
					SustainabilityRankingRequired,
					SustainabilityRankingID,
					GiftCard
				)
				SELECT 
					INSERTED.Item_Key,
					INSERTED.Item_Description,
					INSERTED.Sign_Description,
					INSERTED.Ingredients,
					INSERTED.SubTeam_No,
					INSERTED.Sales_Account,
					INSERTED.Package_Desc1,
					INSERTED.Package_Desc2,
					INSERTED.Package_Unit_ID,
					INSERTED.Min_Temperature,
					INSERTED.Max_Temperature,
					INSERTED.Units_Per_Pallet,
					INSERTED.Average_Unit_Weight,
					INSERTED.Tie,
					INSERTED.High,
					INSERTED.Yield,
					INSERTED.Brand_ID,
					INSERTED.Category_ID,
					INSERTED.Origin_ID,
					INSERTED.ShelfLife_Length,
					INSERTED.ShelfLife_ID,
					INSERTED.Retail_Unit_ID,
					INSERTED.Vendor_Unit_ID,
					INSERTED.Distribution_Unit_ID,
					INSERTED.WFM_Item,
					INSERTED.Not_Available,
					INSERTED.Pre_Order,
					INSERTED.NoDistMarkup,
					INSERTED.Organic,
					INSERTED.Refrigerated,
					INSERTED.Keep_Frozen,
					INSERTED.Shipper_Item,
					INSERTED.Full_Pallet_Only,
					INSERTED.POS_Description,
					INSERTED.Retail_Sale,
					INSERTED.Food_Stamps,
					INSERTED.Price_Required,
					INSERTED.Quantity_Required,
					INSERTED.ItemType_ID,
					INSERTED.HFM_Item,
					INSERTED.ScaleDesc1,
					INSERTED.ScaleDesc2,
					INSERTED.Not_AvailableNote,
					INSERTED.CountryProc_ID,
					INSERTED.Manufacturing_Unit_ID,
					INSERTED.EXEDistributed,
					INSERTED.DistSubTeam_No,
					INSERTED.CostedByWeight,
					INSERTED.TaxClassID,
					INSERTED.User_ID,
					INSERTED.User_ID_Date,
					INSERTED.LabelType_ID,
					INSERTED.QtyProhibit,
					INSERTED.GroupList,
					INSERTED.Case_Discount,
					INSERTED.Coupon_Multiplier,
					INSERTED.Misc_Transaction_Sale,
					INSERTED.Misc_Transaction_Refund,
					INSERTED.Recall_Flag,
					INSERTED.Manager_ID,
					INSERTED.Ice_Tare,
					INSERTED.PurchaseThresholdCouponAmount,
					INSERTED.PurchaseThresholdCouponSubTeam,
					INSERTED.Product_Code,
					INSERTED.Unit_Price_Category,
					INSERTED.StoreJurisdictionID,
					INSERTED.CatchweightRequired,
					INSERTED.Cost_Unit_ID,
   					INSERTED.Freight_Unit_ID,
					INSERTED.Discountable,
					INSERTED.ClassID,
					INSERTED.SustainabilityRankingRequired,
					INSERTED.SustainabilityRankingID,
					INSERTED.GiftCard
				FROM   
					INSERTED
					INNER JOIN DELETED ON DELETED.Item_Key = INSERTED.Item_Key  
				WHERE  
					INSERTED.Item_Description <> DELETED.Item_Description
					OR  INSERTED.Sign_Description <> DELETED.Sign_Description
					OR  ISNULL(INSERTED.Ingredients, '''') <> ISNULL(DELETED.Ingredients, '''')
					OR  INSERTED.SubTeam_No <> DELETED.SubTeam_No
					OR  ISNULL(INSERTED.Sales_Account, '''') <> ISNULL(DELETED.Sales_Account, '''')
					OR  INSERTED.Package_Desc1 <> DELETED.Package_Desc1
					OR  INSERTED.Package_Desc2 <> DELETED.Package_Desc2
					OR  ISNULL(INSERTED.Package_Unit_ID, 0) <> ISNULL(DELETED.Package_Unit_ID, 0)
					OR  INSERTED.Min_Temperature <> DELETED.Min_Temperature
					OR  INSERTED.Max_Temperature <> DELETED.Max_Temperature
					OR  INSERTED.Units_Per_Pallet <> DELETED.Units_Per_Pallet
					OR  ISNULL(INSERTED.Average_Unit_Weight, 0) <> ISNULL(DELETED.Average_Unit_Weight, 0)
					OR  INSERTED.Tie <> DELETED.Tie
					OR  INSERTED.High <> DELETED.High
					OR  INSERTED.Yield <> DELETED.Yield
					OR  ISNULL(INSERTED.Brand_ID, 0) <> ISNULL(DELETED.Brand_ID, 0)
					OR  ISNULL(INSERTED.Category_ID, 0) <> ISNULL(DELETED.Category_ID, 0)
					OR  ISNULL(INSERTED.ProdHierarchyLevel4_ID, 0) <> ISNULL(DELETED.ProdHierarchyLevel4_ID, 0)
					OR  ISNULL(INSERTED.Origin_ID, 0) <> ISNULL(DELETED.Origin_ID, 0)
					OR  INSERTED.ShelfLife_Length <> DELETED.ShelfLife_Length
					OR  ISNULL(INSERTED.ShelfLife_ID, 0) <> ISNULL(DELETED.ShelfLife_ID, 0)
					OR  ISNULL(INSERTED.Retail_Unit_ID, 0) <> ISNULL(DELETED.Retail_Unit_ID, 0)
					OR  ISNULL(INSERTED.Vendor_Unit_ID, 0) <> ISNULL(DELETED.Vendor_Unit_ID, 0)
					OR  ISNULL(INSERTED.Distribution_Unit_ID, 0) <> ISNULL(DELETED.Distribution_Unit_ID, 0)
					OR  INSERTED.WFM_Item <> DELETED.WFM_Item
					OR  INSERTED.Not_Available <> DELETED.Not_Available
					OR  INSERTED.Pre_Order <> DELETED.Pre_Order
					OR  INSERTED.NoDistMarkup <> DELETED.NoDistMarkup
					OR  INSERTED.Organic <> DELETED.Organic
					OR  INSERTED.Refrigerated <> DELETED.Refrigerated
					OR  INSERTED.Keep_Frozen <> DELETED.Keep_Frozen
					OR  INSERTED.Shipper_Item <> DELETED.Shipper_Item
					OR  INSERTED.Full_Pallet_Only <> DELETED.Full_Pallet_Only
					OR  INSERTED.POS_Description <> DELETED.POS_Description
					OR  INSERTED.Retail_Sale <> DELETED.Retail_Sale
					OR  INSERTED.Food_Stamps <> DELETED.Food_Stamps
					OR  INSERTED.Price_Required <> DELETED.Price_Required
					OR  INSERTED.Quantity_Required <> DELETED.Quantity_Required
					OR  INSERTED.ItemType_ID <> DELETED.ItemType_ID
					OR  INSERTED.HFM_Item <> DELETED.HFM_Item
					OR  ISNULL(INSERTED.ScaleDesc1, '''') <> ISNULL(DELETED.ScaleDesc1, '''')
					OR  ISNULL(INSERTED.ScaleDesc2, '''') <> ISNULL(DELETED.ScaleDesc2, '''')
					OR  ISNULL(INSERTED.Not_AvailableNote, '''') <> ISNULL(DELETED.Not_AvailableNote, '''')
					OR  ISNULL(INSERTED.CountryProc_ID, 0) <> ISNULL(DELETED.CountryProc_ID, 0)
					OR  ISNULL(INSERTED.Manufacturing_Unit_ID, 0) <> ISNULL(DELETED.Manufacturing_Unit_ID, 0)
					OR  INSERTED.EXEDistributed <> DELETED.EXEDistributed
					OR  INSERTED.DistSubTeam_No <> DELETED.DistSubTeam_No
					OR  INSERTED.CostedByWeight <> DELETED.CostedByWeight
					OR  INSERTED.TaxClassID <> DELETED.TaxClassID
					OR  INSERTED.LabelType_ID <> DELETED.LabelType_ID
					OR  ISNULL(INSERTED.QtyProhibit, 0) <> ISNULL(DELETED.QtyProhibit, 0)
					OR  ISNULL(INSERTED.GroupList, 0) <> ISNULL(DELETED.GroupList, 0)
					OR  ISNULL(INSERTED.Case_Discount, 0) <> ISNULL(DELETED.Case_Discount, 0)
					OR  ISNULL(INSERTED.Coupon_Multiplier, 0) <> ISNULL(DELETED.Coupon_Multiplier, 0)
					OR  ISNULL(INSERTED.Misc_Transaction_Sale, 0) <> ISNULL(DELETED.Misc_Transaction_Sale, 0)
					OR  ISNULL(INSERTED.Misc_Transaction_Refund, 0) <> ISNULL(DELETED.Misc_Transaction_Refund, 0)
					OR  ISNULL(INSERTED.Recall_Flag, 0) <> ISNULL(DELETED.Recall_Flag, 0)
					OR  ISNULL(INSERTED.Manager_ID, 0) <> ISNULL(DELETED.Manager_ID, 0)
					OR  ISNULL(INSERTED.Ice_Tare, 0) <> ISNULL(DELETED.Ice_Tare, 0)
					OR  ISNULL(INSERTED.PurchaseThresholdCouponAmount, 0) <> ISNULL(DELETED.PurchaseThresholdCouponAmount, 0)
					OR  ISNULL(INSERTED.PurchaseThresholdCouponSubTeam, 0) <> ISNULL(DELETED.PurchaseThresholdCouponSubTeam, 0)
					OR  ISNULL(INSERTED.Product_Code, 0) <> ISNULL(DELETED.Product_Code, 0)
					OR  ISNULL(INSERTED.Unit_Price_Category, 0) <> ISNULL(DELETED.Unit_Price_Category, 0)
					OR  ISNULL(INSERTED.StoreJurisdictionID, 0) <> ISNULL(DELETED.StoreJurisdictionID, 0)
					OR  INSERTED.CatchweightRequired <> DELETED.CatchweightRequired
					OR  INSERTED.Cost_Unit_ID <> DELETED.Cost_Unit_ID
   					OR	INSERTED.Freight_Unit_ID <> DELETED.Freight_Unit_ID
					OR	INSERTED.Discountable <> DELETED.Discountable
					OR	INSERTED.ClassID <> DELETED.ClassID
					OR	INSERTED.SustainabilityRankingRequired <> DELETED.SustainabilityRankingRequired
					OR	INSERTED.SustainabilityRankingID <> DELETED.SustainabilityRankingID
					OR	INSERTED.GiftCard <> DELETED.GiftCard
	    
				SELECT @error_no = @@ERROR
			END
	
		DECLARE @BatchOrganicChanges BIT = (SELECT dbo.fn_InstanceDataValue(''BatchOrganicChanges'', NULL)); -- Organic changes are controlled by ''BatchOrganicChanges'' IDF
		IF @error_no = 0
			BEGIN -- send down PriceBatchDetail records to allow item changes to be batched
				INSERT INTO PriceBatchDetail
				(
					Store_No,
					Item_Key,
					ItemChgTypeID,
					InsertApplication
				)
				SELECT 
					Store.Store_No,
					INSERTED.Item_Key,
					2,
					''ItemUpdate Trigger''
				FROM   
					INSERTED
					INNER JOIN DELETED
						ON  DELETED.Item_Key = INSERTED.Item_Key
					CROSS JOIN (
							SELECT Store_No
							FROM   Store(NOLOCK)
							WHERE  WFM_Store = 1
									OR  Mega_Store = 1
						) Store
				WHERE  
					(INSERTED.Remove_Item = 0 AND INSERTED.Deleted_Item = 0)
					AND (
							-- Don''t allow maintenance to be created if Icon is doing the update, unless...                       
							(ISNULL(inserted.LastModifiedUser_ID, 0) <> ISNULL(@IconControllerUserId, 0)) 
							  -- ... icon is doing the update and it''s a subteam update (then do create maintenance)... 
							OR (ISNULL(inserted.LastModifiedUser_ID, 0) = ISNULL(@IconControllerUserId, 0) AND inserted.SubTeam_No <> deleted.SubTeam_No)
							  -- ... or if icon is doing the update and it''s a package unit update
							  -- ...but only if it''s a package unit update for a store not under GPM
							OR ( ISNULL(inserted.LastModifiedUser_ID, 0) = ISNULL(@IconControllerUserId, 0) AND 
								(inserted.Package_Desc1 <> deleted.Package_Desc1 
								OR inserted.Package_Desc2 <> deleted.Package_Desc2 
								OR inserted.Package_Unit_ID <> deleted.Package_Unit_ID) AND
								ISNULL(dbo.fn_InstanceDataValue(''GlobalPriceManagement'', Store.Store_No), 0)= 0 )
						)
					AND (
							INSERTED.Item_Description <> DELETED.Item_Description
							OR INSERTED.POS_Description <> DELETED.POS_Description
							OR INSERTED.Sign_Description <> DELETED.Sign_Description
							OR INSERTED.Food_Stamps <> DELETED.Food_Stamps
							OR INSERTED.Price_Required <> DELETED.Price_Required
							OR INSERTED.Quantity_Required <> DELETED.Quantity_Required
							OR (INSERTED.Organic <> DELETED.Organic
								AND @BatchOrganicChanges = 1)
							OR INSERTED.Retail_Sale <> DELETED.Retail_Sale
							OR INSERTED.ItemType_ID <> DELETED.ItemType_ID
							OR ISNULL(INSERTED.Retail_Unit_ID, 0) <> ISNULL(DELETED.Retail_Unit_ID, 0)
							OR INSERTED.SubTeam_No <> DELETED.SubTeam_No
							OR ISNULL(INSERTED.Origin_ID, 0) <> ISNULL(DELETED.Origin_ID, 0)
							OR ISNULL(INSERTED.Brand_ID, 0) <> ISNULL(DELETED.Brand_ID, 0)
							OR INSERTED.Package_Desc1 <> DELETED.Package_Desc1
							OR INSERTED.Package_Desc2 <> DELETED.Package_Desc2
							OR INSERTED.TaxClassID <> DELETED.TaxClassID
							OR ISNULL(INSERTED.QtyProhibit, 0) <> ISNULL(DELETED.QtyProhibit, 0)
							OR ISNULL(INSERTED.GroupList, 0) <> ISNULL(DELETED.GroupList, 0)
							OR ISNULL(INSERTED.Package_Unit_ID, 0) <> ISNULL(DELETED.Package_Unit_ID, 0)
							OR ISNULL(INSERTED.Case_Discount, 0) <> ISNULL(DELETED.Case_Discount, 0)
							OR ISNULL(INSERTED.Coupon_Multiplier, 0) <> ISNULL(DELETED.Coupon_Multiplier, 0)
							OR ISNULL(INSERTED.Misc_Transaction_Sale, 0) <> ISNULL(DELETED.Misc_Transaction_Sale, 0)
							OR ISNULL(INSERTED.Misc_Transaction_Refund, 0) <> ISNULL(DELETED.Misc_Transaction_Refund, 0)
							OR ISNULL(INSERTED.Recall_Flag, 0) <> ISNULL(DELETED.Recall_Flag, 0)
							OR ISNULL(INSERTED.Ice_Tare, 0) <> ISNULL(DELETED.Ice_Tare, 0)
							OR ISNULL(INSERTED.PurchaseThresholdCouponAmount, 0) <> 
								ISNULL(DELETED.PurchaseThresholdCouponAmount, 0)
							OR ISNULL(INSERTED.PurchaseThresholdCouponSubTeam, 0) <> 
								ISNULL(DELETED.PurchaseThresholdCouponSubTeam, 0)
							OR ISNULL(INSERTED.Unit_Price_Category, 0) <> ISNULL(DELETED.Unit_Price_Category, 0)
							OR ISNULL(INSERTED.FSA_Eligible, 0) <> ISNULL(DELETED.FSA_Eligible, 0)
						)
					AND (
							dbo.fn_HasPendingItemChangePriceBatchDetailRecord(INSERTED.Item_Key, Store.Store_No) = 0
						)
	    
				SELECT @error_no = @@ERROR
			END
		IF @error_no = 0
			BEGIN -- If Retail_Sale flag 1->0 then delete all identifiers (default and alternate) from VSC
				DECLARE @ItemsChangedFromRetailSale TABLE (Item_Key int, RetailSaleChanged bit);
			
				INSERT INTO 
					@ItemsChangedFromRetailSale
				SELECT
					inserted.Item_Key,
					CASE
						WHEN INSERTED.Retail_Sale = 0 AND DELETED.Retail_Sale = 1 THEN 1
						ELSE 0
					END
				FROM 
					INSERTED 
					INNER JOIN DELETED ON DELETED.Item_Key = INSERTED.Item_Key
				IF EXISTS (SELECT * FROM @ItemsChangedFromRetailSale icr WHERE icr.RetailSaleChanged = 1)
					BEGIN
						DECLARE @EnableUPCIRMAToIConFlow_RS bit
						SELECT  @EnableUPCIRMAToIConFlow_RS = acv.Value
								FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
								ON acv.EnvironmentID = ace.EnvironmentID 
								INNER JOIN AppConfigApp aca
								ON acv.ApplicationID = aca.ApplicationID 
								INNER JOIN AppConfigKey ack
								ON acv.KeyID = ack.KeyID 
								WHERE aca.Name = ''IRMA Client'' AND
								ack.Name = ''EnableUPCIRMAToIConFlow'' and
								SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = ''IRMA CLIENT''),1,1)
	
						DECLARE @EnablePLUIRMAIConFlow_RS bit
						SELECT @EnablePLUIRMAIConFlow_RS = acv.Value
								FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
								ON acv.EnvironmentID = ace.EnvironmentID 
								INNER JOIN AppConfigApp aca
								ON acv.ApplicationID = aca.ApplicationID 
								INNER JOIN AppConfigKey ack
								ON acv.KeyID = ack.KeyID 
								WHERE aca.Name = ''IRMA Client'' AND
								ack.Name = ''EnablePLUIRMAIConFlow'' and
								SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = ''IRMA CLIENT''),1,1)
			
						DELETE FROM @Identifiers -- Clear any existing identifiers
						INSERT INTO @Identifiers
						SELECT
									   ii.Item_Key as Item_Key,
									   ii.Identifier as Identifier,
											  CASE
													 WHEN (len(ii.Identifier) <= 6) OR (len(ii.Identifier) = 11 and ii.Identifier like ''2%00000'') then ''PLU''
													 ELSE ''UPC''
											  END    
								 FROM ItemIdentifier ii JOIN inserted ON ii.Item_Key = inserted.Item_Key
								 -- WHERE ii.Default_Identifier = 1
					
						DELETE FROM ValidatedScanCode WHERE ScanCode IN (SELECT Identifier FROM @Identifiers)
					END
				SELECT @error_no = @@ERROR
			END
		IF @error_no = 0
			BEGIN -- If Retail_Sale flag 0->1 then insert as new Item(s) into IconItemChangeQueue so icon can manage the Item''s information. 
				DECLARE @ItemsChangedToRetailSale TABLE (Item_Key int, RetailSaleChanged bit);
			
				INSERT INTO 
					@ItemsChangedToRetailSale
				SELECT
					inserted.Item_Key,
					CASE
						WHEN INSERTED.Retail_Sale = 1 AND DELETED.Retail_Sale = 0 THEN 1
						ELSE 0
					END
				FROM 
					INSERTED 
					INNER JOIN DELETED ON DELETED.Item_Key = INSERTED.Item_Key
				IF EXISTS (SELECT * FROM @ItemsChangedToRetailSale icr WHERE icr.RetailSaleChanged = 1)
					BEGIN
						DECLARE @EnableUPCIRMAToIConFlow bit
						SELECT  @EnableUPCIRMAToIConFlow = acv.Value
								FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
								ON acv.EnvironmentID = ace.EnvironmentID 
								INNER JOIN AppConfigApp aca
								ON acv.ApplicationID = aca.ApplicationID 
								INNER JOIN AppConfigKey ack
								ON acv.KeyID = ack.KeyID 
								WHERE aca.Name = ''IRMA Client'' AND
								ack.Name = ''EnableUPCIRMAToIConFlow'' and
								SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = ''IRMA CLIENT''),1,1)
	
						DECLARE @EnablePLUIRMAIConFlow bit
						SELECT @EnablePLUIRMAIConFlow = acv.Value
								FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
								ON acv.EnvironmentID = ace.EnvironmentID 
								INNER JOIN AppConfigApp aca
								ON acv.ApplicationID = aca.ApplicationID 
								INNER JOIN AppConfigKey ack
								ON acv.KeyID = ack.KeyID 
								WHERE aca.Name = ''IRMA Client'' AND
								ack.Name = ''EnablePLUIRMAIConFlow'' and
								SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = ''IRMA CLIENT''),1,1)
			
						DELETE FROM @Identifiers -- Clear any existing identifers
						INSERT INTO @Identifiers
						SELECT
									   ii.Item_Key as Item_Key,
									   ii.Identifier as Identifier,
											  CASE
													 WHEN (len(ii.Identifier) <= 6) OR (len(ii.Identifier) = 11 and ii.Identifier like ''2%00000'') then ''PLU''
													 ELSE ''UPC''
											  END    
								 FROM ItemIdentifier ii JOIN inserted ON ii.Item_Key = inserted.Item_Key
								 
						DECLARE @newItemChgTypeID tinyint
						SELECT @newItemChgTypeID = itemchgtypeid FROM itemchgtype WHERE itemchgtypedesc like ''new''
						INSERT INTO IconItemChangeQueue 
						(
							Item_Key,
							Identifier,
							ItemChgTypeID
						)
						SELECT
							inserted.Item_Key    as Item_Key,
							i.Identifier         as Identifier,
							@newItemChgTypeID    as ItemChgTypeID
						FROM
							inserted
							JOIN @Identifiers i on inserted.Item_Key = i.Item_Key
						WHERE
							(@EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIConFlow = 1)
							OR (@EnableUPCIRMAToIConFlow = 1 AND i.IdentifierType = ''UPC'')
							OR (@EnablePLUIRMAIConFlow = 1 AND i.IdentifierType = ''PLU'')
					END
				SELECT @error_no = @@ERROR
			END
	
		IF @error_no = 0
			BEGIN -- insert to PLUMCorpChgQueue if needed
				INSERT INTO PLUMCorpChgQueue
				(
					Item_Key,
					ActionCode,
					Store_No
				)
				SELECT 
					INSERTED.Item_Key,
					''C'',
					s.Store_No
				FROM   
					INSERTED
					INNER JOIN DELETED ON  DELETED.Item_Key = INSERTED.Item_Key
					CROSS JOIN Store s
					JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
				WHERE  
					INSERTED.Remove_Item = 0
					AND INSERTED.Deleted_Item = 0
					AND (
							-- Don''t allow maintenance to be created if Icon is doing the update, unless it''s a subteam update.
							(ISNULL(inserted.LastModifiedUser_ID, 0) <> ISNULL(@IconControllerUserId, 0)) 
							OR (ISNULL(inserted.LastModifiedUser_ID, 0) = ISNULL(@IconControllerUserId, 0) AND inserted.SubTeam_No <> deleted.SubTeam_No)
						)
					AND (
							ISNULL(INSERTED.Ingredients, '''') <> ISNULL(DELETED.Ingredients, '''')
							OR ISNULL(INSERTED.ScaleDesc1, '''') <> ISNULL(DELETED.ScaleDesc1, '''')
							OR ISNULL(INSERTED.ScaleDesc2, '''') <> ISNULL(DELETED.ScaleDesc2, '''')
							OR ISNULL(INSERTED.ScaleDesc3, '''') <> ISNULL(DELETED.ScaleDesc3, '''')
							OR ISNULL(INSERTED.ScaleDesc4, '''') <> ISNULL(DELETED.ScaleDesc4, '''')
							OR ISNULL(INSERTED.Retail_Unit_ID, 0) <> ISNULL(DELETED.Retail_Unit_ID, 0)
							OR INSERTED.SubTeam_No <> DELETED.SubTeam_No
							OR INSERTED.Package_Desc1 <> DELETED.Package_Desc1
							OR INSERTED.Package_Desc2 <> DELETED.Package_Desc2
							OR ISNULL(INSERTED.Package_Unit_ID, 0) <> ISNULL(DELETED.Package_Unit_ID, 0)
							OR ISNULL(INSERTED.ShelfLife_Length, 0) <> ISNULL(DELETED.ShelfLife_Length, 0)
							OR ISNULL(INSERTED.ScaleTare, 0) <> ISNULL(DELETED.ScaleTare, 0)
							OR ISNULL(INSERTED.ScaleUseBy, 0) <> ISNULL(DELETED.ScaleUseBy, 0)
							OR ISNULL(INSERTED.ScaleForcedTare, 0) <> ISNULL(DELETED.ScaleForcedTare, 0)
						)
					AND EXISTS (
							SELECT *
							FROM   ItemIdentifier II
							WHERE  II.Item_Key = INSERTED.Item_Key
									AND dbo.fn_IsScaleItem(II.Identifier) = 1
									AND II.Scale_Identifier = 1
						) --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
	               
					AND s.WFM_Store = 1 AND si.Authorized = 1 AND
					NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = ''C'') AND
					NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = ''C'')
	    
				SELECT @error_no = @@ERROR
			END
	
		IF @error_no = 0
			BEGIN -- Insert non-batchable changes when GloCon makes changes to Items
				DECLARE @EnableIconItemNonBatchableChanges bit = 0
				SELECT @EnableIconItemNonBatchableChanges = FlagValue
				FROM dbo.InstanceDataFlags
				WHERE FlagKey = ''EnableIconItemNonBatchableChanges''
				IF(@EnableIconItemNonBatchableChanges = 1)
				BEGIN
					DECLARE @priceBatchStatusIdProcessed int = (select PriceBatchStatusID from dbo.PriceBatchStatus where PriceBatchStatusDesc = ''Processed'')
					;WITH changedItems AS
					(
						SELECT
							ins.Item_Key,
							ins.POS_Description,
							ins.Food_Stamps,
							ins.TaxClassID
						FROM INSERTED ins
						JOIN DELETED d on ins.Item_Key = d.Item_Key
						WHERE ins.LastModifiedUser_ID = @IconControllerUserId
							AND ins.Retail_Sale = 1
							AND ins.Remove_Item = 0 
							AND ins.Deleted_Item = 0
							AND ((ins.POS_Description <> d.POS_Description)
								OR (ins.Food_Stamps <> d.Food_Stamps)
								OR (ins.TaxClassID <> d.TaxClassID))
					)
					MERGE dbo.ItemNonBatchableChanges AS inbc
					USING 
						(SELECT * FROM changedItems) AS i
					ON i.Item_Key = inbc.Item_Key
					WHEN MATCHED THEN
						UPDATE SET inbc.POS_Description = i.POS_Description,
									inbc.Food_Stamps = i.Food_Stamps,
									inbc.TaxClassID = i.TaxClassID
					WHEN NOT MATCHED THEN
						INSERT (Item_Key, POS_Description, Food_Stamps, TaxClassID)
						VALUES (i.Item_Key, i.POS_Description, i.Food_Stamps, i.TaxClassID);
				
					;WITH changedItems AS
					(
						SELECT
							ins.Item_Key,
							ins.POS_Description,
							ins.Food_Stamps,
							ins.TaxClassID
						FROM INSERTED ins
						JOIN DELETED d on ins.Item_Key = d.Item_Key
						WHERE ins.LastModifiedUser_ID = @IconControllerUserId
							AND ins.Retail_Sale = 1
							AND ins.Remove_Item = 0 
							AND ins.Deleted_Item = 0
							AND ((ins.POS_Description <> d.POS_Description)
								OR (ins.Food_Stamps <> d.Food_Stamps)
								OR (ins.TaxClassID <> d.TaxClassID))
					)
					UPDATE pbd
					SET POS_Description = i.POS_Description,
						Food_Stamps = i.Food_Stamps
					FROM changedItems i
					JOIN dbo.PriceBatchDetail pbd on pbd.Item_Key = i.Item_Key
					JOIN dbo.PriceBatchHeader pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
					WHERE pbh.PriceBatchStatusID <> @priceBatchStatusIdProcessed
				END							
			END
		IF @error_no = 0
			BEGIN -- update Item.LastModifiedUser_ID
				UPDATE ITEM
				   SET LastModifiedUser_ID = NULL
				  FROM INSERTED
				 WHERE INSERTED.Item_Key = ITEM.Item_Key
				   AND INSERTED.LastModifiedUser_ID = @IconControllerUserId
				SELECT @error_no = @@ERROR
			END
		IF @error_no <> 0
			BEGIN -- rollback transaction because of error
				ROLLBACK TRAN
				DECLARE @Severity SMALLINT
				SELECT @Severity = ISNULL(
						   (
							   SELECT severity
							   FROM   MASTER.dbo.sysmessages
							   WHERE  ERROR = @error_no
						   ),
						   16
					   )
	    
				RAISERROR 
				(
					''ItemUpdate Trigger failed with @@ERROR: %d'',
					@Severity,
					1,
					@error_no
				)
			END
	END
END
' 
GO
/******************************************************************************
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [BizTalk] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [ExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IConInterface] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Item] TO [IConInterface] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [IConInterface] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [iCONReportingRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [iCONReportingRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IMHARole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [IRMA_Teradata] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [IRMAAdminRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRMAAVCIRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRMAClientRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Item] TO [IRMAClientRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[Item] TO [IRMAExcelRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[Item] TO [IRMAExcelRole] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[Item] TO [IRMAExcelRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRMAExcelRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Item] TO [IRMAExcelRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRMAPDXExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRMAPromoRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [IRMAReports] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRMAReportsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[Item] TO [IRMASLIMRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[Item] TO [IRMASLIMRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRMASLIMRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Item] TO [IRMASLIMRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRMASupportRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [IRMASupportRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [NutriChefDataWriter] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [sobluesky] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [SODataControl] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Item] TO [spice_user] AS [dbo]
GO
GRANT DELETE ON [dbo].[Item] TO [SQLExcel] AS [dbo]
GO
GRANT INSERT ON [dbo].[Item] TO [SQLExcel] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[Item] TO [SQLExcel] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [SQLExcel] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Item] TO [SQLExcel] AS [dbo]
GO
GRANT SELECT ON [dbo].[Item] TO [TibcoDataWriter] AS [dbo]
GO
/******************************************************************************
		16. Check FL Checks (generated from VS schema compare)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__SubTeam_No__0BAD2365]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Package_Un__0E899010]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Brand_ID__15368D9F]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Manager__0E899010]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Origin_ID__1812FA4A]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__ShelfLife___1AEF66F5]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Distributi__1EBFF7D9]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Cost_Unit___1FB41C12]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Freight_Un__20A8404B]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK_Item_ItemOrigin_CountryProc_ID]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__Category_I__162AB1D8]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK_Item_ProdHierarchyLevel4]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK_Item_StoreJurisdictionID]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK__Item__User_ID__378BA5A3]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK_Item_TaxClass1]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK_Item_LabelType_ID]
GO
ALTER TABLE [dbo].[Item] WITH CHECK CHECK CONSTRAINT [FK_Item_SubTeam]
GO
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables (takes about 5 minutes in TEST): --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM [dbo].[Item_Unaligned](NOLOCK)

SELECT @new = count(*)
FROM [dbo].[Item](NOLOCK)

PRINT N'[dbo].[Item]_Unaligned Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'[dbo].[Item] Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count. --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count. --- [dbo].[Item] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
