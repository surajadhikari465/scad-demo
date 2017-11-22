
using System.Threading.Tasks;

namespace Mammoth.Common.DataAccess.CommandQuery
{
    public interface IQueryHandlerAsync<TParameters, TResult> where TParameters : IQuery<TResult> 
    {
        Task<TResult> QueryAsync(TParameters parameters);
    }
}
