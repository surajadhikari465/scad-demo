using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetProductSelectionGroupTypesQuery : IQueryHandler<GetProductSelectionGroupTypesParameters, List<ProductSelectionGroupType>>
    {
        private IconContext context;

        public GetProductSelectionGroupTypesQuery(IconContext context)
        {
            this.context = context;
        }
        public List<ProductSelectionGroupType> Search(GetProductSelectionGroupTypesParameters parameters)
        {
            return context.ProductSelectionGroupType.ToList();
        }
    }
}
