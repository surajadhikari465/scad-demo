SET NOCOUNT ON

SELECT
	hc.hierarchyClassName 'Brand Name',
	CASE
		WHEN ba.traitValue IS NULL THEN ''
		ELSE ba.traitValue
	END 'Brand Abbreviation',
	CASE
		WHEN dgn.traitValue IS NULL THEN ''
		ELSE dgn.traitValue
	END 'Ownership Level',
	hc.hierarchyClassID 'Brand ID'
FROM
	HierarchyClass hc
	JOIN Hierarchy h ON hc.hierarchyID = h.hierarchyID
	LEFT JOIN HierarchyClassTrait ba ON hc.hierarchyClassID = ba.hierarchyClassID
		AND ba.traitID = 66 --trait ID for brand abbreviation
	LEFT JOIN HierarchyClassTrait dgn ON hc.hierarchyClassID = dgn.hierarchyClassID
		AND dgn.traitID = 188
WHERE
	h.hierarchyName = 'Brands'
ORDER BY
	hc.hierarchyClassID