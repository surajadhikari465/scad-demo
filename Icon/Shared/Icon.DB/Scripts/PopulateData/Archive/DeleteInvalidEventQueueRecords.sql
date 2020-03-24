DECLARE @populateHospitalityScriptKey VARCHAR(128) = 'DeleteInvalidEventQueueRecords'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @populateHospitalityScriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @populateHospitalityScriptKey;	

	DELETE FROM app.EventQueue
	WHERE EventId = 14
		   AND RegionCode IS NULL
		   AND InProcessBy = 2
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@populateHospitalityScriptKey, GETDATE())

END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @populateHospitalityScriptKey
END
GO