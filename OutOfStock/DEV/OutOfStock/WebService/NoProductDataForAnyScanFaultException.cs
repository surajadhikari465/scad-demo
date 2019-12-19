using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace OutOfStock.WebService
{
    [DataContract]
    public class NoProductDataForAnyScanFaultException
    {
        [DataMember(Name = "Message")] 
        private string message;

        public NoProductDataForAnyScanFaultException(string message)
        {
            this.message = message;
        }

        public string Message { get { return message; } }
    }
}