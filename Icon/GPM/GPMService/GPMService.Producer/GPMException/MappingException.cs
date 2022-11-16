using System;

namespace GPMService.Producer.GPMException
{
    [Serializable]
    internal class MappingException : Exception
    {
        public MappingException() { }
        public MappingException(string message) : base(message) { }
        public MappingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
