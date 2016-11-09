-- Should be run on UDM 3.0 (or after HCT table is present)
-- Load Flat File
IF object_id('dbo.#CCHTemp') IS NOT NULL
	DROP TABLE #CCHTemp
CREATE TABLE #CCHTemp (
	taxclassabbrev NVARCHAR(255),
	taxclasslong NVARCHAR(255)
	);
BULK INSERT #CCHTemp
FROM '\\irmadevfile\e$\ICONData\CCHTax.txt' WITH (FirstRow = 2)

-- Show 'Before' 
SELECT * INTO #TaxClassBefore
FROM #CCHTemp foo
LEFT JOIN ( SELECT * FROM hierarchyclass WHERE HIERARCHYID = 3 ) bar 
ON LEFT(foo.taxclasslong, 7) = LEFT(bar.hierarchyClassName, 7)

SELECT * FROM #TaxClassBefore

-- Begin Merge
MERGE INTO hierarchyclass hc
USING #CCHTemp tax
	ON LEFT(tax.taxclasslong, 7) = LEFT(hc.hierarchyClassName, 7)
WHEN MATCHED THEN
		UPDATE SET hierarchyclassname = tax.taxclasslong
WHEN NOT MATCHED THEN
		INSERT ( hierarchylevel, HIERARCHYID, hierarchyclassname )
		VALUES ( 1, 3, tax.taxclasslong );

-- Show 'After'
SELECT * INTO #TaxClassAfter FROM #CCHTemp foo
LEFT JOIN ( SELECT * FROM hierarchyclass WHERE HIERARCHYID = 3 ) bar 
ON LEFT(foo.taxclasslong, 7) = LEFT(bar.hierarchyClassName, 7)

SELECT * FROM #TaxClassAfter

---- Put Tax Class Abbreviations into HCT 
MERGE INTO hierarchyclasstrait hct
USING #TaxClassAfter tca
ON tca.hierarchyclassid = hct.hierarchyclassid and hct.traitid = (select traitid from trait where traitcode='ABR')
WHEN MATCHED THEN
UPDATE SET hct.traitvalue = tca.taxclassabbrev
WHEN NOT MATCHED THEN
   INSERT (traitid, hierarchyClassID, traitValue)
   VALUES ((select traitid from trait where traitcode='ABR'), tca.hierarchyclassid, tca.taxclassabbrev);

-- Select HC and HCT to check our work
SELECT * 
FROM HierarchyClass hc
JOIN HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
WHERE hc.hierarchyID = 3

-- Clean Up
DROP TABLE #CCHTemp, #TaxClassBefore, #TaxClassAfter
