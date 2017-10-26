using Icon.Logging;
using Icon.Testing.Builders;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Framework.RenewableContext;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.DataAccess.Queries;
using System.Transactions;
namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetInstanceDataFlagValueByFlagKeyQueryTests
    {
        private IrmaContext context;
        private GetInstanceDataFlagValueByFlagKeyQueryHandler getInstanceDataFlagValueByFlagKeyQueryHandler;
        private TransactionScope transaction;
        string flagKey ;

        [TestInitialize]
        public void Initialize()
        {
            this.transaction = new TransactionScope();
            this.context = new IrmaContext(ConnectionBuilder.GetConnection("FL"));    
            flagKey = "GlobalPriceManagement";
            getInstanceDataFlagValueByFlagKeyQueryHandler = new GetInstanceDataFlagValueByFlagKeyQueryHandler();
            setFloridaForGPM();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void GetInstanceDataFlag_ForGlobalPriceManagement_ShouldReturnTrue()
        {
            // Given.
            var query = new GetInstanceDataFlagValueByFlagKeyQuery
            {
                FlagKey = flagKey,
                StoreNo = null,
                Context = context
            };
            // When.
            var queryResults = getInstanceDataFlagValueByFlagKeyQueryHandler.Execute(query);

            // Then.
            Assert.AreEqual(true, queryResults);
        }

        private void setFloridaForGPM()
        {
            context.Database.ExecuteSqlCommand("update instanceDataFlags SET FlagValue = 1 WHERE FlagKey = 'GlobalPriceManagement'");
        }
    }
}