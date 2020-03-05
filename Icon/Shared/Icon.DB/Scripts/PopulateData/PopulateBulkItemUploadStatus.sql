--The script didn't populate 'Error' status due to incorrect table name.
DECLARE @populateBulkItemUploadStatusScriptKey VARCHAR(128) = 'PopulateBulkItemUploadStatusKey'

PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @populateBulkItemUploadStatusScriptKey;	

set identity_insert dbo.BulkUploadStatus on

if not exists (select * from dbo.BulkUploadStatus where [Status] = 'New')
	insert into dbo.BulkUploadStatus (Id,Status) values (0,'New')

if not exists (select * from dbo.BulkUploadStatus where [Status] = 'Processing')
	insert into dbo.BulkUploadStatus (Id,Status) values (1,'Processing')

if not exists (select * from dbo.BulkUploadStatus where [Status] = 'Complete')
	insert into dbo.BulkUploadStatus (Id,Status) values (2,'Complete')

if not exists (select * from dbo.BulkUploadStatus where [Status] = 'Error')
	insert into dbo.BulkUploadStatus (Id,Status) values (3,'Error')

set identity_insert dbo.BulkUploadStatus off

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @populateBulkItemUploadStatusScriptKey))
  INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@populateBulkItemUploadStatusScriptKey, GETDATE())
