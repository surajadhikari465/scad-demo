using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Data;

namespace Icon.Services.ItemPublisher.Infrastructure
{
    public interface IProviderFactory
    {
        SqlDbProvider Provider { get; }
        IDbTransaction Transaction { get; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}