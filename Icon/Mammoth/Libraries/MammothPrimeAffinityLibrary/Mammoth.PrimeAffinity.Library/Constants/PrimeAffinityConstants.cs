namespace Mammoth.PrimeAffinity.Library.Constants
{
    public static class PrimeAffinityConstants
    {
        public const int MessageBatchSize = 100;
        public static class Errors
        {
            public static class Codes
            {
                public const string FailedToSendToEsb = "FailedToSendToEsb";
            }
        }
        public static class Esb
        {
            public const string SourceMessageHeaderKey = "Source";
            public const string SourceMessageHeaderValue = "Mammoth";
            public const string IconMessageIdMessageHeaderKey = "IconMessageID";
            public const string NonReceivingSystemsMessageHeaderKey = "nonReceivingSysName";
            public const string MessageActionAddOrUpdate = "AddOrUpdate";
            public const string MessageActionDelete = "Delete";
        }
    }
}
