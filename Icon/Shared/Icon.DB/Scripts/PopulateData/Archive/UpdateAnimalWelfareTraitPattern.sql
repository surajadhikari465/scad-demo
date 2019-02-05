declare @scriptKey varchar(128)

-- PBI 29184 - As GDT I want enumeration removed for several attributes in Icon - Add New Columns to Database
set @scriptKey = 'UpdateAnimalWelfareRatingTraitPattern'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN

	-- Populate the new columns with values from old columns using a post deploy script
	UPDATE dbo.Trait
	SET traitPattern = '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C\+]{0,50}$'
	WHERE traitCode = 'AWR'
	

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO