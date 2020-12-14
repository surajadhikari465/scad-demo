DECLARE @scriptKey varchar(128) = 'SCM1-616_PopulateHealthCheck';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @Today DATETIME;
	SET @Today = GETDATE();

	IF NOT EXISTS (SELECT * FROM app.HealthCheck WHERE CheckId = 1)
		INSERT INTO app.HealthCheck(CheckId) 
        VALUES (1);

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO