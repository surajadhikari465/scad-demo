CREATE PROCEDURE eplum.GetEPlumAttributes
	@BusinessUnitID INT,
	@StartDateTimeUtc DATETIME2(7),
	@EndDateTimeUtc DATETIME2(7)
AS
BEGIN

SET NOCOUNT ON;

-- Set Attribute IDs
DECLARE @nutritionRequiredId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'NR');
DECLARE @refrigerateId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'RFD');
DECLARE @linkedScanCodeId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'LSC');
DECLARE @forceTareId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'FTA');
DECLARE @shelfLifeId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'SHL');
DECLARE @unwrappedTareWeightId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'UTA');
DECLARE @wrappedTareWeightId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'WTA');
DECLARE @useByEabId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'EAB');
DECLARE @cfsSendToScaleId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'CFS');
DECLARE @scaleExtraTextId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'SET');
DECLARE @numberOfDigitsSentToScale INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'NDS');
DECLARE @colorAddedId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'CLA');
DECLARE @countryOfProcessingId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'COP');
DECLARE @originId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'ORN');

-- Other local variables
DECLARE @Region NVARCHAR(2) = (SELECT Region FROM Locale WHERE BusinessUnitID = @BusinessUnitID);
DECLARE @BeginningOfToday DATETIME2 = CAST(CAST(GETDATE() AS DATE) AS datetime2);
DECLARE @EndOfToday DATETIME2 = CAST(DATEADD(ms, -3, DATEADD(dd, 1, DATEDIFF(dd, 0, GETDATE()))) AS datetime2);

-- Put ItemIDs we need into a temp table
-- Optional Attributes to avoid a left join on main global item query
CREATE TABLE #itemExtended
(
	Region NCHAR(2) NOT NULL,
	ItemID INT NOT NULL,
	BusinessUnitID INT NOT NULL,
	NutritionRequired NVARCHAR(255) NULL,
	Refrigerated NVARCHAR(255) NULL,
	LinkedScanCode NVARCHAR(13) NULL,
	LinkedScanCodeBrand NVARCHAR(255) NULL,
	ForceTare NVARCHAR(255) NULL,
	ShelfLife NVARCHAR(255) NULL,
	UnwrappedTareWeight NVARCHAR(255) NULL,
	WrappedTareWeight NVARCHAR(255) NULL,
	UseByEab NVARCHAR(255) NULL,
	CfsSendToScale NVARCHAR(255) NULL,
	ScaleExtraText NVARCHAR(MAX) NULL,
	ColorAdded NVARCHAR(MAX) NULL,
	CountryOfProcessing NVARCHAR(MAX) NULL,
	NumberOfDigitsSentToScale NVARCHAR(255) NULL,
	Origin NVARCHAR(MAX) NULL,
	Agency_NonGMO NVARCHAR(255) NULL,
	Agency_Organic NVARCHAR(255) NULL,
	Agency_Vegan NVARCHAR(255) NULL,
	IsCheeseRaw bit NULL,
	IsMsc bit NULL,
	IsVegetarian bit NULL,
	Rating_AnimalWelfare NVARCHAR(255) NULL,
	Rating_HealthyEating NVARCHAR(255) NULL,
	Seafood_FreshOrFrozen NVARCHAR(255) NULL,
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
DELETE FROM stage.ItemStoreKeysEPlum
	OUTPUT 
		@Region					as Region,
		deleted.ItemID			as ItemID,
		deleted.BusinessUnitID	as BusinessUnitID,
		deleted.InsertDateUtc	as OriginalStageInsertDate
	INTO #itemExtended (Region, ItemID, BusinessUnitID, OriginalStageInsertDate)
WHERE BusinessUnitID = @BusinessUnitID
	AND InsertDateUtc BETWEEN @StartDateTimeUtc AND @EndDateTimeUtc

CREATE NONCLUSTERED INDEX IX_#itemStores_Region_ItemID_BusinessUnitID ON #ItemExtended (Region ASC, ItemID ASC, BusinessUnitID ASC);

--Update Global Extended Attributes
UPDATE ist
SET NutritionRequired = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
	AND ia.AttributeID = @nutritionRequiredId

UPDATE ist
SET Refrigerated = AttributeValue
FROM #itemExtended ist
JOIN ItemAttributes_Ext ia	on ist.ItemID = ia.ItemID
	AND ia.AttributeID = @refrigerateId

--Update Sign Attributes
UPDATE ist
SET 
	Agency_NonGMO = sgn.Agency_NonGMO,
	Agency_Organic = sgn.Agency_Organic,
	Agency_Vegan = sgn.Agency_Vegan,
	IsCheeseRaw = sgn.IsCheeseRaw,
	IsMsc = sgn.IsMsc,
	IsVegetarian = sgn.IsVegetarian,
	Rating_AnimalWelfare = sgn.Rating_AnimalWelfare,
	Rating_HealthyEating = sgn.Rating_HealthyEating,
	Seafood_FreshOrFrozen = sgn.Seafood_FreshOrFrozen
FROM #itemExtended ist
JOIN dbo.ItemAttributes_Sign sgn on ist.ItemID = sgn.ItemID

--Update Nutrition Attributes
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
	Transfat = n.TransFat,
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

-- ItemLocale Extended attributes data
UPDATE ist
SET ForceTare = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
	AND ist.ItemID = ia.ItemID
	AND l.LocaleID = ia.LocaleID
	AND ia.AttributeID = @forceTareId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET ShelfLife = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
	AND ist.ItemID = ia.ItemID
	AND l.LocaleID = ia.LocaleID
	AND ia.AttributeID = @shelfLifeId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET UnwrappedTareWeight = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
	AND ist.ItemID = ia.ItemID
	AND l.LocaleID = ia.LocaleID
	AND ia.AttributeID = @unwrappedTareWeightId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET WrappedTareWeight = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
	AND ist.ItemID = ia.ItemID
	AND l.LocaleID = ia.LocaleID
	AND ia.AttributeID = @wrappedTareWeightId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET
	LinkedScanCode = ia.AttributeValue,
	LinkedScanCodeBrand = hc.HierarchyClassName
FROM #itemExtended ist
JOIN Locale l on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt ia	on ist.Region = ia.Region
	AND ist.ItemID = ia.ItemID
	AND l.LocaleID = ia.LocaleID
	AND ia.AttributeID = @linkedScanCodeId
JOIN Items i on ia.AttributeValue = i.ScanCode
JOIN HierarchyClass hc on i.BrandHCID = hc.HierarchyClassID
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET UseByEab = ia.AttributeValue
FROM #itemExtended		ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
	AND ist.ItemID = ia.ItemID
	AND l.LocaleID = ia.LocaleID
	AND ia.AttributeID = @useByEabId
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

UPDATE ist
SET CfsSendToScale = ia.AttributeValue
FROM #itemExtended				ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
	AND ist.ItemID = ia.ItemID
	AND l.LocaleID = ia.LocaleID
	AND ia.AttributeID = @cfsSendToScaleId
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
SET NumberOfDigitsSentToScale = ia.AttributeValue
FROM #itemExtended				ist
JOIN Locale						l	on ist.BusinessUnitID = l.BusinessUnitID
JOIN ItemLocaleAttributesExt	ia	on	ist.Region = ia.Region
	AND ist.ItemID = ia.ItemID
	AND l.LocaleID = ia.LocaleID
	AND ia.AttributeID = @numberOfDigitsSentToScale
WHERE ia.Region = @Region
AND l.Region = @Region
OPTION (RECOMPILE)

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
	AND p.StartDate <= @BeginningOfToday
	AND p.PriceType = 'REG'
GROUP BY
	p.Region,
	p.ItemID,
	p.BusinessUnitID,
	p.PriceType
OPTION (RECOMPILE)

CREATE NONCLUSTERED INDEX IX_#regularPriceKeys ON #regularPriceKeys (Region, ItemID, BusinessUnitID, StartDate, PriceType);

-- Linked Scan Code Price Keys
SELECT
	p.Region,
	st.ItemID,
	p.ItemID as LinkCodeItemID,
	p.BusinessUnitID,
	MAX(StartDate)  as StartDate,
	p.PriceType
INTO #linkedScanCodePriceKeys
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

CREATE NONCLUSTERED INDEX IX_#linkedScanCodePriceKeys ON #linkedScanCodePriceKeys (Region, LinkCodeItemID, BusinessUnitID, StartDate, PriceType);

-- Set prices for REGs and TPRs and RWDs separately
CREATE TABLE #prices
(
	Region NCHAR(2),
    ItemID INT,
    BusinessUnitID INT,
    RegularPriceMultiple TINYINT,
    RegularPrice DECIMAL(9, 2),
    RegularStartDate DATETIME2,
    RegularPriceType NVARCHAR(3),
    RegularPriceTypeAttribute NVARCHAR(10),
    RegularSellableUOM NVARCHAR(3),
    TprMultiple TINYINT NULL,
    TprPrice DECIMAL(9, 2) NULL,
    TprPriceType NVARCHAR(3) NULL,
    TprPriceTypeAttribute NVARCHAR(10) NULL,
    TprSellableUOM NVARCHAR(3) NULL,
    TprStartDate DATETIME2 NULL,
    TprEndDate DATETIME2 NULL,
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
	reg.StartDate			AS RegularStartDate,
	reg.PriceType			AS RegularPriceType,
	reg.PriceTypeAttribute	AS RegularPriceTypeAttribute,
	reg.SellableUOM			AS RegularSellableUOM,
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

-- Sale Prices (TPR and RWD)
UPDATE p
SET
	p.TprMultiple = CASE WHEN sal.PriceType = 'TPR' THEN sal.Multiple ELSE NULL END,     
	p.TprPrice = CASE WHEN sal.PriceType = 'TPR' THEN sal.Price ELSE NULL END,    
	p.TprPriceType = CASE WHEN sal.PriceType = 'TPR' THEN sal.PriceType ELSE NULL END,
	p.TprPriceTypeAttribute = CASE WHEN sal.PriceType = 'TPR' THEN sal.PriceTypeAttribute ELSE NULL END,
	p.TprSellableUOM = CASE WHEN sal.PriceType = 'TPR' THEN sal.SellableUOM ELSE NULL END,
	p.TprStartDate = CASE WHEN sal.PriceType = 'TPR' THEN sal.StartDate ELSE NULL END,
	p.TprEndDate = CASE WHEN sal.PriceType = 'TPR' THEN sal.EndDate ELSE NULL END
FROM #prices p
INNER JOIN gpm.Prices sal on p.ItemID = sal.ItemID
	AND p.BusinessUnitID = sal.BusinessUnitID
WHERE sal.Region = @Region
	AND sal.StartDate <= @BeginningOfToday
	AND sal.EndDate >= @EndOfToday
	AND (sal.PriceType = 'TPR' OR sal.PriceType = 'RWD')
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

CREATE NONCLUSTERED INDEX IX_#prices_ItemID_BusinessUnitID ON #prices (Region ASC, ItemID ASC, BusinessUnitID ASC)
	INCLUDE
	(
		RegularPriceMultiple,
		RegularPrice,
		RegularStartDate,
		RegularPriceType,
		RegularPriceTypeAttribute,
		RegularSellableUOM,
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
	ist.Region,
	i.ItemID,
	ist.BusinessUnitID,
	i.ScanCode,
	i.PSNumber					AS SubTeamNumber,
	i.RetailSize,
	i.RetailUOM,
	i.Desc_CustomerFriendly		AS CustomerFriendlyDescription,
	sb.HierarchyClassName		AS MerchandiseSubBrick,
	ist.NutritionRequired,
	ist.Refrigerated,
	ist.Agency_NonGMO			AS NonGmoAgency,
	ist.Agency_Organic			AS OrganicAgency,
	ist.Agency_Vegan			AS VeganAgency,
	ist.IsCheeseRaw,
	ist.IsMsc,
	ist.IsVegetarian,
	ist.Rating_AnimalWelfare	AS AnimalWelfareRating,
	ist.Rating_HealthyEating	AS HealthyEatingRating,
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
	ist.LinkedScanCode,
	ist.LinkedScanCodeBrand,
	ist.UseByEab					AS UseBy,
	ist.ForceTare,
	NULL AS PercentageTareWeight, -- We are returning NULL for PercentageTareWeight because Informatica which consumes this SPROC needs a column for it. Even though there is no value we can provide.
	ist.ShelfLife,
	ist.UnwrappedTareWeight,
	ist.CfsSendToScale,
	ist.WrappedTareWeight,
	ist.ScaleExtraText				AS ExtraText,
	il.DefaultScanCode,
	ist.ColorAdded,
	ist.CountryOfProcessing,
	ist.NumberOfDigitsSentToScale	AS ScalePLUDigits,
	ist.Origin,
	il.ScaleItem,
	il.RetailUnit,
    p.RegularPriceMultiple,
    p.RegularPrice,
    p.RegularPriceType,
    p.RegularPriceTypeAttribute		AS RegularPriceReason,
	p.RegularStartDate,
    p.RegularSellableUOM,
    p.TprMultiple,
    p.TprPrice,
    p.TprPriceType,
	p.TprPriceTypeAttribute			AS TprPriceReason,
    p.TprSellableUOM,
	p.TprStartDate,
	p.TprEndDate,
	p.RewardPrice,
	p.RewardPriceType,
	p.RewardPriceTypeAttribute		AS RewardPriceReason,
	p.RewardPriceMultiple,
	p.RewardPriceSellableUOM,
	p.RewardPriceStartDate,
	p.RewardPriceEndDate,
	p.LinkedScanCodePrice,
	sa.TerritoryAbbrev				AS GeographicalState,
	il.Authorized					AS IsAuthorized 
FROM #itemExtended ist
INNER JOIN dbo.Items i on ist.ItemID = i.ItemID
INNER JOIN dbo.Hierarchy_Merchandise m on i.HierarchyMerchandiseID = m.HierarchyMerchandiseID
INNER JOIN dbo.HierarchyClass sb on m.SubBrickHCID = sb.HierarchyClassID
INNER JOIN dbo.ItemLocaleAttributes	il on ist.Region = il.Region
	AND ist.ItemID = il.ItemID
	AND ist.BusinessUnitID = il.BusinessUnitID
INNER JOIN #prices p on ist.Region = p.Region
	AND ist.ItemID = p.ItemID
	AND ist.BusinessUnitID = p.BusinessUnitID
INNER JOIN StoreAddress sa on il.BusinessUnitID = sa.BusinessUnitID
WHERE (il.ScaleItem = 1 OR ist.CfsSendToScale = 1)
	AND il.Region = @Region
OPTION (RECOMPILE)

END TRY
BEGIN CATCH

	--re-insert data back to the staging table
	INSERT INTO stage.ItemStoreKeysEPlum
		(BusinessUnitID, ItemID, InsertDateUtc)
	SELECT
		BusinessUnitID, ItemID, OriginalStageInsertDate
	FROM #itemExtended

	THROW
END CATCH

DROP TABLE #regularPriceKeys
DROP TABLE #itemExtended
DROP TABLE #prices
DROP TABLE #linkedScanCodePriceKeys

END
GO

GRANT EXEC ON eplum.GetEPlumAttributes TO dds_eplum_role
GO