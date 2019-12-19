using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Queries;
using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateNationalHierarchyManagerHandler : IManagerHandler<UpdateNationalHierarchyManager>
    {
        private IconContext context;
        private ICommandHandler<UpdateNationalHierarchyCommand> updateNationalHierarchyCommandHandler;
        private ICommandHandler<UpdateNationalHierarchyTraitsCommand> updateNationalHierarchyClassTraitsCommandHandler;
        private ICommandHandler<AddVimEventCommand> addVimHierarchyClassEventCommandHandler;
        private IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>> getNationalClassQuery;
        private ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler;
        private AppSettings settings;

        public UpdateNationalHierarchyManagerHandler(
            IconContext context,
            IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>> getNationalClassQuery,
            ICommandHandler<UpdateNationalHierarchyCommand> updateNationalHierarchyCommandHandler,
            ICommandHandler<UpdateNationalHierarchyTraitsCommand> updateNationalHierarchyClassTraitsCommandHandler,
            ICommandHandler<AddVimEventCommand> addVimHierarchyClassEventCommandHandler,
            ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler,
            AppSettings settings
            )
        {
            this.context = context;
            this.getNationalClassQuery = getNationalClassQuery;
            this.updateNationalHierarchyCommandHandler = updateNationalHierarchyCommandHandler;
            this.updateNationalHierarchyClassTraitsCommandHandler = updateNationalHierarchyClassTraitsCommandHandler;
            this.addVimHierarchyClassEventCommandHandler = addVimHierarchyClassEventCommandHandler;
            this.addHierarchyClassMessageHandler = addHierarchyClassMessageHandler;
            this.settings = settings;
        }

        public void Execute(UpdateNationalHierarchyManager data)
        {
            UpdateNationalHierarchyCommand updateNationalHierarchyCommand = new UpdateNationalHierarchyCommand() { NationalHierarchy = data.NationalHierarchy }; ;
            Validate(data);
            AddHierarchyClassMessageCommand addHierarchyClassMessageCommand = new AddHierarchyClassMessageCommand()
            {
                HierarchyClass = data.NationalHierarchy,
                ClassNameChange = true,
                NationalClassCode = data.NationalClassCode
            };

            updateNationalHierarchyCommandHandler.Execute(updateNationalHierarchyCommand);
            UpdateNationalHierarchyTraitsCommand updateNationalHierarchyClassTraitCommand = new UpdateNationalHierarchyTraitsCommand() { NationalHierarchy = data.NationalHierarchy, TraitCode = TraitCodes.NationalClassCode, TraitValue = data.NationalClassCode };
            updateNationalHierarchyClassTraitsCommandHandler.Execute(updateNationalHierarchyClassTraitCommand);
            updateNationalHierarchyClassTraitCommand = new UpdateNationalHierarchyTraitsCommand() { NationalHierarchy = data.NationalHierarchy, TraitCode = TraitCodes.ModifiedUser, TraitValue = data.UserName };
            updateNationalHierarchyClassTraitsCommandHandler.Execute(updateNationalHierarchyClassTraitCommand);
            updateNationalHierarchyClassTraitCommand = new UpdateNationalHierarchyTraitsCommand() { NationalHierarchy = data.NationalHierarchy, TraitCode = TraitCodes.ModifiedDate, TraitValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture) };
            updateNationalHierarchyClassTraitsCommandHandler.Execute(updateNationalHierarchyClassTraitCommand);
            addVimHierarchyClassEventCommandHandler.Execute(new AddVimEventCommand { EventReferenceId = updateNationalHierarchyCommand.NationalHierarchy.hierarchyClassID, EventTypeId = VimEventTypes.Nationalclassupdate, EventMessage = updateNationalHierarchyCommand.NationalHierarchy.hierarchyClassName });

            GenerateHierarchyUpdateEvents(data.NationalHierarchy.hierarchyClassID, data.NationalHierarchy.hierarchyClassName, EventTypeNames.NationalClassUpdate);
            addHierarchyClassMessageHandler.Execute(addHierarchyClassMessageCommand);
        }

        private void Validate(UpdateNationalHierarchyManager data)
        {
            var nationalClassToUpdate = context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.NationalHierarchy.hierarchyClassID);

            if (nationalClassToUpdate.hierarchyClassName != data.NationalHierarchy.hierarchyClassName ||
                data.NationalHierarchy.hierarchyLevel == HierarchyLevels.NationalClass)
            {
                var duplicateHierarchyClasses = context.HierarchyClass.ContainsDuplicateNationalClass(data.NationalHierarchy.hierarchyClassName,
               nationalClassToUpdate.hierarchyLevel, nationalClassToUpdate.hierarchyID, nationalClassToUpdate.hierarchyClassID, data.NationalClassCode, data.NationalHierarchy.hierarchyParentClassID, true);

                if (duplicateHierarchyClasses)
                {
                    throw new DuplicateValueException(String.Format(@"Error: The name ""{0}"" is already in use at this level of the hierarchy.", data.NationalHierarchy.hierarchyClassName));
                }
            }

            //Check for duplicate class code 
            var nationalClass = getNationalClassQuery.Search(new GetNationalClassByClassCodeParameters() { ClassCode = data.NationalClassCode });
            var duplicates = nationalClass.Where(hc => hc.hierarchyClassID != data.NationalHierarchy.hierarchyClassID);
            if (duplicates.Any())
            {
                throw new DuplicateValueException(String.Format(@"Error: The class code ""{0}"" is already in use by other national class", data.NationalClassCode));
            }
        }

        private void GenerateHierarchyUpdateEvents(int hierarchyClassId, string hierarchyClassName, string eventName)
        {
            int hierarchyClassUpdateEventId;
            string[] hierarchyClassUpdateConfiguredRegions = settings.HierarchyClassUpdateEventConfiguredRegions;
            var hierarchyUpdateEvents = new List<EventQueue>();

            hierarchyClassUpdateEventId = context.EventType.Single(et => et.EventName == eventName).EventId;

            foreach (string region in hierarchyClassUpdateConfiguredRegions)
            {
                hierarchyUpdateEvents.Add(new EventQueue()
                {
                    EventId = hierarchyClassUpdateEventId,
                    EventMessage = hierarchyClassName,
                    EventReferenceId = hierarchyClassId,
                    RegionCode = region.Trim(),
                    InsertDate = DateTime.Now,
                });
            }

            context.EventQueue.AddRange(hierarchyUpdateEvents);
            context.SaveChanges();
        }
    }
}
