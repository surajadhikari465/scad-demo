using System;

namespace GPMService.Producer.GPMException
{
    [Serializable]
    internal class ActionNotSuppliedException : Exception
    {
        public ActionNotSuppliedException() { }
        public ActionNotSuppliedException(string message) : base(message) { }
        public ActionNotSuppliedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
