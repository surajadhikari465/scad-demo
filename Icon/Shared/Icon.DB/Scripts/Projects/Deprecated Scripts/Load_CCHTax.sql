/*
IF object_id('dbo.#CCHTemp') IS NOT NULL
   DROP TABLE #CCHTemp

CREATE TABLE #CCHTemp (
   taxclass nvarchar( 255 ));

BULK INSERT #CCHTemp
   FROM '\\irmadevfile\e$\ICONData\CCHTax.txt'

DELETE FROM itemhierarchyclass
WHERE  hierarchyclassid IN ( SELECT
                                hierarchyclassid
                             FROM
                                hierarchyclass
                             WHERE
                              hierarchyid = 3 )
DELETE FROM hierarchyclass
WHERE  hierarchyid = 3

DELETE FROM hierarchyprototype
WHERE  hierarchyid = 3

INSERT INTO hierarchyprototype
            (hierarchyid,
             hierarchylevel,
             hierarchylevelname,
             itemsattached)
VALUES      (3,
             1,
             'Tax',
             1)

INSERT INTO hierarchyclass
            ([hierarchyid],
             hierarchylevel,
             hierarchyclassname)
SELECT
   3        AS [hierarchyID],
   1        AS [hierarchylevel],
   taxclass AS [hierarchyClassName]
FROM
   #CCHTemp

DROP TABLE #CCHTemp

SELECT
   *
FROM
   hierarchyclass
WHERE
   hierarchyid = 3 
*/