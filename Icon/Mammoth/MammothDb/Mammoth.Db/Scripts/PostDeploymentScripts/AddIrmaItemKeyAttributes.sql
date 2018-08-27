DECLARE @scriptKey VARCHAR(128) = 'AddIrmaItemKeyAttributes';

IF(NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
    PRINT 'running script ' + @scriptKey

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'IIK')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc,AddedDate) 
						VALUES (1, 'IIK', 'IRMA Item Key', GETDATE());

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'IDI')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc,AddedDate) 
						VALUES (1, 'IDI', 'Default Identifier', GETDATE());

    INSERT INTO app.PostDeploymentScriptHistory VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
    PRINT 'Skipping script ' + @scriptKey
END
GO

