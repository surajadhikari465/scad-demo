using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Excel.ModelMappers
{
    public class ItemExcelModelMapper : IExcelModelMapper<ItemSearchModel, ItemExcelModel>
    {
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQuery;
        private IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesByTraitQuery;

        public ItemExcelModelMapper(IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQuery,
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesByTraitQuery)
        {
            this.getHierarchyLineageQuery = getHierarchyLineageQuery;
            this.getCertificationAgenciesByTraitQuery = getCertificationAgenciesByTraitQuery;
        }

        public IEnumerable<ItemExcelModel> Map(IEnumerable<ItemSearchModel> models)
        {
            var hierarchyClasses = getHierarchyLineageQuery.Search(new GetHierarchyLineageParameters());

            var brands = GetHierarchyClassDictionary(hierarchyClasses.BrandHierarchyList);
            var merchandiseHierarchyClasses = GetHierarchyClassDictionary(hierarchyClasses.MerchandiseHierarchyList);
            var taxHierarchyClasses = GetHierarchyClassDictionary(hierarchyClasses.TaxHierarchyList);
            var nationalHierarchyClasses = GetHierarchyClassDictionary(hierarchyClasses.NationalHierarchyList);
            var browsingHierarchyClasses = GetHierarchyClassDictionary(hierarchyClasses.BrowsingHierarchyList);

            var glutenFreeAgencies = GetAgencyDictionary(getCertificationAgenciesByTraitQuery.Search(new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = Traits.Codes.GlutenFree }));
            var kosherAgencies = GetAgencyDictionary(getCertificationAgenciesByTraitQuery.Search(new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = Traits.Codes.Kosher }));
            var nonGmoAgencies = GetAgencyDictionary(getCertificationAgenciesByTraitQuery.Search(new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = Traits.Codes.NonGmo }));
            var organicAgencies = GetAgencyDictionary(getCertificationAgenciesByTraitQuery.Search(new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = Traits.Codes.Organic }));
            var veganAgencies = GetAgencyDictionary(getCertificationAgenciesByTraitQuery.Search(new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = Traits.Codes.Vegan }));

            return models.Select(model => new ItemExcelModel
            {
                ScanCode = model.ScanCode,
                Brand = brands[model.BrandHierarchyClassId.Value],
                ProductDescription = model.ProductDescription,
                PosDescription = model.PosDescription,
                PackageUnit = model.PackageUnit,
                FoodStampEligible = ConversionUtility.ConvertBitStringToYesNo(model.FoodStampEligible),
                PosScaleTare = model.PosScaleTare,
                RetailSize = model.RetailSize,
                Uom = model.RetailUom,
                DeliverySystem = model.DeliverySystem,
                Merchandise = model.MerchandiseHierarchyClassId.HasValue ? merchandiseHierarchyClasses[model.MerchandiseHierarchyClassId.Value] : null,
                NationalClass = model.NationalHierarchyClassId.HasValue ? nationalHierarchyClasses[model.NationalHierarchyClassId.Value] : null,
                Tax = model.TaxHierarchyClassId.HasValue ? taxHierarchyClasses[model.TaxHierarchyClassId.Value] : null,
                Browsing = model.BrowsingHierarchyClassId.HasValue ? browsingHierarchyClasses[model.BrowsingHierarchyClassId.Value] : null,
                Validated = ConversionUtility.ConvertBoolToYesNo(model.GetValidationStatus()),
                DepartmentSale = ConversionUtility.ConvertBoolToYesNo(model.GetDepartmentSale()),
                HiddenItem = ConversionUtility.ConvertBoolToYesNo(model.GetHiddenItemStatus()),
                Notes = model.Notes,
                AnimalWelfareRating = GetDescription(AnimalWelfareRatings.AsDictionary, model.AnimalWelfareRatingId),
                Biodynamic = ConversionUtility.ConvertNullableBoolToYesNo(model.Biodynamic),
                CheeseAttributeMilkType = GetDescription(MilkTypes.AsDictionary, model.CheeseMilkTypeId),
                CheeseAttributeRaw = ConversionUtility.ConvertNullableBoolToYesNo(model.CheeseRaw),
                EcoScaleRating = GetDescription(EcoScaleRatings.AsDictionary, model.EcoScaleRatingId),
                GlutenFree = model.GlutenFreeAgencyId.HasValue ? glutenFreeAgencies[model.GlutenFreeAgencyId.Value] : null,
                Kosher = model.KosherAgencyId.HasValue ? kosherAgencies[model.KosherAgencyId.Value] : null,
                Msc = ConversionUtility.ConvertNullableBoolToYesNo(model.Msc),
                NonGmo = model.NonGmoAgencyId.HasValue ? nonGmoAgencies[model.NonGmoAgencyId.Value] : null,
                Organic = model.OrganicAgencyId.HasValue ? organicAgencies[model.OrganicAgencyId.Value] : null,
                PremiumBodyCare = ConversionUtility.ConvertNullableBoolToYesNo(model.PremiumBodyCare),
                SeafoodFreshOrFrozen = GetDescription(SeafoodFreshOrFrozenTypes.AsDictionary, model.SeafoodFreshOrFrozenId),
                SeafoodWildOrFarmRaised = GetDescription(SeafoodCatchTypes.AsDictionary, model.SeafoodCatchTypeId),
                Vegan = model.VeganAgencyId.HasValue ? veganAgencies[model.VeganAgencyId.Value] : null,
                Vegetarian = ConversionUtility.ConvertNullableBoolToYesNo(model.Vegetarian),
                WholeTrade = ConversionUtility.ConvertNullableBoolToYesNo(model.WholeTrade),
                GrassFed = ConversionUtility.ConvertNullableBoolToYesNo(model.GrassFed),
                PastureRaised = ConversionUtility.ConvertNullableBoolToYesNo(model.PastureRaised),
                FreeRange = ConversionUtility.ConvertNullableBoolToYesNo(model.FreeRange),
                DryAged = ConversionUtility.ConvertNullableBoolToYesNo(model.DryAged),
                AirChilled = ConversionUtility.ConvertNullableBoolToYesNo(model.AirChilled),
                MadeInHouse = ConversionUtility.ConvertNullableBoolToYesNo(model.MadeInHouse),
                AlcoholByVolume = model.AlcoholByVolume,
                CaseinFree = ConversionUtility.ConvertBitStringToYesNo(model.CaseinFree),
                DrainedWeight = model.DrainedWeight,
                DrainedWeightUom = model.DrainedWeightUom,
                FairTradeCertified = model.FairTradeCertified,
                Hemp = ConversionUtility.ConvertBitStringToYesNo(model.Hemp),
                LocalLoanProducer = ConversionUtility.ConvertBitStringToYesNo(model.LocalLoanProducer),
                MainProductName = model.MainProductName,
                NutritionRequired = ConversionUtility.ConvertBitStringToYesNo(model.NutritionRequired),
                OrganicPersonalCare = ConversionUtility.ConvertBitStringToYesNo(model.OrganicPersonalCare),
                Paleo = ConversionUtility.ConvertBitStringToYesNo(model.Paleo),
                ProductFlavorType = model.ProductFlavorType,
                CreatedDate = model.CreatedDate,
                LastModifiedDate = model.LastModifiedDate,
                LastModifiedUser = model.LastModifiedUser
            });
        }

        private Dictionary<int, string> GetAgencyDictionary(List<HierarchyClass> list)
        {
            return list.ToDictionary(hc => hc.hierarchyClassID, hc => hc.hierarchyClassName + "|" + hc.hierarchyClassID);
        }

        private static Dictionary<int, string> GetHierarchyClassDictionary(List<HierarchyClassModel> hierarchyClasses)
        {
            return hierarchyClasses.ToDictionary(hc => hc.HierarchyClassId, hc => hc.HierarchyLineage + "|" + hc.HierarchyClassId);
        }

        private static string GetDescription(Dictionary<int, string> idToDescriptionDictionary, int? id)
        {
            return id.HasValue ? idToDescriptionDictionary[id.Value] : null;
        }
    }
}