DECLARE @scriptKey varchar(128)

SET @scriptKey = 'AddOrderedByInforToAttributesTable';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @Today DATETIME;
	SET @Today = GETDATE();

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'OBI')
		INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (1,'OBI','Ordered By Infor',@Today);

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO