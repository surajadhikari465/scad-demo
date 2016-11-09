/*

IF object_id('dbo.##BrowsingTemp') IS NOT NULL
   DROP TABLE ##BrowsingTemp

CREATE TABLE ##BrowsingTemp (
   [parent code]        nvarchar( 255 ),
   [parent description] nvarchar( 255 ),
   [sub code]           nvarchar( 255 ),
   [sub description]    nvarchar( 255 ),
   [subsub code]        nvarchar( 255 ),
   [subsub description] nvarchar( 255 ));

BULK INSERT ##BrowsingTemp
   FROM '\\irmadevfile\e$\ICONData\Browsing.txt'
   WITH( FirstRow = 2 )
SELECT * FROM ##BrowsingTemp

DECLARE @ParentTemp TABLE (
   hierarchyclassname     varchar( 255 ),
   hierarchylevel         int,
   hierarchyid            int,
   browsingid             varchar( 255 ),
   hierarchyparentclassid int)
DECLARE @ParentOutputTemp TABLE (
   hierarchyclassid   int,
   browsingid         varchar( 255 ),
   hierarchyclassname varchar( 255 ))
DECLARE @SubTemp TABLE (
   hierarchyclassname     varchar( 255 ),
   hierarchylevel         int,
   hierarchyid            int,
   browsingid             varchar( 255 ),
   hierarchyparentclassid int)
DECLARE @SubOutputTemp TABLE (
   hierarchyclassid   int,
   browsingid         varchar( 255 ),
   hierarchyclassname varchar( 255 ))
DECLARE @SubSubTemp TABLE (
   hierarchyclassname     varchar( 255 ),
   hierarchylevel         int,
   hierarchyid            int,
   browsingid             varchar( 255 ),
   hierarchyparentclassid int)
DECLARE @SubSubOutputTemp TABLE (
   hierarchyclassid   int,
   browsingid         varchar( 255 ),
   hierarchyclassname varchar( 255 ))

DELETE FROM itemhierarchyclass
WHERE  hierarchyclassid IN ( SELECT
                                hierarchyclassid
                             FROM
                                hierarchyclass
                             WHERE
                              hierarchyid = 4 )

DELETE FROM hierarchyclass
WHERE  hierarchyid = 4

DELETE FROM hierarchyprototype
WHERE  hierarchyid = 4

INSERT INTO hierarchyprototype
VALUES     (4,
            1,
            'Parent',
            1)

INSERT INTO hierarchyprototype
VALUES     (4,
            2,
            'Sub',
            1)

INSERT INTO hierarchyprototype
VALUES     (4,
            3,
            'Sub, Sub',
            1)

INSERT INTO @ParentTemp
SELECT
   hierarchyclassname,
   hierarchylevel,
   hierarchyid,
   browsingid,
   hierarchyparentclassid
FROM
   ( SELECT
        DISTINCT
        [parent description] AS hierarchyClassName,
        1                    AS hierarchyLevel,
        4                    AS hierarchyID,
        [parent code]        AS BrowsingID,
        NULL                 AS hierarchyParentClassID
     FROM
        ##BrowsingTemp gs1
     WHERE
      [parent description] IS NOT NULL ) Parent

-- Populate the HierarchyClass table with Parent data
MERGE INTO hierarchyclass
USING @ParentTemp AS st
ON 1 = 0
WHEN NOT MATCHED THEN
   INSERT ( hierarchylevel,
            hierarchyid,
            hierarchyparentclassid,
            hierarchyclassname)
   VALUES ( 1,
            4,
            NULL,
            st.hierarchyclassname)
OUTPUT inserted.hierarchyclassid,
       st.browsingid,
       st.hierarchyclassname
INTO @ParentOutputTemp (hierarchyClassID, BrowsingID, hierarchyClassName);

--Setting up @SubTemp table 
INSERT INTO @SubTemp
SELECT
   hierarchyclassname,
   hierarchylevel,
   hierarchyid,
   browsingid,
   hierarchyparentclassid
FROM
   ( SELECT
        DISTINCT
        [sub description]   AS hierarchyClassName,
        2                   AS hierarchyLevel,
        4                   AS hierarchyID,
        [sub code]          AS BrowsingID,
        st.hierarchyclassid AS hierarchyParentClassID
     --FROM zzGS1Test gs1
     FROM
        ##BrowsingTemp gs1
     INNER JOIN @ParentOutputTemp st ON st.browsingid = gs1.[parent code] ) family

MERGE INTO hierarchyclass
USING @SubTemp AS ft
ON 1 = 0
WHEN NOT MATCHED THEN
   INSERT ( hierarchylevel,
            hierarchyid,
            hierarchyparentclassid,
            hierarchyclassname)
   VALUES ( 2,
            4,
            ft.hierarchyparentclassid,
            ft.hierarchyclassname)
OUTPUT inserted.hierarchyclassid,
       ft.browsingid,
       ft.hierarchyclassname
INTO @SubOutputTemp (hierarchyClassID, BrowsingID, hierarchyClassName);

--Setting up @SubSubTemp table 
INSERT INTO @SubSubTemp
SELECT
   hierarchyclassname,
   hierarchylevel,
   hierarchyid,
   browsingid,
   hierarchyparentclassid
FROM
   ( SELECT
        DISTINCT
        [subsub description] AS hierarchyClassName,
        2                    AS hierarchyLevel,
        4                    AS hierarchyID,
        [subsub code]        AS BrowsingID,
        ft.hierarchyclassid  AS hierarchyParentClassID
     FROM
        ##BrowsingTemp gs1
     INNER JOIN @SubOutputTemp ft ON ft.browsingid = gs1.[subsub code] ) class

GO

DROP TABLE ##BrowsingTemp

GO

SELECT
   *
FROM
   hierarchyclass
WHERE
   hierarchyid = 4
ORDER  BY
   hierarchyclassname,hierarchyclassid,hierarchylevel 
*/