--Formatted with PoorSQL

CREATE PROCEDURE [app].[GetBrands]
AS
BEGIN
	DECLARE @brandAbbreviationTraitId INT
		,@desegnationID INT
		,@parentCompanyID INT
		,@zipCodeID INT
		,@localityID INT

	IF (object_id('tempdb..#brands') IS NOT NULL)
		DROP TABLE #brands;

	SET @brandAbbreviationTraitId = (SELECT traitID	FROM Trait WHERE traitDesc = 'Brand Abbreviation');
	SET @desegnationID = (SELECT traitID FROM Trait	WHERE traitDesc = 'Designation');
	SET @parentCompanyID = (SELECT traitID FROM Trait WHERE traitDesc = 'Parent Company');
	SET @zipCodeID = (SELECT traitID FROM Trait	WHERE traitCode = 'ZIP');
	SET @localityID = (SELECT traitID	FROM Trait WHERE traitDesc = 'Locality');

	SELECT bhv.hierarchyClassID
		,hct.traitValue AS BrandAbbreviation
		,cast(NULL AS VARCHAR(255)) AS Designation
		,cast(NULL AS VARCHAR(255)) AS ParentCompany
		,cast(NULL AS VARCHAR(255)) AS ZipCode
		,cast(NULL AS VARCHAR(255)) AS Locality
	INTO #brands
	FROM BrandHierarchyView bhv
	LEFT JOIN HierarchyClassTrait hct ON hct.hierarchyClassID = bhv.hierarchyClassID
		AND hct.traitID = @brandAbbreviationTraitId

	CREATE INDEX ix_id ON #brands (HierarchyClassID);

	UPDATE b
	SET Designation = hct.traitValue
	FROM #brands b
	LEFT JOIN HierarchyClassTrait hct ON hct.hierarchyClassID = b.hierarchyClassID
	WHERE hct.traitID = @desegnationID;

	UPDATE b
	SET ParentCompany = hct.traitValue
	FROM #brands b
	LEFT JOIN HierarchyClassTrait hct ON hct.hierarchyClassID = b.hierarchyClassID
	WHERE hct.traitID = @parentCompanyID;

	UPDATE b
	SET ZipCode = hct.traitValue
	FROM #brands b
	LEFT JOIN HierarchyClassTrait hct ON hct.hierarchyClassID = b.hierarchyClassID
	WHERE hct.traitID = @zipCodeID;

	UPDATE b
	SET Locality = hct.traitValue
	FROM #brands b
	LEFT JOIN HierarchyClassTrait hct ON hct.hierarchyClassID = b.hierarchyClassID
	WHERE hct.traitID = @localityID;

	SELECT bhv.hierarchyClassID AS HierarchyClassID
		,bhv.hierarchyClassName AS HierarchyClassName
		,bhv.hierarchyID AS [HierarchyID]
		,bhv.hierarchyLevel AS HierarchyLevel
		,bhv.hierarchyParentClassId AS HierarchyParentClassId
		,hct.BrandAbbreviation
		,hct.Designation
		,hct.Locality
		,hct.ParentCompany
		,hct.ZipCode
	FROM BrandHierarchyView bhv
	INNER JOIN #brands hct ON hct.hierarchyClassID = bhv.hierarchyClassID
	ORDER BY bhv.hierarchyClassName;

	IF (object_id('tempdb..#brands') IS NOT NULL)
		DROP TABLE #brands;
END
GO