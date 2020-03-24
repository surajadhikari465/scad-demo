DECLARE @scriptKey VARCHAR(128) = '23351_AddAttributePublisherAppId'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	SET IDENTITY_INSERT app.App ON

	INSERT INTO app.App(AppID, AppName)
	VALUES (20, 'Attribute Publisher')

	SET IDENTITY_INSERT app.App OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
	VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO