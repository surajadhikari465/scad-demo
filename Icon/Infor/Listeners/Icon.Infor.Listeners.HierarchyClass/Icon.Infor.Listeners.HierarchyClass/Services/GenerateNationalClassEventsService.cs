using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Services
{
    public class GenerateNationalClassEventsService : IHierarchyClassService
    {
        private ICommandHandler<GenerateNationalClassEventsCommand> generateNationalClassEventsCommandHandler;

        public GenerateNationalClassEventsService(
            ICommandHandler<GenerateNationalClassEventsCommand> generateNationalClassEventsCommandHandler)
        {
            this.generateNationalClassEventsCommandHandler = generateNationalClassEventsCommandHandler;
        }

        public void ProcessHierarchyClassMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            var nationalClassMessages = GetNationalClassMessages(hierarchyClasses);

            if (nationalClassMessages.Any())
            {
                generateNationalClassEventsCommandHandler.Execute(
                    new GenerateNationalClassEventsCommand { HierarchyClasses = nationalClassMessages });
            }
        }

        private IEnumerable<InforHierarchyClassModel> GetNationalClassMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            return hierarchyClasses.Where(hc => hc.HierarchyName == HierarchyNames.National && hc.ErrorCode == null);
        }
    }
}
