--Formatted with PoorSQL

CREATE PROCEDURE [app].[GetManufacturers]
AS
BEGIN
	DECLARE @zipCodeID INT
		,@arCustomerID INT

	IF (object_id('tempdb..#manufacturers') IS NOT NULL)
		DROP TABLE #manufacturers;

	SET @zipCodeID = (SELECT traitID FROM Trait	WHERE traitCode = 'ZIP');
	SET @arCustomerID = (SELECT traitID	FROM Trait WHERE traitCode = 'ARC');

	SELECT mhv.hierarchyClassID
		,hct.traitValue AS ZipCode
		,cast(NULL AS VARCHAR(255)) AS ArCustomerId
	INTO #manufacturers
	FROM ManufacturerHierarchyView mhv
	LEFT JOIN HierarchyClassTrait hct ON hct.hierarchyClassID = mhv.hierarchyClassID
	AND hct.traitID = @zipCodeID

	CREATE INDEX ix_hierarchyClassID ON #manufacturers (HierarchyClassID);

	UPDATE m
	SET ArCustomerId = hct.traitValue
	FROM #manufacturers m
	LEFT JOIN HierarchyClassTrait hct ON hct.hierarchyClassID = m.hierarchyClassID
	WHERE hct.traitID = @arCustomerID;

	SELECT mhv.hierarchyClassID AS HierarchyClassID
		,mhv.hierarchyClassName AS HierarchyClassName
		,mhv.hierarchyID AS [HierarchyID]
		,hct.ArCustomerId
		,hct.ZipCode
	FROM ManufacturerHierarchyView mhv
	INNER JOIN #manufacturers hct ON hct.hierarchyClassID = mhv.hierarchyClassID
	ORDER BY mhv.hierarchyClassName;

	IF (object_id('tempdb..#manufacturers') IS NOT NULL)
		DROP TABLE #manufacturers;
END
