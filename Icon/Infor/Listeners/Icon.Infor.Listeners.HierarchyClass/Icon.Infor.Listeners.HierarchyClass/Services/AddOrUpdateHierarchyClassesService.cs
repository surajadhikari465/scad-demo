using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Services
{
    public class AddOrUpdateHierarchyClassesService : IHierarchyClassService
    {
        private IHierarchyClassListenerSettings settings;
        private ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler;
        private ICommandHandler<GenerateHierarchyClassEventsCommand> generateHierarchyClassEventsCommandHandler;
        private ICommandHandler<GenerateHierarchyClassMessagesCommand> generateHierarchyClassMessagesCommandHandler;

        public AddOrUpdateHierarchyClassesService(
            IHierarchyClassListenerSettings settings,
            ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler,
            ICommandHandler<GenerateHierarchyClassEventsCommand> generateHierarchyClassEventsCommandHandler,
            ICommandHandler<GenerateHierarchyClassMessagesCommand> generateHierarchyClassMessagesCommandHandler)
        {
            this.settings = settings;
            this.addOrUpdateHierarchyClassesCommandHandler = addOrUpdateHierarchyClassesCommandHandler;
            this.generateHierarchyClassEventsCommandHandler = generateHierarchyClassEventsCommandHandler;
            this.generateHierarchyClassMessagesCommandHandler = generateHierarchyClassMessagesCommandHandler;
        }

        public void ProcessHierarchyClassMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            var addUpdateMessages = GetAddOrUpdateMessages(hierarchyClasses);

            if (addUpdateMessages.Any())
            {
                addOrUpdateHierarchyClassesCommandHandler.Execute(
                    new AddOrUpdateHierarchyClassesCommand { HierarchyClasses = addUpdateMessages });

                //only generate events if allowed by hierarchy class type and settings,
                // and for messages which were successfully added/updated
                if (ShouldGenerateEvents(addUpdateMessages.First().HierarchyName, settings) 
                    && addUpdateMessages.Any(hc => hc.ErrorCode == null))
                {
                    // generate events for the global controller to send to IRMA
                    generateHierarchyClassEventsCommandHandler.Execute(
                        new GenerateHierarchyClassEventsCommand
                        {
                            HierarchyClasses = addUpdateMessages.Where(hc => hc.ErrorCode == null)
                        });
                }

                if (ShouldGenerateMessages(addUpdateMessages.First().HierarchyName)
                    && addUpdateMessages.Any(hc => hc.ErrorCode == null))
                {
                    generateHierarchyClassMessagesCommandHandler.Execute(
                        new GenerateHierarchyClassMessagesCommand
                        {
                            HierarchyClasses = addUpdateMessages.Where(hc => hc.ErrorCode == null)
                        });
                }
            }
        }

        private IEnumerable<InforHierarchyClassModel> GetAddOrUpdateMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            return hierarchyClasses.Where(hc => hc.Action == ActionEnum.AddOrUpdate && hc.ErrorCode == null);
        }

        private bool ShouldGenerateEvents(string hierarchyName, IHierarchyClassListenerSettings settings)
        {
            switch (hierarchyName)
            {
                case HierarchyNames.National:
                case HierarchyNames.Brands:
                    //always generate event for a brand and national add/update
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
                case HierarchyNames.National:
                    return settings.EnableNationalClassMessageGeneration;
                default:
                    return false;
            }
        }
    }
}
