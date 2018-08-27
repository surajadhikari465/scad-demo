--PBI 28239: Pass IRMA item_key and default_identifier traits to Mammoth
DECLARE @scriptKey VARCHAR(128) = 'AddIrmaItemKeyTraitCodes'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;

	DECLARE @traitGroupID INT;
	SELECT  @traitGroupID = traitGroupID FROM dbo.TraitGroup WHERE traitGroupCode = 'ILA';
	
	--regular expression for validation
	DECLARE @patternAnyInteger nvarchar(255)    = N'^[0-9]+$';
	DECLARE @patternOneOrZero nvarchar(255) = N'0|1';
	
	SET IDENTITY_INSERT dbo.Trait ON;

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'IIK')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (171, @TraitGroupID, N'IIK', N'IRMA Item Key', @patternAnyInteger);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'IDI')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (172, @TraitGroupID, N'IDI', N'Default Identifier', @patternOneOrZero);

	SET IDENTITY_INSERT dbo.Trait OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
go
