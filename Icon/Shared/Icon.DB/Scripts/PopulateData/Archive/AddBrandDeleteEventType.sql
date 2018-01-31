declare @scriptKey varchar(128)

-- Product Backlog Item 17488: Purge AppLog on ItemCatalog
set @scriptKey = 'AddBrandDeleteEventType'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	if not exists (select * from app.EventType where EventId = 11)
	begin
		set identity_insert app.EventType on

		insert into app.EventType(EventId, EventName)
		values (11, 'Brand Delete')

		set identity_insert app.EventType off
	end

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO