using Icon.Logging;
using Icon.Testing.Builders;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Framework.RenewableContext;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetIrmaPosDataQueryTests
    {
        private GetIrmaPosDataQueryHandler getIrmaPosDataQueryHandler;
        private GlobalIrmaContext globalContext;
        private IrmaContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<GetIrmaPosDataQueryHandler>> mockLogger;
        private Mock<IIrmaContextProvider> mockContextProvider;
        private List<IConPOSPushPublish> irmaPosDataMarkedByController;
        private List<IConPOSPushPublish> irmaPosDataNotMarkedByController;
        private Random random;
        private int testStoreNumber;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ILogger<GetIrmaPosDataQueryHandler>>();
            this.getIrmaPosDataQueryHandler = new GetIrmaPosDataQueryHandler(mockLogger.Object);

            StartupOptions.Instance = 99;

            this.context = new IrmaContext(ConnectionBuilder.GetConnection("FL"));
            this.globalContext = new GlobalIrmaContext(this.context, ConnectionBuilder.GetConnection("FL"));
            this.mockContextProvider = new Mock<IIrmaContextProvider>();
            this.mockContextProvider.Setup(cp => cp.GetRegionalContext(It.IsAny<string>())).Returns(this.context);

            this.random = new Random(10000);
            this.testStoreNumber = context.Store.First(s => s.WFM_Store && s.Internal && s.BusinessUnit_ID.HasValue).Store_No;

            irmaPosDataMarkedByController = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(testStoreNumber)
                    .WithIdentifier("11111")
                    .WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
                    .WithInProcessBy(StartupOptions.Instance),
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(testStoreNumber)
                    .WithIdentifier("22222")
                    .WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
                    .WithInProcessBy(StartupOptions.Instance),
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(testStoreNumber)
                    .WithIdentifier("33333")
                    .WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
                    .WithInProcessBy(StartupOptions.Instance),
            };

            irmaPosDataNotMarkedByController = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(testStoreNumber)
                    .WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
                    .WithInProcessBy(2),
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(testStoreNumber)
                    .WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
                    .WithInProcessBy(2),
            };

            transaction = context.Database.BeginTransaction();

            context.IConPOSPushPublish.AddRange(irmaPosDataMarkedByController);
            context.IConPOSPushPublish.AddRange(irmaPosDataNotMarkedByController);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageDuplicatePosDataRecord()
        {
            var duplicatePosDataRecord = new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(testStoreNumber)
                    .WithIdentifier("22222")
                    .WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
                    .WithInProcessBy(StartupOptions.Instance);

            context.IConPOSPushPublish.Add(duplicatePosDataRecord);
            context.SaveChanges();
        }

        [TestMethod]
        public void GetIrmaPosData_NoPosDataIsMarkedByThisController_NoRecordsShouldBeReturned()
        {
            // Given.
            var query = new GetIrmaPosDataQuery
            {
                Context = this.context,
                Instance = StartupOptions.Instance
            };

            // When.
            var queryResults = getIrmaPosDataQueryHandler.Execute(query);

            // Then.
            bool onlyMarkedDataIsReturned = !queryResults.Intersect(irmaPosDataNotMarkedByController).Any();
            
            Assert.IsTrue(onlyMarkedDataIsReturned);
        }

        [TestMethod]
        public void GetIrmaPosData_PosDataIsMarkedByThisController_PosDataShouldBeReturned()
        {
            // Given.
            var query = new GetIrmaPosDataQuery
            {
                Context = this.context,
                Instance = StartupOptions.Instance
            };

            // When.
            var queryResults = getIrmaPosDataQueryHandler.Execute(query);

            // Then.
            Assert.AreEqual(irmaPosDataMarkedByController.Count, queryResults.Count);
        }

        [TestMethod]
        public void GetIrmaPosData_DuplicatePosDataRecordsExist_OnlyOneOfTheDuplicateRecordsShouldBeReturned()
        {
            // Given.
            StageDuplicatePosDataRecord();

            var query = new GetIrmaPosDataQuery
            {
                Context = this.context,
                Instance = StartupOptions.Instance
            };

            // When.
            var queryResults = getIrmaPosDataQueryHandler.Execute(query);

            // Then.
            Assert.AreEqual(irmaPosDataMarkedByController.Count, queryResults.Count);
        }
    }
}
