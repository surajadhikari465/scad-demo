using System;

namespace GPMService.Producer.GPMException
{
    [Serializable]
    internal class InvalidMessageHeaderException : Exception
    {
        public InvalidMessageHeaderException() { }
        public InvalidMessageHeaderException(string message) : base(message) { }
        public InvalidMessageHeaderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
