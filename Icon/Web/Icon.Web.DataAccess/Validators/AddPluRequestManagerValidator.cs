using Icon.Framework;
using Icon.Web.Common;

using Icon.Web.DataAccess.Managers;
using System;
using Icon.Common;
using Icon.Common.Validators;

namespace Icon.Web.Mvc.Validators.Managers
{
    public class AddPluRequestManagerValidator : IObjectValidator<AddPluRequestManager>
    {

        public AddPluRequestManagerValidator() { }

        public ObjectValidationResult Validate(AddPluRequestManager manager)
        {
            if (!string.IsNullOrEmpty(manager.NationalPlu) && !new ScanCodeValidator().Validate(manager.NationalPlu ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult(
                    $"National PLU must be {Constants.ScanCodeMaxLength} or fewer numbers.");
            }

            if (String.IsNullOrWhiteSpace(manager.BrandName) || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandName).IsValid(manager.BrandName, null))
            {
                return ObjectValidationResult.InvalidResult(
                    $"Brand name is required: {ValidatorErrorMessages.BrandNameError}");
            }

            if (String.IsNullOrWhiteSpace(manager.ProductDescription) || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription).IsValid(manager.ProductDescription, null))
            {
                return ObjectValidationResult.InvalidResult(
                    $"Product description is required: {ValidatorErrorMessages.ProductDescriptionError}");
            }

            if (String.IsNullOrWhiteSpace(manager.PosDescription) || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription).IsValid(manager.PosDescription, null))
            {
                return ObjectValidationResult.InvalidResult(
                    $"POS description is required: {ValidatorErrorMessages.PosDescriptionError}");
            }

            if (!new PackageUnitValidator().Validate(manager.PackageUnit.ToString()))
            {
                return ObjectValidationResult.InvalidResult(
                    $"Item Pack must be a whole number with {Constants.PackageUnitMaxLength} or fewer digits.");
            }

            if (!manager.FinancialHierarchyClassId.HasValue)
            {
                return ObjectValidationResult.InvalidResult(String.Format("SubTeam is required"));
            }

            if (manager.NationalHierarchyClassId < 1)
            {
                return ObjectValidationResult.InvalidResult(String.Format("National Class is required"));
            }

            if (!new RetailUomValidator().Validate(manager.RetailUom))
            {
                return ObjectValidationResult.InvalidResult(
                    $"UOM is required and should be one of the following: {String.Join(", ", UomCodes.ByName.Values)}.");
            }

            return ObjectValidationResult.ValidResult();
        }
    }
}