/******************************************************************************
		SO [dbo].[Date]
		Change Steps
******************************************************************************/
PRINT N'Status: BEGIN FL [dbo].[Date] ChangeSteps (takes about 1 minute in TEST):'+ ' ---  [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 1, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking. (This Step is N/A)'
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Defaults. (This Step is N/A)'
GO
/******************************************************************************
		3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Day_Of_Year'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Day_Of_Year'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Day_Of_Month'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Day_Of_Month'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Day_Of_Week'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Day_Of_Week'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Day_Name'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Day_Name'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Week'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Week'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Period'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Period'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Quarter'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Quarter'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Year'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Year'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Date', N'COLUMN',N'Date_Key'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Date', @level2type=N'COLUMN',@level2name=N'Date_Key'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers. (This Step is N/A)'
GO
/******************************************************************************
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys. (This Step is N/A)'
GO
/******************************************************************************
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Date]') AND name = N'idxDateYearPeriod')
DROP INDEX [idxDateYearPeriod] ON [dbo].[Date]
GO
/******************************************************************************
		7. Rename SO PK 
******************************************************************************/
PRINT N'Status: 7. Rename SO PK  --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Date]') AND name = N'PK_Date_Date_Key')
EXECUTE sp_rename N'[dbo].[PK_Date_Date_Key]', N'PK_Date_Date_Key_Unaligned';
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Date]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[Date]', N'Date_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Date]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Date](
	[Date_Key] [smalldatetime] NOT NULL,
	[Year] [smallint] NULL,
	[Quarter] [smallint] NULL,
	[Period] [tinyint] NULL,
	[Week] [tinyint] NULL,
	[Day_Name] [char](9) NULL,
	[Day_Of_Week] [tinyint] NULL,
	[Day_Of_Month] [tinyint] NULL,
	[Day_Of_Year] [smallint] NULL,
 CONSTRAINT [PK_Date_Date_Key] PRIMARY KEY CLUSTERED 
(
	[Date_Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [Warehouse]
) ON [Warehouse]
END
GO
/******************************************************************************
		10. Populate FL Table in Batches
******************************************************************************/
PRINT N'Status: 10. Populate FL Table (not enough rows to do this in batches) --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].Date_Unaligned)
    BEGIN
        INSERT INTO [dbo].[Date] ([Date_Key], [Year], [Quarter], [Period], [Week], [Day_Name], [Day_Of_Week], [Day_Of_Month], [Day_Of_Year])
        SELECT   [Date_Key],
                 [Year],
                 [Quarter],
                 [Period],
                 [Week],
                 [Day_Name],
                 [Day_Of_Week],
                 [Day_Of_Month],
                 [Day_Of_Year]
        FROM     [dbo].[Date_Unaligned]
        ORDER BY [Date_Key] ASC;
    END
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking. (This Step is N/A)'
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Date]') AND name = N'idxDateYearPeriod')
CREATE NONCLUSTERED INDEX [idxDateYearPeriod] ON [dbo].[Date]
(
	[Year] ASC,
	[Period] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [Warehouse]
GO
/******************************************************************************
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys. (This Step is N/A)'
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers. (This Step is N/A)'
GO
/******************************************************************************
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		16. Check FL Checks (generated from VS schema compare)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Defaults. (This Step is N/A)'
GO
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables (takes about 5 minutes in TEST): --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM [dbo].[Date_Unaligned](NOLOCK)

SELECT @new = count(*)
FROM [dbo].[Date](NOLOCK)

PRINT N'[dbo].[Date_Unaligned] Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'[dbo].[Date] Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count. --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count. --- [dbo].[Date] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
