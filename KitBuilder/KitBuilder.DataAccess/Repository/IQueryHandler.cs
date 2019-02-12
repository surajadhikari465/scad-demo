namespace KitBuilder.DataAccess.Repository
{
	public interface IQueryHandler<TParameters, TResult> where TParameters : IQuery<TResult>
	{
		TResult Search(TParameters parameters);
	}
}
