
CREATE VIEW [app].[DW_PLUMap]
AS
SELECT 'FL' AS REGION,
	plu.flplu AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.flPLU IS NOT NULL

UNION

SELECT 'MA' AS REGION,
	plu.maplu AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.maPLU IS NOT NULL

UNION

SELECT 'MW' AS REGION,
	plu.mwPLU AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.mwPLU IS NOT NULL

UNION

SELECT 'NA' AS REGION,
	plu.naplu AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.naPLU IS NOT NULL

UNION

SELECT 'NC' AS REGION,
	plu.ncPLU AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.ncPLU IS NOT NULL

UNION

SELECT 'NE' AS REGION,
	plu.neplu AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.nePLU IS NOT NULL

UNION

SELECT 'PN' AS REGION,
	plu.pnplu AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.pnPLU IS NOT NULL

UNION

SELECT 'RM' AS REGION,
	plu.rmplu AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.rmPLU IS NOT NULL

UNION

SELECT 'SO' AS REGION,
	plu.soplu AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.soPLU IS NOT NULL

UNION

SELECT 'SP' AS REGION,
	plu.spplu AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.spPLU IS NOT NULL

UNION

SELECT 'SW' AS REGION,
	plu.swplu AS [PLU],
	sc.scancode AS [Scan Code]
FROM scancode sc
JOIN app.PLUMap plu ON sc.itemid = plu.itemid
WHERE plu.swPLU IS NOT NULL

GO
