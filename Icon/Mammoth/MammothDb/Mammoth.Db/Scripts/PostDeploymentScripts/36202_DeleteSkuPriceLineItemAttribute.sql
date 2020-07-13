DECLARE @key VARCHAR(128) = 'DeleteSkuPriceLineItemAttribute';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @key
			)
		)
BEGIN

DELETE
FROM ItemAttributes_Ext
WHERE attributeid IN (
		SELECT attributeid
		FROM Attributes
		WHERE AttributeCode IN (
				'Pl'
				,'pld'
				,'SKU'
				)
		)

DELETE
FROM Attributes
WHERE AttributeCode IN (
		'Pl'
		,'pld'
		,'SKU'
		)

INSERT INTO app.PostDeploymentScriptHistory (
		ScriptKey
		,RunTime
		)
	VALUES (
		@key
		,GETDATE()
		);

END
ELSE
BEGIN
	PRINT '[' + Convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @key;
END
GO