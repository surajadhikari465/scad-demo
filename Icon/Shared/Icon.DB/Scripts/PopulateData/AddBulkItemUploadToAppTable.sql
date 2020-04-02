DECLARE @populateHospitalityScriptKey VARCHAR(128) = 'AddBulkItemUploadToAppTable'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @populateHospitalityScriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @populateHospitalityScriptKey;	

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppID = 22 AND AppName = 'Bulk Item Upload')
	BEGIN
		SET IDENTITY_INSERT app.App ON

		INSERT INTO app.App (AppID, AppName)
		VALUES (22, 'Bulk Item Upload')

		SET IDENTITY_INSERT app.App OFF
	END
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@populateHospitalityScriptKey, GETDATE())

END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @populateHospitalityScriptKey
END
GO