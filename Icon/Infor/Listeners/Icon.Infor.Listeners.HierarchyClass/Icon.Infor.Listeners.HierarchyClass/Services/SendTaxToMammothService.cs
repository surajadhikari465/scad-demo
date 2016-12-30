using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Services
{
    public class SendTaxToMammothService : IHierarchyClassService
    {
        private ICommandHandler<AddOrUpdateTaxClassesInMammothCommand> addOrUpdateTaxClassesInMammothCommandHandler;

        public SendTaxToMammothService(
            ICommandHandler<AddOrUpdateTaxClassesInMammothCommand> addOrUpdateTaxClassesInMammothCommandHandler)
        {
            this.addOrUpdateTaxClassesInMammothCommandHandler = addOrUpdateTaxClassesInMammothCommandHandler;
        }

        public void ProcessHierarchyClassMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            var addUpdateMessages = GetAddOrUpdateMessages(hierarchyClasses);

            if (addUpdateMessages.Any())
            {
                addOrUpdateTaxClassesInMammothCommandHandler.Execute(
                    new AddOrUpdateTaxClassesInMammothCommand { TaxHierarchyClasses = addUpdateMessages });
            }
        }

        private IEnumerable<InforHierarchyClassModel> GetAddOrUpdateMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            return hierarchyClasses.Where(hc => hc.Action == ActionEnum.AddOrUpdate && hc.ErrorCode == null && hc.HierarchyName == Hierarchies.Names.Tax);
        }
    }
}
