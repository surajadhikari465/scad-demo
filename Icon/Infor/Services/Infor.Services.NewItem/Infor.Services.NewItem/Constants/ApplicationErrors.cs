using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Constants
{
    public static class ApplicationErrors
    {
        public static class Codes
        {
            public const string UnexpectedProcessingError = "UnexpectedProcessingError";
            public const string FailedToAddItemsToIcon = "FailedToAddItemsToIcon";
            public const string FailedToSendMessageToEsb = "FailedToSendMessageToEsb";
        }
    }
}
