using Icon.Framework;

namespace Services.NewItem.Constants
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
            public const string FailedToAddGloConEventError = "FailedToAddGloConEventError";
            public const string InvalidCustomerFriendlyDescription = "InvalidCustomerFriendlyDescription";
            public const string FailedToFinalizeItemsError = "FailedToFinalizeItemsError";
        }

        public static class Details
        {
            public const string PluDoesNotExistInIconError = "PLUs does not exist in Icon, meaning that the PLU was created in IRMA first.";
            public const string UnexpectedProcessingError = "An unexpected error occurred while processing the items. Error Details: {0}";
            public const string FailedToAddItemsToIcon = "An unexpected error occurred when attempting to add an IRMA Item Subscription in Icon. Error Details: {0}";
             public const string FailedToAddGloConEventError = "An unexpected error occurred when attempting to add an item event for the Global Event Controller. Error Details: {0}";
            public static readonly string InvalidBrand = "The item's Brand '{0}' does not exist in  Please choose a different Brand.";
            public static readonly string InvalidTaxClassCode = "The item's Tax Class has a Tax Class Code '{PropertyValue}' which does not exist in  Please choose a different Tax Class .";
            public static readonly string InvalidNationalClassCode = "The item's National Class has a National Class Code '{PropertyValue}' which does not exist in  Please choose a different National Class.";
            public static readonly string InvalidProductDescription = "Product Description has invalid value '{PropertyValue}'." +
                " Product Description is required and must be less than " + LengthConstants.ProductDescriptionMaxLength + " characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @. Please remove the invalid characters and refresh the item.";
            public static readonly string InvalidPosDescription = "POS Description has invalid value '{PropertyValue}'." +
                " POS Description is requried and must be less than " + LengthConstants.PosDescriptionMaxLength + " characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @. Please remove the invalid characters and refresh the item.";
            public static readonly string InvalidRetailUom = "Retail UOM has invalid value '{PropertyValue}'." + " Retail UOM is required and must be one of the following: " + string.Join(",", UomCodes.ByName.Values) + ". Please remove the invalid UOM and refresh the item.";
            public static readonly string InvalidCustomerFriendlyDescription = "Customer Friendly Description has invalid value '{PropertyValue}'." + " Maximum length is " + LengthConstants.CustomerFriendlyDescriptionMaxLength + " characters.";
        }
    }
}
