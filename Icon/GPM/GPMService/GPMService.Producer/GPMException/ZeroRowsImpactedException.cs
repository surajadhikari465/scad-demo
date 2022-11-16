using System;

namespace GPMService.Producer.GPMException
{
    [Serializable]
    internal class ZeroRowsImpactedException : Exception
    {
        public ZeroRowsImpactedException() { }
        public ZeroRowsImpactedException(string message) : base(message) { }
        public ZeroRowsImpactedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
