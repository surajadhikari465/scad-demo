DECLARE @scriptKey varchar(128)

SET @scriptKey = 'AddScaleItemAttributes';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @Today DATETIME;
	SET @Today = GETDATE();

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'FTA')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) 
        VALUES (1,'FTA','Force Tare',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SHL')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) 
        VALUES (1,'SHL','Shelf Life',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'UTA')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) 
        VALUES (1,'UTA','Unwrapped Tare Weight',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'WTA')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) 
        VALUES (1,'WTA','Wrapped Tare Weight',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'EAB')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) 
        VALUES (1,'EAB','Use By EAB',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CFS')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) 
        VALUES (1,'CFS','CFS Send to Scale',@Today);

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO