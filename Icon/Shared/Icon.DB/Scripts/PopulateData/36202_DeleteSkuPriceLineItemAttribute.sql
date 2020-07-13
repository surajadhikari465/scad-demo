DECLARE @key VARCHAR(128) = 'DeleteSkuPriceLineItemAttribute';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @key
			)
		)
BEGIN

DECLARE @itemAttributeGroupId INT = (
		SELECT attributegroupid
		FROM AttributeGroup
		WHERE AttributeGroupName = 'Global Item'
		)

UPDATE Item
SET itemAttributesJson = JSON_MODIFY(itemAttributesJson, '$.PriceLine', NULL)
WHERE JSON_VALUE(itemAttributesJson, '$.PriceLine') IS NOT NULL

UPDATE Item
SET itemAttributesJson = JSON_MODIFY(itemAttributesJson, '$.SKU', NULL)
WHERE JSON_VALUE(itemAttributesJson, '$.SKU') IS NOT NULL

UPDATE Item
SET itemAttributesJson = JSON_MODIFY(itemAttributesJson, '$.PriceLineDescription', NULL)
WHERE JSON_VALUE(itemAttributesJson, '$.PriceLineDescription') IS NOT NULL

DELETE
FROM ItemColumnDisplayOrder
WHERE ReferenceId IN (
		SELECT attributeId
		FROM Attributes
		WHERE attributegroupid = @itemAttributeGroupId
			AND attributename IN (
				'Sku'
				,'PriceLine'
				,'PriceLineDescription'
				)
		)
	AND ColumnType = 'Attribute'

DELETE
FROM AttributesWebConfiguration
WHERE attributeId IN (
		SELECT attributeId
		FROM Attributes
		WHERE attributegroupid = @itemAttributeGroupId
			AND attributename IN (
				'Sku'
				,'PriceLine'
				,'PriceLineDescription'
				)
		)

DELETE
FROM Attributes
WHERE attributegroupid = @itemAttributeGroupId
	AND attributename IN (
		'Sku'
		,'PriceLine'
		,'PriceLineDescription'
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