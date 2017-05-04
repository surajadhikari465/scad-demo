declare @scriptKey varchar(128)

-- VSTS 21235 - Add MaintFlag Row to DbStatus Table
set @scriptKey = 'AddMaintFlagRowToDbStatusTable'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	-- Update date isn't specified for added row because it will be given a default value.
	if not exists (select 1 from app.DbStatus where StatusFlagName = 'IsOfflineForMaintenance')
	begin
		print 'Adding [IsOfflineForMaintenance] flag to [DbStatus] table...'
		insert into app.DbStatus(StatusFlagName, StatusFlagValue) -- LastUpdateDate will get default of getdate()
			select StatusFlagName = 'IsOfflineForMaintenance', StatusFlagValue = 0
	end
	else
	begin
		print 'Flag [IsOfflineForMaintenance] in [DbStatus] table already exists.'
	end

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO