DECLARE @scriptKey varchar(128) = '37503_AddScaleItemToAttributes';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @Today DATETIME;
	SET @Today = GETDATE();

	IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SCL')
		INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) 
        VALUES (1,'SCL','Scale Item',@Today);

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO