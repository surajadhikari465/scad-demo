using Icon.Framework;
using Icon.Web.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Icon.Common;
using Icon.Common.Validators;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BrandNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var hierarchyClassModel = validationContext.ObjectInstance as HierarchyClassViewModel;

            if (new HierarchyClassNameValidator().Validate(value as string ?? String.Empty))
            {
                return ValidationResult.Success;
            }
            else if (hierarchyClassModel != null && hierarchyClassModel.HierarchyName != HierarchyNames.Brands)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(String.Format("Please enter a brand name with {0} or fewer characters.", Constants.IconBrandNameMaxLength));
        }
    }
}