using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetUomQuery : IQueryHandler<GetUomParameters, List<UOM>>
    {
        private readonly IconContext context;

        public GetUomQuery(IconContext context)
        {
            this.context = context;
        }

        public List<UOM> Search(GetUomParameters parameters)
        {
            return context.UOM.ToList();
        }
    }
}
