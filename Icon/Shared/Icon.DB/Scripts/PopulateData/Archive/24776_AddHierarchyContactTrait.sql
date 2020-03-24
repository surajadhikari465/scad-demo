DECLARE @scriptKey VARCHAR(128) = '24776_AddHierarchyContactTrait'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	
	
	SET IDENTITY_INSERT dbo.Trait ON;
	
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'HCU')
		INSERT dbo.Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
          VALUES (234, 'HCU', '', 'Hierarchy Contact Update Date', 7)

	SET IDENTITY_INSERT dbo.Trait OFF;

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
      VALUES(@scriptKey, GETDATE());
END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO