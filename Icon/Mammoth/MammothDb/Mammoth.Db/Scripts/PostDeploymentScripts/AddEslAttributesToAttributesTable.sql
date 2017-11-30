DECLARE @scriptKey varchar(128)

SET @scriptKey = 'AddEslAttributesToAttributesTable';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	PRINT '...Adding New Esl Attributes';

	DECLARE @Today DATETIME;
	SET @Today = GETDATE();

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CFD')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'CFD','Customer Friendly Description',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'FT')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'FT','Fair Trade',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'FXT')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'FXT','Flexible Text',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'MOG')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'MOG','Made With Organic Grapes',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'MBG')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'MBG','Made with Biodynamic Grapes',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'NR')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'NR','Nutrition Required',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'PRB')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'PRB','Prime Beef',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'RFA')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'RFA','Rainforest Alliance',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'RFD')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'RFD','Refrigerated or Shelf Stable',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SBF')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'SBF','Smithsonian Bird Friendly',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'WIC')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'WIC','WIC',@Today);

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO