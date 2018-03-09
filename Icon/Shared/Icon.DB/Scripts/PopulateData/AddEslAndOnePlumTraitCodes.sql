--PBI 25777: Update Icon to receive and send new Global Attributes for SLAW, ESL and/or OnePlum
DECLARE @scriptKey VARCHAR(128) = 'AddEslAndOnePlumTraitCodes'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;
	SET IDENTITY_INSERT dbo.Trait ON;

	DECLARE @traitID INT = 157;
	DECLARE @traitGroupID INT = 1;
	DECLARE @regExStdString    NVARCHAR(255) = N'[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Z}].{0,255}$';
	DECLARE @regExStdString60  NVARCHAR(255) = Replace(@regExStdString, N'255', N'60');
	DECLARE @regExStdString300 NVARCHAR(255) = Replace(@regExStdString, N'255', N'300');
	DECLARE @regExOneOrZero    NVARCHAR(255) = N'0|1';
	DECLARE @regExUint3Digits  NVARCHAR(255) = N'^\d{1,3}$'
	DECLARE @regExUint9Digits  NVARCHAR(255) = N'^\d{1,9}$'

	SET @traitID+=1;
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'FXT')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@TraitID, @TraitGroupID, N'FXT', N'Flexible Text', @regExStdString300);

	SET @traitID+=1;
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'MOG')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@TraitID, @TraitGroupID, N'MOG', N'Made with Organic Grapes', @regExOneOrZero);

	SET @traitID+=1;
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'PRB')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@TraitID, @TraitGroupID, N'PRB', N'Prime Beef', @regExOneOrZero);

	SET @traitID+=1;
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'RFA')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@TraitID, @TraitGroupID, N'RFA', N'Rainforest Alliance', @regExOneOrZero);

	SET @traitID+=1;
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'RFD')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@TraitID, @TraitGroupID, N'RFD', N'Refrigerated', @regExOneOrZero);

	SET @traitID+=1;
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'SMF')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@TraitID, @TraitGroupID, N'SMF', N'Smithsonian Bird Friendly', @regExOneOrZero);

	SET @traitID+=1;
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'WIC')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@TraitID, @TraitGroupID, N'WIC', N'WIC Eligible', @regExOneOrZero);

	SET @traitID+=1;
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'SLF')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@TraitID, @TraitGroupID, N'SLF', N'Shelf Life', @regExUint3Digits);

	SET @traitID+=1;
	IF NOT EXISTS (SELECT 1 FROM dbo.Trait WHERE traitCode= 'ITG')
	INSERT dbo.Trait (traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES  (@TraitID, @TraitGroupID, N'ITG', N'Self Checkout Item Tare Group', @regExStdString60);


	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
	SET IDENTITY_INSERT dbo.Trait OFF
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
go
