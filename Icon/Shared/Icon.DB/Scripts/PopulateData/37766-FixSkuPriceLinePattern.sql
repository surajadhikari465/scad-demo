DECLARE @key VARCHAR(128) = 'FixSkuPriceLineCharacterSetPattern';

IF(Not Exists(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @key))
BEGIN


DECLARE @SkuAttributeGroupId INT = (
			SELECT AttributeGroupID
			FROM AttributeGroup
			WHERE AttributeGroupName = 'SKU'  
			)
DECLARE @PriceLineAttributeGroupId INT = (
			SELECT AttributeGroupID
			FROM AttributeGroup
			WHERE AttributeGroupName = 'PriceLine' 
			)
DECLARE @SkuAttributeId INT = (
		SELECT AttributeID
		FROM Attributes
		WHERE AttributeName = 'SKUDescription' and AttributeGroupId = @SkuAttributeGroupId
		)
DECLARE @PriceLineAttributeId INT = (
		SELECT AttributeID
		FROM Attributes
		WHERE AttributeName = 'PriceLineDescription' and AttributeGroupId = @PriceLineAttributeGroupId
		)

UPDATE AttributesWebConfiguration
SET CharacterSetRegexPattern = '^[A-Za-z0-9\s!"#\$%&''\(\)\*\+,\-\./:;<=>\?@\[\\\]\^_`\{\|\}~]*$'
WHERE attributeid = @SkuAttributeId

UPDATE AttributesWebConfiguration
SET CharacterSetRegexPattern = '^[A-Za-z0-9\s!"#\$%&''\(\)\*\+,\-\./:;<=>\?@\[\\\]\^_`\{\|\}~]*$'
WHERE attributeid = @PriceLineAttributeId 

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@key, GetDate());
END
ELSE
BEGIN
	print '[' + Convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @key;
END
GO