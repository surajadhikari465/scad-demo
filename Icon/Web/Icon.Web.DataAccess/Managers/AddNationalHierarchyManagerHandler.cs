using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Managers
{
    public class AddNationalHierarchyManagerHandler : IManagerHandler<AddNationalHierarchyManager>
    {
        private IconContext context;
        private ICommandHandler<AddNationalHierarchyCommand> addNationalHierarchyCommandHandler;
        private ICommandHandler<AddVimEventCommand> addVimHierarchyEventCommandHandler;
        private IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>> getNationalClassQuery;
        private ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler;
        private AppSettings settings;

        public AddNationalHierarchyManagerHandler(
            IconContext context,
            IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>> getNationalClassQuery,
            ICommandHandler<AddNationalHierarchyCommand> addNationalHierarchyCommandHandler,
            ICommandHandler<AddVimEventCommand> addVimHierarchyEventCommandHandler,
            ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler,
            AppSettings settings
            )
        {
            this.context = context;
            this.getNationalClassQuery = getNationalClassQuery;
            this.addNationalHierarchyCommandHandler = addNationalHierarchyCommandHandler;
            this.addVimHierarchyEventCommandHandler = addVimHierarchyEventCommandHandler;
            this.addHierarchyClassMessageHandler = addHierarchyClassMessageHandler;
            this.settings = settings;
        }

        public void Execute(AddNationalHierarchyManager data)
        {
            Validate(data);

            AddNationalHierarchyCommand addNationalHierarchyCommand = new AddNationalHierarchyCommand()
            {
                NationalHierarchy = data.NationalHierarchy,
                NationalClassCode = data.NationalClassCode,
                UserName = data.UserName
            };

            AddHierarchyClassMessageCommand addHierarchyClassMessageCommand = new AddHierarchyClassMessageCommand()
            {
                HierarchyClass = data.NationalHierarchy,
                ClassNameChange = true,
                NationalClassCode = data.NationalClassCode
            };

            addNationalHierarchyCommandHandler.Execute(addNationalHierarchyCommand);
            addVimHierarchyEventCommandHandler.Execute(new AddVimEventCommand { EventReferenceId = addNationalHierarchyCommand.NationalHierarchy.hierarchyClassID, EventTypeId = VimEventTypes.Nationalclassadd, EventMessage = addNationalHierarchyCommand.NationalHierarchy.hierarchyClassName });
            GenerateHierarchyAddEvents(data.NationalHierarchy.hierarchyClassID, data.NationalHierarchy.hierarchyClassName, EventTypeNames.NationalClassUpdate);
            addHierarchyClassMessageHandler.Execute(addHierarchyClassMessageCommand);
        }

        private void Validate(AddNationalHierarchyManager data)
        {
            var duplicateHierarchyClasses = context.HierarchyClass.ContainsDuplicateNationalClass(data.NationalHierarchy.hierarchyClassName,
                data.NationalHierarchy.hierarchyLevel, data.NationalHierarchy.hierarchyID, data.NationalHierarchy.hierarchyClassID, data.NationalClassCode, data.NationalHierarchy.hierarchyParentClassID, false);

            if (duplicateHierarchyClasses)
            {
                throw new DuplicateValueException(String.Format(@"Error: The name ""{0}"" is already in use at this level of the hierarchy.", data.NationalHierarchy.hierarchyClassName));
            }

            //Check for duplicate class code 
            var duplicateNationalClass = getNationalClassQuery.Search(new GetNationalClassByClassCodeParameters() { ClassCode = data.NationalClassCode });
            if (duplicateNationalClass.Any())
            {
                throw new DuplicateValueException(String.Format(@"Error: The class code ""{0}"" is already in use by other national class", data.NationalClassCode));
            }
        }

        private void GenerateHierarchyAddEvents(int hierarchyClassId, string hierarchyClassName, string eventName)
        {
            int hierarchyClassAddEventId;
            string[] hierarchyClassAddConfiguredRegions = settings.HierarchyClassAddEventConfiguredRegions;
            var hierarchyAddEvents = new List<EventQueue>();

            hierarchyClassAddEventId = context.EventType.Single(et => et.EventName == eventName).EventId;

            foreach (string region in hierarchyClassAddConfiguredRegions)
            {
                var hierarchyAddEvent = new EventQueue();

                hierarchyAddEvent.EventId = hierarchyClassAddEventId;
                hierarchyAddEvent.EventMessage = hierarchyClassName;
                hierarchyAddEvent.EventReferenceId = hierarchyClassId;
                hierarchyAddEvent.RegionCode = region.Trim();
                hierarchyAddEvent.InsertDate = DateTime.Now;

                hierarchyAddEvents.Add(hierarchyAddEvent);
            }

            context.EventQueue.AddRange(hierarchyAddEvents);
            context.SaveChanges();
        }
    }
}
