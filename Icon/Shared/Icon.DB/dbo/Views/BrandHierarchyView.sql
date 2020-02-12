CREATE VIEW [dbo].[BrandHierarchyView]
AS
SELECT hc.HIERARCHYID AS HIERARCHYID
	,hc.hierarchyClassID AS HierarchyClassId
	,hc.hierarchyClassName AS HierarchyClassName
	,hc.hierarchyLevel AS HierarchyLevel
	,hc.hierarchyClassName + ' | ' + hct.traitValue AS HierarchyLineage
	,hc.hierarchyParentClassID AS HierarchyParentClassId
FROM HierarchyClass hc
JOIN Hierarchy h ON hc.HIERARCHYID = h.HIERARCHYID
JOIN HierarchyClassTrait hct ON hc.hierarchyClassID = hct.hierarchyClassID
	AND hct.traitID = (
		SELECT traitID
		FROM Trait
		WHERE TraitCode = 'BA'
		)
WHERE h.hierarchyName = 'Brands'