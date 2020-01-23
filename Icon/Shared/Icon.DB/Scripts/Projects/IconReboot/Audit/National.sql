SET NOCOUNT ON
SELECT
	hc4.hierarchyClassID 'Family ID',
	hc4.hierarchyClassName 'Family Name',
	hc3.hierarchyClassID 'Category ID',
	hc3.hierarchyClassName 'Category Name',
	hc2.hierarchyClassID 'Sub Category ID',
	hc2.hierarchyClassName 'Sub Category Name',
	hc.hierarchyClassID 'Class ID',
	hc.hierarchyClassName 'Class Name',
	hct.traitValue 'National Class Code'
FROM
	HierarchyClass hc
	JOIN HierarchyClass hc2 ON hc.hierarchyParentClassID = hc2.hierarchyClassID
	JOIN HierarchyClass hc3 ON hc2.hierarchyParentClassID = hc3.hierarchyClassID
	JOIN HierarchyClass hc4 ON hc3.hierarchyParentClassID = hc4.hierarchyClassID
	LEFT JOIN HierarchyClassTrait hct ON hc.hierarchyClassID = hct.hierarchyClassID
	AND hct.traitID = 69
WHERE
	hc.hierarchyID = 6
ORDER BY
	hc4.hierarchyClassID,
	hc3.hierarchyClassID,
	hc2.hierarchyClassID,
	hc.hierarchyClassID