DECLARE @scriptKey varchar(128)

SET @scriptKey = 'AddGlobalPricingProgramToAttributesTable';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	PRINT '...Adding New Esl Attributes';

	DECLARE @Today DATETIME;
	SET @Today = GETDATE();

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'GPP')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'GPP','Global Pricing Program',@Today);
	
	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO