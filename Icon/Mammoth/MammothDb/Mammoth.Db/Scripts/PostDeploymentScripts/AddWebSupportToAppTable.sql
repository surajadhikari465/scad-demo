DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddWebSupportToAppTable'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	SET IDENTITY_INSERT app.App ON
	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 15 AND AppName = 'Mammoth Web Support')
		INSERT INTO app.App (AppID, AppName) VALUES (15, 'Mammoth Web Support')
	SET IDENTITY_INSERT app.App OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO

