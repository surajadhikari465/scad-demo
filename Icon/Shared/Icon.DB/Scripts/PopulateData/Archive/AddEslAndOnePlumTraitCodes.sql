--PBI 25777: Update Icon to receive and send new Global Attributes for SLAW, ESL and/or OnePlum
DECLARE @scriptKey VARCHAR(128) = 'AddEslAndOnePlumTraitCodes'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;
	SET IDENTITY_INSERT dbo.Trait ON;

	DECLARE @traitGroupID INT;
	SELECT  @traitGroupID = traitGroupID FROM dbo.TraitGroup WHERE traitGroupCode = 'IA';

	--regular expressions for validation (trait patterns)
	DECLARE @patternAnyText255 nvarchar(255)    = N'[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Z}].{0,255}$|^$';
	DECLARE @patternAnyText60 nvarchar(255)  = Replace(@patternAnyText255, N'255', N'60');
	DECLARE @patternAnyText300 nvarchar(255) = Replace(@patternAnyText255, N'255', N'300');
	DECLARE @patternIntZeroTo100 nvarchar(255)  = N'^[0-9][0-9]?$|^100$|^$';
	DECLARE @patternRefigeratedEnum nvarchar(255)= N'^[Rr][Ee][Ff][Rr][Ii][Gg][Ee][Rr][Aa][Tt][Ee][Dd]$|^[Ss][Hh][Ee][Ll][Ff][- ]?[Ss][Tt][Aa][Bb][Ll][Ee]$|^$';
	DECLARE @patternBoolYesNoTrueFalseOneZero nvarchar(255) = N'^[YyNnTtFf01]$|^[Yy][Ee][Ss]$|^[Nn][Oo]$|^[Oo][Nn]$|^[Oo][Ff][Ff]$|^[Tt][Rr][Uu][Ee]$|^[Ff][Aa][Ll][Ss][Ee]$|^$';

	--remove enum-like validation pattern for FairTradeCertified (FTC)
	UPDATE dbo.Trait SET traitPattern = @patternAnyText255 WHERE traitCode = 'FTC'

	--make sure we can store item traits up to 300 chars for FXT
	ALTER TABLE dbo.ItemTrait 
	ALTER COLUMN traitValue NVARCHAR(300)

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'GPP')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (154, @TraitGroupID, N'GPP', N'Global Pricing Program', @patternAnyText255);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'FXT')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (158, @TraitGroupID, N'FXT', N'Flexible Text', @patternAnyText300);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'MOG')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (159, @TraitGroupID, N'MOG', N'Made with Organic Grapes', @patternAnyText255);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'PRB')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (160, @TraitGroupID, N'PRB', N'Prime Beef', @patternBoolYesNoTrueFalseOneZero);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'RFA')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (161, @TraitGroupID, N'RFA', N'Rainforest Alliance', @patternBoolYesNoTrueFalseOneZero);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'RFD')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (162, @TraitGroupID, N'RFD', N'Refrigerated', @patternRefigeratedEnum);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'SMF')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (163, @TraitGroupID, N'SMF', N'Smithsonian Bird Friendly', @patternBoolYesNoTrueFalseOneZero);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'WIC')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (164, @TraitGroupID, N'WIC', N'WIC Eligible', @patternBoolYesNoTrueFalseOneZero);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'SLF')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (165, @TraitGroupID, N'SLF', N'Shelf Life', @patternIntZeroTo100);

	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'ITG')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (166, @TraitGroupID, N'ITG', N'Self Checkout Item Tare Group', @patternAnyText60);

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
	SET IDENTITY_INSERT dbo.Trait OFF
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
go
