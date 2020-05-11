using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Mappers
{
    public class AttributeErrorMessageMapper : IAttributeErrorMessageMapper
    {
        public string Map(AttributeColumn attributeColumn, string value)
        {
            // parent company is excluded because it follows different validation rules.

            string errorMessage;
            

            switch (attributeColumn.ColumnHeader.Name)
            {
                case "Zip Code":
                    errorMessage = "Zip Code must be 10 characters or less and only contain letters, numbers, space and hyphens";
                    break;
                case "Locality":
                    errorMessage = "Locality allows up to 35 characters";
                    break;
                case "Designation":
                    errorMessage = "Designation allowed values are 'Global' or 'Regional'";
                    break;
                case "Brand Abbreviation":
                    errorMessage = "Brand Abbreviation allows 10 or fewer valid characters (Letters, Numbers, and Ampersands only)";
                    break;
                case "Brand Name":
                    errorMessage = "Brand Name allows up to 35 characters";
                    break;
                default:
                    errorMessage = $"'{value}' does not meet traitPattern [ {attributeColumn.RegexPattern} ] requirements";
                    break;
            }

            return errorMessage;
        }
    }
}