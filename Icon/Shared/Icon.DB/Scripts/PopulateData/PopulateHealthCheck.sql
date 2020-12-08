DECLARE @key VARCHAR(128) = 'PopulateHealthCheck';

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @key ))
BEGIN
	INSERT INTO [app].[HealthCheck] (CheckId) VALUES (1);	
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@key, GetDate());
END
ELSE
BEGIN
	PRINT '[' + Convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @key;
END
GO