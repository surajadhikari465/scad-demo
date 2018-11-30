using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Excel.Validators.Factories
{
    public class NewItemExcelModelValidatorFactory : IExcelValidatorFactory<NewItemExcelModel>
    {
        private const string YesNoPattern = "^[Y|y|N|n]?$";

        private readonly Container container;

        public NewItemExcelModelValidatorFactory(Container container)
        {
            this.container = container;
        }

        public IEnumerable<IExcelValidator<NewItemExcelModel>> CreateValidators()
        {
            var certificationAgencyQuery = this.container.GetInstance<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>();
            var getHierarchyLineageQuery = this.container.GetInstance<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            var getAffinitySubBricksQuery = this.container.GetInstance<IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>>>();
            var getTaxHierarchyClassesWithoutAbbrevQuery = this.container.GetInstance<IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>>>();
            var getScanCodesNotReadyToValidateQuery = this.container.GetInstance<IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>>>();
            var getNewScanCodeUploadsQuery = this.container.GetInstance<IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>>>();
            var getExistingScanCodeUploadsQuery = this.container.GetInstance<IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>>>();
            var posScaleTareValidator = new PosScaleTareValidator();
            var retailSizeValidator = new RetailSizeValidator();

            return new List<IExcelValidator<NewItemExcelModel>>()
            {
                new RegexItemModelValidator<NewItemExcelModel>(
                    x => x.ScanCode,
                    string.Format("^[0-9]{{1,{0}}}$", Constants.ScanCodeMaxLength),
                    string.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength)),

                new DuplicateNewItemScanCodeValidator(),

                new ScanCodeAlreadyExistsValidator(getExistingScanCodeUploadsQuery),

                new NewItemRequiredInformationExcelValidator(),

                new RegexItemModelValidator<NewItemExcelModel>(
                    x => x.ProductDescription,
                    TraitPatterns.ProductDescription,
                    "Product Description is invalid. " + ValidatorErrorMessages.ProductDescriptionError),

                new RegexItemModelValidator<NewItemExcelModel>(
                    x => x.PosDescription,
                    TraitPatterns.PosDescription,
                    "POS Description is invalid. " + ValidatorErrorMessages.PosDescriptionError),

                new RegexItemModelValidator<NewItemExcelModel>(
                    x => x.PackageUnit,
                    TraitPatterns.PackageUnit,
                    string.Format("Item Pack must be a whole number with {0} or fewer digits.", Constants.PackageUnitMaxLength)),

                new PredicateExcelValidator<NewItemExcelModel>(x =>
                    {
                        return x.Size == string.Empty || retailSizeValidator.Validate(x.Size);
                    },
                    "Size must be greater than zero and contain only numbers and decimal points with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal."),

                new PredicateExcelValidator<NewItemExcelModel>(x =>
                    {
                        return posScaleTareValidator.Validate(x.PosScaleTare);
                    },
                    "POS Scale Tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point."),

                // Valid lists for entry
                new PredicateExcelValidator<NewItemExcelModel>(x =>
                    {
                        var validUoms = UomCodes.ByName.Values.ToList();
                        return string.IsNullOrEmpty(x.Uom) || validUoms.Contains(x.Uom);
                    },
                    string.Format("UOM should be one of the following: {0}.", string.Join(", ", UomCodes.ByName.Values))),

                new RegexOrEmptyStringItemModelValidator<NewItemExcelModel>(
                    x => x.AlcoholByVolume,
                    TraitPatterns.AlcoholByVolume,
                    ValidatorErrorMessages.AlcoholByVolumeError),

                new RegexOrEmptyStringItemModelValidator<NewItemExcelModel>(
                    x => x.DrainedWeight,
                    TraitPatterns.DrainedWeight,
                    string.Format("Drained Weight is not recognized. Valid entry should be a number with maximum of 4 decimal places.")),

                //new RegexOrEmptyStringItemModelValidator<NewItemExcelModel>(
                //    x => x.DrainedWeightUom,
                //    TraitPatterns.DrainedWeightUom,
                //    string.Format("Drained Weight UOM is not recognized. Valid entries are {0}", string.Join(", ", DrainedWeightUoms.AsArray))),
           
                new RegexOrEmptyStringItemModelValidator<NewItemExcelModel>(
                    x => x.MainProductName,
                    TraitPatterns.MainProductName,
                    string.Format("Main Product Name is not recognized. Valid entry should be a text value with maximum of 255 characters")),

                new RegexOrEmptyStringItemModelValidator<NewItemExcelModel>(
                    x => x.ProductFlavorType,
                    TraitPatterns.ProductFlavorType,
                    string.Format("Product Flavor or Type is not recognized. Valid entry should be a text value with maximum of 255 characters")),

                // Certification Agencies
                new CertificationAgencyValidator<NewItemExcelModel>(x => x.Vegan, TraitCodes.Vegan, certificationAgencyQuery),
                new CertificationAgencyValidator<NewItemExcelModel>(x => x.GlutenFree, TraitCodes.GlutenFree, certificationAgencyQuery),
                new CertificationAgencyValidator<NewItemExcelModel>(x => x.Kosher, TraitCodes.Kosher, certificationAgencyQuery),
                new CertificationAgencyValidator<NewItemExcelModel>(x => x.NonGmo, TraitCodes.NonGmo, certificationAgencyQuery),
                new CertificationAgencyValidator<NewItemExcelModel>(x => x.Organic, TraitCodes.Organic, certificationAgencyQuery),

                // Yes or No traits.
                new RegexItemModelValidator<NewItemExcelModel>(x => x.AirChilled, YesNoPattern, "Air Chilled should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.Biodynamic, YesNoPattern, "Biodynamic should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.CheeseAttributeRaw, YesNoPattern, "Cheese Attribute: Raw should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.DepartmentSale, YesNoPattern, "Department Sale must be blank, Y, or N."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.DryAged, YesNoPattern, "Dry Aged should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.FoodStampEligible, YesNoPattern, "Food Stamp Eligible must be blank, Y, or N."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.FreeRange, YesNoPattern, "Free Range should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.GrassFed, YesNoPattern, "Grass Fed should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.HiddenItem, YesNoPattern, "HiddenItem must be blank, Y, or N."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.MadeInHouse, YesNoPattern, "Made In House should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.Msc, YesNoPattern, "MSC should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.PastureRaised, YesNoPattern, "Pasture Raised should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.PremiumBodyCare, YesNoPattern, "Premium Body Care should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.Validated, YesNoPattern, "Validated should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.Vegetarian, YesNoPattern, "Vegetarian should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.WholeTrade, YesNoPattern, "Whole Trade should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.CaseinFree, YesNoPattern, "Casein Free should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.Hemp, YesNoPattern, "Hemp should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.OrganicPersonalCare, YesNoPattern, "Organic Personal Care should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.Paleo, YesNoPattern, "Paleo should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.LocalLoanProducer, YesNoPattern, "Local Loan Producer should be Y, N, or blank."),
                new RegexItemModelValidator<NewItemExcelModel>(x => x.NutritionRequired, YesNoPattern, "Nutrition Required should be Y, N, or blank."),

                new ValidatedNewItemsHaveRequiredInformationExcelValidator(),

                new ItemWithoutUpdatesExcelValidator<NewItemExcelModel>(),
            };
        }
    }
}