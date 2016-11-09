using Icon.Common.CustomExceptions;
using System;

namespace Icon.Web.DataAccess.Infrastructure
{
    public class DuplicateValueException : IconBaseException
    {
        public DuplicateValueException()
        {
        }

        public DuplicateValueException(string message)
            : base(message)
        {
        }

        public DuplicateValueException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public DuplicateValueException(string message, string target, string action, string cause, Exception inner)
            : base(message, target, action, cause, inner)
        {
        }
    }
}
