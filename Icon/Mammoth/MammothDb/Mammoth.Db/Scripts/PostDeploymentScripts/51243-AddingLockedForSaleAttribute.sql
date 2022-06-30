DECLARE @scriptKey VARCHAR(128) = 'AddingLockedForSaleAttribute';

IF (
		NOT EXISTS (
			SELECT *
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @scriptKey
			)
		)
BEGIN
	PRINT 'running script ' + @scriptKey
	PRINT '...Adding New Locked For Sale Attribute';

	DECLARE @Today DATETIME;

	SET @Today = GETDATE();

	IF NOT EXISTS (
			SELECT 1
			FROM Attributes
			WHERE AttributeCode = 'LFS'
			)
		INSERT INTO Attributes (
			AttributeGroupID
			,AttributeCode
			,AttributeDesc
			,AddedDate
			)
		VALUES (
			1
			,'LFS'
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

