﻿declare @scriptKey varchar(128)

set @scriptKey = 'Re-PopulateRetentionPolicyTable'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @serverName nvarchar(50);
	SET @serverName = @@SERVERNAME;

	-- DEV and TEST Instance
	IF @serverName = 'CEWD6587\MAMMOTH'
	BEGIN
		USE Mammoth_Dev

		-- DEV DB
		TRUNCATE TABLE [app].[RetentionPolicy]

		INSERT INTO [app].[RetentionPolicy]([Schema], [Table], [ReferenceColumn], [DaysToKeep], [TimeToStart], [TimeToEnd], [IncludedInDailyPurge], [DailyPurgeCompleted], [PurgeJobName], [LastPurgedDateTime])
			VALUES 
			('app', 'AppLog', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueueItemLocale', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueuePrice', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueueItemLocaleArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueuePriceArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('gpm', 'MessageArchivePrice', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', NULL)

		-- TEST DB
		USE Mammoth

		TRUNCATE TABLE [app].[RetentionPolicy]

			INSERT INTO [app].[RetentionPolicy]([Schema], [Table], [ReferenceColumn], [DaysToKeep], [TimeToStart], [TimeToEnd], [IncludedInDailyPurge], [DailyPurgeCompleted], [PurgeJobName], [LastPurgedDateTime])
			VALUES 
			('app', 'AppLog', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueueItemLocale', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueuePrice', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueueItemLocaleArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueuePriceArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('gpm', 'MessageArchivePrice', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', NULL)

	END

	-- QA Instance
	IF @serverName = 'QA-01-MAMMOTH\MAMMOTH'
	BEGIN
		USE Mammoth

		TRUNCATE TABLE [app].[RetentionPolicy]

		INSERT INTO [app].[RetentionPolicy]([Schema], [Table], [ReferenceColumn], [DaysToKeep], [TimeToStart], [TimeToEnd], [IncludedInDailyPurge], [DailyPurgeCompleted], [PurgeJobName], [LastPurgedDateTime])
		VALUES 
		('app', 'AppLog', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('esb', 'MessageQueueItemLocale', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('esb', 'MessageQueuePrice', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('esb', 'MessageQueueItemLocaleArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('esb', 'MessageQueuePriceArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('gpm', 'MessageArchivePrice', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', NULL)

	END

	-- PRD Instance
	IF @serverName = 'PRD-01-MAMMOTH\MAMMOTH'
	BEGIN
		USE Mammoth

		TRUNCATE TABLE [app].[RetentionPolicy]

		INSERT INTO [app].[RetentionPolicy]([Schema], [Table], [ReferenceColumn], [DaysToKeep], [TimeToStart], [TimeToEnd], [IncludedInDailyPurge], [DailyPurgeCompleted], [PurgeJobName], [LastPurgedDateTime])
		VALUES 
		('app', 'AppLog', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('esb', 'MessageQueueItemLocale', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('esb', 'MessageQueuePrice', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('esb', 'MessageQueueItemLocaleArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('esb', 'MessageQueuePriceArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
		('gpm', 'MessageArchivePrice', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', NULL)
	END

	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO