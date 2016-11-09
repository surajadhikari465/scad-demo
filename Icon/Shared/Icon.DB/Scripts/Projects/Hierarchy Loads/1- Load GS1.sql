-- 
DELETE
FROM HierarchyPrototype
WHERE HIERARCHYID = 1
INSERT INTO HierarchyPrototype (
	HIERARCHYID,
	hierarchyLevel,
	hierarchyLevelName,
	itemsAttached
	)
VALUES (
	1,
	1,
	'Segment',
	1
	)
INSERT INTO HierarchyPrototype (
	HIERARCHYID,
	hierarchyLevel,
	hierarchyLevelName,
	itemsAttached
	)
VALUES (
	1,
	2,
	'Family',
	1
	)
INSERT INTO HierarchyPrototype (
	HIERARCHYID,
	hierarchyLevel,
	hierarchyLevelName,
	itemsAttached
	)
VALUES (
	1,
	3,
	'Class',
	1
	)
INSERT INTO HierarchyPrototype (
	HIERARCHYID,
	hierarchyLevel,
	hierarchyLevelName,
	itemsAttached
	)
VALUES (
	1,
	4,
	'GS1 Brick',
	1
	)
INSERT INTO HierarchyPrototype (
	HIERARCHYID,
	hierarchyLevel,
	hierarchyLevelName,
	itemsAttached
	)
VALUES (
	1,
	5,
	'Sub Brick',
	1
	)
CREATE TABLE ##GS1Temp (
	[Segment Code] NVARCHAR(255),
	[Segment Description] NVARCHAR(255),
	[Family Code] NVARCHAR(255),
	[Family Description] NVARCHAR(255),
	[Class Code] NVARCHAR(255),
	[Class Description] NVARCHAR(255),
	[GS1 Brick Code] NVARCHAR(255),
	[GS1 Brick Description] NVARCHAR(255),
	[Sub Brick Code] NVARCHAR(255),
	[Sub Brick Description] NVARCHAR(255)
	);
BULK INSERT ##GS1Temp
FROM '\\irmadevfile\e$\icondata\WFMMerchHier.txt' WITH (FirstRow = 2)
DECLARE @SegmentTemp TABLE (
	hierarchyClassName VARCHAR(255),
	hierarchyLevel INT,
	HIERARCHYID INT,
	gsID VARCHAR(255),
	hierarchyParentClassID INT
	)
DECLARE @SegmentOutputTemp TABLE (
	hierarchyClassID INT,
	gsID VARCHAR(255),
	hierarchyClassName VARCHAR(255)
	)
DECLARE @FamilyTemp TABLE (
	hierarchyClassName VARCHAR(255),
	hierarchyLevel INT,
	HIERARCHYID INT,
	gsID VARCHAR(255),
	hierarchyParentClassID INT
	)
DECLARE @FamilyOutputTemp TABLE (
	hierarchyClassID INT,
	gsID VARCHAR(255),
	hierarchyClassName VARCHAR(255)
	)
DECLARE @ClassTemp TABLE (
	hierarchyClassName VARCHAR(255),
	hierarchyLevel INT,
	HIERARCHYID INT,
	gsID VARCHAR(255),
	hierarchyParentClassID INT
	)
DECLARE @ClassOutputTemp TABLE (
	hierarchyClassID INT,
	gsID VARCHAR(255),
	hierarchyClassName VARCHAR(255)
	)
DECLARE @BrickTemp TABLE (
	hierarchyClassName VARCHAR(255),
	hierarchyLevel INT,
	HIERARCHYID INT,
	gsID VARCHAR(255),
	hierarchyParentClassID INT
	)
DECLARE @BrickOutputTemp TABLE (
	hierarchyClassID INT,
	gsID VARCHAR(255),
	hierarchyClassName VARCHAR(255)
	)
DECLARE @SubBrickTemp TABLE (
	hierarchyClassName VARCHAR(255),
	hierarchyLevel INT,
	HIERARCHYID INT,
	gsID VARCHAR(255),
	hierarchyParentClassID INT
	)
DECLARE @SubBrickOutputTemp TABLE (
	hierarchyClassID INT,
	gsID VARCHAR(255),
	hierarchyClassName VARCHAR(255)
	)
INSERT INTO @SegmentTemp
SELECT hierarchyClassName,
	hierarchyLevel,
	HIERARCHYID,
	gsID,
	hierarchyParentClassID
FROM (
	SELECT DISTINCT [Segment Description] AS hierarchyClassName,
		1 AS hierarchyLevel,
		1 AS HIERARCHYID,
		[Segment Code] AS gsID,
		NULL AS hierarchyParentClassID
	FROM ##GS1Temp gs1
	WHERE [Segment Description] IS NOT NULL
	) segment
MERGE INTO HierarchyClass
USING @SegmentTemp AS st
	ON 1 = 0
WHEN NOT MATCHED
	THEN
		INSERT (
			hierarchyLevel,
			HIERARCHYID,
			hierarchyParentClassID,
			hierarchyClassName
			)
		VALUES (
			1,
			1,
			NULL,
			st.hierarchyClassName
			)
OUTPUT inserted.hierarchyClassID,
	st.gsID,
	st.hierarchyClassName
INTO @SegmentOutputTemp(hierarchyClassID, gsID, hierarchyClassName);--Setting up @FamilyTemp table 
INSERT INTO @FamilyTemp
SELECT hierarchyClassName,
	hierarchyLevel,
	HIERARCHYID,
	gsID,
	hierarchyParentClassID
FROM (
	SELECT DISTINCT [Family Description] AS hierarchyClassName,
		2 AS hierarchyLevel,
		1 AS HIERARCHYID,
		[Family Code] AS gsID,
		st.hierarchyClassID AS hierarchyParentClassID
	FROM ##GS1Temp gs1
	INNER JOIN @SegmentOutputTemp st ON st.gsID = gs1.[Segment Code]
	) family
MERGE INTO HierarchyClass
USING @FamilyTemp AS ft
	ON 1 = 0
WHEN NOT MATCHED
	THEN
		INSERT (
			hierarchyLevel,
			HIERARCHYID,
			hierarchyParentClassID,
			hierarchyClassName
			)
		VALUES (
			2,
			1,
			ft.hierarchyParentClassID,
			ft.hierarchyClassName
			)
OUTPUT inserted.hierarchyClassID,
	ft.gsID,
	ft.hierarchyClassName
INTO @FamilyOutputTemp(hierarchyClassID, gsID, hierarchyClassName);--Setting up @ClassTemp table 
INSERT INTO @ClassTemp
SELECT hierarchyClassName,
	hierarchyLevel,
	HIERARCHYID,
	gsID,
	hierarchyParentClassID
FROM (
	SELECT DISTINCT [Class Description] AS hierarchyClassName,
		2 AS hierarchyLevel,
		1 AS HIERARCHYID,
		[Class Code] AS gsID,
		ft.hierarchyClassID AS hierarchyParentClassID
	FROM ##GS1Temp gs1
	INNER JOIN @FamilyOutputTemp ft ON ft.gsID = gs1.[Family Code]
	) class
MERGE INTO HierarchyClass
USING @ClassTemp AS ct
	ON 1 = 0
WHEN NOT MATCHED
	THEN
		INSERT (
			hierarchyLevel,
			HIERARCHYID,
			hierarchyParentClassID,
			hierarchyClassName
			)
		VALUES (
			3,
			1,
			ct.hierarchyParentClassID,
			ct.hierarchyClassName
			)
OUTPUT inserted.hierarchyClassID,
	ct.gsID,
	ct.hierarchyClassName
INTO @ClassOutputTemp(hierarchyClassID, gsID, hierarchyClassName);--Setting up @BrickTemp table 
INSERT INTO @BrickTemp
SELECT hierarchyClassName,
	hierarchyLevel,
	HIERARCHYID,
	gsID,
	hierarchyParentClassID
FROM (
	SELECT DISTINCT [GS1 Brick Description] AS hierarchyClassName,
		2 AS hierarchyLevel,
		1 AS HIERARCHYID,
		[GS1 Brick Code] AS gsID,
		ct.hierarchyClassID AS hierarchyParentClassID
	FROM ##GS1Temp gs1
	INNER JOIN @ClassOutputTemp ct ON ct.gsID = gs1.[Class Code]
	) brick 

MERGE INTO HierarchyClass
USING @BrickTemp AS bt
	ON 1 = 0
WHEN NOT MATCHED
	THEN
		INSERT (
			hierarchyLevel,
			HIERARCHYID,
			hierarchyParentClassID,
			hierarchyClassName
			)
		VALUES (
			4,
			1,
			bt.hierarchyParentClassID,
			bt.hierarchyClassName
			)
OUTPUT inserted.hierarchyClassID,
	bt.gsID,
	bt.hierarchyClassName
INTO @BrickOutputTemp(hierarchyClassID, gsID, hierarchyClassName);--Setting up @AttributeTemp table 
INSERT INTO @SubBrickTemp
SELECT hierarchyClassName,
	hierarchyLevel,
	HIERARCHYID,
	gsID,
	hierarchyParentClassID
FROM (
	SELECT DISTINCT [Sub Brick Description] AS hierarchyClassName,
		5 AS hierarchyLevel,
		1 AS HIERARCHYID,
		[Sub Brick Code] AS gsID,
		bt.hierarchyClassID AS hierarchyParentClassID
	FROM ##GS1Temp gs1
	INNER JOIN @BrickOutputTemp bt ON bt.gsID = gs1.[GS1 Brick Code]
		AND gs1.[Sub Brick Code] IS NOT NULL
	) attribute 

MERGE INTO HierarchyClass
USING @SubBrickTemp AS at
	ON 1 = 0
WHEN NOT MATCHED
	THEN
		INSERT (
			hierarchyLevel,
			HIERARCHYID,
			hierarchyParentClassID,
			hierarchyClassName
			)
		VALUES (
			5,
			1,
			at.hierarchyParentClassID,
			at.hierarchyClassName
			)
OUTPUT inserted.hierarchyClassID,
	at.gsID,
	at.hierarchyClassName
INTO @SubBrickOutputTemp(hierarchyClassID, gsID, hierarchyClassName);

INSERT INTO hierarchyclasstrait (
	traitid,
	hierarchyclassid,
	traitvalue
	)
SELECT t.traitid,
	sb.hierarchyclassid,
	sb.gsID
FROM @SubBrickOutputTemp sb
CROSS JOIN (
	SELECT traitid
	FROM trait
	WHERE traitcode = 'SBC'
	) t

SELECT *
FROM @SubBrickOutputTemp

DROP TABLE ##GS1Temp 
GO

SELECT *
FROM HierarchyClass
WHERE HIERARCHYID = 1
ORDER BY hierarchyClassName,
	hierarchyClassID,
	hierarchyLevel

SELECT *
FROM HierarchyClassTrait