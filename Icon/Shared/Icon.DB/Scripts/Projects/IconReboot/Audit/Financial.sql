SET NOCOUNT ON
SELECT
	hc.hierarchyClassID 'Financial Hierarchy ID',
	SUBSTRING(
		hc.hierarchyClassName,
		CHARINDEX('(', hc.hierarchyClassName) + 1,
		4
	) 'Subteam',
	hc.hierarchyClassName 'Subteam Name'
FROM
	HierarchyClass hc
	JOIN Hierarchy h ON hc.hierarchyID = h.hierarchyID
WHERE
	h.hierarchyName = 'Financial'
ORDER BY
	hc.hierarchyClassID