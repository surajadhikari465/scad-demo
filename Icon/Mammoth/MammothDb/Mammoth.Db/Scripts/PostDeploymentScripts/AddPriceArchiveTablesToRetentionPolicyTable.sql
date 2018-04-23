DECLARE @scriptKey varchar(128)

SET @scriptKey = 'AddPriceArchiveTablesToRetentionPolicyTable'

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @serverName nvarchar(50);
	SET @serverName = @@SERVERNAME;

		INSERT INTO [app].[RetentionPolicy]([Schema], [Table], [ReferenceColumn], [DaysToKeep], [TimeToStart], [TimeToEnd], [IncludedInDailyPurge], [DailyPurgeCompleted], [PurgeJobName], [LastPurgedDateTime])
			VALUES 
			('esb', 'PriceMessageArchiveDetail', 'InsertDateUtc', 10, 21, 24, 1, 0, 'Data History Purge', NULL)

	    INSERT INTO app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO