using NutritionWebApi.Common.Models;

namespace NutritionWebApi.Tests.Common.Builders
{
    public class NutritionItemModelBuilder
    {
        private int RecipeId;
        private string Plu;
        private string RecipeName;
        private string Allergens;
        private string Ingredients;
        private float? ServingsPerPortion;
        private string ServingSizeDesc;
        private string ServingPerContainer;
        private int? HshRating;
        private int? ServingUnits;
        private int? SizeWeight;
        private int? Calories;
        private int? CaloriesFat;
        private int? CaloriesSaturatedFat;
        private decimal? TotalFatWeight;
        private int? TotalFatPercentage;
        private decimal? SaturatedFatWeight;
        private int? SaturatedFatPercent;
        private decimal? PolyunsaturatedFat;
        private decimal? MonounsaturatedFat;
        private decimal? CholesterolWeight;
        private int? CholesterolPercent;
        private decimal? SodiumWeight;
        private int? SodiumPercent;
        private decimal? PotassiumWeight;
        private int? PotassiumPercent;
        private decimal? TotalCarbohydrateWeight;
        private int? TotalCarbohydratePercent;
        private decimal? DietaryFiberWeight;
        private int? DietaryFiberPercent;
        private decimal? SolubleFiber;
        private decimal? InsolubleFiber;
        private decimal? Sugar;
        private decimal? SugarAlcohol;
        private decimal? OtherCarbohydrates;
        private decimal? ProteinWeight;
        private int? ProteinPercent;
        private int? VitaminA;
        private int? Betacarotene;
        private int? VitaminC;
        private int? Calcium;
        private int? Iron;
        private int? VitaminD;
        private int? VitaminE;
        private int? Thiamin;
        private int? Riboflavin;
        private int? Niacin;
        private int? VitaminB6;
        private int? Folate;
        private int? VitaminB12;
        private int? Biotin;
        private int? PantothenicAcid;
        private int? Phosphorous;
        private int? Iodine;
        private int? Magnesium;
        private int? Zinc;
        private int? Copper;
        private decimal? Transfat;
        private int? CaloriesFromTransfat;
        private decimal? Om6Fatty;
        private decimal? Om3Fatty;
        private decimal? Starch;
        private int? Chloride;
        private int? Chromium;
        private int? VitaminK;
        private int? Manganese;
        private int? Molybdenum;
        private int? Selenium;
        private decimal? TransfatWeight;
        private decimal? AddedSugarsWeight;
        private short? AddedSugarsPercent;
        private decimal? CalciumWeight;
        private decimal? IronWeight;
        private decimal? VitaminDWeight;
        private string FrenchAllergens;
        private string FrenchIngredients;
        private short? SugarPercent;
        private int? ProfCenterID;
        private string FrenchServingSizeDesc;

        public NutritionItemModelBuilder()
        {
            RecipeId = -1;
        }

        public NutritionItemModelBuilder WithRecipeId(int value) { this.RecipeId = value; return this; }
        public NutritionItemModelBuilder WithPlu(string value) { this.Plu = value; return this; }
        public NutritionItemModelBuilder WithRecipeName(string value) { this.RecipeName = value; return this; }
        public NutritionItemModelBuilder WithAllergens(string value) { this.Allergens = value; return this; }
        public NutritionItemModelBuilder WithIngredients(string value) { this.Ingredients = value; return this; }
        public NutritionItemModelBuilder WithServingsPerPortion(float? value) { this.ServingsPerPortion = value; return this; }
        public NutritionItemModelBuilder WithServingSizeDesc(string value) { this.ServingSizeDesc = value; return this; }
        public NutritionItemModelBuilder WithServingPerContainer(string value) { this.ServingPerContainer = value; return this; }
        public NutritionItemModelBuilder WithHshRating(int? value) { this.HshRating = value; return this; }
        public NutritionItemModelBuilder WithServingUnits(int? value) { this.ServingUnits = value; return this; }
        public NutritionItemModelBuilder WithSizeWeight(int? value) { this.SizeWeight = value; return this; }
        public NutritionItemModelBuilder WithCalories(int? value) { this.Calories = value; return this; }
        public NutritionItemModelBuilder WithCaloriesFat(int? value) { this.CaloriesFat = value; return this; }
        public NutritionItemModelBuilder WithCaloriesSaturatedFat(int? value) { this.CaloriesSaturatedFat = value; return this; }
        public NutritionItemModelBuilder WithTotalFatWeight(decimal? value) { this.TotalFatWeight = value; return this; }
        public NutritionItemModelBuilder WithTotalFatPercentage(int? value) { this.TotalFatPercentage = value; return this; }
        public NutritionItemModelBuilder WithSaturatedFatWeight(decimal? value) { this.SaturatedFatWeight = value; return this; }
        public NutritionItemModelBuilder WithSaturatedFatPercent(int? value) { this.SaturatedFatPercent = value; return this; }
        public NutritionItemModelBuilder WithPolyunsaturatedFat(decimal? value) { this.PolyunsaturatedFat = value; return this; }
        public NutritionItemModelBuilder WithMonounsaturatedFat(decimal? value) { this.MonounsaturatedFat = value; return this; }
        public NutritionItemModelBuilder WithCholesterolWeight(decimal? value) { this.CholesterolWeight = value; return this; }
        public NutritionItemModelBuilder WithCholesterolPercent(int? value) { this.CholesterolPercent = value; return this; }
        public NutritionItemModelBuilder WithSodiumWeight(decimal? value) { this.SodiumWeight = value; return this; }
        public NutritionItemModelBuilder WithSodiumPercent(int? value) { this.SodiumPercent = value; return this; }
        public NutritionItemModelBuilder WithPotassiumWeight(decimal? value) { this.PotassiumWeight = value; return this; }
        public NutritionItemModelBuilder WithPotassiumPercent(int? value) { this.PotassiumPercent = value; return this; }
        public NutritionItemModelBuilder WithTotalCarbohydrateWeight(decimal? value) { this.TotalCarbohydrateWeight = value; return this; }
        public NutritionItemModelBuilder WithTotalCarbohydratePercent(int? value) { this.TotalCarbohydratePercent = value; return this; }
        public NutritionItemModelBuilder WithDietaryFiberWeight(decimal? value) { this.DietaryFiberWeight = value; return this; }
        public NutritionItemModelBuilder WithDietaryFiberPercent(int? value) { this.DietaryFiberPercent = value; return this; }
        public NutritionItemModelBuilder WithSolubleFiber(decimal? value) { this.SolubleFiber = value; return this; }
        public NutritionItemModelBuilder WithInsolubleFiber(decimal? value) { this.InsolubleFiber = value; return this; }
        public NutritionItemModelBuilder WithSugar(decimal? value) { this.Sugar = value; return this; }
        public NutritionItemModelBuilder WithSugarAlcohol(decimal? value) { this.SugarAlcohol = value; return this; }
        public NutritionItemModelBuilder WithOtherCarbohydrates(decimal? value) { this.OtherCarbohydrates = value; return this; }
        public NutritionItemModelBuilder WithProteinWeight(decimal? value) { this.ProteinWeight = value; return this; }
        public NutritionItemModelBuilder WithProteinPercent(int? value) { this.ProteinPercent = value; return this; }
        public NutritionItemModelBuilder WithVitaminA(int? value) { this.VitaminA = value; return this; }
        public NutritionItemModelBuilder WithBetacarotene(int? value) { this.Betacarotene = value; return this; }
        public NutritionItemModelBuilder WithVitaminC(int? value) { this.VitaminC = value; return this; }
        public NutritionItemModelBuilder WithCalcium(int? value) { this.Calcium = value; return this; }
        public NutritionItemModelBuilder WithIron(int? value) { this.Iron = value; return this; }
        public NutritionItemModelBuilder WithVitaminD(int? value) { this.VitaminD = value; return this; }
        public NutritionItemModelBuilder WithVitaminE(int? value) { this.VitaminE = value; return this; }
        public NutritionItemModelBuilder WithThiamin(int? value) { this.Thiamin = value; return this; }
        public NutritionItemModelBuilder WithRiboflavin(int? value) { this.Riboflavin = value; return this; }
        public NutritionItemModelBuilder WithNiacin(int? value) { this.Niacin = value; return this; }
        public NutritionItemModelBuilder WithVitaminB6(int? value) { this.VitaminB6 = value; return this; }
        public NutritionItemModelBuilder WithFolate(int? value) { this.Folate = value; return this; }
        public NutritionItemModelBuilder WithVitaminB12(int? value) { this.VitaminB12 = value; return this; }
        public NutritionItemModelBuilder WithBiotin(int? value) { this.Biotin = value; return this; }
        public NutritionItemModelBuilder WithPantothenicAcid(int? value) { this.PantothenicAcid = value; return this; }
        public NutritionItemModelBuilder WithPhosphorous(int? value) { this.Phosphorous = value; return this; }
        public NutritionItemModelBuilder WithIodine(int? value) { this.Iodine = value; return this; }
        public NutritionItemModelBuilder WithMagnesium(int? value) { this.Magnesium = value; return this; }
        public NutritionItemModelBuilder WithZinc(int? value) { this.Zinc = value; return this; }
        public NutritionItemModelBuilder WithCopper(int? value) { this.Copper = value; return this; }
        public NutritionItemModelBuilder WithTransfat(decimal? value) { this.Transfat = value; return this; }
        public NutritionItemModelBuilder WithCaloriesFromTransfat(int? value) { this.CaloriesFromTransfat = value; return this; }
        public NutritionItemModelBuilder WithOm6Fatty(decimal? value) { this.Om6Fatty = value; return this; }
        public NutritionItemModelBuilder WithOm3Fatty(decimal? value) { this.Om3Fatty = value; return this; }
        public NutritionItemModelBuilder WithStarch(decimal? value) { this.Starch = value; return this; }
        public NutritionItemModelBuilder WithChloride(int? value) { this.Chloride = value; return this; }
        public NutritionItemModelBuilder WithChromium(int? value) { this.Chromium = value; return this; }
        public NutritionItemModelBuilder WithVitaminK(int? value) { this.VitaminK = value; return this; }
        public NutritionItemModelBuilder WithManganese(int? value) { this.Manganese = value; return this; }
        public NutritionItemModelBuilder WithMolybdenum(int? value) { this.Molybdenum = value; return this; }
        public NutritionItemModelBuilder WithSelenium(int? value) { this.Selenium = value; return this; }
        public NutritionItemModelBuilder WithTransfatWeight(decimal? value) { this.TransfatWeight = value; return this; }
        public NutritionItemModelBuilder WithAddedSugarsWeight(decimal? value) { this.AddedSugarsWeight = value; return this; }
        public NutritionItemModelBuilder WithAddedSugarsPercent(short? value) { this.AddedSugarsPercent = value; return this; }
        public NutritionItemModelBuilder WithCalciumWeight(decimal? value) { this.CalciumWeight = value; return this; }
        public NutritionItemModelBuilder WithIronWeight(decimal? value) { this.IronWeight = value; return this; }
        public NutritionItemModelBuilder WithVitaminDWeight(decimal? value) { this.VitaminDWeight = value; return this; }
        public NutritionItemModelBuilder WithCanadaAllergens(string value) { this.FrenchAllergens = value; return this; }
        public NutritionItemModelBuilder WithCanadaIngredients(string value) { this.FrenchIngredients = value; return this; }
        public NutritionItemModelBuilder WithProfitCenter(int? value) { this.ProfCenterID = value; return this; }
        public NutritionItemModelBuilder WithCanadaSugarPercent(short? value) { this.SugarPercent = value; return this; }
        public NutritionItemModelBuilder WithFrenchServingSizeDesc(string value) { this.FrenchServingSizeDesc = value; return this; }
        
        private NutritionItemModel Build()
        {
            NutritionItemModel nutritionItemModel = new NutritionItemModel()
            {
                RecipeId = this.RecipeId,
                Plu = this.Plu,
                RecipeName = this.RecipeName,
                Allergens = this.Allergens,
                Ingredients = this.Ingredients,
                ServingsPerPortion = this.ServingsPerPortion,
                ServingSizeDesc = this.ServingSizeDesc,
                ServingPerContainer = this.ServingPerContainer,
                HshRating = this.HshRating,
                ServingUnits = this.ServingUnits,
                SizeWeight = this.SizeWeight,
                Calories = this.Calories,
                CaloriesFat = this.CaloriesFat,
                CaloriesSaturatedFat = this.CaloriesSaturatedFat,
                TotalFatWeight = this.TotalFatWeight,
                TotalFatPercentage = this.TotalFatPercentage,
                SaturatedFatWeight = this.SaturatedFatWeight,
                SaturatedFatPercent = this.SaturatedFatPercent,
                PolyunsaturatedFat = this.PolyunsaturatedFat,
                MonounsaturatedFat = this.MonounsaturatedFat,
                CholesterolWeight = this.CholesterolWeight,
                CholesterolPercent = this.CholesterolPercent,
                SodiumWeight = this.SodiumWeight,
                SodiumPercent = this.SodiumPercent,
                PotassiumWeight = this.PotassiumWeight,
                PotassiumPercent = this.PotassiumPercent,
                TotalCarbohydrateWeight = this.TotalCarbohydrateWeight,
                TotalCarbohydratePercent = this.TotalCarbohydratePercent,
                DietaryFiberWeight = this.DietaryFiberWeight,
                DietaryFiberPercent = this.DietaryFiberPercent,
                SolubleFiber = this.SolubleFiber,
                InsolubleFiber = this.InsolubleFiber,
                Sugar = this.Sugar,
                SugarAlcohol = this.SugarAlcohol,
                OtherCarbohydrates = this.OtherCarbohydrates,
                ProteinWeight = this.ProteinWeight,
                ProteinPercent = this.ProteinPercent,
                VitaminA = this.VitaminA,
                Betacarotene = this.Betacarotene,
                VitaminC = this.VitaminC,
                Calcium = this.Calcium,
                Iron = this.Iron,
                VitaminD = this.VitaminD,
                VitaminE = this.VitaminE,
                Thiamin = this.Thiamin,
                Riboflavin = this.Riboflavin,
                Niacin = this.Niacin,
                VitaminB6 = this.VitaminB6,
                Folate = this.Folate,
                VitaminB12 = this.VitaminB12,
                Biotin = this.Biotin,
                PantothenicAcid = this.PantothenicAcid,
                Phosphorous = this.Phosphorous,
                Iodine = this.Iodine,
                Magnesium = this.Magnesium,
                Zinc = this.Zinc,
                Copper = this.Copper,
                Transfat = this.Transfat,
                CaloriesFromTransfat = this.CaloriesFromTransfat,
                Om6Fatty = this.Om6Fatty,
                Om3Fatty = this.Om3Fatty,
                Starch = this.Starch,
                Chloride = this.Chloride,
                Chromium = this.Chromium,
                VitaminK = this.VitaminK,
                Manganese = this.Manganese,
                Molybdenum = this.Molybdenum,
                Selenium = this.Selenium,
                TransfatWeight = this.TransfatWeight,
                AddedSugarsWeight = this.AddedSugarsWeight,
                AddedSugarsPercent = this.AddedSugarsPercent,
                CalciumWeight = this.CalciumWeight,
                IronWeight = this.IronWeight,
                VitaminDWeight = this.VitaminDWeight,
                ProfCenterID = this.ProfCenterID,
                FrenchAllergens = this.FrenchAllergens,
                FrenchIngredients = this.FrenchIngredients,
                SugarPercent = this.SugarPercent,
                FrenchServingSizeDesc = this.FrenchServingSizeDesc
            };

            return nutritionItemModel;
        }

        public static implicit operator NutritionItemModel(NutritionItemModelBuilder builder)
        {
            return builder.Build();
        }
    }
}