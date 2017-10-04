declare @scriptKey varchar(128)

set @scriptKey = 'PopulateAttributes'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	PRINT '...Populating Attributes data';

	DECLARE @Today datetime;
	SET @Today = GETDATE();

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'AGE')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) VALUES (2,'AGE','Age Restrict',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'NA')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'NA','Authorized For Sale',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CSD')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CSD','Case Discount Eligible',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CHB')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CHB','Chicago Baby',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CLA')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CLA','Color Added',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'COP')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'COP','Country of Processing',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'DSC')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'DSC','Discontinued',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'EST')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'EST','Electronic Shelf Tag',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'EX')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'EX','Exclusive',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LTD')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LTD','Label Type Desc',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LSC')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LSC','Linked Scan Code',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LI')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LI','Local Item',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LCY')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LCY','Locality',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SRP')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SRP','MSRP',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'NDS')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'NDS','Number of Digits Sent To Scale',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'ORN')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'ORN','Origin',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'PCD')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'PCD','Product Code',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'RES')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'RES','Restricted Hours',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SET')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SET','Scale Extra Text',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SC')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SC','Sign Caption',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SBW')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SBW','Sold by Weight',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'TU')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'TU','Tag UOM',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'TMD')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'TMD','TM Discount Eligible',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SHT')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SHT','Sign Romance Short',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LNG')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LNG','Sign Romance Long',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'RTU')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'RTU','Retail Unit',@Today);
	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CFD')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CFD','Customer Friendly Description',@Today);
	
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())

END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO