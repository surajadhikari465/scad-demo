using System;
using System.Runtime.Serialization;

namespace OOSCommon
{
    public class NoProductDataForAnyScanException : Exception
    {
        public NoProductDataForAnyScanException()
        {
        }

        public NoProductDataForAnyScanException(string message) : base(message)
        {
        }

        public NoProductDataForAnyScanException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NoProductDataForAnyScanException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
