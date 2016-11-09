using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PartialScanCodeAttribute : ValidationAttribute
    {
        public string ScanCodeProperty { get; private set; }

        public PartialScanCodeAttribute(string scanCodeProperty)            
        {
            if (string.IsNullOrEmpty(scanCodeProperty))
            {
                throw new ArgumentNullException("ScanCodeProperty");
            }

            ScanCodeProperty = scanCodeProperty;
        }      

         protected override ValidationResult IsValid(object value, ValidationContext validationContext)
         {
             if (value != null)
             {
                 var scanCodeProperty = validationContext.ObjectInstance.GetType().GetProperty(ScanCodeProperty);

                 string scanCodeValue = scanCodeProperty.GetValue(validationContext.ObjectInstance, null) as string ?? string.Empty;

                 bool partialSearch = (bool)value;
                 if (partialSearch)
                 {
                     if (String.IsNullOrEmpty(scanCodeValue) || scanCodeValue.Length < 3)
                     {
                         return new ValidationResult("Please enter at least 3 digits.");
                     }
                 }               
             }

             return ValidationResult.Success;
         }   
    }
}