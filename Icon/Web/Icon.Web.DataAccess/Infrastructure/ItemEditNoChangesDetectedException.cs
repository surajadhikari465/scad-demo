using Icon.Common.CustomExceptions;
using System;

namespace Icon.Web.DataAccess.Infrastructure
{
    public class ItemEditNoChangesDetectedException : IconBaseException
    {
        public ItemEditNoChangesDetectedException()
        {
        }

        public ItemEditNoChangesDetectedException(string message)
            : base(message)
        {
        }

        public ItemEditNoChangesDetectedException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public ItemEditNoChangesDetectedException(string message, string target, string action, string cause, Exception inner)
            : base(message, target, action, cause, inner)
        {
        }
    }
}
