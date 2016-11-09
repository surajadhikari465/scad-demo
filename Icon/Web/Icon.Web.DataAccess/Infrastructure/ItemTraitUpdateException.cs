using Icon.Common.CustomExceptions;
using System;

namespace Icon.Web.DataAccess.Infrastructure
{
    public class ItemTraitUpdateException : IconBaseException
    {
        public ItemTraitUpdateException()
        {
        }

        public ItemTraitUpdateException(string message)
            : base(message)
        {
        }

        public ItemTraitUpdateException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public ItemTraitUpdateException(string message, string target, string action, string cause, Exception inner)
            : base(message, target, action, cause, inner)
        {
        }
    }
}
