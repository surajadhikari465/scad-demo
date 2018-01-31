DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddAppsToAppTable'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	SET IDENTITY_INSERT app.App ON
	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 9 AND AppName = 'Price Listener')
		INSERT INTO app.App (AppID, AppName) VALUES (9, 'Price Listener')

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 10 AND AppName = 'Price Message Archiver')
		INSERT INTO app.App (AppID, AppName) VALUES (10, 'Price Message Archiver')

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 11 AND AppName = 'Active Price Service')
		INSERT INTO app.App (AppID, AppName) VALUES (11, 'Active Price Service')

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 12 AND AppName = 'Active Price Message Archiver')
		INSERT INTO app.App (AppID, AppName) VALUES (12, 'Active Price Message Archiver')

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 13 AND AppName = 'R10 Price Service')
		INSERT INTO app.App (AppID, AppName) VALUES (13, 'R10 Price Service')

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 14 AND AppName = 'IRMA Price Service')
		INSERT INTO app.App (AppID, AppName) VALUES (14, 'IRMA Price Service')
	SET IDENTITY_INSERT app.App OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO

