declare @scriptKey varchar(128)

set @scriptKey = 'PopulateMessageType'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	SET IDENTITY_INSERT esb.MessageType ON
	

	IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Item Locale')
	BEGIN
		INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
		VALUES	(1, 'Item Locale')
	END

	IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Price')
	BEGIN
		INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
		VALUES (2, 'Price')
	END
	

	SET IDENTITY_INSERT esb.MessageType OFF

	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO