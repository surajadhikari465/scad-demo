DECLARE @scriptKey VARCHAR(128) = 'AddLocaleSubtypeTrait'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	

	DECLARE @TraitGroupID int

	SET @TraitGroupID = (SELECT TraitGroupId FROM TraitGroup WHERE traitGroupCode= 'LT')

	INSERT INTO Trait ( traitCode, traitPattern, traitDesc, traitGroupID )
	VALUES ('LST', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,60}$', 'Locale Subtype', @TraitGroupID)

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
	
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO