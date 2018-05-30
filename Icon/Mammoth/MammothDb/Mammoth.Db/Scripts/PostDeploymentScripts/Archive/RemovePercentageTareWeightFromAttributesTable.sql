DECLARE @scriptKey VARCHAR(128) = 'RemovePercentageTareWeightToAttributesTable';

IF (NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
    PRINT 'running script ' + @scriptKey 

	DELETE FROM dbo.Attributes WHERE AttributeCode = 'PTA'

    INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, GETDATE())

END
ELSE
BEGIN
    PRINT 'Skipping script ' + @scriptKey
END
GO
