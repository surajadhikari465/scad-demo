using Icon.Esb.Services.ConfirmationBod;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.HierarchyClass.Notifier
{
    public interface IHierarchyClassListenerNotifier
    {
        void NotifyOfError(IEsbMessage message, ConfirmationBodEsbErrorTypes errorType , List<InforHierarchyClassModel> hierarchyClassModelsWithErrors);
    }
}
