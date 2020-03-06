declare @scriptKey varchar(128)

set @scriptKey = '30737_PopulateGlobalItemMessageType'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	SET IDENTITY_INSERT esb.MessageType ON
	

	IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Global Item')
	BEGIN
		INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
		VALUES	(8, 'Global Item')
	END	

	SET IDENTITY_INSERT esb.MessageType OFF

	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO