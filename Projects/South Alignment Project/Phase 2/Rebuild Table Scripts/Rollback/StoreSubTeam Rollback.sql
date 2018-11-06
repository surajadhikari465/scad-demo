/******************************************************************************
		SO [dbo].[StoreSubTeam]
		Rollback
******************************************************************************/
PRINT N'Status: Begin [dbo].[StoreSubTeam] ROLLBACK  --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[StoreSubTeam]  --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 30, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[StoreSubTeam] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [StoreSubTeam] DROP CONSTRAINT [DF_StoreSubTeam_CasePriceDiscount]
GO
ALTER TABLE [StoreSubTeam] DROP CONSTRAINT [DF_StoreSubTeam_CostFactor]
GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers in FL.  This Step is N/A'
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Team]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] DROP CONSTRAINT [FK_StoreSubTeam_Team]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] DROP CONSTRAINT [FK_StoreSubTeam_SubTeam]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Store]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] DROP CONSTRAINT [FK_StoreSubTeam_Store]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_CycleCountVendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] DROP CONSTRAINT [FK_StoreSubTeam_CycleCountVendor]
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Indexes in FL.  This Step is N/A'
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]') AND name = N'PK_StoreSubTeam')
EXECUTE sp_rename N'[dbo].[PK_StoreSubTeam]', N'PK_StoreSubTeam_Rollback';
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StoreSubTeam_Unaligned]') AND name = N'PK_StoreSubTeam_Unaligned')
EXECUTE sp_rename N'[dbo].[PK_StoreSubTeam_Unaligned]', N'PK_StoreSubTeam';
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[StoreSubTeam]', N'StoreSubTeam_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreSubTeam_Unaligned]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[StoreSubTeam_Unaligned]', N'StoreSubTeam';
GO
/******************************************************************************
		10. Create SO Defaults (manually generated)
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [StoreSubTeam] WITH NOCHECK ADD CONSTRAINT [DF_StoreSubTeam_CasePriceDiscount] DEFAULT (0) FOR [CasePriceDiscount]
GO
ALTER TABLE [StoreSubTeam] WITH NOCHECK ADD CONSTRAINT [DF__StoreSubT__CostF__5DE755D6] DEFAULT ((0)) FOR [CostFactor]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[StoreSubTeam] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[StoreSubTeam] TO [IConInterface] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[StoreSubTeam] TO [iCONReportingRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[StoreSubTeam] TO [IMHARole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[StoreSubTeam] TO [IRMA_Teradata] AS [dbo]
GO
GRANT DELETE ON [dbo].[StoreSubTeam] TO [IRMAAdminRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[StoreSubTeam] TO [IRMAAdminRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[StoreSubTeam] TO [IRMAAdminRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[StoreSubTeam] TO [IRMAAdminRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[StoreSubTeam] TO [IRMAAVCIRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[StoreSubTeam] TO [IRMAClientRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[StoreSubTeam] TO [IRMAReports] AS [dbo]
GO
GRANT SELECT ON [dbo].[StoreSubTeam] TO [IRMAReportsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[StoreSubTeam] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[StoreSubTeam] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[StoreSubTeam] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[StoreSubTeam] TO [IRMASLIMRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[StoreSubTeam] TO [IRMASupportRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[StoreSubTeam] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[StoreSubTeam] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[StoreSubTeam] TO [sobluesky] AS [dbo]
GO
/******************************************************************************
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]') AND name = N'idxStoreSubTeam_GetPriceBatchSent')
CREATE NONCLUSTERED INDEX [idxStoreSubTeam_GetPriceBatchSent] ON [dbo].[StoreSubTeam]
(
	[Store_No] ASC,
	[SubTeam_No] ASC
)
INCLUDE ( 	[CasePriceDiscount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/******************************************************************************
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_CycleCountVendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_StoreSubTeam_CycleCountVendor] FOREIGN KEY([ICVID])
REFERENCES [dbo].[CycleCountVendor] ([ICVID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_CycleCountVendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] CHECK CONSTRAINT [FK_StoreSubTeam_CycleCountVendor]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Store]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH NOCHECK ADD  CONSTRAINT [FK_StoreSubTeam_Store] FOREIGN KEY([Store_No])
REFERENCES [dbo].[Store] ([Store_No])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Store]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] CHECK CONSTRAINT [FK_StoreSubTeam_Store]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH NOCHECK ADD  CONSTRAINT [FK_StoreSubTeam_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] CHECK CONSTRAINT [FK_StoreSubTeam_SubTeam]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Team]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH NOCHECK ADD  CONSTRAINT [FK_StoreSubTeam_Team] FOREIGN KEY([Team_No])
REFERENCES [dbo].[Team] ([Team_No])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Team]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] CHECK CONSTRAINT [FK_StoreSubTeam_Team]
GO

/******************************************************************************
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'There are no SO Triggers. (This Step is N/A.)'
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'There are no SO Extended Properties. (This Step is N/A.)'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: 17. Finish Up --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: **** Operation Complete ****: --- [dbo].[StoreSubTeam] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO