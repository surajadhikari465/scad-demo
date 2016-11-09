namespace RegionalEventController.DataAccess.Interfaces
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Execute(TQuery parameters);
    }
}