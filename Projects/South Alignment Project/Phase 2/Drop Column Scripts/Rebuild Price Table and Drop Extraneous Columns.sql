/******************************************************************************
		1. Rebuild Price Table and Drop Extraneous Columns
******************************************************************************/
PRINT N'Status: 1. Rebuild Price Table and Drop Extraneous Columns --- [dbo].[Price] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO	
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[Price] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 1, SYSDATETIME()), 9)
GO
USE ItemCatalog
--USE ItemCatalog_Test
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[Price] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Multiple__514B77A6]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Multiple__514B77A6]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Price__523F9BDF]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Price__523F9BDF]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__MSRPPrice__5333C018]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__MSRPPrice__5333C018]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__MSRPMulti__5427E451]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__MSRPMulti__5427E451]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__PricingMe__551C088A]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__PricingMe__551C088A]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Mult__56102CC3]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Sale_Mult__56102CC3]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Pric__570450FC]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Sale_Pric__570450FC]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Max___57F87535]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Sale_Max___57F87535]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Earn__59E0BDA7]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Sale_Earn__59E0BDA7]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Earn__5AD4E1E0]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Sale_Earn__5AD4E1E0]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Earn__5BC90619]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Sale_Earn__5BC90619]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Tax_Table_A]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Tax_Table_A]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Tax_Table_B]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Tax_Table_B]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Tax_Table_C]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Tax_Table_C]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Tax_Table_D]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Tax_Table_D]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Restricted_Hours]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Restricted_Hours]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_IBM_Discount]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Price_IBM_Discount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Competiti__7552690E]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Competiti__7552690E]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__LocalTag__6EE562A1]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__LocalTag__6EE562A1]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_GlutenFreeTag]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Price_GlutenFreeTag]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_POSPrice]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Price_POSPrice]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_POSSale_Price]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Price_POSSale_Price]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_CompFlag]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Price_CompFlag]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_Discountable]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF_Price_Discountable]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__LocalItem__49E9E53D]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__LocalItem__49E9E53D]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Electroni__41D3F3F3]') AND type = 'D')
	ALTER TABLE [Price] DROP CONSTRAINT [DF__Price__Electroni__41D3F3F3]
GO
/******************************************************************************
		3. Drop Foreign Keys on SO Price
******************************************************************************/
PRINT N'Status: 3. Drop Foreign Keys on SO Price --- [dbo].[Price] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Price_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK_Price_LinkCode]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Price_CompetitivePriceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK_Price_CompetitivePriceType]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK_ExceptionSubTeam_SubTeam]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Store_No__4F632F34]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK__Price__Store_No__4F632F34]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Item_Key__4E6F0AFB]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK__Price__Item_Key__4E6F0AFB]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__KitchenRoute__ID__0E89555]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK__KitchenRoute__ID__0E89555]
GO
/******************************************************************************
		4. Drop Indexes on SO Price
******************************************************************************/
PRINT N'Status: 4. Drop Indexes on SO Price --- [dbo].[Price] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Price]') AND name = N'IX_PriceCompetitive')
DROP INDEX [IX_PriceCompetitive] ON [dbo].[Price]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Price]') AND name = N'IX_Price_ItemKey_INC_POSLinkCode')
DROP INDEX [IX_Price_ItemKey_INC_POSLinkCode] ON [dbo].[Price]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Price]') AND name = N'idxStoreNoItemKey')
DROP INDEX [idxStoreNoItemKey] ON [dbo].[Price]
GO
/******************************************************************************
		5. Drop Columns on SO Price
******************************************************************************/
PRINT N'Status: 5. Drop Columns on SO Price --- [dbo].[Price] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[Price]
DROP COLUMN [Competitive],
	COLUMN [CompetitiveLastChecked],
	COLUMN [CompetitiveMultiple],
	COLUMN [CompetitivePrice],
	COLUMN [GlutenFreeTag],
	COLUMN [LocalTag],
	COLUMN [Tax_Table_A],
	COLUMN [Tax_Table_B],
	COLUMN [Tax_Table_C],
	COLUMN [Tax_Table_D];
GO
/******************************************************************************
		6. Create Indexes from FL Price
******************************************************************************/
PRINT N'Status: 6. Create Indexes on FL Price --- [dbo].[Price] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Price]') AND name = N'idxStoreNoItemKey')
CREATE NONCLUSTERED INDEX [idxStoreNoItemKey] ON [dbo].[Price]
(
	[Store_No] ASC,
	[Item_Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Price]') AND name = N'IX_Price_ItemKey_INC_POSLinkCode')
CREATE NONCLUSTERED INDEX [IX_Price_ItemKey_INC_POSLinkCode] ON [dbo].[Price]
(
	[Item_Key] ASC
)
INCLUDE ([POSLinkCode]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
		7. Create Foreign Keys from FL Price
******************************************************************************/
PRINT N'Status: 7. Create Foreign Keys on FL Price --- [dbo].[Price] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__KitchenRoute__ID__0E89555]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK__KitchenRoute__ID__0E89555] FOREIGN KEY([KitchenRoute_ID])
REFERENCES [dbo].[KitchenRoute] ([KitchenRoute_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__KitchenRoute__ID__0E89555]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] CHECK CONSTRAINT [FK__KitchenRoute__ID__0E89555]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Item_Key__4E6F0AFB]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK__Price__Item_Key__4E6F0AFB] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Item_Key__4E6F0AFB]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] CHECK CONSTRAINT [FK__Price__Item_Key__4E6F0AFB]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Store_No__4F632F34]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK__Price__Store_No__4F632F34] FOREIGN KEY([Store_No])
REFERENCES [dbo].[Store] ([Store_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Store_No__4F632F34]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] CHECK CONSTRAINT [FK__Price__Store_No__4F632F34]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_ExceptionSubTeam_SubTeam] FOREIGN KEY([ExceptionSubteam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] CHECK CONSTRAINT [FK_ExceptionSubTeam_SubTeam]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Price_CompetitivePriceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_Price_CompetitivePriceType] FOREIGN KEY([CompetitivePriceTypeID])
REFERENCES [dbo].[CompetitivePriceType] ([CompetitivePriceTypeID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Price_CompetitivePriceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] CHECK CONSTRAINT [FK_Price_CompetitivePriceType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Price_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_Price_LinkCode] FOREIGN KEY([LinkedItem])
REFERENCES [dbo].[Item] ([Item_Key])
GO
/******************************************************************************
		8. Create FL Defaults (manually generated)
******************************************************************************/
PRINT N'Status: 8. Create FL Defaults (manually generated) --- [dbo].[Price] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO	
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Multiple__514B77A6]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__Multiple__514B77A6] DEFAULT ((1))
	FOR [Multiple]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Price__523F9BDF]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__Price__523F9BDF] DEFAULT ((0))
	FOR [Price]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__MSRPPrice__5333C018]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__MSRPPrice__5333C018] DEFAULT ((0))
	FOR [MSRPPrice]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__MSRPMulti__5427E451]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__MSRPMulti__5427E451] DEFAULT ((1))
	FOR [MSRPMultiple]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__PricingMe__551C088A]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__PricingMe__551C088A] DEFAULT ((0))
	FOR [PricingMethod_ID]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Mult__56102CC3]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__Sale_Mult__56102CC3] DEFAULT ((1))
	FOR [Sale_Multiple]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Pric__570450FC]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__Sale_Pric__570450FC] DEFAULT ((0))
	FOR [Sale_Price]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Max___57F87535]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__Sale_Max___57F87535] DEFAULT ((0))
	FOR [Sale_Max_Quantity]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Earn__59E0BDA7]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__Sale_Earn__59E0BDA7] DEFAULT ((0))
	FOR [Sale_Earned_Disc1]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Earn__5AD4E1E0]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__Sale_Earn__5AD4E1E0] DEFAULT ((0))
	FOR [Sale_Earned_Disc2]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Sale_Earn__5BC90619]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__Sale_Earn__5BC90619] DEFAULT ((0))
	FOR [Sale_Earned_Disc3]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Restricted_Hours]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF_Restricted_Hours] DEFAULT ((0))
	FOR [Restricted_Hours]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_IBM_Discount]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF_Price_IBM_Discount] DEFAULT ((0))
	FOR [IBM_Discount]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_POSPrice]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF_Price_POSPrice] DEFAULT ((0))
	FOR [POSPrice]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_POSSale_Price]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF_Price_POSSale_Price] DEFAULT ((0))
	FOR [POSSale_Price]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_CompFlag]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF_Price_CompFlag] DEFAULT ((0))
	FOR [CompFlag]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Price_Discountable]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF_Price_Discountable] DEFAULT ((1))
	FOR [Discountable]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__LocalItem__02C7CD7D]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__LocalItem__02C7CD7D] DEFAULT ((0))
	FOR [LocalItem]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Price__Electroni__3E8E0C81]') AND type = 'D')
	ALTER TABLE [Price] WITH NOCHECK ADD CONSTRAINT [DF__Price__Electroni__3E8E0C81] DEFAULT ((0))
	FOR [ElectronicShelfTag]
GO
/******************************************************************************
		9. Operation Complete
******************************************************************************/
PRINT N'Status: 9. Operation Complete... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
