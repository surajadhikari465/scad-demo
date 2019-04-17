using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBrandsQuery : IQueryHandler<GetBrandsParameters, List<BrandModel>>
    {
        private readonly IconContext context;

        public GetBrandsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<BrandModel> Search(GetBrandsParameters parameters)
        {
            return this.context.Database.SqlQuery<BrandModel>("EXEC app.GetBrands").ToList();
        }
    }
}
