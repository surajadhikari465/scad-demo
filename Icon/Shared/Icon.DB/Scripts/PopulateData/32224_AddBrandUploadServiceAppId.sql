DECLARE @scriptKey VARCHAR(128) = '32224_BrandUploadAppId'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN

set identity_insert app.app on

if not exists (select 1 from app.app where AppName = 'Bulk Brand Upload')
	insert into app.app (AppID, AppName) values (23, 'Bulk Brand Upload')

set identity_insert app.app off

	INSERT INTO app.PostDeploymentScriptHistory(ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())

END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO