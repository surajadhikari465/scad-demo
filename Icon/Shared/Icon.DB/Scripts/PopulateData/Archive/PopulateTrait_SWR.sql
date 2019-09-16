DECLARE @scriptKey VARCHAR(128) = 'PopulateTrait_SWR';

IF (EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey;
END
ELSE
BEGIN
		SET IDENTITY_INSERT dbo.Trait ON;

		INSERT INTO Trait (
			traitID
			,traitCode
			,traitPattern
			,traitDesc
			,traitGroupID
			)
		VALUES (
			220
			,'SWR'
			,'0|1' --Boolean
			,'Sodium Warning Required'
			,5
			)

		SET IDENTITY_INSERT dbo.Trait OFF;

		INSERT INTO app.PostDeploymentScriptHistory(ScriptKey, RunTime)
		VALUES (@scriptKey, GETDATE());
END