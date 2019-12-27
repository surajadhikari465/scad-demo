DECLARE @scriptKey VARCHAR(128) = 'DeleteDataFromPluTables'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	TRUNCATE TABLE app.PLUSequence;
	TRUNCATE TABLE app.PLUCategory;
	TRUNCATE TABLE app.PLURequest;
	TRUNCATE TABLE app.PLURequestChangeHistory;
	TRUNCATE TABLE app.PLURequestChangeType;

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO