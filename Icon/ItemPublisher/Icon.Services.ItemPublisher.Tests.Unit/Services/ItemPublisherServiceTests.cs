using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Models.Builders;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services.Tests
{
    [TestClass()]
    public class ItemPublisherServiceTests
    {
        private ItemPublisherService service;
        private Mock<IItemPublisherRepository> mockRepository;
        private Mock<ILogger<ItemPublisherService>> mockLogger;
        private Mock<IItemProcessor> mockItemProcessor;

        [TestInitialize]
        public void Initialize()
        {
            mockRepository = new Mock<IItemPublisherRepository>();
            mockLogger = new Mock<ILogger<ItemPublisherService>>();
            mockItemProcessor = new Mock<IItemProcessor>();
            service = new ItemPublisherService(mockRepository.Object, mockLogger.Object, new ServiceSettings(), mockItemProcessor.Object);
        }

        /// <summary>
        /// Tests that when a single record exists to process that it's processed by the item processor
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_ItemProcessorShouldBeCalledWhenSuccessful()
        {
            // Given.
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));
            mockItemProcessor.Setup(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<MessageSendResult>()
            {
                new MessageSendResult(true,"success")
            }));
            mockItemProcessor.Setup(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<MessageSendResult>()
            {
                new MessageSendResult(true,"success")
            }));

            int numerOfCalls = 1;
            mockRepository.SetupSequence(m => m.DequeueMessageQueueItems(It.IsAny<int>()))
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem>());
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            mockRepository.Setup(x => x.GetMessageItemModels(It.IsAny<List<MessageQueueItem>>())).Returns(() =>
            {
                // this code allows us to return one record on the first call and 0 on subsequent calls.
                if (numerOfCalls > 0)
                {
                    numerOfCalls--;
                    return Task.FromResult(new List<MessageQueueItemModel>()
                    {
                        messageQueueItemModelBuilder.Build(new Repositories.Entities.Item()
                        {
                            ItemAttributesJson = "{'IsActive':1}",
                        },
                        new List<Repositories.Entities.Hierarchy>() { },
                        new Nutrition())
                    });
                }
                else
                {
                    return Task.FromResult(new List<MessageQueueItemModel>() { });
                }
            });


            // When.
            await service.Process(1);

            // Then
            mockItemProcessor.Verify(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>()));
            mockItemProcessor.Verify(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>()));
        }

        /// <summary>
        /// Tests that the service will process batches of records using the config setting
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_MultipleBatchesShouldExecute()
        {
            // Given.
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));
            mockItemProcessor.Setup(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<MessageSendResult>()
            {
                new MessageSendResult(true,"success")
            }));
            mockItemProcessor.Setup(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<MessageSendResult>()
            {
                new MessageSendResult(true,"success")
            }));

            int numerOfCalls = 5;
            mockRepository.SetupSequence(m => m.DequeueMessageQueueItems(It.IsAny<int>()))
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem>());
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            mockRepository.Setup(x => x.GetMessageItemModels(It.IsAny<List<MessageQueueItem>>())).Returns(() =>
            {
                // this code allows us to return one record on the first call and 0 on subsequent calls.
                if (numerOfCalls > 0)
                {
                    numerOfCalls--;
                    return Task.FromResult(new List<MessageQueueItemModel>()
                    {
                        messageQueueItemModelBuilder.Build(new Repositories.Entities.Item()
                        {
                            ItemAttributesJson = "{'IsActive':1 }",
                        },
                        new List<Repositories.Entities.Hierarchy>() { },
                        new Nutrition())
                    });
                }
                else
                {
                    return Task.FromResult(new List<MessageQueueItemModel>() { });
                }
            });

            // When.
            await service.Process(1);

            // Then.
            mockItemProcessor.Verify(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>()), Times.Exactly(5), "With a batch size of 1 and a queue count of 5 we should have had 5 executions.");
            mockItemProcessor.Verify(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>()), Times.Exactly(5), "With a batch size of 1 and a queue count of 5 we should have had 5 executions."); 
        }

        /// <summary>
        /// Tests that an archive record is created when the record is processed
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_ArchiveRecordShouldBeCreated()
        {
            // Given.
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));
            mockItemProcessor.Setup(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<MessageSendResult>()
            {
                new MessageSendResult(true,"success")
            }));
            mockItemProcessor.Setup(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<MessageSendResult>()
            {
                new MessageSendResult(true,"success")
            }));

            int numerOfCalls = 1;
            mockRepository.SetupSequence(m => m.DequeueMessageQueueItems(It.IsAny<int>()))
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem>());
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            mockRepository.Setup(x => x.GetMessageItemModels(It.IsAny<List<MessageQueueItem>>())).Returns(() =>
            {
                // this code allows us to return one record on the first call and 0 on subsequent calls.
                if (numerOfCalls > 0)
                {
                    numerOfCalls--;
                    return Task.FromResult(new List<MessageQueueItemModel>()
                    {
                        messageQueueItemModelBuilder.Build(new Repositories.Entities.Item()
                        {
                             ItemAttributesJson = "{'IsActive':1}",
                        },
                        new List<Repositories.Entities.Hierarchy>() { },
                        new Nutrition())
                    });
                }
                else
                {
                    return Task.FromResult(new List<MessageQueueItemModel>() { });
                }
            });

            // When.
            await service.Process(1);

            // Then.
            mockRepository.Verify(x => x.AddMessageQueueHistoryRecords(It.IsAny<List<MessageQueueItemArchive>>()), "AddMessageQueueHistoryRecords shold have been called once becase we had one queue item to process");
        }

        /// <summary>
        /// Tests that if a SQL exception occurs that the app does not crash and attempts to retry with polly.
        /// This tests sets up a SQLException to occur and verifies the call to process retries and eventually stops after 10 seconds.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_SqlExceptionOccurs_ProcessShouldNotCrash()
        {
            // Given.
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));

            // There isn't a way to create SqlException directly so this Func is a way to generate one
            // from https://stackoverflow.com/questions/1386962/how-to-throw-a-sqlexception-when-needed-for-mocking-and-unit-testing
            Func<SqlException> throwSqlException = () =>
            {
                SqlException exception = null;
                try
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=.;Database=GUARANTEED_TO_FAIL;Connection Timeout=1");
                    conn.Open();
                }
                catch (SqlException ex)
                {
                    exception = ex;
                }
                return (exception);
            };

            mockRepository.SetupSequence(m => m.DequeueMessageQueueItems(It.IsAny<int>()))
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem>());
            mockRepository.Setup(x => x.GetMessageItemModels(It.IsAny<List<MessageQueueItem>>())).Throws(throwSqlException());

            // When.
            var startTime = Stopwatch.StartNew();
            await service.Process(1);
            var elapsedMS = startTime.ElapsedMilliseconds;

            // Then.
            Assert.IsTrue(elapsedMS > 9999 && elapsedMS < 15000);
        }

        [TestMethod]
        public async Task Process_ESBFailureOccurred_RecordsShouldBeSentToDeadLetterQueue()
        {
            // Given.
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));
            mockItemProcessor.Setup(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Throws(new Exception("ESB exception"));

            mockRepository.SetupSequence(m => m.DequeueMessageQueueItems(It.IsAny<int>()))
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem>());
            mockRepository.Setup(x => x.GetMessageItemModels(It.IsAny<List<MessageQueueItem>>()))
                .Returns(Task.FromResult(new List<MessageQueueItemModel>()
                {
                    new MessageQueueItemModel()
                    {
                        Item=new ItemModel()
                        {
                            ItemId=1
                        }
                    }
                }));

            // When
            await service.Process(1);

            // Then
            mockRepository.Verify(x => x.AddDeadLetterMessageQueueRecord(It.IsAny<MessageDeadLetterQueue>()));
        }

        [TestMethod]
        public async Task Process_DatabaseFailureOccurred_RecordsShouldBeSentToDeadLetterQueue()
        {
            // Given.
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));

            mockRepository.SetupSequence(m => m.DequeueMessageQueueItems(It.IsAny<int>()))
                .ReturnsAsync(new List<MessageQueueItem> { new MessageQueueItem { } })
                .ReturnsAsync(new List<MessageQueueItem>());
            mockRepository.Setup(x => x.GetMessageItemModels(It.IsAny<List<MessageQueueItem>>()))
                .Throws(new Exception("Exception from the database"));

            // When
            await service.Process(1);

            // Then
            mockRepository.Verify(x => x.AddDeadLetterMessageQueueRecord(It.IsAny<MessageDeadLetterQueue>()));
        }
    }
}