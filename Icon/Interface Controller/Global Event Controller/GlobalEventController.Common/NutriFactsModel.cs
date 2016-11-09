using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Framework;

namespace GlobalEventController.Common
{
    public class NutriFactsModel
    {
        public string Plu { get; set; }
        public string RecipeName { get; set; }
        public string Allergens { get; set; }
        public string Ingredients { get; set; }
        public Nullable<double> ServingsPerPortion { get; set; }
        public string ServingSizeDesc { get; set; }
        public string ServingPerContainer { get; set; }
        public Nullable<int> HshRating { get; set; }
        public Nullable<byte> ServingUnits { get; set; }
        public Nullable<int> SizeWeight { get; set; }
        public Nullable<int> Calories { get; set; }
        public Nullable<int> CaloriesFat { get; set; }
        public Nullable<int> CaloriesSaturatedFat { get; set; }
        public Nullable<decimal> TotalFatWeight { get; set; }
        public Nullable<short> TotalFatPercentage { get; set; }
        public Nullable<decimal> SaturatedFatWeight { get; set; }
        public Nullable<short> SaturatedFatPercent { get; set; }
        public Nullable<decimal> PolyunsaturatedFat { get; set; }
        public Nullable<decimal> MonounsaturatedFat { get; set; }
        public Nullable<decimal> CholesterolWeight { get; set; }
        public Nullable<short> CholesterolPercent { get; set; }
        public Nullable<decimal> SodiumWeight { get; set; }
        public Nullable<short> SodiumPercent { get; set; }
        public Nullable<decimal> PotassiumWeight { get; set; }
        public Nullable<short> PotassiumPercent { get; set; }
        public Nullable<decimal> TotalCarbohydrateWeight { get; set; }
        public Nullable<short> TotalCarbohydratePercent { get; set; }
        public Nullable<decimal> DietaryFiberWeight { get; set; }
        public Nullable<short> DietaryFiberPercent { get; set; }
        public Nullable<decimal> SolubleFiber { get; set; }
        public Nullable<decimal> InsolubleFiber { get; set; }
        public Nullable<decimal> Sugar { get; set; }
        public Nullable<decimal> SugarAlcohol { get; set; }
        public Nullable<decimal> OtherCarbohydrates { get; set; }
        public Nullable<decimal> ProteinWeight { get; set; }
        public Nullable<short> ProteinPercent { get; set; }
        public Nullable<short> VitaminA { get; set; }
        public Nullable<short> Betacarotene { get; set; }
        public Nullable<short> VitaminC { get; set; }
        public Nullable<short> Calcium { get; set; }
        public Nullable<short> Iron { get; set; }
        public Nullable<short> VitaminD { get; set; }
        public Nullable<short> VitaminE { get; set; }
        public Nullable<short> Thiamin { get; set; }
        public Nullable<short> Riboflavin { get; set; }
        public Nullable<short> Niacin { get; set; }
        public Nullable<short> VitaminB6 { get; set; }
        public Nullable<short> Folate { get; set; }
        public Nullable<short> VitaminB12 { get; set; }
        public Nullable<short> Biotin { get; set; }
        public Nullable<short> PantothenicAcid { get; set; }
        public Nullable<short> Phosphorous { get; set; }
        public Nullable<short> Iodine { get; set; }
        public Nullable<short> Magnesium { get; set; }
        public Nullable<short> Zinc { get; set; }
        public Nullable<short> Copper { get; set; }
        public Nullable<decimal> Transfat { get; set; }
        public Nullable<int> CaloriesFromTransfat { get; set; }
        public Nullable<decimal> Om6Fatty { get; set; }
        public Nullable<decimal> Om3Fatty { get; set; }
        public Nullable<decimal> Starch { get; set; }
        public Nullable<short> Chloride { get; set; }
        public Nullable<short> Chromium { get; set; }
        public Nullable<short> VitaminK { get; set; }
        public Nullable<short> Manganese { get; set; }
        public Nullable<short> Molybdenum { get; set; }
        public Nullable<short> Selenium { get; set; }
        public Nullable<decimal> TransfatWeight { get; set; }

        public NutriFactsModel()
        { }

        public NutriFactsModel (ItemNutrition itemNutrition)
        {           
            Plu = itemNutrition.Plu;
            RecipeName = itemNutrition.RecipeName;
            Allergens = itemNutrition.Allergens;
            Ingredients = itemNutrition.Ingredients;
            ServingsPerPortion = itemNutrition.ServingsPerPortion;
            ServingSizeDesc = itemNutrition.ServingSizeDesc;
            ServingPerContainer = itemNutrition.ServingPerContainer;
            HshRating = itemNutrition.HshRating;
            ServingUnits = itemNutrition.ServingUnits;
            SizeWeight = itemNutrition.SizeWeight;
            Calories = itemNutrition.Calories;
            CaloriesFat = itemNutrition.CaloriesFat;
            CaloriesSaturatedFat = itemNutrition.CaloriesSaturatedFat;
            TotalFatWeight = itemNutrition.TotalFatWeight;
            TotalFatPercentage = itemNutrition.TotalFatPercentage;
            SaturatedFatWeight = itemNutrition.SaturatedFatWeight;
            SaturatedFatPercent = itemNutrition.SaturatedFatPercent;
            PolyunsaturatedFat = itemNutrition.PolyunsaturatedFat;
            MonounsaturatedFat = itemNutrition.MonounsaturatedFat;
            CholesterolWeight = itemNutrition.CholesterolWeight;
            CholesterolPercent = itemNutrition.CholesterolPercent;
            SodiumWeight = itemNutrition.SodiumWeight;
            SodiumPercent = itemNutrition.SodiumPercent;
            PotassiumWeight = itemNutrition.PotassiumWeight;
            PotassiumPercent = itemNutrition.PotassiumPercent;
            TotalCarbohydrateWeight = itemNutrition.TotalCarbohydrateWeight;
            TotalCarbohydratePercent = itemNutrition.TotalCarbohydratePercent;
            DietaryFiberWeight = itemNutrition.DietaryFiberWeight;
            DietaryFiberPercent = itemNutrition.DietaryFiberPercent;
            SolubleFiber = itemNutrition.SolubleFiber;
            InsolubleFiber = itemNutrition.InsolubleFiber;
            Sugar = itemNutrition.Sugar;
            SugarAlcohol = itemNutrition.SugarAlcohol;
            OtherCarbohydrates = itemNutrition.OtherCarbohydrates;
            ProteinWeight = itemNutrition.ProteinWeight;
            ProteinPercent = itemNutrition.ProteinPercent;
            VitaminA = itemNutrition.VitaminA;
            Betacarotene = itemNutrition.Betacarotene;
            VitaminC = itemNutrition.VitaminC;
            Calcium = itemNutrition.Calcium;
            Iron = itemNutrition.Iron;
            VitaminD = itemNutrition.VitaminD;
            VitaminE = itemNutrition.VitaminE;
            Thiamin = itemNutrition.Thiamin;
            Riboflavin = itemNutrition.Riboflavin;
            Niacin = itemNutrition.Niacin;
            VitaminB6 = itemNutrition.VitaminB6;
            Folate = itemNutrition.Folate;
            VitaminB12 = itemNutrition.VitaminB12;
            Biotin = itemNutrition.Biotin;
            PantothenicAcid = itemNutrition.PantothenicAcid;
            Phosphorous = itemNutrition.Phosphorous;
            Iodine = itemNutrition.Iodine;
            Magnesium = itemNutrition.Magnesium;
            Zinc = itemNutrition.Zinc;
            Copper = itemNutrition.Copper;
            Transfat = itemNutrition.Transfat;
            CaloriesFromTransfat = itemNutrition.CaloriesFromTransfat;
            Om6Fatty = itemNutrition.Om6Fatty;
            Om3Fatty = itemNutrition.Om3Fatty;
            Starch = itemNutrition.Starch;
            Chloride = itemNutrition.Chloride;
            Chromium = itemNutrition.Chromium;
            VitaminK = itemNutrition.VitaminK;
            Manganese = itemNutrition.Manganese;
            Molybdenum = itemNutrition.Molybdenum;
            Selenium = itemNutrition.Selenium;
            TransfatWeight = itemNutrition.TransfatWeight;
        }
    }
}
