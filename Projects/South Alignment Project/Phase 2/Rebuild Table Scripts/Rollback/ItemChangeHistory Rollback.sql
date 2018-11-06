/******************************************************************************
	SO ItemChangeHistory
	Rollback
******************************************************************************/
PRINT N'Status: Begin ItemChangeHistory Rollback'+ ' --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 5, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO

SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_Host_Name]
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_Effective_Date]
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_EXEDistributed]
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_DeletedItem]
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_Insert_Date]
GO
ALTER TABLE [ItemChangeHistory] DROP CONSTRAINT [DF_ItemChangeHistory_CatchweightRequired]
GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemChangeHistory_TaxClass]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]'))
ALTER TABLE [dbo].[ItemChangeHistory] DROP CONSTRAINT [FK_ItemChangeHistory_TaxClass]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Manager__0E898877]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]'))
ALTER TABLE [dbo].[ItemChangeHistory] DROP CONSTRAINT [FK__Item__Manager__0E898877]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]') AND name = N'ICHItemKeyInsertDateSubteamUserID')
DROP INDEX [ICHItemKeyInsertDateSubteamUserID] ON [dbo].[ItemChangeHistory]
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]') AND name = N'ICHItemKeyInsertDateSubteamUserID')
DROP INDEX [ICHItemKeyInsertDateSubteamUserID] ON [dbo].[ItemChangeHistory]
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No PK on ItemChangeHistory (This Step is N/A)'
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No PK on ItemChangeHistory (This Step is N/A)'
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[ItemChangeHistory]', N'ItemChangeHistory_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[ItemChangeHistory_Unaligned]', N'ItemChangeHistory';
GO
/******************************************************************************
		10. Create SO Defaults
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [ItemChangeHistory]WITH NOCHECK ADD CONSTRAINT [DF_ItemChangeHistory_Host_Name] DEFAULT (host_name()) FOR [Host_Name]
GO
ALTER TABLE [ItemChangeHistory]WITH NOCHECK ADD CONSTRAINT [DF_ItemChangeHistory_Effective_Date] DEFAULT (getdate()) FOR [Effective_Date]
GO
ALTER TABLE [ItemChangeHistory]WITH NOCHECK ADD CONSTRAINT [DF_ItemChangeHistory_EXEDistributed] DEFAULT (0) FOR [EXEDistributed]
GO
ALTER TABLE [ItemChangeHistory]WITH NOCHECK ADD CONSTRAINT [DF_ItemChangeHistory_Insert_Date] DEFAULT (getdate()) FOR [Insert_Date]
GO
ALTER TABLE [ItemChangeHistory]WITH NOCHECK ADD CONSTRAINT [DF_ItemChangeHistory_CatchweightRequired] DEFAULT ((0)) FOR [CatchweightRequired]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemChangeHistory]') AND name = N'idxItemChangeHistory_EffectiveDate')
CREATE CLUSTERED INDEX [idxItemChangeHistory_EffectiveDate] ON [dbo].[ItemChangeHistory]
(
	[Effective_Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
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
	[Sign_Description]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'(This Step is N/A)'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: 17. Finish Up --- [dbo].[ItemChangeHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO

PRINT N'Status: **** Operation Complete ****: --- [dbo].[ItemChangeHistory] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO