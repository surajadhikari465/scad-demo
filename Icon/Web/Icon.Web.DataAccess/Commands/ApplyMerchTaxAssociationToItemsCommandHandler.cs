using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class ApplyMerchTaxAssociationToItemsCommandHandler : ICommandHandler<ApplyMerchTaxAssociationToItemsCommand>
    {
        private IconContext context;

        public ApplyMerchTaxAssociationToItemsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(ApplyMerchTaxAssociationToItemsCommand data)
        {
            context.ApplyMerchTaxMappingToItems(data.MerchandiseHierarchyClassId, data.TaxHierarchyClassId);
        }
    }
}
