CREATE VIEW [dbo].[NationalClassHierarchyView]
AS
SELECT hclv4.HIERARCHYID AS HIERARCHYID
	,hclv4.hierarchyClassID AS HierarchyClassId
	,hclv4.hierarchyClassName AS HierarchyClassName
	,hclv4.hierarchyLevel AS HierarchyLevel
	,hclv1.hierarchyClassName + ' | ' + hclv2.hierarchyClassName + ' | ' + hclv3.hierarchyClassName + ' | ' + hclv4.hierarchyClassName + ': ' + hct.traitValue AS HierarchyLineage
	,hclv4.hierarchyParentClassID AS HierarchyParentClassId
FROM HierarchyClass hclv4
JOIN HierarchyClass hclv3 ON hclv4.hierarchyParentClassID = hclv3.hierarchyClassID
JOIN HierarchyClass hclv2 ON hclv3.hierarchyParentClassID = hclv2.hierarchyClassID
JOIN HierarchyClass hclv1 ON hclv2.hierarchyParentClassID = hclv1.hierarchyClassID
JOIN HierarchyClassTrait hct ON hclv4.hierarchyClassID = hct.hierarchyClassID
	AND hct.traitID = (
		SELECT TRAITID
		FROM Trait
		WHERE traitCode = 'NCC'
		)
JOIN Hierarchy h ON hclv4.HIERARCHYID = h.HIERARCHYID
WHERE h.hierarchyName = 'National'