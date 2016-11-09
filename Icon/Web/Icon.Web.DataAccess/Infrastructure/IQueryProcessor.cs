
using Icon.Common.DataAccess;
namespace Icon.Web.DataAccess.Infrastructure
{
    public interface IQueryProcessor
    {
        TResult Process<TResult>(IQuery<TResult> query);
    }
}
