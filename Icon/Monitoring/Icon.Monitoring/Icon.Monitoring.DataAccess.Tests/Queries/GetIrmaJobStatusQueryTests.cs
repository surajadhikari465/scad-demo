namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System.Configuration;
    using System.Data.SqlClient;

    using Common.Constants;
    using Common.Enums;
    using DataAccess.Queries;
   
    [TestClass]
    public class GetIrmaJobStatusQueryTests
    {
        [TestMethod]
        public void GetJobStatusForPosPushInFL_DataExists()
        {
            // Given
            var provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
            provider.Connection.Open();

            var parameters = new GetIrmaJobStatusQueryParameters { Classname = IrmaJobClassNames.POSPushJob };
            var query = new GetIrmaJobStatusQuery(provider) { TargetRegion = IrmaRegions.FL };

            // When
            var posPushJobStatus = query.Search(parameters);

            // Then
            Assert.IsNotNull(posPushJobStatus);
            Assert.AreEqual("POSPushJob", posPushJobStatus.Classname);
            Assert.AreEqual(IrmaRegions.FL, posPushJobStatus.Region);
            Assert.IsNotNull(posPushJobStatus.LastRun);
            Assert.IsFalse(string.IsNullOrEmpty(posPushJobStatus.Status));

            provider.Connection.Close();
        }

        [TestMethod]
        public void GetJobStatusForCheckSendOrderInFL_DataExists()
        {
            // Given
            var provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
            provider.Connection.Open();

            var parameters = new GetIrmaJobStatusQueryParameters { Classname = IrmaJobClassNames.CheckSendOrderStatusJob };
            var query = new GetIrmaJobStatusQuery(provider) { TargetRegion = IrmaRegions.FL };

            // When
            var posPushJobStatus = query.Search(parameters);

            // Then
            Assert.IsNotNull(posPushJobStatus);
            Assert.AreEqual("CheckSendOrderStatusJob", posPushJobStatus.Classname);
            Assert.AreEqual(IrmaRegions.FL, posPushJobStatus.Region);
            Assert.IsNotNull(posPushJobStatus.LastRun);
            Assert.IsFalse(string.IsNullOrEmpty(posPushJobStatus.Status));

            provider.Connection.Close();
        }
    }
}
