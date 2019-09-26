declare @scriptKey varchar(128)

set @scriptKey = 'PopulateRetentionPolicyForStagingESLePlumTables'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 
	INSERT INTO [app].[RetentionPolicy]([Schema], [Table], [ReferenceColumn], [DaysToKeep], [TimeToStart], [TimeToEnd], [IncludedInDailyPurge], [DailyPurgeCompleted], [PurgeJobName], [LastPurgedDateTime])
	VALUES
    ('stage', 'ItemStoreKeysEPlum', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', null),
	('stage', 'ItemStoreKeysEsl', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', null)
 
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO