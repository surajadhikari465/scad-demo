using Icon.Common.CustomExceptions;
using System;

namespace Icon.Web.DataAccess.Infrastructure
{
    public class HierarchyClassTraitUpdateException : IconBaseException
    {
        public HierarchyClassTraitUpdateException()
        {
        }

        public HierarchyClassTraitUpdateException(string message)
            : base(message)
        {
        }

        public HierarchyClassTraitUpdateException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public HierarchyClassTraitUpdateException(string message, string target, string action, string cause, Exception inner)
            : base(message, target, action, cause, inner)
        {
        }
    }
}
