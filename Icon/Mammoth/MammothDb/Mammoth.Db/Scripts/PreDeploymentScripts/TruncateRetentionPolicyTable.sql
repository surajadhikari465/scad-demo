-- This pre-deployment script will truncate the [app].[RetentionPolicy] table
-- so that the alter table statement in dacpac step won't fail.
-- This table will be re-populated in the post-deployment script.

declare @scriptKey varchar(128)

set @scriptKey = 'Re-PopulateRetentionPolicyForStagingTables'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @serverName nvarchar(50);
	SET @serverName = @@SERVERNAME;

	-- DEV and TEST Instance
	IF @serverName = 'CEWD6587\MAMMOTH'
	BEGIN
		-- DEV DB
		USE Mammoth_Dev
		TRUNCATE TABLE [app].[RetentionPolicy]

		-- TEST DB
		USE Mammoth
		TRUNCATE TABLE [app].[RetentionPolicy]
	END

	-- QA Instance
	IF @serverName = 'QA-01-MAMMOTH\MAMMOTH'
	BEGIN
		USE Mammoth
		TRUNCATE TABLE [app].[RetentionPolicy]
	END

	-- PRD Instance
	IF @serverName = 'PRD-01-MAMMOTH\MAMMOTH'
	BEGIN
		USE Mammoth
		TRUNCATE TABLE [app].[RetentionPolicy]
	END
END
