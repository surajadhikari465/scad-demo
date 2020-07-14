DECLARE @key VARCHAR(128) = 'AddItemGroupsAndMembers';
IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @key
			)
		)
BEGIN
DECLARE @SkuItemGroupTypeId INT = (
		SELECT ItemGroupTypeId
		FROM ItemGroupType
		WHERE ItemGroupTypeName = 'SKU'
		)
DECLARE @PriceLineItemGroupTypeId INT = (
		SELECT ItemGroupTypeId
		FROM ItemGroupType
		WHERE ItemGroupTypeName = 'Price Line'
		)
DECLARE @BrandhierarchyId VARCHAR(20) = (
		SELECT HIERARCHYID
		FROM Hierarchy
		WHERE Hierarchyname = 'Brands'
		)
CREATE TABLE #tmSku
(
	ItemId INT,
	ScanCode NVARCHAR(13),
	Sku NVARCHAR(255),
	CustomerFriendlyDescription NVARCHAR(255)
)

INSERT #tmSku(ItemId, ScanCode, Sku, CustomerFriendlyDescription)
SELECT item.itemid
	,scancode
	,JSON_VALUE(ItemAttributesJson, '$.SKU') AS Sku
	,JSON_VALUE(ItemAttributesJson, '$.CustomerFriendlyDescription') AS CustomerFriendlyDescription
FROM item
JOIN scancode ON item.itemid = scancode.itemid
WHERE JSON_VALUE(ItemAttributesJson, '$.SKU') IS NOT NULL
UNION
SELECT item.itemid
	,scancode
	,scancode AS Sku
	,JSON_VALUE(ItemAttributesJson, '$.CustomerFriendlyDescription') AS CustomerFriendlyDescription
FROM item
JOIN scancode ON item.itemid = scancode.itemid
WHERE JSON_VALUE(ItemAttributesJson, '$.SKU') IS NULL


INSERT INTO [dbo].[ItemGroup] (
	[ItemGroupTypeId]
	,[ItemGroupAttributesJson]
	,[LastModifiedBy]
	)
SELECT DISTINCT @SkuItemGroupTypeId
	,(SELECT CustomerFriendlyDescription as SKUDescription FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
	,sku
FROM #tmSku
WHERE #tmSku.scanCode = #tmSku.Sku

INSERT INTO [dbo].[ItemGroupMember] (
	[ItemId]
	,[ItemGroupId]
	,[IsPrimary]
	,[LastModifiedBy]
	)
SELECT ItemId
	,ItemGroupId
	,CASE 
		WHEN scanCode = Sku
			THEN 1
		ELSE 0
		END
	,'Script'
FROM #tmSku
INNER JOIN ItemGroup ON ItemGroup.LastModifiedBy = #tmSku.Sku
AND ItemGroup.ItemGroupTypeId = @SkuItemGroupTypeId

UPDATE ItemGroup
SET LastModifiedBy = 'Script'

CREATE TABLE #tmpPriceLine
(
	ItemId INT,
	ScanCode NVARCHAR(13),
	PriceLine NVARCHAR(255),
	PriceLineDescription NVARCHAR(255),
	CustomerFriendly NVARCHAR(255),
	BrandName NVARCHAR(255),
	RetailSize NVARCHAR(20),
	UOM NVARCHAR(4)
)

INSERT #tmpPriceLine(ItemId, ScanCode, PriceLine, PriceLineDescription, CustomerFriendly, BrandName, RetailSize, UOM)
SELECT item.itemid
	,scancode
	,JSON_VALUE(ItemAttributesJson, '$.PriceLine') AS PriceLine
	,JSON_VALUE(ItemAttributesJson, '$.PriceLineDescription') AS PriceLineDescription
	,JSON_VALUE(ItemAttributesJson, '$.CustomerFriendlyDescription') AS CustomerFriendly
	,HierarchyClass.hierarchyClassName AS BrandName
	,JSON_VALUE(ItemAttributesJson, '$.RetailSize') AS RetailSize
	,JSON_VALUE(ItemAttributesJson, '$.UOM') AS UOM
FROM item
JOIN scancode ON item.itemid = scancode.itemid
INNER JOIN ItemHierarchyClass ON ItemHierarchyClass.itemid = item.ItemId
INNER JOIN HierarchyClass ON ItemHierarchyClass.hierarchyClassID = HierarchyClass.hierarchyClassID
	AND HierarchyClass.HIERARCHYID = @BrandhierarchyId
WHERE JSON_VALUE(ItemAttributesJson, '$.PriceLine') IS NOT NULL
	OR JSON_VALUE(ItemAttributesJson, '$.PriceLineDescription') IS NOT NULL
UNION 
SELECT item.itemid
	,scancode
	,scancode AS PriceLine
	,NULL AS PriceLineDescription
	,JSON_VALUE(ItemAttributesJson, '$.CustomerFriendlyDescription') AS CustomerFriendly
	,HierarchyClass.hierarchyClassName AS BrandName
	,JSON_VALUE(ItemAttributesJson, '$.RetailSize') AS RetailSize
	,JSON_VALUE(ItemAttributesJson, '$.UOM') AS UOM
FROM item
JOIN scancode ON item.itemid = scancode.itemid
INNER JOIN ItemHierarchyClass ON ItemHierarchyClass.itemid = item.ItemId
INNER JOIN HierarchyClass ON ItemHierarchyClass.hierarchyClassID = HierarchyClass.hierarchyClassID
	AND HierarchyClass.HIERARCHYID = @BrandhierarchyId
WHERE JSON_VALUE(ItemAttributesJson, '$.PriceLine') IS NULL
	OR JSON_VALUE(ItemAttributesJson, '$.PriceLineDescription') IS NULL

INSERT INTO [dbo].[ItemGroup] (
	[ItemGroupTypeId]
	,[ItemGroupAttributesJson]
	,[LastModifiedBy]
	)
SELECT  @PriceLineItemGroupTypeId
	,(SELECT (BrandName +' '+ Replace(Replace(CustomerFriendly,'"',''),'\','\\')+ ' '+  RetailSize + ' '+ UOM) as PriceLineDescription ,RetailSize as PriceLineSize,UOM as PriceLineUOM FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
	,'Script'
FROM #tmpPriceLine
WHERE BrandName IS NOT NULL
	AND Convert(bigint,#tmpPriceLine.PriceLine) = Convert(bigint,#tmpPriceLine.scanCode)

INSERT INTO [dbo].[ItemGroupMember] (
	[ItemId]
	,[ItemGroupId]
	,[IsPrimary]
	,[LastModifiedBy]
	)
SELECT ItemId
	,ItemGroupId
	,CASE 
		WHEN Convert(bigint,scanCode) = Convert(bigint,PriceLine)
			THEN 1
		ELSE 0
		END
	,'Script'
FROM #tmpPriceLine
INNER JOIN ItemGroup ON JSON_VALUE(ItemGroup.ItemGroupAttributesJson, '$.PriceLineDescription') = BrandName + ' ' + CustomerFriendly + ' ' + RetailSize + ' ' + UOM

DROP TABLE #tmpPriceLine

DROP TABLE #tmSku

	INSERT INTO app.PostDeploymentScriptHistory (
		ScriptKey
		,RunTime
		)
	VALUES (
		@key
		,GetDate()
		);
END
ELSE
BEGIN
	PRINT '[' + Convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @key;
END
GO