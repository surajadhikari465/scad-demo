
namespace MammothR10Price
{
    internal class Constants
    {
        public struct MessageProperty
        {
            public const string TransactionId = "TransactionID";
            public const string CorrelationId = "CorrelationID";
            public const string SequenceId = "SequenceID";
            public const string TransactionType = "TransactionType";
            public const string Source = "Source";
            public const string ResetFlag = "ResetFlag";
            public const string nonReceivingSysName = "nonReceivingSysName";
            public const string MammothMessageId = "MammothMessageID";
            public const string MessageId = "MessageID";
        }

        public struct PriceType
        {
            public const string RWD = "RWD";
            public const string TPR = "TPR";
            public const string REG = "REG";
            public const string PMI = "PMI";
            public const string PMD = "PMD";
        }

        public struct Description
        {
            public const string Reward = "Reward";
            public const string Rewards = "Rewards";
            public const string TemporaryPriceReduction = "Temperorary Price Reduction";
            public const string RegularPrice = "Regular Price";
            public const string PrimeMemberIncremental = "Prime Member Incremental";
            public const string PrimeMemberDeal = "Prime Member Deal";
        }

        public struct Source
        {
            public const string Mammoth = "Mammoth";
        }

        public struct Action
        {
            public const string DELETE = "DELETE";
        }
        public struct ErrorSeverity
        {
            public const string Fatal = "Fatal";
            public const string Error = "Error";
        }
    }
}
