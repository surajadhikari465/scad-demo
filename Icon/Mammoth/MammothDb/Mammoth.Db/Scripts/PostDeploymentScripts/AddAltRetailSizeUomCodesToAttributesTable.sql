DECLARE @scriptKey varchar(128)

SET @scriptKey = 'AddAltRetailSizeUomCodesToAttributesTable';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @Today DATETIME;
	SET @Today = GETDATE();

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'ASZ')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'ASZ','Alt Retail Size',@Today);

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'AUM')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'AUM','Alt Retail Uom',@Today);

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO