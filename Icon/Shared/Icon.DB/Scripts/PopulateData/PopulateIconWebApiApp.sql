DECLARE @scriptKey VARCHAR(128) = 'AddIconWebApiApp'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey;	

	SET IDENTITY_INSERT [app].[App] ON 

	INSERT INTO app.App (AppID,AppName)
	Values (18, 'Icon Web Api')

	SET IDENTITY_INSERT [app].[App] OFF 
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO