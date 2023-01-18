using System;

namespace IrmaPriceListenerService
{
    public class Constants
    {
        public struct MessageAttribute
        {
            public const string TransactionId = "TransactionID";
        }

        public struct PriceType
        {
            public const string Rwd = "RWD";
        }

        public struct Action
        {
            public const string Delete = "DELETE";
        }

        public struct ErrorSeverity
        {
            public const string Fatal = "Fatal";
            public const string Error = "Error";
        }
    }
}
