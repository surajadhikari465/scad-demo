namespace GPMService.Producer.Helpers
{
    internal class Constants
    {
        public struct ProducerType
        {
            public const string NearRealTime = "NearRealTime";
        }
        public struct MessageHeaders
        {
            public const string NearRealTime = "NearRealTime";
            public const string CorrelationID = "CorrelationID";
            public const string TransactionID = "TransactionID";
            public const string TransactionType = "TransactionType";
            public const string ResetFlag = "ResetFlag";
            public const string Source = "Source";
            public const string nonReceivingSysName = "nonReceivingSysName";
            public const string SequenceID = "SequenceID";
        }
        public struct JMSMessageHeaders
        {
            public const string JMSXDeliveryCount = "JMSXDeliveryCount";
        }
        public struct ErrorCodes
        {
            public const string OutOfSequenceRedelivery = "OutOfSequenceRedelivery";
        }
    }
}
