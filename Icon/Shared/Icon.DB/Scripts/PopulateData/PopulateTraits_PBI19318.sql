--PBI 19318: As a APT user, I need 5 new locale traits added to Icon to cover features migrating from Infor and to provide a single source of data for brand contacts and attributes.
--PBI 14463: As a APT user, I need a new trait group and 2 new brand traits added to Icon to cover features migrating from Infor and to provide a single source of data for brand contacts and attributes.
DECLARE @scriptKey VARCHAR(128) = 'PopulateTraits_PBI19318'

IF(Exists(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
  BEGIN
    PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey;
  END
ELSE
  BEGIN
    SET IDENTITY_INSERT dbo.TraitGroup ON;

    IF NOT EXISTS(SELECT 1 FROM dbo.TraitGroup WHERE traitGroupCode= 'BT')
	  INSERT dbo.TraitGroup(traitGroupID, traitGroupCode, traitGroupDesc) 
			VALUES(8, 'BT', 'Brand Traits');

    SET IDENTITY_INSERT dbo.TraitGroup OFF;

    DECLARE @traitGroupID INT = 5; --Locale Traits
    SET IDENTITY_INSERT dbo.Trait ON;

    IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'IDT')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(183, @TraitGroupID, 'IDT', N'Ident', '0|1'); --Boolean

    IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'LL')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(184, @TraitGroupID, 'LL', N'Liquor Licensing','(Beer|Wine|Spirit)$'); --Pick list

    IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'MI')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern)
			VALUES(185, @TraitGroupID, 'MI', N'PrimeNow Merchant ID','^[0-9]{11}$');  --11 digit integer

    IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'MIE')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(186, @TraitGroupID, 'MIE', N'PrimeNow Merchant ID Encrypted','^[a-zA-Z0-9]{13}$'); --Alphanumeric 13 character string

    IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'LZ')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(187, @TraitGroupID, 'LZ', N'Local Zone','^.{1,35}$'); --Alphanumeric & special characters up to 35 characters
    

    SET @traitGroupID = 8; --Brand Traits
    IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'GRD')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(188, @TraitGroupID, 'GRD', N'Global/Regional Designation','(Global|Regional)$'); --Pick list

    IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'PCO')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(189, @TraitGroupID, 'PCO', N'Hierarchy ID for chosen brand','^[0-9]{1,10}$');

	  SET IDENTITY_INSERT dbo.Trait OFF;
    
    INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE());
  END