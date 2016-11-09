namespace Mammoth.ItemLocale.Controller.DataAccess.Tests
{
    using Common.DataAccess.DbProviders;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    public static class IrmaTestDbProviderFactory
    {
        private const string IrmaItemCatalog = "ItemCatalog";

        public static SqlDbProvider CreateAndOpen(string region)
        {
            var regionKey = string.Format("{0}_{1}", IrmaItemCatalog, region);
            var dbProvider = new SqlDbProvider
            {
                Connection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings[regionKey].ConnectionString)
            };

            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction(IsolationLevel.Snapshot);

            return dbProvider;
        }
    }
}
