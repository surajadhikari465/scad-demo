CREATE PROCEDURE dbo.CopyGpmDataForAudit
AS
BEGIN
	PRINT 'Starting copy of GPM Data for Audit ' + CAST(SYSDATETIME() AS NVARCHAR(100))

	PRINT 'Truncating existing tables ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	TRUNCATE TABLE Staging.dbo.ItemAttributes_Locale_FL_Audit
	TRUNCATE TABLE Staging.dbo.Items_Audit
	TRUNCATE TABLE Staging.dbo.ItemTypes_Audit
	TRUNCATE TABLE Staging.dbo.Locales_FL_Audit
	TRUNCATE TABLE Staging.gpm.Price_FL_Audit
	TRUNCATE TABLE Staging.dbo.ItemAttributes_Nutrition_Audit
	TRUNCATE TABLE Staging.dbo.ItemAttributes_Locale_FL_Ext_Audit
	TRUNCATE TABLE Staging.dbo.Attributes_Audit

	--Insert ItemAttributes_Locale_FL
	PRINT 'Inserting ItemAttributes_Locale_FL ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	INSERT INTO Staging.dbo.ItemAttributes_Locale_FL_Audit (
		Region
		,ItemAttributeLocaleID
		,ItemID
		,BusinessUnitID
		,Discount_Case
		,Discount_TM
		,Restriction_Age
		,Restriction_Hours
		,Authorized
		,Discontinued
		,LocalItem
		,ScaleItem
		,OrderedByInfor
		,DefaultScanCode
		,LabelTypeDesc
		,Product_Code
		,RetailUnit
		,Sign_Desc
		,Locality
		,Sign_RomanceText_Long
		,Sign_RomanceText_Short
		,AltRetailUOM
		,AltRetailSize
		,MSRP
		,AddedDate
		,ModifiedDate
		)
	SELECT Region
		,ItemAttributeLocaleID
		,ItemID
		,BusinessUnitID
		,Discount_Case
		,Discount_TM
		,Restriction_Age
		,Restriction_Hours
		,Authorized
		,Discontinued
		,LocalItem
		,ScaleItem
		,OrderedByInfor
		,DefaultScanCode
		,LabelTypeDesc
		,Product_Code
		,RetailUnit
		,Sign_Desc
		,Locality
		,Sign_RomanceText_Long
		,Sign_RomanceText_Short
		,AltRetailUOM
		,AltRetailSize
		,MSRP
		,AddedDate
		,ModifiedDate
	FROM Mammoth.dbo.ItemAttributes_Locale_FL

	--Insert Items
	PRINT 'Inserting Items ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	INSERT INTO Staging.dbo.Items_Audit (
		ItemID
		,ItemTypeID
		,ScanCode
		,HierarchyMerchandiseID
		,HierarchyNationalClassID
		,BrandHCID
		,TaxClassHCID
		,PSNumber
		,Desc_Product
		,Desc_POS
		,PackageUnit
		,RetailSize
		,RetailUOM
		,FoodStampEligible
		,Desc_CustomerFriendly
		,AddedDate
		,ModifiedDate
		)
	SELECT ItemID
		,ItemTypeID
		,ScanCode
		,HierarchyMerchandiseID
		,HierarchyNationalClassID
		,BrandHCID
		,TaxClassHCID
		,PSNumber
		,Desc_Product
		,Desc_POS
		,PackageUnit
		,RetailSize
		,RetailUOM
		,FoodStampEligible
		,Desc_CustomerFriendly
		,AddedDate
		,ModifiedDate
	FROM Mammoth.dbo.Items

	--Insert ItemTypes
	PRINT 'Inserting ItemTypes ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	INSERT INTO Staging.dbo.ItemTypes_Audit (
		itemTypeID
		,itemTypeCode
		,itemTypeDesc
		,AddedDate
		,ModifiedDate
		)
	SELECT itemTypeID
		,itemTypeCode
		,itemTypeDesc
		,AddedDate
		,ModifiedDate
	FROM Mammoth.dbo.ItemTypes

	--Insert Locales_FL
	PRINT 'Inserting Locales_FL ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	INSERT INTO Staging.dbo.Locales_FL_Audit (
		Region
		,LocaleID
		,BusinessUnitID
		,StoreName
		,StoreAbbrev
		,PhoneNumber
		,LocaleOpenDate
		,LocaleCloseDate
		,AddedDate
		,ModifiedDate
		)
	SELECT Region
		,LocaleID
		,BusinessUnitID
		,StoreName
		,StoreAbbrev
		,PhoneNumber
		,LocaleOpenDate
		,LocaleCloseDate
		,AddedDate
		,ModifiedDate
	FROM Mammoth.dbo.Locales_FL

	--Insert Price_FL
	PRINT 'Inserting Price_FL ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	INSERT INTO Staging.gpm.Price_FL_Audit (
		Region
		,PriceID
		,GpmID
		,ItemID
		,BusinessUnitID
		,StartDate
		,EndDate
		,Price
		,PercentOff
		,PriceType
		,PriceTypeAttribute
		,SellableUOM
		,CurrencyCode
		,Multiple
		,TagExpirationDate
		,InsertDateUtc
		,ModifiedDateUtc
		)
	SELECT Region
		,PriceID
		,GpmID
		,ItemID
		,BusinessUnitID
		,StartDate
		,EndDate
		,Price
		,PercentOff
		,PriceType
		,PriceTypeAttribute
		,SellableUOM
		,CurrencyCode
		,Multiple
		,TagExpirationDate
		,InsertDateUtc
		,ModifiedDateUtc
	FROM Mammoth.gpm.Price_FL

	-- Insert Attributes Table
	PRINT 'Inserting Attributes ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	INSERT INTO Staging.dbo.Attributes_Audit
		(AttributeID
		,AttributeGroupID
		,AttributeCode
		,AttributeDesc
		,AddedDate
		,ModifiedDate)
	SELECT AttributeID 
		,AttributeGroupID
		,AttributeCode
		,AttributeDesc
		,AddedDate
		,ModifiedDate
	FROM Mammoth.dbo.Attributes

	-- Insert ItemLocale Extended Attributes
	PRINT 'Inserting ItemAttributes_Locale_FL_Ext ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	INSERT INTO Staging.dbo.ItemAttributes_Locale_FL_Ext_Audit
		(Region
		,ItemAttributeLocaleID
		,ItemID
		,LocaleID
		,AttributeID
		,AttributeValue
		,AddedDate
		,ModifiedDate)
	SELECT Region
		  ,ItemAttributeLocaleID
		  ,ItemID
		  ,LocaleID
		  ,AttributeID
		  ,AttributeValue
		  ,AddedDate
		  ,ModifiedDate
	FROM Mammoth.dbo.ItemAttributes_Locale_FL_Ext

	-- Insert ItemAttributes_Nutrition
	PRINT 'Inserting ItemAttributes_Nutrition ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	INSERT INTO Staging.dbo.ItemAttributes_Nutrition_Audit
		(ItemAttributeID
		,ItemID
		,RecipeName
		,Allergens
		,Ingredients
		,ServingsPerPortion
		,ServingSizeDesc
		,ServingPerContainer
		,HshRating
		,ServingUnits
		,SizeWeight
		,Calories
		,CaloriesFat
		,CaloriesSaturatedFat
		,TotalFatWeight
		,TotalFatPercentage
		,SaturatedFatWeight
		,SaturatedFatPercent
		,PolyunsaturatedFat
		,MonounsaturatedFat
		,CholesterolWeight
		,CholesterolPercent
		,SodiumWeight
		,SodiumPercent
		,PotassiumWeight
		,PotassiumPercent
		,TotalCarbohydrateWeight
		,TotalCarbohydratePercent
		,DietaryFiberWeight
		,DietaryFiberPercent
		,SolubleFiber
		,InsolubleFiber
		,Sugar
		,SugarAlcohol
		,OtherCarbohydrates
		,ProteinWeight
		,ProteinPercent
		,VitaminA
		,Betacarotene
		,VitaminC
		,Calcium
		,Iron
		,VitaminD
		,VitaminE
		,Thiamin
		,Riboflavin
		,Niacin
		,VitaminB6
		,Folate
		,VitaminB12
		,Biotin
		,PantothenicAcid
		,Phosphorous
		,Iodine
		,Magnesium
		,Zinc
		,Copper
		,Transfat
		,CaloriesFromTransFat
		,Om6Fatty
		,Om3Fatty
		,Starch
		,Chloride
		,Chromium
		,VitaminK
		,Manganese
		,Molybdenum
		,Selenium
		,TransFatWeight
		,AddedDate
		,ModifiedDate)
	SELECT ItemAttributeID
		,ItemID
		,RecipeName
		,Allergens
		,Ingredients
		,ServingsPerPortion
		,ServingSizeDesc
		,ServingPerContainer
		,HshRating
		,ServingUnits
		,SizeWeight
		,Calories
		,CaloriesFat
		,CaloriesSaturatedFat
		,TotalFatWeight
		,TotalFatPercentage
		,SaturatedFatWeight
		,SaturatedFatPercent
		,PolyunsaturatedFat
		,MonounsaturatedFat
		,CholesterolWeight
		,CholesterolPercent
		,SodiumWeight
		,SodiumPercent
		,PotassiumWeight
		,PotassiumPercent
		,TotalCarbohydrateWeight
		,TotalCarbohydratePercent
		,DietaryFiberWeight
		,DietaryFiberPercent
		,SolubleFiber
		,InsolubleFiber
		,Sugar
		,SugarAlcohol
		,OtherCarbohydrates
		,ProteinWeight
		,ProteinPercent
		,VitaminA
		,Betacarotene
		,VitaminC
		,Calcium
		,Iron
		,VitaminD
		,VitaminE
		,Thiamin
		,Riboflavin
		,Niacin
		,VitaminB6
		,Folate
		,VitaminB12
		,Biotin
		,PantothenicAcid
		,Phosphorous
		,Iodine
		,Magnesium
		,Zinc
		,Copper
		,Transfat
		,CaloriesFromTransFat
		,Om6Fatty
		,Om3Fatty
		,Starch
		,Chloride
		,Chromium
		,VitaminK
		,Manganese
		,Molybdenum
		,Selenium
		,TransFatWeight
		,AddedDate
		,ModifiedDate
	FROM Mammoth.dbo.ItemAttributes_Nutrition

	PRINT 'Finished copying GPM Data for Audit ' + CAST(SYSDATETIME() AS NVARCHAR(100))
END