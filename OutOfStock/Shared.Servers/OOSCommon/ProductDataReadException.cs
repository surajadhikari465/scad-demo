using System;
using System.Runtime.Serialization;

namespace OOSCommon
{
    public class ProductDataReadException : Exception
    {
        public ProductDataReadException()
        {
        }

        public ProductDataReadException(string message) : base(message)
        {
        }

        public ProductDataReadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ProductDataReadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
