
create procedure SaveRawScan 
@data varchar(max)
as
begin
	insert into RawScans (Message) values (@data)
end