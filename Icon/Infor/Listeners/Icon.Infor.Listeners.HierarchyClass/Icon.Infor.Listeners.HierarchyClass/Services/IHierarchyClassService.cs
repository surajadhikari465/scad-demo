using System.Collections.Generic;
using Icon.Infor.Listeners.HierarchyClass.Models;

namespace Icon.Infor.Listeners.HierarchyClass
{
    public interface IHierarchyClassService
    {
        void ProcessHierarchyClassMessages(IEnumerable<HierarchyClassModel> hierarchyClasses);
    }
}