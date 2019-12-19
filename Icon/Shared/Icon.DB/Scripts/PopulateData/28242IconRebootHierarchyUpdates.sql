DECLARE @scriptKey VARCHAR(128) = '28242IconRebootHierarchyUpdates';

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN

	-- update sub brick (level 5)
	UPDATE HierarchyClass
	SET hierarchyClassName = SUBSTRING(hierarchyClassName, 0, charindex(':', hierarchyClassName))
	WHERE HIERARCHYID = 1
		AND hierarchyLevel = 5
		AND hierarchyClassName LIKE '%:%'

	-- update national class level 4
	UPDATE HierarchyClass
	SET hierarchyClassName = SUBSTRING(hierarchyClassName, 0, charindex(':', hierarchyClassName))
	WHERE HIERARCHYID = 6
		AND hierarchyLevel = 4
		AND hierarchyClassName LIKE '%:%'
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO