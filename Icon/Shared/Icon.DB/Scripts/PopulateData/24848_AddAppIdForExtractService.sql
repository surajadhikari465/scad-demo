DECLARE @scriptKeyExtractServiceAppId VARCHAR(128) = '24848_AddAppIdForExtractService'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKeyExtractServiceAppId))
BEGIN
	
	if (NOT EXISTS (select 1 from app.app where AppName = 'SCAD Extract Service'))
	BEGIN
		
		set identity_insert app.app on
		insert into app.App (appid, AppName) values (21, 'SCAD Extract Service' )
		set identity_insert app.app off 

	END
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKeyExtractServiceAppId, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKeyExtractServiceAppId
END
GO