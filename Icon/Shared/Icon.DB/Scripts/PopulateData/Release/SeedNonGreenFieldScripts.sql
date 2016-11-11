/* Script to seed some of the other scripts so they don't run */


declare @scriptKey varchar(128)

set @scriptKey = 'SeedNonGreenFieldScripts'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing :' + @scriptKey

	insert into app.PostDeploymentScriptHistory values('IconMasterData', getdate())
	insert into app.PostDeploymentScriptHistory values('IconPopulateData', getdate())


	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
	print 'Finished :' + @scriptKey
END
ELSE
BEGIN
	print 'Executing :' + @scriptKey
END
GO
