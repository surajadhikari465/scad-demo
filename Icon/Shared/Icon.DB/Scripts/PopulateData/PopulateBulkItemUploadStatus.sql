DECLARE @populateBulkItemUploadStatusScriptKey VARCHAR(128) = 'PopulateBulkItemUploadStatusKey'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @populateBulkItemUploadStatusScriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @populateBulkItemUploadStatusScriptKey;	

	set identity_insert dbo.BulkItemUploadStatus on

	if not exists (select * from dbo.BulkItemUploadStatus where [Status] = 'New')
		insert into dbo.BulkItemUploadStatus (Id,Status) values (0,'New')

	if not exists (select * from dbo.BulkItemUploadStatus where [Status] = 'Processing')
		insert into dbo.BulkItemUploadStatus (Id,Status) values (1,'Processing')

	if not exists (select * from dbo.BulkItemUploadStatus where [Status] = 'Complete')
		insert into dbo.BulkItemUploadStatus (Id,Status) values (2,'Complete')

	if not exists (select * from dbo.BulkItemUploadStatus where [Status] = 'Error')
		insert into dbo.BulkItemUploadStatus (Id,Status) values (3,'Error')

	set identity_insert dbo.BulkItemUploadStatus off

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@populateBulkItemUploadStatusScriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @populateBulkItemUploadStatusScriptKey
END
GO
