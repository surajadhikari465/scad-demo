CREATE VIEW [dbo].[BrandHierarchyView]
AS
SELECT hc.HIERARCHYID AS HIERARCHYID
	,hc.hierarchyClassID AS HierarchyClassId
	,hc.hierarchyClassName AS HierarchyClassName
	,hc.hierarchyLevel AS HierarchyLevel
	,hc.hierarchyClassName AS HierarchyLineage
	,hc.hierarchyParentClassID AS HierarchyParentClassId
FROM HierarchyClass hc
JOIN Hierarchy h ON hc.HIERARCHYID = h.HIERARCHYID
WHERE h.hierarchyName = 'Brands'