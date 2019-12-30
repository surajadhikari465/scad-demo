using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;
using Icon.Common;

namespace Icon.Web.DataAccess.Managers
{
    public class ManufacturerManagerHandler : IManagerHandler<ManufacturerManager>
    {
        private IconContext context;
        private ICommandHandler<AddManufacturerCommand> addManufacturerCommandHandler;
        private ICommandHandler<UpdateManufacturerCommand> updateManufacturerCommandHandler;
        private ICommandHandler<UpdateManufacturerHierarchyClassTraitsCommand> updateHierarchyClassTraitsCommandHandler;
        private ICommandHandler<AddManufacturerMessageCommand> addManufacturerMessageCommandHandler;
        private IMapper mapper;

        public ManufacturerManagerHandler(
            IconContext context,
            ICommandHandler<AddManufacturerCommand> addManufacturerCommandHandler,
            ICommandHandler<UpdateManufacturerHierarchyClassTraitsCommand> updateHierarchyClassTraitsCommandHandler,
            ICommandHandler<AddManufacturerMessageCommand> addManufacturerMessageCommandHandler,
            ICommandHandler<UpdateManufacturerCommand> updateManufacturerCommandHandler,
            IMapper mapper)
        {
            this.context = context;
            this.addManufacturerCommandHandler = addManufacturerCommandHandler;
            this.updateHierarchyClassTraitsCommandHandler = updateHierarchyClassTraitsCommandHandler;
            this.addManufacturerMessageCommandHandler = addManufacturerMessageCommandHandler;
            this.updateManufacturerCommandHandler = updateManufacturerCommandHandler;
            this.mapper = mapper;
        }

        public void Execute(ManufacturerManager data)
        {
            try
            {
                Validate(data);

                if (data.Manufacturer.hierarchyClassID == 0)
                {
                    AddManufacturerCommand command = mapper.Map<AddManufacturerCommand>(data);
                    addManufacturerCommandHandler.Execute(command);
                }
                else
                {
                    UpdateManufacturerCommand command = mapper.Map<UpdateManufacturerCommand>(data);
                    updateManufacturerCommandHandler.Execute(command);
                }

                if (data.IsManufacturerHierarchyMessage)
                {
                    addManufacturerMessageCommandHandler.Execute(new AddManufacturerMessageCommand()
                    {
                        Manufacturer = data.Manufacturer,
                        Action = MessageActionTypes.AddOrUpdate,
                        ZipCode = data.ZipCode,
                        ArCustomerId = data.ArCustomerId
                    });
                }

                UpdateManufacturerHierarchyClassTraitsCommand updateHierarchyClassTraitCommand = mapper.Map<UpdateManufacturerHierarchyClassTraitsCommand>(data);
                updateHierarchyClassTraitsCommandHandler.Execute(updateHierarchyClassTraitCommand);
            }
            catch (DuplicateValueException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException($"An error occurred when processing Manufacturer {data.Manufacturer.hierarchyClassName} (ID: {data.Manufacturer.hierarchyClassID}).", ex);
            }
        }

        void Validate(ManufacturerManager data)
        {
            using (var context = new IconContext())
            {
                data.Manufacturer.hierarchyClassName = String.IsNullOrWhiteSpace(data.Manufacturer.hierarchyClassName) ? null : data.Manufacturer.hierarchyClassName.Trim();

                if (data.Manufacturer.hierarchyClassName == null)
                {
                    throw new Exception("The manufacturer name is missing.");
                }

                if (context.HierarchyClass.Any(x => x.hierarchyID == Hierarchies.Manufacturer && x.hierarchyClassID != data.Manufacturer.hierarchyClassID && String.Compare(x.hierarchyClassName, data.Manufacturer.hierarchyClassName, true) == 0))
                {
                    throw new DuplicateValueException($"The manufacturer {data.Manufacturer.hierarchyClassName} already exists.");
                }
            }
        }
    }
}