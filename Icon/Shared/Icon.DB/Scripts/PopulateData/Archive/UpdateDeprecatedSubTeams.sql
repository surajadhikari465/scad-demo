DECLARE @scriptKey VARCHAR(128) = 'UpdateDeprecatedSubTeamNames'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	UPDATE HierarchyClass
	SET hierarchyClassName = 'ZZZ Do Not Use Ready to Eat Seafood (5920)'
	WHERE hierarchyClassName = 'Ready to Eat Seafood (5920)'

	UPDATE HierarchyClass
	SET hierarchyClassName = 'ZZZ Do Not Use Ready to Eat Meat (5910)'
	WHERE hierarchyClassName = 'Ready to Eat Meat (5910)'

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
	VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO