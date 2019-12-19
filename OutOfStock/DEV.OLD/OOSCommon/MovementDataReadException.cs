using System;
using System.Runtime.Serialization;

namespace OOSCommon
{
    public class MovementDataReadException : Exception
    {
        public MovementDataReadException()
        {
        }

        public MovementDataReadException(string message) : base(message)
        {
        }

        public MovementDataReadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MovementDataReadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
