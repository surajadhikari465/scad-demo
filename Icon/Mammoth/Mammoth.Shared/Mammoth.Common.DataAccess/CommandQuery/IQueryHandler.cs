
namespace Mammoth.Common.DataAccess.CommandQuery
{
    public interface IQueryHandler<TParameters, TResult> where TParameters : IQuery<TResult> 
    {
        TResult Search(TParameters parameters);
    }
}
