using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetFinancialClassByMerchandiseClassQuery : IQueryHandler<GetFinancialClassByMerchandiseClassParameters, HierarchyClass>
    {
        private IDbContextFactory<IconContext> iconContextFactory;

        public GetFinancialClassByMerchandiseClassQuery(IDbContextFactory<IconContext> iconContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
        }

        public HierarchyClass Search(GetFinancialClassByMerchandiseClassParameters parameters)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                var merchFinTrait = parameters.MerchandiseHierarchyClass.HierarchyClassTrait
                    .SingleOrDefault(hct => hct.Trait.traitCode == TraitCodes.MerchFinMapping);

                if (merchFinTrait == null)
                {
                    return null;
                }
                else
                {
                    return context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassName == merchFinTrait.traitValue);
                }
            }
        }
    }
}
