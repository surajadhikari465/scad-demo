-- This pre-deployment script will truncate the [app].[RetentionPolicy] table
-- so that the alter table statement in dacpac step won't fail.
-- This table will be re-populated in the post-deployment script.

DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'PreDeploy_Re-PopulateRetentionPolicyForStagingTables'

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT 'Running pre-deployment script ' + @scriptKey 
	TRUNCATE TABLE [app].[RetentionPolicy]

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END