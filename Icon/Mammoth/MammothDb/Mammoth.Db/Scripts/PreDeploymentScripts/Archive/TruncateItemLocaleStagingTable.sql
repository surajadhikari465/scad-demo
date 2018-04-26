DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'TruncateItemLocaleStagingTable';

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	IF EXISTS (SELECT COUNT(1) FROM stage.ItemLocale)
	BEGIN
		TRUNCATE TABLE stage.ItemLocale
	END

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO