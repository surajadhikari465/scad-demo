namespace Icon.Web.Mvc.Excel.Validators.Factories
{
    using Common;
    using Common.Validators;
    using DataAccess.Models;
    using DataAccess.Queries;
    using Framework;
    using Icon.Common.DataAccess;
    using Models;
    using SimpleInjector;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ItemExcelValidatorFactory : IExcelValidatorFactory<ItemExcelModel>
    {
        private const string YesNoPattern = "^[Y|y|N|n]?$";

        private readonly Container container;

        public ItemExcelValidatorFactory(Container container)
        {
            this.container = container;
        }

        public IEnumerable<IExcelValidator<ItemExcelModel>> CreateValidators()
        {
            var certificationAgencyQuery = this.container.GetInstance<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>();
            var getHierarchyLineageQuery = this.container.GetInstance<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            var getAffinitySubBricksQuery = this.container.GetInstance<IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>>>();
            var getTaxHierarchyClassesWithoutAbbrevQuery = this.container.GetInstance<IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>>>();
            var getScanCodesNotReadyToValidateQuery = this.container.GetInstance<IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>>>();
            var getNewScanCodeUploadsQuery = this.container.GetInstance<IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>>>();
            var posScaleTareValidator = new PosScaleTareValidator();
            var retailSizeValidator = new RetailSizeValidator();

            return new List<IExcelValidator<ItemExcelModel>>()
            {
                new RegexItemModelValidator<ItemExcelModel>(
                    x => x.ScanCode,
                    string.Format("^[0-9]{{1,{0}}}$", Constants.ScanCodeMaxLength),
                    string.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength)),

                new DuplicateScanCodeValidator(),

                new RegexItemModelValidator<ItemExcelModel>(
                    x => x.ProductDescription,
                    TraitPatterns.ProductDescription,
                    "Product Description is invalid. " + ValidatorErrorMessages.ProductDescriptionError),

                new RegexItemModelValidator<ItemExcelModel>(
                    x => x.PosDescription,
                    TraitPatterns.PosDescription,
                    "POS Description is invalid. " + ValidatorErrorMessages.PosDescriptionError),

                new RegexItemModelValidator<ItemExcelModel>(
                    x => x.PackageUnit,
                    TraitPatterns.PackageUnit,
                    string.Format("Item Pack must be a whole number with {0} or fewer digits.", Constants.PackageUnitMaxLength)),

                new PredicateExcelValidator<ItemExcelModel>(x =>
                    {
                        return x.RetailSize == string.Empty || retailSizeValidator.Validate(x.RetailSize);
                    },
                    "Size must be greater than zero and contain only numbers and decimal points with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal."),

                new PredicateExcelValidator<ItemExcelModel>(x =>
                    {
                        return posScaleTareValidator.Validate(x.PosScaleTare);
                    },
                    "POS Scale Tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point."),

                // Valid lists for entry

                new PredicateExcelValidator<ItemExcelModel>(x =>
                    {
                        var validUoms = UomCodes.ByName.Values.ToList();
                        return string.IsNullOrEmpty(x.Uom) || validUoms.Contains(x.Uom);
                    },
                    string.Format("UOM should be one of the following: {0}.", string.Join(", ", UomCodes.ByName.Values))),

                new PredicateExcelValidator<ItemExcelModel>(x =>
                    {
                        var validDelivery = DeliverySystems.AsDictionary.Values.ToList();
                        return string.IsNullOrEmpty(x.DeliverySystem) || validDelivery.Contains(x.DeliverySystem);
                    },
                    string.Format("Delivery System should be one of the following: {0}.", String.Join(", ", DeliverySystems.AsDictionary.Values))),

                new PredicateExcelValidator<ItemExcelModel>(x =>
                    {
                        return string.IsNullOrEmpty(x.AnimalWelfareRating)
                        || x.AnimalWelfareRating == Constants.ExcelImportRemoveValueKeyword
                        || AnimalWelfareRatings.Descriptions.AsArray.Contains(x.AnimalWelfareRating, StringComparer.OrdinalIgnoreCase);
                    },
                    string.Format("Animal Welfare Rating is not recognized.  Valid entries are {0}.", string.Join(", ", AnimalWelfareRatings.Descriptions.AsArray))),

                new PredicateExcelValidator<ItemExcelModel>(x =>
                    {
                        return string.IsNullOrEmpty(x.CheeseAttributeMilkType)
                        || x.CheeseAttributeMilkType == Constants.ExcelImportRemoveValueKeyword
                        || MilkTypes.Descriptions.AsArray.Contains(x.CheeseAttributeMilkType, StringComparer.OrdinalIgnoreCase);
                    },
                    string.Format("Cheese Attribute: Milk Type is not recognized.  Valid entries are {0}.", string.Join(", ", MilkTypes.Descriptions.AsArray))),

                new PredicateExcelValidator<ItemExcelModel>(x =>
                    {
                        return string.IsNullOrEmpty(x.EcoScaleRating)
                        || x.EcoScaleRating == Constants.ExcelImportRemoveValueKeyword
                        || EcoScaleRatings.Descriptions.AsArray.Contains(x.EcoScaleRating, StringComparer.OrdinalIgnoreCase);
                    },
                    string.Format("Eco-Scale Rating is not recognized.  Valid entries are {0}.", string.Join(", ", EcoScaleRatings.Descriptions.AsArray))),

                new PredicateExcelValidator<ItemExcelModel>(x =>
                    {
                        return string.IsNullOrEmpty(x.SeafoodFreshOrFrozen)
                        || x.SeafoodFreshOrFrozen == Constants.ExcelImportRemoveValueKeyword
                        || SeafoodFreshOrFrozenTypes.Descriptions.AsArray.Contains(x.SeafoodFreshOrFrozen, StringComparer.OrdinalIgnoreCase);
                    },
                    string.Format("Fresh Or Frozen is not recognized.  Valid entries are {0}.", string.Join(", ", SeafoodFreshOrFrozenTypes.Descriptions.AsArray))),

                new PredicateExcelValidator<ItemExcelModel>(x =>
                    {
                        return string.IsNullOrEmpty(x.SeafoodWildOrFarmRaised)
                        || x.SeafoodWildOrFarmRaised == Constants.ExcelImportRemoveValueKeyword
                        || SeafoodCatchTypes.Descriptions.AsArray.Contains(x.SeafoodWildOrFarmRaised, StringComparer.OrdinalIgnoreCase);
                    },
                    string.Format("Seafood: Wild Or Farm Raised is not recognized.  Valid entries are {0}.", string.Join(", ", SeafoodCatchTypes.Descriptions.AsArray))),

                new RegexOrEmptyStringItemModelValidator<ItemExcelModel>(
                    x => x.AlcoholByVolume,
                    TraitPatterns.AlcoholByVolume,
                    ValidatorErrorMessages.AlcoholByVolumeError),
                new RegexOrEmptyStringItemModelValidator<ItemExcelModel>(
                    x => x.DrainedWeight,
                    TraitPatterns.DrainedWeight,
                    "Drained Weight is not recognized. Valid entry should be a number with maximum of 4 decimal places."),
                new RegexOrEmptyStringItemModelValidator<ItemExcelModel>(
                    x => x.DrainedWeightUom,
                    TraitPatterns.DrainedWeightUom + "|" + Constants.ExcelImportRemoveValueKeyword,
                    string.Format("Drained Weight UOM is not recognized. Valid entries are {0}", string.Join(", ", DrainedWeightUoms.AsArray))),
                new RegexOrEmptyStringItemModelValidator<ItemExcelModel>(
                    x => x.FairTradeCertified,
                    TraitPatterns.FairTradeCertified + "|" + Constants.ExcelImportRemoveValueKeyword,
                    string.Format("Fair Trade Certified is not recognized.  Valid entries are {0}.", string.Join(", ", FairTradeCertifiedValues.AsArray))),
                new RegexOrEmptyStringItemModelValidator<ItemExcelModel>(
                    x => x.MainProductName,
                    TraitPatterns.MainProductName,
                    "Main Product Name is not recognized. Valid entry should be a text value with maximum of 255 characters"),
                new RegexOrEmptyStringItemModelValidator<ItemExcelModel>(
                    x => x.ProductFlavorType,
                    TraitPatterns.ProductFlavorType,
                    "Product Flavor or Type is not recognized. Valid entry should be a text value with maximum of 255 characters"),

                // Certification Agencies
                new CertificationAgencyValidator<ItemExcelModel>(x => x.Vegan, TraitCodes.Vegan, certificationAgencyQuery),
                new CertificationAgencyValidator<ItemExcelModel>(x => x.GlutenFree, TraitCodes.GlutenFree, certificationAgencyQuery),
                new CertificationAgencyValidator<ItemExcelModel>(x => x.Kosher, TraitCodes.Kosher, certificationAgencyQuery),
                new CertificationAgencyValidator<ItemExcelModel>(x => x.NonGmo, TraitCodes.NonGmo, certificationAgencyQuery),
                new CertificationAgencyValidator<ItemExcelModel>(x => x.Organic, TraitCodes.Organic, certificationAgencyQuery),

                // Yes or No traits.
                new RegexItemModelValidator<ItemExcelModel>(x => x.AirChilled, YesNoPattern, "Air Chilled should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.Biodynamic, YesNoPattern, "Biodynamic should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.CheeseAttributeRaw, YesNoPattern, "Cheese Attribute: Raw should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.DepartmentSale, YesNoPattern, "Department Sale must be blank, Y, or N."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.DryAged, YesNoPattern, "Dry Aged should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.FoodStampEligible, YesNoPattern, "Food Stamp Eligible must be blank, Y, or N."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.FreeRange, YesNoPattern, "Free Range should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.GrassFed, YesNoPattern, "Grass Fed should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.HiddenItem, YesNoPattern, "HiddenItem must be blank, Y, or N."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.MadeInHouse, YesNoPattern, "Made In House should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.Msc, YesNoPattern, "MSC should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.PastureRaised, YesNoPattern, "Pasture Raised should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.PremiumBodyCare, YesNoPattern, "Premium Body Care should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.Validated, YesNoPattern, "Validated should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.Vegetarian, YesNoPattern, "Vegetarian should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.WholeTrade, YesNoPattern, "Whole Trade should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.CaseinFree, YesNoPattern, "Casein Free should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.Hemp, YesNoPattern, "Hemp should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.OrganicPersonalCare, YesNoPattern, "Organic Personal Care should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.Paleo, YesNoPattern, "Paleo should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.LocalLoanProducer, YesNoPattern, "Local Loan Producer should be Y, N, or blank."),
                new RegexItemModelValidator<ItemExcelModel>(x => x.NutritionRequired, YesNoPattern, "Nutrition Required should be Y, N, or blank."),

                new HierarchyClassValidator(getHierarchyLineageQuery),

                new AffinitySubBrickAssociationsValidator(getAffinitySubBricksQuery),

                new ItemsAreAbleToBeValidatedExcelValidator(getScanCodesNotReadyToValidateQuery),

                new ScanCodesExistsValidator(getNewScanCodeUploadsQuery),

                new ItemWithoutUpdatesExcelValidator<ItemExcelModel>(),
            };
        }
    }
}