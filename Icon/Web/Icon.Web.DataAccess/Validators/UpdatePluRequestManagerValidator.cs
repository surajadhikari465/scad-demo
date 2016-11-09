using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Managers;
using System;

namespace Icon.Web.Mvc.Validators.Managers
{
    public class UpdatePluRequestManagerValidator : IObjectValidator<UpdatePluRequestManager>
    {

        public UpdatePluRequestManagerValidator() { }

        public ObjectValidationResult Validate(UpdatePluRequestManager manager)
        {
            if (!string.IsNullOrEmpty(manager.NationalPlu) && !new ScanCodeValidator().Validate(manager.NationalPlu ?? String.Empty))
            {
                return ObjectValidationResult.InvalidResult(String.Format("National PLU must be {0} or fewer numbers.", Constants.ScanCodeMaxLength));
            }

            if (String.IsNullOrWhiteSpace(manager.BrandName) || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandName).IsValid(manager.BrandName, null))
            {
                return ObjectValidationResult.InvalidResult(String.Format("Brand name is required: {0}", ValidatorErrorMessages.BrandNameError));
            }

            if (String.IsNullOrWhiteSpace(manager.ProductDescription) || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription).IsValid(manager.ProductDescription, null))
            {
                return ObjectValidationResult.InvalidResult(String.Format("Product description is required: {0}", ValidatorErrorMessages.ProductDescriptionError));
            }

            if (String.IsNullOrWhiteSpace(manager.PosDescription) || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription).IsValid(manager.PosDescription, null))
            {
                return ObjectValidationResult.InvalidResult(String.Format("POS description is required: {0}", ValidatorErrorMessages.PosDescriptionError));
            }

            if (!new PackageUnitValidator().Validate(manager.PackageUnit.ToString()))
            {
                return ObjectValidationResult.InvalidResult(String.Format("Item Pack must be a whole number with {0} or fewer digits.", Constants.PackageUnitMaxLength));
            }

            if (manager.FinancialHierarchyClassId < 1)
            {
                return ObjectValidationResult.InvalidResult(String.Format("SubTeam is required"));
            }

            if (manager.NationalHierarchyClassId < 1)
            {
                return ObjectValidationResult.InvalidResult(String.Format("National Class is required"));
            }

            if (!new RetailUomValidator().Validate(manager.RetailUom))
            {
                return ObjectValidationResult.InvalidResult(String.Format("UOM isrequired and should be one of the following: {0}.", String.Join(", ", UomCodes.ByName.Values)));
            }

            return ObjectValidationResult.ValidResult();
        }
    }
}