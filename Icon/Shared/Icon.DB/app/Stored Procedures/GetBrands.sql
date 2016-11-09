CREATE PROCEDURE [app].[GetBrands]
AS
BEGIN
-- ==================================================
-- Variables
-- ==================================================
DECLARE @brandHierarchyId int;
DECLARE @brandAbbreviationTraitId int;

SET @brandHierarchyId = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Brands');
SET @brandAbbreviationTraitId = (SELECT traitID FROM Trait WHERE traitDesc = 'Brand Abbreviation');

-- ==================================================
-- Main
-- ==================================================
SELECT
	hc.hierarchyClassID			as HierarchyClassID,
	hc.hierarchyClassName		as HierarchyClassName,
	hc.hierarchyID				as HierarchyID,
	hc.hierarchyLevel			as HierarchyLevel,
	hc.hierarchyParentClassId	as HierarchyParentClassId,
	hct.traitValue				as BrandAbbreviation
FROM
	HierarchyClass hc
	LEFT JOIN HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
										 AND hct.traitID = @brandAbbreviationTraitId
WHERE
	hc.hierarchyID = @brandHierarchyId
ORDER BY
	hc.hierarchyClassName
END
GO