declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'AddInforProductMessageType'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	IF NOT EXISTS(SELECT * FROM app.MessageType m WHERE m.MessageTypeId = 12)
	BEGIN
		SET IDENTITY_INSERT app.MessageType ON
	
		INSERT INTO app.MessageType(MessageTypeId, MessageTypeName)
		VALUES (12, 'Infor Product')
	
		SET IDENTITY_INSERT app.MessageType OFF
	END

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO