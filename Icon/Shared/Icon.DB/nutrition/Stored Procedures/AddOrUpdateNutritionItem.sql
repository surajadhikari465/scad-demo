CREATE PROCEDURE [nutrition].[AddOrUpdateNutritionItem] @NutritionItem nutrition.NutritionItemType readonly
AS
BEGIN
	DECLARE @resultMessage VARCHAR(250);
	DECLARE @updatedCount INT;
	DECLARE @insertedCount INT;
	DECLARE @distinctNutritionIDs TABLE (
		id INT
		,plu VARCHAR(13)
		,isNewPlu BIT
		);
	DECLARE @nutritionUpdateEventType INT;
	DECLARE @nutritionAddEventType INT;
	DECLARE @nutritionUpdateSetting INT;
	DECLARE @validationDateTraitID INT;

	SET @nutritionUpdateEventType = (
			SELECT EventId
			FROM app.EventType
			WHERE EventName = 'Nutrition Update'
			)
	SET @nutritionAddEventType = (
			SELECT EventId
			FROM app.EventType
			WHERE EventName = 'Nutrition Add'
			)
	SET @nutritionUpdateSetting = (
			SELECT s.SettingsId
			FROM app.Settings s
			JOIN app.SettingSection ss ON s.SettingSectionId = ss.SettingSectionId
				AND ss.SectionName = 'Item'
			WHERE s.KeyName = 'SendItemNutritionUpdatesToIRMA'
			);
	SET @validationDateTraitID = (
			SELECT traitID
			FROM Trait
			WHERE traitCode = 'VAL'
			)

	SELECT *
	INTO #nutritionItem
	FROM @NutritionItem;

	UPDATE [nutrition].[ItemNutrition]
	SET [RecipeName] = newni.RecipeName
		,[Allergens] = newni.Allergens
		,[Ingredients] = newni.Ingredients
		,[ServingsPerPortion] = newni.ServingsPerPortion
		,[ServingSizeDesc] = newni.ServingSizeDesc
		,[ServingPerContainer] = newni.ServingPerContainer
		,[HshRating] = newni.HshRating
		,[ServingUnits] = newni.ServingUnits
		,[SizeWeight] = newni.SizeWeight
		,[Calories] = newni.Calories
		,[CaloriesFat] = newni.CaloriesFat
		,[CaloriesSaturatedFat] = newni.CaloriesSaturatedFat
		,[TotalFatWeight] = newni.TotalFatWeight
		,[TotalFatPercentage] = newni.TotalFatPercentage
		,[SaturatedFatWeight] = newni.SaturatedFatWeight
		,[SaturatedFatPercent] = newni.SaturatedFatPercent
		,[PolyunsaturatedFat] = newni.PolyunsaturatedFat
		,[MonounsaturatedFat] = newni.MonounsaturatedFat
		,[CholesterolWeight] = newni.CholesterolWeight
		,[CholesterolPercent] = newni.CholesterolPercent
		,[SodiumWeight] = newni.SodiumWeight
		,[SodiumPercent] = newni.SodiumPercent
		,[PotassiumWeight] = newni.PotassiumWeight
		,[PotassiumPercent] = newni.PotassiumPercent
		,[TotalCarbohydrateWeight] = newni.TotalCarbohydrateWeight
		,[TotalCarbohydratePercent] = newni.TotalCarbohydratePercent
		,[DietaryFiberWeight] = newni.DietaryFiberWeight
		,[DietaryFiberPercent] = newni.DietaryFiberPercent
		,[SolubleFiber] = newni.SolubleFiber
		,[InsolubleFiber] = newni.InsolubleFiber
		,[Sugar] = newni.Sugar
		,[SugarAlcohol] = newni.SugarAlcohol
		,[OtherCarbohydrates] = newni.OtherCarbohydrates
		,[ProteinWeight] = newni.ProteinWeight
		,[ProteinPercent] = newni.ProteinPercent
		,[VitaminA] = newni.VitaminA
		,[Betacarotene] = newni.Betacarotene
		,[VitaminC] = newni.VitaminC
		,[Calcium] = newni.Calcium
		,[Iron] = newni.Iron
		,[VitaminD] = newni.VitaminD
		,[VitaminE] = newni.VitaminE
		,[Thiamin] = newni.Thiamin
		,[Riboflavin] = newni.Riboflavin
		,[Niacin] = newni.Niacin
		,[VitaminB6] = newni.VitaminB6
		,[Folate] = newni.Folate
		,[VitaminB12] = newni.VitaminB12
		,[Biotin] = newni.Biotin
		,[PantothenicAcid] = newni.PantothenicAcid
		,[Phosphorous] = newni.Phosphorous
		,[Iodine] = newni.Iodine
		,[Magnesium] = newni.Magnesium
		,[Zinc] = newni.Zinc
		,[Copper] = newni.Copper
		,[Transfat] = newni.Transfat
		,[CaloriesFromTransfat] = newni.CaloriesFromTransfat
		,[Om6Fatty] = newni.Om6Fatty
		,[Om3Fatty] = newni.Om3Fatty
		,[Starch] = newni.Starch
		,[Chloride] = newni.Chloride
		,[Chromium] = newni.Chromium
		,[VitaminK] = newni.VitaminK
		,[Manganese] = newni.Manganese
		,[Molybdenum] = newni.Molybdenum
		,[Selenium] = newni.Selenium
		,[TransfatWeight] = newni.TransfatWeight
		,[AddedSugarsWeight] = newni.AddedSugarsWeight
		,[AddedSugarsPercent] = newni.AddedSugarsPercent
		,[CalciumWeight] = newni.CalciumWeight
		,[IronWeight] = newni.IronWeight
		,[VitaminDWeight] = newni.VitaminDWeight
		,[ModifiedDate] = SYSDATETIME()
		,[ProfitCenter] = newni.ProfitCenter
		,[CanadaAllergens] = newni.CanadaAllergens
		,[CanadaIngredients] = newni.CanadaIngredients
		,[CanadaSugarPercent] = newni.CanadaSugarPercents
	OUTPUT INSERTED.RecipeId
		,newni.[Plu]
		,0
	INTO @distinctNutritionIDs
	FROM [nutrition].[ItemNutrition] oldni
	INNER JOIN #nutritionItem newni ON newni.Plu = oldni.Plu

	SET @updatedCount = @@RowCount

	IF (@updatedCount > 0)
		SET @resultMessage = cast(@updatedCount AS VARCHAR) + ' items updated successfully.'

	INSERT INTO [nutrition].[ItemNutrition] (
		[Plu]
		,[RecipeName]
		,[Allergens]
		,[Ingredients]
		,[ServingsPerPortion]
		,[ServingSizeDesc]
		,[ServingPerContainer]
		,[HshRating]
		,[ServingUnits]
		,[SizeWeight]
		,[Calories]
		,[CaloriesFat]
		,[CaloriesSaturatedFat]
		,[TotalFatWeight]
		,[TotalFatPercentage]
		,[SaturatedFatWeight]
		,[SaturatedFatPercent]
		,[PolyunsaturatedFat]
		,[MonounsaturatedFat]
		,[CholesterolWeight]
		,[CholesterolPercent]
		,[SodiumWeight]
		,[SodiumPercent]
		,[PotassiumWeight]
		,[PotassiumPercent]
		,[TotalCarbohydrateWeight]
		,[TotalCarbohydratePercent]
		,[DietaryFiberWeight]
		,[DietaryFiberPercent]
		,[SolubleFiber]
		,[InsolubleFiber]
		,[Sugar]
		,[SugarAlcohol]
		,[OtherCarbohydrates]
		,[ProteinWeight]
		,[ProteinPercent]
		,[VitaminA]
		,[Betacarotene]
		,[VitaminC]
		,[Calcium]
		,[Iron]
		,[VitaminD]
		,[VitaminE]
		,[Thiamin]
		,[Riboflavin]
		,[Niacin]
		,[VitaminB6]
		,[Folate]
		,[VitaminB12]
		,[Biotin]
		,[PantothenicAcid]
		,[Phosphorous]
		,[Iodine]
		,[Magnesium]
		,[Zinc]
		,[Copper]
		,[Transfat]
		,[CaloriesFromTransfat]
		,[Om6Fatty]
		,[Om3Fatty]
		,[Starch]
		,[Chloride]
		,[Chromium]
		,[VitaminK]
		,[Manganese]
		,[Molybdenum]
		,[Selenium]
		,[TransfatWeight]
		,[InsertDate]
		,[ModifiedDate]
		,[AddedSugarsWeight]
		,[AddedSugarsPercent]
		,[CalciumWeight]
		,[IronWeight]
		,[VitaminDWeight]
		,[ProfitCenter]
		,[CanadaAllergens]
		,[CanadaIngredients]
		,[CanadaSugarPercent]
		)
	OUTPUT INSERTED.RecipeId
		,INSERTED.Plu
		,1
	INTO @distinctNutritionIDs
	SELECT [Plu]
		,[RecipeName]
		,[Allergens]
		,[Ingredients]
		,[ServingsPerPortion]
		,[ServingSizeDesc]
		,[ServingPerContainer]
		,[HshRating]
		,[ServingUnits]
		,[SizeWeight]
		,[Calories]
		,[CaloriesFat]
		,[CaloriesSaturatedFat]
		,[TotalFatWeight]
		,[TotalFatPercentage]
		,[SaturatedFatWeight]
		,[SaturatedFatPercent]
		,[PolyunsaturatedFat]
		,[MonounsaturatedFat]
		,[CholesterolWeight]
		,[CholesterolPercent]
		,[SodiumWeight]
		,[SodiumPercent]
		,[PotassiumWeight]
		,[PotassiumPercent]
		,[TotalCarbohydrateWeight]
		,[TotalCarbohydratePercent]
		,[DietaryFiberWeight]
		,[DietaryFiberPercent]
		,[SolubleFiber]
		,[InsolubleFiber]
		,[Sugar]
		,[SugarAlcohol]
		,[OtherCarbohydrates]
		,[ProteinWeight]
		,[ProteinPercent]
		,[VitaminA]
		,[Betacarotene]
		,[VitaminC]
		,[Calcium]
		,[Iron]
		,[VitaminD]
		,[VitaminE]
		,[Thiamin]
		,[Riboflavin]
		,[Niacin]
		,[VitaminB6]
		,[Folate]
		,[VitaminB12]
		,[Biotin]
		,[PantothenicAcid]
		,[Phosphorous]
		,[Iodine]
		,[Magnesium]
		,[Zinc]
		,[Copper]
		,[Transfat]
		,[CaloriesFromTransfat]
		,[Om6Fatty]
		,[Om3Fatty]
		,[Starch]
		,[Chloride]
		,[Chromium]
		,[VitaminK]
		,[Manganese]
		,[Molybdenum]
		,[Selenium]
		,[TransfatWeight]
		,SYSDATETIME()
		,NULL
		,[AddedSugarsWeight]
		,[AddedSugarsPercent]
		,[CalciumWeight]
		,[IronWeight]
		,[VitaminDWeight]
		,[ProfitCenter]
		,[CanadaAllergens]
		,[CanadaIngredients]
		,[CanadaSugarPercent]
	FROM @NutritionItem newItem
	WHERE NOT EXISTS (
			SELECT 1
			FROM nutrition.ItemNutrition oldItem
			WHERE oldItem.Plu = newItem.Plu
			)

	SET @insertedCount = @@ROWCOUNT

	IF (@insertedCount > 0)
	BEGIN
		IF (len(@resultMessage) > 0)
			SET @resultMessage = @resultMessage + ' and ' + cast(@insertedCount AS VARCHAR) + ' items inserted successfully.'
		ELSE
			SET @resultMessage = cast(@insertedCount AS VARCHAR) + ' items inserted successfully.'
	END

	--Update / insert item sign attribute for health status rating
	UPDATE isa
	SET isa.HealthyEatingRatingId = her.HealthyEatingRatingId
	FROM dbo.ItemSignAttribute isa
	JOIN dbo.ScanCode sc ON isa.ItemID = sc.itemID
	JOIN nutrition.ItemNutrition itn ON sc.scanCode = itn.Plu
	JOIN dbo.HealthyEatingRating her ON itn.HshRating = her.HealthyEatingRatingId
	JOIN @NutritionItem newitem ON sc.scanCode = newitem.Plu

	INSERT INTO dbo.ItemSignAttribute (
		ItemId
		,HealthyEatingRatingId
		)
	SELECT sc.itemID
		,her.HealthyEatingRatingId
	FROM dbo.ScanCode sc
	JOIN nutrition.ItemNutrition itn ON sc.scanCode = itn.Plu
	JOIN dbo.HealthyEatingRating her ON itn.HshRating = her.HealthyEatingRatingId
	JOIN @NutritionItem newitem ON sc.scanCode = newitem.Plu
	WHERE NOT EXISTS (
			SELECT 1
			FROM dbo.ItemSignAttribute isa
			WHERE isa.ItemID = sc.itemID
			)
		--Generate Item Update events for validated scan codes
		

	;WITH config_CTE
	AS (
		SELECT r.RegionCode
		FROM app.RegionalSettings rs
		JOIN app.Regions r ON rs.RegionId = r.RegionId
		WHERE rs.SettingsId = @nutritionUpdateSetting
			AND rs.Value = 1
		)
	INSERT INTO app.EventQueue
	SELECT @nutritionUpdateEventType
		,dii.plu
		,dii.id
		,iis.regionCode
		,sysdatetime()
		,NULL
		,NULL
	FROM @distinctNutritionIDs dii
	JOIN ScanCode sc ON sc.scanCode = dii.plu
	JOIN app.IRMAItemSubscription iis ON sc.scanCode = iis.identifier
		AND iis.deleteDate IS NULL
	JOIN Item i ON sc.itemID = i.itemID
	JOIN ItemTrait it ON i.itemID = it.itemID
		AND it.traitID = @validationDateTraitID
	JOIN config_CTE config ON iis.regionCode = config.RegionCode
	WHERE dii.isNewPlu = 0

	;WITH config_CTE
	AS (
		SELECT r.RegionCode
		FROM app.RegionalSettings rs
		JOIN app.Regions r ON rs.RegionId = r.RegionId
		WHERE rs.SettingsId = @nutritionUpdateSetting
			AND rs.Value = 1
		)
	INSERT INTO app.EventQueue
	SELECT @nutritionAddEventType
		,dii.plu
		,dii.id
		,iis.regionCode
		,sysdatetime()
		,NULL
		,NULL
	FROM @distinctNutritionIDs dii
	JOIN ScanCode sc ON sc.scanCode = dii.plu
	JOIN app.IRMAItemSubscription iis ON sc.scanCode = iis.identifier
		AND iis.deleteDate IS NULL
	JOIN Item i ON sc.itemID = i.itemID
	JOIN ItemTrait it ON i.itemID = it.itemID
		AND it.traitID = @validationDateTraitID
	JOIN config_CTE config ON iis.regionCode = config.RegionCode
	WHERE dii.isNewPlu = 1

	-- generate message to ESB using the MessageQueueItem table
	DECLARE @itemIds esb.MessageQueueItemIdsType

	INSERT INTO @itemIds
	SELECT sc.ItemId
		,GETUTCDATE()
		,GETUTCDATE()
	FROM @distinctNutritionIDs dids
	INNER JOIN dbo.ScanCode sc ON sc.scanCode = dids.plu

	EXEC esb.AddMessageQueueItem @itemIds

	SELECT @resultMessage;
END
GO

