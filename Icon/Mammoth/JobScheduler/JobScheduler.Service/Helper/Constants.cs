namespace JobScheduler.Service.Helper
{
    internal class Constants
    {
        public struct XmlNamespaces
        {
            public const string MammothJobSchedule = "http://schemas.wfm.com/Mammoth/JobSchedule.xsd";
        }

        public struct ConsumerServices
        {
            public const string ExtractService = "ExtractService";
            public const string ActivePriceService = "ActivePriceService";
            public const string ExpiringTprService = "ExpiringTprService";
        }

        public struct ConsumerQueues
        {
            public const string ExtractServiceQueue = "WFMSB1.SCAD.Audit.ExtractService.Queue.V1";
            public const string ActivePriceServiceQueue = "WFMSB1.Mammoth.Retail.JustInTime.PricePushStarter.Queue.V1";
            public const string ExpiringTprServiceQueue = "WFMSB1.Mammoth.Retail.JustInTime.InactiveTprPriceStarter.Queue.V1";
        }

        public struct ConsumerS3Buckets
        {
            public const string ExtractServiceBucketName = "jobscheduler-extractservice";
            public const string ActivePriceServiceBucketName = "jobscheduler-activepriceservice";
            public const string ExpiringTprServiceBucketName = "jobscheduler-expiringtprservice";
        }
        public struct ErrorSeverity
        {
            public const string Fatal = "Fatal";
            public const string Error = "Error";
        }
    }
}
