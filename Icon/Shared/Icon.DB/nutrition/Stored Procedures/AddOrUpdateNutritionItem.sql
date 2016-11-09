CREATE PROCEDURE [nutrition].[AddOrUpdateNutritionItem]
	@NutritionItem nutrition.NutritionItemType readonly
AS
BEGIN	
declare @resultMessage varchar(250);
Declare @updatedCount int;
Declare @insertedCount int;
DECLARE @distinctNutritionIDs table(id int, plu varchar(13), isNewPlu bit);
declare @nutritionUpdateEventType int;
declare @nutritionAddEventType int;
declare @nutritionUpdateSetting int;
declare @validationDateTraitID int;
			

	SET @nutritionUpdateEventType = (select EventId from app.EventType where EventName = 'Nutrition Update')
	SET @nutritionAddEventType = (select EventId from app.EventType where EventName = 'Nutrition Add')
	
			
	SET @nutritionUpdateSetting = (select s.SettingsId from app.Settings s
	                      join app.SettingSection ss on s.SettingSectionId = ss.SettingSectionId and ss.SectionName = 'Item'
						  where s.KeyName = 'SendItemNutritionUpdatesToIRMA');
	SET @validationDateTraitID = (select traitID from Trait where traitCode = 'VAL')
 
 


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
		  ,[ModifiedDate] = SYSDATETIME()

		 output INSERTED.RecipeId, newni.[Plu], 0 into @distinctNutritionIDs
	
		FROM [nutrition].[ItemNutrition] oldni
		INNER JOIN  @NutritionItem newni ON newni.Plu = oldni.Plu

	set @updatedCount  =  @@RowCount
	IF(@updatedCount > 0)
			SET @resultMessage = cast(@updatedCount as varchar) + ' items updated successfully.'
		
	insert into [nutrition].[ItemNutrition]
        (	[Plu]
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
		)	
		 output INSERTED.RecipeId, INSERTED.Plu, 1 into @distinctNutritionIDs	
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
		  ,null
		 FROM @NutritionItem newItem
		 
		 WHERE
		NOT EXISTS (SELECT 1 FROM  nutrition.ItemNutrition oldItem
              WHERE oldItem.Plu = newItem.Plu)
	
	SET @insertedCount = @@ROWCOUNT
		
	IF(@insertedCount > 0)
		BEGIN			
			IF(len(@resultMessage) > 0)
				SET @resultMessage = @resultMessage + ' and ' +  cast(@insertedCount as varchar) + ' items inserted successfully.'
			ELSE
				SET @resultMessage = cast(@insertedCount as varchar) + ' items inserted successfully.'
		END


	--Update / insert item sign attribute for health status rating

	update isa
	set isa.HealthyEatingRatingId = her.HealthyEatingRatingId
	from dbo.ItemSignAttribute isa
	JOIN dbo.ScanCode sc on isa.ItemID = sc.itemID
	join nutrition.ItemNutrition itn on sc.scanCode = itn.Plu
	JOIN dbo.HealthyEatingRating her on itn.HshRating = her.HealthyEatingRatingId
	JOIN @NutritionItem newitem on sc.scanCode = newitem.Plu

	insert into dbo.ItemSignAttribute(ItemId, HealthyEatingRatingId)
	select sc.itemID, her.HealthyEatingRatingId
	from dbo.ScanCode sc
	join nutrition.ItemNutrition itn on sc.scanCode = itn.Plu
	JOIN dbo.HealthyEatingRating her on itn.HshRating = her.HealthyEatingRatingId
	JOIN @NutritionItem newitem on sc.scanCode = newitem.Plu
	where not exists (select 1 from dbo.ItemSignAttribute isa where isa.ItemID = sc.itemID)


	
	--Generate Item Update events for validated scan codes

	;
	 WITH config_CTE AS
	 ( SELECT r.RegionCode  from app.RegionalSettings rs 
				join app.Regions r on rs.RegionId = r.RegionId
				 where rs.SettingsId = @nutritionUpdateSetting and rs.Value = 1
	 )
	
	
	INSERT INTO app.EventQueue
	SELECT @nutritionUpdateEventType, dii.plu, dii.id, iis.regionCode, sysdatetime(), null, null
	FROM @distinctNutritionIDs dii	
	JOIN ScanCode sc
		ON sc.scanCode = dii.plu
	JOIN app.IRMAItemSubscription iis
		ON sc.scanCode = iis.identifier AND iis.deleteDate is NULL
	JOIN Item i
		ON sc.itemID = i.itemID
	JOIN ItemTrait it
		ON i.itemID = it.itemID
		AND it.traitID = @validationDateTraitID
	JOIN config_CTE config 
		ON iis.regionCode = config.RegionCode
	where dii.isNewPlu = 0

	;
	 WITH config_CTE AS
	 ( SELECT r.RegionCode  from app.RegionalSettings rs 
				join app.Regions r on rs.RegionId = r.RegionId
				 where rs.SettingsId = @nutritionUpdateSetting and rs.Value = 1
	 )

	INSERT INTO app.EventQueue
	SELECT @nutritionAddEventType, dii.plu, dii.id, iis.regionCode, sysdatetime(), null, null
	FROM @distinctNutritionIDs dii	
	JOIN ScanCode sc
		ON sc.scanCode = dii.plu
	JOIN app.IRMAItemSubscription iis
		ON sc.scanCode = iis.identifier AND iis.deleteDate is NULL
	JOIN Item i
		ON sc.itemID = i.itemID
	JOIN ItemTrait it
		ON i.itemID = it.itemID
		AND it.traitID = @validationDateTraitID
	JOIN config_CTE config 
		ON iis.regionCode = config.RegionCode
	where dii.isNewPlu = 1


	---	--Generate messages to ESB for validated items.
	declare @distinctItemIDs app.UpdatedItemIDsType
	insert into @distinctItemIDs
	select distinct sc.itemID
	from @distinctNutritionIDs dii
	JOIN ScanCode sc
		ON sc.scanCode = dii.plu

	exec app.GenerateItemUpdateMessages @distinctItemIDs

	 select @resultMessage;
END

GO