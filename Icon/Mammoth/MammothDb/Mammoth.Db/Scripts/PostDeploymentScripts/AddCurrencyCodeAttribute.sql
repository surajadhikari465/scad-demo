DECLARE @scriptKey VARCHAR(128) = 'AddCurrencyCodeAttribute';

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
    PRINT 'running script ' + @scriptKey

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CUR')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc,AddedDate) 
						VALUES (1, 'CUR', 'Currency Code', GETDATE());

    INSERT INTO app.PostDeploymentScriptHistory VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
    PRINT 'Skipping script ' + @scriptKey
END
GO

