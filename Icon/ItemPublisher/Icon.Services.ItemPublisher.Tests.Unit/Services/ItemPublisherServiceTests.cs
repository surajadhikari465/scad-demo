using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure.Esb;
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
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services.Tests
{
    [TestClass()]
    public class ItemPublisherServiceTests
    {
        /// <summary>
        /// Tests that when a single record exists to process that it's processed by the item processor
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_ItemProcessorShouldBeCalledWhenSuccessful()
        {
            // Given.
            Mock<IItemPublisherRepository> mockRepository = new Mock<IItemPublisherRepository>();
            Mock<ILogger<ItemPublisherService>> mockLogger = new Mock<ILogger<ItemPublisherService>>();
            Mock<IItemProcessor> mockItemProcessor = new Mock<IItemProcessor>();
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));
            mockItemProcessor.Setup(x => x.ProcessDepartmentSaleRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<EsbSendResult>()
            {
                new EsbSendResult(true,"success")
            }));
            mockItemProcessor.Setup(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<EsbSendResult>()
            {
                new EsbSendResult(true,"success")
            }));
            mockItemProcessor.Setup(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<EsbSendResult>()
            {
                new EsbSendResult(true,"success")
            }));

            int numerOfCalls = 1;
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            mockRepository.Setup(x => x.DequeueMessageQueueRecords(It.IsAny<int>())).Returns(() =>
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

            ItemPublisherService service = new ItemPublisherService(mockRepository.Object, mockLogger.Object, new ServiceSettings(), mockItemProcessor.Object);

            // When.
            await service.Process(1);

            // Then
            mockItemProcessor.Verify(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>()));
            mockItemProcessor.Verify(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>()));
            mockItemProcessor.Verify(x => x.ProcessDepartmentSaleRecords(It.IsAny<List<MessageQueueItemModel>>()));
        }

        /// <summary>
        /// Tests that the service will process batches of records using the config setting
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_MultipleBatchesShouldExecute()
        {
            // Given.
            Mock<IItemPublisherRepository> mockRepository = new Mock<IItemPublisherRepository>();
            Mock<ILogger<ItemPublisherService>> mockLogger = new Mock<ILogger<ItemPublisherService>>();
            Mock<IItemProcessor> mockItemProcessor = new Mock<IItemProcessor>();
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));
            mockItemProcessor.Setup(x => x.ProcessDepartmentSaleRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<EsbSendResult>()
            {
                new EsbSendResult(true,"success")
            }));
            mockItemProcessor.Setup(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<EsbSendResult>()
            {
                new EsbSendResult(true,"success")
            }));
            mockItemProcessor.Setup(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<EsbSendResult>()
            {
                new EsbSendResult(true,"success")
            }));

            int numerOfCalls = 5;
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            mockRepository.Setup(x => x.DequeueMessageQueueRecords(It.IsAny<int>())).Returns(() =>
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

            ItemPublisherService service = new ItemPublisherService(mockRepository.Object, mockLogger.Object, new ServiceSettings(), mockItemProcessor.Object);

            // When.
            await service.Process(1);

            // Then.
            mockItemProcessor.Verify(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>()), Times.Exactly(5), "With a batch size of 1 and a queue count of 5 we should have had 5 executions.");
            mockItemProcessor.Verify(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>()), Times.Exactly(5), "With a batch size of 1 and a queue count of 5 we should have had 5 executions.");
            mockItemProcessor.Verify(x => x.ProcessDepartmentSaleRecords(It.IsAny<List<MessageQueueItemModel>>()), Times.Exactly(5), "With a batch size of 1 and a queue count of 5 we should have had 5 executions.");
        }

        /// <summary>
        /// Tests that an archive record is created when the record is processed
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_ArchiveRecordShouldBeCreated()
        {
            // Given.
            Mock<IItemPublisherRepository> mockRepository = new Mock<IItemPublisherRepository>();
            Mock<ILogger<ItemPublisherService>> mockLogger = new Mock<ILogger<ItemPublisherService>>();
            Mock<IItemProcessor> mockItemProcessor = new Mock<IItemProcessor>();
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));
            mockItemProcessor.Setup(x => x.ProcessDepartmentSaleRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<EsbSendResult>()
            {
                new EsbSendResult(true,"success")
            }));
            mockItemProcessor.Setup(x => x.ProcessNonRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<EsbSendResult>()
            {
                new EsbSendResult(true,"success")
            }));
            mockItemProcessor.Setup(x => x.ProcessRetailRecords(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult(new List<EsbSendResult>()
            {
                new EsbSendResult(true,"success")
            }));

            int numerOfCalls = 1;
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            mockRepository.Setup(x => x.DequeueMessageQueueRecords(It.IsAny<int>())).Returns(() =>
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

            ItemPublisherService service = new ItemPublisherService(mockRepository.Object, mockLogger.Object, new ServiceSettings(), mockItemProcessor.Object);

            // When.
            await service.Process(1);

            // Then.
            mockRepository.Verify(x => x.AddMessageQueueHistoryRecords(It.IsAny<List<MessageQueueItemArchive>>()), "AddMessageQueueHistoryRecords shold have been called once becase we had one queue item to process");
        }

        /// <summary>
        /// Tests that if a SQL exception occurs that the app does not crash and attempts to retry with polly.
        /// This tests sets up a SQLException to occur and verifies the call to process does not return within 10 seconds because it should
        /// retry forever.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_SqlExceptionOccurs_ProcessShouldNotCrash()
        {
            // Given.
            Mock<IItemPublisherRepository> mockRepository = new Mock<IItemPublisherRepository>();
            Mock<ILogger<ItemPublisherService>> mockLogger = new Mock<ILogger<ItemPublisherService>>();
            Mock<IItemProcessor> mockItemProcessor = new Mock<IItemProcessor>();
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

            mockRepository.Setup(x => x.DequeueMessageQueueRecords(It.IsAny<int>())).Throws(throwSqlException());

            ItemPublisherService service = new ItemPublisherService(mockRepository.Object, mockLogger.Object, new ServiceSettings(), mockItemProcessor.Object);

            // When.
            int timeout = 10000;
            var task = service.Process(1);

            // Then
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                Assert.Fail("The task completed and it should not have. It should have continued to retry forever");
            }
        }

        /// <summary>
        /// Tests that any exception besides SqlException is thrown up
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_GeneralExceptionOccurs_ProcessShouldThrowException()
        {
            // Given.
            Mock<IItemPublisherRepository> mockRepository = new Mock<IItemPublisherRepository>();
            Mock<ILogger<ItemPublisherService>> mockLogger = new Mock<ILogger<ItemPublisherService>>();
            Mock<IItemProcessor> mockItemProcessor = new Mock<IItemProcessor>();
            mockItemProcessor.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));

            mockRepository.Setup(x => x.DequeueMessageQueueRecords(It.IsAny<int>())).Throws(new Exception(("test")));

            ItemPublisherService service = new ItemPublisherService(mockRepository.Object, mockLogger.Object, new ServiceSettings(), mockItemProcessor.Object);

            try
            {
                // When
                await service.Process(1);
                // Then
                Assert.Fail("An exception that is not handled by the Polly retry logic should have been thrown up");
            }
            catch (Exception)
            {
                // Pass
            }
        }
    }
}