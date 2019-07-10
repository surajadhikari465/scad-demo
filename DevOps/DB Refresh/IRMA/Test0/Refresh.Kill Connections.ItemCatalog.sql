USE [master]

declare @dbname sysname
set @dbname = 'ItemCatalog_Test'

declare @spid int
select @spid = MIN(spid) from master.dbo.sysprocesses
where dbid = DB_ID(@dbname) and spid > 50

while @spid is not null
begin
	execute ('kill ' + @spid)
	select @spid = MIN(spid) from master.dbo.sysprocesses
	where dbid = DB_ID(@dbname) and spid > @spid and spid > 50
end
go
