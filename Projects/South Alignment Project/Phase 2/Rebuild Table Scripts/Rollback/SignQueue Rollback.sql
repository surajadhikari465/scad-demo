/******************************************************************************
		SO [dbo].[SignQueue]
		Rollback
******************************************************************************/
PRINT N'Status: Begin [dbo].[SignQueue] Rollback --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[SignQueue]  --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 30, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking on [dbo].[SignQueue] (This Step is N/A)'
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__Sign___01EEAF01]
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__New_I__33AD2C35]
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__Price__34A1506E]
GO
ALTER TABLE [SignQueue] DROP CONSTRAINT [DF__SignQueue__Item___359574A7]
GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers on [dbo].[SignQueue] (This Step is N/A)'
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SignQueue]') AND name = N'IX_SignQueue_Store_No')
DROP INDEX [IX_SignQueue_Store_No] ON [dbo].[SignQueue]
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'PK_SignQueue_Rollback') AND type in (N'U'))
ALTER TABLE [dbo].[SignQueue] DROP CONSTRAINT PK_SignQueue_Rollback
GO
EXECUTE sp_rename N'[dbo].[PK_SignQueue]', N'PK_SignQueue_Rollback';
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[PK_SignQueue_Unaligned]', N'PK_SignQueue';
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SignQueue_Rollback]') AND type in (N'U'))  
DROP TABLE dbo.SignQueue_Rollback
GO
EXECUTE sp_rename N'[dbo].[SignQueue]', N'SignQueue_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[SignQueue_Unaligned]', N'SignQueue';
GO
/******************************************************************************
		10. Create SO Defaults
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [SignQueue] WITH NOCHECK ADD CONSTRAINT [DF__SignQueue__Sign___01EEAF01] DEFAULT (0) FOR [Sign_Printed]
GO
ALTER TABLE [SignQueue] WITH NOCHECK ADD CONSTRAINT [DF__SignQueue__New_I__33AD2C35] DEFAULT (0) FOR [New_Item]
GO
ALTER TABLE [SignQueue] WITH NOCHECK ADD CONSTRAINT [DF__SignQueue__Price__34A1506E] DEFAULT (0) FOR [Price_Change]
GO
ALTER TABLE [SignQueue] WITH NOCHECK ADD CONSTRAINT [DF__SignQueue__Item___359574A7] DEFAULT (0) FOR [Item_Change]
GO
ALTER TABLE [SignQueue] WITH NOCHECK ADD CONSTRAINT [DF__SignQueue__Compe__76468D47] DEFAULT ((0)) FOR [Competitive]
GO
ALTER TABLE [SignQueue] WITH NOCHECK ADD CONSTRAINT [DF__SignQueue__Local__6FD986DA] DEFAULT ((0)) FOR [LocalTag]
GO
ALTER TABLE [SignQueue] WITH NOCHECK ADD CONSTRAINT [DF_SignQueue_GlutenFreeTag] DEFAULT ((0)) FOR [GlutenFreeTag]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking on SignQueue (This Step is N/A)'
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Item___7588D81C]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue]  WITH NOCHECK ADD  CONSTRAINT [FK__SignQueue__Item___7588D81C] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Item___7588D81C]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] CHECK CONSTRAINT [FK__SignQueue__Item___7588D81C]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Store__767CFC55]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue]  WITH NOCHECK ADD  CONSTRAINT [FK__SignQueue__Store__767CFC55] FOREIGN KEY([Store_No])
REFERENCES [dbo].[Store] ([Store_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__Store__767CFC55]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] CHECK CONSTRAINT [FK__SignQueue__Store__767CFC55]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue]  WITH NOCHECK ADD  CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] CHECK CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4]
GO
/******************************************************************************
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers on SignQueue (This Step is N/A)'
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SignQueue', N'COLUMN',N'LastQueuedType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1=Price Batch; 2=Scan Gun; 3=Manual' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SignQueue', @level2type=N'COLUMN',@level2name=N'LastQueuedType'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: 17. Finish Up --- [dbo].[SignQueue] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: **** Operation Complete ****: --- [dbo].[SignQueue] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO