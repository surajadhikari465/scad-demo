DECLARE @updateAttributesDisplayOrderScriptKey VARCHAR(128) = '35524_FixTraitPatterns'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @updateAttributesDisplayOrderScriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @updateAttributesDisplayOrderScriptKey;

	UPDATE trait
	SET traitPattern = '^[A-Za-z0-9\s-]{0,10}$'
	WHERE traitcode = 'zip'
		
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@updateAttributesDisplayOrderScriptKey, GETDATE())

END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @updateAttributesDisplayOrderScriptKey
END
GO