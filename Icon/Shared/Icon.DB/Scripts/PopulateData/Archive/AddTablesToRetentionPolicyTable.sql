DECLARE @scriptKey VARCHAR(128) = 'AddTablesToRetentionPolicyTable'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;
	
	DECLARE @serverName nvarchar(128) = (CONVERT(nvarchar, (SELECT SERVERPROPERTY('ComputerNamePhysicalNetBIOS'))));

	-- Set all current records to have a ReferenceColumn of InsertDate
	UPDATE [app].[RetentionPolicy]
	SET ReferenceColumn = 'InsertDate'

	-- Add some tables that aren't currently being purged
	IF NOT EXISTS (SELECT 1 FROM app.RetentionPolicy WHERE [Schema] = 'app' AND [Table] = 'ItemMovementTransactionHistory')
		INSERT INTO app.RetentionPolicy ([Database],[Schema],[Table],[DaysToKeep],[ReferenceColumn])
		VALUES ('Icon','app','ItemMovementTransactionHistory',7,'InsertDate');

	IF NOT EXISTS (SELECT 1 FROM app.RetentionPolicy WHERE [Schema] = 'esb' AND [Table] = 'MessageQueueItemArchive')
		INSERT INTO app.RetentionPolicy ([Database],[Schema],[Table],[DaysToKeep],[ReferenceColumn])
		VALUES ('Icon','esb','MessageQueueItemArchive',10,'InsertDateUtc');

	IF NOT EXISTS (SELECT 1 FROM app.RetentionPolicy WHERE [Schema] = 'esb' AND [Table] = 'MessageQueueAttributeArchive')
		INSERT INTO app.RetentionPolicy ([Database],[Schema],[Table],[DaysToKeep],[ReferenceColumn])
		VALUES ('Icon','esb','MessageQueueAttributeArchive',10,'InsertDateUtc');

	IF NOT EXISTS (SELECT 1 FROM app.RetentionPolicy WHERE [Schema] = 'esb' AND [Table] = 'MessageArchive')
		INSERT INTO app.RetentionPolicy ([Database],[Schema],[Table],[DaysToKeep],[ReferenceColumn])
		VALUES ('Icon','esb','MessageArchive',10,'InsertDateUtc');

	-- set days to keep to only 5 for lower environments
	IF (@serverName <> 'CEWP6804' OR @serverName <> 'ODWP6804')
	BEGIN
		UPDATE app.RetentionPolicy
		SET DaysToKeep = 5
	END

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO