DECLARE @scriptKey VARCHAR(128) = 'AddEmailNotifyOperatorForJobs'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	EXEC msdb.dbo.sp_add_operator
		@name=N'IRMA Developers', 
		@enabled=1, 
		@email_address=N'IRMA.Developers@wholefoods.com', 
		@category_name=N'[Uncategorized]'


	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO