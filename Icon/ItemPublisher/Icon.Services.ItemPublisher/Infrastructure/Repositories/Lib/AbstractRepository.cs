namespace Icon.Services.ItemPublisher.Infrastructure.Repositories
{
    /// <summary>
    /// AbstractRepository is the base repository class that all repositories can inherit from.
    /// It provides a IProviderFactory and also transactional methods.
    /// </summary>
    public abstract class AbstractRepository
    {
        protected readonly IProviderFactory DbProviderFactory;

        public bool TransactionActive
        {
            get
            {
                return this.DbProviderFactory.Transaction != null;
            }
        }
        public AbstractRepository(IProviderFactory dbProviderFactory)
        {
            this.DbProviderFactory = dbProviderFactory;
        }

        public void BeginTransaction()
        {
            this.DbProviderFactory.BeginTransaction();
        }

        public void CommitTransaction()
        {
            this.DbProviderFactory.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            this.DbProviderFactory.RollbackTransaction();
        }
    }
}