declare @scriptKey varchar(128) = 'AddMammothAuditApp',
        @appName varchar(128) = 'Mammoth Audit';

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	PRINT 'running script ' + @scriptKey 

	SET IDENTITY_INSERT [app].[App] ON;

	IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = @appName)
		INSERT INTO app.App (AppID, AppName) VALUES (25, @appName);

	SET IDENTITY_INSERT [app].[App] OFF;

	INSERT INTO app.PostDeploymentScriptHistory values(@scriptKey, GetDate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO