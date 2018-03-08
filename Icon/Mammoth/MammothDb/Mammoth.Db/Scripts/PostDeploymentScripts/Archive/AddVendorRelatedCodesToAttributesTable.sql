DECLARE @scriptKey varchar(128)

SET @scriptKey = 'AddVendorRelatedCodesToAttributesTable';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @Today DATETIME;
	SET @Today = GETDATE();

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'VCS')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'VCS','Vendor Case Size',@Today);

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'VND')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'VND','Vendor Name',@Today);

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'VIN')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'VIN','Vendor Item ID',@Today);

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'VNK')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'VNK','IRMA Vendor Key',@Today);

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO