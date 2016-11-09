if not exists (select MessageTypeId from app.MessageType where MessageTypeName = 'eWIC')
	begin
		insert into app.MessageType values ('eWIC')
	end
