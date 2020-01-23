SET NOCOUNT ON
SELECT
	hc5.hierarchyClassID 'Segment ID',
	hc5.hierarchyClassName 'Segment Name',
	hc4.hierarchyClassID 'Family ID',
	hc4.hierarchyClassName 'Family Name',
	hc3.hierarchyClassID 'Class ID',
	hc3.hierarchyClassName 'Class Name',
	hc2.hierarchyClassID 'GS1 Brick ID',
	hc2.hierarchyClassName 'GS1 Brick Name',
	hc.hierarchyClassID 'Sub Brick ID',
	hc.hierarchyClassName 'Sub Brick Name',
	hct.traitValue 'Sub Brick Code'
FROM
	HierarchyClass hc
	JOIN HierarchyClass hc2 ON hc.hierarchyParentClassID = hc2.hierarchyClassID
	JOIN HierarchyClass hc3 ON hc2.hierarchyParentClassID = hc3.hierarchyClassID
	JOIN HierarchyClass hc4 ON hc3.hierarchyParentClassID = hc4.hierarchyClassID
	JOIN HierarchyClass hc5 ON hc4.hierarchyParentClassID = hc5.hierarchyClassID
	LEFT JOIN HierarchyClassTrait hct ON hc.hierarchyClassID = hct.hierarchyClassID
	AND hct.traitID = 52
WHERE
	hc.hierarchyID = 1
ORDER BY
	hc5.hierarchyClassID,
	hc4.hierarchyClassID,
	hc3.hierarchyClassID,
	hc2.hierarchyClassID,
	hc.hierarchyClassID