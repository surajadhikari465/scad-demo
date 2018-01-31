declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'InsertInforHierarchyClassErrors'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @inforHierarchyClassListenerAppId int = (select AppID from app.App where AppName = 'Infor Hierarchy Class Listener')

	insert into infor.Errors(AppId, ErrorCode, ErrorDetails)
	values (@inforHierarchyClassListenerAppId, 'DuplicateHierarchyClass', 'A hierarchy class already exists with the name of ''{PropertyValue}'' and with the same hierarchy, hierarchy level, and parent hierarchy class.'),
		(@inforHierarchyClassListenerAppId, 'DuplicateSubBrickCode', 'Sub-Brick Code ''{PropertyValue}'' already exists. Sub-Brick Codes must be unique.')

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO