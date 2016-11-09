/* Script to seed some of the other scripts so they don't run */


declare @scriptKey varchar(128)

set @scriptKey = 'SeedNonGreenFieldScripts'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	insert into app.PostDeploymentScriptHistory values('PopulateAppsInAppTable', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateAttributes', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateCurrency', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateHierarchies', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateItemPriceType', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateItemType', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateMessageAction', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateMessageStatus', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateMessageType', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateRetentionPolicyTable', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateUom', getdate())
	insert into app.PostDeploymentScriptHistory values('PopulateRegionsTable', getdate())
	insert into app.PostDeploymentScriptHistory values('MAMMOTH . 03 . ETL . Load', getdate())
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
