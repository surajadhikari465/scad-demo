using Dapper;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Interfaces;
using NutritionWebApi.DataAccess.Extensions;
using System.Data;
using System.Linq;

namespace NutritionWebApi.DataAccess.Commands
{
    public class AddOrUpdateNutritionItemCommandHandler : ICommandHandler<AddOrUpdateNutritionItemCommand>
    {
        private IDbConnectionProvider DbConnectionProvider { get; set; }

        public AddOrUpdateNutritionItemCommandHandler(IDbConnectionProvider DbProvider)
        {
            this.DbConnectionProvider = DbProvider;
        }

        /// <summary>
        /// returns exectuion results
        /// </summary>
        /// <param name="updateNutritionItemCommandData"></param>
        /// <returns></returns>
        public string Execute(AddOrUpdateNutritionItemCommand updateNutritionItemCommandData)
        {
            const string storedProcedure = "nutrition.AddOrUpdateNutritionItem";

            var itemListData = updateNutritionItemCommandData.NutritionItems.ConvertAll(item => new
            {
                Plu = item.Plu,
                RecipeName = item.RecipeName,
                Allergens = item.Allergens,
                Ingredients = item.Ingredients,
                ServingsPerPortion = item.ServingsPerPortion,
                ServingSizeDesc = item.ServingSizeDesc,
                ServingPerContainer = item.ServingPerContainer,
                HshRating = item.HshRating,
                ServingUnits = item.ServingUnits,
                SizeWeight = item.SizeWeight,
                Calories = item.Calories,
                CaloriesFat = item.CaloriesFat,
                CaloriesSaturatedFat = item.CaloriesSaturatedFat,
                TotalFatWeight = item.TotalFatWeight,
                TotalFatPercentage = item.TotalFatPercentage,
                SaturatedFatWeight = item.SaturatedFatWeight,
                SaturatedFatPercent = item.SaturatedFatPercent,
                PolyunsaturatedFat = item.PolyunsaturatedFat,
                MonounsaturatedFat = item.MonounsaturatedFat,
                CholesterolWeight = item.CholesterolWeight,
                CholesterolPercent = item.CholesterolPercent,
                SodiumWeight = item.SodiumWeight,
                SodiumPercent = item.SodiumPercent,
                PotassiumWeight = item.PotassiumWeight,
                PotassiumPercent = item.PotassiumPercent,
                TotalCarbohydrateWeight = item.TotalCarbohydrateWeight,
                TotalCarbohydratePercent = item.TotalCarbohydratePercent,
                DietaryFiberWeight = item.DietaryFiberWeight,
                DietaryFiberPercent = item.DietaryFiberPercent,
                SolubleFiber = item.SolubleFiber,
                InsolubleFiber = item.InsolubleFiber,
                Sugar = item.Sugar,
                SugarAlcohol = item.SugarAlcohol,
                OtherCarbohydrates = item.OtherCarbohydrates,
                ProteinWeight = item.ProteinWeight,
                ProteinPercent = item.ProteinPercent,
                VitaminA = item.VitaminA,
                Betacarotene = item.Betacarotene,
                VitaminC = item.VitaminC,
                Calcium = item.Calcium,
                Iron = item.Iron,
                VitaminD = item.VitaminD,
                VitaminE = item.VitaminE,
                Thiamin = item.Thiamin,
                Riboflavin = item.Riboflavin,
                Niacin = item.Niacin,
                VitaminB6 = item.VitaminB6,
                Folate = item.Folate,
                VitaminB12 = item.VitaminB12,
                Biotin = item.Biotin,
                PantothenicAcid = item.PantothenicAcid,
                Phosphorous = item.Phosphorous,
                Iodine = item.Iodine,
                Magnesium = item.Magnesium,
                Zinc = item.Zinc,
                Copper = item.Copper,
                Transfat = item.Transfat,
                CaloriesFromTransfat = item.CaloriesFromTransfat,
                Om6Fatty = item.Om6Fatty,
                Om3Fatty = item.Om3Fatty,
                Starch = item.Starch,
                Chloride = item.Chloride,
                Chromium = item.Chromium,
                VitaminK = item.VitaminK,
                Manganese = item.Manganese,
                Molybdenum = item.Molybdenum,
                Selenium = item.Selenium,
                TransfatWeight = item.TransfatWeight,
                AddedSugarsWeight = item.AddedSugarsWeight,
                AddedSugarsPercent = item.AddedSugarsPercent,
                CalciumWeight = item.CalciumWeight,
                IronWeight = item.IronWeight,
                VitaminDWeight = item.VitaminDWeight,
                ProfitCenter = item.ProfCenterID,
                CanadaAllergens = item.FrenchAllergens,
                CanadaIngredients = item.FrenchIngredients,
                CanadaSugarPercent = item.SugerPercent
            });

            string result = this.DbConnectionProvider.Connection.Query<string>(
                storedProcedure,
                new { @NutritionItem = itemListData.ToDataTable() },
                commandType: CommandType.StoredProcedure).Single();

            return result;
        }
    }
}