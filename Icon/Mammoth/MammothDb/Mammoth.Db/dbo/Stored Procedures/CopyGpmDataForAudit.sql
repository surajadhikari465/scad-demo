CREATE PROCEDURE dbo.CopyGpmDataForAudit
AS
BEGIN
	print 'Starting copy of GPM Data for Audit ' + CAST(SYSDATETIME() AS NVARCHAR(100))

	print 'Truncating existing tables ' + CAST(SYSDATETIME() AS NVARCHAR(100))
	TRUNCATE TABLE Staging.dbo.ItemAttributes_Locale_FL_Audit
	TRUNCATE TABLE Staging.dbo.Items_Audit
	TRUNCATE TABLE Staging.dbo.ItemTypes_Audit
	TRUNCATE TABLE Staging.dbo.Locales_FL_Audit
	TRUNCATE TABLE Staging.gpm.Price_FL_Audit

	--Insert ItemAttributes_Locale_FL
	print 'Inserting ItemAttributes_Locale_FL ' + CAST(SYSDATETIME() AS NVARCHAR(100))
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
	print 'Inserting Items ' + CAST(SYSDATETIME() AS NVARCHAR(100))
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
	print 'Inserting ItemTypes ' + CAST(SYSDATETIME() AS NVARCHAR(100))
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
	print 'Inserting Locales_FL ' + CAST(SYSDATETIME() AS NVARCHAR(100))
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
	print 'Inserting Price_FL ' + CAST(SYSDATETIME() AS NVARCHAR(100))
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

	print 'Finished copying GPM Data for Audit ' + CAST(SYSDATETIME() AS NVARCHAR(100))
END