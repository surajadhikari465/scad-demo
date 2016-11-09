
namespace GlobalEventController.DataAccess.Infrastructure
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery parameters);
    }
}
