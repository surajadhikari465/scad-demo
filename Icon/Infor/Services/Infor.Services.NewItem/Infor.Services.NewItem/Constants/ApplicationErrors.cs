using Icon.Framework;

namespace Infor.Services.NewItem.Constants
{
    public static class ApplicationErrors
    {
        public static class Codes
        {
            public const string PluDoesNotExistInIconError = "PluDoesNotExistInIconError";
            public const string UnexpectedProcessingError = "UnexpectedProcessingError";
            public const string FailedToAddItemsToIcon = "FailedToAddItemsToIcon";
            public const string FailedToSendMessageToEsb = "FailedToSendMessageToEsb";
            public const string InvalidBrand = "Invalid Brand";
            public const string InvalidTaxClassCode = "Invalid Tax Class Code";
            public const string InvalidNationalClassCode = "Invalid National Class Code";
            public const string InvalidProductDescription = "Invalid Product Description";
            public const string InvalidPosDescription = "Invalid POS Description";
            public const string InvalidRetailUom = "Invalid Retail UOM";
        }

        public static class Details
        {
            public const string PluDoesNotExistInIconError = "Unable to send item to Infor because they are PLUs that don't exist in Icon, meaning that the PLU was created in IRMA first.";
            public const string UnexpectedProcessingError = "An unexpected error occurred while processing the items. Error Details: {0}";
            public const string FailedToAddItemsToIcon = "An unexpected error occurred when attempting to add an IRMA Item Subscription in Icon. Error Details: {0}";
            public const string FailedToSendMessageToEsb = "An unexpected error occurred when attempting to send the item to the ESB. Error Details: {0}";
            public static readonly string InvalidBrand = "The item's Brand '{0}' does not exist in Infor. Please choose a different Brand which is managed by Infor and then refresh the item.";
            public static readonly string InvalidTaxClassCode = "The item's Tax Class has a Tax Class Code '{PropertyValue}' which does not exist in Infor. Please choose a different Tax Class which is managed by Infor and then refresh the item.";
            public static readonly string InvalidNationalClassCode = "The item's National Class has a National Class Code '{PropertyValue}' which does not exist in Infor. Please choose a different National Class which is managed by Infor and then refresh the item.";
            public static readonly string InvalidProductDescription = "Product Description has invalid value '{PropertyValue}'." +
                " Product Description is required and must be less than " + LengthConstants.ProductDescriptionMaxLength + " characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @. Please remove the invalid characters and refresh the item.";
            public static readonly string InvalidPosDescription = "POS Description has invalid value '{PropertyValue}'." +
                " POS Description is requried and must be less than " + LengthConstants.PosDescriptionMaxLength + " characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @. Please remove the invalid characters and refresh the item.";
            public static readonly string InvalidRetailUom = "Retail UOM has invalid value '{PropertyValue}'." + " Retail UOM is required and must be one of the following: " + string.Join(",", UomCodes.ByName.Values) + ". Please remove the invalid UOM and refresh the item.";
        }
    }
}
