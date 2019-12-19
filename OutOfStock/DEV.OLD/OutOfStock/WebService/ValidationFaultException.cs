using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OutOfStock.WebService
{
    [DataContract]
    public class ValidationFaultException
    {
        [DataMember(Name = "Message")]
        private string message;

        public ValidationFaultException(string error)
        {
            this.message = error;
        }

        public string Message { get { return message; } }
    }
}