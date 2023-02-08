using System;

namespace WebSupport.Helpers
{
    public class Constants
    {
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
            public const string MammothGpmJitBucket = "mammoth-gpm-just-in-time-prices";
            public const string ProcessBodBucket = "mammothgpm-bridge-processbod";
        }

        public struct Source
        {
            public const string Infor = "Infor";
        }
    }
}