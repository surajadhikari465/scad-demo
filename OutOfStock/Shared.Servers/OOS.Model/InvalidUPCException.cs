using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OOS.Model
{
    public class InvalidUPCException : Exception
    {
        public InvalidUPCException()
        {
        }

        public InvalidUPCException(string message) : base(message)
        {
        }

        public InvalidUPCException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidUPCException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
