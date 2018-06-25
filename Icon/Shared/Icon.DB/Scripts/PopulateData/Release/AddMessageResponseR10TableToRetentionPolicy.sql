DECLARE @scriptKey VARCHAR(128) = 'AddMessageResponseR10TableToRetentionPolicy'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	

	DELETE FROM app.RetentionPolicy
	WHERE [Table] = 'R10MessageResponse'

	INSERT INTO app.RetentionPolicy([Server], [Database], [Schema], [Table], [DaysToKeep])
	VALUES (@@SERVERNAME, 'Icon', 'app', 'MessageResponseR10', 10)
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO
