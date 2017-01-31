using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Monitoring.DataAccess.Queries;
using System.Data.SqlClient;
using System.Configuration;
using Icon.Monitoring.Common;
using Dapper;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetMammothApiMessageQueueIdQueryTests
    {
        private SqlDbProvider provider;
        private GetMammothApiMessageQueueIdQuery query;
        private GetApiMessageQueueIdParameters parameters;

        [TestInitialize]
        public void Initialize()
        {
            provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            provider.Connection.Open();
            provider.Transaction = provider.Connection.BeginTransaction();
            query = new GetMammothApiMessageQueueIdQuery(provider);
            parameters = new GetApiMessageQueueIdParameters();

            provider.Connection.Execute("truncate table esb.MessageQueuePrice", transaction: provider.Transaction);
            provider.Connection.Execute("truncate table esb.MessageQueueItemLocale", transaction: provider.Transaction);
        }

        [TestCleanup]
        public void CleanUp()
        {
            provider.Transaction.Rollback();
            provider.Connection.Close();
        }

        [TestMethod]
        public void GetApiMessageQueueIdSearch_DataExists_ReturnsMessageQueueId()
        {
            //Given
            int messageQueueId = InsertTestMessageQueuePrice();
            Assert.AreNotEqual(0, messageQueueId);

            parameters.MessageQueueType = MessageQueueTypes.Price;

            //When
            int result = query.Search(parameters);

            //Then
            Assert.AreEqual(messageQueueId, result);
        }

        [TestMethod]
        public void GetApiMessageQueueIdSearch_DataDoesntExist_Returns0()
        {
            //Given
            parameters.MessageQueueType = MessageQueueTypes.Price;

            //When
            int result = query.Search(parameters);

            //Then
            Assert.AreEqual(0, result);
        }

        private int InsertTestMessageQueuePrice()
        {
            return provider.Connection.Query<int>(
                    @"insert into esb.MessageQueuePrice
                        ([MessageTypeId]
                        ,[MessageStatusId]
                        ,[MessageHistoryId]
                        ,[MessageActionId]
                        ,[InsertDate]
                        ,[ItemId]
                        ,[ItemTypeCode]
                        ,[ItemTypeDesc]
                        ,[BusinessUnitId]
                        ,[LocaleName]
                        ,[ScanCode]
                        ,[UomCode]
                        ,[CurrencyCode]
                        ,[PriceTypeCode]
                        ,[SubPriceTypeCode]
                        ,[Price]
                        ,[Multiple]
                        ,[StartDate]
                        ,[EndDate]
                        ,[InProcessBy]
                        ,[ProcessedDate])
                values (@MessageTypeId,
                        @MessageStatusId,
                        @MessageHistoryId,
                        @MessageActionId,
                        @InsertDate,
                        @ItemId,
                        @ItemTypeCode,
                        @ItemTypeDesc,
                        @BusinessUnitId,
                        @LocaleName,
                        @ScanCode,
                        @UomCode,
                        @CurrencyCode,
                        @PriceTypeCode,
                        @SubPriceTypeCode,
                        @Price,
                        @Multiple,
                        @StartDate,
                        @EndDate,
                        @InProcessBy,
                        @ProcessedDate)

                      select SCOPE_IDENTITY()",
                            new
                            {
                                @MessageTypeId = 2,
                                @MessageStatusId = 1,
                                @MessageHistoryId = (int?)null,
                                @MessageActionId = 1,
                                @InsertDate = DateTime.Now,
                                @ItemId = 1,
                                @ItemTypeCode = "TES",
                                @ItemTypeDesc = "TEST",
                                @BusinessUnitId = 1,
                                @LocaleName = "TEST",
                                @ScanCode = "TEST",
                                @UomCode = "TES",
                                @CurrencyCode = "TES",
                                @PriceTypeCode = "TES",
                                @SubPriceTypeCode = "TES",
                                @Price = 1,
                                @Multiple = 1,
                                @StartDate = DateTime.Now,
                                @EndDate = (DateTime?)null,
                                @InProcessBy = (int?)null,
                                @ProcessedDate = (DateTime?)null
                            },
                            provider.Transaction)
                            .FirstOrDefault();
        }
    }
}
