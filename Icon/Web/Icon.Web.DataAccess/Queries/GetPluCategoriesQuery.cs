using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluCategoriesQuery : IQueryHandler<GetPluCategoriesParameters, List<PLUCategory>>
    {
        private readonly IconContext context;

        public GetPluCategoriesQuery(IconContext context)
        {
            this.context = context;
        }

        public List<PLUCategory> Search(GetPluCategoriesParameters parameters)
        {   
            var categoryList = this.context.PLUCategory.ToList();
            categoryList.SortPluCategoriesByBeginRange();
            return categoryList;
        }
    }
}
