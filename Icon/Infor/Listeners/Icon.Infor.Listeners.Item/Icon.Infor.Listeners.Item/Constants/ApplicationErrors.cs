using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Constants
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
        }

        public static class Messages
        {
            public const string UnableToParseItem = "Unable to parse item from message.";
            public const string UnableToParseMessage = "Failed to parse Infor Item message.";
            public const string UnableToArchiveMessage = "An unexpected exception occurred which caused a failure to archive message from Infor.";
            public const string ItemAddOrUpdateError = "An unexpected exception occurred which caused a failure to add or update the item.";
            public const string GenerateItemMessagesError = "An unexpected exception occurred which caused a failure to generate messages to downstream systems such as IRMA, SLAW, R10, and Mammoth.";
        }
    }
}
