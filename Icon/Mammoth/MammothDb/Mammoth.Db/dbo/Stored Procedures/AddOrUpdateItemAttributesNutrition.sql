CREATE PROCEDURE [dbo].[AddOrUpdateItemAttributesNutrition] @nutritionAttributes [dbo].ItemNutritionAttributesType READONLY
AS
BEGIN 
  -- =========================================
  -- Formatted by Poor SQL
	-- Locale Variables
	-- =========================================
	DECLARE @today DATETIME = GETDATE()
		,@totalRecordCount INT
		,@insertRecordCount INT;

	-- =========================================
	-- Insert / Update based on ItemID
	-- =========================================
	IF (object_id('tempdb..#tmpDelete') IS NOT NULL)
		DROP TABLE #tmpDelete;

	IF (object_id('tempdb..#nutrition') IS NOT NULL)
		DROP TABLE #nutrition;

	IF (object_id('tempdb..#insertNutrition') IS NOT NULL)
		DROP TABLE #insertNutrition;

	SELECT * INTO #nutritionAttributes FROM @nutritionAttributes;

	SELECT DISTINCT ItemID
	INTO #tmpDelete
	FROM #nutritionAttributes
	WHERE IsNull(Allergens, '') = ''
		AND IsNull(Ingredients, '') = ''
		AND IsNull(ServingsPerPortion, 0) <= 0
		AND IsNull(ServingSizeDesc, '') = ''
		AND IsNull(ServingPerContainer, '') = ''
		AND IsNull(HshRating, 0) <= 0
		AND IsNull(ServingUnits, 0) <= 0
		AND IsNull(SizeWeight, 0) <= 0
		AND IsNull(Calories, 0) <= 0
		AND IsNull(CaloriesFat, 0) <= 0
		AND IsNull(CaloriesSaturatedFat, 0) <= 0
		AND IsNull(TotalFatWeight, 0) <= 0
		AND IsNull(TotalFatPercentage, 0) <= 0
		AND IsNull(SaturatedFatWeight, 0) <= 0
		AND IsNull(SaturatedFatPercent, 0) <= 0
		AND IsNull(PolyunsaturatedFat, 0) <= 0
		AND IsNull(MonounsaturatedFat, 0) <= 0
		AND IsNull(CholesterolWeight, 0) <= 0
		AND IsNull(CholesterolPercent, 0) <= 0
		AND IsNull(SodiumWeight, 0) <= 0
		AND IsNull(SodiumPercent, 0) <= 0
		AND IsNull(PotassiumWeight, 0) <= 0
		AND IsNull(PotassiumPercent, 0) <= 0
		AND IsNull(TotalCarbohydrateWeight, 0) <= 0
		AND IsNull(TotalCarbohydratePercent, 0) <= 0
		AND IsNull(DietaryFiberWeight, 0) <= 0
		AND IsNull(DietaryFiberPercent, 0) <= 0
		AND IsNull(SolubleFiber, 0) <= 0
		AND IsNull(InsolubleFiber, 0) <= 0
		AND IsNull(Sugar, 0) <= 0
		AND IsNull(SugarAlcohol, 0) <= 0
		AND IsNull(OtherCarbohydrates, 0) <= 0
		AND IsNull(ProteinWeight, 0) <= 0
		AND IsNull(ProteinPercent, 0) <= 0
		AND IsNull(VitaminA, 0) <= 0
		AND IsNull(Betacarotene, 0) <= 0
		AND IsNull(VitaminC, 0) <= 0
		AND IsNull(Calcium, 0) <= 0
		AND IsNull(Iron, 0) <= 0
		AND IsNull(VitaminD, 0) <= 0
		AND IsNull(VitaminE, 0) <= 0
		AND IsNull(Thiamin, 0) <= 0
		AND IsNull(Riboflavin, 0) <= 0
		AND IsNull(Niacin, 0) <= 0
		AND IsNull(VitaminB6, 0) <= 0
		AND IsNull(Folate, 0) <= 0
		AND IsNull(VitaminB12, 0) <= 0
		AND IsNull(Biotin, 0) <= 0
		AND IsNull(PantothenicAcid, 0) <= 0
		AND IsNull(Phosphorous, 0) <= 0
		AND IsNull(Iodine, 0) <= 0
		AND IsNull(Magnesium, 0) <= 0
		AND IsNull(Zinc, 0) <= 0
		AND IsNull(Copper, 0) <= 0
		AND IsNull(TransFat, 0) <= 0
		AND IsNull(TransFatWeight, 0) <= 0
		AND IsNull(CaloriesFromTransFat, 0) <= 0
		AND IsNull(Om6Fatty, 0) <= 0
		AND IsNull(Om3Fatty, 0) <= 0
		AND IsNull(Starch, 0) <= 0
		AND IsNull(Chloride, 0) <= 0
		AND IsNull(Chromium, 0) <= 0
		AND IsNull(VitaminK, 0) <= 0
		AND IsNull(Manganese, 0) <= 0
		AND IsNull(Molybdenum, 0) <= 0
		AND IsNull(Selenium, 0) <= 0
		AND IsNull(AddedSugarsWeight, 0) <= 0
		AND IsNull(AddedSugarsPercent, 0) <= 0
		AND IsNull(CalciumWeight, 0) <= 0
		AND IsNull(IronWeight, 0) <= 0
		AND IsNull(VitaminDWeight, 0) <= 0
		AND IsNull(ProfitCenter, 0) <= 0 
		AND IsNull(CanadaAllergens, '') = ''
		AND IsNull(CanadaIngredients, '') = ''
		AND IsNull(CanadaSugarPercent, 0) <= 0
		AND IsNull(CanadaServingSizeDesc, '') = '';

	DELETE A
	FROM ItemAttributes_Nutrition A
	INNER JOIN #tmpDelete B ON B.ItemID = A.ItemID;

	SELECT A.*
	INTO #nutrition
	FROM #nutritionAttributes A
	LEFT JOIN #tmpDelete B ON B.ItemID = A.ItemID
	WHERE B.ItemID IS NULL;

	SET @totalRecordCount = @@ROWCOUNT;

	SELECT ItemID
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
		,TransFat
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
		,AddedSugarsWeight
		,AddedSugarsPercent
		,CalciumWeight
		,IronWeight
		,VitaminDWeight
		,ProfitCenter
		,CanadaAllergens
		,CanadaIngredients
		,CanadaSugarPercent
		,CanadaServingSizeDesc
	INTO #insertNutrition
	FROM #nutrition n
	WHERE NOT EXISTS (
			SELECT 1
			FROM dbo.ItemAttributes_Nutrition i
			WHERE i.ItemID = n.ItemID
			);

	SET @insertRecordCount = @@ROWCOUNT;

	BEGIN TRY
		BEGIN TRANSACTION

		IF @totalRecordCount <> @insertRecordCount
			UPDATE n
			SET n.RecipeName = t.RecipeName
				,n.Allergens = t.Allergens
				,n.Ingredients = t.Ingredients
				,n.ServingsPerPortion = t.ServingsPerPortion
				,n.ServingSizeDesc = t.ServingSizeDesc
				,n.ServingPerContainer = t.ServingPerContainer
				,n.HshRating = t.HshRating
				,n.ServingUnits = t.ServingUnits
				,n.SizeWeight = t.SizeWeight
				,n.Calories = t.Calories
				,n.CaloriesFat = t.CaloriesFat
				,n.CaloriesSaturatedFat = t.CaloriesSaturatedFat
				,n.TotalFatWeight = t.TotalFatWeight
				,n.TotalFatPercentage = t.TotalFatPercentage
				,n.SaturatedFatWeight = t.SaturatedFatWeight
				,n.SaturatedFatPercent = t.SaturatedFatPercent
				,n.PolyunsaturatedFat = t.PolyunsaturatedFat
				,n.MonounsaturatedFat = t.MonounsaturatedFat
				,n.CholesterolWeight = t.CholesterolWeight
				,n.CholesterolPercent = t.CholesterolPercent
				,n.SodiumWeight = t.SodiumWeight
				,n.SodiumPercent = t.SodiumPercent
				,n.PotassiumWeight = t.PotassiumWeight
				,n.PotassiumPercent = t.PotassiumPercent
				,n.TotalCarbohydrateWeight = t.TotalCarbohydrateWeight
				,n.TotalCarbohydratePercent = t.TotalCarbohydratePercent
				,n.DietaryFiberWeight = t.DietaryFiberWeight
				,n.DietaryFiberPercent = t.DietaryFiberPercent
				,n.SolubleFiber = t.SolubleFiber
				,n.InsolubleFiber = t.InsolubleFiber
				,n.Sugar = t.Sugar
				,n.SugarAlcohol = t.SugarAlcohol
				,n.OtherCarbohydrates = t.OtherCarbohydrates
				,n.ProteinWeight = t.ProteinWeight
				,n.ProteinPercent = t.ProteinPercent
				,n.VitaminA = t.VitaminA
				,n.Betacarotene = t.Betacarotene
				,n.VitaminC = t.VitaminC
				,n.Calcium = t.Calcium
				,n.Iron = t.Iron
				,n.VitaminD = t.VitaminD
				,n.VitaminE = t.VitaminE
				,n.Thiamin = t.Thiamin
				,n.Riboflavin = t.Riboflavin
				,n.Niacin = t.Niacin
				,n.VitaminB6 = t.VitaminB6
				,n.Folate = t.Folate
				,n.VitaminB12 = t.VitaminB12
				,n.Biotin = t.Biotin
				,n.PantothenicAcid = t.PantothenicAcid
				,n.Phosphorous = t.Phosphorous
				,n.Iodine = t.Iodine
				,n.Magnesium = t.Magnesium
				,n.Zinc = t.Zinc
				,n.Copper = t.Copper
				,n.TransFat = t.TransFat
				,n.CaloriesFromTransFat = t.CaloriesFromTransFat
				,n.Om6Fatty = t.Om6Fatty
				,n.Om3Fatty = t.Om3Fatty
				,n.Starch = t.Starch
				,n.Chloride = t.Chloride
				,n.Chromium = t.Chromium
				,n.VitaminK = t.VitaminK
				,n.Manganese = t.Manganese
				,n.Molybdenum = t.Molybdenum
				,n.Selenium = t.Selenium
				,n.TransFatWeight = t.TransFatWeight
				,n.AddedSugarsWeight = t.AddedSugarsWeight
				,n.AddedSugarsPercent = t.AddedSugarsPercent
				,n.CalciumWeight = t.CalciumWeight
				,n.IronWeight = t.IronWeight
				,n.VitaminDWeight = t.VitaminDWeight
				,n.ProfitCenter = t.ProfitCenter
				,n.CanadaAllergens = t.CanadaAllergens
				,n.CanadaIngredients = t.CanadaIngredients
				,n.CanadaSugarPercent = t.CanadaSugarPercent
				,n.CanadaServingSizeDesc = t.CanadaServingSizeDesc
			FROM dbo.ItemAttributes_Nutrition n
			INNER JOIN #nutrition t ON n.ItemID = t.ItemID

		IF @insertRecordCount > 0
			INSERT INTO dbo.ItemAttributes_Nutrition (
				ItemID
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
				,TransFat
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
				,AddedSugarsWeight
				,AddedSugarsPercent
				,CalciumWeight
				,IronWeight
				,VitaminDWeight
				,AddedDate
				,ProfitCenter
				,CanadaAllergens
				,CanadaIngredients
				,CanadaSugarPercent
				,CanadaServingSizeDesc
				)
			SELECT ItemID
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
				,TransFat
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
				,AddedSugarsWeight
				,AddedSugarsPercent
				,CalciumWeight
				,IronWeight
				,VitaminDWeight
				,@today
				,ProfitCenter
				,CanadaAllergens
				,CanadaIngredients
				,CanadaSugarPercent
				,CanadaServingSizeDesc
			FROM #insertNutrition

		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION;

		THROW
	END CATCH

	IF (object_id('temp_db..#tmpDelete') IS NOT NULL)
		DROP TABLE #tmpDelete;

	IF (object_id('temp_db..#nutrition') IS NOT NULL)
		DROP TABLE #nutrition;

	IF (object_id('temp_db..#insertNutrition') IS NOT NULL)
		DROP TABLE #insertNutrition;
END
GO

GRANT EXECUTE ON [dbo].[AddOrUpdateItemAttributesNutrition] TO [MammothRole]
GO