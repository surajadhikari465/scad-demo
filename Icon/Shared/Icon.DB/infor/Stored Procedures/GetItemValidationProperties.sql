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
			SELECT [hierarchyId]
			FROM dbo.Hierarchy
			WHERE hierarchyName = 'Brands'
			)
		,@financialHierarchyId INT = (
			SELECT [hierarchyId]
			FROM dbo.Hierarchy
			WHERE hierarchyName = 'Financial'
			)
		,@merchandiseHierarchyId INT = (
			SELECT [hierarchyId]
			FROM dbo.Hierarchy
			WHERE hierarchyName = 'Merchandise'
			)
		,@nationalHierarchyId INT = (
			SELECT [hierarchyId]
			FROM dbo.Hierarchy
			WHERE hierarchyName = 'National'
			)
		,@taxHierarchyId INT = (
			SELECT [hierarchyId]
			FROM Hierarchy
			WHERE hierarchyName = 'Tax'
			)
	DECLARE @nationalClassLevel INT = (
			SELECT hierarchyLevel
			FROM dbo.HierarchyPrototype
			WHERE hierarchyLevelName = 'National Class'
			)
		,@subBrickLevel INT = (
			SELECT hierarchyLevel
			FROM dbo.HierarchyPrototype
			WHERE hierarchyLevelName = 'Sub Brick'
			)
	DECLARE @modifiedDateTraitId INT = (
			SELECT traitID
			FROM dbo.Trait
			WHERE traitCode = 'MOD'
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
		modifiedDate.traitValue AS ModifiedDate,
		seq.SequenceID AS SequenceId
	FROM #tempItems i
	LEFT JOIN dbo.HierarchyClass brands ON i.BrandHierarchyClassId = brands.hierarchyClassID
		AND brands.[hierarchyId] = @brandHierarchyId
	LEFT JOIN dbo.HierarchyClass financial ON infor.GetHierarchyClassIdFromInfor(@financialHierarchyId, i.FinancialHierarchyClassId) = financial.hierarchyClassID
		AND financial.[hierarchyId] = @financialHierarchyId
	LEFT JOIN dbo.HierarchyClass merchandise ON i.MerchandiseHierarchyClassId = merchandise.hierarchyClassID
		AND merchandise.[hierarchyId] = @merchandiseHierarchyId
		AND merchandise.hierarchyLevel = @subBrickLevel
	LEFT JOIN dbo.HierarchyClass nat ON i.NationalHierarchyClassId = nat.hierarchyClassID
		AND nat.[hierarchyId] = @nationalHierarchyId
		AND nat.hierarchyLevel = @nationalClassLevel
	LEFT JOIN dbo.HierarchyClass tax ON infor.GetHierarchyClassIdFromInfor(@taxHierarchyId, i.TaxHierarchyClassId) = tax.hierarchyClassID
		AND tax.[hierarchyId] = @taxHierarchyId
	LEFT JOIN dbo.ItemTrait modifiedDate ON i.ItemId = modifiedDate.itemID
		AND modifiedDate.traitID = @modifiedDateTraitId
	LEFT JOIN infor.ItemSequence seq ON i.ItemId = seq.ItemID
END