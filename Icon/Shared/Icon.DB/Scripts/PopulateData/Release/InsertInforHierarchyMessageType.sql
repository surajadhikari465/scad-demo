declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'InsertInforHierarchyMessageType'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	IF NOT EXISTS (SELECT * FROM app.MessageType mt WHERE mt.MessageTypeName = 'Infor Hierarchy')
	BEGIN
		SET IDENTITY_INSERT app.MessageType ON

		INSERT INTO app.MessageType(MessageTypeId, MessageTypeName)
		VALUES (13, 'Infor Hierarchy')

		SET IDENTITY_INSERT app.MessageType OFF
	END

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO