--PBI: 23063
DECLARE @scriptKey VARCHAR(128) = '23063_PopulateAttributeGroupsTable';

IF(NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	
	INSERT INTO dbo.AttributeGroups(AttributeGroupDesc, AttributeGroupCode, AddedDate)
	VALUES	
		('Global Item', 'GLI', GETDATE()),
		('Item Locale', 'ITL', GETDATE()),
		('Nutrition', 'NUT', GETDATE())

    INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE());
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @scriptKey + ' already applied.'
END
GO