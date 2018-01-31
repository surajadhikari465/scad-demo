DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddActivePriceSentArchiveToRetentionPolicy'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	IF NOT EXISTS (SELECT * FROM app.RetentionPolicy WHERE [Table] = 'ActivePriceSentArchive')
		INSERT INTO app.RetentionPolicy([Schema], [Table], ReferenceColumn, DaysToKeep, TimeToStart, TimeToEnd, IncludedInDailyPurge, DailyPurgeCompleted, PurgeJobName)
		VALUES ('app', 'ActivePriceSentArchive', 'InsertDateUtc', 1, 21, 24, 1, 0, 'Data History Purge')

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO

