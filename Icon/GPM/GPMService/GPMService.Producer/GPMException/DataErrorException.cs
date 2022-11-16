using System;

namespace GPMService.Producer.GPMException
{
    [Serializable]
    internal class DataErrorException : Exception
    {
        public DataErrorException() { }
        public DataErrorException(string message) : base(message) { }
        public DataErrorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
