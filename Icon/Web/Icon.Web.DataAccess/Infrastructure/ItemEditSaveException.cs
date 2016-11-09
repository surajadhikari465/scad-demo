using Icon.Common.CustomExceptions;
using System;

namespace Icon.Web.DataAccess.Infrastructure
{
    public class ItemEditSaveException : IconBaseException
    {
        public ItemEditSaveException()
        {
        }

        public ItemEditSaveException(string message)
            : base(message)
        {
        }

        public ItemEditSaveException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public ItemEditSaveException(string message, string target, string action, string cause, Exception inner)
            : base(message, target, action, cause, inner)
        {
        }
    }
}
