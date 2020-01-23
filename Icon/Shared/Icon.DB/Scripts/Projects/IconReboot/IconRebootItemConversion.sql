USE Icon
GO

SET NOCOUNT ON;

--Disable item history if it's turned on
IF (SELECT temporal_type FROM sys.tables WHERE name = 'Item' AND SCHEMA_NAME(schema_id) = 'dbo') <> 0
BEGIN
	PRINT 'Disabling item history'
	ALTER TABLE dbo.Item
	SET (SYSTEM_VERSIONING = OFF)
END

--Disable ItemHierarchyClass history if it's turned on
IF (SELECT temporal_type FROM sys.tables WHERE name = 'ItemHierarchyClass' AND SCHEMA_NAME(schema_id) = 'dbo') <> 0
BEGIN
	PRINT 'Disabling ItemHierarchyClass history'
	ALTER TABLE dbo.ItemHierarchyClass
	SET (SYSTEM_VERSIONING = OFF)
END


PRINT 'Dropping period'
ALTER TABLE dbo.Item DROP PERIOD FOR SYSTEM_TIME;
ALTER TABLE dbo.ItemHierarchyClass DROP PERIOD FOR SYSTEM_TIME
GO

BEGIN TRY

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES t WHERE t.TABLE_SCHEMA = 'dbo' AND t.TABLE_NAME = 'ScanCodeBackup')
BEGIN
	RAISERROR('ScanCodeBackup is missing which means you need to run the attribute conversion script again before running this script', 15, 10)
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'temp_InforItemsFile')
BEGIN
  DROP TABLE dbo.temp_InforItemsFile 
END

	CREATE TABLE dbo.temp_InforItemsFile (
		[ItemID] INT NULL
		,[ItemName] NVARCHAR(255) NULL
		,[BarcodeType] NVARCHAR(255) NULL
		,[ScanCode] NVARCHAR(255) NULL
		,[Validated] NVARCHAR(255) NULL
		,[Merchandise-Association] NVARCHAR(255) NULL
		,[National-Association] NVARCHAR(255) NULL
		,[Brand] NVARCHAR(255) NULL
		,[Item Type] NVARCHAR(255) NULL
		,[Subteam] NVARCHAR(255) NULL
		,[Tax] NVARCHAR(255) NULL
		,[365 Eligible] NVARCHAR(255) NULL
		,[ABF] NVARCHAR(255) NULL
		,[Accessory] NVARCHAR(255) NULL
		,[Age/Gender] NVARCHAR(255) NULL
		,[Air Chilled] NVARCHAR(255) NULL
		,[Alcohol By Volume] NVARCHAR(255) NULL
		,[Allocated] NVARCHAR(255) NULL
		,[Animal Welfare Rating] NVARCHAR(255) NULL
		,[Appellation] NVARCHAR(255) NULL
		,[Beer Style] NVARCHAR(255) NULL
		,[Biodynamic] NVARCHAR(255) NULL
		,[Blackhawk Commission Dollar] NVARCHAR(255) NULL
		,[Blackhawk Commission Percent] NVARCHAR(255) NULL
		,[Casein Free] NVARCHAR(255) NULL
		,[Category Management] NVARCHAR(255) NULL
		,[Cheese Attribute: Milk Type] NVARCHAR(255) NULL
		,[Country of Origin] NVARCHAR(255) NULL
		,[Cube] NVARCHAR(255) NULL
		,[Customer Friendly Description] NVARCHAR(255) NULL
		,[dadlkj] NVARCHAR(255) NULL
		,[Data Source] NVARCHAR(255) NULL
		,[Delivery System] NVARCHAR(255) NULL
		,[Dimensions Data Source] NVARCHAR(255) NULL
		,[Disposable] NVARCHAR(255) NULL
		,[Drained Weight] NVARCHAR(255) NULL
		,[Drained Weight UOM] NVARCHAR(255) NULL
		,[Dry Aged] NVARCHAR(255) NULL
		,[Eco-Scale Rating] NVARCHAR(255) NULL
		,[EStore Eligible] NVARCHAR(255) NULL
		,[EStore Nutrition Required] NVARCHAR(255) NULL
		,[Exclusive (Date)] NVARCHAR(255) NULL
		,[Exclusive (Yes/No)] NVARCHAR(255) NULL
		,[Fair Trade Certified] NVARCHAR(255) NULL
		,[Fair Trade Claim] NVARCHAR(255) NULL
		,[Fat Free Claim] NVARCHAR(255) NULL
		,[Flex Sign Text] NVARCHAR(255) NULL
		,[Food Stamp Eligible] NVARCHAR(255) NULL
		,[Fragrance Free] NVARCHAR(255) NULL
		,[Free Range] NVARCHAR(255) NULL
		,[Fresh or Frozen] NVARCHAR(255) NULL
		,[Global Pricing Program] NVARCHAR(255) NULL
		,[Gluten Free] NVARCHAR(255) NULL
		,[Gluten Free Claim] NVARCHAR(255) NULL
		,[GMO Transparency] NVARCHAR(255) NULL
		,[Good Better Best] NVARCHAR(255) NULL
		,[Grass Fed] NVARCHAR(255) NULL
		,[Halal] NVARCHAR(255) NULL
		,[Hemp] NVARCHAR(255) NULL
		,[Homeopathic] NVARCHAR(255) NULL
		,[Hormone Free Claim] NVARCHAR(255) NULL
		,[Hospitality Item] NVARCHAR(255) NULL
		,[Image Map] NVARCHAR(255) NULL
		,[Item Depth] NVARCHAR(255) NULL
		,[Item Height] NVARCHAR(255) NULL
		,[Item Pack] NVARCHAR(255) NULL
		,[Item Status] NVARCHAR(255) NULL
		,[Item Weight] NVARCHAR(255) NULL
		,[Item Width] NVARCHAR(255) NULL
		,[IX One Brand] NVARCHAR(255) NULL
		,[IX One ID] NVARCHAR(255) NULL
		,[Juice Content] NVARCHAR(255) NULL
		,[Kitchen Description] NVARCHAR(255) NULL
		,[Kitchen Item] NVARCHAR(255) NULL
		,[Kosher] NVARCHAR(255) NULL
		,[Kosher Claim] NVARCHAR(255) NULL
		,[Labeling] NVARCHAR(255) NULL
		,[Line] NVARCHAR(255) NULL
		,[Line Extension] NVARCHAR(255) NULL
		,[Local Loan Producer] NVARCHAR(255) NULL
		,[Low Fat Claim] NVARCHAR(255) NULL
		,[Made In House] NVARCHAR(255) NULL
		,[Made with Organic Grapes] NVARCHAR(255) NULL
		,[Manufacturer] NVARCHAR(255) NULL
		,[Manufacturer Item Number] NVARCHAR(255) NULL
		,[MSC] NVARCHAR(255) NULL 
		,[Multipurpose] NVARCHAR(255) NULL
		,[Natural Claim] NVARCHAR(255) NULL
		,[Non-GMO] NVARCHAR(255) NULL
		,[Non GMO Claim] NVARCHAR(255) NULL
		,[No Sulfites] NVARCHAR(255) NULL
		,[Notes] NVARCHAR(255) NULL
		,[Nutrition Required] NVARCHAR(255) NULL
		,[Organic] NVARCHAR(255) NULL
		,[Organic Claim] NVARCHAR(255) NULL
		,[Organic Personal Care] NVARCHAR(255) NULL
		,[Other 3P Eligible] NVARCHAR(255) NULL
		,[Ownership (Brand Level)] NVARCHAR(255) NULL
		,[Package Group] NVARCHAR(255) NULL
		,[Package Group Type] NVARCHAR(255) NULL
		,[Packaging Type] NVARCHAR(255) NULL
		,[Paleo] NVARCHAR(255) NULL
		,[Paleo Claim] NVARCHAR(255) NULL
		,[Pasture Raised] NVARCHAR(255) NULL
		,[Pct Tare Weight] NVARCHAR(255) NULL
		,[PMD Verified] NVARCHAR(255) NULL
		,[POS Description] NVARCHAR(255) NULL
		,[POS Scale Tare] NVARCHAR(255) NULL
		,[Premium Body Care] NVARCHAR(255) NULL
		,[Price Line] NVARCHAR(255) NULL
		,[Price Line Description] NVARCHAR(255) NULL
		,[Prime] NVARCHAR(255) NULL
		,[Prime Now Eligible] NVARCHAR(255) NULL
		,[Private Label] NVARCHAR(255) NULL
		,[Product Description] NVARCHAR(255) NULL
		,[Product Flavor or Type] NVARCHAR(255) NULL
		,[Prohibit Discount] NVARCHAR(255) NULL
		,[Rainforest Alliance] NVARCHAR(255) NULL
		,[Raw] NVARCHAR(255) NULL
		,[Refrigerated or Shelf Stable] NVARCHAR(255) NULL
		,[Regional Local Item] NVARCHAR(255) NULL
		,[Rennet] NVARCHAR(255) NULL
		,[Retail Size] NVARCHAR(255) NULL
		,[SCO Item Tare Group] NVARCHAR(255) NULL
		,[Seafood: Wild Or Farm Raised] NVARCHAR(255) NULL
		,[Seasonal In and Out/Gifting] NVARCHAR(255) NULL
		,[Shelf Life] NVARCHAR(255) NULL
		,[Skin Type] NVARCHAR(255) NULL
		,[SKU] NVARCHAR(255) NULL
		,[Smithsonian Bird Friendly] NVARCHAR(255) NULL
		,[Smoked] NVARCHAR(255) NULL
		,[Sold Hot or Cold] NVARCHAR(255) NULL
		,[Travel Size/Single Use/Kit] NVARCHAR(255) NULL
		,[Tray Depth] NVARCHAR(255) NULL
		,[Tray Height] NVARCHAR(255) NULL
		,[Tray Width] NVARCHAR(255) NULL
		,[UOM] NVARCHAR(255) NULL
		,[URL1] NVARCHAR(255) NULL
		,[Value Added] NVARCHAR(255) NULL
		,[Variant Size] NVARCHAR(255) NULL
		,[Varietal] NVARCHAR(255) NULL
		,[Vegan] NVARCHAR(255) NULL
		,[Vegan Claim] NVARCHAR(255) NULL
		,[Vegetarian] NVARCHAR(255) NULL
		,[Vegetarian Claim] NVARCHAR(255) NULL
		,[WFM Eligible] NVARCHAR(255) NULL
		,[Whole Trade] NVARCHAR(255) NULL
		,[Created On] NVARCHAR(255) NULL
		,[Created By] NVARCHAR(255) NULL
		,[Modified On] NVARCHAR(255) NULL
		,[Modified By] NVARCHAR(255) NULL
		);

	CREATE NONCLUSTERED INDEX IX_ItemId ON dbo.temp_InforItemsFile (itemId)
	CREATE NONCLUSTERED INDEX IX_ItemTypeId ON dbo.temp_InforItemsFile ([Item Type])

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'temp_ItemManufactureHierarchy')
BEGIN
	CREATE TABLE dbo.temp_ItemManufactureHierarchy
	(
		[ItemID] INT NULL,
		[Manufacturer] NVARCHAR(255) NULL
	)
END

TRUNCATE TABLE dbo.temp_ItemManufactureHierarchy
TRUNCATE TABLE dbo.temp_InforItemsFile

PRINT 'Inserting Infor Items into InforItemFile table...'

BULK INSERT dbo.temp_InforItemsFile
FROM '\\ODWD6801\Temp\IconConversion\im_item.csv' -- needs to be added with file path and filename. Currently C:\TEMP\IconConversion\infor_im_prod_item_20190520.csv
	WITH (FIRSTROW = 2
		,CODEPAGE = '65001'
		,FORMAT = 'CSV');

TRUNCATE TABLE dbo.ItemHistory

PRINT 'Updating dbo.Item table...'
UPDATE dbo.Item
SET ItemAttributesJson = ISNULL((
	SELECT [365 Eligible] AS [365Eligible]
		,[ABF] AS ABF
		,[Accessory] AS Accessory
		,[Age/Gender] AS AgeGender
		,NULLIF([Air Chilled], 'No') AS AirChilled
		,[Alcohol By Volume] AS AlcoholByVolume
		,[Allocated] AS Allocated
		,[Animal Welfare Rating] AS AnimalWelfareRating
		,[Appellation] AS Appellation
		,[Beer Style] AS BeerStyle
		,NULLIF([Biodynamic], 'No') AS Biodynamic
		,[Blackhawk Commission Dollar] AS BlackhawkCommissionDollar
		,[Blackhawk Commission Percent] AS BlackhawkCommissionPercent
		,[Casein Free] AS CaseinFree
		,[Category Management] AS CategoryManagement
		,[Cheese Attribute: Milk Type] AS CheeseAttributeMilkType
		,[Country of Origin] AS CountryofOrigin
		,[Cube] AS [Cube]
		,[Created By] AS CreatedBy
		,[Created On] AS CreatedDateTimeUtc
		,[Customer Friendly Description] AS CustomerFriendlyDescription
		,[Data Source] AS DataSource
		,[Delivery System] AS DeliverySystem
		,[Dimensions Data Source] AS DimensionsDataSource
		,[Disposable] AS Disposable
		,[Drained Weight] AS DrainedWeight
		,[Drained Weight UOM] AS DrainedWeightUOM
		,NULLIF([Dry Aged], 'No') AS DryAged
		,[Eco-Scale Rating] AS EcoScaleRating
		,NULLIF([EStore Eligible], 'No') AS EStoreEligible
		,NULLIF([EStore Nutrition Required], 'No') AS EStoreNutritionRequired
		,[Exclusive (Date)] AS ExclusiveDate
		,[Exclusive (Yes/No)] AS ExclusiveYesNo
		,[Fair Trade Certified] AS FairTradeCertified
		,[Fair Trade Claim] AS FairTradeClaim
		,[Fat Free Claim] AS FatFreeClaim
		,[Flex Sign Text] AS FlexSignText
		,CASE 
			WHEN UPPER([Food Stamp Eligible]) = 'TRUE'
				THEN 'true'
			ELSE 'false'
			END AS FoodStampEligible
		,[Fragrance Free] AS FragranceFree
		,NULLIF([Free Range], 'No') AS FreeRange
		,[Fresh or Frozen] AS FreshorFrozen
		,[Global Pricing Program] AS GlobalPricingProgram
		,[Gluten Free] AS GlutenFree
		,[Gluten Free Claim] AS GlutenFreeClaim
		,[GMO Transparency] AS GMOTransparency
		,[Good Better Best] AS GoodBetterBest
		,NULLIF([Grass Fed], 'No') AS GrassFed
		,[Halal] AS Halal
		,[Hemp] AS Hemp
		,[Homeopathic] AS Homeopathic
		,[Hormone Free Claim] AS HormoneFreeClaim
		,NULLIF([Hospitality Item], 'No') AS HospitalityItem
		,[Image Map] AS ImageMap
		,[Item Depth] AS ItemDepth
		,[Item Height] AS ItemHeight
		,[Item Pack] AS ItemPack
		,CASE 
			WHEN UPPER(f.[Item Status]) = 'HIDDEN'
				THEN 'true'
			ELSE 'false'
			END AS Inactive
		,[Item Weight] AS ItemWeight
		,[Item Width] AS ItemWidth
		,[IX One Brand] AS IXOneBrand
		,[IX One ID] AS IXOneID
		,[Juice Content] AS JuiceContent
		,[Kitchen Description] AS KitchenDescription
		,NULLIF([Kitchen Item], 'No') AS KitchenItem
		,[Kosher] AS Kosher
		,[Kosher Claim] AS KosherClaim
		,[Labeling] AS Labeling
		,[Line] AS Line
		,[Line Extension] AS LineExtension
		,[Local Loan Producer] AS LocalLoanProducer
		,[Low Fat Claim] AS LowFatClaim
		,NULLIF([Made In House], 'No') AS MadeInHouse
		,[Made with Organic Grapes] AS MadewithOrganicGrapes
		,COALESCE([Modified By], [Created By]) AS ModifiedBy
		,COALESCE([Modified On], [Created On]) AS ModifiedDateTimeUtc
		,NULLIF([MSC], 'No') AS MSC
		,[Multipurpose] AS Multipurpose
		,[Natural Claim] AS NaturalClaim
		,[Non-GMO] AS NonGMO
		,[Non GMO Claim] AS NonGMOClaim
		,[No Sulfites] AS NoSulfites
		,[Notes] AS Notes
		,[Nutrition Required] AS NutritionRequired
		,[Organic] AS Organic
		,[Organic Claim] AS OrganicClaim
		,[Organic Personal Care] AS OrganicPersonalCare
		,[Other 3P Eligible] AS Other3PEligible
		,[Package Group] AS PackageGroup
		,[Package Group Type] AS PackageGroupType
		,[Packaging Type] AS PackagingType
		,[Paleo] AS Paleo
		,[Paleo Claim] AS PaleoClaim
		,NULLIF([Pasture Raised], 'No') AS PastureRaised
		,[PMD Verified] AS PMDVerified
		,[POS Description] AS POSDescription
		,[POS Scale Tare] AS POSScaleTare
		,NULLIF([Premium Body Care], 'No') AS PremiumBodyCare
		,[Price Line] AS PriceLine
		,[Price Line Description] AS PriceLineDescription
		,[Prime] AS Prime
		,[Prime Now Eligible] AS PrimeNowEligible
		,[Private Label] AS PrivateLabel
		,[Product Flavor or Type] AS ProductFlavororType
		,[Product Description] AS ProductDescription
		,CASE 
			WHEN UPPER([Prohibit Discount]) = 'TRUE'
				THEN 'true'
			ELSE 'false'
			END AS ProhibitDiscount
		,[Rainforest Alliance] AS RainforestAlliance
		,NULLIF([Raw], 'No') AS Raw
		,[Refrigerated or Shelf Stable] AS RefrigeratedorShelfStable
		,[Regional Local Item] AS RegionalLocalItem
		,[Rennet] AS Rennet
		,[Retail Size] AS RetailSize
		,[SCO Item Tare Group] AS SCOItemTareGroup
		,[Seafood: Wild Or Farm Raised] AS SeafoodWildOrFarmRaised
		,[Seasonal In and Out/Gifting] AS SeasonalInandOutGifting
		,[Shelf Life] AS ShelfLife
		,[Skin Type] AS SkinType
		,[SKU] AS SKU
		,[Smithsonian Bird Friendly] AS SmithsonianBirdFriendly
		,[Smoked] AS Smoked
		,[Sold Hot or Cold] AS SoldHotorCold
		,[Travel Size/Single Use/Kit] AS TravelSizeSingleUseKit
		,[Tray Depth] AS TrayDepth
		,[Tray Height] AS TrayHeight
		,[Tray Width] AS TrayWidth
		,[UOM] AS UOM
		,[URL1] AS URL1
		,[Value Added] AS ValueAdded
		,[Variant Size] AS VariantSize
		,[Varietal] AS Varietal
		,[Vegan] AS Vegan
		,[Vegan Claim] AS VeganClaim
		,NULLIF([Vegetarian], 'No') AS Vegetarian
		,[Vegetarian Claim] AS VegetarianClaim
		,NULLIF([WFM Eligible], 'No') AS WFMEligible
		,NULLIF([Whole Trade], 'No') AS WholeTrade
		,[ItemName] as RequestNumber
	FROM temp_InforItemsFile f
	WHERE f.ItemID = item.ItemId
	FOR JSON PATH
		,WITHOUT_ARRAY_WRAPPER
	), '{}')

-- Set the SysStartTimeUtc equal to the ModifiedOn which comes from Infor file
PRINT 'Updating Item.SysStartTimeUtc column with the date coming from Infor...'
UPDATE dbo.Item
SET SysStartTimeUtc =  IsNULL(ISNULL(JSON_VALUE(ItemAttributesJson,'$.ModifiedDateTimeUtc'),JSON_VALUE(ItemAttributesJson,'$."CreatedDateTimeUtc"')),SYSUTCDATETIME() )

-- Update ScanCode table with the proper barcode type reference
PRINT 'Updating ScanCode table with proper barcode type mapping...'
UPDATE sc
SET barcodeTypeID = b.BarcodeTypeId
FROM ScanCode sc
INNER JOIN temp_InforItemsFile f ON f.ItemID = sc.ItemId
INNER JOIN dbo.BarcodeType b ON f.BarcodeType = b.BarcodeType

-- Insert Manufacturer values into temp table
PRINT 'Inserting manufacturer data into hierarchy tables...'
INSERT INTO dbo.temp_ItemManufactureHierarchy (
	[ItemID]
	,[Manufacturer])
SELECT
	ItemID
	,[Manufacturer]
FROM temp_InforItemsFile f
WHERE [Manufacturer] IS NOT NULL
	AND [Manufacturer] != '' AND [Manufacturer] != '#N/A' 

DECLARE @ManufactureHierarchyId INT
SET @ManufactureHierarchyId = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Manufacturer')

IF NOT EXISTS (SELECT 1 FROM [dbo].[HierarchyPrototype] WHERE [hierarchyID] = @ManufactureHierarchyId)
BEGIN
	INSERT INTO [dbo].[HierarchyPrototype] (
		[hierarchyID]
		,[hierarchyLevel]
		,[hierarchyLevelName]
		,[itemsAttached])
	SELECT
		@ManufactureHierarchyId
		,1
		,'Manufacturer'
		,1
END

-- Delete any records associated to Manufacturer (there should be none but done in case the DB wasn't refreshed)
DELETE [dbo].[HierarchyClassTrait]
WHERE [hierarchyClassID] IN (
		SELECT [hierarchyClassID]
		FROM [dbo].[HierarchyClass]
		WHERE [hierarchyID] = @ManufactureHierarchyId
		)

DELETE [dbo].[ItemHierarchyClass]
WHERE [hierarchyClassID] IN (
		SELECT [hierarchyClassID]
		FROM [dbo].[HierarchyClass]
		WHERE [hierarchyID] = @ManufactureHierarchyId
		)

DELETE [dbo].[HierarchyClass]
WHERE [hierarchyID] = @ManufactureHierarchyId

-- Insert records into HierarchyClass and ItemHierarchyClass
INSERT INTO [dbo].[HierarchyClass] (
	[hierarchyLevel]
	,[hierarchyID]
	,[hierarchyParentClassID]
	,[hierarchyClassName])
SELECT DISTINCT
	1
	,@ManufactureHierarchyId
	,NULL
	,UPPER([Manufacturer]) -- GDT wants these to be upper case
FROM dbo.temp_ItemManufactureHierarchy

INSERT INTO [dbo].[ItemHierarchyClass] (
	[itemID]
	,[hierarchyClassID]
	,[localeID])
SELECT
	im.[ItemID]
	,[hierarchyClassID]
	,1
FROM dbo.temp_ItemManufactureHierarchy im
INNER JOIN [HierarchyClass] h ON im.Manufacturer = h.hierarchyClassName
	AND h.hierarchyID = @ManufactureHierarchyId
INNER JOIN Item i ON i.ItemId = im.ItemID

-- This block is for updating the item type. The item type comes over from Infor in the format 'Blackhawk Fee'
-- and we have to convert it with this case statment into a code and then into an item type id and then update all of
-- the items
PRINT 'Updating Item with proper ItemTypeID...'
IF OBJECT_ID('tempdb..#tmpItems') IS NOT NULL
	DROP TABLE #tmpItems

SELECT itemId
	,CASE 
		WHEN PARSENAME(REPLACE([Item Type], '|', '.'), 1) = 'Blackhawk Fee'
			THEN 'FEE'
		WHEN PARSENAME(REPLACE([Item Type], '|', '.'), 1) = 'Bottle Deposit'
			THEN 'DEP'
		WHEN PARSENAME(REPLACE([Item Type], '|', '.'), 1) = 'Bottle Return'
			THEN 'RTN'
		WHEN PARSENAME(REPLACE([Item Type], '|', '.'), 1) = 'Coupon'
			THEN 'CPN'
		WHEN PARSENAME(REPLACE([Item Type], '|', '.'), 1) = 'CRV'
			THEN 'DEP'
		WHEN PARSENAME(REPLACE([Item Type], '|', '.'), 1) = 'CRV Credit'
			THEN 'RTN'
		WHEN PARSENAME(REPLACE([Item Type], '|', '.'), 1) = 'Legacy POS Only'
			THEN 'NRT'
		WHEN PARSENAME(REPLACE([Item Type], '|', '.'), 1) = 'N/A'
			THEN 'RTL'
		WHEN PARSENAME(REPLACE([Item Type], '|', '.'), 1) = 'Non-Retail'
			THEN 'NRT'
		END AS ItemTypeCode
INTO #tmpItems
FROM temp_InforItemsFile

UPDATE dbo.Item
SET ItemTypeId = it.ItemTypeId
FROM dbo.item i
INNER JOIN #tmpItems t ON t.itemId = i.ItemId
INNER JOIN ItemType it ON t.itemTypeCode = it.itemTypeCode

-- Matching the modified date for ItemHierarchyClass to match the Item modified date.
PRINT 'Updating ItemHierarchyClass.SysStartTimeUtc...'
UPDATE ihc
SET SysStartTimeUtc = i.SysStartTimeUtc
FROM dbo.ItemHierarchyClass ihc
INNER JOIN Item i ON i.ItemId = ihc.ItemId

-- update all items to have a DepartmentSale attribute with a value of yes if the item has the the department sale(DPT) trait 
-- assigned through ItemTrait.
PRINT 'Updating Items that need to have Department Sale values...'
UPDATE Item
SET ItemAttributesJson = JSON_MODIFY(i.ItemAttributesJson,'$.DepartmentSale', CASE WHEN TraitValue = 1 THEN 'Yes' ELSE 'No' END)
FROM Item i
INNER JOIN ItemTrait it ON i.itemID = it.itemID
	AND it.traitid IN (
		SELECT traitid
		FROM trait
		WHERE traitCode = 'DPT')

-- Update bar code type id on ScanCode table from the backup if there are any records with a NULL value:
IF EXISTS (SELECT 1 FROM ScanCode WHERE barcodeTypeID IS NULL)
BEGIN
UPDATE sc
	SET sc.barcodeTypeID = b.barcodeTypeID
	FROM dbo.ScanCode sc
	JOIN dbo.ScanCodeBackup b on sc.itemID = b.ItemID
	WHERE sc.barcodeTypeID IS NULL
END

PRINT 'Enabling system versioning on Item and ItemHierarchyClass...'
ALTER TABLE dbo.Item ADD PERIOD FOR SYSTEM_TIME(SysStartTimeUtc, SysEndTimeUtc)
ALTER TABLE dbo.Item SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ItemHistory));

ALTER TABLE dbo.ItemHierarchyClass ADD PERIOD FOR SYSTEM_TIME(SysStartTimeUtc, SysEndTimeUtc)
ALTER TABLE dbo.ItemHierarchyClass SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ItemHierarchyClassHistory));

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES t WHERE t.TABLE_SCHEMA = 'dbo' AND t.TABLE_NAME = 'ScanCodeBackup')
	DROP TABLE dbo.ScanCodeBackup

DROP TABLE dbo.temp_InforItemsFile
DROP TABLE dbo.temp_ItemManufactureHierarchy

END TRY
BEGIN CATCH
	PRINT 'Error Occurred'
	SELECT   
		ERROR_NUMBER() AS ErrorNumber  
		,ERROR_MESSAGE() AS ErrorMessage;
END CATCH