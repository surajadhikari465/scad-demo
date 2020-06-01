CREATE procedure MarkScanAsComplete 
	@id int, 
	@elapsedMs bigint
as
begin
	update RawScans set Status = 'complete', elapsedMs = @elapsedMs where id = @id
end