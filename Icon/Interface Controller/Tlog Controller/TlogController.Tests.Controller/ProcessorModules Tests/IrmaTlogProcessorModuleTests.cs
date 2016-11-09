using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TlogController.Controller.ProcessorModules;
using Icon.Framework;
using Icon.Logging;
using Moq;
using TlogController.DataAccess.Queries;
using System.Collections.Generic;
using TlogController.DataAccess.BulkCommands;
using TlogController.DataAccess.Infrastructure;
using TlogController.DataAccess.Interfaces;
using TlogController.Common;
using Irma.Framework;
using TlogController.DataAccess.Models;
using Icon.Testing.Builders;

namespace TlogController.Tests.Controller.ProcessorModules_Tests
{
    [TestClass]
    public class IrmaTlogProcessorModuleTests
    {
        private Mock<ILogger<IrmaTlogProcessorModule>> mockLogger;
        private Mock<IBulkCommandHandler<BulkUpdateSalesSumByitemCommand>> mockBulkUpdateSalesSumByitemCommandHandler;
        private Mock<IBulkCommandHandler<BulkInsertTlogReprocessRequestsCommand>> mockBulkInsertTlogReprocessRequestsCommandHandler;
        private Mock<IIconTlogProcessorModule> mockIconTlogProcessorModule;
        private IrmaContext irmaContext;
        private string regionCode = "FL";


        private IrmaTlogProcessorModule module;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<IrmaTlogProcessorModule>>();
            this.mockBulkUpdateSalesSumByitemCommandHandler = new Mock<IBulkCommandHandler<BulkUpdateSalesSumByitemCommand>>();
            this.mockBulkInsertTlogReprocessRequestsCommandHandler = new Mock<IBulkCommandHandler<BulkInsertTlogReprocessRequestsCommand>>();
            this.mockIconTlogProcessorModule = new Mock<IIconTlogProcessorModule>();
            irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(regionCode.ToString()));

            this.module = new IrmaTlogProcessorModule(this.mockLogger.Object, irmaContext, this.mockBulkUpdateSalesSumByitemCommandHandler.Object, this.mockBulkInsertTlogReprocessRequestsCommandHandler.Object, mockIconTlogProcessorModule.Object);
        }

        [TestMethod]
        public void PushSalesSumByitemDataInBulkToIrma_FailedToUpdateInBulk_AllTransactionsProcessedPropertyMaskedAsFalse()
        {
            // Given
            this.mockBulkUpdateSalesSumByitemCommandHandler.Setup(ms => ms.Execute(It.IsAny<BulkUpdateSalesSumByitemCommand>())).Throws(new Exception());
            IrmaTlog testIrmaTlog = StageIrmaTlog("FL");

            // When
            try
            {
                this.module.PushSalesSumByitemDataInBulkToIrma(testIrmaTlog);
            }
            catch (Exception) { }
            
            // Then
            foreach (ItemMovementTransaction itemMovementTransaction in testIrmaTlog.ItemMovementTransactionList)
            {
                Assert.IsTrue(itemMovementTransaction.Processed == false);
            }
        }

        [TestMethod]
        public void PushSalesSumByitemDataInBulkToIrma_SuccessfullyUpdateInBulk_AllTransactionsProcessedPropertyMaskedAsTrue()
        {
            // Given
            this.mockBulkUpdateSalesSumByitemCommandHandler.Setup(ms => ms.Execute(It.IsAny<BulkUpdateSalesSumByitemCommand>())).Returns(3);
            IrmaTlog testIrmaTlog = StageIrmaTlog("FL");

            // When
            this.module.PushSalesSumByitemDataInBulkToIrma(testIrmaTlog);
      
            // Then
            foreach (ItemMovementTransaction itemMovementTransaction in testIrmaTlog.ItemMovementTransactionList)
            {
                Assert.IsTrue(itemMovementTransaction.Processed == true);
            }
        }

        private IrmaTlog StageIrmaTlog(string regionCode)
        {
            var random = new Random();

            var testItemMovementToIrma = new List<Icon.Testing.CustomModels.ItemMovementToIrma>
            {
                new TestItemMovementToIrmaBuilder().WithItemMovementID(1).WithESBMessageID("TestMessage1").WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build(),
                new TestItemMovementToIrmaBuilder().WithItemMovementID(2).WithESBMessageID("TestMessage1").WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build(),
                new TestItemMovementToIrmaBuilder().WithItemMovementID(3).WithESBMessageID("TestMessage2").WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build(),
                new TestItemMovementToIrmaBuilder().WithItemMovementID(4).WithESBMessageID("TestMessage2").WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build(),
                new TestItemMovementToIrmaBuilder().WithItemMovementID(5).WithESBMessageID("TestMessage3").WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build(),
                new TestItemMovementToIrmaBuilder().WithItemMovementID(6).WithESBMessageID("TestMessage3").WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build(),
                new TestItemMovementToIrmaBuilder().WithItemMovementID(7).WithESBMessageID("TestMessage3").WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build()
            }.ConvertAll(n => new ItemMovementToIrma
            {
                ItemMovementID = n.ItemMovementID,
                ESBMessageID = n.ESBMessageID,
                TransDate = n.TransDate,
                BusinessUnitId = n.BusinessUnitId,
                Identifier = n.Identifier,
                ItemType = n.ItemType,
                ItemVoid = n.ItemVoid,
                Quantity = n.Quantity,
                Weight = n.Weight,
                BasePrice = n.BasePrice,
                MarkdownAmount = n.MarkdownAmount
            });

            var testItemMovementTransaction = new List<Icon.Testing.CustomModels.ItemMovementTransaction>
            {
                new TestItemMovementTransactionBuilder().WithESBMessageID("TestMessage1").WithFirstItemMovementToIrmaIndex(0).WithLastItemMovementToIrmaIndex(1).Build(),
                new TestItemMovementTransactionBuilder().WithESBMessageID("TestMessage2").WithFirstItemMovementToIrmaIndex(2).WithLastItemMovementToIrmaIndex(3).Build(),
                new TestItemMovementTransactionBuilder().WithESBMessageID("TestMessage3").WithFirstItemMovementToIrmaIndex(4).WithLastItemMovementToIrmaIndex(6).Build()
            }.ConvertAll(t => new ItemMovementTransaction
            {
                ESBMessageID = t.ESBMessageID,
                LastItemMovementToIrmaIndex = t.LastItemMovementToIrmaIndex,
                FirstItemMovementToIrmaIndex = t.FirstItemMovementToIrmaIndex,
                Processed = t.Processed
            });

            IrmaTlog testIrmaTlog = new IrmaTlog
            {
                RegionCode = regionCode,
                ItemMovementToIrmaList = testItemMovementToIrma,
                TlogReprocessRequestList = null,
                ItemMovementTransactionList = testItemMovementTransaction
            };

            return testIrmaTlog;
        }
    }
}
