DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddPrimeAffinityMessageType'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	SET IDENTITY_INSERT esb.MessageType ON

	IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Prime PSG' AND MessageTypeId = 3)
		INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
		VALUES (3, 'Prime PSG')
		
	SET IDENTITY_INSERT esb.MessageType OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO

