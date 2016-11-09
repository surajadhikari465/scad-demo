declare @scriptKey varchar(128)

set @scriptKey = 'PopulateMessageAction'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	SET IDENTITY_INSERT esb.MessageAction ON
	

	IF NOT EXISTS (SELECT 1 FROM esb.MessageAction WHERE MessageActionName = 'AddOrUpdate')
		INSERT INTO esb.MessageAction(MessageActionId, MessageActionName) VALUES (1, 'AddOrUpdate')
	IF NOT EXISTS (SELECT 1 FROM esb.MessageAction WHERE MessageActionName = 'Delete')
		INSERT INTO esb.MessageAction(MessageActionId, MessageActionName) VALUES (2, 'Delete')
	

	SET IDENTITY_INSERT esb.MessageAction OFF
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO	