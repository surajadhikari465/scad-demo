using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Validators.Managers
{
    public class AddItemManagerValidator : IObjectValidator<AddItemManager>
    {
        private IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>> getExistingScanCodeUploadsQuery;
        private IQueryHandler<GetTaxClassesWithNoAbbreviationParameters, List<string>> getTaxClassesWithNoAbbreviationQuery;
        private IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>> getDuplicateBrandsByTrimmedNameQuery;

        public AddItemManagerValidator(
            IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>> getExistingScanCodeUploadsQuery,
            IQueryHandler<GetTaxClassesWithNoAbbreviationParameters, List<string>> getTaxClassesWithNoAbbreviationQuery,
            IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>> getDuplicateBrandsByTrimmedNameQuery)
        {
            this.getExistingScanCodeUploadsQuery = getExistingScanCodeUploadsQuery;
            this.getTaxClassesWithNoAbbreviationQuery = getTaxClassesWithNoAbbreviationQuery;
            this.getDuplicateBrandsByTrimmedNameQuery = getDuplicateBrandsByTrimmedNameQuery;
        }

        public ObjectValidationResult Validate(AddItemManager manager)
        {
            if (string.IsNullOrWhiteSpace(manager.Item.ScanCode) || !new ScanCodeValidator().Validate(manager.Item.ScanCode))
            {
                return ObjectValidationResult.InvalidResult(string.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength));
            }

            if (string.IsNullOrWhiteSpace(manager.Item.BrandName) || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandName).IsValid(manager.Item.BrandName, null))
            {
                return ObjectValidationResult.InvalidResult(string.Format("Brand name is required: {0}", ValidatorErrorMessages.BrandNameError));
            }

            if (string.IsNullOrWhiteSpace(manager.Item.ProductDescription) || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription).IsValid(manager.Item.ProductDescription, null))
            {
                return ObjectValidationResult.InvalidResult(string.Format("Product description is required: {0}", ValidatorErrorMessages.ProductDescriptionError));
            }

            if (string.IsNullOrWhiteSpace(manager.Item.PosDescription) || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription).IsValid(manager.Item.PosDescription, null))
            {
                return ObjectValidationResult.InvalidResult(string.Format("POS description is required: {0}", ValidatorErrorMessages.PosDescriptionError));
            }

            if (!new PackageUnitValidator().Validate(manager.Item.PackageUnit))
            {
                return ObjectValidationResult.InvalidResult(string.Format("Item Pack must be a whole number with {0} or fewer digits.", Constants.PackageUnitMaxLength));
            }

            if (!new PosScaleTareValidator().Validate(manager.Item.PosScaleTare))
            {
                return ObjectValidationResult.InvalidResult("POS scale tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point.");
            }

            if (!new RetailSizeValidator().Validate(manager.Item.RetailSize))
            {
                return ObjectValidationResult.InvalidResult("Size must be greater than zero and contain only numbers and decimal points with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal.");
            }

            if (!new RetailUomValidator().Validate(manager.Item.RetailUom))
            {
                return ObjectValidationResult.InvalidResult(string.Format("UOM should be one of the following: {0}.", string.Join(", ", UomCodes.ByName.Values)));
            }

            if (manager.Item.IsValidated == "1")
            {
                int hierarchyClassId = 0;
                if (!int.TryParse(manager.Item.MerchandiseId, out hierarchyClassId) || hierarchyClassId < 1)
                {
                    return ObjectValidationResult.InvalidResult("Merchandise hierarchy class is required for validated items.");
                }

                if (!int.TryParse(manager.Item.TaxId, out hierarchyClassId) || hierarchyClassId < 1)
                {
                    return ObjectValidationResult.InvalidResult("Tax hierarchy class is required for validated items.");
                }
                if (!int.TryParse(manager.Item.NationalId, out hierarchyClassId) || hierarchyClassId < 1)
                {
                    return ObjectValidationResult.InvalidResult("National hierarchy class is required for validated items.");
                }
            }

            var scanCodeExists = getExistingScanCodeUploadsQuery.Search(new GetExistingScanCodeUploadsParameters
                {
                    ScanCodes = new List<ScanCodeModel> 
                    { 
                        new ScanCodeModel() { ScanCode = manager.Item.ScanCode }
                    }
                }).Any();

            if (scanCodeExists)
            {
                return ObjectValidationResult.InvalidResult(string.Format("Scan code {0} already exists.", manager.Item.ScanCode));
            }

            int taxId = 0;
            int.TryParse(manager.Item.TaxId, out taxId);
            if (int.TryParse(manager.Item.TaxId, out taxId) && taxId > 0)
            {
                var taxClassesWithoutAbbreviations = getTaxClassesWithNoAbbreviationQuery.Search(new GetTaxClassesWithNoAbbreviationParameters
                {
                    TaxClasses = new List<string> { manager.Item.TaxId.ToString() }
                });

                if (taxClassesWithoutAbbreviations.Any())
                {
                    return ObjectValidationResult.InvalidResult(string.Format("Tax Hierarchy Class {0} does not have a Tax Abbreviation.  Cannot associate items to Tax Hierarchy Classes with no Tax Abbreviation.", taxClassesWithoutAbbreviations.First()));
                }
            }

            if (manager.Item.BrandName.Length > Constants.IrmaBrandNameMaxLength)
            {
                GetDuplicateBrandsByTrimmedNameParameters parameters = new GetDuplicateBrandsByTrimmedNameParameters
                {
                    LongBrandNameList = new Dictionary<string, string>() { { manager.Item.BrandName, manager.Item.BrandName.Substring(0, Constants.IrmaBrandNameMaxLength) } }
                };

                var existingBrandWithNameEqualToIrmaBrandNameLimit = getDuplicateBrandsByTrimmedNameQuery.Search(parameters);

                if (existingBrandWithNameEqualToIrmaBrandNameLimit.Count > 0)
                {
                    return ObjectValidationResult.InvalidResult(string.Format("Brand name {0} when shortened to {1} characters is {2} which is an already existing Brand in Icon and may cause conflicts with IRMA.",
                        manager.Item.BrandName,
                        Constants.IrmaBrandNameMaxLength,
                        existingBrandWithNameEqualToIrmaBrandNameLimit.First()));
                }
            }

            return ObjectValidationResult.ValidResult();
        }
    }
}