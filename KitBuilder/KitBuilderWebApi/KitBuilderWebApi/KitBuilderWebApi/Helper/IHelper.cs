using System.Linq;

namespace KitBuilderWebApi.Helper
{
    public interface IHelper<T,Y>
    {
        IQueryable<T> SetOrderBy(IQueryable<T> DataBeforePaging, Y Parameters);
        object GetPaginationData(PagedList<T> DataAfterPaging, Y Parameters);       
    }
}