DECLARE @PopulateBulkItemUploadFileTypesScriptKey VARCHAR(128) = 'PopulateBulkItemUploadFileTypesKey'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @PopulateBulkItemUploadFileTypesScriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @PopulateBulkItemUploadFileTypesScriptKey;	

	if not exists (select * from dbo.BulkItemUploadFileTypes where [FileType] = 'Add')
		insert into dbo.BulkItemUploadFileTypes (Id,FileType) values (0,'Add')

	if not exists (select * from dbo.BulkItemUploadFileTypes where [FileType] = 'Update')
		insert into dbo.BulkItemUploadFileTypes (Id,FileType) values (1,'Update')

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@PopulateBulkItemUploadFileTypesScriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @PopulateBulkItemUploadFileTypesScriptKey
END
GO
