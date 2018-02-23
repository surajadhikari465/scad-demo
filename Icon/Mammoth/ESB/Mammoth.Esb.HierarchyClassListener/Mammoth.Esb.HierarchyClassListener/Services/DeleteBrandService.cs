using System;
using Icon.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using System.Linq;
using Mammoth.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class DeleteBrandService : IHierarchyClassService<DeleteBrandRequest>
    {
        private ICommandHandler<DeleteBrandsParameter> deleteBrandsCommandHandler;

        public DeleteBrandService(ICommandHandler<DeleteBrandsParameter> deleteBrandsCommand)
        {
            this.deleteBrandsCommandHandler = deleteBrandsCommand;
        }

        public void ProcessHierarchyClasses(DeleteBrandRequest request)
        {
            var brands = request.HierarchyClasses
                .Where(hc => hc.HierarchyId == Hierarchies.Brands && hc.Action == ActionEnum.Delete)
                .ToList();

            if (brands.Any())
            {
                var command = new DeleteBrandsParameter { Brands = brands };
                this.deleteBrandsCommandHandler.Execute(command);
            }
        }
    }
}
