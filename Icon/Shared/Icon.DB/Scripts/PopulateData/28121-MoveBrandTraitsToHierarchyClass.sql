DECLARE @scriptKey VARCHAR(128) = '28121-MoveBrandTraitsToHierarchyClass'
IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	DECLARE @brandTraitGroupID INT = (
			SELECT traitGroupID
			FROM TraitGroup
			WHERE traitGroupCode = 'BT'
			);
	DECLARE @hytTraitGroupID INT = (
			SELECT traitGroupID
			FROM TraitGroup
			WHERE traitGroupCode = 'HYT'
			);
	UPDATE dbo.Trait
	SET traitGroupID = @hytTraitGroupID
	WHERE traitGroupID = @brandTraitGroupID

	DELETE
	FROM TraitGroup
	WHERE traitGroupCode in ('BT', 'MT')
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO