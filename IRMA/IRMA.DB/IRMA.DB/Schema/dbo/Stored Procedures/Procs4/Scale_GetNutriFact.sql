
-- Change History
-- Date			Initial		TFS		Comments
-- 07/02/2015	DN			16245	Renamed Columns:
--									Size			-> ServingsPerPortion
--									Size Text		-> ServingSizeDesc
--									PerContainer	-> ServingPerContainer


CREATE PROCEDURE dbo.Scale_GetNutriFact 
	@NutriFactsID as int
AS
	
BEGIN


	SELECT 
		Scale_LabelFormat_ID, 
		ServingUnits, 
		Description, 
		[ServingsPerPortion], 
		SizeWeight, 
		Calories, 
		CaloriesFat, 
		CaloriesSaturatedFat, 
		ServingPerContainer, 
		TotalFatWeight, 
		TotalFatPercentage, 
		SaturatedFatWeight, 
		SaturatedFatPercent, 
		PolyunsaturatedFat, 
		MonounsaturatedFat, 
		CholesterolWeight, 
		CholesterolPercent, 
		SodiumWeight, 
		SodiumPercent, 
		PotassiumWeight, 
		PotassiumPercent, 
		TotalCarbohydrateWeight, 
		TotalCarbohydratePercent, 
		DietaryFiberWeight, 
		DietaryFiberPercent, 
		SolubleFiber, 
		InsolubleFiber, 
		Sugar, 
		SugarAlcohol, 
		OtherCarbohydrates, 
		ProteinWeight, 
		ProteinPercent, 
		VitaminA, 
		Betacarotene, 
		VitaminC, 
		Calcium, 
		Iron, 
		VitaminD, 
		VitaminE, 
		Thiamin, 
		Riboflavin, 
		Niacin, 
		VitaminB6, 
		Folate, 
		VitaminB12, 
		Biotin, 
		PantothenicAcid, 
		Phosphorous, 
		Iodine, 
		Magnesium, 
		Zinc, 
		Copper, 
		Transfat, 
		TransfatWeight,
		CaloriesFromTransFat, 
		Om6Fatty, 
		Om3Fatty, 
		Starch, 
		Chloride, 
		Chromium, 
		VitaminK, 
		Manganese, 
		Molybdenum, 
		Selenium ,
        ServingSizeDesc
	FROM 
		NutriFacts
	WHERE
		NutriFactsID = @NutriFactsID
    
END




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetNutriFact] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetNutriFact] TO [IRMAClientRole]
    AS [dbo];

