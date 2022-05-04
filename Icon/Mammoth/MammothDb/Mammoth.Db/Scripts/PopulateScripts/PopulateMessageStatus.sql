declare @scriptKey varchar(128)

set @scriptKey = 'PopulateMessageStatus'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	SET IDENTITY_INSERT esb.MessageStatus ON
	

	IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Ready')
		INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(1, 'Ready')
	
	IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Sent')
		INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(2, 'Sent')
	
	IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Failed')
		INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(3, 'Failed')
	
	IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Associated')
		INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(4, 'Associated')
	
	IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Staged')
		INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(5, 'Staged')
	
	IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Consumed')
		INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(6, 'Consumed')
	
	IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'SentToEsb')
		INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(7, 'SentToEsb')

	IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'SentToActiveMq')
		INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(8, 'SentToActiveMq')

	SET IDENTITY_INSERT esb.MessageStatus OFF
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
