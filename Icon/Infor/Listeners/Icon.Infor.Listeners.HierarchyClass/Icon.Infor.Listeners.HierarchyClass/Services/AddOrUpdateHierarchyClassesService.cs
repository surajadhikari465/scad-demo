using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Icon.Infor.Listeners.HierarchyClass.Extensions;

namespace Icon.Infor.Listeners.HierarchyClass
{
    public class AddOrUpdateHierarchyClassesService : IHierarchyClassService
    {
        private IHierarchyClassListenerSettings settings;
        private ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler;
        private ICommandHandler<GenerateHierarchyClassEventsCommand> generateHierarchyClassEventsCommandHandler;

        public AddOrUpdateHierarchyClassesService(
            IHierarchyClassListenerSettings settings,
            ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler,
            ICommandHandler<GenerateHierarchyClassEventsCommand> generateHierarchyClassEventsCommandHandler)
        {
            this.settings = settings;
            this.addOrUpdateHierarchyClassesCommandHandler = addOrUpdateHierarchyClassesCommandHandler;
            this.generateHierarchyClassEventsCommandHandler = generateHierarchyClassEventsCommandHandler;
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
            }
        }

        private IEnumerable<InforHierarchyClassModel> GetAddOrUpdateMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            return hierarchyClasses.Where(hc => hc.Action == ActionEnum.AddOrUpdate && hc.ErrorCode == null);
        }

        internal bool ShouldGenerateEvents(string hierarchyName, IHierarchyClassListenerSettings settings)
        {
            switch (hierarchyName)
            {
                case HierarchyNames.National:
                    //generate an event for a national add/update only if enabled by settings
                    return settings.EnableNationalClassEventGeneration;
                case HierarchyNames.Brands:
                    //always generate event for a brand add/update
                    return true;
                default:
                    return false;
            }
        }
    }
}
