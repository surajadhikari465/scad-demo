CREATE PROCEDURE infor.ValidateItems
	@items infor.ValidateItemType READONLY
AS
BEGIN
	DECLARE @inforItemListenerAppId int = (SELECT AppID FROM app.App WHERE AppName = 'Infor Item Listener')
	DECLARE @brandHierarchyId int = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Brands'),
			@financialHierarchyId int = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Financial'),
			@merchandiseHierarchyId int = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Merchandise'),
			@nationalHierarchyId int = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'National'),
			@taxHierarchyId int = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Tax')
	DECLARE @nationalClassLevel int = (SELECT hierarchyLevel FROM HierarchyPrototype WHERE hierarchyLevelName = 'National Class'),
			@subBrickLevel int = (SELECT hierarchyLevel FROM HierarchyPrototype WHERE hierarchyLevelName = 'Sub Brick')
	DECLARE @nonExistentBrandErrorCode nvarchar(50) = 'NonExistentBrand',
			@nonExistentSubTeamErrorCode nvarchar(50) = 'NonExistentSubTeam',
			@nonExistentSubBrickErrorCode nvarchar(50) = 'NonExistentSubBrick',
			@nonExistentNationalClassErrorCode nvarchar(50) = 'NonExistentNationalClass',
			@nonExisteTaxnErrorCode nvarchar(50) = 'NonExistentTax'

	SELECT 
		i.ItemId,
		CASE 
			WHEN brands.hierarchyClassID IS NULL THEN @nonExistentBrandErrorCode
			WHEN financial.hierarchyClassID IS NULL THEN @nonExistentSubTeamErrorCode
			WHEN merchandise.hierarchyClassID IS NULL THEN @nonExistentSubBrickErrorCode
			WHEN nat.hierarchyClassID IS NULL THEN @nonExistentNationalClassErrorCode
			WHEN tax.hierarchyClassID IS NULL THEN @nonExisteTaxnErrorCode
			ELSE null
		END AS ErrorCode,
		CASE
			WHEN brands.hierarchyClassID IS NULL THEN infor.GetValidationError(@nonExistentBrandErrorCode, @inforItemListenerAppId, i.BrandHierarchyClassId) 
			WHEN financial.hierarchyClassID IS NULL THEN infor.GetValidationError(@nonExistentSubTeamErrorCode, @inforItemListenerAppId, i.FinancialHierarchyClassId)
			WHEN merchandise.hierarchyClassID IS NULL THEN infor.GetValidationError(@nonExistentSubBrickErrorCode, @inforItemListenerAppId, i.MerchandiseHierarchyClassId)
			WHEN nat.hierarchyClassID IS NULL THEN infor.GetValidationError(@nonExistentNationalClassErrorCode, @inforItemListenerAppId, i.NationalHierarchyClassId)
			WHEN tax.hierarchyClassID IS NULL THEN infor.GetValidationError(@nonExisteTaxnErrorCode, @inforItemListenerAppId, i.TaxHierarchyClassId)
			ELSE null
		END AS ErrorDetails
	FROM @items i
	LEFT JOIN HierarchyClass brands on i.BrandHierarchyClassId = brands.hierarchyClassID
		AND brands.hierarchyID = @brandHierarchyId
	LEFT JOIN HierarchyClass financial on infor.GetHierarchyClassIdFromInfor(@financialHierarchyId, i.FinancialHierarchyClassId) = financial.hierarchyClassID
		AND financial.hierarchyID = @financialHierarchyId
	LEFT JOIN HierarchyClass merchandise on i.MerchandiseHierarchyClassId = merchandise.hierarchyClassID
		AND merchandise.hierarchyID = @merchandiseHierarchyId
		AND merchandise.hierarchyLevel = @subBrickLevel
	LEFT JOIN HierarchyClass nat on i.NationalHierarchyClassId = nat.hierarchyClassID
		AND nat.hierarchyID = @nationalHierarchyId
		AND nat.hierarchyLevel = @nationalClassLevel
	LEFT JOIN HierarchyClass tax on infor.GetHierarchyClassIdFromInfor(@taxHierarchyId, i.TaxHierarchyClassId)  = tax.hierarchyClassID
		AND tax.hierarchyID = @taxHierarchyId
END