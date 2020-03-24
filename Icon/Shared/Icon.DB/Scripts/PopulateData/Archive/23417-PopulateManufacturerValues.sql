DECLARE @scriptKey VARCHAR(128) = '23417-PopulateManufacturerValues2'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	IF NOT EXISTS (
			SELECT *
			FROM dbo.Hierarchy
			WHERE hierarchyName = 'Manufacturer'
			)
		INSERT INTO dbo.Hierarchy
		VALUES ('Manufacturer')

	DECLARE @traitGroupID INT = (
			SELECT traitGroupID
			FROM TraitGroup
			WHERE traitGroupCode = 'HYT'
			);

	IF NOT EXISTS (
			SELECT *
			FROM dbo.Trait
			WHERE traitCode = 'ARC'
			)
		INSERT INTO dbo.Trait
		VALUES (
			'ARC'
			,'^[a-zA-Z0-9_]*$'
			,'AR Customer ID'
			,@traitGroupID
			)
	ELSE
		UPDATE dbo.Trait
		SET traitGroupID = @traitGroupID
		WHERE traitCode = 'ARC'

	DELETE
	FROM dbo.Trait
	WHERE traitCode = 'MZP'

END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO