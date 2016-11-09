using Icon.Common.CustomExceptions;
using System;

namespace Icon.Web.DataAccess.Infrastructure
{
    public class ItemHierarchyClassUpdateException : IconBaseException
    {
        public ItemHierarchyClassUpdateException()
        {
        }

        public ItemHierarchyClassUpdateException(string message)
            : base(message)
        {
        }

        public ItemHierarchyClassUpdateException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public ItemHierarchyClassUpdateException(string message, string target, string action, string cause, Exception inner)
            : base(message, target, action, cause, inner)
        {
        }
    }
}
