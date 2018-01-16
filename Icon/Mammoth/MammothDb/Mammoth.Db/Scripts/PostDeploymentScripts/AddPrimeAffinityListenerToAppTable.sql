DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddPrimeAffinityListenerToAppTable'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	SET IDENTITY_INSERT app.App ON

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 20 AND AppName = 'Prime Affinity Listener')
		INSERT INTO app.App (AppID, AppName) VALUES (20, 'Prime Affinity Listener')

	SET IDENTITY_INSERT app.App OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO

