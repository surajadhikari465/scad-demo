using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Utility;
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
    public class UpdateItemManagerValidator : IObjectValidator<UpdateItemManager>
    {
        private IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> getItemsByBulkScanCodeSearchQuery;
        private IQueryHandler<GetTaxClassesWithNoAbbreviationParameters, List<string>> getTaxClassesWithNoAbbreviationQuery;

        public UpdateItemManagerValidator(
            IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> getItemsByBulkScanCodeSearchQuery,
            IQueryHandler<GetTaxClassesWithNoAbbreviationParameters, List<string>> getTaxClassesWithNoAbbreviationQuery)
        {
            this.getItemsByBulkScanCodeSearchQuery = getItemsByBulkScanCodeSearchQuery;
            this.getTaxClassesWithNoAbbreviationQuery = getTaxClassesWithNoAbbreviationQuery;
        }

        public ObjectValidationResult Validate(UpdateItemManager manager)
        {
            if (String.IsNullOrWhiteSpace(manager.Item.ScanCode) || !new ScanCodeValidator().Validate(manager.Item.ScanCode ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult(String.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength));
            }

            if (!IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription).IsValid(manager.Item.ProductDescription, null))
            {
                return ObjectValidationResult.InvalidResult(String.Format("Product description is required: {0}", ValidatorErrorMessages.ProductDescriptionError));
            }

            if (!IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription).IsValid(manager.Item.PosDescription, null))
            {
                return ObjectValidationResult.InvalidResult(String.Format("POS description is required: {0}", ValidatorErrorMessages.PosDescriptionError));
            }

            if (!new PackageUnitValidator().Validate(manager.Item.PackageUnit ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult("Item Pack must be a whole number with three or fewer digits.");
            }

            if (!new PosScaleTareValidator().Validate(manager.Item.PosScaleTare ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult("POS scale tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point.");
            }

            if (!new RetailSizeValidator().Validate(manager.Item.RetailSize ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult("Size must be greater than zero and contain only numbers and decimal points with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal.");
            }

            if (!new RetailUomValidator().Validate(manager.Item.RetailUom ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult(String.Format("UOM should be one of the following: {0}.", String.Join(", ", UomCodes.ByName.Values)));
            }

            if (!IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.Notes).IsValid(manager.Item.Notes ?? String.Empty, null))
            {
                return ObjectValidationResult.InvalidResult(String.Format("Notes should be {0} or fewer valid characters.", Constants.NotesMaxLength));
            }

            var item = getItemsByBulkScanCodeSearchQuery.Search(new GetItemsByBulkScanCodeSearchParameters { ScanCodes = new List<string> { manager.Item.ScanCode } }).First();

            if (item.GetValidationStatus())
            {
                if (!HierarchyClassExists(manager.Item.MerchandiseId))
                {
                    return ObjectValidationResult.InvalidResult("Merchandise hierarchy class is required for validated items.");
                }

                if (!HierarchyClassExists(manager.Item.TaxId))
                {
                    return ObjectValidationResult.InvalidResult("Tax hierarchy class is required for validated items.");
                }

                if (!HierarchyClassExists(manager.Item.NationalId))
                {
                    return ObjectValidationResult.InvalidResult("National hierarchy class is required for validated items.");
                }
            }

            int taxId = 0;            
            if (int.TryParse(manager.Item.TaxId, out taxId) && taxId > 0)
            {
                var taxClassesWithoutAbbreviations = getTaxClassesWithNoAbbreviationQuery.Search(new GetTaxClassesWithNoAbbreviationParameters
                {
                    TaxClasses = new List<string> { taxId.ToString() }
                });

                if (taxClassesWithoutAbbreviations.Any())
                {
                    return ObjectValidationResult.InvalidResult(String.Format("Tax Hierarchy Class {0} does not have a Tax Abbreviation.  Cannot associate items to Tax Hierarchy Classes with no Tax Abbreviation.", taxClassesWithoutAbbreviations.First()));
                }
            }

            return ObjectValidationResult.ValidResult();
        }

        private static bool HierarchyClassExists(string hierarchyClassIdString)
        {
            int hierarchyClassId = 0;
            int.TryParse(hierarchyClassIdString, out hierarchyClassId);

            return hierarchyClassId > 0;
        }
    }
}