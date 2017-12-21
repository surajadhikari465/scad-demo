DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'UpdateCustomerFriendlyDescriptionTraitPattern'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	IF EXISTS (SELECT 1 FROM [dbo].[Trait] WHERE traitCode= 'CFD')
		UPDATE dbo.Trait
		SET traitPattern = N'^.{0,60}$'
		WHERE traitCode = 'CFD'

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
go
