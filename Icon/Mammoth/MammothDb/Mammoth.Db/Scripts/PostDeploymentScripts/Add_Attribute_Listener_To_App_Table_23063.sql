--PBI: 23063
DECLARE @scriptKey VARCHAR(128) = 'Add_Attribute_Listener_To_App_Table_23063';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @scriptKey
			)
		)
BEGIN
	INSERT INTO app.App (AppName)
	VALUES ('Mammoth Attribute Listener')

	INSERT INTO app.PostDeploymentScriptHistory (
		ScriptKey
		,RunTime
		)
	VALUES (
		@scriptKey
		,GETDATE()
		);
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @scriptKey + ' already applied.'
END
GO