using AutoMapper;
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

        public AddNationalHierarchyManagerHandler(
            IconContext context,
            IQueryHandler<GetNationalClassByClassCodeParameters, List<HierarchyClass>> getNationalClassQuery,
            ICommandHandler<AddNationalHierarchyCommand> addNationalHierarchyCommandHandler,
            ICommandHandler<AddVimEventCommand> addVimHierarchyEventCommandHandler)
        {
            this.context = context;
            this.getNationalClassQuery = getNationalClassQuery;
            this.addNationalHierarchyCommandHandler = addNationalHierarchyCommandHandler;
            this.addVimHierarchyEventCommandHandler = addVimHierarchyEventCommandHandler;
        }

        public void Execute(AddNationalHierarchyManager data)
        {
            Validate(data);

             using (var transaction = this.context.Database.BeginTransaction())
            {

                AddNationalHierarchyCommand addNationalHierarchyCommand = new AddNationalHierarchyCommand()
                {
                    NationalHierarchy = data.NationalHierarchy,
                    NationalClassCode = data.NationalClassCode,
                    UserName = data.UserName
                };

                try
                {
                    addNationalHierarchyCommandHandler.Execute(addNationalHierarchyCommand);
                    addVimHierarchyEventCommandHandler.Execute(new AddVimEventCommand { EventReferenceId = addNationalHierarchyCommand.NationalHierarchy.hierarchyClassID, EventTypeId = VimEventTypes.Nationalclassadd, EventMessage = addNationalHierarchyCommand.NationalHierarchy.hierarchyClassName });
                    transaction.Commit();
                }
                catch (DuplicateValueException exception)
                {
                    transaction.Rollback();
                    throw new CommandException(exception.Message, exception);
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new CommandException(String.Format("Error adding HierarchyClassName {0}, hierarchyID {1}, parentId {2}.",
                        data.NationalHierarchy.hierarchyClassName, data.NationalHierarchy.hierarchyID, data.NationalHierarchy.hierarchyParentClassID), exception);
                }

            }
        }

        private void Validate(AddNationalHierarchyManager data)
        {
            var duplicateHierarchyClasses = context.HierarchyClass.ContainsDuplicateNationalClass(data.NationalHierarchy.hierarchyClassName,
                data.NationalHierarchy.hierarchyLevel, data.NationalHierarchy.hierarchyID, data.NationalHierarchy.hierarchyClassID, data.NationalClassCode, data.NationalHierarchy.hierarchyParentClassID, false);

            if (duplicateHierarchyClasses)
            {
                throw new DuplicateValueException(String.Format(@"Error: The name ""{0}"" is already in use at this level of the hierarchy.", data.NationalHierarchy.hierarchyClassName));
            }
            //Check for duplicate calss code 
            if (data.NationalHierarchy.hierarchyLevel == HierarchyLevels.NationalClass)
            {
                var duplicates = getNationalClassQuery.Search(new GetNationalClassByClassCodeParameters() { ClassCode = data.NationalClassCode });
                if (duplicates.Any())
                {
                    throw new DuplicateValueException(String.Format(@"Error: The class code ""{0}"" is already in use by other national calss", data.NationalClassCode));
                }
            }
        }
    }
}
