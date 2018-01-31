declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'InsertInforHierarchyMismatchError'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @inforHierarchyClassListenerAppId int = (select AppID from app.App where AppName = 'Infor Hierarchy Class Listener')

	insert into infor.Errors(AppId, ErrorCode, ErrorDetails)
	values (@inforHierarchyClassListenerAppId, 'HierarchyMismatch', 'A hierarchy class with the same ID exists under a different hierarchy than ''{PropertyValue}''.')

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO