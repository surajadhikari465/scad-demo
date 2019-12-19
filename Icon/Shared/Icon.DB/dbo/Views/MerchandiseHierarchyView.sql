CREATE VIEW [dbo].[MerchandiseHierarchyView]
AS
SELECT hclv5.HIERARCHYID AS HIERARCHYID
	,hclv5.hierarchyClassID AS HierarchyClassId
	,hclv5.hierarchyClassName AS HierarchyClassName
	,hclv5.hierarchyLevel AS HierarchyLevel
	,hclv1.hierarchyClassName + ' | ' + hclv2.hierarchyClassName + ' | ' + hclv3.hierarchyClassName + ' | ' + hclv4.hierarchyClassName + ' | ' + hclv5.hierarchyClassName + ': ' + nch.hierarchyClassName AS HierarchyLineage
	,hclv2.hierarchyParentClassID AS HierarchyParentClassId
FROM HierarchyClass hclv5
JOIN HierarchyClass hclv4 ON hclv5.hierarchyParentClassID = hclv4.hierarchyClassID
JOIN HierarchyClass hclv3 ON hclv4.hierarchyParentClassID = hclv3.hierarchyClassID
JOIN HierarchyClass hclv2 ON hclv3.hierarchyParentClassID = hclv2.hierarchyClassID
JOIN HierarchyClass hclv1 ON hclv2.hierarchyParentClassID = hclv1.hierarchyClassID
JOIN HierarchyClassTrait hct ON hclv5.hierarchyClassID = hct.hierarchyClassID
	AND hct.traitID = (
		SELECT traitID
		FROM Trait
		WHERE TraitCode = 'MFM'
		)
INNER JOIN dbo.HierarchyClass nch ON nch.hierarchyClassID = hct.traitValue
JOIN Hierarchy h ON hclv5.HIERARCHYID = h.HIERARCHYID
WHERE h.hierarchyName = 'Merchandise'