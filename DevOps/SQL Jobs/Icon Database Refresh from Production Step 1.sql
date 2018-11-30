
--Restrict DB and Refresh from PRD 
	USE [master]
GO
declare @dbname sysname
set @dbname = 'Icon'

declare @spid int
select @spid = MIN(spid) from master.dbo.sysprocesses
where dbid = DB_ID(@dbname) and spid > 50

while @spid is not null
begin
	execute ('kill ' + @spid)
	select @spid = MIN(spid) from master.dbo.sysprocesses
	where dbid = DB_ID(@dbname) and spid > @spid and spid > 50
end
GO
ALTER DATABASE [iCON] SET restricted_user WITH ROLLBACK IMMEDIATE
GO
RESTORE DATABASE [iCON] FROM  DISK = N'\\atx-nas\irmabackups\SQLSHARED3-PRD3\SHARED3P\Icon\iCON.Full.bak' WITH  FILE = 1,  
MOVE N'Icon' TO N'E:\SQL_DATA\SQLSHARED2012D\Icon_Primary.mdf', 
MOVE N'Icon_log' TO N'E:\SQL_LOGS\SQLSHARED2012D\Icon_Primary.ldf', 
NOUNLOAD,  REPLACE,  STATS = 10
GO