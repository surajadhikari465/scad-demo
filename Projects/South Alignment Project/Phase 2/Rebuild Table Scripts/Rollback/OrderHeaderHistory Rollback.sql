/******************************************************************************
		SO [dbo].[OrderHeaderHistory]
		Rollback
******************************************************************************/
PRINT N'Status: Begin [dbo].[OrderHeaderHistory] ROLLBACK  --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[OrderHeaderHistory]  --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 30, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [OrderHeaderHistory] DROP CONSTRAINT [DF_OrderHeaderHistory_InsertDate]
GO
ALTER TABLE [OrderHeaderHistory] DROP CONSTRAINT [DF_OrderHeaderHistory_FromQueue]
GO
ALTER TABLE [OrderHeaderHistory] DROP CONSTRAINT [DF_OrderHeaderHistory_PayByAgreedCost]
GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderHistory]') AND name = N'idxOrderHeaderHistoryOrderHeader_ID')
DROP INDEX [idxOrderHeaderHistoryOrderHeader_ID] ON [dbo].[OrderHeaderHistory]
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Primary Key on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Primary Key on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[OrderHeaderHistory]', N'OrderHeaderHistory_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderHistory_Unaligned]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[OrderHeaderHistory_Unaligned]', N'OrderHeaderHistory';
GO
/******************************************************************************
		10. Create SO Defaults (manually generated)
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [OrderHeaderHistory] WITH NOCHECK ADD CONSTRAINT [DF_OrderHeaderHistory_InsertDate] DEFAULT (getdate()) FOR [InsertDate]
GO
ALTER TABLE [OrderHeaderHistory] WITH NOCHECK ADD CONSTRAINT [DF_OrderHeaderHistory_FromQueue] DEFAULT (0) FOR [FromQueue]
GO
ALTER TABLE [OrderHeaderHistory] WITH NOCHECK ADD CONSTRAINT [DF_OrderHeaderHistory_AccrualException] DEFAULT ((0)) FOR [AccrualException]
GO
ALTER TABLE [OrderHeaderHistory] WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__PayBy__240BF313] DEFAULT ((0)) FOR [PayByAgreedCost]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [sobluesky] AS [dbo]
GO
/******************************************************************************
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderHistory]') AND name = N'idxClustered_OrderDate')
CREATE CLUSTERED INDEX [idxClustered_OrderDate] ON [dbo].[OrderHeaderHistory]
(
	[OrderDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderHistory]') AND name = N'idxOrderHeaderHistoryOrderHeader_ID')
CREATE NONCLUSTERED INDEX [idxOrderHeaderHistoryOrderHeader_ID] ON [dbo].[OrderHeaderHistory]
(
	[OrderHeader_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
/******************************************************************************
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Extended Properties on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: 17. Finish Up --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: **** Operation Complete ****: --- [dbo].[OrderHeaderHistory] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
