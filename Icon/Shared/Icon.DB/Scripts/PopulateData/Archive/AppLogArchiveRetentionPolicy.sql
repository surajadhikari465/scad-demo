DECLARE @scriptKey VARCHAR(128) = 'AppLogArchiveToRetentionPolicy';

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
  PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

  IF(Not Exists(SELECT 1 FROM app.RetentionPolicy WHERE [Table] = 'AppLogArchive'))
    INSERT INTO app.RetentionPolicy ([Server], [Database], [Schema], [Table], DaysToKeep)
      VALUES (@@SERVERNAME, 'Icon', 'app', 'AppLogArchive', 365);
END
ELSE
BEGIN
  PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey;
END