using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.ErrorUtility
{
    public class ErrorMapper : IErrorMapper
    {
        public string GetAssociatedErrorCode(Type type)
        {
            if(type == typeof(AddOrUpdateHierarchyClassesCommandHandler))
            {
                return ApplicationErrors.Codes.AddOrUpdateHierarchyClassError;
            }
            else if(type == typeof(DeleteHierarchyClassesCommandHandler))
            {
                return ApplicationErrors.Codes.DeleteHierarchyClassError;
            }
            else
            {
                return ApplicationErrors.Codes.UnexpectedError;
            }
        }

        public string GetFormattedErrorDetails(Type type, Exception exception)
        {
            if (type == typeof(AddOrUpdateHierarchyClassesCommandHandler))
            {
                return $"{ApplicationErrors.Descriptions.AddOrUpdateHierarchyClassError} Exception Details: {exception.ToString()}";
            }
            else if (type == typeof(DeleteHierarchyClassesCommandHandler))
            {
                return $"{ApplicationErrors.Descriptions.DeleteHierarchyClassError} Exception Details: {exception.ToString()}";
            }
            else
            {
                return $"{ApplicationErrors.Descriptions.UnexpectedError} Exception Details: {exception.ToString()}";
            }
        }
    }
}
