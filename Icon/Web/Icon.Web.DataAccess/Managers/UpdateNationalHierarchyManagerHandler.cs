using AutoMapper;
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

        public UpdateNationalHierarchyManagerHandler(
            IconContext context,
            IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>> getNationalClassQuery,
            ICommandHandler<UpdateNationalHierarchyCommand> updateNationalHierarchyCommandHandler,
            ICommandHandler<UpdateNationalHierarchyTraitsCommand> updateNationalHierarchyClassTraitsCommandHandler,
            ICommandHandler<AddVimEventCommand> addVimHierarchyClassEventCommandHandler
            )
        {
            this.context = context;
            this.getNationalClassQuery = getNationalClassQuery;
            this.updateNationalHierarchyCommandHandler = updateNationalHierarchyCommandHandler;
            this.updateNationalHierarchyClassTraitsCommandHandler = updateNationalHierarchyClassTraitsCommandHandler;
            this.addVimHierarchyClassEventCommandHandler = addVimHierarchyClassEventCommandHandler;
        }

        public void Execute(UpdateNationalHierarchyManager data)
        {
            UpdateNationalHierarchyCommand updateNationalHierarchyCommand = new UpdateNationalHierarchyCommand() { NationalHierarchy = data.NationalHierarchy }; ;
            Validate(data);
            
            using (var transaction = this.context.Database.BeginTransaction())
            {

                try
                {
                    updateNationalHierarchyCommandHandler.Execute(updateNationalHierarchyCommand);
                    UpdateNationalHierarchyTraitsCommand updateNationalHierarchyClassTraitCommand = new UpdateNationalHierarchyTraitsCommand() { NationalHierarchy = data.NationalHierarchy, TraitCode = TraitCodes.NationalClassCode, TraitValue = data.NationalClassCode };
                    updateNationalHierarchyClassTraitsCommandHandler.Execute(updateNationalHierarchyClassTraitCommand);
                    updateNationalHierarchyClassTraitCommand = new UpdateNationalHierarchyTraitsCommand() { NationalHierarchy = data.NationalHierarchy, TraitCode = TraitCodes.ModifiedUser, TraitValue = data.UserName };
                    updateNationalHierarchyClassTraitsCommandHandler.Execute(updateNationalHierarchyClassTraitCommand);
                    updateNationalHierarchyClassTraitCommand = new UpdateNationalHierarchyTraitsCommand() { NationalHierarchy = data.NationalHierarchy, TraitCode = TraitCodes.ModifiedDate, TraitValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture) };
                    updateNationalHierarchyClassTraitsCommandHandler.Execute(updateNationalHierarchyClassTraitCommand);
                    addVimHierarchyClassEventCommandHandler.Execute(new AddVimEventCommand { EventReferenceId = updateNationalHierarchyCommand.NationalHierarchy.hierarchyClassID, EventTypeId = VimEventTypes.Nationalclassupdate, EventMessage = updateNationalHierarchyCommand.NationalHierarchy.hierarchyClassName });
                    
                    transaction.Commit();
                }
                catch (HierarchyClassTraitUpdateException exception)
                {
                    transaction.Rollback();
                    throw new CommandException(exception.Message, exception);
                }
                catch (ArgumentException exception)
                {
                    transaction.Rollback();
                    throw new CommandException(exception.Message, exception);
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new CommandException(String.Format("There was an error updating HierarchyClassID {0}.  Error: {1}",
                        data.NationalHierarchy.hierarchyClassID, exception.Message), exception);
                }
            }
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

            //Check for duplicate calss code 
            if(data.NationalHierarchy.hierarchyLevel == HierarchyLevels.NationalClass)
            {
                var nationalClass = getNationalClassQuery.Search(new GetNationalClassByClassCodeParameters() { ClassCode = data.NationalClassCode });
                var duplicates = nationalClass.Where(hc => hc.hierarchyClassID != data.NationalHierarchy.hierarchyClassID);
                if (duplicates.Any())
                {
                    throw new DuplicateValueException(String.Format(@"Error: The class code ""{0}"" is already in use by other national calss", data.NationalClassCode));
                }
            }
        }
    }
}
