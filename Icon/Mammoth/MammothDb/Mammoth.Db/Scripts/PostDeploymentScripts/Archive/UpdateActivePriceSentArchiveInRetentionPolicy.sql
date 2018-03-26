DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'UpdateActivePriceSentArchiveInRetentionPolicy'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	IF EXISTS (SELECT * FROM app.RetentionPolicy WHERE [Table] = 'ActivePriceSentArchive')
	BEGIN
		UPDATE app.RetentionPolicy
		SET [Schema] = 'gpm'
		WHERE [Table] = 'ActivePriceSentArchive'
	END

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO