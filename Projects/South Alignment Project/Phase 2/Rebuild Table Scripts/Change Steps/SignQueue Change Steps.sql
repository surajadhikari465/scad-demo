/******************************************************************************
		SO [dbo].[SignQueue]
		Change Steps
******************************************************************************/
PRINT N'Status: Begin [dbo].[SignQueue]  - Rebuild (takes about 45 minutes in TEST): --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 45, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__Sign___01EEAF01]
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__New_I__33AD2C35]
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__Price__34A1506E]
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__Item___359574A7]
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__Compe__76468D47]
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__Local__6FD986DA]
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF_SignQueue_GlutenFreeTag]
GO
/******************************************************************************
		3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SignQueue', N'COLUMN',N'LastQueuedType'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SignQueue', @level2type=N'COLUMN',@level2name=N'LastQueuedType'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] DROP CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Store__767CFC55]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] DROP CONSTRAINT [FK__SignQueue__Store__767CFC55]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Item___7588D81C]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] DROP CONSTRAINT [FK__SignQueue__Item___7588D81C]
GO
/******************************************************************************
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SignQueue]') AND name = N'IX_SignQueue_Store_No')
DROP INDEX [IX_SignQueue_Store_No] ON [dbo].[SignQueue]
GO
/******************************************************************************
		7. Rename SO PK 
******************************************************************************/
PRINT N'Status: 7. Rename SO PK  --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SignQueue]') AND name = N'PK_SignQueue')
EXECUTE sp_rename N'[dbo].[PK_SignQueue]', N'PK_SignQueue_Unaligned';
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[SignQueue]', N'SignQueue_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SignQueue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SignQueue](
	[Item_Key] [int] NOT NULL,
	[Store_No] [int] NOT NULL,
	[Sign_Description] [varchar](60) NOT NULL,
	[Ingredients] [varchar](3500) NULL,
	[Identifier] [varchar](13) NOT NULL,
	[Sold_By_Weight] [bit] NOT NULL,
	[Multiple] [tinyint] NOT NULL,
	[Price] [smallmoney] NOT NULL,
	[MSRPMultiple] [tinyint] NOT NULL,
	[MSRPPrice] [smallmoney] NOT NULL,
	[Case_Price] [money] NOT NULL,
	[Sale_Multiple] [tinyint] NOT NULL,
	[Sale_Price] [smallmoney] NOT NULL,
	[Sale_Start_Date] [smalldatetime] NULL,
	[Sale_End_Date] [smalldatetime] NULL,
	[Sale_Earned_Disc1] [tinyint] NOT NULL,
	[Sale_Earned_Disc2] [tinyint] NOT NULL,
	[Sale_Earned_Disc3] [tinyint] NOT NULL,
	[PricingMethod_ID] [int] NULL,
	[SubTeam_No] [int] NULL,
	[Origin_Name] [varchar](25) NULL,
	[Brand_Name] [varchar](25) NULL,
	[Retail_Unit_Abbr] [varchar](5) NULL,
	[Retail_Unit_Full] [varchar](25) NULL,
	[Package_Unit] [varchar](5) NULL,
	[Package_Desc1] [decimal](9, 4) NOT NULL,
	[Package_Desc2] [decimal](9, 4) NOT NULL,
	[Sign_Printed] [tinyint] NOT NULL CONSTRAINT [DF__SignQueue__Sign___01EEAF01]  DEFAULT ((0)),
	[Organic] [bit] NOT NULL,
	[Vendor_Id] [int] NULL,
	[User_ID] [int] NOT NULL,
	[User_ID_Date] [datetime] NOT NULL,
	[ItemType_ID] [int] NOT NULL,
	[ScaleDesc1] [varchar](64) NULL,
	[ScaleDesc2] [varchar](64) NULL,
	[POS_Description] [varchar](26) NULL,
	[Restricted_Hours] [bit] NOT NULL,
	[Quantity_Required] [bit] NOT NULL,
	[Price_Required] [bit] NOT NULL,
	[Retail_Sale] [bit] NOT NULL,
	[Discountable] [bit] NOT NULL,
	[Food_Stamps] [bit] NOT NULL,
	[IBM_Discount] [bit] NOT NULL,
	[New_Item] [bit] NOT NULL CONSTRAINT [DF__SignQueue__New_I__33AD2C35]  DEFAULT ((0)),
	[Price_Change] [bit] NOT NULL CONSTRAINT [DF__SignQueue__Price__34A1506E]  DEFAULT ((0)),
	[Item_Change] [bit] NOT NULL CONSTRAINT [DF__SignQueue__Item___359574A7]  DEFAULT ((0)),
	[LastQueuedType] [tinyint] NOT NULL,
	[POSPrice] [smallmoney] NULL,
	[POSSale_Price] [smallmoney] NULL,
	[PriceChgTypeId] [tinyint] NULL,
	[TagTypeID] [int] NULL,
	[TagTypeID2] [int] NULL,
	[Retail_Unit_ID] [int] NULL,
	[Package_Unit_ID] [int] NULL,
	[Item_Description] [varchar](60) NULL,
	[Case_Discount] [bit] NULL,
	[Coupon_Multiplier] [bit] NULL,
	[Misc_Transaction_Sale] [smallint] NULL,
	[Misc_Transaction_Refund] [smallint] NULL,
	[Recall_Flag] [bit] NULL,
	[Ice_Tare] [int] NULL,
	[Product_Code] [varchar](15) NULL,
	[Unit_Price_Category] [int] NULL,
	[NotAuthorizedForSale] [bit] NULL,
	[KitchenRoute_ID] [int] NULL,
	[Routing_Priority] [tinyint] NULL,
	[Consolidate_Price_To_Prev_Item] [bit] NULL,
	[Print_Condiment_On_Receipt] [bit] NULL,
	[Age_Restrict] [bit] NULL,
	[LocalItem] [bit] NULL,
	[ItemSurcharge] [int] NULL,
 CONSTRAINT [PK_SignQueue] PRIMARY KEY CLUSTERED 
(
	[Item_Key] ASC,
	[Store_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING ON
GO
/******************************************************************************
		10. Populate FL Table in Batches
******************************************************************************/
PRINT N'Status: 10. Populate FL Table in Batches (takes about 45 minutes in TEST for batches of 10000): --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for this Step to Complete: --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 45, SYSDATETIME()), 9)
GO
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SET XACT_ABORT ON;
IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[SignQueue_Unaligned])
    BEGIN
		
		IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiqnQueueCount]') AND type in (N'U'))  
		DROP TABLE [dbo].[SiqnQueueCount];
		
-- This Table is used to create a quasi-unique id so that we can use batches to insert into the aligned table:

		CREATE TABLE [dbo].[SiqnQueueCount] (rn INT NOT NULL, Item_Key INT NOT NULL, Store_No INT NOT NULL);
		
		INSERT INTO [dbo].[SiqnQueueCount] (rn, Item_Key, Store_No)
		SELECT ROW_NUMBER() OVER (
				ORDER BY item_key ASC,
					store_no ASC
				) AS rn,
			sq.item_key,
			sq.store_no
		FROM [dbo].[SignQueue_Unaligned] sq(NOLOCK)
	END
GO
IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[SignQueue_Unaligned])
    BEGIN
		        		
		DECLARE @RowsToLoad BIGINT;
		DECLARE @RowsPerBatch INT = 10000;
		DECLARE @LeftBoundary BIGINT = 0;
		DECLARE @RightBoundary BIGINT = @RowsPerBatch;

		SELECT @RowsToLoad = MAX([RN]) FROM SiqnQueueCount

		WHILE @LeftBoundary < @RowsToLoad
		BEGIN		
			INSERT INTO [dbo].[SignQueue] ([Item_Key], [Store_No], [Sign_Description], [Ingredients], [Identifier], [Sold_By_Weight], [Multiple], [Price], [MSRPMultiple], [MSRPPrice], [Case_Price], [Sale_Multiple], [Sale_Price], [Sale_Start_Date], [Sale_End_Date], [Sale_Earned_Disc1], [Sale_Earned_Disc2], [Sale_Earned_Disc3], [PricingMethod_ID], [SubTeam_No], [Origin_Name], [Brand_Name], [Retail_Unit_Abbr], [Retail_Unit_Full], [Package_Unit], [Package_Desc1], [Package_Desc2], [Sign_Printed], [Organic], [Vendor_Id], [User_ID], [User_ID_Date], [ItemType_ID], [ScaleDesc1], [ScaleDesc2], [POS_Description], [Restricted_Hours], [Quantity_Required], [Price_Required], [Retail_Sale], [Discountable], [Food_Stamps], [IBM_Discount], [New_Item], [Price_Change], [Item_Change], [LastQueuedType], [POSPrice], [POSSale_Price], [PriceChgTypeId], [TagTypeID], [TagTypeID2], [Retail_Unit_ID], [Package_Unit_ID], [Item_Description], [Case_Discount], [Coupon_Multiplier], [Misc_Transaction_Sale], [Misc_Transaction_Refund], [Recall_Flag], [Ice_Tare], [Product_Code], [Unit_Price_Category], [NotAuthorizedForSale], [KitchenRoute_ID], [Routing_Priority], [Consolidate_Price_To_Prev_Item], [Print_Condiment_On_Receipt], [Age_Restrict], [LocalItem], [ItemSurcharge])
			SELECT 
				src.[Item_Key],
				src.[Store_No],
				src.[Sign_Description],
				src.[Ingredients],
				src.[Identifier],
				src.[Sold_By_Weight],
				src.[Multiple],
				src.[Price],
				src.[MSRPMultiple],
				src.[MSRPPrice],
				src.[Case_Price],
				src.[Sale_Multiple],
				src.[Sale_Price],
				src.[Sale_Start_Date],
				src.[Sale_End_Date],
				src.[Sale_Earned_Disc1],
				src.[Sale_Earned_Disc2],
				src.[Sale_Earned_Disc3],
				src.[PricingMethod_ID],
				src.[SubTeam_No],
				src.[Origin_Name],
				src.[Brand_Name],
				src.[Retail_Unit_Abbr],
				src.[Retail_Unit_Full],
				src.[Package_Unit],
				src.[Package_Desc1],
				src.[Package_Desc2],
				src.[Sign_Printed],
				src.[Organic],
				src.[Vendor_Id],
				src.[User_ID],
				src.[User_ID_Date],
				src.[ItemType_ID],
				src.[ScaleDesc1],
				src.[ScaleDesc2],
				src.[POS_Description],
				src.[Restricted_Hours],
				src.[Quantity_Required],
				src.[Price_Required],
				src.[Retail_Sale],
				src.[Discountable],
				src.[Food_Stamps],
				src.[IBM_Discount],
				src.[New_Item],
				src.[Price_Change],
				src.[Item_Change],
				src.[LastQueuedType],
				src.[POSPrice],
				src.[POSSale_Price],
				src.[PriceChgTypeId],
				src.[TagTypeID],
				src.[TagTypeID2],
				src.[Retail_Unit_ID],
				src.[Package_Unit_ID],
				src.[Item_Description],
				src.[Case_Discount],
				src.[Coupon_Multiplier],
				src.[Misc_Transaction_Sale],
				src.[Misc_Transaction_Refund],
				src.[Recall_Flag],
				src.[Ice_Tare],
				src.[Product_Code],
				src.[Unit_Price_Category],
				src.[NotAuthorizedForSale],
				src.[KitchenRoute_ID],
				src.[Routing_Priority],
				src.[Consolidate_Price_To_Prev_Item],
				src.[Print_Condiment_On_Receipt],
				src.[Age_Restrict],
				src.[LocalItem],
				src.[ItemSurcharge]
			FROM [dbo].[SignQueue_Unaligned] src
			JOIN [dbo].[SiqnQueueCount] c on src.[Item_key] = c.[Item_Key] and src.[Store_No] = c.[Store_No]
			WHERE
				c.[RN] > @LeftBoundary
				AND c.[RN] <= @RightBoundary
			ORDER BY c.[RN]

			SET @LeftBoundary = @LeftBoundary + @RowsPerBatch;
			SET @RightBoundary = @RightBoundary + @RowsPerBatch;

		END
    END
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
GO
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SignQueue]') AND name = N'IX_SignQueue_Store_No')
CREATE NONCLUSTERED INDEX [IX_SignQueue_Store_No] ON [dbo].[SignQueue]
(
	[Store_No] ASC
)
INCLUDE ( 	[Sign_Description],
	[SubTeam_No],
	[Identifier],
	[Brand_Name],
	[Multiple],
	[Sale_Multiple],
	[Price],
	[Sale_Price],
	[PriceChgTypeId],
	[Sign_Printed],
	[LastQueuedType]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Item___7588D81C]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue]  WITH CHECK ADD  CONSTRAINT [FK__SignQueue__Item___7588D81C] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Item___7588D81C]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] CHECK CONSTRAINT [FK__SignQueue__Item___7588D81C]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Store__767CFC55]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue]  WITH CHECK ADD  CONSTRAINT [FK__SignQueue__Store__767CFC55] FOREIGN KEY([Store_No])
REFERENCES [dbo].[Store] ([Store_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Store__767CFC55]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] CHECK CONSTRAINT [FK__SignQueue__Store__767CFC55]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue]  WITH CHECK ADD  CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] CHECK CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4]
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
GRANT SELECT ON [dbo].[SignQueue] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SignQueue] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SignQueue] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SignQueue] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[SignQueue] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[SignQueue] TO [sobluesky] AS [dbo]
GO
/******************************************************************************
		16. Check FL Checks (generated from VS schema compare)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[SignQueue] WITH CHECK CHECK CONSTRAINT [FK__SignQueue__Item___7588D81C]
GO
ALTER TABLE [dbo].[SignQueue] WITH CHECK CHECK CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4]
GO
ALTER TABLE [dbo].[SignQueue] WITH CHECK CHECK CONSTRAINT [FK__SignQueue__Store__767CFC55]
GO
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables (takes about 5 minutes in TEST): --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM SignQueue_unaligned(NOLOCK)

SELECT @new = count(*)
FROM SignQueue(NOLOCK)

PRINT N'SignQueue_Unaligned Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'SignQueue Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count. --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count. --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
/******************************************************************************
		18. Drop Count Table 
******************************************************************************/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiqnQueueCount]') AND type in (N'U'))  
DROP TABLE [dbo].[SiqnQueueCount]

