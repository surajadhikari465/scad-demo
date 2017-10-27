declare @scriptKey varchar(128)

set @scriptKey = 'Re-PopulateRetentionPolicyForStagingTables'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 
	INSERT INTO [app].[RetentionPolicy]([Schema], [Table], [ReferenceColumn], [DaysToKeep], [TimeToStart], [TimeToEnd], [IncludedInDailyPurge], [DailyPurgeCompleted], [PurgeJobName], [LastPurgedDateTime])
	VALUES 
	('app', 'AppLog', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
	('esb', 'MessageQueueItemLocale', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
	('esb', 'MessageQueuePrice', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
	('esb', 'MessageQueueItemLocaleArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
	('esb', 'MessageQueuePriceArchive', 'InsertDate', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
	('gpm', 'MessageArchivePrice', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
	('esb', 'MessageArchive', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', NULL),
	('gpm', 'MessageArchivePriceR10', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', NULL)
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO