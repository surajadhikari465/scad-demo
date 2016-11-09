/*
IF object_id('dbo.##GS1Temp') IS NOT NULL
   DROP TABLE ##GS1Temp

CREATE TABLE ##GS1Temp (
   [segment code]                     nvarchar( 255 ),
   [segment description]              nvarchar( 255 ),
   [family code]                      nvarchar( 255 ),
   [family description]               nvarchar( 255 ),
   [class code]                       nvarchar( 255 ),
   [class description]                nvarchar( 255 ),
   [brick code]                       nvarchar( 255 ),
   [brick description]                nvarchar( 255 ),
   [core attribute type code]         nvarchar( 255 ),
   [core attribute  type description] nvarchar( 255 ),
   [core attribute value code]        nvarchar( 255 ),
   [core attribute value description] nvarchar( 255 ));

BULK INSERT ##GS1Temp
   FROM '\\irmadevfile\e$\ICONData\GS1.txt'
   WITH
      (
         FirstRow = 2
      )
DECLARE @SegmentTemp TABLE (
   hierarchyclassname     varchar( 255 ),
   hierarchylevel         int,
   hierarchyid            int,
   gsid                   varchar( 255 ),
   hierarchyparentclassid int)
DECLARE @SegmentOutputTemp TABLE (
   hierarchyclassid   int,
   gsid               varchar( 255 ),
   hierarchyclassname varchar( 255 ))
DECLARE @FamilyTemp TABLE (
   hierarchyclassname     varchar( 255 ),
   hierarchylevel         int,
   hierarchyid            int,
   gsid                   varchar( 255 ),
   hierarchyparentclassid int)
DECLARE @FamilyOutputTemp TABLE (
   hierarchyclassid   int,
   gsid               varchar( 255 ),
   hierarchyclassname varchar( 255 ))
DECLARE @ClassTemp TABLE (
   hierarchyclassname     varchar( 255 ),
   hierarchylevel         int,
   hierarchyid            int,
   gsid                   varchar( 255 ),
   hierarchyparentclassid int)
DECLARE @ClassOutputTemp TABLE (
   hierarchyclassid   int,
   gsid               varchar( 255 ),
   hierarchyclassname varchar( 255 ))
DECLARE @BrickTemp TABLE (
   hierarchyclassname     varchar( 255 ),
   hierarchylevel         int,
   hierarchyid            int,
   gsid                   varchar( 255 ),
   hierarchyparentclassid int)
DECLARE @BrickOutputTemp TABLE (
   hierarchyclassid   int,
   gsid               varchar( 255 ),
   hierarchyclassname varchar( 255 ))
DECLARE @AttributeTemp TABLE (
   hierarchyclassname     varchar( 255 ),
   hierarchylevel         int,
   hierarchyid            int,
   gsid                   varchar( 255 ),
   hierarchyparentclassid int)
DECLARE @AttributeOutputTemp TABLE (
   hierarchyclassid   int,
   gsid               varchar( 255 ),
   hierarchyclassname varchar( 255 ))
DECLARE @ValueTemp TABLE (
   hierarchyclassname     varchar( 255 ),
   hierarchylevel         int,
   hierarchyid            int,
   gsid                   varchar( 255 ),
   hierarchyparentclassid int)
DECLARE @ValueOutputTemp TABLE (
   hierarchyclassid   int,
   gsid               varchar( 255 ),
   hierarchyclassname varchar( 255 ))

DELETE FROM hierarchyclass
WHERE  hierarchyid = 1

DELETE FROM hierarchyprototype
WHERE  hierarchyid = 1

INSERT INTO hierarchyprototype
VALUES     (1,
            1,
            'Family',
            1)

INSERT INTO hierarchyprototype
VALUES     (1,
            2,
            'Segment',
            1)

INSERT INTO hierarchyprototype
VALUES     (1,
            3,
            'Class',
            1)

INSERT INTO hierarchyprototype
VALUES     (1,
            4,
            'Brick',
            1)

INSERT INTO @SegmentTemp
SELECT
   hierarchyclassname,
   hierarchylevel,
   hierarchyid,
   gsid,
   hierarchyparentclassid
FROM
   ( SELECT
        DISTINCT
        [segment description] AS hierarchyClassName,
        1                     AS hierarchyLevel,
        1                     AS hierarchyID,
        [segment code]        AS gsID,
        NULL                  AS hierarchyParentClassID
     FROM
        ##GS1Temp gs1
     WHERE
      [segment description] IS NOT NULL
      AND [segment code] = 50000000
      AND [family code]   = 50250000 ) segment

-- Populate the HierarchyClass table with Segment data
MERGE INTO hierarchyclass
USING @SegmentTemp AS st
ON 1 = 0
WHEN NOT MATCHED THEN
   INSERT ( hierarchylevel,
            hierarchyid,
            hierarchyparentclassid,
            hierarchyclassname)
   VALUES ( 1,
            1,
            NULL,
            st.hierarchyclassname)
OUTPUT inserted.hierarchyclassid,
       st.gsid,
       st.hierarchyclassname
INTO @SegmentOutputTemp (hierarchyClassID, gsID, hierarchyClassName);

--Setting up @FamilyTemp table 
INSERT INTO @FamilyTemp
SELECT
   hierarchyclassname,
   hierarchylevel,
   hierarchyid,
   gsid,
   hierarchyparentclassid
FROM
   ( SELECT
        DISTINCT
        [family description] AS hierarchyClassName,
        2                    AS hierarchyLevel,
        1                    AS hierarchyID,
        [family code]        AS gsID,
        st.hierarchyclassid  AS hierarchyParentClassID
     FROM
        ##GS1Temp gs1
     INNER JOIN @SegmentOutputTemp st ON st.gsid = gs1.[segment code] ) family

-- Populate zzHierarchyClass table with Family data
MERGE INTO hierarchyclass
USING @FamilyTemp AS ft
ON 1 = 0
WHEN NOT MATCHED THEN
   INSERT ( hierarchylevel,
            hierarchyid,
            hierarchyparentclassid,
            hierarchyclassname)
   VALUES ( 2,
            1,
            ft.hierarchyparentclassid,
            ft.hierarchyclassname)
OUTPUT inserted.hierarchyclassid,
       ft.gsid,
       ft.hierarchyclassname
INTO @FamilyOutputTemp (hierarchyClassID, gsID, hierarchyClassName);

--Setting up @ClassTemp table 
INSERT INTO @ClassTemp
SELECT
   hierarchyclassname,
   hierarchylevel,
   hierarchyid,
   gsid,
   hierarchyparentclassid
FROM
   ( SELECT
        DISTINCT
        [class description] AS hierarchyClassName,
        2                   AS hierarchyLevel,
        1                   AS hierarchyID,
        [class code]        AS gsID,
        ft.hierarchyclassid AS hierarchyParentClassID
     FROM
        ##GS1Temp gs1
     INNER JOIN @FamilyOutputTemp ft ON ft.gsid = gs1.[family code] ) class

-- Populate zzHierarchyClass table with Class data
MERGE INTO hierarchyclass
USING @ClassTemp AS ct
ON 1 = 0
WHEN NOT MATCHED THEN
   INSERT ( hierarchylevel,
            hierarchyid,
            hierarchyparentclassid,
            hierarchyclassname)
   VALUES ( 3,
            1,
            ct.hierarchyparentclassid,
            ct.hierarchyclassname)
OUTPUT inserted.hierarchyclassid,
       ct.gsid,
       ct.hierarchyclassname
INTO @ClassOutputTemp (hierarchyClassID, gsID, hierarchyClassName);

--Setting up @BrickTemp table 
INSERT INTO @BrickTemp
SELECT
   hierarchyclassname,
   hierarchylevel,
   hierarchyid,
   gsid,
   hierarchyparentclassid
FROM
   ( SELECT
        DISTINCT
        [brick description] AS hierarchyClassName,
        2                   AS hierarchyLevel,
        1                   AS hierarchyID,
        [brick code]        AS gsID,
        ct.hierarchyclassid AS hierarchyParentClassID
     FROM
        ##GS1Temp gs1
     INNER JOIN @ClassOutputTemp ct ON ct.gsid = gs1.[class code] ) brick

-- Populate zzHierarchyClass table with Brick data
MERGE INTO hierarchyclass
USING @BrickTemp AS bt
ON 1 = 0
WHEN NOT MATCHED THEN
   INSERT ( hierarchylevel,
            hierarchyid,
            hierarchyparentclassid,
            hierarchyclassname)
   VALUES ( 4,
            1,
            bt.hierarchyparentclassid,
            bt.hierarchyclassname)
OUTPUT inserted.hierarchyclassid,
       bt.gsid,
       bt.hierarchyclassname
INTO @BrickOutputTemp (hierarchyClassID, gsID, hierarchyClassName);

GO

DROP TABLE ##GS1Temp

GO 

*/