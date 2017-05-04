using Dapper;
using Icon.Monitoring.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetDvoJobStatusQueryTests
    {
        private GetDvoJobStatusQuery query;
        private GetDvoJobStatusParameters parameters;
        private SqlDbProvider db;

        [TestInitialize]
        public void Initialize()
        {
            db = new SqlDbProvider();
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();

            query = new GetDvoJobStatusQuery(db);
            parameters = new GetDvoJobStatusParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            db.Transaction.Rollback();
            db.Connection.Close();
        }

        [TestMethod]
        public void GetDvoJobStatusQuery_RegionsIsNull_ReturnsEmptyList()
        {
            //Given
            parameters.Regions = null;

            //When
            var result = query.Search(parameters);
            
            //Then
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetDvoJobStatusQuery_RegionsIsEmpty_ReturnsEmptyList()
        {
            //Given
            parameters.Regions = new List<string>();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetDvoJobStatusQuery_StatusesExist_ReturnsStatusesMatchingTheDvoJobAndRegionsParameter()
        {
            //Given
            parameters.Regions = new List<string> { "FL", "MA", "MW", "NA", "NC", "NE", "PN", "RM", "SO", "SP", "SW", "UK" };

            string insertSql = "insert monitor.JobStatus(JobName, Status, Region) values(@JobName, @Status, @Region)";
            foreach (var region in parameters.Regions)
            {
                db.Connection.Execute(
                    sql: insertSql,
                    param: new { JobName = Common.Constants.CustomJobNames.DvoJobName, Status = "TEST", Region = region },
                    transaction: db.Transaction);
            }

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(parameters.Regions.Count, result.Count);

            //Make sure every region is returned from the query
            var statusRegions = result.Select(s => s.Region);
            Assert.IsTrue(parameters.Regions.TrueForAll(r => statusRegions.Contains(r)));
        }
    }
}
