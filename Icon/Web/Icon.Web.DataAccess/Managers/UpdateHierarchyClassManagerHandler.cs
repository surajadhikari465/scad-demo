using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;
using Icon.Common.Validators;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateHierarchyClassManagerHandler : IManagerHandler<UpdateHierarchyClassManager>
    {
        private IconContext context;
        private ICommandHandler<UpdateHierarchyClassCommand> hierarchyClassHandler;
        private ICommandHandler<UpdateHierarchyClassTraitCommand> hierarchyClassTraitHandler;
        private ICommandHandler<AddTaxEventCommand> addTaxEventHandler;
        private ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler;
        private IMapper mapper;

        public UpdateHierarchyClassManagerHandler(IconContext context,
            ICommandHandler<UpdateHierarchyClassCommand> hierarchyClassHandler,
            ICommandHandler<UpdateHierarchyClassTraitCommand> hierarchyClassTraitHandler,
            ICommandHandler<AddTaxEventCommand> addTaxEventHandler,
            ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler,
            IMapper mapper)
        {
            this.context = context;
            this.hierarchyClassHandler = hierarchyClassHandler;
            this.hierarchyClassTraitHandler = hierarchyClassTraitHandler;
            this.addTaxEventHandler = addTaxEventHandler;
            this.addHierarchyClassMessageHandler = addHierarchyClassMessageHandler;
            this.mapper = mapper;
        }

        public void Execute(UpdateHierarchyClassManager data)
        {
            ValidateHierarchyClass(data);
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    // Update Hierarchy Class - Events are generated inside handler
                    UpdateHierarchyClassCommand updateHierarchyClassCommand = mapper.Map<UpdateHierarchyClassCommand>(data);
                    hierarchyClassHandler.Execute(updateHierarchyClassCommand);

                    // Update Hierarchy Class Traits
                    UpdateHierarchyClassTraitCommand updateHierarchyClassTraitCommand = mapper.Map<UpdateHierarchyClassTraitCommand>(data);
                    hierarchyClassTraitHandler.Execute(updateHierarchyClassTraitCommand);

                    // Generate Hierarchy Messages
                    // HierarchyData.ClassNameChange will indicate if a Hierarchy Message will be generated.
                    AddHierarchyClassMessageCommand addHierarchyClassMessageCommand = mapper.Map<AddHierarchyClassMessageCommand>(updateHierarchyClassCommand);
                    addHierarchyClassMessageHandler.Execute(addHierarchyClassMessageCommand);

                    // Generate Tax Update Event
                    addTaxEventHandler.Execute(new AddTaxEventCommand
                    {
                        TaxAbbreviation = data.TaxAbbreviation,
                        HierarchyClassId = data.UpdatedHierarchyClass.hierarchyClassID
                    });


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
                        data.UpdatedHierarchyClass.hierarchyClassID, exception.Message), exception);
                }
            }
        }

        private void ValidateHierarchyClass(UpdateHierarchyClassManager data)
        {
            if (data.UpdatedHierarchyClass.hierarchyID == Hierarchies.Merchandise && data.UpdatedHierarchyClass.hierarchyLevel == HierarchyLevels.SubBrick)
            {
                var validationResult = new SubBrickCodeValidator().Validate(data.SubBrickCode);
                if(!validationResult.IsValid)
                {
                    throw new ArgumentException(validationResult.Error);
                }
                else if (context.HierarchyClassTrait.Any(hct => hct.traitID == Traits.SubBrickCode && hct.traitValue == data.SubBrickCode && hct.hierarchyClassID != data.UpdatedHierarchyClass.hierarchyClassID))
                {
                    throw new ArgumentException(string.Format("Sub-Brick Code {0} already exists. Sub-Brick Codes must be unique.", data.SubBrickCode));
                }
            }
        }
    }
}