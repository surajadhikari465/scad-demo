using System.ComponentModel.DataAnnotations;

namespace WFM.OutOfStock.API.Validation
{
    public sealed class ListItemsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var listItems = value as string[];
            if (listItems == null || listItems.Length <= 0)
            {
                return new ValidationResult("At least one item must be included in a list");
            }

            if (listItems.Length > 500)
            {
                return new ValidationResult("Lists must not contain more than 500 items");
            }

            return ValidationResult.Success;
        }
    }
}