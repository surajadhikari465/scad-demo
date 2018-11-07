/******************************************************************************
		SO [dbo].[SubTeam]
		Rollback
******************************************************************************/
PRINT N'Status: Begin [dbo].[SubTeam] ROLLBACK  --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[SubTeam]  --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 30, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO

SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[SubTeam] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF_SubTeam_Target_Margin]
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF_SubTeam_EXEWarehouseSent]
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF_SubTeam_Retail]
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF__SubTeam__EXEDist__0AD533D4]
GO
--ALTER TABLE [SubTeam] DROP CONSTRAINT [DF__SubTeam__Beverag__63CFDBBF]
--GO
--ALTER TABLE [SubTeam] DROP CONSTRAINT [DF__SubTeam__Aligned__5C4263AC]
--GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[SubTeamAddUpdate]'))
DROP TRIGGER [dbo].[SubTeamAddUpdate]
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys on SubTeam (This Step is N/A).'
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Indexes on SubTeam (This Step is N/A).'
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SubTeam]') AND name = N'PK_SubTeam_SubTeam_No')
EXECUTE sp_rename N'[dbo].[PK_SubTeam_SubTeam_No]', N'PK_SubTeam_SubTeam_No_Rollback';
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SubTeam_Unaligned]') AND name = N'PK_SubTeam_SubTeam_No_Unaligned')
EXECUTE sp_rename N'[dbo].[PK_SubTeam_SubTeam_No_Unaligned]', N'PK_SubTeam_SubTeam_No';
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SubTeam]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[SubTeam]', N'SubTeam_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SubTeam_Unaligned]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[SubTeam_Unaligned]', N'SubTeam';
GO
/******************************************************************************
		10. Create SO Defaults (manually generated)
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [SubTeam] WITH NOCHECK ADD CONSTRAINT [DF_SubTeam_Target_Margin] DEFAULT (0) FOR [Target_Margin]
GO
ALTER TABLE [SubTeam] WITH NOCHECK ADD CONSTRAINT [DF_SubTeam_EXEWarehouseSent] DEFAULT (0) FOR [EXEWarehouseSent]
GO
ALTER TABLE [SubTeam] WITH NOCHECK ADD CONSTRAINT [DF_SubTeam_Retail] DEFAULT (0) FOR [Retail]
GO
ALTER TABLE [SubTeam] WITH NOCHECK ADD CONSTRAINT [DF__SubTeam__EXEDist__0AD533D4] DEFAULT (0) FOR [EXEDistributed]
GO
ALTER TABLE [SubTeam] WITH NOCHECK ADD CONSTRAINT [DF__SubTeam__Beverag__2050A292] DEFAULT ((0)) FOR [Beverage]
GO
ALTER TABLE [SubTeam] WITH NOCHECK ADD CONSTRAINT [DF__SubTeam__Aligned__05F8D820] DEFAULT ((0)) FOR [AlignedSubTeam]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[SubTeam] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[SubTeam] TO [BizTalk] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [ExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IConInterface] AS [dbo]
GO
GRANT UPDATE ON [dbo].[SubTeam] TO [IConInterface] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[SubTeam] TO [IConInterface] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [iCONReportingRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[SubTeam] TO [iCONReportingRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IMHARole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[SubTeam] TO [IRMA_Teradata] AS [dbo]
GO
GRANT DELETE ON [dbo].[SubTeam] TO [IRMAAdminRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[SubTeam] TO [IRMAAdminRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[SubTeam] TO [IRMAAdminRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[SubTeam] TO [IRMAAdminRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IRMAAVCIRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IRMAPDXExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IRMAPromoRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[SubTeam] TO [IRMAReports] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IRMAReportsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[SubTeam] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[SubTeam] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IRMASLIMRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IRMASupportRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[SubTeam] TO [IRMASupportRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [sobluesky] AS [dbo]
GO
GRANT SELECT ON [dbo].[SubTeam] TO [SODataControl] AS [dbo]
GO
/******************************************************************************
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Indexes on SubTeam (This Step is N/A).'
GO
/******************************************************************************
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys on SubTeam (This Step is N/A).'
GO
/******************************************************************************
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[SubTeamAddUpdate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[SubTeamAddUpdate]
ON [dbo].[SubTeam]
FOR INSERT, UPDATE
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
    INSERT INTO PMProductChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT ''SubTeam'', CONVERT(varchar(255), Inserted.SubTeam_No), Inserted.SubTeam_Name, CONVERT(varchar(255), Inserted.Team_No),
           Team.Team_Name, CASE WHEN Deleted.SubTeam_No IS NULL THEN ''ADD'' ELSE ''CHANGE'' END
    FROM Inserted
    INNER JOIN
        Team
        ON Team.Team_No = Inserted.Team_No
    LEFT JOIN
        Deleted
        ON Inserted.SubTeam_No = Deleted.SubTeam_No
    WHERE (ISNULL(Deleted.SubTeam_Name, '''') <> ISNULL(Inserted.SubTeam_Name, ''''))
          OR (ISNULL(Deleted.Team_No, 0) <> ISNULL(Inserted.Team_No, 0))
    SELECT @Error_No = @@ERROR
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR (''SubTeamAddUpdate trigger failed with @@ERROR: %d'', @Severity, 1, @Error_No)
    END
END' 
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'SubTeam_No'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sub team number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'SubTeam_No'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'Team_No'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Team number refer to the team table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'Team_No'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'SubTeam_Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sub team name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'SubTeam_Name'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'SubTeam_Abbreviation'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sub team abbreviation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'SubTeam_Abbreviation'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'SubTeamType_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 = Retail, 2 = Manufacturing, 3 = RetailManufacturing, 4 = Expense, 5 = Packaging, 6 = Supplies, 7=Front End' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'SubTeamType_ID'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: 17. Finish Up --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: **** Operation Complete ****: --- [dbo].[SubTeam] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
