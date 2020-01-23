SET NOCOUNT ON
SELECT
	hc.hierarchyClassID 'Tax Hierarchy ID',
	substring(hc.hierarchyClassName, 0, 8) 'Tax Class ID',
	hc.hierarchyClassName 'Tax Class',
	CASE
		WHEN abr.traitValue IS NULL THEN 'NULL'
		ELSE abr.traitValue
	END 'Tax Abbreviation',
	CASE
		WHEN rom.traitValue IS NULL THEN 'NULL'
		ELSE rom.traitValue
	END 'Tax Romance'
FROM
	HierarchyClass hc
	JOIN Hierarchy h ON hc.hierarchyID = h.hierarchyID
	LEFT JOIN HierarchyClassTrait abr ON hc.hierarchyClassID = abr.hierarchyClassID
	AND abr.traitID = 51
	LEFT JOIN HierarchyClassTrait rom ON hc.hierarchyClassID = rom.hierarchyClassID
	AND rom.traitID = 67
WHERE
	h.hierarchyName = 'Tax'
ORDER BY
	hc.hierarchyLevel,
	hc.hierarchyClassID