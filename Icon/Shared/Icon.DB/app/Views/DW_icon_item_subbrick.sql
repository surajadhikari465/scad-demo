
CREATE VIEW [app].[DW_icon_item_subbrick]
AS
SELECT RIGHT('0000000000000' + sc.scanCode, 13) AS scancode,
	isnull(sbc.traitValue, '') AS [sub brick],
   sbc.hierarchyClassID as [subbrick hierarchyclassid]
FROM scancode sc
LEFT JOIN (
	SELECT ihc.itemID,
		hc.hierarchyclassname,
		hc.hierarchyclassid
	FROM itemhierarchyclass ihc
	JOIN HierarchyClass hc ON ihc.hierarchyclassid = hc.hierarchyclassid
		AND hc.HIERARCHYID = 1
	) merch ON sc.itemID = merch.itemID
LEFT JOIN (
	SELECT hc.hierarchyClassID,
		sbc.traitValue
	FROM HierarchyClass hc
	JOIN HierarchyClassTrait sbc ON hc.hierarchyClassID = sbc.hierarchyClassID
	JOIN trait t ON sbc.traitid = t.traitid
		AND t.traitcode = 'sbc'
	WHERE hc.HIERARCHYID = 1
	) sbc ON merch.hierarchyClassID = sbc.hierarchyClassID

GO
