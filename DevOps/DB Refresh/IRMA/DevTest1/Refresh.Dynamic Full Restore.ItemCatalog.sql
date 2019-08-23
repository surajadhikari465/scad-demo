use ItemCatalog
set nocount on
declare
	@dbName nvarchar(32) = 'ItemCatalog',
	@fullBackupFile nvarchar(128)

select top 1 @fullBackupFile=replace([bmf].[physical_device_name], 'diff', 'Full')
FROM msdb..restorehistory rs
INNER JOIN msdb..backupset bs ON [rs].[backup_set_id] = [bs].[backup_set_id]
INNER JOIN msdb..backupmediafamily bmf ON [bs].[media_set_id] = [bmf].[media_set_id] 
where [rs].[destination_database_name] like @dbName
order by [rs].[restore_date] desc

select QueriedFullBackupFile=@fullBackupFile

DECLARE @restoreSql NVARCHAR(max) = '
use master
RESTORE DATABASE [' + @dbName + '] 
FROM
	DISK = N''' + @fullBackupFile + '''
WITH FILE = 1, 
'

SELECT @restoreSql += 'MOVE N' + QUOTENAME(a.name, '''') + ' TO N' + QUOTENAME(a.filename, '''') + ',' + CHAR(13) + CHAR(10)
FROM dbo.sysfiles a

select @restoreSql += '
NOUNLOAD, REPLACE, NORECOVERY, STATS = 10
'

use master
print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Restoring FULL backup...'
PRINT (@restoreSql)
exec sp_executesql @restoreSql
