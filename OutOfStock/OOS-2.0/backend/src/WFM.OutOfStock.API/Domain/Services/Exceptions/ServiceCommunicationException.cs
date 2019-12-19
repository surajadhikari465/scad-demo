using System;
using Microsoft.AspNetCore.Mvc;

namespace WFM.OutOfStock.API.Services.Exceptions
{
    /// <summary>
    /// Composite exception type covering exceptions encountered relating to Out Of Stock WCF service communication
    /// </summary>
    public sealed class ServiceCommunicationException : Exception
    {
        public ServiceCommunicationException(string message, Exception innerException): 
            base(message, innerException) { }

        public ServiceCommunicationException(string message) : base(message) { }
    }
}