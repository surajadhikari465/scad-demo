using System.Collections.Generic;
using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.DataAccess.Queries;

namespace Icon.Web.DataAccess.Infrastructure
{
    public interface IItemQueryBuilder
    {
        string BuildQuery(GetItemsParameters parameters);
    }
}