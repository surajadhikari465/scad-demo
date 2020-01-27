using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace Icon.Esb.R10Listener.Tests
{
    [TestClass]
    public class CommandHandlerTestBase<CommandHandlerType, CommandType>
    {
        protected CommandHandlerType commandHandler;
        protected CommandType command;
        protected TransactionScope transaction;
        protected SqlConnection sqlConnection;

        [TestInitialize]
        public void BaseInitialize()
        {
            transaction = new TransactionScope();
            Initialize();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            Cleanup();
            transaction.Dispose();
        }

        protected virtual void Initialize() { }

        protected virtual void Cleanup() { }
    }
}
