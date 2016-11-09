using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluCategoryByIdQuery : IQueryHandler<GetPluCategoryByIdParameters, PLUCategory>
    {
        private readonly IconContext context;

        public GetPluCategoryByIdQuery(IconContext context)
        {
            this.context = context;
        }

        public PLUCategory Search(GetPluCategoryByIdParameters parameters)
        {
            return this.context.PLUCategory.SingleOrDefault(pc => pc.PluCategoryID == parameters.PluCategoryID); 
        }
    }
}
