DECLARE @scriptKey VARCHAR(128) = 'AddEmergencyPriceServiceToAppTable';

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
    PRINT 'running script ' + @scriptKey

	SET IDENTITY_INSERT app.App ON

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppName = 'Emergency Price Service' AND AppID = 22)
	BEGIN
		INSERT INTO app.App(AppID, AppName)
		VALUES (22, 'Emergency Price Service')
	END

	SET IDENTITY_INSERT app.App OFF

    INSERT INTO app.PostDeploymentScriptHistory VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
    PRINT 'Skipping script ' + @scriptKey
END
GO

