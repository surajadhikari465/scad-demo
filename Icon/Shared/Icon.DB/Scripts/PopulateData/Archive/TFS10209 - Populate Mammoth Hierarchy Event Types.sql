if not exists (select Name from mammoth.EventType where Name = 'HierarchyClassAdd')
	begin
		insert into mammoth.EventType
           (Name)
     VALUES
		   ('HierarchyClassAdd'),
		   ('HierarchyClassUpdate'),
		   ('HierarchyClassDelete')
	end