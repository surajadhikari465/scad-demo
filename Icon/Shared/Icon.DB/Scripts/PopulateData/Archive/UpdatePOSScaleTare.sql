declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'UpdatePOSScaleTare'

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @posScaleTareId int = (select traitID from Trait t where t.traitDesc = 'POS Scale Tare')

	update Trait
	set traitPattern = '^[0-9](\.[0-9]{1,4})?$'
	where traitID = @posScaleTareId

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO