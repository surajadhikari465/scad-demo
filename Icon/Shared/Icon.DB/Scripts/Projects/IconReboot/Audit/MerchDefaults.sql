SET NOCOUNT ON;

DECLARE @prohibitDiscountId int;
DECLARE @merchFinMappingId int;
DECLARE @nonMerchTrait int;

DECLARE @prohibitDiscountName nvarchar(25) = 'Prohibit Discount';
DECLARE @subTeam nvarchar(25) = 'Subteam';
DECLARE @itemType nvarchar(25) = 'Item Type';

SET @prohibitDiscountId = (SELECT traitID FROM Trait WHERE traitDesc = 'Prohibit Discount');
SET @merchFinMappingId = (SELECT traitID FROM Trait WHERE traitDesc = 'Merch Fin Mapping');
SET @nonMerchTrait = (SELECT traitID FROM Trait WHERE traitDesc = 'Non-Merchandise');

-- Prohibit Discount
SELECT
	br.hierarchyClassID AS NodeCode,
	brl.hierarchyLevelName AS LevelName,
	br.hierarchyClassName AS NodeName,
	@prohibitDiscountName AS ItemAttributeName,
	CASE WHEN hct.traitValue = 1 THEN 'true' ELSE 'false' END AS ItemAttributeDefaultValue
FROM dbo.HierarchyClass br
JOIN dbo.Hierarchy h on br.hierarchyID = h.hierarchyID
JOIN dbo.HierarchyPrototype brl on br.hierarchyID = brl.hierarchyID
	AND br.hierarchyLevel = brl.hierarchyLevel
JOIN dbo.HierarchyClassTrait hct on br.hierarchyClassID = hct.hierarchyClassID
	AND hct.traitID = @prohibitDiscountId
WHERE h.hierarchyName = 'Merchandise'
UNION ALL
-- SubTeam Association (e.g. Merch-Fin Mapping)
SELECT
	br.hierarchyClassID AS NodeCode,
	brl.hierarchyLevelName AS LevelName,
	br.hierarchyClassName AS NodeName,
	@subTeam AS ItemAttributeName,
	st.hierarchyClassName AS ItemAttributeDefaultValue
FROM dbo.HierarchyClass br
JOIN dbo.Hierarchy h on br.hierarchyID = h.hierarchyID
JOIN dbo.HierarchyPrototype brl on br.hierarchyID = brl.hierarchyID
	AND br.hierarchyLevel = brl.hierarchyLevel
JOIN dbo.HierarchyClassTrait hct on br.hierarchyClassID = hct.hierarchyClassID
	AND hct.traitID = @merchFinMappingId
JOIN dbo.HierarchyClass st on CAST(hct.traitValue AS int) = st.hierarchyClassID
WHERE h.hierarchyName = 'Merchandise'
UNION ALL
-- ItemType (e.g. Non-Merchandise Trait)
SELECT
	br.hierarchyClassID AS NodeCode,
	brl.hierarchyLevelName AS LevelName,
	br.hierarchyClassName AS NodeName,
	@itemType AS ItemAttributeName,
	hct.traitValue AS ItemAttributeDefaultValue
FROM dbo.HierarchyClass br
JOIN dbo.Hierarchy h on br.hierarchyID = h.hierarchyID
JOIN dbo.HierarchyPrototype brl on br.hierarchyID = brl.hierarchyID
	AND br.hierarchyLevel = brl.hierarchyLevel
JOIN dbo.HierarchyClassTrait hct on br.hierarchyClassID = hct.hierarchyClassID
	AND hct.traitID = @nonMerchTrait
JOIN dbo.ItemType it on hct.traitValue = it.itemTypeDesc
WHERE h.hierarchyName = 'Merchandise'