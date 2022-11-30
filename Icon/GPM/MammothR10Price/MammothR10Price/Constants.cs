using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public const string NonReceivingSystems = "NonReceivingSystems";
            public const string MammothMessageId = "MammothMessageID";
        }

        public struct Source
        {
            public const string Mammoth = "Mammoth";
        }
    }
}
