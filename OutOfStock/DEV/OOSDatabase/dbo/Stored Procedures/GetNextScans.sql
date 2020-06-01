CREATE procedure GetNextScans @count int
as 
begin
	update top (@count) RawScans
	set status = 'processing'
	output deleted.id, deleted.Message, deleted.createdOn
	where Status = 'new'
	
end