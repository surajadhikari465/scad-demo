using InterfaceController.Common;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Queries;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetJobStatusQueryTests
    {
        private GetJobStatusQueryHandler getJobStatusQueryHandler;
        private IrmaContext context;
        private IrmaContextProvider contextProvider;

        [TestInitialize]
        public void Initialize()
        {
            contextProvider = new IrmaContextProvider();
            context = contextProvider.GetRegionalContext(ConnectionBuilder.GetConnection("SP"));
            getJobStatusQueryHandler = new GetJobStatusQueryHandler();
        }

        [TestMethod]
        public void GetJobStatus_PosPushStatusIsQueried_JobStatusShouldBeReturned()
        {
            // Given.
            var query = new GetJobStatusQuery
            {
                Context = context,
                JobName = "POSPushJob"
            };

            // When.
            var jobStatus = getJobStatusQueryHandler.Execute(query);

            // Then.
            Assert.IsNotNull(jobStatus);
            Assert.AreEqual(jobStatus.Classname, "POSPushJob");
        }
    }
}
