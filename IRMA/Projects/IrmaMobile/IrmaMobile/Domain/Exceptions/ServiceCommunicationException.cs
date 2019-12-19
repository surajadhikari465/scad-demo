using System;

namespace IrmaMobile.Domain.Exceptions
{
    /// <summary>
    /// Composite exception type covering exceptions encountered relating to WCF service communication
    /// </summary>
    public sealed class ServiceCommunicationException : Exception
    {
        public ServiceCommunicationException(string message, Exception innerException) :
            base(message, innerException)
        { }

        public ServiceCommunicationException(string message) : base(message) { }
    }
}