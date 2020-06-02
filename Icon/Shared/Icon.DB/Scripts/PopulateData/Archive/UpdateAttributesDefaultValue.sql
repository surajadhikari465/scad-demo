DECLARE @UpdateAttributesDefaultValueScriptKey VARCHAR(128) = 'UpdateAttributesDefaultValue'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @UpdateAttributesDefaultValueScriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @UpdateAttributesDefaultValueScriptKey;	

	update Attributes
		set DefaultValue = replace(defaultValue,'"','')
	WHERE DefaultValue is Not null
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@UpdateAttributesDefaultValueScriptKey, GETDATE())

END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @UpdateAttributesDefaultValueScriptKey
END
GO