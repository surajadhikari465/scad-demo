using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemTraitByTraitCodeQuery : IQueryHandler<GetItemTraitByTraitCodeParameters, ItemTrait>
    {
        private readonly IconContext context;

        public GetItemTraitByTraitCodeQuery(IconContext context)
        {
            this.context = context;
        }

        public ItemTrait Search(GetItemTraitByTraitCodeParameters parameters)
        {
            var itemTrait = context.ScanCode
                .Single(sc => sc.scanCode == parameters.ScanCode)
                .Item
                .ItemTrait.Single(it => it.Trait.traitCode == parameters.TraitCode);

            return itemTrait;
        }
    }
}
