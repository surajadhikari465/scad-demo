/******************************************************************************
		SO [dbo].[ItemUnit]
		Change Steps
******************************************************************************/
PRINT N'Status: BEGIN FL [dbo].[ItemUnit] ChangeSteps (takes about 1 minute in TEST):'+ ' ---  [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 1, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[ItemUnit] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [ItemUnit] DROP CONSTRAINT [DF__ItemUnit__Weight__430CD787]
GO
ALTER TABLE [ItemUnit] DROP CONSTRAINT [DF_ItemUnit_IsPackageUnit]
GO
/******************************************************************************
		3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'Unit_Abbreviation'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'Unit_Abbreviation'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'User_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'User_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'Weight_Unit'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'Weight_Unit'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'Unit_Name'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'Unit_Name'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'Unit_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'Unit_ID'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnitAddUpdate]'))
DROP TRIGGER [dbo].[ItemUnitAddUpdate]
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[Chk_Constraint_ItemUnit_PlumUnitAbbr]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit] DROP CONSTRAINT [Chk_Constraint_ItemUnit_PlumUnitAbbr]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUnit_1__13]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit] DROP CONSTRAINT [FK_ItemUnit_1__13]
GO
/******************************************************************************
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[Chk_Constraint_ItemUnit_PlumUnitAbbr]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit] DROP CONSTRAINT [Chk_Constraint_ItemUnit_PlumUnitAbbr]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUnit_1__13]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit] DROP CONSTRAINT [FK_ItemUnit_1__13]
GO
/******************************************************************************
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Indexes in South (This Step is N/A)'
GO
/******************************************************************************
		7. Rename SO PK 
******************************************************************************/
PRINT N'Status: 7. Rename SO PK  --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit]') AND name = N'PK_ItemUnit_Unit_ID')
EXECUTE sp_rename N'[dbo].[PK_ItemUnit_Unit_ID]', N'PK_ItemUnit_Unit_ID_Unaligned';
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[ItemUnit]', N'ItemUnit_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemUnit](
	[Unit_ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Unit_Name] [varchar](25) NOT NULL,
	[Weight_Unit] [bit] NOT NULL CONSTRAINT [DF__ItemUnit__Weight__430CD787]  DEFAULT ((0)),
	[User_ID] [int] NULL,
	[Unit_Abbreviation] [varchar](5) NULL,
	[UnitSysCode] [varchar](5) NULL,
	[IsPackageUnit] [bit] NOT NULL CONSTRAINT [DF_ItemUnit_IsPackageUnit]  DEFAULT ((0)),
	[PlumUnitAbbr] [varchar](5) NULL,
	[EDISysCode] [char](2) NULL,
	[LastUpdateTimestamp] [datetime] NULL,
 CONSTRAINT [PK_ItemUnit_Unit_ID] PRIMARY KEY CLUSTERED 
(
	[Unit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/******************************************************************************
		10. Populate FL Table in Batches
******************************************************************************/
PRINT N'Status: 10. Populate FL Table (not enough rows to perform batch operations) --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[ItemUnit_Unaligned])
BEGIN
    SET IDENTITY_INSERT [dbo].[ItemUnit] ON;
    INSERT INTO [dbo].[ItemUnit] ([Unit_ID], [Unit_Name], [Weight_Unit], [User_ID], [Unit_Abbreviation], [UnitSysCode], [IsPackageUnit], [EDISysCode], [PlumUnitAbbr], [LastUpdateTimestamp])
    SELECT   [Unit_ID],
                [Unit_Name],
                [Weight_Unit],
                [User_ID],
                [Unit_Abbreviation],
                [UnitSysCode],
                [IsPackageUnit],
                [EDISysCode],
                [PlumUnitAbbr],
                [LastUpdateTimestamp]
    FROM     [dbo].[ItemUnit_Unaligned]
    ORDER BY [Unit_ID] ASC;
    SET IDENTITY_INSERT [dbo].[ItemUnit] OFF;
END
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[ItemUnit] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit]') AND name = N'idxItemUnitName')
CREATE UNIQUE NONCLUSTERED INDEX [idxItemUnitName] ON [dbo].[ItemUnit]
(
	[Unit_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit]') AND name = N'idxItemUnitUserID')
CREATE NONCLUSTERED INDEX [idxItemUnitUserID] ON [dbo].[ItemUnit]
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUnit_1__13]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit]  WITH CHECK ADD  CONSTRAINT [FK_ItemUnit_1__13] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUnit_1__13]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit] CHECK CONSTRAINT [FK_ItemUnit_1__13]
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[Chk_Constraint_ItemUnit_PlumUnitAbbr]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit]  WITH CHECK ADD  CONSTRAINT [Chk_Constraint_ItemUnit_PlumUnitAbbr] CHECK  (([PlumUnitAbbr]='KG' OR [PlumUnitAbbr]='HG' OR [PlumUnitAbbr]='BC' OR [PlumUnitAbbr]='LB' OR [PlumUnitAbbr]='HB' OR [PlumUnitAbbr]='QB' OR [PlumUnitAbbr]='FW' OR [PlumUnitAbbr]='FP' OR [PlumUnitAbbr]='OK' OR [PlumUnitAbbr]='OG' OR [PlumUnitAbbr]='OP' OR [PlumUnitAbbr]='OH' OR [PlumUnitAbbr]='OQ' OR [PlumUnitAbbr]='OB' OR [PlumUnitAbbr]='0' OR [PlumUnitAbbr]='1' OR [PlumUnitAbbr]='3'))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[Chk_Constraint_ItemUnit_PlumUnitAbbr]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit] CHECK CONSTRAINT [Chk_Constraint_ItemUnit_PlumUnitAbbr]
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnitAddUpdate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[ItemUnitAddUpdate] ON [dbo].[ItemUnit] 
FOR INSERT,UPDATE
AS
 BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
    update ItemUnit 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where ItemUnit.Unit_Id = i.Unit_id
    SELECT @Error_No = @@ERROR
  
 
    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR (''ItemUnitAddUpdate trigger failed with @@ERROR: %d'', @Severity, 1, @Error_No)
    END
END' 
GO
/******************************************************************************
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUnit] TO [BizTalk] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [ExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IConInterface] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUnit] TO [IConInterface] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUnit] TO [iCONReportingRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IMHARole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUnit] TO [IRMA_Teradata] AS [dbo]
GO
GRANT DELETE ON [dbo].[ItemUnit] TO [IRMAAdminRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[ItemUnit] TO [IRMAAdminRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IRMAAdminRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ItemUnit] TO [IRMAAdminRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUnit] TO [IRMAAdminRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IRMAPDXExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IRMAPromoRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUnit] TO [IRMAReports] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IRMAReportsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUnit] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUnit] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IRMASLIMRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IRMASupportRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemUnit] TO [IRMASupportRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [sobluesky] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [SODataControl] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [SOInventory] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemUnit] TO [TibcoDataWriter] AS [dbo]
GO
/******************************************************************************
		16. Check FL Checks (generated from VS schema compare)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[ItemUnit] WITH CHECK CHECK CONSTRAINT [Chk_Constraint_ItemUnit_PlumUnitAbbr]
GO
ALTER TABLE [dbo].[ItemUnit] WITH CHECK CHECK CONSTRAINT [FK_ItemUnit_1__13]
GO
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables (takes about 5 minutes in TEST): --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM [dbo].[ItemUnit_Unaligned](NOLOCK)

SELECT @new = count(*)
FROM [dbo].[ItemUnit](NOLOCK)

PRINT N'[dbo].[ItemUnit_Unaligned] Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'[dbo].[ItemUnit] Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count. --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count. --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
