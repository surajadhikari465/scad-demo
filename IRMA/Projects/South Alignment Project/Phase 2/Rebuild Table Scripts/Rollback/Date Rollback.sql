/******************************************************************************
		SO [dbo].[Date]
		Rollback
******************************************************************************/
PRINT N'Status: Begin [dbo].[Date] ROLLBACK  --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[Date]  --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 1, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking. (This Step is N/A)'
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Defaults. (This Step is N/A)'
GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers. (This Step is N/A)'
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys. (This Step is N/A)'
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Date]') AND name = N'idxDateYearPeriod')
DROP INDEX [idxDateYearPeriod] ON [dbo].[Date]
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Primary Key. (This Step is N/A)'
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Primary Key. (This Step is N/A)'
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Date]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[Date]', N'Date_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Date_Unaligned]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[Date_Unaligned]', N'Date';
GO
/******************************************************************************
		10. Create SO Defaults (manually generated)
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Defaults. (This Step is N/A)'
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking. (This Step is N/A)'
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
GRANT SELECT ON [dbo].[Date] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Date] TO [IRMAPDXExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Date] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Date] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Date] TO [IRMASupportRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[Date] TO [IRSUser] AS [dbo]
GO
GRANT INSERT ON [dbo].[Date] TO [IRSUser] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[Date] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[Date] TO [IRSUser] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Date] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[Date] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[Date] TO [sobluesky] AS [dbo]
GO
/******************************************************************************
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Date]') AND name = N'idxDateYearPeriod')
CREATE NONCLUSTERED INDEX [idxDateYearPeriod] ON [dbo].[Date]
(
	[Year] ASC,
	[Period] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [Warehouse]
GO
/******************************************************************************
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys. (This Step is N/A)'
GO
/******************************************************************************
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers. (This Step is N/A)'
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Date_Key'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains the current date for the record' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Date_Key'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Year'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contrains the fiscal year' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Year'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Quarter'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Holds the fiscal period 1 to 4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Quarter'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Period'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains the period of the fiscal year' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Period'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Week'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains the week of the current month' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Week'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Day_Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains the actual day name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Day_Name'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Day_Of_Week'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains the day of the week 1 to 7r' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Day_Of_Week'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Day_Of_Month'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains the day within the current month 1 to 31' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Day_Of_Month'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Day_Of_Year'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains the day within the year 1 to 365' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Day_Of_Year'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains the fiscal date information' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: **** Operation Complete ****: --- [dbo].[Date] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
