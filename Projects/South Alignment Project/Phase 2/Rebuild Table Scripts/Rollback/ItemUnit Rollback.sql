/******************************************************************************
		SO [dbo].[ItemUnit]
		Rollback
******************************************************************************/
PRINT N'Status: Begin [dbo].[ItemUnit] ROLLBACK  --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[ItemUnit]  --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 1, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[ItemUnit] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [ItemUnit] DROP CONSTRAINT [DF__ItemUnit__Weight__430CD787]
GO
ALTER TABLE [ItemUnit] DROP CONSTRAINT [DF_ItemUnit_IsPackageUnit]
GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnitAddUpdate]'))
DROP TRIGGER [dbo].[ItemUnitAddUpdate]
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[Chk_Constraint_ItemUnit_PlumUnitAbbr]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit] DROP CONSTRAINT [Chk_Constraint_ItemUnit_PlumUnitAbbr]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUnit_1__13]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit] DROP CONSTRAINT [FK_ItemUnit_1__13]
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit]') AND name = N'idxItemUnitUserID')
DROP INDEX [idxItemUnitUserID] ON [dbo].[ItemUnit]
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit]') AND name = N'PK_ItemUnit_Unit_ID')
EXECUTE sp_rename N'[dbo].[PK_ItemUnit_Unit_ID]', N'PK_ItemUnit_Unit_ID_Rollback';
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit_Unaligned') AND name = N'PK_ItemUnit_Unit_ID_Unaligned')
EXECUTE sp_rename N'[dbo].[PK_ItemUnit_Unit_ID_Unaligned]', N'PK_ItemUnit_Unit_ID';
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[ItemUnit]', N'ItemUnit_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemUnit_Unaligned]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[ItemUnit_Unaligned]', N'ItemUnit';
GO
/******************************************************************************
		10. Create SO Defaults (manually generated)
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [ItemUnit] WITH NOCHECK ADD CONSTRAINT [DF__ItemUnit__Weight__430CD787] DEFAULT (0) FOR [Weight_Unit]
GO
ALTER TABLE [ItemUnit] WITH NOCHECK ADD CONSTRAINT [DF_ItemUnit_IsPackageUnit] DEFAULT (0) FOR [IsPackageUnit]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[ItemUnit] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Indexes in South (This Step is N/A)'
GO
/******************************************************************************
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUnit_1__13]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUnit]'))
ALTER TABLE [dbo].[ItemUnit]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemUnit_1__13] FOREIGN KEY([User_ID])
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
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'Unit_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unit id ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'Unit_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'Unit_Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unit name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'Unit_Name'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'Weight_Unit'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Weight or unit 1 = Yes or 0 = No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'Weight_Unit'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'User_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User id refer to user table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'User_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemUnit', N'COLUMN',N'Unit_Abbreviation'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unit abbreviation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemUnit', @level2type=N'COLUMN',@level2name=N'Unit_Abbreviation'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: 17. Finish Up --- [dbo].[ItemUnit] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: **** Operation Complete ****: --- [dbo].[ItemUnit] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
