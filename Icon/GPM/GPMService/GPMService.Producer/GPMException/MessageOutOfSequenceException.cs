using System;

namespace GPMService.Producer.GPMException
{
    [Serializable]
    internal class MessageOutOfSequenceException : Exception
    {
        public MessageOutOfSequenceException() { }
        public MessageOutOfSequenceException(string message) : base(message) { }
        public MessageOutOfSequenceException(string message, Exception innerException) : base(message, innerException) { }

    }
}
