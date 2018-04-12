DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'RemovePercentageTareWeightFromIcon'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	DECLARE @ptaTraitId INT = (SELECT TraitID FROM Trait WHERE traitCode = 'PTA')

	DELETE FROM ItemTrait
	WHERE traitID = @ptaTraitId

	DELETE FROM dbo.Trait 
	WHERE traitID = @ptaTraitId

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
go
