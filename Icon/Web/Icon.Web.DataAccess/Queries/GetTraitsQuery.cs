using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTraitsQuery : IQueryHandler<GetTraitsParameters, List<Trait>>
    {
        private IconContext context;

        public GetTraitsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<Trait> Search(GetTraitsParameters parameters)
        {
            IQueryable<Trait> traits = this.context.Trait;

            if (parameters.IncludeNavigation)
            {
                traits = traits.Include(t => t.TraitGroup);
            }

            return traits.ToList();
        }
    }
}
