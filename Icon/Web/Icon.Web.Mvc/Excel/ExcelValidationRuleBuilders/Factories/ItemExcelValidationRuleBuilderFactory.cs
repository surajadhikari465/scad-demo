using Icon.Framework;
using Icon.Web.Mvc.Excel.Columns;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;
using static Icon.Web.Mvc.Excel.ExcelHelper;

namespace Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders.Factories
{
    public class ItemExcelValidationRuleBuilderFactory : IExcelValidationRuleBuilderFactory<ItemExcelModel>
    {
        public IEnumerable<IExcelValidationRuleBuilder> CreateBuilders()
        {
            var itemColumns = ItemColumns.Columns.ToList();

            return new List<IExcelValidationRuleBuilder>
            {
                new HierarchyClassExcelValidationRuleBuilder("Brands", itemColumns.IndexOf(ExcelExportColumnNames.Brand)),
                new HierarchyClassExcelValidationRuleBuilder("Merchandise", itemColumns.IndexOf(ExcelExportColumnNames.Merchandise)),
                new HierarchyClassExcelValidationRuleBuilder("Tax", itemColumns.IndexOf(ExcelExportColumnNames.Tax)),
                new HierarchyClassExcelValidationRuleBuilder("Browsing", itemColumns.IndexOf(ExcelExportColumnNames.Browsing)),
                new HierarchyClassExcelValidationRuleBuilder("National", itemColumns.IndexOf(ExcelExportColumnNames.NationalClass)),

                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.FoodStampEligible)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.DepartmentSale)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.Validated)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.HiddenItem)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.Biodynamic)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.CheeseAttributeRaw)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.PremiumBodyCare)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.Vegetarian)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.WholeTrade)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.GrassFed)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.PastureRaised)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.FreeRange)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.DryAged)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.AirChilled)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.MadeInHouse)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.Msc)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.CaseinFree)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.Hemp)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.LocalLoanProducer)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.NutritionRequired)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.OrganicPersonalCare)),
                new YesOrNoExcelValidationRuleBuilder(itemColumns.IndexOf(ExcelExportColumnNames.Paleo)),

                new RemovableValueCollectionExcelValidationRuleBuilder(AnimalWelfareRatings.Descriptions.AsArray, itemColumns.IndexOf(ExcelExportColumnNames.AnimalWelfareRating)),
                new RemovableValueCollectionExcelValidationRuleBuilder(MilkTypes.Descriptions.AsArray, itemColumns.IndexOf(ExcelExportColumnNames.CheeseAttributeMilkType)),
                new RemovableValueCollectionExcelValidationRuleBuilder(EcoScaleRatings.Descriptions.AsArray, itemColumns.IndexOf(ExcelExportColumnNames.EcoScaleRating)),
                new RemovableValueCollectionExcelValidationRuleBuilder(SeafoodFreshOrFrozenTypes.Descriptions.AsArray, itemColumns.IndexOf(ExcelExportColumnNames.SeafoodFreshOrFrozen)),
                new RemovableValueCollectionExcelValidationRuleBuilder(SeafoodCatchTypes.Descriptions.AsArray, itemColumns.IndexOf(ExcelExportColumnNames.SeafoodWildOrFarmRaised)),
                new RemovableValueCollectionExcelValidationRuleBuilder(DrainedWeightUoms.AsArray, itemColumns.IndexOf(ExcelExportColumnNames.DrainedWeightUom)),               
            };
        }
    }
}