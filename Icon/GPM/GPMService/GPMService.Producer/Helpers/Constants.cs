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
            public const string ActionNotSupplied = "Action Value not supplied.";
            public const string DataError = "Data Error";
            public const string DatabaseError = "Database Error";
            public const string InvalidMessageHeaderError = "InvalidMessageHeader";
            public const string MappingError = "Error In Mapping";
            public const string OutOfSequenceRedelivery = "OutOfSequenceRedelivery";
            public const string RetryError = "RetryError";
            public const string ZeroRowsImpacted = "Zero Rows were Impacted";
        }
        public struct ErrorTypes
        {
            public const string Data = "Data";
            public const string DatabaseConstraint = "DatabaseConstraint";
            public const string Schema = "Schema";
        }
        public struct MessageTypeNames
        {
            public const string ProcessBOD = "ProcessBOD";
            public const string ConfirmBOD = "ConfirmBOD";
        }
        public struct XmlNamespaces
        {
            public const string InforOAGIS = "http://schema.infor.com/InforOAGIS/2";
        }
    }
}
