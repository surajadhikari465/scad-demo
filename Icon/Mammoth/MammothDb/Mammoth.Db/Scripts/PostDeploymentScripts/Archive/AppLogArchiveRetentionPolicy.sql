DECLARE @scriptKey VARCHAR(128) = 'AppLogArchiveToRetentionPolicy';

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
  PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

  IF(Not Exists(SELECT 1 FROM app.RetentionPolicy WHERE [Table] = 'AppLogArchive'))
    INSERT INTO app.RetentionPolicy([Schema], [Table], ReferenceColumn, DaysToKeep, TimeToStart, TimeToEnd, IncludedInDailyPurge, DailyPurgeCompleted, PurgeJobName)
      VALUES('app', 'AppLogArchive', 'InsertDate', 365, 21, 24, 1, 0, 'Data History Purge');
END
ELSE
BEGIN
  PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey;
END