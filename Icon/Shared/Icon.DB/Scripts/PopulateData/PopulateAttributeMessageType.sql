DECLARE @scriptKey VARCHAR(128) = '23351_PopulateAttributeMessageType'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	SET IDENTITY_INSERT app.MessageType ON

	INSERT INTO app.MessageType(MessageTypeId, MessageTypeName)
	VALUES (14, 'Attribute')

	SET IDENTITY_INSERT app.MessageType OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
	VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO