DECLARE @scriptKey VARCHAR(128) = 'UpdateFairTradeCertifiedAttribute';

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
    PRINT 'running script ' + @scriptKey

    UPDATE dbo.Attributes
    SET AttributeCode = 'FTC', AttributeDesc = 'Fair Trade Certified', ModifiedDate = GETDATE()
    WHERE AttributeCode = 'FT'

    INSERT INTO app.PostDeploymentScriptHistory
    VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
    PRINT 'Skipping script ' + @scriptKey
END
GO

