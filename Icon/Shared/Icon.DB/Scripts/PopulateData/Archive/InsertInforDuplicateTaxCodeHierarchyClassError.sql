declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'InsertInforDuplicateTaxCodeHierarchyClassError'

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @inforHierarchyClassListenerAppId int = (select AppID from app.App where AppName = 'Infor Hierarchy Class Listener')

	insert into infor.Errors(AppId, ErrorCode, ErrorDetails)
	values (@inforHierarchyClassListenerAppId, 'DuplicateTaxCode', 'A Tax Hierarchy Class already exists with a Tax Code ''{PropertyValue}'' at the start of its name. The first 7 characters of a Tax Class Name which represent the Tax Code must be unique.')

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO