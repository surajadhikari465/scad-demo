
CREATE PROCEDURE [dbo].[UpdateNutriFactsFromIcon]
	
	@IconNutrifacts dbo.[IconItemNutriFactsType] READONLY,
	@UserName varchar(25)
AS
BEGIN
	SET NOCOUNT ON;



	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @userId int;
	DECLARE @now datetime;
	DECLARE @NewNutrifacts table(plu varchar(50), nutrifactsId int, isScalePlu bit);

	SET @userId = (SELECT u.User_ID FROM Users u WHERE u.UserName = @UserName);
	SET @now = (SELECT GETDATE());

	SELECT DISTINCT icn.*, dbo.fn_IsScaleItem(plu) as IsScaleItem, dbo.fn_IsCustomerFacingScaleItem(plu) as IsCfsItem
	INTO #distinctList	
	FROM @IconNutrifacts icn

	-- Update Nutrifacts for non-scale items
	
	BEGIN TRY
		UPDATE nf
		SET          
            Description = dl.Plu,
            ServingsPerPortion = dl.ServingsPerPortion,
            ServingSizeDesc = dl.ServingSizeDesc,
            ServingPerContainer = dl.ServingPerContainer,
            HshRating = dl.HshRating,
            ServingUnits = dl.ServingUnits,
            SizeWeight = dl.SizeWeight,
            Calories = dl.Calories,
            CaloriesFat = dl.CaloriesFat,
            CaloriesSaturatedFat = dl.CaloriesSaturatedFat,
            TotalFatWeight = dl.TotalFatWeight,
            TotalFatPercentage = dl.TotalFatPercentage,
            SaturatedFatWeight = dl.SaturatedFatWeight,
            SaturatedFatPercent = dl.SaturatedFatPercent,
            PolyunsaturatedFat = dl.PolyunsaturatedFat,
            MonounsaturatedFat = dl.MonounsaturatedFat,
            CholesterolWeight = dl.CholesterolWeight,
            CholesterolPercent = dl.CholesterolPercent,
            SodiumWeight = dl.SodiumWeight,
            SodiumPercent = dl.SodiumPercent,
            PotassiumWeight = dl.PotassiumWeight,
            PotassiumPercent = dl.PotassiumPercent,
            TotalCarbohydrateWeight = dl.TotalCarbohydrateWeight,
            TotalCarbohydratePercent = dl.TotalCarbohydratePercent,
            DietaryFiberWeight = dl.DietaryFiberWeight,
            DietaryFiberPercent = dl.DietaryFiberPercent,
            SolubleFiber = dl.SolubleFiber,
            InsolubleFiber = dl.InsolubleFiber,
            Sugar = dl.Sugar,
            SugarAlcohol = dl.SugarAlcohol,
            OtherCarbohydrates = dl.OtherCarbohydrates,
            ProteinWeight = dl.ProteinWeight,
            ProteinPercent = dl.ProteinPercent,
            VitaminA = dl.VitaminA,
            Betacarotene = dl.Betacarotene,
            VitaminC = dl.VitaminC,
            Calcium = dl.Calcium,
            Iron = dl.Iron,
            VitaminD = dl.VitaminD,
            VitaminE = dl.VitaminE,
            Thiamin = dl.Thiamin,
            Riboflavin = dl.Riboflavin,
            Niacin = dl.Niacin,
            VitaminB6 = dl.VitaminB6,
            Folate = dl.Folate,
            VitaminB12 = dl.VitaminB12,
            Biotin = dl.Biotin,
            PantothenicAcid = dl.PantothenicAcid,
            Phosphorous = dl.Phosphorous,
            Iodine = dl.Iodine,
            Magnesium = dl.Magnesium,
            Zinc = dl.Zinc,
            Copper = dl.Copper,
            Transfat = dl.Transfat,
            CaloriesFromTransfat = dl.CaloriesFromTransfat,
            Om6Fatty = dl.Om6Fatty,
            Om3Fatty = dl.Om3Fatty,
            Starch = dl.Starch,
            Chloride = dl.Chloride,
            Chromium = dl.Chromium,
            VitaminK = dl.VitaminK,
            Manganese = dl.Manganese,
            Molybdenum = dl.Molybdenum,
            Selenium = dl.Selenium,
            TransfatWeight = dl.TransfatWeight
		FROM NutriFacts nf
		JOIN ItemNutrition inf on nf.NutriFactsID = inf.NutriFactsID
		JOIN ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
											AND ii.Item_Key = inf.ItemKey											
		JOIN #distinctList		dl on ii.Identifier = dl.Plu and (dl.IsScaleItem = 0 OR dl.IsCfsItem = 1)
		


	-- Update Nutrifacts for scale items
	UPDATE nf
		SET          
            Description = dl.Plu,
            ServingsPerPortion = dl.ServingsPerPortion,
            ServingSizeDesc = dl.ServingSizeDesc,
            ServingPerContainer = dl.ServingPerContainer,
            HshRating = dl.HshRating,
            ServingUnits = dl.ServingUnits,
            SizeWeight = dl.SizeWeight,
            Calories = dl.Calories,
            CaloriesFat = dl.CaloriesFat,
            CaloriesSaturatedFat = dl.CaloriesSaturatedFat,
            TotalFatWeight = dl.TotalFatWeight,
            TotalFatPercentage = dl.TotalFatPercentage,
            SaturatedFatWeight = dl.SaturatedFatWeight,
            SaturatedFatPercent = dl.SaturatedFatPercent,
            PolyunsaturatedFat = dl.PolyunsaturatedFat,
            MonounsaturatedFat = dl.MonounsaturatedFat,
            CholesterolWeight = dl.CholesterolWeight,
            CholesterolPercent = dl.CholesterolPercent,
            SodiumWeight = dl.SodiumWeight,
            SodiumPercent = dl.SodiumPercent,
            PotassiumWeight = dl.PotassiumWeight,
            PotassiumPercent = dl.PotassiumPercent,
            TotalCarbohydrateWeight = dl.TotalCarbohydrateWeight,
            TotalCarbohydratePercent = dl.TotalCarbohydratePercent,
            DietaryFiberWeight = dl.DietaryFiberWeight,
            DietaryFiberPercent = dl.DietaryFiberPercent,
            SolubleFiber = dl.SolubleFiber,
            InsolubleFiber = dl.InsolubleFiber,
            Sugar = dl.Sugar,
            SugarAlcohol = dl.SugarAlcohol,
            OtherCarbohydrates = dl.OtherCarbohydrates,
            ProteinWeight = dl.ProteinWeight,
            ProteinPercent = dl.ProteinPercent,
            VitaminA = dl.VitaminA,
            Betacarotene = dl.Betacarotene,
            VitaminC = dl.VitaminC,
            Calcium = dl.Calcium,
            Iron = dl.Iron,
            VitaminD = dl.VitaminD,
            VitaminE = dl.VitaminE,
            Thiamin = dl.Thiamin,
            Riboflavin = dl.Riboflavin,
            Niacin = dl.Niacin,
            VitaminB6 = dl.VitaminB6,
            Folate = dl.Folate,
            VitaminB12 = dl.VitaminB12,
            Biotin = dl.Biotin,
            PantothenicAcid = dl.PantothenicAcid,
            Phosphorous = dl.Phosphorous,
            Iodine = dl.Iodine,
            Magnesium = dl.Magnesium,
            Zinc = dl.Zinc,
            Copper = dl.Copper,
            Transfat = dl.Transfat,
            CaloriesFromTransfat = dl.CaloriesFromTransfat,
            Om6Fatty = dl.Om6Fatty,
            Om3Fatty = dl.Om3Fatty,
            Starch = dl.Starch,
            Chloride = dl.Chloride,
            Chromium = dl.Chromium,
            VitaminK = dl.VitaminK,
            Manganese = dl.Manganese,
            Molybdenum = dl.Molybdenum,
            Selenium = dl.Selenium,
            TransfatWeight = dl.TransfatWeight
		FROM
		NutriFacts nf
		JOIN ItemScale its on nf.NutriFactsID = its.Nutrifact_ID
		JOIN ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
											AND ii.Item_Key = its.Item_Key
		JOIN #distinctList		dl on ii.Identifier = dl.Plu  and (dl.IsScaleItem = 1 OR dl.IsCfsItem = 1)

	--Insert new rows for non-scale items

	insert into NutriFacts ([Description]
      ,[ServingsPerPortion]
      ,[SizeWeight]
      ,[Calories]
      ,[CaloriesFat]
      ,[CaloriesSaturatedFat]
      ,[ServingPerContainer]
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
      ,[CaloriesFromTransFat]
      ,[Om6Fatty]
      ,[Om3Fatty]
      ,[Starch]
      ,[Chloride]
      ,[Chromium]
      ,[VitaminK]
      ,[Manganese]
      ,[Molybdenum]
      ,[Selenium]
      ,[ServingSizeDesc]
      ,[TransfatWeight]
      ,[HshRating]
      ,[Scale_LabelFormat_ID])
	output inserted.[Description], inserted.NutriFactsID, 0 into @NewNutrifacts
	
	select ii.Identifier
      ,[ServingsPerPortion]
      ,[SizeWeight]
      ,[Calories]
      ,[CaloriesFat]
      ,[CaloriesSaturatedFat]
      ,[ServingPerContainer]
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
      ,[CaloriesFromTransFat]
      ,[Om6Fatty]
      ,[Om3Fatty]
      ,[Starch]
      ,[Chloride]
      ,[Chromium]
      ,[VitaminK]
      ,[Manganese]
      ,[Molybdenum]
      ,[Selenium]
      ,[ServingSizeDesc]
      ,[TransfatWeight]
      ,[HshRating]
      ,0
	from #distinctList dl
	JOIN ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
									 		AND ii.Identifier = dl.Plu
											AND (dl.IsScaleItem = 0) --This includes all the non-sacle items, both 365 CFS items and non-CFS items
											and (not exists (select  1 from ItemNutrition inf where inf.ItemKey = ii.Item_Key)
													OR
												 exists (select  1 from ItemNutrition inf where inf.ItemKey = ii.Item_Key and inf.NutriFactsID is null)
												)	
		

	--Insert new rows for scale items
		
	insert into NutriFacts ([Description]
      ,[ServingsPerPortion]
      ,[SizeWeight]
      ,[Calories]
      ,[CaloriesFat]
      ,[CaloriesSaturatedFat]
      ,[ServingPerContainer]
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
      ,[CaloriesFromTransFat]
      ,[Om6Fatty]
      ,[Om3Fatty]
      ,[Starch]
      ,[Chloride]
      ,[Chromium]
      ,[VitaminK]
      ,[Manganese]
      ,[Molybdenum]
      ,[Selenium]
      ,[ServingSizeDesc]
      ,[TransfatWeight]
      ,[HshRating]
      ,[Scale_LabelFormat_ID])
	output inserted.[Description], inserted.NutriFactsID, 1 into @NewNutrifacts
	
	select ii.Identifier
      ,[ServingsPerPortion]
      ,[SizeWeight]
      ,[Calories]
      ,[CaloriesFat]
      ,[CaloriesSaturatedFat]
      ,[ServingPerContainer]
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
      ,[CaloriesFromTransFat]
      ,[Om6Fatty]
      ,[Om3Fatty]
      ,[Starch]
      ,[Chloride]
      ,[Chromium]
      ,[VitaminK]
      ,[Manganese]
      ,[Molybdenum]
      ,[Selenium]
      ,[ServingSizeDesc]
      ,[TransfatWeight]
      ,[HshRating]
      ,0
	from #distinctList dl
	JOIN ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
									 		AND ii.Identifier = dl.Plu
											AND (dl.IsScaleItem = 1) -- Only includes Scale Items
											and (	not exists (select  1 from ItemScale its where its.Item_Key = ii.Item_Key)
													OR
													exists (select  1 from ItemScale its where its.Item_Key = ii.Item_Key and its.Nutrifact_ID is null)
												)


	--Insert into ItemNutrition with newly added nutrifacts. This table should only contains non-scale items with nutrition.

	Insert into ItemNutrition (ItemKey, NutriFactsID)
	select ii.Item_Key, nnf.nutrifactsId
	from @NewNutrifacts nnf
	JOIN ItemIdentifier ii on nnf.plu = ii.Identifier  and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0
	where (nnf.isScalePlu = 0) and not exists (select 1 from ItemNutrition inf where inf.ItemKey = ii.Item_Key)

	--Update ItemNutrition with newly added nutrifacts
	update inf
	set inf.NutriFactsID = nnf.nutrifactsId
	from ItemNutrition inf
	JOIN ItemIdentifier ii on inf.ItemKey = ii.Item_Key and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0
	JOIN @NewNutrifacts nnf on ii.Identifier = nnf.plu and (nnf.isScalePlu = 0 OR dbo.fn_IsCustomerFacingScaleItem(nnf.plu) = 1)
	where inf.NutriFactsID is null

	-- Insert into Itemscale with newly added nutrifacts
	Insert into ItemScale (Item_Key, Nutrifact_ID)
	select ii.Item_Key, nnf.nutrifactsId
	from @NewNutrifacts nnf
	JOIN ItemIdentifier ii on ii.Identifier = nnf.plu and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0	
	where (nnf.isScalePlu = 1 OR dbo.fn_IsCustomerFacingScaleItem(nnf.plu) = 1) and  not exists (select 1 from ItemScale itsl where itsl.Item_Key = ii.Item_Key)

	--Update Itemscale with newly added nutrifacts
	Update itsl
	set itsl.Nutrifact_ID = nnf.nutrifactsId
	from ItemScale itsl
	JOIN ItemIdentifier ii on itsl.Item_Key = ii.Item_Key and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0
	JOIN @NewNutrifacts nnf on ii.Identifier = nnf.plu and (nnf.isScalePlu = 1 OR dbo.fn_IsCustomerFacingScaleItem(nnf.plu) = 1)
	where itsl.Nutrifact_ID is null

	--Update ItemScale when a non-scale, non-CFS item with nutrifact data is converted to a CFS item
	update itsl
	set itsl.Nutrifact_ID = inf.NutriFactsID
	from ItemScale itsl
	JOIN ItemNutrition inf on itsl.Item_Key = inf.ItemKey
	where itsl.Nutrifact_ID is null
	  and inf.NutriFactsID is not null
	
	--Update item attribute table checkbox 8 based on HSH value
	
	;merge
		ItemAttribute with (updlock, rowlock) ia
	using
		#distinctList dl 
		JOIN ItemIdentifier ii on ii.Identifier = dl.Plu
	on
		ia.Item_Key = ii.Item_Key
	when matched then
		update set
			ia.Check_Box_8 = (case when dl.HshRating > 0 then 1 else 0 end)
	when not matched then
		insert
			(
				Item_Key, 
				Check_Box_8				
			)
		values
			(
				ii.Item_Key,
				case when dl.HshRating > 0 then 1 else 0 end			
			);

	

-- SCALE INGREDIENTS------

-- Update ingredients if ingeredients key present in item scale table
update si
set si.Description = dl.Plu,
	si.Ingredients = dl.Ingredients
from Scale_Ingredient si
join ItemScale its on si.Scale_Ingredient_ID = its.Scale_Ingredient_ID
join ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
									 		AND ii.Item_Key = its.Item_Key
JOIN  #distinctList dl on ii.Identifier = dl.Plu and  (dl.IsScaleItem = 1 OR dl.IsCfsItem = 1)



-- Insert Scale ingredients
DECLARE @NewScaleIngredients table(plu varchar(50), Ingredient_ID int);

insert into Scale_Ingredient(Scale_LabelType_ID, Description, Ingredients)
output inserted.Description, inserted.Scale_Ingredient_ID into @NewScaleIngredients
select se.Scale_LabelType_ID, dl.plu, dl.Ingredients
from #distinctList dl
JOIN ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
									 		AND ii.Identifier = dl.Plu
											AND (dl.IsScaleItem = 1 AND dl.IsCfsItem = 0)
JOIN ItemScale its on its.Item_Key = ii.Item_Key and its.Scale_Ingredient_ID is null
left join Scale_ExtraText se on its.Scale_ExtraText_ID = se.Scale_ExtraText_ID

-- update item scale with new scale ingredients id 
update its
set  its.Scale_Ingredient_ID = nsi.Ingredient_ID
from ItemScale its
join ItemIdentifier		ii on	ii.Item_Key = its.Item_Key
JOIN  @NewScaleIngredients nsi on ii.Identifier = nsi.Plu


-- SCALE Allergens------

-- Update Allergens if ingeredients key present in item scale table
update sa
set sa.Description = dl.Plu,
	sa.Allergens = dl.Allergens
from Scale_Allergen sa
join ItemScale its on sa.Scale_Allergen_ID = its.Scale_Allergen_ID
join ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1											
									 		AND ii.Item_Key = its.Item_Key
JOIN  #distinctList dl on ii.Identifier = dl.Plu AND (dl.IsScaleItem = 1 OR dl.IsCfsItem = 1)


-- Insert Scale Allergens
DECLARE @NewScaleAllergens table(plu varchar(50), Allergen_ID int);

insert into Scale_Allergen (Scale_LabelType_ID, Description, Allergens)
output inserted.Description, inserted.Scale_Allergen_ID into @NewScaleAllergens
select se.Scale_LabelType_ID, dl.plu, dl.Allergens
from #distinctList dl
JOIN ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1											
									 		AND ii.Identifier = dl.Plu
											AND (dl.IsScaleItem = 1 AND dl.IsCfsItem = 0)
JOIN ItemScale its on its.Item_Key = ii.Item_Key and its.Scale_Allergen_ID is null
left join Scale_ExtraText se on its.Scale_ExtraText_ID = se.Scale_ExtraText_ID

-- update item scale with new scale Allergens id 
update its
set  its.Scale_Allergen_ID = nsa.Allergen_ID
from ItemScale its
join ItemIdentifier		ii on	ii.Item_Key = its.Item_Key
JOIN  @NewScaleAllergens nsa on ii.Identifier = nsa.Plu



-- NON_SCALE INGREDIENTS------

-- Update ingredients if ingeredients key present in item nutrifacts table
update si
set si.Description = dl.Plu,
	si.Ingredients = dl.Ingredients
from Scale_Ingredient si
join ItemNutrition itn on si.Scale_Ingredient_ID = itn.Scale_Ingredient_ID
join ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
									 		AND ii.Item_Key = itn.ItemKey
JOIN  #distinctList dl on ii.Identifier = dl.Plu AND (dl.IsScaleItem = 0 OR dl.IsCfsItem = 1)


-- Insert non-Scale ingredients

delete from @NewScaleIngredients

insert into Scale_Ingredient(Scale_LabelType_ID, Description, Ingredients)
output inserted.Description, inserted.Scale_Ingredient_ID into @NewScaleIngredients
select 0, dl.plu, dl.Ingredients
from #distinctList dl
JOIN ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
									 		AND ii.Identifier = dl.Plu
											AND (dl.IsScaleItem = 0 OR dl.IsCfsItem = 1)
JOIN ItemNutrition itn on itn.ItemKey = ii.Item_Key and itn.Scale_Ingredient_ID is null

-- update item nutrifact with new scale ingredients id 
update itn
set  itn.Scale_Ingredient_ID = nsi.Ingredient_ID
from ItemNutrition itn
join ItemIdentifier		ii on	ii.Item_Key = itn.ItemKey
JOIN  @NewScaleIngredients nsi on ii.Identifier = nsi.Plu


-- SCALE Allergens------

-- Update Allergens if ingeredients key present in item nutrifacts table
update sa
set sa.Description = dl.Plu,
	sa.Allergens = dl.Allergens
from Scale_Allergen sa
join ItemNutrition itn on sa.Scale_Allergen_ID = itn.Scale_Allergen_ID
join ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
									 		AND ii.Item_Key = itn.ItemKey
JOIN  #distinctList dl on ii.Identifier = dl.Plu AND (dl.IsScaleItem = 0 OR dl.IsCfsItem = 1)


-- Insert Scale Allergens
delete from @NewScaleAllergens

insert into Scale_Allergen (Scale_LabelType_ID, Description, Allergens)
output inserted.Description, inserted.Scale_Allergen_ID into @NewScaleAllergens
select 0, dl.plu, dl.Allergens
from #distinctList dl
JOIN ItemIdentifier		ii on	ii.Deleted_Identifier = 0
											AND ii.Default_Identifier = 1
									 		AND ii.Identifier = dl.Plu
											AND (dl.IsScaleItem = 0 OR dl.IsCfsItem = 1)
JOIN ItemNutrition itn on itn.ItemKey = ii.Item_Key and itn.Scale_Allergen_ID is null

-- update item scale with new scale Allergens id 
update itn
set  itn.Scale_Allergen_ID = nsa.Allergen_ID
from ItemNutrition itn
join ItemIdentifier		ii on	ii.Item_Key = itn.ItemKey
JOIN  @NewScaleAllergens nsa on ii.Identifier = nsa.Plu

--Update ItemScale when a non-scale, non-CFS item with infredients data is converted to a CFS item and 
update itsl
set itsl.Scale_Ingredient_ID = inf.Scale_Ingredient_ID
from ItemScale itsl
JOIN ItemNutrition inf on itsl.Item_Key = inf.ItemKey
where itsl.Scale_Ingredient_ID is null
  and inf.Scale_Ingredient_ID is not null

--Update ItemScale when a non-scale, non-CFS item with allergens data is converted to a CFS item
update itsl
set itsl.Scale_Allergen_ID = inf.Scale_Allergen_ID
from ItemScale itsl
JOIN ItemNutrition inf on itsl.Item_Key = inf.ItemKey
where itsl.Scale_Allergen_ID is null
  and inf.Scale_Allergen_ID is not null

	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('[UpdateNutriFactsFromIcon] failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateNutriFactsFromIcon] TO [IConInterface]
    AS [dbo];

