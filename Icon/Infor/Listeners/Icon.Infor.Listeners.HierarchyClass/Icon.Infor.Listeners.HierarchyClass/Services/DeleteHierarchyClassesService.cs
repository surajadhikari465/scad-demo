using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Infor.Listeners.HierarchyClass.Extensions;
using Icon.Framework;

namespace Icon.Infor.Listeners.HierarchyClass.Services
{
    public class DeleteHierarchyClassesService : IHierarchyClassService
    {
        private IHierarchyClassListenerSettings settings;
        private ICommandHandler<DeleteHierarchyClassesCommand> deleteHierarchyClassesCommandHandler;
        private ICommandHandler<GenerateHierarchyClassEventsCommand> generateHierarchyClassEventsCommandHandler;
        private ICommandHandler<GenerateHierarchyClassMessagesCommand> generateHierarchyClassMessagesCommandHandler;

        public DeleteHierarchyClassesService(
            IHierarchyClassListenerSettings settings,
            ICommandHandler<DeleteHierarchyClassesCommand> deleteHierarchyClassesCommandHandler,
            ICommandHandler<GenerateHierarchyClassEventsCommand> generateHierarchyClassEventsCommandHandler)
        {
            this.settings = settings;
            this.deleteHierarchyClassesCommandHandler = deleteHierarchyClassesCommandHandler;
            this.generateHierarchyClassEventsCommandHandler = generateHierarchyClassEventsCommandHandler;
        }

        public void ProcessHierarchyClassMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            var deleteMessages = GetDeleteMessages(hierarchyClasses);

            if (deleteMessages.Any())
            {
                deleteHierarchyClassesCommandHandler.Execute(
                    new DeleteHierarchyClassesCommand { HierarchyClasses = deleteMessages });

                //only generate events if allowed by hierarchy class type and settings,
                // and for messages which were successfully found & deleted
                if (ShouldGenerateEvents(deleteMessages.First().HierarchyName)
                    && deleteMessages.Any(hc => hc.ErrorCode == null))
                {
                    // generate events for the global controller to send to IRMA
                    generateHierarchyClassEventsCommandHandler.Execute(
                        new GenerateHierarchyClassEventsCommand
                        {
                            HierarchyClasses = deleteMessages.Where(hc => hc.ErrorCode == null)
                        });
                }

                if (ShouldGenerateMessages(deleteMessages.First().HierarchyName)
                    && deleteMessages.Any(hc => hc.ErrorCode == null))
                {
                    generateHierarchyClassMessagesCommandHandler.Execute(
                        new GenerateHierarchyClassMessagesCommand
                        {
                            HierarchyClasses = deleteMessages.Where(hc => hc.ErrorCode == null)
                        });
                }
            }
        }

        private IEnumerable<InforHierarchyClassModel> GetDeleteMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            return hierarchyClasses.Where(h => h.Action == ActionEnum.Delete);
        }

        private bool ShouldGenerateEvents(string hierarchyName)
        {
            switch (hierarchyName)
            {
                //always generate events for a National or Brand class delete
                case HierarchyNames.National:
                case HierarchyNames.Brands:
                    return true;
                default:
                    return false;
            }
        }

        private bool ShouldGenerateMessages(string hierarchyName)
        {
            switch (hierarchyName)
            {
                //always generate messages for a Merchandise or Brand class delete
                case HierarchyNames.Merchandise:
                case HierarchyNames.Brands:
                    return true;
                default:
                    return false;
            }
        }
    }
}
