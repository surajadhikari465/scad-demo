using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitBuilderWebApi.Helper
{
    public interface IHelper<T,Y>
    {
        IQueryable<T> SetOrderBy(IQueryable<T> DataBeforePaging, Y Parameters);
        object getPaginationData(PagedList<T> DataAfterPaging, Y Parameters);
    }
}
