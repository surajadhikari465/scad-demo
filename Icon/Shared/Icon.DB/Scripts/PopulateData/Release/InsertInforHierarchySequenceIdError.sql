declare @scriptKey varchar(128)

set @scriptKey = 'InsertInforHierarchySequenceIdError'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @inforHierarchyClassListenerAppId int = (select AppID from app.App where AppName = 'Infor Hierarchy Class Listener')

	insert into infor.Errors(AppId, ErrorCode, ErrorDetails)
	values (@inforHierarchyClassListenerAppId, 'InvalidSequenceIDCode', 'The Sequence ID ''{PropertyValue}'' is less than the stored Sequence ID.')

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO