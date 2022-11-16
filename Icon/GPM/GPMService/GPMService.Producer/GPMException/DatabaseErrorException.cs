using System;

namespace GPMService.Producer.GPMException
{
    [Serializable]
    internal class DatabaseErrorException : Exception
    {
        public DatabaseErrorException() { }
        public DatabaseErrorException(string message) : base(message) { }
        public DatabaseErrorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
