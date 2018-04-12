DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddIrmaPriceMessageType'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	SET IDENTITY_INSERT esb.MessageType ON

	IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Irma Price' AND MessageTypeId = 3)
		INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
		VALUES (6, 'Irma Price')
		
	SET IDENTITY_INSERT esb.MessageType OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO