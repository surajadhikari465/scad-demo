using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.Common
{
    public struct EsbMessageSendResult
    {
        public EsbMessageSendResult(int numberOfRecordsSent, int numberOfMessagesSent)
        {
            NumberOfRecordsSent = numberOfRecordsSent;
            NumberOfMessagesSent = numberOfMessagesSent;
        }

        public int NumberOfRecordsSent;
        public int NumberOfMessagesSent;
    }
}
