DECLARE @scriptKey VARCHAR(128) = 'AddPercentageTareWeightToAttributesTable';

IF (NOT EXISTS(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
    PRINT 'running script ' + @scriptKey 

    IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'PTA')
        INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) 
        VALUES (1, 'PTA', 'Percentage Tare Weight', GETDATE());
    
    INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, GETDATE())

END
ELSE
BEGIN
    PRINT 'Skipping script ' + @scriptKey
END
GO
