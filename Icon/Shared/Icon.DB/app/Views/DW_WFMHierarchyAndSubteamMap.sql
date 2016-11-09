
CREATE VIEW [app].[DW_WFMHierarchyAndSubteamMap]
AS
SELECT shc.hierarchyClassID AS [segment hierarchyclassid],
	shc.hierarchyClassName AS [segment hierarchyclassname],
	fhc.hierarchyClassID AS [family hierarchyclassid],
	fhc.hierarchyClassName AS [family hierarchyclassname],
	chc.hierarchyClassID AS [class hierarchyclassid],
	chc.hierarchyClassName AS [class hierarchyclassname],
	ghc.hierarchyClassID AS [gs1 brick hierarchyclassid],
	ghc.hierarchyClassName AS [gs1 brick hierarchyclassname],
	bhc.hierarchyClassID AS [subbrick hierarchyclassid],
	bhc.hierarchyClassName AS [sub brick hierarchyclassname],
	sbc.traitvalue AS [subbrick code],
	SUBSTRING(mfm.traitValue, CHARINDEX('(', mfm.traitValue) + 1, CHARINDEX(')', mfm.traitValue, CHARINDEX('(', mfm.traitValue) - 1) - CHARINDEX('(', mfm.traitValue) - 1) AS [subteam no]
FROM hierarchyclass shc
LEFT JOIN hierarchyclass fhc ON shc.hierarchyClassID = fhc.hierarchyParentClassID
	AND fhc.HIERARCHYID = 1
	AND fhc.hierarchyLevel = 2
LEFT JOIN hierarchyclass chc ON fhc.hierarchyClassID = chc.hierarchyParentClassID
	AND chc.HIERARCHYID = 1
	AND chc.hierarchyLevel = 3
LEFT JOIN hierarchyclass ghc ON chc.hierarchyClassID = ghc.hierarchyParentClassID
	AND ghc.HIERARCHYID = 1
	AND ghc.hierarchyLevel = 4
LEFT JOIN hierarchyclass bhc ON ghc.hierarchyClassID = bhc.hierarchyParentClassID
	AND bhc.HIERARCHYID = 1
	AND bhc.hierarchyLevel = 5
LEFT JOIN hierarchyclasstrait sbc ON bhc.hierarchyClassID = sbc.hierarchyClassID
	AND sbc.traitID = 52
LEFT JOIN hierarchyclasstrait mfm ON bhc.hierarchyClassID = mfm.hierarchyClassID
	AND mfm.traitID = 49
WHERE shc.HIERARCHYID = 1
	AND shc.hierarchyLevel = 1
	AND sbc.hierarchyClassID IS NOT NULL

GO


