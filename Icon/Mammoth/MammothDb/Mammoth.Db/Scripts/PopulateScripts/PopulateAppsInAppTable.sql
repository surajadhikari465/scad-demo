declare @scriptKey varchar(128)

set @scriptKey = 'PopulateAppsInAppTable'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	SET IDENTITY_INSERT [app].[App] ON;
	IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Web Api')
		INSERT INTO app.App (AppID, AppName) VALUES (1, 'Web Api');
	IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'ItemLocale Controller')
		INSERT INTO app.App (AppID, AppName) VALUES (2, 'ItemLocale Controller');
	IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Price Controller')
		INSERT INTO app.App (AppID, AppName) VALUES (3, 'Price Controller');
	IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'API Controller')
		INSERT INTO app.App (AppID, AppName) VALUES (4, 'API Controller');
	IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Product Listener')
		INSERT INTO app.App (AppID, AppName) VALUES (5, 'Product Listener');
	IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Locale Listener')
		INSERT INTO app.App (AppID, AppName) VALUES (6, 'Locale Listener');
	IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Hierarchy Class Listener')
		INSERT INTO app.App (AppID, AppName) VALUES (7, 'Hierarchy Class Listener');
	IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Mammoth Data Purge')
		INSERT INTO app.App (AppID, AppName) VALUES (8, 'Mammoth Data Purge');
	SET IDENTITY_INSERT [app].[App] OFF;

	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO