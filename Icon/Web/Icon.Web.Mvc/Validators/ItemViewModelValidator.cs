using Icon.Framework;
using Icon.Web.Common.Validators;
using Icon.Web.Mvc.Models;
using System;
using Icon.Web.Common;

namespace Icon.Web.Mvc.Validators
{
    public class ItemViewModelValidator : IObjectValidator<ItemViewModel>
    {
        public ObjectValidationResult Validate(ItemViewModel itemViewModel)
        {
            if (!new ScanCodeValidator().Validate(itemViewModel.ScanCode ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult("Scan code is required, must be numeric, and must be 12 or fewer characters long.");
            }

            if (!IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription).IsValid(itemViewModel.ProductDescription ?? String.Empty, null))
            {
                return ObjectValidationResult.InvalidResult("Product description is required and must be 60 or fewer characters long.");
            }

            if (!IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription).IsValid(itemViewModel.PosDescription ?? String.Empty, null))
            {
                return ObjectValidationResult.InvalidResult("POS description is required and must be 25 or fewer characters long.");
            }

            if (!new PosScaleTareValidator().Validate(itemViewModel.PosScaleTare ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult("POS scale tare is required and must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point.");
            }

            if (!new RetailSizeValidator().Validate(itemViewModel.RetailSize ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult("Size must be greater than zero and contain only numbers and decimal points with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal.");
            }

            if (!new RetailUomValidator().Validate(itemViewModel.RetailUom ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult(String.Format("UOM should be one of the following: {0}.", String.Join(", ", UomCodes.ByName.Values)));
            }

            if (itemViewModel.IsValidated)
            {
                if (!itemViewModel.MerchandiseHierarchyClassId.HasValue)
                {
                    return ObjectValidationResult.InvalidResult("Merchandising hierarchy class is required for validated items.");
                }

                if (!itemViewModel.TaxHierarchyClassId.HasValue)
                {
                    return ObjectValidationResult.InvalidResult("Tax hierarchy class is required is required for validated items.");
                }
                if (!itemViewModel.NationalHierarchyClassId.HasValue)
                {
                    return ObjectValidationResult.InvalidResult("National hierarchy class is required is required for validated items.");
                }
            }

            return ObjectValidationResult.ValidResult();
        }
    }
}