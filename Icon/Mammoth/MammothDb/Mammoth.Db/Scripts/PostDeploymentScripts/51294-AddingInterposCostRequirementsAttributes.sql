DECLARE @scriptKey VARCHAR(128) = 'InterposCostRequirementsInAttributes';

IF (
		NOT EXISTS (
			SELECT *
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @scriptKey
			)
		)
BEGIN
	PRINT 'running script ' + @scriptKey
	PRINT '...Adding New Interpos and Cost Attribute';

	DECLARE @Today DATETIME;

	SET @Today = GETDATE();

	IF NOT EXISTS (
			SELECT 1
			FROM Attributes
			WHERE AttributeCode = 'QTY'
			)
		INSERT INTO Attributes (
			AttributeGroupID
			,AttributeCode
			,AttributeDesc
			,AddedDate
			)
		VALUES (
			1
			,'QTY'
			,'Quantity Required'
			,@Today
			);

	IF NOT EXISTS (
			SELECT 1
			FROM Attributes
			WHERE AttributeCode = 'PRQ'
			)
		INSERT INTO Attributes (
			AttributeGroupID
			,AttributeCode
			,AttributeDesc
			,AddedDate
			)
		VALUES (
			1
			,'PRQ'
			,'Price Required'
			,@Today
			);

	IF NOT EXISTS (
			SELECT 1
			FROM Attributes
			WHERE AttributeCode = 'QPR'
			)
		INSERT INTO Attributes (
			AttributeGroupID
			,AttributeCode
			,AttributeDesc
			,AddedDate
			)
		VALUES (
			1
			,'QPR'
			,'Quantity Prohibit'
			,@Today
			);

	IF NOT EXISTS (
			SELECT 1
			FROM Attributes
			WHERE AttributeCode = 'CBW'
			)
		INSERT INTO Attributes (
			AttributeGroupID
			,AttributeCode
			,AttributeDesc
			,AddedDate
			)
		VALUES (
			1
			,'CBW'
			,'Costed By Weight'
			,@Today
			);

	IF NOT EXISTS (
			SELECT 1
			FROM Attributes
			WHERE AttributeCode = 'CWR'
			)
		INSERT INTO Attributes (
			AttributeGroupID
			,AttributeCode
			,AttributeDesc
			,AddedDate
			)
		VALUES (
			1
			,'CWR'
			,'Catch Weight Required'
			,@Today
			);

	IF NOT EXISTS (
			SELECT 1
			FROM Attributes
			WHERE AttributeCode = 'CW'
			)
		INSERT INTO Attributes (
			AttributeGroupID
			,AttributeCode
			,AttributeDesc
			,AddedDate
			)
		VALUES (
			1
			,'CW'
			,'Catch Wt Req'
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

