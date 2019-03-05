DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddEslOnePlumAppsToAppTable'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	SET IDENTITY_INSERT app.App ON
	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 23 AND AppName = 'OnePlum Listener')
		INSERT INTO app.App (AppID, AppName) VALUES (23, 'OnePlum Listener')

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 24 AND AppName = 'Esl Listener')
		INSERT INTO app.App (AppID, AppName) VALUES (24, 'Esl Listener')

	SET IDENTITY_INSERT app.App OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO

