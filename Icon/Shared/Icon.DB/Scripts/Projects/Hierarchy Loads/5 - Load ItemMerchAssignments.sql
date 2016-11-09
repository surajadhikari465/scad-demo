-- should be run after the Load GS1.sql, Load Fin.sql, Load Map.sql scripts have been run

CREATE TABLE #ItemMerchTemp (
	[upc] NVARCHAR(255),
	[Sub Brick Code] NVARCHAR(255)
	);

BULK INSERT #ItemMerchTemp
FROM '\\irmadevfile\e$\icondata\ItemMerchAssignment.txt' WITH (FirstRow = 2)

UPDATE #ItemMerchTemp
SET [upc] = SUBSTRING([upc], PATINDEX('%[^0 ]%', [upc] + ' '), LEN([upc]))

SELECT count(upc)
FROM #ItemMerchTemp
WHERE [Sub Brick Code] IS NOT NULL

INSERT INTO ItemHierarchyClass
SELECT sc.itemID,
	sbc.hierarchyClassID,
	'1' AS localeid
FROM #ItemMerchTemp imt
JOIN ScanCode sc ON imt.upc = sc.scanCode
JOIN (
	SELECT hct.hierarchyClassID,
		hct.traitValue
	FROM HierarchyClassTrait hct
	JOIN trait t ON hct.traitID = t.traitID
		AND traitcode = 'sbc'
	) sbc ON imt.[Sub Brick Code] = sbc.traitValue
WHERE imt.[Sub Brick Code] IS NOT NULL

DROP TABLE #ItemMerchTemp