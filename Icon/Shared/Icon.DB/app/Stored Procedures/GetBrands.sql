--Formatted with PoorSQL

CREATE PROCEDURE [app].[GetBrands]
AS
BEGIN
	DECLARE @brandHierarchyId INT
		,@brandAbbreviationTraitId INT
		,@desegnationID INT
		,@parentCompanyID INT
		,@zipCodeID INT
		,@localityID INT

	IF (object_id('tempdb..#brands') IS NOT NULL)
		DROP TABLE #brands;

	SET @brandHierarchyId = (SELECT [hierarchyID]	FROM Hierarchy WHERE hierarchyName = 'Brands');
	SET @brandAbbreviationTraitId = (SELECT traitID	FROM Trait WHERE traitDesc = 'Brand Abbreviation');
	SET @desegnationID = (SELECT traitID FROM Trait	WHERE traitDesc = 'Designation');
	SET @parentCompanyID = (SELECT traitID FROM Trait WHERE traitDesc = 'Parent Company');
	SET @zipCodeID = (SELECT traitID FROM Trait	WHERE traitDesc = 'Zip Code');
	SET @localityID = (SELECT traitID	FROM Trait WHERE traitDesc = 'Locality');

	SELECT A.hierarchyClassID
		,B.traitValue AS BrandAbbreviation
		,cast(NULL AS VARCHAR(255)) AS Designation
		,cast(NULL AS VARCHAR(255)) AS ParentCompany
		,cast(NULL AS VARCHAR(255)) AS ZipCode
		,cast(NULL AS VARCHAR(255)) AS Locality
	INTO #brands
	FROM HierarchyClass A
	LEFT JOIN HierarchyClassTrait B ON B.hierarchyClassID = A.hierarchyClassID
		AND B.traitID = @brandAbbreviationTraitId
	WHERE A.HIERARCHYID = @brandHierarchyId;

	CREATE INDEX ix_id ON #brands (HierarchyClassID);

	UPDATE A
	SET Designation = B.traitValue
	FROM #brands A
	LEFT JOIN HierarchyClassTrait B ON B.hierarchyClassID = A.hierarchyClassID
	WHERE B.traitID = @desegnationID;

	UPDATE A
	SET ParentCompany = B.traitValue
	FROM #brands A
	LEFT JOIN HierarchyClassTrait B ON B.hierarchyClassID = A.hierarchyClassID
	WHERE B.traitID = @parentCompanyID;

	UPDATE A
	SET ZipCode = B.traitValue
	FROM #brands A
	LEFT JOIN HierarchyClassTrait B ON B.hierarchyClassID = A.hierarchyClassID
	WHERE B.traitID = @zipCodeID;

	UPDATE A
	SET Locality = B.traitValue
	FROM #brands A
	LEFT JOIN HierarchyClassTrait B ON B.hierarchyClassID = A.hierarchyClassID
	WHERE B.traitID = @localityID;

	SELECT A.hierarchyClassID AS HierarchyClassID
		,A.hierarchyClassName AS HierarchyClassName
		,A.hierarchyID AS [HierarchyID]
		,A.hierarchyLevel AS HierarchyLevel
		,A.hierarchyParentClassId AS HierarchyParentClassId
		,B.BrandAbbreviation
		,B.Designation
		,B.Locality
		,B.ParentCompany
		,B.ZipCode
	FROM HierarchyClass A
	INNER JOIN #brands B ON B.hierarchyClassID = A.hierarchyClassID
	ORDER BY A.hierarchyClassName;

	IF (object_id('tempdb..#brands') IS NOT NULL)
		DROP TABLE #brands;
END
GO