DECLARE @scriptKey varchar(128);
SET @scriptKey = 'AddEsbMessageTypeForCheckpointRequest'

IF(NOT EXISTS(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey
	
	SET IDENTITY_INSERT esb.MessageType ON	

	IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Checkpoint Request')
	BEGIN
		INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
		VALUES	(7, 'Checkpoint Request')
	END

	SET IDENTITY_INSERT esb.MessageType OFF
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO