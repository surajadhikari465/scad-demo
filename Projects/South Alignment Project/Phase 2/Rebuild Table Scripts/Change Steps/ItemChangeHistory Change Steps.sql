/******************************************************************************
		SO ItemChangeHistory
		Change Steps
******************************************************************************/
PRINT N'Status: Begin ItemChangeHistory Rebuild (Takes 5 minutes in TEST) --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 5, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking. (This Step is N/A)'
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_Host_Name]
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_Effective_Date]
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_EXEDistributed]
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_Insert_Date]
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_CatchweightRequired]
GO
/******************************************************************************
		3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Extended Properties. (This Step is N/A)'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers. (This Step is N/A)'
GO
/******************************************************************************
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemChangeHistory_TaxClass]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]'))
ALTER TABLE [dbo].[ItemChangeHistory] DROP CONSTRAINT [FK_ItemChangeHistory_TaxClass]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Manager__0E898877]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]'))
ALTER TABLE [dbo].[ItemChangeHistory] DROP CONSTRAINT [FK__Item__Manager__0E898877]
GO
/******************************************************************************
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]') AND name = N'ICHItemKeyInsertDateSubteamUserID')
DROP INDEX [ICHItemKeyInsertDateSubteamUserID] ON [dbo].[ItemChangeHistory]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]') AND name = N'idxItemChangeHistory_EffectiveDate')
DROP INDEX [idxItemChangeHistory_EffectiveDate] ON [dbo].[ItemChangeHistory] WITH ( ONLINE = OFF )
GO
/******************************************************************************
		7. Rename SO PK 
******************************************************************************/
PRINT N'Status: 7. Rename SO PK  --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No PK on ItemChangeHistory (This Step is N/A)'
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[ItemChangeHistory]', N'ItemChangeHistory_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemChangeHistory](
	[Item_Key] [int] NULL,
	[Item_Description] [varchar](60) NULL,
	[Sign_Description] [varchar](60) NULL,
	[Ingredients] [varchar](3500) NULL,
	[SubTeam_No] [int] NULL,
	[Sales_Account] [varchar](6) NULL,
	[Package_Desc1] [decimal](9, 4) NULL,
	[Package_Desc2] [decimal](9, 4) NULL,
	[Package_Unit_ID] [int] NULL,
	[Min_Temperature] [smallint] NULL,
	[Max_Temperature] [smallint] NULL,
	[Units_Per_Pallet] [smallint] NULL,
	[Average_Unit_Weight] [decimal](9, 4) NULL,
	[Tie] [tinyint] NULL,
	[High] [tinyint] NULL,
	[Yield] [decimal](9, 4) NULL,
	[Brand_ID] [int] NULL,
	[Category_ID] [int] NULL,
	[Origin_ID] [int] NULL,
	[ShelfLife_Length] [smallint] NULL,
	[ShelfLife_ID] [int] NULL,
	[Retail_Unit_ID] [int] NULL,
	[Vendor_Unit_ID] [int] NULL,
	[Distribution_Unit_ID] [int] NULL,
	[Cost_Unit_ID] [int] NULL,
	[Freight_Unit_ID] [int] NULL,
	[Discontinue_Item] [bit] NULL,
	[WFM_Item] [bit] NULL,
	[Not_Available] [bit] NULL,
	[Pre_Order] [bit] NULL,
	[Remove_Item] [tinyint] NULL,
	[NoDistMarkup] [bit] NULL,
	[Organic] [bit] NULL,
	[Refrigerated] [bit] NULL,
	[Keep_Frozen] [bit] NULL,
	[Shipper_Item] [bit] NULL,
	[Full_Pallet_Only] [bit] NULL,
	[POS_Description] [varchar](26) NULL,
	[Retail_Sale] [bit] NULL,
	[Food_Stamps] [bit] NULL,
	[Discountable] [bit] NULL,
	[Price_Required] [bit] NULL,
	[Quantity_Required] [bit] NULL,
	[ItemType_ID] [int] NULL,
	[HFM_Item] [bit] NULL,
	[ScaleDesc1] [varchar](64) NULL,
	[ScaleDesc2] [varchar](64) NULL,
	[Not_AvailableNote] [varchar](255) NULL,
	[CountryProc_ID] [int] NULL,
	[Host_Name] [varchar](20) NULL CONSTRAINT [DF_ItemChangeHistory_Host_Name]  DEFAULT (host_name()),
	[Effective_Date] [datetime] NOT NULL CONSTRAINT [DF_ItemChangeHistory_Effective_Date]  DEFAULT (getdate()),
	[Manufacturing_Unit_ID] [int] NULL,
	[EXEDistributed] [bit] NOT NULL CONSTRAINT [DF_ItemChangeHistory_EXEDistributed]  DEFAULT ((0)),
	[ClassID] [int] NULL,
	[DistSubTeam_No] [int] NULL,
	[CostedByWeight] [bit] NULL,
	[TaxClassID] [int] NULL,
	[Deleted_Item] [bit] NOT NULL CONSTRAINT [DF_ItemChangeHistory_DeletedItem]  DEFAULT ((0)),
	[User_ID] [int] NULL,
	[User_ID_Date] [datetime] NULL,
	[LabelType_ID] [int] NULL,
	[Insert_Date] [datetime] NOT NULL CONSTRAINT [DF_ItemChangeHistory_Insert_Date]  DEFAULT (getdate()),
	[QtyProhibit] [bit] NULL,
	[GroupList] [int] NULL,
	[Case_Discount] [bit] NULL,
	[Coupon_Multiplier] [bit] NULL,
	[Misc_Transaction_Sale] [smallint] NULL,
	[Misc_Transaction_Refund] [smallint] NULL,
	[Recall_Flag] [bit] NULL,
	[Manager_ID] [tinyint] NULL,
	[Ice_Tare] [int] NULL,
	[PurchaseThresholdCouponAmount] [smallmoney] NULL,
	[PurchaseThresholdCouponSubTeam] [bit] NULL,
	[Product_Code] [varchar](15) NULL,
	[Unit_Price_Category] [int] NULL,
	[StoreJurisdictionID] [int] NULL,
	[CatchweightRequired] [bit] NOT NULL CONSTRAINT [DF_ItemChangeHistory_CatchweightRequired]  DEFAULT ((0)),
	[SustainabilityRankingRequired] [bit] NULL,
	[SustainabilityRankingID] [int] NULL,
	[GiftCard] [bit] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING ON
GO
/******************************************************************************
		10. Populate FL Table
******************************************************************************/
PRINT N'Status: 10. Populate FL Table (Takes 15 seconds in TEST for 2 million rows): --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET NOCOUNT ON;
IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[ItemChangeHistory_Unaligned])
    BEGIN
        INSERT INTO [dbo].[ItemChangeHistory] ([Item_Key], [Item_Description], [Sign_Description], [Ingredients], [SubTeam_No], [Sales_Account], [Package_Desc1], [Package_Desc2], [Package_Unit_ID], [Min_Temperature], [Max_Temperature], [Units_Per_Pallet], [Average_Unit_Weight], [Tie], [High], [Yield], [Brand_ID], [Category_ID], [Origin_ID], [ShelfLife_Length], [ShelfLife_ID], [Retail_Unit_ID], [Vendor_Unit_ID], [Distribution_Unit_ID], [Cost_Unit_ID], [Freight_Unit_ID], [Discontinue_Item], [WFM_Item], [Not_Available], [Pre_Order], [Remove_Item], [NoDistMarkup], [Organic], [Refrigerated], [Keep_Frozen], [Shipper_Item], [Full_Pallet_Only], [POS_Description], [Retail_Sale], [Food_Stamps], [Discountable], [Price_Required], [Quantity_Required], [ItemType_ID], [HFM_Item], [ScaleDesc1], [ScaleDesc2], [Not_AvailableNote], [CountryProc_ID], [Host_Name], [Effective_Date], [Manufacturing_Unit_ID], [EXEDistributed], [ClassID], [DistSubTeam_No], [CostedByWeight], [Deleted_Item], [SustainabilityRankingRequired], [SustainabilityRankingID], [TaxClassID], [User_ID], [User_ID_Date], [LabelType_ID], [Insert_Date], [QtyProhibit], [GroupList], [Case_Discount], [Coupon_Multiplier], [Misc_Transaction_Sale], [Misc_Transaction_Refund], [Recall_Flag], [Manager_ID], [Ice_Tare], [PurchaseThresholdCouponAmount], [PurchaseThresholdCouponSubTeam], [Product_Code], [Unit_Price_Category], [StoreJurisdictionID], [CatchweightRequired], [GiftCard])
        SELECT src.[Item_Key],
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
               src.[Discontinue_Item],
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
               src.[Host_Name],
               src.[Effective_Date],
               src.[Manufacturing_Unit_ID],
               src.[EXEDistributed],
               src.[ClassID],
               src.[DistSubTeam_No],
               src.[CostedByWeight],
               ISNULL(src.[Deleted_Item],0) AS [Deleted_Item], -- adjusted for old south null data
               src.[SustainabilityRankingRequired],
               src.[SustainabilityRankingID],
               src.[TaxClassID],
               src.[User_ID],
               src.[User_ID_Date],
               src.[LabelType_ID],
               src.[Insert_Date],
               src.[QtyProhibit],
               src.[GroupList],
               src.[Case_Discount],
               src.[Coupon_Multiplier],
               src.[Misc_Transaction_Sale],
               src.[Misc_Transaction_Refund],
               src.[Recall_Flag],
               src.[Manager_ID],
               src.[Ice_Tare],
               src.[PurchaseThresholdCouponAmount],
               src.[PurchaseThresholdCouponSubTeam],
               src.[Product_Code],
               src.[Unit_Price_Category],
               src.[StoreJurisdictionID],
               src.[CatchweightRequired],
               src.[GiftCard]
        FROM   [dbo].[ItemChangeHistory_Unaligned] src;
    END
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]') AND name = N'ICHItemKeyInsertDateSubteamUserID')
CREATE NONCLUSTERED INDEX [ICHItemKeyInsertDateSubteamUserID] ON [dbo].[ItemChangeHistory]
(
	[Item_Key] ASC,
	[Insert_Date] ASC,
	[SubTeam_No] ASC,
	[User_ID] ASC
)
INCLUDE ( 	[Item_Description],
	[Sign_Description]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/******************************************************************************
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Manager__0E898877]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]'))
ALTER TABLE [dbo].[ItemChangeHistory]  WITH CHECK ADD  CONSTRAINT [FK__Item__Manager__0E898877] FOREIGN KEY([Manager_ID])
REFERENCES [dbo].[ItemManager] ([Manager_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Manager__0E898877]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]'))
ALTER TABLE [dbo].[ItemChangeHistory] CHECK CONSTRAINT [FK__Item__Manager__0E898877]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemChangeHistory_TaxClass]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]'))
ALTER TABLE [dbo].[ItemChangeHistory]  WITH CHECK ADD  CONSTRAINT [FK_ItemChangeHistory_TaxClass] FOREIGN KEY([TaxClassID])
REFERENCES [dbo].[TaxClass] ([TaxClassID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemChangeHistory_TaxClass]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]'))
ALTER TABLE [dbo].[ItemChangeHistory] CHECK CONSTRAINT [FK_ItemChangeHistory_TaxClass]
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers. (This Step is N/A)'
GO
/******************************************************************************
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
GRANT SELECT ON [dbo].[ItemChangeHistory] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemChangeHistory] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemChangeHistory] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemChangeHistory] TO [IRMASupportRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemChangeHistory] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemChangeHistory] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemChangeHistory] TO [sobluesky] AS [dbo]
GO
/******************************************************************************
		16. Check FL Checks (generated from VS schema compare)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[ItemChangeHistory] WITH CHECK CHECK CONSTRAINT [FK__Item__Manager__0E898877];
GO
ALTER TABLE [dbo].[ItemChangeHistory] WITH CHECK CHECK CONSTRAINT [FK_ItemChangeHistory_TaxClass];
GO
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM itemchangehistory_unaligned(NOLOCK)

SELECT @new = count(*)
FROM itemchangehistory(NOLOCK)

PRINT N'itemchangehistory_Unaligned Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'itemchangehistory Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count. --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count. --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
