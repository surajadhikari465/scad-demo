CREATE PROCEDURE [extract].[ProductHierarchy]
AS
BEGIN
	DECLARE @ncctrait INT
	DECLARE @NationalHierarchyId INT

	SELECT @NationalHierarchyId = HIERARCHYID
	FROM Hierarchy
	WHERE hierarchyName = 'National'

	SELECT @ncctrait = traitid
	FROM trait
	WHERE traitcode = 'ncc'

	SELECT sc.scancode
		,i.ProductDescription
		,ihc.itemid
		,hcFam.hierarchyClassID AS 'FAMILY_ID'
		,hcFam.hierarchyClassName AS 'FAMILY_DESC'
		,hcCat.hierarchyClassID AS 'CATEGORY_ID'
		,hcCat.hierarchyClassName AS 'CATEGORY_DESC'
		,hcSubCat.hierarchyClassID AS 'SUBCATEGORY_ID'
		,hcSubCat.hierarchyClassName AS 'SUBCATEGORY_DESC'
		,hcClass.hierarchyClassID AS 'CLASS_ID'
		,hcClass.hierarchyClassName AS 'CLASS_DESC'
		,hctClass.traitValue AS 'NCC_ClASS'
		,hctSubCat.traitValue AS 'NCC_SUBCATEGORY'
		,hctCat.traitValue AS 'NCC_CATEGORY'
		,hctFam.traitValue AS 'NCC_FAMILY'
	FROM HierarchyClass hcFam
	INNER JOIN HierarchyClass hcCat ON hcFam.hierarchyClassID = hcCat.hierarchyParentClassID
		AND hcFam.hierarchyLevel = 1
	INNER JOIN HierarchyClass hcSubCat ON hcCat.hierarchyClassID = hcSubCat.hierarchyParentClassID
		AND hcCat.hierarchyLevel = 2
	INNER JOIN HierarchyClass hcClass ON hcSubCat.hierarchyClassID = hcClass.hierarchyParentClassID
		AND hcSubCat.hierarchyLevel = 3
	INNER JOIN ItemHierarchyClass ihc ON ihc.hierarchyClassID = hcClass.hierarchyClassID
	INNER JOIN Scancode sc ON ihc.itemID = sc.itemID
	INNER JOIN HierarchyClassTrait hctClass ON hctClass.hierarchyClassID = hcclass.hierarchyClassID
		AND hctclass.traitID = @ncctrait
	INNER JOIN HierarchyClassTrait hctSubCat ON hctSubCat.hierarchyClassID = hcSubCat.hierarchyClassID
		AND hctSubCat.traitID = @ncctrait
	INNER JOIN HierarchyClassTrait hctCat ON hctCat.hierarchyClassID = hcCat.hierarchyClassID
		AND hctCat.traitID = @ncctrait
	INNER JOIN HierarchyClassTrait hctFam ON hctFam.hierarchyClassID = hcFam.hierarchyClassID
		AND hctFam.traitID = @ncctrait
	INNER JOIN Item i on ihc.itemID = i.ItemId
	WHERE hcFam.HIERARCHYID = @NationalHierarchyId
	
END
GO