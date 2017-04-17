declare @dbName nvarchar(256), @schemaName nvarchar(64)
select @dbName = db_name(), @schemaName = 'app'
if @dbname like 'itemcat%'
	select @schemaName = 'dbo'

-- Ensure the feature of the maint flag is in place before we try to use it.
-- For Icon/Mamm, schema=app.  For IRMA, schema=dbo.

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' and table_schema = @schemaName AND TABLE_NAME='dbstatus')
begin
	if @schemaName = 'app'
		update app.dbstatus set statusFlagValue = 0, lastUpdateDate = getdate() where StatusFlagName = 'IsOfflineForMaintenance'
	else
		update dbo.dbstatus set statusFlagValue = 0, lastUpdateDate = getdate() where StatusFlagName = 'IsOfflineForMaintenance'
end
else
	print 'DB-Status table does not exist... yet?'

