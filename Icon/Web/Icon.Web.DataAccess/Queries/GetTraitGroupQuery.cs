using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTraitGroupQuery : IQueryHandler<GetTraitGroupParameters, List<TraitGroup>>
    {
        private readonly IconContext context;

        public GetTraitGroupQuery(IconContext context)
        {
            this.context = context;
        }

        public List<TraitGroup> Search(GetTraitGroupParameters parameters)
        {
            return context.TraitGroup.Include(t => t.Trait).ToList();
        }
    }
}
