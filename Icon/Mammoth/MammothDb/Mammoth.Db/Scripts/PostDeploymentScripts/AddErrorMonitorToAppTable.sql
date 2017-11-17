DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddErrorMonitorToAppTable'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	SET IDENTITY_INSERT app.App ON
	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 16 AND AppName = 'Error Message Listener')
		INSERT INTO app.App (AppID, AppName) VALUES (16, 'Error Message Listener')

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 17 AND AppName = 'Error Message Monitor')
		INSERT INTO app.App (AppID, AppName) VALUES (17, 'Error Message Monitor')

	SET IDENTITY_INSERT app.App OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO

