--PBI 19318: As a APT user, I need 5 new locale traits added to Icon to cover features migrating from Infor and to provide a single source of data for brand contacts and attributes.
--PBI 14463: As a APT user, I need a new trait group and 2 new brand traits added to Icon to cover features migrating from Infor and to provide a single source of data for brand contacts and attributes.
--PBI 19321: As a APT user, I need new item traits to be added and saved to Icon db to cover features migrating from Infor and to provide a single source of data for brand contacts and attributes.
--PBI 14463: As a APT user, I need a 2 more new brand traits added to Icon: Zip Code (Zip), Locality (LCL)
DECLARE @scriptKey VARCHAR(128) = 'PopulateTraits'

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
			VALUES(189, @TraitGroupID, 'PCO', N'Parent Company','^[0-9]{1,10}$');
			
	SET @traitGroupID = 1; --Item Attributes
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'DAT')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(190, @TraitGroupID, 'DAT', N'Data Source', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'GMT')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(191, @TraitGroupID, 'GMT', N'GMO Transparency', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'IDP')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(192, @TraitGroupID, 'IDP', N'Item Depth', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'IHT')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(193, @TraitGroupID, 'IHT', N'Item Height', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'IWD')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(194, @TraitGroupID, 'IWD', N'Item Width', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'CUB')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(195, @TraitGroupID, 'CUB', N'Cube', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'IWT')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(196, @TraitGroupID, 'IWT', N'Weight', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'TDP')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(197, @TraitGroupID, 'TDP', N'Tray Depth', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'THT')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(198, @TraitGroupID, 'THT', N'Tray Height', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'TWD')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(199, @TraitGroupID, 'TWD', N'Tray Width', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'LBL')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(200, @TraitGroupID, 'LBL', N'Labeling', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'COO')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(201, @TraitGroupID, 'COO', N'Country of Origin', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'PG')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(202, @TraitGroupID, 'PG', N'Package Group', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'PGT')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(203, @TraitGroupID, 'PGT', N'Package Group Type', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'PRL')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(204, @TraitGroupID, 'PRL', N'Private Label', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'APL')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(205, @TraitGroupID, 'APL', N'Appellation', '');
		
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'FT')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(206, @TraitGroupID, 'FT', N'Fair Trade Claim', '0|1'); --Boolean
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'GFC')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(207, @TraitGroupID, 'GFC', N'Gluten Free Claim', '0|1'); --Boolean
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'NGC')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(208, @TraitGroupID, 'NGC', N'Non-GMO Claim', '0|1'); --Boolean
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'OC')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(209, @TraitGroupID, 'OC', N'Organic Claim', '0|1'); --Boolean
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'VAR')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(210, @TraitGroupID, 'VAR', N'Varietal', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'BES')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(211, @TraitGroupID, 'BES', N'Beer Style', '');
			
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'LEX')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(212, @TraitGroupID, 'LEX', N'Line Extension', '');

    SET @traitGroupID = 8; --Brand Traits
    IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'ZIP')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(213, @TraitGroupID, 'ZIP', N'Zip Code','^[0-9]{5}(?:-[0-9]{4})?$');

    IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'Locality')
	  INSERT dbo.Trait(traitID, traitGroupID, traitCode, traitDesc, traitPattern) 
			VALUES(214, @TraitGroupID, 'LCL', N'Locality','^.{1,35}$');
	  SET IDENTITY_INSERT dbo.Trait OFF;
    
    INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE());
  END