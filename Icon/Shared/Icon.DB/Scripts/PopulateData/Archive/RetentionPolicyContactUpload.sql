DECLARE @key VARCHAR(128) = 'RetentionPolicyContactUpload';

IF(Not Exists(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @key))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @key
	
	DECLARE @currentServerName NVARCHAR(255) = (select @@SERVERNAME)

	INSERT INTO app.RetentionPolicy([Database], [Schema], [Table], DaysToKeep, ReferenceColumn)
		VALUES ('Icon', 'dbo', 'BulkContactUpload', 45, 'FileUploadTime');

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@key, GetDate());

END
ELSE
BEGIN
	print '[' + Convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @key;
END
GO