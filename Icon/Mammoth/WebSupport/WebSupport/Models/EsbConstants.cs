using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSupport.Models
{
    public static class EsbConstants
    {
        public const string NonReceivingSystemsKey = "nonReceivingSysName";
        public const string TransactionIdKey = "TransactionID";
        public const string CorrelationIdKey = "CorrelationID";
        public const string TransactionTypeKey = "TransactionType";
        public const string SequenceIdKey = "SequenceID";
        public const string SourceKey = "Source";
        public const string MammothSourceValueName = "Mammoth";
        public const string TagExpirationDateTraitCode = "NTE";
        public const string TagExpirationDateTraitDescription = "Tag Expiration Date";
        public const string CheckPointRequest = "CheckPointRequest";
        public const string PriceResetKey = "ResetFlag";
        public const string PriceTransactionTypeValue = "Price";
        public const string PriceResetTrueValue = "1";
        public const string PriceResetFalseValue = "0";
        public const string CheckPointRequestMaxItemsKey = "MaxItemsForCheckpointReq";

    }
}