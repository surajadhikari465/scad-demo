declare @scriptKey varchar(128)

set @scriptKey = 'Re-PopulateRetentionPolicyTable'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

		TRUNCATE TABLE [app].[RetentionPolicy]

		INSERT INTO [app].[RetentionPolicy]([Schema], [Table], [ReferenceColumn], [DaysToKeep], [TimeToStart], [TimeToEnd], [IncludedInDailyPurge], [DailyPurgeCompleted], [PurgeJobName], [LastPurgedDateTime])
			VALUES 
			('app', 'AppLog', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueueItemLocale', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueuePrice', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueueItemLocaleArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('esb', 'MessageQueuePriceArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
			('gpm', 'MessageArchivePrice', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', NULL)

	   INSERT INTO app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO