DECLARE @scriptKey VARCHAR(128) = 'PopulateAttributesWebConfiguration'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	INSERT INTO dbo.AttributesWebConfiguration (
		AttributeId,
		GridColumnWidth
		)
	SELECT AttributeId
		,200
	FROM Attributes a
	WHERE a.AttributeId NOT IN (SELECT c.AttributeId FROM dbo.AttributesWebConfiguration c)

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO