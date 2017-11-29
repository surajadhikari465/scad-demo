declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'InsertHierarchyClassListenerDeleteErrors'

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @inforHierarchyClassListenerAppId int = (select AppID from app.App where AppName = 'Infor Hierarchy Class Listener')

	insert into infor.Errors(AppId, ErrorCode, ErrorDetails)
	values (@inforHierarchyClassListenerAppId, 'DeleteNonExistantHierarchyClass', '''{PropertyValue}'' does not exists in Icon and cannot be deleted.')
	
	insert into infor.Errors(AppId, ErrorCode, ErrorDetails)
	values (@inforHierarchyClassListenerAppId, 'DeleteHierarchyClassAssociatedToItems', '''{PropertyValue}'' is associated to items and cannot be deleted.')

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO