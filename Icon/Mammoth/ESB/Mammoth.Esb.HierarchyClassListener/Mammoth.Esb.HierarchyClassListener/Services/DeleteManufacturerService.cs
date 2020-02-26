using Icon.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using System.Linq;
using Mammoth.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class DeleteManufacturerService : IHierarchyClassService<DeleteManufacturerRequest>
    {
        private ICommandHandler<DeleteManufacturerParameter> deleteManufacturerCommandHandler;

        public DeleteManufacturerService(ICommandHandler<DeleteManufacturerParameter> deleteManufacturerCommand)
        {
            this.deleteManufacturerCommandHandler = deleteManufacturerCommand;
        }

        public void ProcessHierarchyClasses(DeleteManufacturerRequest request)
        {
            var manufacturer = request.HierarchyClasses
                .Where(hc => hc.HierarchyId == Hierarchies.Manufacturer && hc.Action == ActionEnum.Delete)
                .ToList();

            if (manufacturer.Any())
            {
                var command = new DeleteManufacturerParameter { Manufacturer = manufacturer };
                this.deleteManufacturerCommandHandler.Execute(command);
            }
        }
    }
}