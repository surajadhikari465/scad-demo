declare @scriptKey varchar(128)

set @scriptKey = 'FixPackageUnitTraitPattern'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	update Trait 
	set traitPattern = '^[1-9][0-9]{0,2}$'
	where traitDesc = 'Package Unit'

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO