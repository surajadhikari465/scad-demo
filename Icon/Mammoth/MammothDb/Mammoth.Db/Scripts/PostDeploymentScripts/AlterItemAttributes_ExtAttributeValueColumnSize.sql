DECLARE @scriptKey VARCHAR(128) = 'AlterItemAttributes_ExtAttributeValueColumnSize.'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	ALTER TABLE dbo.ItemAttributes_Ext
	ALTER COLUMN AttributeValue NVARCHAR(300)

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Script already applied: ' + @scriptKey
END
GO

