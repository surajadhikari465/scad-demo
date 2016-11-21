using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.HierarchyClass.Notifier
{
    public interface IHierarchyClassListenerNotifier
    {
        void NotifyOfError(IEsbMessage message, List<InforHierarchyClassModel> hierarchyClassModelsWithErrors);
    }
}
