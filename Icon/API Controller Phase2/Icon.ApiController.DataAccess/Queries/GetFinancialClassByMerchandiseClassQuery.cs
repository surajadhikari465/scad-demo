using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetFinancialClassByMerchandiseClassQuery : IQueryHandler<GetFinancialClassByMerchandiseClassParameters, HierarchyClass>
    {
        private IRenewableContext<IconContext> globalContext;

        public GetFinancialClassByMerchandiseClassQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public HierarchyClass Search(GetFinancialClassByMerchandiseClassParameters parameters)
        {
            var merchFinTrait = parameters.MerchandiseHierarchyClass.HierarchyClassTrait
                .SingleOrDefault(hct => hct.Trait.traitCode == TraitCodes.MerchFinMapping);

            if (merchFinTrait == null)
            {
                return null;
            }
            else
            {
                return globalContext.Context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassName == merchFinTrait.traitValue);
            }
        }
    }
}
