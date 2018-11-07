/******************************************************************************
		SO [dbo].[StoreSubTeam]
		Change Steps
******************************************************************************/
PRINT N'Status: BEGIN FL [dbo].[StoreSubTeam] ChangeSteps (takes about 1 minute in TEST):'+ ' ---  [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 1, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[StoreSubTeam] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [StoreSubTeam] DROP CONSTRAINT [DF_StoreSubTeam_CasePriceDiscount]
GO
ALTER TABLE [StoreSubTeam] DROP CONSTRAINT [DF__StoreSubT__CostF__5DE755D6]
GO
/******************************************************************************
		3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Extended Properties in SO.  (This Step is N/A)'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'There are no SO Triggers. (This Step is N/A.)'
GO
/******************************************************************************
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]') AND name = N'idxStoreSubTeam_GetPriceBatchSent')
DROP INDEX [idxStoreSubTeam_GetPriceBatchSent] ON [dbo].[StoreSubTeam]
GO
/******************************************************************************
		7. Rename SO PK 
******************************************************************************/
PRINT N'Status: 7. Rename SO PK  --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]') AND name = N'PK_StoreSubTeam')
EXECUTE sp_rename N'[dbo].[PK_StoreSubTeam]', N'PK_StoreSubTeam_Unaligned';
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[StoreSubTeam]', N'StoreSubTeam_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StoreSubTeam](
	[Store_No] [int] NOT NULL,
	[Team_No] [int] NOT NULL,
	[SubTeam_No] [int] NOT NULL,
	[CasePriceDiscount] [decimal](9, 4) NOT NULL CONSTRAINT [DF_StoreSubTeam_CasePriceDiscount]  DEFAULT ((0)),
	[CostFactor] [decimal](9, 4) NOT NULL CONSTRAINT [DF_StoreSubTeam_CostFactor]  DEFAULT ((0)),
	[ICVID] [int] NULL,
	[PS_Team_No] [int] NULL,
	[PS_SubTeam_No] [int] NULL,
 CONSTRAINT [PK_StoreSubTeam] PRIMARY KEY CLUSTERED 
(
	[Store_No] ASC,
	[SubTeam_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/******************************************************************************
		10. Populate FL Table in Batches
******************************************************************************/
PRINT N'Status: 10. Populate FL Table in Batches --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[StoreSubTeam_Unaligned])
    BEGIN
        INSERT INTO [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No], [Team_No], [CasePriceDiscount], [ICVID], [CostFactor], [PS_Team_No], [PS_SubTeam_No])
        SELECT   src.[Store_No],
                 src.[SubTeam_No],
                 src.[Team_No],
                 src.[CasePriceDiscount],
                 src.[ICVID],
                 src.[CostFactor],
                 src.[PS_Team_No],
                 src.[PS_SubTeam_No]
        FROM     [dbo].[StoreSubTeam_Unaligned] src
        ORDER BY [Store_No] ASC, [SubTeam_No] ASC;
    END
GO
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[StoreSubTeam] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Indexes in FL.  This Step is N/A'
GO
/******************************************************************************
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_CycleCountVendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_StoreSubTeam_CycleCountVendor] FOREIGN KEY([ICVID])
REFERENCES [dbo].[CycleCountVendor] ([ICVID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_CycleCountVendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] CHECK CONSTRAINT [FK_StoreSubTeam_CycleCountVendor]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Store]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_StoreSubTeam_Store] FOREIGN KEY([Store_No])
REFERENCES [dbo].[Store] ([Store_No])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Store]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] CHECK CONSTRAINT [FK_StoreSubTeam_Store]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_StoreSubTeam_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] CHECK CONSTRAINT [FK_StoreSubTeam_SubTeam]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Team]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_StoreSubTeam_Team] FOREIGN KEY([Team_No])
REFERENCES [dbo].[Team] ([Team_No])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_Team]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] CHECK CONSTRAINT [FK_StoreSubTeam_Team]
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'There are no FL Triggers. (This Step is N/A.)'
GO
/******************************************************************************
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		16. Check FL Checks (generated from VS schema compare)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[StoreSubTeam] WITH CHECK CHECK CONSTRAINT [FK_StoreSubTeam_Store]
GO
ALTER TABLE [dbo].[StoreSubTeam] WITH CHECK CHECK CONSTRAINT [FK_StoreSubTeam_SubTeam]
GO
ALTER TABLE [dbo].[StoreSubTeam] WITH CHECK CHECK CONSTRAINT [FK_StoreSubTeam_Team]
GO
ALTER TABLE [dbo].[StoreSubTeam] WITH CHECK CHECK CONSTRAINT [FK_StoreSubTeam_CycleCountVendor]
GO
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables (takes about 5 minutes in TEST): --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM [dbo].[StoreSubTeam_Unaligned](NOLOCK)

SELECT @new = count(*)
FROM [dbo].[StoreSubTeam](NOLOCK)

PRINT N'[dbo].[StoreSubTeam_Unaligned] Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'[dbo].[StoreSubTeam] Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count. --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count. --- [dbo].[StoreSubTeam] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
