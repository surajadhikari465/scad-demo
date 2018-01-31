declare @scriptKey varchar(128)

set @scriptKey = 'FixTaxAbbreviationAndTaxRomanceTraitPattern'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey
	
	update Trait 
	set traitPattern = '^[\d]{7} [^<>]{1,142}$'
	where traitDesc = 'Tax Romance'

	update Trait 
	set traitPattern = '^[\d]{7} [^<>]{1,42}$'
	where traitDesc = 'Tax Abbreviation'

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO