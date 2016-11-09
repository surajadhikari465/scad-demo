using Icon.Common.CustomExceptions;
using System;

namespace PushController.Common.Exceptions
{
    public class ScanCodeNotFoundException : IconBaseException
    {
        public ScanCodeNotFoundException() { }

        public ScanCodeNotFoundException(string message) : base(message) { }

        public ScanCodeNotFoundException(string message, Exception inner) : base(message, inner) { }
        public ScanCodeNotFoundException(string message, string target, string action, string cause, Exception inner)
            : base(message, target, action, cause, inner) { }
    }
}
