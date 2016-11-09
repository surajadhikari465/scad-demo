SET IDENTITY_INSERT [dbo].[Hierarchy] ON;
INSERT INTO Hierarchy (
	HIERARCHYID,
	hierarchyName
	)
VALUES (
	'5',
	'Financial'
	)
SET IDENTITY_INSERT [dbo].[Hierarchy] OFF;

CREATE TABLE #FinTemp (
	hier NVARCHAR(255),
	code NVARCHAR(255)
	);
BULK INSERT #FinTemp
FROM '\\irmadevfile\e$\ICONData\FinancialHierarchy.txt'

DECLARE @ID INT
SELECT @ID = HIERARCHYID
FROM Hierarchy
WHERE hierarchyName = 'Financial'
PRINT @id
INSERT INTO hierarchyprototype (
	HIERARCHYID,
	hierarchylevel,
	hierarchylevelname,
	itemsattached
	)
VALUES (
	@ID,
	1,
	'Financial',
	1
	)
-----------------------------------------------
DECLARE @SegmentOutputTemp TABLE (
	hierarchyClassID INT,
	hier NVARCHAR(255),
	code NVARCHAR(255)
	)
MERGE INTO HierarchyClass
USING #FinTemp AS st
	ON 1 = 0
WHEN NOT MATCHED
	THEN
		INSERT (
			hierarchyLevel,
			HIERARCHYID,
			hierarchyClassName
			)
		VALUES (
			1,
			@ID,
			--	NULL,
			st.hier
			)
OUTPUT inserted.hierarchyClassID,
	st.code,
	st.hier
INTO @SegmentOutputTemp(hierarchyClassID, code, hier);
INSERT INTO hierarchyclasstrait (
	traitid,
	hierarchyclassid,
	traitvalue
	)
SELECT t.traitid,
	sb.hierarchyclassid,
	sb.code
FROM @SegmentOutputTemp sb
CROSS JOIN (
	SELECT traitid
	FROM trait
	WHERE traitcode = 'FIN'
	) t
SELECT *
FROM @SegmentOutputTemp
DROP TABLE #FinTemp
SELECT *
FROM hierarchyclass
WHERE HIERARCHYID = @ID


select * from Hierarchy
