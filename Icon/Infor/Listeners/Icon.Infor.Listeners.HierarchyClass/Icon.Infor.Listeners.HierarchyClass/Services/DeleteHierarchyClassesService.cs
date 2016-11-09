using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.HierarchyClass.Services
{
    public class DeleteHierarchyClassesService : IHierarchyClassService
    {
        private ICommandHandler<DeleteHierarchyClassesCommand> deleteHierarchyClassesCommandHandler;

        public DeleteHierarchyClassesService(
            ICommandHandler<DeleteHierarchyClassesCommand> deleteHierarchyClassesCommandHandler)
        {
            this.deleteHierarchyClassesCommandHandler = deleteHierarchyClassesCommandHandler;
        }

        public void ProcessHierarchyClassMessages(IEnumerable<HierarchyClassModel> hierarchyClasses)
        {
            var deleteMessages = GetDeleteMessages(hierarchyClasses);

            if (deleteMessages.Any())
            {
                deleteHierarchyClassesCommandHandler.Execute(
                    new DeleteHierarchyClassesCommand { HierarchyClasses = deleteMessages });
            }
        }

        private IEnumerable<HierarchyClassModel> GetDeleteMessages(IEnumerable<HierarchyClassModel> hierarchyClasses)
        {
            return hierarchyClasses.Where(h => h.Action == ActionEnum.Delete);
        }
    }
}
