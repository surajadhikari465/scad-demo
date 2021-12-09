DECLARE @deploymentKey NVARCHAR(255) = 'EsbDeprecation_InsertMessageStatus'
IF(
	NOT EXISTS (
		SELECT 1
		FROM app.PostDeploymentScriptHistory
		WHERE ScriptKey = @deploymentKey
	)
)
BEGIN

	DECLARE @sentToEsb NVARCHAR(255) = 'SentToEsb'
	DECLARE @sentToActiveMq NVARCHAR(255) = 'SentToActiveMq'

	IF (NOT EXISTS (SELECT [MessageStatusName] FROM [Icon].[app].[MessageStatus] WHERE [MessageStatusName] = @sentToEsb))
	BEGIN
		INSERT INTO [Icon].[app].[MessageStatus] ([MessageStatusName])
		VALUES (@sentToEsb)
	END

	IF (NOT EXISTS (SELECT [MessageStatusName] FROM [Icon].[app].[MessageStatus] WHERE [MessageStatusName] = @sentToActiveMq))
	BEGIN
		INSERT INTO [Icon].[app].[MessageStatus] ([MessageStatusName])
		VALUES (@sentToActiveMq)
	END

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
	VALUES (@deploymentKey, GetDate());

END
ELSE
BEGIN
	PRINT '[' + Convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @deploymentKey;
END
GO