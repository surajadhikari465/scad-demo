﻿using System;

namespace Icon.Services.ItemPublisher.Repositories.Entities
{
    public class Nutrition
    {
        public int RecipeId { get; set; }
        public string Plu { get; set; }
        public string RecipeName { get; set; }
        public string Allergens { get; set; }
        public string Ingredients { get; set; }
        public double? ServingsPerPortion { get; set; }
        public string ServingSizeDesc { get; set; }
        public string ServingPerContainer { get; set; }
        public int? HshRating { get; set; }
        public short? ServingUnits { get; set; }
        public int? SizeWeight { get; set; }
        public int? Calories { get; set; }
        public int? CaloriesFat { get; set; }
        public int? CaloriesSaturatedFat { get; set; }
        public decimal? TotalFatWeight { get; set; }
        public short? TotalFatPercentage { get; set; }
        public decimal? SaturatedFatWeight { get; set; }
        public short? SaturatedFatPercent { get; set; }
        public decimal? PolyunsaturatedFat { get; set; }
        public decimal? MonounsaturatedFat { get; set; }
        public decimal? CholesterolWeight { get; set; }
        public short? CholesterolPercent { get; set; }
        public decimal? SodiumWeight { get; set; }
        public short? SodiumPercent { get; set; }
        public decimal? PotassiumWeight { get; set; }
        public short? PotassiumPercent { get; set; }
        public decimal? TotalCarbohydrateWeight { get; set; }
        public short? TotalCarbohydratePercent { get; set; }
        public decimal? DietaryFiberWeight { get; set; }
        public short? DietaryFiberPercent { get; set; }
        public decimal? SolubleFiber { get; set; }
        public decimal? InsolubleFiber { get; set; }
        public decimal? Sugar { get; set; }
        public decimal? SugarAlcohol { get; set; }
        public decimal? OtherCarbohydrates { get; set; }
        public decimal? ProteinWeight { get; set; }
        public short? ProteinPercent { get; set; }
        public short? VitaminA { get; set; }
        public short? Betacarotene { get; set; }
        public short? VitaminC { get; set; }
        public short? Calcium { get; set; }
        public short? Iron { get; set; }
        public short? VitaminD { get; set; }
        public short? VitaminE { get; set; }
        public short? Thiamin { get; set; }
        public short? Riboflavin { get; set; }
        public short? Niacin { get; set; }
        public short? VitaminB6 { get; set; }
        public short? Folate { get; set; }
        public short? VitaminB12 { get; set; }
        public short? Biotin { get; set; }
        public short? PantothenicAcid { get; set; }
        public short? Phosphorous { get; set; }
        public short? Iodine { get; set; }
        public short? Magnesium { get; set; }
        public short? Zinc { get; set; }
        public short? Copper { get; set; }
        public decimal? Transfat { get; set; }
        public int? CaloriesFromTransfat { get; set; }
        public decimal? Om6Fatty { get; set; }
        public decimal? Om3Fatty { get; set; }
        public decimal? Starch { get; set; }
        public short? Chloride { get; set; }
        public short? Chromium { get; set; }
        public short? VitaminK { get; set; }
        public short? Manganese { get; set; }
        public short? Molybdenum { get; set; }
        public short? Selenium { get; set; }
        public decimal? TransfatWeight { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? AddedSugarsWeight { get; set; }
        public short? AddedSugarsPercent { get; set; }
        public decimal? CalciumWeight { get; set; }
        public decimal? IronWeight { get; set; }
        public decimal? VitaminDWeight { get; set; }
        public bool IsDeleted { get; set; }
    }
}