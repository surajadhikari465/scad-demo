DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddJobSchedulerToAppTable'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	SET IDENTITY_INSERT app.App ON
	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 18 AND AppName = 'Job Scheduler')
		INSERT INTO app.App (AppID, AppName) VALUES (18, 'Job Scheduler')

	SET IDENTITY_INSERT app.App OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO

