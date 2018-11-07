/******************************************************************************
		SO [dbo].[SubTeam]
		Change Steps
******************************************************************************/
PRINT N'Status: BEGIN FL [dbo].[SubTeam] ChangeSteps (takes about 1.5 hours in TEST):'+ ' ---  [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 30, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[SubTeam] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF_SubTeam_Target_Margin]
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF_SubTeam_EXEWarehouseSent]
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF_SubTeam_Retail]
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF__SubTeam__EXEDist__0AD533D4]
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF__SubTeam__Beverag__2050A292]
GO
ALTER TABLE [SubTeam] DROP CONSTRAINT [DF__SubTeam__Aligned__05F8D820]
GO
/******************************************************************************
		3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'SubTeamType_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'SubTeamType_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'SubTeam_Abbreviation'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'SubTeam_Abbreviation'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'SubTeam_Name'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'SubTeam_Name'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'Team_No'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'Team_No'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SubTeam', N'COLUMN',N'SubTeam_No'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubTeam', @level2type=N'COLUMN',@level2name=N'SubTeam_No'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[SubTeamAddUpdate]'))
DROP TRIGGER [dbo].[SubTeamAddUpdate]
GO
/******************************************************************************
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys on SubTeam (This Step is N/A).'
GO
/******************************************************************************
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Indexes on SubTeam (This Step is N/A).'
GO
/******************************************************************************
		7. Rename SO PK 
******************************************************************************/
PRINT N'Status: 7. Rename SO PK  --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SubTeam]') AND name = N'PK_SubTeam_SubTeam_No')
EXECUTE sp_rename N'[dbo].[PK_SubTeam_SubTeam_No]', N'PK_SubTeam_SubTeam_No_Unaligned';
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[SubTeam]', N'SubTeam_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SubTeam]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SubTeam](
	[SubTeam_No] [int] NOT NULL,
	[Team_No] [int] NULL,
	[SubTeam_Name] [varchar](100) NULL,
	[SubTeam_Abbreviation] [varchar](10) NULL,
	[Dept_No] [int] NULL,
	[SubDept_No] [int] NULL,
	[Buyer_User_ID] [int] NULL,
	[Target_Margin] [decimal](9, 4) NOT NULL CONSTRAINT [DF_SubTeam_Target_Margin]  DEFAULT ((0)),
	[JDA] [int] NULL,
	[GLPurchaseAcct] [int] NULL,
	[GLDistributionAcct] [int] NULL,
	[GLTransferAcct] [int] NULL,
	[GLSalesAcct] [int] NULL,
	[Transfer_To_Markup] [decimal](9, 4) NULL,
	[EXEWarehouseSent] [bit] NOT NULL CONSTRAINT [DF_SubTeam_EXEWarehouseSent]  DEFAULT ((0)),
	[ScaleDept] [int] NULL,
	[Retail] [bit] NOT NULL CONSTRAINT [DF_SubTeam_Retail]  DEFAULT ((0)),
	[EXEDistributed] [bit] NOT NULL CONSTRAINT [DF__SubTeam__EXEDist__0AD533D4]  DEFAULT ((0)),
	[SubTeamType_ID] [tinyint] NULL,
	[PurchaseThresholdCouponAvailable] [bit] NULL,
	[GLSuppliesAcct] [int] NULL,
	[GLPackagingAcct] [int] NULL,
	[FixedSpoilage] [bit] NULL,
	[InventoryCountByCase] [bit] NULL,
	[Beverage] [bit] NULL DEFAULT ((0)),
	[AlignedSubTeam] [bit] NULL DEFAULT ((0)),
 CONSTRAINT [PK_SubTeam_SubTeam_No] PRIMARY KEY CLUSTERED 
(
	[SubTeam_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/******************************************************************************
		10. Populate FL Table
******************************************************************************/
PRINT N'Status: 10. Populate FL Table --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].SubTeam_Unaligned)
    BEGIN
        INSERT INTO [dbo].[SubTeam] ([SubTeam_No], [Team_No], [SubTeam_Name], [SubTeam_Abbreviation], [Dept_No], [SubDept_No], [Buyer_User_ID], [Target_Margin], [JDA], [GLPurchaseAcct], [GLDistributionAcct], [GLTransferAcct], [GLSalesAcct], [Transfer_To_Markup], [EXEWarehouseSent], [ScaleDept], [Retail], [EXEDistributed], [SubTeamType_ID], [GLPackagingAcct], [GLSuppliesAcct], [PurchaseThresholdCouponAvailable], [FixedSpoilage], [InventoryCountByCase], [Beverage], [AlignedSubTeam])
        SELECT   src.[SubTeam_No],
                 src.[Team_No],
                 src.[SubTeam_Name],
                 src.[SubTeam_Abbreviation],
                 src.[Dept_No],
                 src.[SubDept_No],
                 src.[Buyer_User_ID],
                 src.[Target_Margin],
                 src.[JDA],
                 src.[GLPurchaseAcct],
                 src.[GLDistributionAcct],
                 src.[GLTransferAcct],
                 src.[GLSalesAcct],
                 src.[Transfer_To_Markup],
                 src.[EXEWarehouseSent],
                 src.[ScaleDept],
                 src.[Retail],
                 src.[EXEDistributed],
                 src.[SubTeamType_ID],
                 src.[GLPackagingAcct],
                 src.[GLSuppliesAcct],
                 src.[PurchaseThresholdCouponAvailable],
                 src.[FixedSpoilage],
                 src.[InventoryCountByCase],
                 src.[Beverage],
                 src.[AlignedSubTeam]
        FROM     [dbo].[SubTeam_Unaligned] src
        ORDER BY [SubTeam_No] ASC;
    END
GO
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[SubTeam] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Indexes on SubTeam (This Step is N/A).'
GO
/******************************************************************************
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys on SubTeam (This Step is N/A).'
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		16. Check FL Checks (generated from VS schema compare)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Check Checks generated from schema compare'
GO 
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables (takes about 5 minutes in TEST): --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM [dbo].[SubTeam_Unaligned](NOLOCK)

SELECT @new = count(*)
FROM [dbo].[SubTeam](NOLOCK)

PRINT N'[dbo].[SubTeam_Unaligned] Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'[dbo].[SubTeam] Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count. --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count. --- [dbo].[SubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
