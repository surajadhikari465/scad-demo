declare @scriptKey varchar(128)

set @scriptKey = 'AddNationalClassUpdateAndDeleteEventType'

if(not exists(select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
begin
	print 'Executing :' + @scriptKey

	if(not exists (select * from app.EventType et where et.EventId = 12))
	begin
		set identity_insert app.EventType on

		insert into app.EventType(EventId, EventName)
		values(12, 'National Class Update')

		set identity_insert app.EventType off
	end

	if(not exists (select * from app.EventType et where et.EventId = 13))
	begin
		set identity_insert app.EventType on

		insert into app.EventType(EventId, EventName)
		values(13, 'National Class Delete')

		set identity_insert app.EventType off
	end

	print 'Finished :' + @scriptKey

end
else
begin
	print 'Skipping :' + @scriptKey
end
go