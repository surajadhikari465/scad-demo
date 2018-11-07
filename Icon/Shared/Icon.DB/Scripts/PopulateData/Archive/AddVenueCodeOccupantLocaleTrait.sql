DECLARE @scriptKey VARCHAR(128) = 'AddVenueCodeOccupantLocaleTrait'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	

	DECLARE @TraitGroupID int

	SET @TraitGroupID = (SELECT TraitGroupId FROM TraitGroup WHERE traitGroupCode= 'LT')

	INSERT INTO Trait ( traitCode, traitPattern, traitDesc, traitGroupID )
	VALUES ('VNC', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,60}$', 'Venue Code', @TraitGroupID)

	INSERT INTO Trait ( traitCode, traitPattern, traitDesc, traitGroupID )
	VALUES ('VNO', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$', 'Venue Occupant', @TraitGroupID)

	INSERT INTO Trait ( traitCode, traitPattern, traitDesc, traitGroupID )
	VALUES ('VST', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$', 'Venue Sub Type', @TraitGroupID)

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
	
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO