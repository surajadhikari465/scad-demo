using Icon.Common.Context;
using InterfaceController.Common;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Queries;
using System.Linq;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetAppConfigKeysQueryTests
    {
        private GetAppConfigKeysQueryHandler getAppConfigQueryHandler;
        private IrmaContext context;
        private IrmaContextProvider contextProvider;

        [TestInitialize]
        public void Initialize()
        {
            contextProvider = new IrmaContextProvider();
            context = contextProvider.GetRegionalContext(ConnectionBuilder.GetConnection("SP"));
            getAppConfigQueryHandler = new GetAppConfigKeysQueryHandler();
        }

        [TestMethod]
        public void GetAppConfigKeys_QueryExecutesSuccessfully_ConfigKeysShouldBeReturned()
        {
            // Given.
            var query = new GetAppConfigKeysQuery
            {
                Context = context,
                ApplicationName = "POS PUSH JOB"
            };

            // When.
            var configKeys = getAppConfigQueryHandler.Execute(query);

            // Then.
            string errorNotificationEmail = configKeys.Single(k => k.Key == "primaryErrorNotification").Value;

            Assert.IsNotNull(errorNotificationEmail);
        }
    }
}
