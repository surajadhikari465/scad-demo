using Dapper;
using Mammoth.Common.ControllerApplication.Http;
using Mammoth.Common.DataAccess;
using Mammoth.Common.Email;
using Mammoth.Logging;
using Mammoth.Price.Controller.Services;
using Mammoth.Price.Controller.Tests.Specs.TestInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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

namespace Mammoth.Price.Controller.Tests.Specs
{
    [TestClass]
    public class ControllerApplicationSpecs
    {
        private DateTime now = DateTime.Now;

        [TestCleanup]
        public void Cleanup()
        {
            DeleteEvents();
        }

        private void DeleteEvents()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_MA"].ConnectionString))
            {
                connection.Execute("delete mammoth.PriceChangeQueue where InsertDate = @now", new { now = this.now });
            }
        }

        [TestMethod]
        public void Given_100_Events_When_The_Controller_Successfully_Sends_All_Events_Then_It_Should_Delete_All_Events_From_Queue_And_Archive_All_Events()
        {
            //Given
            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            Mock<IHttpClientWrapper> mockHttpClient = new Mock<IHttpClientWrapper>();
            Mock<ILogger> mockLogger = new Mock<ILogger>();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            mockHttpClient.Setup(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>())).Returns(Task.FromResult(response));

            var container = SimpleInjectorInitializer.InitializeContainer();
            container.Options.AllowOverridingRegistrations = true;
            container.RegisterSingleton<IHttpClientWrapper>(() => mockHttpClient.Object);
            container.RegisterSingleton<IEmailClient>(() => mockEmailClient.Object);
            container.RegisterSingleton<ILogger>(() => mockLogger.Object);

            container.Verify();

            InsertIntoChangeQueue(100, 777);

            var waitHandle = new AutoResetEvent(false);

            Exception exception = null;
            mockLogger.Setup(l => l.Info(It.Is<string>(s => s == "Shutting down Mammoth Price Controller.")))
                .Callback(() =>
                {
                    container.GetInstance<ITopShelfService>().Stop();

                    try
                    {
                        //Then
                        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_MA"].ConnectionString))
                        {
                            var remainingEvents = connection
                                .Query("select * from mammoth.PriceChangeQueue where insertdate > @now",
                                    new { Now = now });
                            Assert.IsFalse(remainingEvents.Any());

                            var archivedEvents = connection.Query("select * from mammoth.ChangeQueueHistory where InsertDate >= @date", new { date = this.now });
                            Assert.AreEqual(100, archivedEvents.Count());
                        }
                    }
                    catch (Exception e)
                    {
                        exception = e;
                    }

                    waitHandle.Set();
                });

            //When
            container.GetInstance<ITopShelfService>().Start();

            waitHandle.WaitOne();

            container.GetInstance<ITopShelfService>().Stop();

            if (exception != null)
            {
                throw exception;
            }
        }

        private void InsertIntoChangeQueue(int numberOfRows, int instance)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_MA"].ConnectionString))
            {
                var items = connection.Query<dynamic>(
                    @"select top (@NumberOfRows) pbd.Item_Key, ii.Identifier, pbd.Store_No, pbd.PriceBatchDetailID
                    from PriceBatchDetail pbd
                    join ItemIdentifier ii on pbd.Item_Key = ii.Item_Key
                    join Item i on pbd.Item_Key = i.Item_Key
                    where pbd.PriceChgTypeID = 1
                        and ii.Deleted_Identifier = 0
                        and ii.Remove_Identifier = 0
                        and ii.Default_Identifier = 1
                        and i.Deleted_Item = 0
                        and i.Remove_Item = 0
                        and pbd.CancelAllSales IS NULL
                        and pbd.InsertApplication <> 'Sale Off'
                        and pbd.PriceChgTypeID = 1
                        and pbd.ItemChgTypeID IS NULL
                        and ii.Item_Key in
                        (
                            select ii.Item_Key 
                            from ItemIdentifier ii
                            group by ii.Item_Key having count(Item_Key) = 1
                        )
                    order by pbd.Insert_Date desc",
                    new { NumberOfRows = numberOfRows });

                int? maxQueueId = connection.Query<int?>("SELECT MAX(QueueID) as QueueID FROM [mammoth].[PriceChangeQueue]").First();
                maxQueueId = maxQueueId.HasValue ? maxQueueId.Value : 0;
                int counter = 1;
                DateTime now = DateTime.Now;

                List<TestQueueModel> queueList = new List<TestQueueModel>();
                foreach (var row in items)
                {
                    queueList.Add(new TestQueueModel
                    {
                        QueueID = maxQueueId.Value + counter,
                        Item_Key = row.Item_Key,
                        Store_No = row.Store_No,
                        Identifier = row.Identifier,
                        EventTypeID = IrmaEventTypes.Price,
                        EventReferenceID = row.PriceBatchDetailID,
                        InsertDate = this.now,
                        ProcessFailedDate = null,
                        InProcessBy = instance
                    });
                }

                // Sql Bulk Copy List into Staging Table
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    connection.Open();
                    bulkCopy.DestinationTableName = "[mammoth].[PriceChangeQueue]";
                    DataTable dataTable = queueList.ToDataTable<TestQueueModel>();
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
