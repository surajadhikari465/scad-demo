DECLARE @key VARCHAR(128) = 'ItemColumnDisplayPopulate';

IF(Not Exists(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @key))
BEGIN

	exec dbo.AddMissingColumnsToItemColumnDisplayOrder
	exec dbo.SetDefaultItemColumnDisplayOrder

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@key, GetDate());

END
ELSE
BEGIN
	print '[' + Convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @key;
END
GO