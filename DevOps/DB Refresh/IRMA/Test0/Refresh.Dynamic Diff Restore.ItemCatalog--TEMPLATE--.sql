set nocount on
declare
	@dbName nvarchar(32) = 'ItemCatalog_Test',
	@restoreSql nvarchar(max) = '',
	@fullBackupFile nvarchar(128)

-- The last restore should only be one of two files: either the full ItemCatalog backup or the diff ItemCatalog backup, each of which only differ by the "Full" or "Diff" in their name.
-- We need full-backup name so we can see if it was updated today (part of logic to determine if diff restore is appropriate).
select top 1 @fullBackupFile=replace([bmf].[physical_device_name], 'diff', 'Full')
FROM msdb..restorehistory rs
INNER JOIN msdb..backupset bs ON [rs].[backup_set_id] = [bs].[backup_set_id]
INNER JOIN msdb..backupmediafamily bmf ON [bs].[media_set_id] = [bmf].[media_set_id] 
where [rs].[destination_database_name] like @dbName
order by [rs].[restore_date] desc

-- Show.
select FullBackupFile=@fullBackupFile

-- Diff restore command template (populated by external script).
select @restoreSql = '
%DiffRestoreCommandSql%
'

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Checking if differential restore is needed...'

-- Check if full backup has been modified today (this means diff restore is NOT needed because full restore will have latest, up to this morning).
declare @todayDate nvarchar(16)
select @todayDate = cast(month(getdate()) as nvarchar) + '/' + right('0'+convert(varchar(3), day(getdate())), 2) + '/' + cast(year(getdate()) as nvarchar)

declare @backupFileInfo table (InfoLine nvarchar(256))
declare @fileInfoCmd nvarchar(256)
select @fileInfoCmd = 'dir ' + @fullBackupFile
select FileInfoCmd = @fileInfoCmd
insert into @backupFileInfo
	exec master..xp_cmdshell @fileInfoCmd

select * from @backupFileInfo

declare @fullBackupFileLastMod nvarchar(10)
select @fullBackupFileLastMod = left(InfoLine, 10) from @backupFileInfo where InfoLine like '%[0-9][0-9*][/][0-9][0-9*][/][0-9][0-9][0-9][0-9]%'

select FullBackupFileLastModDate=@fullBackupFileLastMod, TodaysDate=@todayDate

if @fullBackupFileLastMod <> convert(date, getdate(), 101)
begin
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'No full backup today, so restoring DIFF backup...'
	PRINT (@restoreSql)
	exec sp_executesql @restoreSql
end
else
begin
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Full backup today, so skipping DIFF restore.  Finishing full-backup restore...'
	PRINT 'RESTORE DATABASE [ItemCatalog_Test] with RECOVERY'
	RESTORE DATABASE [ItemCatalog_Test] with RECOVERY
end
