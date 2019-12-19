using System;
using System.Runtime.Serialization;

namespace OOS.Model
{
    public class KnownUploadValidationException : Exception
    {
        public KnownUploadValidationException()
        {
        }

        public KnownUploadValidationException(string message) : base(message)
        {
        }

        public KnownUploadValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public KnownUploadValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
