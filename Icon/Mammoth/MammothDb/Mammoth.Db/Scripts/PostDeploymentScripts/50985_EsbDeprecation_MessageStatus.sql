DECLARE @scriptKey varchar(128) = '50985_EsbDeprecation_InsertMessageStatus';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
	BEGIN
		SET IDENTITY_INSERT esb.MessageStatus ON

		IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'SentToEsb')
			INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(7, 'SentToEsb')

		IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'SentToActiveMq')
			INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(8, 'SentToActiveMq')
		
		SET IDENTITY_INSERT esb.MessageStatus OFF
	END
ELSE
	BEGIN
		print 'Skipping Script ' + @scriptKey;
	END

GO