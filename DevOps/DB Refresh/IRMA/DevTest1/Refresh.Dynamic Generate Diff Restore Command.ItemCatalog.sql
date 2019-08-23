-- NOTE: The only thing this SQL should output is the diff-restore command (no other select or print stmts).  You can't even run a "use itemcatalog" because that outputs a message that will correct the SQL.

set nocount on
declare
	@dbName nvarchar(32) = 'ItemCatalog',
	@restoreSql nvarchar(max) = '',
	@diffBackupFile nvarchar(128),
	@fullBackupFile nvarchar(128)

-- The last restore should only be one of two files: either the full ItemCatalog backup or the diff ItemCatalog backup, each of which only differ by the "Full" or "Diff" in their name.
-- So, we blindly replace a "full" ref with "diff", to ensure we have the diff name.
select top 1 @diffBackupFile=replace([bmf].[physical_device_name], 'full', 'Diff')
FROM msdb..restorehistory rs
INNER JOIN msdb..backupset bs ON [rs].[backup_set_id] = [bs].[backup_set_id]
INNER JOIN msdb..backupmediafamily bmf ON [bs].[media_set_id] = [bmf].[media_set_id] 
where [rs].[destination_database_name] like @dbName
order by [rs].[restore_date] desc

-- Generate restore command.
select @restoreSql = '
use master
RESTORE DATABASE [' + @dbName + '] 
FROM
	DISK = N''' + @diffBackupFile + '''
WITH FILE = 1, 
'

SELECT @restoreSql += 'MOVE N' + QUOTENAME(a.name, '''') + ' TO N' + QUOTENAME(a.filename, '''') + ',' + CHAR(13) + CHAR(10)
FROM itemcatalog.dbo.sysfiles a

select @restoreSql += '
NOUNLOAD, RECOVERY, STATS = 10
'

print @restoreSql
