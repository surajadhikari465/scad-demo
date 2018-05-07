namespace PrimeAffinityController.Constants
{
    public static class ApplicationConstants
    {
        public const int MessageBatchSize = 100;
        public const int SqlTimeoutExceptionNumber = 2;

        public static class Errors
        {
            public static class Codes
            {
                public static string FailedToSendToEsb = "FailedToSendToEsb";
            }
        }
        public static class JobScheduleStatuses
        {
            public const string Ready = "ready";
            public const string Running = "running";
            public const string Failed = "failed";
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
