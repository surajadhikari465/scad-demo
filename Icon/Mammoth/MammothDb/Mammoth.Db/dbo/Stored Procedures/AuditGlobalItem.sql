--Formatted by Poor SQL
CREATE PROCEDURE dbo.AuditGlobalItem @action VARCHAR(25)
	,@groupSize INT = 250000
	,@maxRows INT = 0
	,@groupId INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	IF IsNull(@maxRows, 0) <= 0
		SET @maxRows = 2147483647 --max int

  IF IsNull(@groupSize, 0) <= 0 
    SET @groupSize = 250000;

	IF @action = 'Initilize'
	BEGIN
		SELECT Count(*) [RowCount]
		FROM dbo.Items

		RETURN;
	END

	IF @action = 'Get'
	BEGIN
		DECLARE @minId INT = (@groupId * @groupSize) + (
				CASE 
					WHEN @groupID = 0
						THEN 0
					ELSE 1
					END
				)
			,@fairTradeID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Fair Trade Certifed'
				)
			,@flexibleTextID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Flexible Text'
				)
			,@globalPricingID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Global Pricing Program'
				)
			,@organicGrapesID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Made With Organic Grapes'
				)
			,@scaleDigitsID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Number of Digits Sent To Scale'
				)
			,@nutritionRequiredID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Nutrition Required'
				)
			,@primeBeefID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Prime Beef'
				)
			,@rainforestID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Rainforest Alliance'
				)
			,@refrigeratedShelfID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Refrigerated or Shelf Stable'
				)
			,@smithsonianID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'Smithsonian Bird Friendly'
				)
			,@wicID INT = (
				SELECT AttributeID
				FROM Attributes
				WHERE AttributeDesc = 'WIC'
				);

		IF (object_id('tempdb..#group') IS NOT NULL)
			DROP TABLE #group;

		IF (object_id('tempdb..#items') IS NOT NULL)
			DROP TABLE #items;

		CREATE TABLE #group (ItemID INT INDEX ix_ID NONCLUSTERED);;

		WITH cte
		AS (
			SELECT TOP (@maxRows) ItemID
				,Row_Number() OVER (
					ORDER BY ItemID
					) rowID
			FROM dbo.Items
			)
		INSERT INTO #group (ItemID)
		SELECT TOP (@groupSize) ItemID
		FROM cte
		WHERE rowID >= @minId

		CREATE TABLE #items (
			ItemID INT NOT NULL INDEX ix_ItemID NONCLUSTERED
			,ItemType VARCHAR(255) NULL
			,ScanCode VARCHAR(13) NULL
			,MerchandiseHierarchySubBrick VARCHAR(255) NULL
			,NationalHierarchyClass VARCHAR(255) NULL
			,BrandName VARCHAR(255) NULL
			,TaxClass VARCHAR(255) NULL
			,SubTeamName VARCHAR(255) NULL
			,ProductDesc VARCHAR(255) NULL
			,POSDesc VARCHAR(255) NULL
			,PackageUnit VARCHAR(255) NULL
			,RetailSize VARCHAR(255) NULL
			,RetailUOM VARCHAR(255) NULL
			,FoodStampEligible BIT NULL
			,CustomerFriendlyDesc VARCHAR(255) NULL
			,FairTradeCertifed VARCHAR(300) NULL
			,FlexibleText VARCHAR(300) NULL
			,GlobalPricingProgram VARCHAR(300) NULL
			,MadeWithOrganicGrapes VARCHAR(300) NULL
			,NumberOfDigitsSentToScale VARCHAR(300) NULL
			,NutritionRequired VARCHAR(300) NULL
			,PrimeBeef VARCHAR(300) NULL
			,RainforestAlliance VARCHAR(300) NULL
			,RefrigeratedOrShelfStable VARCHAR(300) NULL
			,SmithsonianBirdFriendly VARCHAR(300) NULL
			,WIC VARCHAR(300) NULL
			)

		INSERT INTO #items (
			ItemID
			,ItemType
			,ScanCode
			,ProductDesc
			,POSDesc
			,PackageUnit
			,RetailSize
			,RetailUOM
			,FoodStampEligible
			,CustomerFriendlyDesc
			,SubTeamName
			,TaxClass
			)
		SELECT A.ItemID
			,C.itemTypeDesc
			,A.ScanCode
			,A.Desc_Product
			,A.Desc_POS
			,A.PackageUnit
			,A.RetailSize
			,A.RetailUOM
			,A.FoodStampEligible
			,A.Desc_CustomerFriendly
			,D.Name
			,E.HierarchyClassName
		FROM dbo.items A
		INNER JOIN #group B ON B.ItemID = A.ItemID
		LEFT JOIN ItemTypes C ON C.itemTypeID = A.ItemID
		LEFT JOIN dbo.Financial_SubTeam D ON D.PSNumber = A.PSNumber
		LEFT JOIN dbo.HierarchyClass E ON E.HierarchyClassID = A.TaxClassHCID;

		UPDATE A
		SET NationalHierarchyClass = D.HierarchyClassName
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN Hierarchy_NationalClass C ON C.HierarchyNationalClassID = B.HierarchyNationalClassID
		INNER JOIN HierarchyClass D ON D.HierarchyClassID = C.ClassHCID;

		UPDATE A
		SET MerchandiseHierarchySubBrick = D.HierarchyClassName
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN Hierarchy_Merchandise C ON C.HierarchyMerchandiseID = B.HierarchyMerchandiseID
		INNER JOIN HierarchyClass D ON D.HierarchyClassID = C.BrickHCID;

		UPDATE A
		SET BrandName = C.HierarchyClassName
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN HierarchyClass C ON C.HierarchyClassID = B.BrandHCID;

		UPDATE A
		SET FairTradeCertifed = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @fairTradeID;

		UPDATE A
		SET FlexibleText = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @flexibleTextID;

		UPDATE A
		SET GlobalPricingProgram = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @globalPricingID;

		UPDATE A
		SET MadeWithOrganicGrapes = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @organicGrapesID;

		UPDATE A
		SET NumberOfDigitsSentToScale = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @scaleDigitsID;

		UPDATE A
		SET NutritionRequired = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @nutritionRequiredID;

		UPDATE A
		SET PrimeBeef = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @primeBeefID;

		UPDATE A
		SET RefrigeratedOrShelfStable = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @refrigeratedShelfID;

		UPDATE A
		SET RainforestAlliance = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @rainforestID;

		UPDATE A
		SET SmithsonianBirdFriendly = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @smithsonianID;

		UPDATE A
		SET WIC = C.AttributeValue
		FROM #items A
		INNER JOIN dbo.Items B ON B.ItemID = A.ItemID
		INNER JOIN dbo.ItemAttributes_Ext C ON C.ItemID = B.ItemID
		WHERE C.AttributeID = @wicID;

		SELECT A.ItemID
			,A.ItemType
			,A.ScanCode
			,A.MerchandiseHierarchySubBrick
			,A.NationalHierarchyClass
			,A.BrandName
			,A.TaxClass
			,A.SubTeamName
			,A.ProductDesc
			,A.POSDesc
			,A.PackageUnit
			,A.RetailSize
			,A.RetailUOM
			,A.FoodStampEligible
			,A.CustomerFriendlyDesc
			,A.FairTradeCertifed
			,A.FlexibleText
			,A.GlobalPricingProgram
			,A.MadeWithOrganicGrapes
			,A.NumberOfDigitsSentToScale
			,A.NutritionRequired
			,A.PrimeBeef
			,A.RainforestAlliance
			,A.RefrigeratedOrShelfStable
			,A.SmithsonianBirdFriendly
			,A.WIC
			,B.RecipeName
			,B.Allergens
			,B.Ingredients
			,B.ServingsPerPortion
			,B.ServingSizeDesc
			,B.ServingPerContainer
			,B.HshRating
			,B.ServingUnits
			,B.SizeWeight
			,B.Calories
			,B.CaloriesFat
			,B.CaloriesSaturatedFat
			,B.TotalFatWeight
			,B.TotalFatPercentage
			,B.SaturatedFatWeight
			,B.SaturatedFatPercent
			,B.PolyunsaturatedFat
			,B.MonounsaturatedFat
			,B.CholesterolWeight
			,B.CholesterolPercent
			,B.SodiumWeight
			,B.SodiumPercent
			,B.PotassiumWeight
			,B.PotassiumPercent
			,B.TotalCarbohydrateWeight
			,B.TotalCarbohydratePercent
			,B.DietaryFiberWeight
			,B.DietaryFiberPercent
			,B.SolubleFiber
			,B.InsolubleFiber
			,B.Sugar
			,B.SugarAlcohol
			,B.OtherCarbohydrates
			,B.ProteinWeight
			,B.ProteinPercent
			,B.VitaminA
			,B.Betacarotene
			,B.VitaminC
			,B.Calcium
			,B.Iron
			,B.VitaminD
			,B.VitaminE
			,B.Thiamin
			,B.Riboflavin
			,B.Niacin
			,B.VitaminB6
			,B.Folate
			,B.VitaminB12
			,B.Biotin
			,B.PantothenicAcid
			,B.Phosphorous
			,B.Iodine
			,B.Magnesium
			,B.Zinc
			,B.Copper
			,B.Transfat
			,B.CaloriesFromTransFat
			,B.Om6Fatty
			,B.Om3Fatty
			,B.Starch
			,B.Chloride
			,B.Chromium
			,B.VitaminK
			,B.Manganese
			,B.Molybdenum
			,B.Selenium
			,B.TransFatWeight
		FROM #items A
		LEFT JOIN dbo.ItemAttributes_Nutrition B ON B.ItemID = A.ItemID;

		IF (object_id('tempdb..#group') IS NOT NULL)
			DROP TABLE #group;

		IF (object_id('tempdb..#items') IS NOT NULL)
			DROP TABLE #items;
	END
END
GO

GRANT EXECUTE ON dbo.AuditGlobalItem TO [MammothRole]
GO