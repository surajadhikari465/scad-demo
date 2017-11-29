using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.ErrorUtility
{
    public interface IErrorMapper
    {
        string GetAssociatedErrorCode(Type type);
        string GetFormattedErrorDetails(Type type, Exception exception);
    }
}
