using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class DeleteMerchTaxMappingByIdCommandHandler : ICommandHandler<DeleteMerchTaxMappingByIdCommand>
    {
        private IconContext context;

        public DeleteMerchTaxMappingByIdCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(DeleteMerchTaxMappingByIdCommand data)
        {
            try
            {
                var merchTaxMappingToDelete = context.HierarchyClassTrait.SingleOrDefault(hct => hct.hierarchyClassID == data.MerchHierarchyClassID && hct.traitID == Traits.MerchDefaultTaxAssociatation);

                context.HierarchyClassTrait.Remove(merchTaxMappingToDelete);
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("There was an error deleting Merch Tax Association for ID {0}.  Error: {1}",
                    data.MerchHierarchyClassID, exception.Message), exception);
            }
        }
    }
}
