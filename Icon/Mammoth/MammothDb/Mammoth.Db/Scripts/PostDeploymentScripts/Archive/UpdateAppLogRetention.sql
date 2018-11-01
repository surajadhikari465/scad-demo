DECLARE @scriptKey varchar(128);
SET @scriptKey = 'UpdateAppLogRetention'

IF(NOT EXISTS(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey
	
	UPDATE app.RetentionPolicy
	SET DaysToKeep = 7
	WHERE [Schema] = 'app'
		AND [Table] = 'AppLog'

	INSERT INTO app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
