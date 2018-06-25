DECLARE @scriptKey VARCHAR(128) = 'DeleteR10MessageResponseTable'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	

	IF OBJECT_ID('app.R10MessageResponse', 'U') IS NOT NULL
		DELETE app.R10MessageResponse
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO
