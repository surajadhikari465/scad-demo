using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OOSCommon
{
    public class InvalidScanDateException : Exception
    {
        public InvalidScanDateException()
        {
        }

        public InvalidScanDateException(string message) : base(message)
        {
        }

        public InvalidScanDateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidScanDateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
