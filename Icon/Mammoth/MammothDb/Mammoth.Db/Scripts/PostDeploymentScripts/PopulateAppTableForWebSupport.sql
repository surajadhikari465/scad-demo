declare @scriptKey varchar(128)
set @scriptKey = 'PopulateAppTableForWebSupport'

IF (NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	IF NOT EXISTS (SELECT 1 FROM app.App WHERE AppName = 'Web Support' AND AppID = 9)
	BEGIN
		SET IDENTITY_INSERT [app].[App] ON
			INSERT INTO [app].[App] (AppID, AppName)
			VALUES (9, 'Web Support')
		SET IDENTITY_INSERT [app].[App] OFF
	END

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO