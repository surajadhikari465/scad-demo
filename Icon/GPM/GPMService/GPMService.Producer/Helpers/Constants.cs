namespace GPMService.Producer.Helpers
{
    internal class Constants
    {
        public struct ProducerType
        {
            public const string NearRealTime = "NearRealTime";
            public struct JustInTime
            {
                public const string ActivePrice = "ActivePrice";
                public const string ExpiringTpr = "ExpiringTpr";
                public const string EmergencyPrice = "EmergencyPrice";
            }
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
            public const string RegionCode = "RegionCode";
            public const string Region = "Region";
            public const string ApproximateReceiveCount = "ApproximateReceiveCount";
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
            public const string MammothPriceMessageArchive = "http://schemas.wfm.com/Mammoth/PriceMessageArchive.xsd";
            public const string MammothMammothPrices = "http://schemas.wfm.com/Mammoth/MammothPrices.xsd";
        }
        public struct PriceActions
        {
            public const string Add = "Add";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string AddOrUpdate = "AddOrUpdate";
        }
        public struct ResetFlagValues
        {
            public const string ResetFlagTrueValue = "1";
            public const string ResetFlagFalseValue = "0";
        }
        public struct Sources
        {
            public const string NearRealTimeSource = "Infor";
            public const string JustInTimeSource = "Mammoth";
        }
        public struct TransactionTypes
        {
            public const string Price = "Price";
            public const string ExpiringTprs = "Expirng Tprs";
        }
        public struct ErrorSeverity
        {
            public const string Fatal = "Fatal";
            public const string Error = "Error";
        }
    }
}
