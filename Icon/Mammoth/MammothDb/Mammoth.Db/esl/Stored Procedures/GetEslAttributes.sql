CREATE PROCEDURE [esl].[GetEslAttributes]
	@BusinessUnitID INT,
	@StartDateTimeUtc DATETIME2(7),
	@EndDateTimeUtc DATETIME2(7)
AS
BEGIN

SET NOCOUNT ON;

-- Set Attribute IDs
DECLARE @flexTextId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'FXT');
DECLARE @fairTradeId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'FT');
DECLARE @madeOrgGrapes INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'MOG');
DECLARE @madeBiodynamicGrapes INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'MBG');
DECLARE @nutritionRequiredId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'NR');
DECLARE @primeBeefId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'PRB');
DECLARE @rainforestAllianceId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'RFA');
DECLARE @refrigerateId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'RFD');
DECLARE @smithsonianBirdId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'SMF');
DECLARE @wicId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'WIC');
DECLARE @globalPricingProgramId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'GPP');
DECLARE @colorAddedId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'CLA');
DECLARE @chicagoBabyId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'CHB');
DECLARE @countryOfProcessingId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'COP');
DECLARE @electronicShelfTagId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'EST');
DECLARE @exclusiveId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'EX');
DECLARE @linkedScanCode INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'LSC');
DECLARE @numDigitsScaleId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'NDS');
DECLARE @originId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'ORN');
DECLARE @scaleExtraTextId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'SET');
DECLARE @tagUomId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'TU');
DECLARE @percentageTareWeightId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'PTA');

-- Other local variables
DECLARE @Region NVARCHAR(2) = (SELECT Region FROM Locale WHERE BusinessUnitID = @BusinessUnitID);

-- Put ItemIDs we need into a temp table
-- Optional Attributes to avoid a left join on main global item query
CREATE TABLE #itemExtended
(
	Region NCHAR(2) NOT NULL,
	ItemID INT NOT NULL,
	BusinessUnitID INT NOT NULL,
	FlexibleText NVARCHAR(255) NULL,
	WIC NVARCHAR(255) NULL,
	FairTrade NVARCHAR(255) NULL,
	MadeWithOrgGrapes NVARCHAR(255) NULL,
	NutritionRequired NVARCHAR(255) NULL,
	PrimeBeef NVARCHAR(255) NULL,
	RainforestAlliance NVARCHAR(255) NULL,
	Refrigerated NVARCHAR(255) NULL,
	SmithsonianBirdFriendly NVARCHAR(255) NULL,
	GlobalPricingProgram NVARCHAR(255) NULL,
	ColorAdded NVARCHAR(MAX) NULL,
	ChicagoBaby NVARCHAR(MAX) NULL,
	CountryOfProcessing NVARCHAR(MAX) NULL,
	ElectronicShelfTag NVARCHAR(MAX) NULL,
	Exclusive NVARCHAR(MAX) NULL,
	LinkedScanCode NVARCHAR(MAX) NULL,
	LinkedScanCodeBrand NVARCHAR(255) NULL,
	NumberOfDigitsSentToScale NVARCHAR(MAX) NULL,
	Origin NVARCHAR(MAX) NULL,
	PercentageTareWeight NVARCHAR(255) NULL,
	ScaleExtraText NVARCHAR(MAX) NULL,
	TagUOM NVARCHAR(MAX) NULL,
	CheeseMilkType nvarchar(255) NULL,
	Agency_GlutenFree nvarchar(255) NULL,
	Agency_Kosher nvarchar(255) NULL,
	Agency_NonGMO nvarchar(255) NULL,
	Agency_Organic nvarchar(255) NULL,
	Agency_Vegan nvarchar(255) NULL,
	IsAirChilled bit NULL,
	IsBiodynamic bit NULL,
	IsCheeseRaw bit NULL,
	IsDryAged bit NULL,
	IsFreeRange bit NULL,
	IsGrassFed bit NULL,
	IsMadeInHouse bit NULL,
	IsMsc bit NULL,
	IsPastureRaised bit NULL,
	IsPremiumBodyCare bit NULL,
	IsVegetarian bit NULL,
	IsWholeTrade bit NULL,
	Rating_AnimalWelfare nvarchar(255) NULL,
	Rating_EcoScale nvarchar(255) NULL,
	Rating_HealthyEating nvarchar(255) NULL,
	Seafood_FreshOrFrozen nvarchar(255) NULL,
	Seafood_CatchType nvarchar(255) NULL,
	RecipeName nvarchar(100) NULL,
	Allergens nvarchar(510) NULL,
	Ingredients nvarchar(4000) NULL,
	ServingsPerPortion float NULL,
	ServingSizeDesc nvarchar(50) NULL,
	ServingPerContainer nvarchar(50) NULL,
	HshRating int NULL,
	ServingUnits tinyint NULL,
	SizeWeight int NULL,
	Calories int NULL,
	CaloriesFat int NULL,
	CaloriesSaturatedFat int NULL,
	TotalFatWeight decimal(10, 1) NULL,
	TotalFatPercentage smallint NULL,
	SaturatedFatWeight decimal(10, 1) NULL,
	SaturatedFatPercent smallint NULL,
	PolyunsaturatedFat decimal(10, 1) NULL,
	MonounsaturatedFat decimal(10, 1) NULL,
	CholesterolWeight decimal(10, 1) NULL,
	CholesterolPercent smallint NULL,
	SodiumWeight decimal(10, 1) NULL,
	SodiumPercent smallint NULL,
	PotassiumWeight decimal(10, 1) NULL,
	PotassiumPercent smallint NULL,
	TotalCarbohydrateWeight decimal(10, 1) NULL,
	TotalCarbohydratePercent smallint NULL,
	DietaryFiberWeight decimal(10, 1) NULL,
	DietaryFiberPercent smallint NULL,
	SolubleFiber decimal(10, 1) NULL,
	InsolubleFiber decimal(10, 1) NULL,
	Sugar decimal(10, 1) NULL,
	SugarAlcohol decimal(10, 1) NULL,
	OtherCarbohydrates decimal(10, 1) NULL,
	ProteinWeight decimal(10, 1) NULL,
	ProteinPercent smallint NULL,
	VitaminA smallint NULL,
	Betacarotene smallint NULL,
	VitaminC smallint NULL,
	Calcium smallint NULL,
	Iron smallint NULL,
	VitaminD smallint NULL,
	VitaminE smallint NULL,
	Thiamin smallint NULL,
	Riboflavin smallint NULL,
	Niacin smallint NULL,
	VitaminB6 smallint NULL,
	Folate smallint NULL,
	VitaminB12 smallint NULL,
	Biotin smallint NULL,
	PantothenicAcid smallint NULL,
	Phosphorous smallint NULL,
	Iodine smallint NULL,
	Magnesium smallint NULL,
	Zinc smallint NULL,
	Copper smallint NULL,
	Transfat decimal(10, 1) NULL,
	CaloriesFromTransFat int NULL,
	Om6Fatty decimal(10, 1) NULL,
	Om3Fatty decimal(10, 1) NULL,
	Starch decimal(10, 1) NULL,
	Chloride smallint NULL,
	Chromium smallint NULL,
	VitaminK smallint NULL,
	Manganese smallint NULL,
	Molybdenum smallint NULL,
	Selenium smallint NULL,
	TransFatWeight decimal(10, 1) NULL
)

-- Pull ItemStore Keys from the staging table while also deleting at the same time
DELETE FROM [stage].[ItemStoreKeysEsl]
	OUTPUT 
		@Region					as Region,
		deleted.ItemID			as ItemID,
		deleted.BusinessUnitID	as BusinessUnitID
	INTO #itemExtended (Region, ItemID, BusinessUnitID)
WHERE BusinessUnitID = @BusinessUnitID
	AND InsertDateUtc BETWEEN @StartDateTimeUtc AND @EndDateTimeUtc

CREATE NONCLUSTERED INDEX IX_#itemStores_Region_ItemID_BusinessUnitID ON #ItemExtended (Region ASC, ItemID ASC, BusinessUnitID ASC);

UPDATE ist
SET FlexibleText = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @flexTextId

UPDATE ist
SET WIC = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @wicId

UPDATE ist
SET FairTrade = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @fairTradeId

UPDATE ist
SET MadeWithOrgGrapes = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @madeOrgGrapes

UPDATE ist
SET NutritionRequired = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @nutritionRequiredId

UPDATE ist
SET PrimeBeef = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @primeBeefId

UPDATE ist
SET RainforestAlliance = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @rainforestAllianceId

UPDATE ist
SET Refrigerated = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @refrigerateId

UPDATE ist
SET SmithsonianBirdFriendly = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @smithsonianBirdId

UPDATE ist
SET GlobalPricingProgram = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @globalPricingProgramId

UPDATE ist
SET PercentageTareWeight = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
							AND ia.AttributeID = @percentageTareWeightId

--sign attributes
UPDATE ist
SET 
	Agency_GlutenFree = sgn.Agency_GlutenFree,
	Agency_Kosher = sgn.Agency_Kosher,
	Agency_NonGMO = sgn.Agency_NonGMO,
	Agency_Organic = sgn.Agency_Organic,
	Agency_Vegan = sgn.Agency_Vegan,
	CheeseMilkType = sgn.CheeseMilkType,
	IsAirChilled = sgn.IsAirChilled,
	IsBiodynamic = sgn.IsBiodynamic,
	IsCheeseRaw = sgn.IsCheeseRaw,
	IsDryAged = sgn.IsDryAged,
	IsFreeRange = sgn.IsFreeRange,
	IsGrassFed = sgn.IsGrassFed,
	IsMadeInHouse = sgn.IsMadeInHouse,
	IsMsc = sgn.IsMsc,
	IsPastureRaised = sgn.IsPastureRaised,
	IsPremiumBodyCare = sgn.IsPremiumBodyCare,
	IsVegetarian = sgn.IsVegetarian,
	IsWholeTrade = sgn.IsWholeTrade,
	Rating_AnimalWelfare = sgn.Rating_AnimalWelfare,
	Rating_EcoScale = sgn.Rating_EcoScale,
	Rating_HealthyEating = sgn.Rating_HealthyEating,
	Seafood_CatchType = sgn.Seafood_CatchType,
	Seafood_FreshOrFrozen = sgn.Seafood_FreshOrFrozen
FROM #itemExtended ist
JOIN dbo.ItemAttributes_Sign sgn on ist.ItemID = sgn.ItemID

--nutrition attributes
UPDATE ist
SET
	RecipeName = n.RecipeName,
	Allergens = n.Allergens,
	Ingredients = n.Ingredients,
	ServingsPerPortion = n.ServingsPerPortion,
	ServingSizeDesc = n.ServingSizeDesc,
	ServingPerContainer = n.ServingPerContainer,
	HshRating = n.HshRating,
	ServingUnits = n.ServingUnits,
	SizeWeight = n.SizeWeight,
	Calories = n.Calories,
	CaloriesFat = n.CaloriesFat,
	CaloriesSaturatedFat = n.CaloriesSaturatedFat,
	TotalFatWeight = n.TotalFatWeight,
	TotalFatPercentage = n.TotalFatPercentage,
	SaturatedFatWeight = n.SaturatedFatWeight,
	SaturatedFatPercent = n.SaturatedFatPercent,
	PolyunsaturatedFat = n.PolyunsaturatedFat,
	MonounsaturatedFat = n.MonounsaturatedFat,
	CholesterolWeight = n.CholesterolWeight,
	CholesterolPercent = n.CholesterolPercent,
	SodiumWeight = n.SodiumWeight,
	SodiumPercent = n.SodiumPercent,
	PotassiumWeight = n.PotassiumWeight,
	PotassiumPercent = n.PotassiumPercent,
	TotalCarbohydrateWeight = n.TotalCarbohydrateWeight,
	TotalCarbohydratePercent = n.TotalCarbohydratePercent,
	DietaryFiberWeight = n.DietaryFiberWeight,
	DietaryFiberPercent = n.DietaryFiberPercent,
	SolubleFiber = n.SolubleFiber,
	InsolubleFiber = n.InsolubleFiber,
	Sugar = n.Sugar,
	SugarAlcohol = n.SugarAlcohol,
	OtherCarbohydrates = n.OtherCarbohydrates,
	ProteinWeight = n.ProteinWeight,
	ProteinPercent = n.ProteinPercent,
	VitaminA = n.VitaminA,
	Betacarotene = n.Betacarotene,
	VitaminC = n.VitaminC,
	Calcium = n.Calcium,
	Iron = n.Iron,
	VitaminD = n.VitaminD,
	VitaminE = n.VitaminE,
	Thiamin = n.Thiamin,
	Riboflavin = n.Riboflavin,
	Niacin = n.Niacin,
	VitaminB6 = n.VitaminB6,
	Folate = n.Folate,
	VitaminB12 = n.VitaminB12,
	Biotin = n.Biotin,
	PantothenicAcid = n.PantothenicAcid,
	Phosphorous = n.Phosphorous,
	Iodine = n.Iodine,
	Magnesium = n.Magnesium,
	Zinc = n.Zinc,
	Copper = n.Copper,
	Transfat = n.Transfat,
	CaloriesFromTransFat = n.CaloriesFromTransFat,
	Om6Fatty = n.Om6Fatty,
	Om3Fatty = n.Om3Fatty,
	Starch = n.Starch,
	Chloride = n.Chloride,
	Chromium = n.Chromium,
	VitaminK = n.VitaminK,
	Manganese = n.Manganese,
	Molybdenum = n.Molybdenum,
	Selenium = n.Selenium,
	TransFatWeight = n.TransFatWeight
FROM #itemExtended ist
JOIN dbo.ItemAttributes_Nutrition n on ist.ItemID = n.ItemID

-- Global Item Data
SELECT
	i.ItemID,
	ist.BusinessUnitID,
	i.ScanCode,
	t.itemTypeDesc,
	b.HierarchyClassName	AS BrandName,
	st.Name					AS SubTeamName,
	st.PSNumber				AS SubTeamNumber,
	i.Desc_Product,
	i.Desc_POS,
	i.Desc_CustomerFriendly,
	i.RetailSize,
	i.RetailUOM,
	i.PackageUnit,
	sb.HierarchyClassName	AS MerchandiseSubBrickName,
	ist.FairTrade,
	ist.FlexibleText,
	ist.MadeWithOrgGrapes,
	ist.WIC,
	ist.PrimeBeef,
	ist.RainforestAlliance,
	ist.Refrigerated,
	ist.SmithsonianBirdFriendly,
	ist.NutritionRequired,
	ist.GlobalPricingProgram,
	ist.PercentageTareWeight,
	ist.Agency_GlutenFree,
	ist.Agency_Kosher,
	ist.Agency_NonGMO,
	ist.Agency_Organic,
	ist.Agency_Vegan,
	ist.CheeseMilkType,
	ist.IsAirChilled,
	ist.IsBiodynamic,
	ist.IsCheeseRaw,
	ist.IsDryAged,
	ist.IsFreeRange,
	ist.IsGrassFed,
	ist.IsMadeInHouse,
	ist.IsMsc,
	ist.IsPastureRaised,
	ist.IsPremiumBodyCare,
	ist.IsVegetarian,
	ist.IsWholeTrade,
	ist.Rating_AnimalWelfare,
	ist.Rating_EcoScale,
	ist.Rating_HealthyEating,
	ist.Seafood_CatchType,
	ist.Seafood_FreshOrFrozen,
    ist.RecipeName,
    ist.Allergens,
    ist.Ingredients,
    ist.ServingsPerPortion,
    ist.ServingSizeDesc,
    ist.ServingPerContainer,
    ist.HshRating,
    ist.ServingUnits,
    ist.SizeWeight,
    ist.Calories,
    ist.CaloriesFat,
    ist.CaloriesSaturatedFat,
    ist.TotalFatWeight,
    ist.TotalFatPercentage,
    ist.SaturatedFatWeight,
    ist.SaturatedFatPercent,
    ist.PolyunsaturatedFat,
    ist.MonounsaturatedFat,
    ist.CholesterolWeight,
    ist.CholesterolPercent,
    ist.SodiumWeight,
    ist.SodiumPercent,
    ist.PotassiumWeight,
    ist.PotassiumPercent,
    ist.TotalCarbohydrateWeight,
    ist.TotalCarbohydratePercent,
    ist.DietaryFiberWeight,
    ist.DietaryFiberPercent,
    ist.SolubleFiber,
    ist.InsolubleFiber,
    ist.Sugar,
    ist.SugarAlcohol,
    ist.OtherCarbohydrates,
    ist.ProteinWeight,
    ist.ProteinPercent,
    ist.VitaminA,
    ist.Betacarotene,
    ist.VitaminC,
    ist.Calcium,
    ist.Iron,
    ist.VitaminD,
    ist.VitaminE,
    ist.Thiamin,
    ist.Riboflavin,
    ist.Niacin,
    ist.VitaminB6,
    ist.Folate,
    ist.VitaminB12,
    ist.Biotin,
    ist.PantothenicAcid,
    ist.Phosphorous,
    ist.Iodine,
    ist.Magnesium,
    ist.Zinc,
    ist.Copper,
    ist.Transfat,
    ist.CaloriesFromTransFat,
    ist.Om6Fatty,
    ist.Om3Fatty,
    ist.Starch,
    ist.Chloride,
    ist.Chromium,
    ist.VitaminK,
    ist.Manganese,
    ist.Molybdenum,
    ist.Selenium,
    ist.TransFatWeight
INTO #globalItem
FROM #itemExtended			ist
INNER JOIN dbo.Items				i	on ist.ItemID = i.ItemID
INNER JOIN dbo.ItemTypes			t	on i.ItemTypeID = t.itemTypeID
INNER JOIN dbo.Financial_SubTeam	st	on i.PSNumber = st.PSNumber
INNER JOIN dbo.HierarchyClass		b	on i.BrandHCID = b.HierarchyClassID
INNER JOIN dbo.Hierarchy_Merchandise m	on i.HierarchyMerchandiseID = m.HierarchyMerchandiseID
INNER JOIN dbo.HierarchyClass		sb	on m.SubBrickHCID = sb.HierarchyClassID

CREATE NONCLUSTERED INDEX IX_#globalItem_ItemID ON #globalItem (ItemID ASC, BusinessUnitID ASC)

-- ItemLocale specific data
UPDATE ist
SET ColorAdded = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
										AND ist.ItemID = ia.ItemID
										AND l.LocaleID = ia.LocaleID
										AND ia.AttributeID = @colorAddedId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET ChicagoBaby = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
										AND ist.ItemID = ia.ItemID
										AND l.LocaleID = ia.LocaleID
										AND ia.AttributeID = @chicagoBabyId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET CountryOfProcessing = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
										AND ist.ItemID = ia.ItemID
										AND l.LocaleID = ia.LocaleID
										AND ia.AttributeID = @countryOfProcessingId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET ElectronicShelfTag = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
										AND ist.ItemID = ia.ItemID
										AND l.LocaleID = ia.LocaleID
										AND ia.AttributeID = @electronicShelfTagId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET Exclusive = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
										AND ist.ItemID = ia.ItemID
										AND l.LocaleID = ia.LocaleID
										AND ia.AttributeID = @exclusiveId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET
	LinkedScanCode = ia.AttributeValue,
	LinkedScanCodeBrand = hc.HierarchyClassName
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on ist.Region = ia.Region
									AND ist.ItemID = ia.ItemID
									AND l.LocaleID = ia.LocaleID
									AND ia.AttributeID = @linkedScanCode
JOIN Items						i	on ia.AttributeValue = i.ScanCode
JOIN HierarchyClass				hc	on i.BrandHCID = hc.HierarchyClassID
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET NumberOfDigitsSentToScale = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
										AND ist.ItemID = ia.ItemID
										AND l.LocaleID = ia.LocaleID
										AND ia.AttributeID = @numDigitsScaleId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET Origin = ia.AttributeValue
FROM #itemExtended				ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
										AND ist.ItemID = ia.ItemID
										AND l.LocaleID = ia.LocaleID
										AND ia.AttributeID = @originId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET ScaleExtraText = ia.AttributeValue
FROM #itemExtended				ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
										AND ist.ItemID = ia.ItemID
										AND l.LocaleID = ia.LocaleID
										AND ia.AttributeID = @scaleExtraTextId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET TagUOM = ia.AttributeValue
FROM #itemExtended				ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
										AND ist.ItemID = ia.ItemID
										AND l.LocaleID = ia.LocaleID
										AND ia.AttributeID = @tagUomId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

-- Item Locale Attributes
SELECT
	il.Region,
	il.ItemID,
	l.BusinessUnitID,
	l.StoreName,
	v.SupplierItemID,
	v.SupplierName,
	v.IrmaVendorKey,
	v.SupplierCaseSize,
	il.Authorized,
	il.Discontinued,
	il.LabelTypeDesc,
	il.LocalItem,
	il.Locality,
	il.MSRP,
	il.Product_Code,
	il.RetailUnit,
	il.Sign_Desc,
	il.Sign_RomanceText_Long,
	il.Sign_RomanceText_Short,
	il.AltRetailSize,
	il.AltRetailUOM,
	il.OrderedByInfor,
	il.ScaleItem,
	ist.ColorAdded,
	ist.ChicagoBaby,
	ist.CountryOfProcessing,
	ist.ElectronicShelfTag,
	ist.Exclusive,
	ist.LinkedScanCode,
	ist.LinkedScanCodeBrand,
	ist.NumberOfDigitsSentToScale,
	ist.Origin,
	ist.ScaleExtraText,
	ist.TagUOM
INTO #itemLocale
FROM #itemExtended				ist
JOIN dbo.Locale					l	on	ist.BusinessUnitID = l.BusinessUnitID
JOIN dbo.ItemLocaleAttributes	il	on	ist.Region = il.Region
	AND ist.ItemID = il.ItemID
	AND ist.BusinessUnitID = il.BusinessUnitID
	AND il.Authorized = 1
JOIN dbo.ItemLocaleSupplier		v	on	ist.Region = v.Region
	AND ist.ItemID = v.ItemID
	AND ist.BusinessUnitID = v.BusinessUnitID
WHERE l.Region = @Region
AND il.Region = @Region
AND v.Region = @Region
OPTION (RECOMPILE)

CREATE NONCLUSTERED INDEX IX_#itemLocale_ItemID_BusinessUnitID ON #itemLocale (ItemID ASC, BusinessUnitID ASC)

-- Gather Price Data
-- Regular Prices
SELECT
	p.Region,
	p.ItemID,
	p.BusinessUnitID,
	MAX(StartDate)  as StartDate,
	p.PriceType
INTO #regularPriceKeys
FROM #itemExtended  st
JOIN gpm.Prices p		on st.ItemID = p.ItemID
						AND st.BusinessUnitID = p.BusinessUnitID	 
WHERE 
	p.Region = @Region
	AND p.StartDate <= CAST(CAST(GETDATE() as date) as datetime)
	AND p.PriceType = 'REG'
GROUP BY
	p.Region,
	p.ItemID,
	p.BusinessUnitID,
	p.PriceType
OPTION (RECOMPILE)

CREATE NONCLUSTERED INDEX IX_#regularPriceKeys ON #regularPriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType);

-- Sale Price Keys
SELECT
	p.Region,
	p.ItemID,
	p.BusinessUnitID,
	MAX(StartDate)  as StartDate,
	p.PriceType
INTO #salePriceKeys
FROM #itemExtended		st
JOIN gpm.Prices p		on st.ItemID = p.ItemID
						AND st.BusinessUnitID = p.BusinessUnitID	
WHERE
	p.Region = @Region
	AND p.StartDate <= CAST(CAST(GETDATE() as date) as datetime)
	AND p.EndDate >= CAST(CAST(GETDATE() as date) as datetime)
	AND p.PriceType = 'TPR'
GROUP BY
	p.Region,
	p.ItemID,
	p.BusinessUnitID,
	p.PriceType
OPTION (RECOMPILE)

CREATE NONCLUSTERED INDEX IX_#salePriceKeys ON #salePriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType);

-- Rewards Price Keys
SELECT
	p.Region,
	p.ItemID,
	p.BusinessUnitID,
	MAX(StartDate)  as StartDate,
	p.PriceType
INTO #rewardPriceKeys
FROM #itemExtended		st
JOIN gpm.Prices p		on st.ItemID = p.ItemID
						AND st.BusinessUnitID = p.BusinessUnitID	
WHERE
	p.Region = @Region
	AND p.StartDate <= CAST(CAST(GETDATE() as date) as datetime)
	AND p.EndDate >= CAST(CAST(GETDATE() as date) as datetime)
	AND p.PriceType = 'RWD'
GROUP BY
	p.Region,
	p.ItemID,
	p.BusinessUnitID,
	p.PriceType
OPTION (RECOMPILE)

CREATE NONCLUSTERED INDEX IX_#rewardPriceKeys ON #rewardPriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType);

-- Linked Scan Code Price Keys
SELECT
	p.Region,
	p.ItemID,
	p.BusinessUnitID,
	MAX(StartDate)  as StartDate,
	p.PriceType
INTO #linkedScanCodePriceKeys
FROM #itemExtended		st
JOIN dbo.Items			i	on st.LinkedScanCode = i.ScanCode
JOIN gpm.Prices			p	on i.ItemID = p.ItemID
							AND st.BusinessUnitID = p.BusinessUnitID
WHERE
	p.Region = @Region
	AND p.StartDate <= CAST(CAST(GETDATE() as date) as datetime)
	AND p.PriceType = 'REG'
GROUP BY
	p.Region,
	p.ItemID,
	p.BusinessUnitID,
	p.PriceType
OPTION (RECOMPILE)

CREATE NONCLUSTERED INDEX IX_#linkedScanCodePriceKeys ON #linkedScanCodePriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType);

-- Set prices for REGs, TPRs, RWDs, and LinkedScanCodePrice separately
CREATE TABLE #prices
(
    ItemID INT,
    BusinessUnitID INT,
    RegularPriceMultiple TINYINT,
    RegularPrice DECIMAL(9, 2),
    RegularStartDate DATETIME2,
    RegularPriceType NVARCHAR(3),
    RegularPriceTypeAttribute NVARCHAR(10),
    RegularSellableUOM NVARCHAR(3),
	NewTagExpiration DATETIME2(7),
    TprMultiple TINYINT,
    TprPrice DECIMAL(9, 2),
    TprPriceType NVARCHAR(3),
    TprPriceTypeAttribute NVARCHAR(10),
    TprSellableUOM NVARCHAR(3),
    TprStartDate DATETIME2,
    TprEndDate DATETIME2,
	RewardPrice DECIMAL(9, 2) NULL,
	RewardPriceType NVARCHAR(3) NULL,
	RewardPriceTypeAttribute NVARCHAR(10) NULL,
	RewardPriceMultiple TINYINT NULL,
	RewardPriceSellableUOM NVARCHAR(3) NULL,
	RewardPriceStartDate DATETIME2,
	RewardPriceEndDate DATETIME2,
	LinkedScanCodePrice DECIMAL(9, 2) NULL,
)

INSERT INTO #prices
SELECT
       reg.ItemID,
       reg.BusinessUnitID,
       reg.Multiple				AS RegularPriceMultiple,
       reg.Price                AS RegularPrice,
       reg.StartDate            AS RegularStartDate,
       reg.PriceType            AS RegularPriceType,
       reg.PriceTypeAttribute   AS RegularPriceTypeAttribute,
       reg.SellableUOM          AS RegularSellableUOM,
	   reg.NewTagExpiration		AS NewTagExpiration,
       null                     AS TprMultiple,
       null                     AS TprPrice,
       null                     AS TprPriceType,
       null                     AS TprPriceTypeAttribute,
       null                     AS TprSellableUOM,
       null                     AS TprStartDate,
       null                     AS TprEndDate,
	   null						AS RewardPrice,
	   null						AS RewardPriceType,
	   null						AS RewardPriceTypeAttribute,
	   null						AS RewardPriceMultiple,
	   null						AS RewardPriceSellableUOM,
	   null						AS RewardPriceStartDate,
	   null						AS RewardPriceEndDate,
	   null						AS LinkedScanCodePrice

FROM #regularPriceKeys    pr
JOIN gpm.Prices	          reg    ON	pr.Region = reg.Region
    AND pr.ItemID = reg.ItemID
    AND pr.BusinessUnitID = reg.BusinessUnitID
    AND pr.StartDate = reg.StartDate
    AND pr.PriceType = reg.PriceType
WHERE reg.Region = @Region
OPTION (RECOMPILE)

-- Price
UPDATE #prices
SET
	TprMultiple = sal.Multiple,     
	TprPrice = sal.Price,     
	TprPriceType = sal.PriceType,
	TprPriceTypeAttribute = sal.PriceTypeAttribute,
	TprSellableUOM =sal.SellableUOM,
	TprStartDate = sal.StartDate,
	TprEndDate = sal.EndDate
FROM #salePriceKeys sr     
INNER JOIN gpm.Prices   sal    ON	sr.Region = sal.Region
    AND sr.ItemID = sal.ItemID
	AND sr.BusinessUnitID = sal.BusinessUnitID
	AND sr.StartDate = sal.StartDate
	AND sr.PriceType = sal.PriceType
WHERE sal.Region = @Region
OPTION (RECOMPILE)

-- Reward Price
UPDATE #prices
SET
	RewardPrice = rwd.Price,
	RewardPriceType = rwd.PriceType,
	RewardPriceTypeAttribute = rwd.PriceTypeAttribute,
	RewardPriceMultiple = rwd.Multiple,
	RewardPriceSellableUOM = rwd.SellableUOM,
	RewardPriceStartDate = rwd.StartDate,
	RewardPriceEndDAte = rwd.EndDate
FROM #rewardPriceKeys	rp     
INNER JOIN gpm.Prices   rwd    ON	rp.Region = rwd.Region
    AND rp.ItemID = rwd.ItemID
	AND rp.BusinessUnitID = rwd.BusinessUnitID
	AND rp.StartDate = rwd.StartDate
	AND rp.PriceType = rwd.PriceType
WHERE rwd.Region = @Region
OPTION (RECOMPILE)

-- Linked ScanCode Price
UPDATE #prices
SET LinkedScanCodePrice = p.Price
FROM #linkedScanCodePriceKeys lp
INNER JOIN gpm.Prices	p   ON	lp.Region = p.Region
	AND lp.ItemID = p.ItemID
	AND lp.BusinessUnitID = p.BusinessUnitID
	AND lp.StartDate = p.StartDate
	AND lp.PriceType = p.PriceType
WHERE p.Region = @Region
OPTION (RECOMPILE)

CREATE NONCLUSTERED INDEX IX_#prices_ItemID_BusinessUnitID ON #prices (ItemID ASC, BusinessUnitID ASC)
	INCLUDE
	(
		RegularPriceMultiple,
		RegularPrice,
		RegularStartDate,
		RegularPriceType,
		RegularPriceTypeAttribute,
		RegularSellableUOM,
		NewTagExpiration,
		TprPrice,
		TprPriceType,
		TprPriceTypeAttribute,
		TprSellableUOM,
		TprStartDate,
		TprEndDate,
		RewardPrice,
		RewardPriceType,
		RewardPriceTypeAttribute,
		RewardPriceMultiple,
		RewardPriceSellableUOM,
		RewardPriceStartDate,
		RewardPriceEndDate,
		LinkedScanCodePrice
	);

-- Bring it all together
SELECT
	il.Region					AS RegionAbbrev,
	g.ItemID,
	il.BusinessUnitID,
	il.StoreName,
	sa.TerritoryAbbrev			AS GeographicalState,
	g.ScanCode,
	g.itemTypeDesc				AS ItemType,
	g.BrandName,
	g.SubTeamName,
	g.SubTeamNumber,
	g.Desc_Product				AS ItemDescription,
	g.Desc_POS					AS PosDescription,
	g.RetailSize,
	g.RetailUOM,
	g.PackageUnit,
	g.MerchandiseSubBrickName	AS MerchandiseSubBrick,
	g.Desc_CustomerFriendly		AS CustomerFriendlyDescription,
	g.FairTrade,
	g.FlexibleText,
	g.MadeWithOrgGrapes			AS MadeWithOrganicGrapes,
	g.WIC,
	g.PrimeBeef,
	g.RainforestAlliance,
	g.Refrigerated,
	g.SmithsonianBirdFriendly,
	g.NutritionRequired,
	g.GlobalPricingProgram,
	g.PercentageTareWeight,
	g.Agency_GlutenFree			AS GlutenFreeAgency,
	g.Agency_Kosher				AS KosherAgency,
	g.Agency_NonGMO				AS NonGmoAgency,
	g.Agency_Organic			AS OrganicAgency,
	g.Agency_Vegan				AS VeganAgency,
	g.CheeseMilkType,
	g.IsAirChilled,
	g.IsBiodynamic,
	g.IsCheeseRaw,
	g.IsDryAged,
	g.IsFreeRange,
	g.IsGrassFed,
	g.IsMadeInHouse,
	g.IsMsc,
	g.IsPastureRaised,
	g.IsPremiumBodyCare,
	g.IsVegetarian,
	g.IsWholeTrade,
	g.Rating_AnimalWelfare		AS AnimalWelfareRating,
	g.Rating_EcoScale			AS EcoScaleRating,
	g.Rating_HealthyEating		AS HealthyEatingRating,
	g.Seafood_CatchType			AS SeafoodCatchType,
	g.Seafood_FreshOrFrozen		AS FreshOrFrozenSeafood,
    g.RecipeName,
    g.Allergens,
    g.Ingredients,
    g.ServingsPerPortion,
    g.ServingSizeDesc,
    g.ServingPerContainer,
    g.HshRating,
    g.ServingUnits,
    g.SizeWeight,
    g.Calories,
    g.CaloriesFat,
    g.CaloriesSaturatedFat,
    g.TotalFatWeight,
    g.TotalFatPercentage,
    g.SaturatedFatWeight,
    g.SaturatedFatPercent,
    g.PolyunsaturatedFat,
    g.MonounsaturatedFat,
    g.CholesterolWeight,
    g.CholesterolPercent,
    g.SodiumWeight,
    g.SodiumPercent,
    g.PotassiumWeight,
    g.PotassiumPercent,
    g.TotalCarbohydrateWeight,
    g.TotalCarbohydratePercent,
    g.DietaryFiberWeight,
    g.DietaryFiberPercent,
    g.SolubleFiber,
    g.InsolubleFiber,
    g.Sugar,
    g.SugarAlcohol,
    g.OtherCarbohydrates,
    g.ProteinWeight,
    g.ProteinPercent,
    g.VitaminA,
    g.Betacarotene,
    g.VitaminC,
    g.Calcium,
    g.Iron,
    g.VitaminD,
    g.VitaminE,
    g.Thiamin,
    g.Riboflavin,
    g.Niacin,
    g.VitaminB6,
    g.Folate,
    g.VitaminB12,
    g.Biotin,
    g.PantothenicAcid,
    g.Phosphorous,
    g.Iodine,
    g.Magnesium,
    g.Zinc,
    g.Copper,
    g.Transfat,
    g.CaloriesFromTransFat,
    g.Om6Fatty,
    g.Om3Fatty,
    g.Starch,
    g.Chloride,
    g.Chromium,
    g.VitaminK,
    g.Manganese,
    g.Molybdenum,
    g.Selenium,
    g.TransFatWeight,
	il.SupplierItemID			AS VendorItemID,
	il.SupplierName				AS VendorName,
	il.IrmaVendorKey			AS VendorKey,
	il.SupplierCaseSize			AS VendorCaseSize,
	il.Authorized,
	il.Discontinued,
	il.LabelTypeDesc,
	il.LocalItem,
	il.Locality,
	il.MSRP,
	il.Product_Code				AS ProductCode,
	il.RetailUnit,
	il.Sign_Desc				AS SignDescription,
	il.Sign_RomanceText_Long	AS SignRomanceTextLong,
	il.Sign_RomanceText_Short	AS SignRomanceTextShort,
	il.AltRetailSize,
	il.AltRetailUOM,
	il.OrderedByInfor,
	il.ColorAdded,
	il.ChicagoBaby				AS UomRegulationChicagoBaby,
	il.CountryOfProcessing,
	il.Exclusive,
	il.LinkedScanCode,
	il.LinkedScanCodeBrand,
	il.Origin,
	il.ScaleExtraText			AS ExtraText,
	il.TagUOM					AS UomRegulationTagUom,
	il.NumberOfDigitsSentToScale AS ScalePLUDigits,
    p.RegularPriceMultiple,
    p.RegularPrice,
    p.RegularStartDate,
    p.RegularPriceType,
    p.RegularPriceTypeAttribute	AS RegularPriceReason,
    p.RegularSellableUOM,
	p.NewTagExpiration,
    p.TprMultiple,
    p.TprPrice,
    p.TprPriceType,
    p.TprPriceTypeAttribute		AS TprPriceReason,
    p.TprSellableUOM,
    p.TprStartDate,
    p.TprEndDate,
	p.RewardPrice,
	p.RewardPriceType,
	p.RewardPriceTypeAttribute	AS RewardPriceReason,
	p.RewardPriceMultiple,
	p.RewardPriceSellableUOM,
	p.RewardPriceStartDate,
	p.RewardPriceEndDate,
	p.LinkedScanCodePrice
FROM #globalItem		g
INNER JOIN #itemLocale	il	on g.ItemID = il.ItemID
							AND g.BusinessUnitID = il.BusinessUnitID
INNER JOIN #prices		p	on g.ItemID = p.ItemID
							AND g.BusinessUnitID = p.BusinessUnitID
INNER JOIN StoreAddress	sa	on g.BusinessUnitID = sa.BusinessUnitID

DROP TABLE #regularPriceKeys
DROP TABLE #salePriceKeys
DROP TABLE #globalItem
DROP TABLE #itemLocale
DROP TABLE #itemExtended
DROP TABLE #prices
DROP TABLE #linkedScanCodePriceKeys
DROP TABLE #rewardPriceKeys

END
GO

GRANT EXEC ON [esl].[GetEslAttributes] TO dds_esl_role
GO