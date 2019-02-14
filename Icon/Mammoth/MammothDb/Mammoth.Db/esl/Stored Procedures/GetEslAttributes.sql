CREATE PROCEDURE esl.GetEslAttributes
	@BusinessUnitID INT,
	@StartDateTimeUtc DATETIME2(7),
	@EndDateTimeUtc DATETIME2(7)
AS
BEGIN

SET NOCOUNT ON;

-- Find whether or not store is on GPM
DECLARE @StoreGpmStatus BIT;
SELECT
	@StoreGpmStatus = IsGpmEnabled
FROM RegionGpmStatus g
INNER JOIN dbo.Locale l on l.Region = g.Region
WHERE l.BusinessUnitID = @BusinessUnitID

-- Set Attribute IDs
DECLARE @flexTextId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'FXT');
DECLARE @fairTradeId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'FTC');
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

-- Other local variables
DECLARE @Region NVARCHAR(2) = (SELECT Region FROM Locale WHERE BusinessUnitID = @BusinessUnitID);
DECLARE @Today datetime = CAST(CAST(GETDATE() as date) as datetime);
DECLARE @BeginningOfToday DATETIME2 = CAST(CAST(GETDATE() AS DATE) AS datetime2);

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
	TransFatWeight decimal(10, 1) NULL,
	OriginalStageInsertDate datetime2(7) NULL
)

BEGIN TRY

-- Pull ItemStore Keys from the staging table while also deleting at the same time
DELETE FROM stage.ItemStoreKeysEsl
	OUTPUT 
		@Region					as Region,
		deleted.ItemID			as ItemID,
		deleted.BusinessUnitID	as BusinessUnitID,
		deleted.InsertDateUtc	as OriginalStageInsertDate
	INTO #itemExtended (Region, ItemID, BusinessUnitID, OriginalStageInsertDate)
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

-- ==============================================
-- Create Price Table to hold price data
-- ==============================================
CREATE TABLE #prices
(
	Region NCHAR(2),
    ItemID INT,
    BusinessUnitID INT,
    RegularPriceMultiple TINYINT,
    RegularPrice DECIMAL(9, 2),
    RegularStartDate DATETIME2(7),
    RegularPriceType NVARCHAR(3),
    RegularPriceTypeAttribute NVARCHAR(10),
    RegularSellableUOM NVARCHAR(3),
	TagExpirationDate DATETIME2(7),
    TprMultiple TINYINT,
    TprPrice DECIMAL(9, 2),
    TprPriceType NVARCHAR(3),
    TprPriceTypeAttribute NVARCHAR(10),
    TprSellableUOM NVARCHAR(3),
    TprStartDate DATETIME2(7),
    TprEndDate DATETIME2(7),
	RewardPrice DECIMAL(9, 2) NULL,
	RewardPriceType NVARCHAR(3) NULL,
	RewardPriceTypeAttribute NVARCHAR(10) NULL,
	RewardPriceMultiple TINYINT NULL,
	RewardPriceSellableUOM NVARCHAR(3) NULL,
	RewardPriceStartDate DATETIME2(7),
	RewardPriceEndDate DATETIME2(7),
	LinkedScanCodePrice DECIMAL(9, 2) NULL,
);

CREATE TABLE #regularPriceKeys
(
	Region NCHAR(2),
	ItemID INT,
	BusinessUnitID INT,
	StartDate DATETIME2(7),
	PriceType NVARCHAR(3)
);

CREATE TABLE #salePriceKeys
(
	Region NCHAR(2),
	ItemID INT,
	BusinessUnitID INT,
	StartDate DATETIME2(7),
	PriceType NVARCHAR(3)
);

CREATE TABLE #rewardPriceKeys
(
	Region NCHAR(2),
	ItemID INT,
	BusinessUnitID INT,
	StartDate DATETIME2(7),
	PriceType NVARCHAR(3)
);

CREATE TABLE #linkedScanCodePriceKeys
(
	Region NCHAR(2),
	ItemID INT,
	LinkCodeItemID INT,
	BusinessUnitID INT,
	StartDate DATETIME2(7),
	PriceType NVARCHAR(3)
);

-- ==============================================
-- Query gpm.Price tables if store is on GPM
-- Query dbo.Price tables if store is not on GPM
-- ==============================================
IF @StoreGpmStatus = 1
BEGIN
	-- Gather Price Data
	-- Regular Prices
	INSERT INTO #regularPriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType)
	SELECT
		p.Region,
		p.ItemID,
		p.BusinessUnitID,
		MAX(StartDate)  as StartDate,
		p.PriceType
	FROM #itemExtended  st
	JOIN gpm.Prices p on st.Region = p.Region
		AND st.ItemID = p.ItemID
		AND st.BusinessUnitID = p.BusinessUnitID	 
	WHERE 
		p.Region = @Region
		AND p.StartDate <= @BeginningOfToday
		AND p.PriceType = 'REG'
	GROUP BY
		p.Region,
		p.ItemID,
		p.BusinessUnitID,
		p.PriceType
	OPTION (RECOMPILE)

	-- Linked Scan Code Price Keys
	INSERT INTO #linkedScanCodePriceKeys (Region, ItemID, LinkCodeItemID, BusinessUnitID, StartDate, PriceType)
	SELECT
		p.Region,
		st.ItemID,
		p.ItemID as LinkedCodeItemID,
		p.BusinessUnitID,
		MAX(StartDate)  as StartDate,
		p.PriceType
	FROM #itemExtended st
	JOIN dbo.Items i on st.LinkedScanCode = i.ScanCode
	JOIN gpm.Prices p on i.ItemID = p.ItemID
		AND st.BusinessUnitID = p.BusinessUnitID
	WHERE
		p.Region = @Region
		AND p.StartDate <= @BeginningOfToday
		AND p.PriceType = 'REG'
	GROUP BY
		p.Region,
		st.ItemID,
		p.ItemID,
		p.BusinessUnitID,
		p.PriceType
	OPTION (RECOMPILE)

	-- Set prices for REGs, TPRs, RWDs, and LinkedScanCodePrice separately
	-- REGs
	INSERT INTO #prices
	(
		Region,
		ItemID,
		BusinessUnitID,
		RegularPriceMultiple,
		RegularPrice,
		RegularStartDate,
		RegularPriceType,
		RegularPriceTypeAttribute,
		RegularSellableUOM,
		TagExpirationDate,
		TprMultiple,
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
	)
	SELECT
		reg.Region,
		reg.ItemID,
		reg.BusinessUnitID,
		reg.Multiple			AS RegularPriceMultiple,
		reg.Price				AS RegularPrice,
		reg.StartDate           AS RegularStartDate,
		reg.PriceType           AS RegularPriceType,
		reg.PriceTypeAttribute  AS RegularPriceTypeAttribute,
		reg.SellableUOM         AS RegularSellableUOM,
		reg.TagExpirationDate	AS TagExpirationDate,
		null                    AS TprMultiple,
		null                    AS TprPrice,
		null                    AS TprPriceType,
		null                    AS TprPriceTypeAttribute,
		null                    AS TprSellableUOM,
		null                    AS TprStartDate,
		null                    AS TprEndDate,
		null					AS RewardPrice,
		null					AS RewardPriceType,
		null					AS RewardPriceTypeAttribute,
		null					AS RewardPriceMultiple,
		null					AS RewardPriceSellableUOM,
		null					AS RewardPriceStartDate,
		null					AS RewardPriceEndDate,
		null					AS LinkedScanCodePrice
	FROM #regularPriceKeys pr
	JOIN gpm.Prices reg ON pr.Region = reg.Region
		AND pr.ItemID = reg.ItemID
		AND pr.BusinessUnitID = reg.BusinessUnitID
		AND pr.StartDate = reg.StartDate
		AND pr.PriceType = reg.PriceType
	WHERE reg.Region = @Region
	OPTION (RECOMPILE)

	-- TPRs
	UPDATE p
	SET
		p.TprMultiple = sal.Multiple,     
		p.TprPrice = sal.Price,    
		p.TprPriceType = sal.PriceType,
		p.TprPriceTypeAttribute = sal.PriceTypeAttribute,
		p.TprSellableUOM = sal.SellableUOM,
		p.TprStartDate = sal.StartDate,
		p.TprEndDate = sal.EndDate
	FROM #prices p
	INNER JOIN gpm.Prices sal on p.ItemID = sal.ItemID
		AND p.BusinessUnitID = sal.BusinessUnitID
	WHERE sal.Region = @Region
		AND sal.StartDate <= @BeginningOfToday
		AND sal.EndDate > @BeginningOfToday
		AND sal.PriceType = 'TPR' 
	OPTION (RECOMPILE)

		-- RWDs)
	UPDATE p
	SET
		p.RewardPrice = rwd.Price,
		p.RewardPriceType = rwd.PriceType,
		p.RewardPriceTypeAttribute = rwd.PriceTypeAttribute,
		p.RewardPriceMultiple = rwd.Multiple,
		p.RewardPriceSellableUOM =  rwd.SellableUOM,
		p.RewardPriceStartDate = rwd.StartDate,
		p.RewardPriceEndDate = rwd.EndDate
	FROM #prices p
	INNER JOIN gpm.Prices rwd on p.ItemID = rwd.ItemID
		ANd p.BusinessUnitID = rwd.BusinessUnitID
	WHERE rwd.Region = @Region
		AND rwd.StartDate <= @BeginningOfToday
		AND rwd.EndDate > @BeginningOfToday
		AND rwd.PriceType = 'RWD'
	OPTION (RECOMPILE)

	-- Linked ScanCode Price
	UPDATE p
	SET LinkedScanCodePrice = lsp.Price
	FROM #prices p
	INNER JOIN #linkedScanCodePriceKeys k on p.Region = k.Region
		AND p.ItemID = k.ItemID
		AND p.BusinessUnitID = k.BusinessUnitID
	INNER JOIN gpm.Prices lsp  ON k.Region = lsp.Region
		AND k.LinkCodeItemID = lsp.ItemID
		AND k.BusinessUnitID = lsp.BusinessUnitID
		AND k.StartDate = lsp.StartDate
		AND k.PriceType = lsp.PriceType
	WHERE lsp.Region = @Region
	OPTION (RECOMPILE)

END -- IF Block
ELSE
BEGIN
	-- Query dbo.Price tables
	-- Regular Prices
	INSERT INTO #regularPriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType)
	SELECT
		p.Region,
		p.ItemID,
		p.BusinessUnitID,
		MAX(StartDate)  as StartDate,
		p.PriceType
	FROM #itemExtended st
	JOIN dbo.Price p on st.Region = p.Region
		AND st.ItemID = p.ItemID
		AND st.BusinessUnitID = p.BusinessUnitID	 
	WHERE 
		p.Region = @Region
		AND p.StartDate <= @BeginningOfToday
		AND p.PriceType = 'REG'
	GROUP BY
		p.Region,
		p.ItemID,
		p.BusinessUnitID,
		p.PriceType
	OPTION (RECOMPILE)

	-- Sale Price Keys
	INSERT INTO #salePriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType)
	SELECT
		p.Region,
		p.ItemID,
		p.BusinessUnitID,
		MAX(StartDate)  as StartDate,
		p.PriceType
	FROM #itemExtended st
	JOIN dbo.Price p on st.Region = p.Region
		AND st.ItemID = p.ItemID
		AND st.BusinessUnitID = p.BusinessUnitID	
	WHERE
		p.Region = @Region
		AND p.StartDate <= @BeginningOfToday
		AND p.EndDate > @BeginningOfToday
		AND p.PriceType <> 'REG'
	GROUP BY
		p.Region,
		p.ItemID,
		p.BusinessUnitID,
		p.PriceType
	OPTION (RECOMPILE)

	-- Linked Scan Code Price Keys
	INSERT INTO #linkedScanCodePriceKeys (Region, ItemID, LinkCodeItemID, BusinessUnitID, StartDate, PriceType)
	SELECT
		p.Region,
		st.ItemID,
		p.ItemID as LinkCodeItemID,
		p.BusinessUnitID,
		MAX(StartDate)  as StartDate,
		p.PriceType
	FROM #itemExtended st
	JOIN dbo.Items i on st.LinkedScanCode = i.ScanCode
	JOIN dbo.Price p on i.ItemID = p.ItemID
		AND st.BusinessUnitID = p.BusinessUnitID
	WHERE
		p.Region = @Region
		AND p.StartDate <= @BeginningOfToday
		AND p.PriceType = 'REG'
	GROUP BY
		p.Region,
		st.ItemID,
		p.ItemID,
		p.BusinessUnitID,
		p.PriceType
	OPTION (RECOMPILE)

	INSERT INTO #prices
	(
		Region,
		ItemID,
		BusinessUnitID,
		RegularPriceMultiple,
		RegularPrice,
		RegularStartDate,
		RegularPriceType,
		RegularPriceTypeAttribute,
		RegularSellableUOM,
		TagExpirationDate,
		TprMultiple,
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
	)
	SELECT
		reg.Region,
		reg.ItemID,
		reg.BusinessUnitID,
		reg.Multiple	AS RegularPriceMultiple,
		reg.Price		AS RegularPrice,
		reg.StartDate	AS RegularStartDate,
		'REG'		    AS RegularPriceType,
		reg.PriceType	AS RegularPriceTypeAttribute,
		reg.PriceUOM    AS RegularSellableUOM,
		NULL			AS TagExpirationDate,
		NULL            AS TprMultiple,
		NULL            AS TprPrice,
		NULL            AS TprPriceType,
		NULL            AS TprPriceTypeAttribute,
		NULL            AS TprSellableUOM,
		NULL            AS TprStartDate,
		NULL            AS TprEndDate,
		NULL			AS RewardPrice,
		NULL			AS RewardPriceType,
		NULL			AS RewardPriceTypeAttribute,
		NULL			AS RewardPriceMultiple,
		NULL			AS RewardPriceSellableUOM,
		NULL			AS RewardPriceStartDate,
		NULL			AS RewardPriceEndDate,
		NULL			AS LinkedScanCodePrice
	FROM #regularPriceKeys    pr
	JOIN dbo.Price reg  ON pr.Region = reg.Region
		AND pr.ItemID = reg.ItemID
		AND pr.BusinessUnitID = reg.BusinessUnitID
		AND pr.StartDate = reg.StartDate
		AND pr.PriceType = reg.PriceType
	WHERE reg.Region = @Region
	OPTION (RECOMPILE)

	-- TPRs
	-- Since IRMA Price tables can have more than one non-REG price with the same StartDate
	-- we need to get the most recently added one, thus the subquery
	UPDATE p
	SET
		TprMultiple = sale.Multiple,     
		TprPrice = sale.Price,
		TprPriceType = 'TPR',
		TprPriceTypeAttribute = sale.PriceType,
		TprSellableUOM = sale.PriceUOM,
		TprStartDate = sale.StartDate,
		TprEndDate = sale.EndDate
	FROM #prices p
	INNER JOIN #salePriceKeys k ON p.ItemID = k.ItemID
		AND p.BusinessUnitID = k.BusinessUnitID
	INNER JOIN (
		SELECT
			a.PriceID,
			a.Region,
			a.ItemID,
			a.BusinessUnitID,
			a.StartDate,
			a.PriceType,
			a.EndDate,
			a.PriceUOM,
			a.Multiple,
			a.Price,
			ROW_NUMBER() OVER (PARTITION BY  a.Region, a.ItemID, a.BusinessUnitID, a.StartDate ORDER BY a.PriceID DESC) AS recent_price
		FROM #salePriceKeys spk
		INNER JOIN dbo.Price a on spk.Region = a.Region
			AND spk.ItemID = a.ItemID
			AND spk.BusinessUnitID = a.BusinessUnitID
			AND spk.StartDate = a.StartDate
		WHERE a.Region = @Region 
			AND a.PriceType <> 'REG'
	) sale on k.Region = sale.Region
		AND k.ItemID = sale.ItemID
		AND k.BusinessUnitID = sale.BusinessUnitID
		AND k.StartDate = sale.StartDate
		AND k.PriceType = sale.PriceType
	WHERE recent_price = 1
	OPTION (RECOMPILE)

	-- Linked ScanCode Price
	UPDATE p
	SET LinkedScanCodePrice = lsp.Price
	FROM #prices p
	INNER JOIN #linkedScanCodePriceKeys k on p.Region = k.Region
		AND p.ItemID = k.ItemID
		AND p.BusinessUnitID = k.BusinessUnitID
	INNER JOIN dbo.Price lsp  ON k.Region = lsp.Region
		AND k.LinkCodeItemID = lsp.ItemID
		AND k.BusinessUnitID = lsp.BusinessUnitID
		AND k.StartDate = lsp.StartDate
		AND k.PriceType = lsp.PriceType
	WHERE lsp.Region = @Region
	OPTION (RECOMPILE)

END -- Else block

CREATE NONCLUSTERED INDEX IX_#regularPriceKeys ON #regularPriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType);
CREATE NONCLUSTERED INDEX IX_#salePriceKeys ON #salePriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType);
CREATE NONCLUSTERED INDEX IX_#rewardPriceKeys ON #rewardPriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType);
CREATE NONCLUSTERED INDEX IX_#linkedScanCodePriceKeys ON #linkedScanCodePriceKeys (Region, LinkCodeItemID, BusinessUnitID, StartDate, PriceType);

CREATE NONCLUSTERED INDEX IX_#prices_ItemID_BusinessUnitID ON #prices (ItemID ASC, BusinessUnitID ASC)
	INCLUDE
	(
		RegularPriceMultiple,
		RegularPrice,
		RegularStartDate,
		RegularPriceType,
		RegularPriceTypeAttribute,
		RegularSellableUOM,
		TagExpirationDate,
		TprMultiple,
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
	ist.ItemID,
	il.BusinessUnitID,
	l.StoreName,
	sa.TerritoryAbbrev			AS GeographicalState,
	i.ScanCode,
	t.itemTypeDesc				AS ItemType,
	b.HierarchyClassName		AS BrandName,
	st.Name						AS SubTeamName,
	i.PSNumber					AS SubTeamNumber,
	i.Desc_Product				AS ItemDescription,
	i.Desc_POS					AS PosDescription,
	i.RetailSize,
	i.RetailUOM,
	i.PackageUnit,
	sb.HierarchyClassName		AS MerchandiseSubBrick,
	i.Desc_CustomerFriendly		AS CustomerFriendlyDescription,
	ist.FairTrade,
	ist.FlexibleText,
	ist.MadeWithOrgGrapes		AS MadeWithOrganicGrapes,
	ist.WIC,
	ist.PrimeBeef,
	ist.RainforestAlliance,
	ist.Refrigerated,
	ist.SmithsonianBirdFriendly,
	ist.NutritionRequired,
	ist.GlobalPricingProgram,
	NULL AS PercentageTareWeight, -- We are returning NULL for PercentageTareWeight because Informatica which consumes this SPROC needs a column for it. Even though there is no value we can provide.
	ist.Agency_GlutenFree		AS GlutenFreeAgency,
	ist.Agency_Kosher			AS KosherAgency,
	ist.Agency_NonGMO			AS NonGmoAgency,
	ist.Agency_Organic			AS OrganicAgency,
	ist.Agency_Vegan			AS VeganAgency,
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
	ist.Rating_AnimalWelfare	AS AnimalWelfareRating,
	ist.Rating_EcoScale			AS EcoScaleRating,
	ist.Rating_HealthyEating	AS HealthyEatingRating,
	ist.Seafood_CatchType		AS SeafoodCatchType,
	ist.Seafood_FreshOrFrozen	AS FreshOrFrozenSeafood,
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
    ist.TransFatWeight,
	v.SupplierItemID			AS VendorItemID,
	v.SupplierName				AS VendorName,
	v.IrmaVendorKey				AS VendorKey,
	v.SupplierCaseSize			AS VendorCaseSize,
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
	ist.ColorAdded,
	ist.ChicagoBaby				AS UomRegulationChicagoBaby,
	ist.CountryOfProcessing,
	ist.Exclusive,
	ist.LinkedScanCode,
	ist.LinkedScanCodeBrand,
	ist.Origin,
	ist.ScaleExtraText			AS ExtraText,
	ist.TagUOM					AS UomRegulationTagUom,
	ist.NumberOfDigitsSentToScale AS ScalePLUDigits,
    p.RegularPriceMultiple,
    p.RegularPrice,
    p.RegularStartDate,
    p.RegularPriceType,
    p.RegularPriceTypeAttribute	AS RegularPriceReason,
    p.RegularSellableUOM,
	p.TagExpirationDate,
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
	p.LinkedScanCodePrice,
	il.ScaleItem
FROM #itemExtended ist
INNER JOIN dbo.ItemLocaleAttributes	il on	ist.Region = il.Region
	AND ist.ItemID = il.ItemID
	AND ist.BusinessUnitID = il.BusinessUnitID
	AND il.Authorized = 1
INNER JOIN dbo.Items i	on il.ItemID = i.ItemID
INNER JOIN dbo.ItemTypes t	on i.ItemTypeID = t.itemTypeID
INNER JOIN dbo.Financial_SubTeam st	on i.PSNumber = st.PSNumber
INNER JOIN dbo.HierarchyClass b	on i.BrandHCID = b.HierarchyClassID
INNER JOIN dbo.Hierarchy_Merchandise m	on i.HierarchyMerchandiseID = m.HierarchyMerchandiseID
INNER JOIN dbo.HierarchyClass sb	on m.SubBrickHCID = sb.HierarchyClassID
INNER JOIN StoreAddress sa	on ist.BusinessUnitID = sa.BusinessUnitID
INNER JOIN dbo.Locale l	on	ist.BusinessUnitID = l.BusinessUnitID
INNER JOIN dbo.ItemLocaleSupplier v	on	ist.Region = v.Region
	AND ist.ItemID = v.ItemID
	AND ist.BusinessUnitID = v.BusinessUnitID
INNER JOIN #prices p	on ist.ItemID = p.ItemID
	AND ist.BusinessUnitID = p.BusinessUnitID
WHERE l.Region = @Region
	AND il.Region = @Region
	AND v.Region = @Region
OPTION (RECOMPILE)

END TRY
BEGIN CATCH

	DECLARE @now datetime2(7) = SYSDATETIME();

	--re-insert data back to the staging table
	INSERT INTO stage.ItemStoreKeysEsl
		(BusinessUnitID, ItemID, InsertDateUtc)
	SELECT
		BusinessUnitID, ItemID, @now
	FROM #itemExtended;

	THROW;
END CATCH

IF OBJECT_ID('tempdb..#regularPriceKeys') IS NOT NULL
	DROP TABLE #regularPriceKeys
IF OBJECT_ID('tempdb..#salePriceKeys') IS NOT NULL
	DROP TABLE #salePriceKeys
IF OBJECT_ID('tempdb..#itemExtended') IS NOT NULL
	DROP TABLE #itemExtended
IF OBJECT_ID('tempdb..#prices') IS NOT NULL
	DROP TABLE #prices
IF OBJECT_ID('tempdb..#linkedScanCodePriceKeys') IS NOT NULL
	DROP TABLE #linkedScanCodePriceKeys
IF OBJECT_ID('tempdb..#rewardPriceKeys') IS NOT NULL
	DROP TABLE #rewardPriceKeys

END
GO

GRANT EXEC ON [esl].[GetEslAttributes] TO dds_esl_role
GO