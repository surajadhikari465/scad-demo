--PBI 28313: As Icon I want to consume the new Ph 5 E Store and Hospitality Item Traits and pass them to the ESB
DECLARE @scriptKey VARCHAR(128) = 'AddHospitalityAndEstoreTraitCodes'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;
	SET IDENTITY_INSERT dbo.Trait ON;

	DECLARE @traitGroupID INT;
	SELECT  @traitGroupID = traitGroupID FROM dbo.TraitGroup WHERE traitGroupCode = 'IA';

	--regular expressions for validation (trait patterns)
	DECLARE @patternExcludeComma255 nvarchar(255)    = N'^[^,]{0,255}$';
	DECLARE @patternBoolYesNoTrueFalseOneZero nvarchar(255) = N'^[YyNnTtFf01]$|^[Yy][Ee][Ss]$|^[Nn][Oo]$|^[Oo][Nn]$|^[Oo][Ff][Ff]$|^[Tt][Rr][Uu][Ee]$|^[Ff][Aa][Ll][Ss][Ee]$|^$';
	DECLARE @patternIntZeroTo999OrBlank nvarchar(255)  = N'^(?:0|[1-9]\d{0,2})$|^\s*$';

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'LIN')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (173, @TraitGroupID, N'LIN', N'Line', @patternExcludeComma255);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'SKU')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (174, @TraitGroupID, N'SKU', N'SKU', @patternExcludeComma255);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'PL')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (175, @TraitGroupID, N'PL', N'Price Line', @patternExcludeComma255)

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'VS')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (176, @TraitGroupID, N'VS', N'Variant Size', @patternExcludeComma255);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'ESN')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (177, @TraitGroupID, N'ESN', N'EStore Nutrition Required', @patternBoolYesNoTrueFalseOneZero);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'PNE')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (178, @TraitGroupID, N'PNE', N'Prime Now Eligible', @patternBoolYesNoTrueFalseOneZero);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'ESE')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (179, @TraitGroupID, N'ESE', N'Estore Eligible', @patternBoolYesNoTrueFalseOneZero);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'TSE')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (180, @TraitGroupID, N'TSE', N'TSF (365) Eligible', @patternBoolYesNoTrueFalseOneZero);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'WFE')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (181, @TraitGroupID, N'WFE', N'WFM Eligilble', @patternBoolYesNoTrueFalseOneZero);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'OTE')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (182, @TraitGroupID, N'OTE', N'Other 3P Eligible', @patternBoolYesNoTrueFalseOneZero);

	UPDATE dbo.Trait 
	SET traitPattern = @patternIntZeroTo999OrBlank
	WHERE traitCode = 'SLF'

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
	SET IDENTITY_INSERT dbo.Trait OFF
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
go
