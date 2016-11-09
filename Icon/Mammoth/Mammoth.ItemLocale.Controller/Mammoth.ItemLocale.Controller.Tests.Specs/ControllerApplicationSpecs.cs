using Dapper;
using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Http;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Common.Email;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.ItemLocale.Controller.Tests.Specs.TestInfrastructure;
using Mammoth.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.Tests.Specs
{
    [TestClass]
    public class ControllerApplicationSpecs
    {
        private const string IrmaConnectionStringKey = "ItemCatalog_FL";
        private const string ControllerLogCompleteMessage = "Shutting down Mammoth ItemLocale Controller.";
        private DateTime now = DateTime.Now;
        private Mock<ILogger> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IHttpClientWrapper> mockHttpClient;
        private ITopShelfService controller;

        [TestInitialize]
        public void Initialize()
        {
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockHttpClient = new Mock<IHttpClientWrapper>();
            this.mockLogger = new Mock<ILogger>();

            var container = SimpleInjectorInitializer.InitializeContainer();
            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton<IHttpClientWrapper>(() => this.mockHttpClient.Object);
            container.RegisterSingleton<IEmailClient>(() => this.mockEmailClient.Object);
            container.RegisterSingleton<ILogger>(() => this.mockLogger.Object);
            container.Verify();

            this.controller = container.GetInstance<ITopShelfService>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            DeleteEvents();
        }

        [TestMethod]
        public void Given_1000_AddOrUpdate_Events_When_The_Controller_Successfully_Sends_All_Events_Then_It_Should_Delete_All_Events_From_Queue_And_Archive_All_Events()
        {
            //Given
            var numberOfEvents = 1000;
            this.now = DateTime.Now;
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            this.mockHttpClient.Setup(m => m.PutAsJsonAsync(
                It.IsAny<string>(),    
                It.IsAny<IEnumerable<ItemLocaleEventModel>>()))
                    .Returns(Task.FromResult(response));

            this.InsertIntoChangeQueue(numberOfEvents, IrmaEventTypes.ItemLocaleAddOrUpdate, false);

            //When
            this.TestController(() =>
            {
                //Then
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[IrmaConnectionStringKey].ConnectionString))
                {
                    var remainingEvents = connection.Query(
                        "select * from mammoth.ItemLocaleChangeQueue where InsertDate >= @date",
                        new { date = this.now });
                    Assert.IsFalse(remainingEvents.Any());

                    var archivedEvents = connection.Query("select * from mammoth.ChangeQueueHistory where InsertDate >= @date", new { date = this.now });
                    Assert.AreEqual(numberOfEvents, archivedEvents.Count());
                }
            });
        }

        private int GetActiveStoresCount(SqlConnection connection)
        {
            string sql = @"DECLARE @ExcludedStoreNo varchar(250);
                            SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo', 'IRMA Client')); 

                            SELECT COUNT(*) 
                            FROM Store s
                            WHERE (s.WFM_Store = 1 OR s.Mega_Store = 1 )
                                AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
                                AND s.Store_No NOT IN (SELECT Key_Value FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|'))";

            int count = connection.Query<int>(sql).First();
            return count;
        }

        [TestMethod]
        public void Given_5_AddOrUpdate_Events_When_The_Controller_Successfully_Sends_All_Events_And_Receives_Failed_Events_InReturn_Then_It_Should_Mark_All_Events_Failed_In_Archive()
        {
            //Given
            int numberOfEvents = 5;
            this.now = DateTime.Now;
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            
            this.mockHttpClient.SetupSequence(m => m.PutAsJsonAsync(
                        It.IsAny<string>(), 
                        It.IsAny<IEnumerable<ItemLocaleEventModel>>()))
                .Returns(Task.FromResult(response))
                .Returns(Task.FromResult(response))
                .Returns(Task.FromResult(response))
                .Returns(Task.FromResult(response))
                .Returns(Task.FromResult(response))
                .Returns(Task.FromResult(response));

            this.InsertIntoChangeQueue(numberOfEvents, IrmaEventTypes.ItemLocaleAddOrUpdate, false);

            //When
            this.TestController(() =>
            {
                //Then
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[IrmaConnectionStringKey].ConnectionString))
                {
                    var remainingEvents = connection.Query(
                        "select * from mammoth.ItemLocaleChangeQueue where InsertDate >= @date and ProcessFailedDate IS NOT NULL",
                        new { date = this.now });
                    Assert.IsFalse(remainingEvents.Any());

                    var archivedEvents = connection.Query<ChangeQueueHistoryModel>("select * from mammoth.ChangeQueueHistory where InsertDate >= @date", new { date = this.now }).ToList();
                    var numberOfStores = GetActiveStoresCount(connection);
                    Assert.AreEqual(numberOfEvents, archivedEvents.Count(e => !String.IsNullOrEmpty(e.ErrorCode)));
                }
            });
        }

        [TestMethod]
        public void Given_100_Deauthorization_Events_When_The_Controller_Successfully_Sends_All_Events_Then_It_Should_Delete_All_Events_From_Queue_And_Archive_All_Events_ForEach_Store()
        {
            // Given
            int numberOfEvents = 100;
            this.now = DateTime.Now;

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            this.mockHttpClient.Setup(m => m.PutAsJsonAsync(
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .Returns(Task.FromResult(response));

            this.InsertIntoChangeQueue(numberOfEvents, IrmaEventTypes.ItemDelete, true);

            // When
            this.TestController(() =>
            {
                // Then
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString))
                {
                    var remainingEvents = connection.Query(
                        "select * from mammoth.ItemLocaleChangeQueue where InsertDate > @Now",
                        new { Now = this.now });
                    Assert.IsFalse(remainingEvents.Any(), "There are events left in the queue table after processing everything.");

                    var archivedEvents = connection.Query<ChangeQueueHistoryModel>("select * from mammoth.ChangeQueueHistory where InsertDate >= @date", new { date = this.now }).ToList();
                    var numberOfStores = GetActiveStoresCount(connection);
                    Assert.AreEqual(numberOfEvents * numberOfStores, archivedEvents.Count());
                }
            });            
        }

        /// <summary>
        /// This will start the controller and run an action when the controller has finished,
        /// as noted when the logger logs that the mammoth
        /// controller has completed; For reference see ControllerApplication.RunService().
        /// </summary>
        /// <param name="then"></param>
        private void TestController(Action then)
        {
            var waitHandle = new AutoResetEvent(false);

            Exception exception = null;
            this.mockLogger.Setup(l => l.Info(It.Is<string>(s => s == ControllerLogCompleteMessage)))
                           .Callback(() =>
                            {
                                this.controller.Stop();

                                try
                                {
                                    then();
                                    
                                }
                                catch (Exception e)
                                {
                                    exception = e;
                                }

                                waitHandle.Set();
                            });

            this.controller.Start();

            waitHandle.WaitOne();

            if (exception != null)
            {
                throw exception;
            }

            this.controller.Stop();
        }

        private void InsertIntoChangeQueue(int rows, int eventTypeId, bool nullStoreNumber)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[IrmaConnectionStringKey].ConnectionString))
            {
                var sql = string.Format(@"SELECT TOP {0}
                                ii.Identifier as Identifier,
                                ii.Item_Key as ItemKey
                            FROM
                                ItemIdentifier ii
                                JOIN Item i on ii.Item_Key = i.Item_Key
                                JOIN ValidatedScanCode vsc on ii.Identifier = vsc.ScanCode
                            WHERE
		                        i.Deleted_Item = 0
		                        AND ii.Deleted_Identifier = 0
		                        AND i.Remove_Item = 0
		                        AND ii.Remove_Identifier = 0
                                AND NOT EXISTS (SELECT 1 FROM mammoth.ItemLocaleChangeQueue q WHERE q.Item_Key = i.Item_Key)", rows);

                var itemIdentifier = connection.Query(sql).ToList();

                int? storeNo = null;
                if (!nullStoreNumber)
                {
                    IEnumerable<int> validStores = connection.Query<int>(
                        @"DECLARE @ExcludedStoreNo varchar(250);
                        SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));
                        SELECT Store_No FROM Store 
                        WHERE 
                            Store_No NOT IN (SELECT Key_Value as BusinessUnitId FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
                            AND (WFM_Store = 1 OR Mega_Store = 1 )
                            AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL);");
                    storeNo = validStores.First();
                }

                int? maxQueueId = connection.Query<int?>("SELECT MAX(QueueID) as QueueID FROM [mammoth].[ItemLocaleChangeQueue]").First() ?? 0;

                var queueList = itemIdentifier.Select((row, i) => new TestQueueModel
                {
                    QueueID = maxQueueId.Value + i,
                    Item_Key = row.ItemKey,
                    Store_No = storeNo,
                    Identifier = row.Identifier,
                    EventTypeID = eventTypeId,
                    EventReferenceID = null,
                    InsertDate = DateTime.Now,
                    ProcessFailedDate = null,
                    InProcessBy = null
                }).ToList();

                // Sql Bulk Copy List into Staging Table
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    connection.Open();
                    bulkCopy.DestinationTableName = "[mammoth].[ItemLocaleChangeQueue]";
                    DataTable dataTable = queueList.ToDataTable<TestQueueModel>();
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        private void DeleteEvents()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[IrmaConnectionStringKey].ConnectionString))
            {
                connection.Execute("delete mammoth.ItemLocaleChangeQueue where InsertDate >= @now", new { now = this.now });
            }
        }
    }
}
