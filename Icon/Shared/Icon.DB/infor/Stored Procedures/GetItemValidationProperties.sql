CREATE PROCEDURE infor.GetItemValidationProperties 
	@items infor.ValidateItemType READONLY
AS
BEGIN
	CREATE TABLE #tempItems
	(
		ItemId int not null,
		BrandHierarchyClassId int not null,
		FinancialHierarchyClassId nvarchar(4) not null,
		MerchandiseHierarchyClassId int not null,
		NationalHierarchyClassId int not null,
		TaxHierarchyClassId nvarchar(7) not null
	)
	INSERT INTO #tempItems
	SELECT * 
	FROM @items

	DECLARE @inforItemListenerAppId INT = (
			SELECT AppID
			FROM app.App
			WHERE AppName = 'Infor Item Listener'
			)
	DECLARE @brandHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Brands'
			)
		,@financialHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Financial'
			)
		,@merchandiseHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Merchandise'
			)
		,@nationalHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'National'
			)
		,@taxHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Tax'
			)
	DECLARE @nationalClassLevel INT = (
			SELECT hierarchyLevel
			FROM HierarchyPrototype
			WHERE hierarchyLevelName = 'National Class'
			)
		,@subBrickLevel INT = (
			SELECT hierarchyLevel
			FROM HierarchyPrototype
			WHERE hierarchyLevelName = 'Sub Brick'
			)
	DECLARE @modifiedDateTraitId INT = (
			SELECT traitID
			FROM Trait t
			WHERE t.traitCode = 'MOD'
			)
	DECLARE @nonExistentBrandErrorCode NVARCHAR(50) = 'NonExistentBrand'
		,@nonExistentSubTeamErrorCode NVARCHAR(50) = 'NonExistentSubTeam'
		,@nonExistentSubBrickErrorCode NVARCHAR(50) = 'NonExistentSubBrick'
		,@nonExistentNationalClassErrorCode NVARCHAR(50) = 'NonExistentNationalClass'
		,@nonExisteTaxnErrorCode NVARCHAR(50) = 'NonExistentTax'
		,@outOfSyncUpdateErrorCode NVARCHAR(50) = 'OutOfSyncItemUpdateErrorCode'

	SELECT 
		i.ItemId AS ItemId,
		brands.hierarchyClassID AS BrandId,
		financial.hierarchyClassID AS SubTeamId,
		merchandise.hierarchyClassID AS SubBrickId,
		nat.hierarchyClassID AS NationalClassId,
		tax.hierarchyClassID AS TaxClassId,
		modifiedDate.traitValue AS ModifiedDate
	FROM #tempItems i
	LEFT JOIN HierarchyClass brands ON i.BrandHierarchyClassId = brands.hierarchyClassID
		AND brands.HIERARCHYID = @brandHierarchyId
	LEFT JOIN HierarchyClass financial ON infor.GetHierarchyClassIdFromInfor(@financialHierarchyId, i.FinancialHierarchyClassId) = financial.hierarchyClassID
		AND financial.HIERARCHYID = @financialHierarchyId
	LEFT JOIN HierarchyClass merchandise ON i.MerchandiseHierarchyClassId = merchandise.hierarchyClassID
		AND merchandise.HIERARCHYID = @merchandiseHierarchyId
		AND merchandise.hierarchyLevel = @subBrickLevel
	LEFT JOIN HierarchyClass nat ON i.NationalHierarchyClassId = nat.hierarchyClassID
		AND nat.HIERARCHYID = @nationalHierarchyId
		AND nat.hierarchyLevel = @nationalClassLevel
	LEFT JOIN HierarchyClass tax ON infor.GetHierarchyClassIdFromInfor(@taxHierarchyId, i.TaxHierarchyClassId) = tax.hierarchyClassID
		AND tax.HIERARCHYID = @taxHierarchyId
	LEFT JOIN ItemTrait modifiedDate ON i.ItemId = modifiedDate.itemID
		AND modifiedDate.traitID = @modifiedDateTraitId
END