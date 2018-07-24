DECLARE @scriptKey VARCHAR(128) = 'AddDefaultValueToEplumSessionTable';

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
    PRINT 'running script ' + @scriptKey

	IF NOT EXISTS (SELECT * FROM eplum.Session)
	BEGIN
		INSERT INTO eplum.Session(SessionID)
		VALUES ('00000')
	END
		  
END
ELSE
BEGIN
    PRINT 'Skipping script ' + @scriptKey
END
GO

