declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'InsertInforOutOfSyncUpdateError'

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @inforItemListenerAppId int = (select AppID from app.App where AppName = 'Infor Item Listener')

	insert into infor.Errors(AppId, ErrorCode, ErrorDetails)
	values (@inforItemListenerAppId, 'OutOfSyncItemUpdateErrorCode', 'Item update rejected: time stamp on update was ''{PropertyValue1}'' but the item was updated more recently at ''{PropertyValue2}''.')

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO