create procedure MarkScanAsFailed @id int as
begin
	update RawScans set Status = 'failed' where id = @id
end