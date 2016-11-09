using Icon.Common.CustomExceptions;
using System;

namespace PushController.Common.Exceptions
{
    public class BusinessUnitNotFoundException : IconBaseException
    {
        public BusinessUnitNotFoundException() { }

        public BusinessUnitNotFoundException(string message) : base(message) { }

        public BusinessUnitNotFoundException(string message, Exception inner) : base(message, inner) { }
        public BusinessUnitNotFoundException(string message, string target, string action, string cause, Exception inner)
            : base(message, target, action, cause, inner) { }
    }
}
