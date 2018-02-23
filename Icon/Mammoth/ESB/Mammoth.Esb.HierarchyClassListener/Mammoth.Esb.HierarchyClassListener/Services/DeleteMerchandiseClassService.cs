using System;
using Icon.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using System.Linq;
using Mammoth.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class DeleteMerchandiseClassService : IHierarchyClassService<DeleteMerchandiseClassRequest>
    {
        private ICommandHandler<DeleteMerchandiseClassParameter> deleteMerchandiseClassesCommand;

        public DeleteMerchandiseClassService(ICommandHandler<DeleteMerchandiseClassParameter> deleteMerchandiseHierarchiesCommand)
        {
            this.deleteMerchandiseClassesCommand = deleteMerchandiseHierarchiesCommand;
        }

        public void ProcessHierarchyClasses(DeleteMerchandiseClassRequest request)
        {
            var merchandiseClasses = request.HierarchyClasses
                .Where(hc => hc.HierarchyId == Hierarchies.Merchandise && hc.Action == ActionEnum.Delete)
                .ToList();

            if (merchandiseClasses.Any())
            {
                var commandParameters = new DeleteMerchandiseClassParameter { MerchandiseClasses = merchandiseClasses };
                this.deleteMerchandiseClassesCommand.Execute(commandParameters);
            }
        }
    }
}
