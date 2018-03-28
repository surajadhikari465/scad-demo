/******************************************************************************
		SO - Point FKs to Aligned Tables
		Change Steps
		Run this Script AFTER running any South Alignment Scripts 
		It will point all the FKs away from unaligned tables
		-- Rollback is generated from Output -- 
		-- Please Set Output to TEXT -- 
******************************************************************************/
PRINT N'/******************************************************************************' 
PRINT N'-- This output is generated to be run as part of the rollback process.'
PRINT N'Status: SO - Point FKs to Aligned Tables (takes about 15 minutes in TEST): --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 15, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Manually Handle FKs with Mulitple Columns		 
		(Store_No, Subteam_No)
		I kind of took a shortcut here because the engine that finds all 
		the FKs and Tables didn't have a super elegant way of handling
		multiple column assignments.
		So I'm just copying and pasting the drop/add in here 
		for those 3 tables that look at StoreSubTeam_Unaligned	
******************************************************************************/
PRINT N'Status: 1. Manually Handle FKs with Mulitple Columns --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'******************************************************************************/'
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreSubTeam_Unaligned]') AND type in (N'U'))
BEGIN
	DECLARE @sql NVARCHAR(MAX)
	DECLARE @count NVARCHAR(5)
	BEGIN
		SET @sql = '
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''[dbo].[FK_OrderItemQueue_StoreSubTeam]'') AND parent_object_id = OBJECT_ID(N''[dbo].[OrderItemQueue]''))
ALTER TABLE [dbo].[OrderItemQueue] DROP CONSTRAINT [FK_OrderItemQueue_StoreSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''[dbo].[FK_OrderItemQueue_StoreSubTeam]'') AND parent_object_id = OBJECT_ID(N''[dbo].[OrderItemQueue]''))
ALTER TABLE [dbo].[OrderItemQueue]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueue_StoreSubTeam] FOREIGN KEY([Store_No], [TransferToSubTeam_No])
REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No])'
		SET @count = (
			SELECT COUNT(DISTINCT CHECKSUM(O2.NAME, F.OBJECT_ID))
			FROM SYS.ALL_OBJECTS O1,
					SYS.ALL_OBJECTS O2,
					SYS.ALL_COLUMNS C1,
					SYS.ALL_COLUMNS C2,
					SYS.FOREIGN_KEYS F
				INNER JOIN SYS.FOREIGN_KEY_COLUMNS K ON (K.CONSTRAINT_OBJECT_ID = F.OBJECT_ID)
				INNER JOIN SYS.INDEXES I ON (
						F.REFERENCED_OBJECT_ID = I.OBJECT_ID
						AND F.KEY_INDEX_ID = I.INDEX_ID
						)
				WHERE O1.OBJECT_ID = F.REFERENCED_OBJECT_ID
					AND O2.OBJECT_ID = F.PARENT_OBJECT_ID
					AND C1.OBJECT_ID = F.REFERENCED_OBJECT_ID
					AND C2.OBJECT_ID = F.PARENT_OBJECT_ID
					AND C1.COLUMN_ID = K.REFERENCED_COLUMN_ID
					AND C2.COLUMN_ID = K.PARENT_COLUMN_ID
					AND CONVERT(SYSNAME, O1.NAME) LIKE '%_Unaligned'
					--AND CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID)) NOT IN ('FK_OrderItemQueue_StoreSubTeam','FK_OrderItemQueueBak_StoreSubTeam','FK_ScanGunStoreSubTeam_StoreSubTeam')  -- Multiple Cols on FK, See manual creation file
			)
		PRINT N'/******************************************************************************' 
		PRINT N'		Altering Table #' + @count + ' --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
		PRINT N'******************************************************************************/'
		PRINT @sql
		PRINT CHAR(10) + 'GO'
		EXEC sp_executesql @sql
	END
	BEGIN
		SET @sql = '
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''[dbo].[FK_OrderItemQueueBak_StoreSubTeam]'') AND parent_object_id = OBJECT_ID(N''[dbo].[OrderItemQueueBak]''))
ALTER TABLE [dbo].[OrderItemQueueBak] DROP CONSTRAINT [FK_OrderItemQueueBak_StoreSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''[dbo].[FK_OrderItemQueueBak_StoreSubTeam]'') AND parent_object_id = OBJECT_ID(N''[dbo].[OrderItemQueueBak]''))
ALTER TABLE [dbo].[OrderItemQueueBak]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueueBak_StoreSubTeam] FOREIGN KEY([Store_No], [TransferToSubTeam_No])
REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No])'	
		SET @count = (
			SELECT COUNT(DISTINCT CHECKSUM(O2.NAME, F.OBJECT_ID))
			FROM SYS.ALL_OBJECTS O1,
					SYS.ALL_OBJECTS O2,
					SYS.ALL_COLUMNS C1,
					SYS.ALL_COLUMNS C2,
					SYS.FOREIGN_KEYS F
				INNER JOIN SYS.FOREIGN_KEY_COLUMNS K ON (K.CONSTRAINT_OBJECT_ID = F.OBJECT_ID)
				INNER JOIN SYS.INDEXES I ON (
						F.REFERENCED_OBJECT_ID = I.OBJECT_ID
						AND F.KEY_INDEX_ID = I.INDEX_ID
						)
				WHERE O1.OBJECT_ID = F.REFERENCED_OBJECT_ID
					AND O2.OBJECT_ID = F.PARENT_OBJECT_ID
					AND C1.OBJECT_ID = F.REFERENCED_OBJECT_ID
					AND C2.OBJECT_ID = F.PARENT_OBJECT_ID
					AND C1.COLUMN_ID = K.REFERENCED_COLUMN_ID
					AND C2.COLUMN_ID = K.PARENT_COLUMN_ID
					AND CONVERT(SYSNAME, O1.NAME) LIKE '%_Unaligned'
					--AND CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID)) NOT IN ('FK_OrderItemQueue_StoreSubTeam','FK_OrderItemQueueBak_StoreSubTeam','FK_ScanGunStoreSubTeam_StoreSubTeam')  -- Multiple Cols on FK, See manual creation file
			)
		PRINT N'/******************************************************************************' 
		PRINT N'		Altering Table #' + @count + ' --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
		PRINT N'******************************************************************************/'
		PRINT @sql
		PRINT CHAR(10) + 'GO'
		EXEC sp_executesql @sql
	END
	BEGIN
		SET @sql = '
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''[dbo].[FK_ScanGunStoreSubTeam_StoreSubTeam]'') AND parent_object_id = OBJECT_ID(N''[dbo].[ScanGunStoreSubTeam]''))
ALTER TABLE [dbo].[ScanGunStoreSubTeam] DROP CONSTRAINT [FK_ScanGunStoreSubTeam_StoreSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''[dbo].[FK_ScanGunStoreSubTeam_StoreSubTeam]'') AND parent_object_id = OBJECT_ID(N''[dbo].[ScanGunStoreSubTeam]''))
ALTER TABLE [dbo].[ScanGunStoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_ScanGunStoreSubTeam_StoreSubTeam] FOREIGN KEY([Store_No], [SubTeam_No])
REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No])'
		SET @count = (
			SELECT COUNT(DISTINCT CHECKSUM(O2.NAME, F.OBJECT_ID))
			FROM SYS.ALL_OBJECTS O1,
					SYS.ALL_OBJECTS O2,
					SYS.ALL_COLUMNS C1,
					SYS.ALL_COLUMNS C2,
					SYS.FOREIGN_KEYS F
				INNER JOIN SYS.FOREIGN_KEY_COLUMNS K ON (K.CONSTRAINT_OBJECT_ID = F.OBJECT_ID)
				INNER JOIN SYS.INDEXES I ON (
						F.REFERENCED_OBJECT_ID = I.OBJECT_ID
						AND F.KEY_INDEX_ID = I.INDEX_ID
						)
				WHERE O1.OBJECT_ID = F.REFERENCED_OBJECT_ID
					AND O2.OBJECT_ID = F.PARENT_OBJECT_ID
					AND C1.OBJECT_ID = F.REFERENCED_OBJECT_ID
					AND C2.OBJECT_ID = F.PARENT_OBJECT_ID
					AND C1.COLUMN_ID = K.REFERENCED_COLUMN_ID
					AND C2.COLUMN_ID = K.PARENT_COLUMN_ID
					AND CONVERT(SYSNAME, O1.NAME) LIKE '%_Unaligned'
					--AND CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID)) NOT IN ('FK_OrderItemQueue_StoreSubTeam','FK_OrderItemQueueBak_StoreSubTeam','FK_ScanGunStoreSubTeam_StoreSubTeam')  -- Multiple Cols on FK, See manual creation file
			)
		PRINT N'/******************************************************************************' 
		PRINT N'		Altering Table #' + @count + ' --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
		PRINT N'******************************************************************************/'
		PRINT @sql
		PRINT CHAR(10) + 'GO'
		EXEC sp_executesql @sql
	END
END
GO
/******************************************************************************
		2. Dynamically Generate SQL to Point to Aligned Tables
******************************************************************************/
PRINT N'/******************************************************************************' 
GO
PRINT N'Status: 2. Dynamically Generate SQL to Point to Aligned Tables --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for this Step to Complete: --- [dbo].[OrderItem] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 46, SYSDATETIME()), 9)
GO
PRINT N'******************************************************************************/'
GO
DECLARE @if NVARCHAR(MAX) = 'IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'''
DECLARE @and NVARCHAR(MAX) = ''') AND parent_object_id = OBJECT_ID(N'''
DECLARE @alter NVARCHAR(MAX) = '''))' + CHAR(10) + 'ALTER TABLE '
DECLARE @drop NVARCHAR(MAX) = ' DROP CONSTRAINT '
DECLARE @if_not NVARCHAR(MAX) = 'IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'''
DECLARE @with_check NVARCHAR(MAX) = '  WITH CHECK ADD  CONSTRAINT '
DECLARE @with_nocheck NVARCHAR(MAX) = '  WITH NOCHECK ADD  CONSTRAINT '
DECLARE @foreign_key NVARCHAR(MAX) = ' FOREIGN KEY('
DECLARE @references NVARCHAR(MAX) = ')' + CHAR(10) + 'REFERENCES '
DECLARE @dot NVARCHAR(1) = '.'
DECLARE @go NVARCHAR(10) = CHAR(10) --+ 'GO' + CHAR(10) + CHAR(10)
DECLARE @open_paren NVARCHAR(10) = ' ('
DECLARE @close_paren NVARCHAR(10) = ')'

WHILE EXISTS (
	SELECT 1
	FROM SYS.ALL_OBJECTS O1,
		SYS.ALL_OBJECTS O2,
		SYS.ALL_COLUMNS C1,
		SYS.ALL_COLUMNS C2,
		SYS.FOREIGN_KEYS F
	INNER JOIN SYS.FOREIGN_KEY_COLUMNS K ON (K.CONSTRAINT_OBJECT_ID = F.OBJECT_ID)
	INNER JOIN SYS.INDEXES I ON (
			F.REFERENCED_OBJECT_ID = I.OBJECT_ID
			AND F.KEY_INDEX_ID = I.INDEX_ID
			)
	WHERE O1.OBJECT_ID = F.REFERENCED_OBJECT_ID
		AND O2.OBJECT_ID = F.PARENT_OBJECT_ID
		AND C1.OBJECT_ID = F.REFERENCED_OBJECT_ID
		AND C2.OBJECT_ID = F.PARENT_OBJECT_ID
		AND C1.COLUMN_ID = K.REFERENCED_COLUMN_ID
		AND C2.COLUMN_ID = K.PARENT_COLUMN_ID
		AND CONVERT(SYSNAME, O1.NAME) LIKE '%_Unaligned'
		--AND CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID)) NOT IN ('FK_OrderItemQueue_StoreSubTeam','FK_OrderItemQueueBak_StoreSubTeam','FK_ScanGunStoreSubTeam_StoreSubTeam')
	)
BEGIN
	DECLARE @sql NVARCHAR(MAX) = (
	SELECT TOP 1 
			@if + QUOTENAME(CONVERT(SYSNAME, SCHEMA_NAME(O2.SCHEMA_ID))) + @dot + QUOTENAME(CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID))) + 
			@and + QUOTENAME(CONVERT(SYSNAME, SCHEMA_NAME(O2.SCHEMA_ID))) + @dot + QUOTENAME(CONVERT(SYSNAME, O2.NAME)) + 
			@alter + QUOTENAME(CONVERT(SYSNAME, SCHEMA_NAME(O2.SCHEMA_ID))) + @dot + QUOTENAME(CONVERT(SYSNAME, O2.NAME)) + @drop + QUOTENAME(CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID))) + 
			@go + 
			@if_not + QUOTENAME(CONVERT(SYSNAME, SCHEMA_NAME(O2.SCHEMA_ID))) + @dot + QUOTENAME(CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID))) + 
			@and + QUOTENAME(CONVERT(SYSNAME, SCHEMA_NAME(O2.SCHEMA_ID))) + @dot + QUOTENAME(CONVERT(SYSNAME, O2.NAME)) + 
			@alter + QUOTENAME(CONVERT(SYSNAME, SCHEMA_NAME(O2.SCHEMA_ID))) + @dot + QUOTENAME(CONVERT(SYSNAME, O2.NAME)) + 
			(CASE WHEN f.is_not_trusted = 1 THEN @with_nocheck ELSE @with_check END) + QUOTENAME(CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID))) + 
			@foreign_key + QUOTENAME(CONVERT(SYSNAME, C2.NAME)) +
		--@references + QUOTENAME(CONVERT(SYSNAME, SCHEMA_NAME(O1.SCHEMA_ID))) + @dot + QUOTENAME(CONVERT(SYSNAME, O1.NAME)) + @open_paren + QUOTENAME(CONVERT(SYSNAME, C1.NAME)) + @close_paren +
			@references + QUOTENAME(CONVERT(SYSNAME, SCHEMA_NAME(O1.SCHEMA_ID))) + @dot + REPLACE(QUOTENAME(CONVERT(SYSNAME, O1.NAME)), '_Unaligned', '') + @open_paren + QUOTENAME(CONVERT(SYSNAME, C1.NAME)) + @close_paren + 
			@go
	FROM SYS.ALL_OBJECTS O1,
		SYS.ALL_OBJECTS O2,
		SYS.ALL_COLUMNS C1,
		SYS.ALL_COLUMNS C2,
		SYS.FOREIGN_KEYS F
	INNER JOIN SYS.FOREIGN_KEY_COLUMNS K ON (K.CONSTRAINT_OBJECT_ID = F.OBJECT_ID)
	INNER JOIN SYS.INDEXES I ON (
			F.REFERENCED_OBJECT_ID = I.OBJECT_ID
			AND F.KEY_INDEX_ID = I.INDEX_ID
			)
	WHERE O1.OBJECT_ID = F.REFERENCED_OBJECT_ID
		AND O2.OBJECT_ID = F.PARENT_OBJECT_ID
		AND C1.OBJECT_ID = F.REFERENCED_OBJECT_ID
		AND C2.OBJECT_ID = F.PARENT_OBJECT_ID
		AND C1.COLUMN_ID = K.REFERENCED_COLUMN_ID
		AND C2.COLUMN_ID = K.PARENT_COLUMN_ID
		AND CONVERT(SYSNAME, O1.NAME) LIKE '%_Unaligned'
		--AND CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID)) NOT IN ('FK_OrderItemQueue_StoreSubTeam','FK_OrderItemQueueBak_StoreSubTeam','FK_ScanGunStoreSubTeam_StoreSubTeam')  -- Multiple Cols on FK, See manual creation file
	)
	DECLARE @count NVARCHAR(5) = (
		SELECT COUNT(DISTINCT CHECKSUM(O2.NAME, F.OBJECT_ID))
		FROM SYS.ALL_OBJECTS O1,
				SYS.ALL_OBJECTS O2,
				SYS.ALL_COLUMNS C1,
				SYS.ALL_COLUMNS C2,
				SYS.FOREIGN_KEYS F
			INNER JOIN SYS.FOREIGN_KEY_COLUMNS K ON (K.CONSTRAINT_OBJECT_ID = F.OBJECT_ID)
			INNER JOIN SYS.INDEXES I ON (
					F.REFERENCED_OBJECT_ID = I.OBJECT_ID
					AND F.KEY_INDEX_ID = I.INDEX_ID
					)
			WHERE O1.OBJECT_ID = F.REFERENCED_OBJECT_ID
				AND O2.OBJECT_ID = F.PARENT_OBJECT_ID
				AND C1.OBJECT_ID = F.REFERENCED_OBJECT_ID
				AND C2.OBJECT_ID = F.PARENT_OBJECT_ID
				AND C1.COLUMN_ID = K.REFERENCED_COLUMN_ID
				AND C2.COLUMN_ID = K.PARENT_COLUMN_ID
				AND CONVERT(SYSNAME, O1.NAME) LIKE '%_Unaligned'
				AND CONVERT(SYSNAME, OBJECT_NAME(F.OBJECT_ID)) NOT IN ('FK_OrderItemQueue_StoreSubTeam','FK_OrderItemQueueBak_StoreSubTeam','FK_ScanGunStoreSubTeam_StoreSubTeam')  -- Multiple Cols on FK, See manual creation file
		)
	PRINT N'/******************************************************************************' 
	PRINT N'		Altering Table #' + @count + ' --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
	PRINT N'******************************************************************************/'
	PRINT @sql
	PRINT CHAR(10) + 'GO'
	EXEC sp_executesql @sql
END
GO 
