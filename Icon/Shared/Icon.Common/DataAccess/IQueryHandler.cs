
namespace Icon.Common.DataAccess
{
    public interface IQueryHandler<TParameters, TResult> where TParameters : IQuery<TResult> 
    {
        TResult Search(TParameters parameters);
    }
}
