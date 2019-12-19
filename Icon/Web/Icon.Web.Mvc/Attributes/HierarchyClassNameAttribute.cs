using Icon.Framework;
using Icon.Web.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Icon.Common.Validators;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HierarchyClassNameAttribute : ValidationAttribute
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

            return new ValidationResult("Please enter name with 35 or fewer characters.");
        }
    }
}