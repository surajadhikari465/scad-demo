using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OOS.Model
{
    public class InvalidStoreException : Exception
    {
        public InvalidStoreException()
        {
        }

        public InvalidStoreException(string message) : base(message)
        {
        }

        public InvalidStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidStoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
