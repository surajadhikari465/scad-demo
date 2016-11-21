using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass
{
    public class AddOrUpdateHierarchyClassesService : IHierarchyClassService
    {
        private ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler;

        public AddOrUpdateHierarchyClassesService(
            ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler)
        {
            this.addOrUpdateHierarchyClassesCommandHandler = addOrUpdateHierarchyClassesCommandHandler;
        }

        public void ProcessHierarchyClassMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            var addUpdateMessages = GetAddOrUpdateMessages(hierarchyClasses);

            if (addUpdateMessages.Any())
            {
                addOrUpdateHierarchyClassesCommandHandler.Execute(
                    new AddOrUpdateHierarchyClassesCommand { HierarchyClasses = addUpdateMessages });
            }
        }

        private IEnumerable<InforHierarchyClassModel> GetAddOrUpdateMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            return hierarchyClasses.Where(hc => hc.Action == ActionEnum.AddOrUpdate && hc.ErrorCode == null);
        }
    }
}
