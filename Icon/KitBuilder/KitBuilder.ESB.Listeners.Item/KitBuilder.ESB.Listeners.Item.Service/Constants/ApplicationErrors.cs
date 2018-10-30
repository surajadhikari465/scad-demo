namespace KitBuilder.ESB.Listeners.Item.Service.Constants
{
    public static class ApplicationErrors
    {
        public static class Codes
        {
            public const string UnableToParseItem = "UnableToParseItem";
            public const string UnableToParseMessage = "UnableToParseMessage";
            public const string UnableToArchiveMessage = "UnableToArchiveMessage";
            public const string ItemAddOrUpdateError = "ItemAddOrUpdateError";
            public const string GenerateItemMessagesError = "GenerateItemMessagesError";
            public const string UnableToArchiveItems = "UnableToArchiveItems";
        }

        public static class Messages
        {
            public const string UnableToParseItem = "Unable to parse item from message.";
            public const string UnableToParseMessage = "Failed to parse Infor Item message.";
            public const string UnableToArchiveMessage = "An unexpected exception occurred which caused a failure to archive message from Infor.";
            public const string ItemAddOrUpdateError = "An unexpected exception occurred which caused a failure to add or update the item.";
            public const string GenerateItemMessagesError = "An unexpected exception occurred which caused a failure to generate messages to downstream systems such as IRMA, SLAW, R10, and Mammoth.";
            public const string UnableToArchiveItems = "An unexpected exception occurred which caused a failure to archive the specific item data sent from Infor.";
        }
    }
}