if not exists (select Name from mammoth.EventType where Name = 'ProductAdd')
	begin
		insert into mammoth.EventType
           (Name)
     VALUES
           ('ProductAdd'),
		   ('ProductUpdate')
	end
