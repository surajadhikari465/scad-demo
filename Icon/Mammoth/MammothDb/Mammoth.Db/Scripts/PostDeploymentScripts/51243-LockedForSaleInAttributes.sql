DECLARE @scriptKey VARCHAR(128) = 'LockedForSaleInAttributes';

IF (
		NOT EXISTS (
			SELECT *
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @scriptKey
			)
		)
BEGIN
	PRINT 'running script ' + @scriptKey
	PRINT '...Changing Locked For Sale Attribute code';

	DECLARE @Today DATETIME;

	SET @Today = GETDATE();

	IF NOT EXISTS (
			SELECT 1
			FROM Attributes
			WHERE AttributeCode = 'RS'
			)
		INSERT INTO Attributes (
			AttributeGroupID
			,AttributeCode
			,AttributeDesc
			,AddedDate
			)
		VALUES (
			1
			,'RS'
			,'Locked For Sale'
			,@Today
			);

	INSERT INTO app.PostDeploymentScriptHistory
	VALUES (
		@scriptKey
		,@Today
		)
END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO

