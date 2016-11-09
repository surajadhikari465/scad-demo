using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutritionWebApi.Common.Models
{
    public class NutritionItemModel
    {
        public int RecipeId { get; set; }
        public string Plu { get; set; }
        public string RecipeName { get; set; }
        public string Allergens { get; set; }
        public string Ingredients { get; set; }
        public float? ServingsPerPortion { get; set; }
        public string ServingSizeDesc { get; set; }
        public string ServingPerContainer { get; set; }
        public int? HshRating { get; set; }
        public int? ServingUnits { get; set; }
        public int? SizeWeight { get; set; }
        public int? Calories { get; set; }
        public int? CaloriesFat { get; set; }
        public int? CaloriesSaturatedFat { get; set; }
        public Decimal? TotalFatWeight { get; set; }
        public int? TotalFatPercentage { get; set; }
        public Decimal? SaturatedFatWeight { get; set; }
        public int? SaturatedFatPercent { get; set; }
        public Decimal? PolyunsaturatedFat { get; set; }
        public Decimal? MonounsaturatedFat { get; set; }
        public Decimal? CholesterolWeight { get; set; }
        public int? CholesterolPercent { get; set; }
        public Decimal? SodiumWeight { get; set; }
        public int? SodiumPercent { get; set; }
        public Decimal? PotassiumWeight { get; set; }
        public int? PotassiumPercent { get; set; }
        public Decimal? TotalCarbohydrateWeight { get; set; }
        public int? TotalCarbohydratePercent { get; set; }
        public Decimal? DietaryFiberWeight { get; set; }
        public int? DietaryFiberPercent { get; set; }
        public Decimal? SolubleFiber { get; set; }
        public Decimal? InsolubleFiber { get; set; }
        public Decimal? Sugar { get; set; }
        public Decimal? SugarAlcohol { get; set; }
        public Decimal? OtherCarbohydrates { get; set; }
        public Decimal? ProteinWeight { get; set; }
        public int? ProteinPercent { get; set; }
        public int? VitaminA { get; set; }
        public int? Betacarotene { get; set; }
        public int? VitaminC { get; set; }
        public int? Calcium { get; set; }
        public int? Iron { get; set; }
        public int? VitaminD { get; set; }
        public int? VitaminE { get; set; }
        public int? Thiamin { get; set; }
        public int? Riboflavin { get; set; }
        public int? Niacin { get; set; }
        public int? VitaminB6 { get; set; }
        public int? Folate { get; set; }
        public int? VitaminB12 { get; set; }
        public int? Biotin { get; set; }
        public int? PantothenicAcid { get; set; }
        public int? Phosphorous { get; set; }
        public int? Iodine { get; set; }
        public int? Magnesium { get; set; }
        public int? Zinc { get; set; }
        public int? Copper { get; set; }
        public Decimal? Transfat { get; set; }
        public int? CaloriesFromTransfat { get; set; }
        public Decimal? Om6Fatty { get; set; }
        public Decimal? Om3Fatty { get; set; }
        public Decimal? Starch { get; set; }
        public int? Chloride { get; set; }
        public int? Chromium { get; set; }
        public int? VitaminK { get; set; }
        public int? Manganese { get; set; }
        public int? Molybdenum { get; set; }
        public int? Selenium { get; set; }
        public Decimal? TransfatWeight { get; set; }
    }
}