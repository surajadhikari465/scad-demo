namespace Icon.Services.ItemPublisher.Infrastructure.Repositories
{
    public interface ITransactionRepository
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}