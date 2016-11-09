using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
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
    public class GetIconPosDataForEsbQueryTests
    {
        private GetIconPosDataForEsbQueryHandler getIconPosDataForEsbQueryHandler;
        private Mock<ILogger<GetIconPosDataForEsbQueryHandler>> mockLogger;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private List<IRMAPush> iconPosDataNotMarkedByThisController;
        private List<IRMAPush> iconPosDataMarkedByThisController;

        [TestInitialize]
        public void Initialize()
        {
            StartupOptions.Instance = 99;

            mockLogger = new Mock<ILogger<GetIconPosDataForEsbQueryHandler>>();
            context = new GlobalIconContext(new IconContext());

            var random = new Random(10000);

            iconPosDataNotMarkedByThisController = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(98),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(98)
            };

            iconPosDataMarkedByThisController = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithIdentifier("11111").WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(StartupOptions.Instance),
                new TestIrmaPushBuilder().WithIdentifier("22222").WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(StartupOptions.Instance)
            };

            transaction = context.Context.Database.BeginTransaction();

            context.Context.IRMAPush.AddRange(iconPosDataNotMarkedByThisController);
            context.Context.IRMAPush.AddRange(iconPosDataMarkedByThisController);
            context.Context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void BuildQueryHandler()
        {
            getIconPosDataForEsbQueryHandler = new GetIconPosDataForEsbQueryHandler(mockLogger.Object, context);
        }

        private void StageDuplicateIrmaPush()
        {
            var duplicateIrmaPush = new TestIrmaPushBuilder().WithIdentifier("22222").WithInProcessBy(StartupOptions.Instance);
            context.Context.IRMAPush.Add(duplicateIrmaPush);
            context.Context.SaveChanges();
        }

        [TestMethod]
        public void GetPosDataForEsb_NoPosDataForEsbIsMarkedByThisController_NoRecordsShouldBeReturned()
        {
            // Given.
            BuildQueryHandler();

            // When.
            var queryResults = getIconPosDataForEsbQueryHandler.Execute(new GetIconPosDataForEsbQuery { Instance = StartupOptions.Instance });

            // Then.
            bool onlyReadyDataIsReturned = !queryResults.Intersect(iconPosDataNotMarkedByThisController).Any();

            Assert.IsTrue(onlyReadyDataIsReturned);
        }

        [TestMethod]
        public void GetPosDataForEsb_PosDataForEsbIsMarkedByThisController_MarkedPosDataShouldBeReturned()
        {
            // Given.
            BuildQueryHandler();

            // When.
            var queryResults = getIconPosDataForEsbQueryHandler.Execute(new GetIconPosDataForEsbQuery { Instance = StartupOptions.Instance });

            // Then.
            Assert.AreEqual(iconPosDataMarkedByThisController.Count, queryResults.Count);
        }

        [TestMethod]
        public void GetPosDataForEsb_TwoRowsWithDuplicateIdentifierAndBusinessUnitId_OnlyOneOfTheDuplicatesShouldBeReturned()
        {
            // Given.
            BuildQueryHandler();
            StageDuplicateIrmaPush();

            // When.
            var queryResults = getIconPosDataForEsbQueryHandler.Execute(new GetIconPosDataForEsbQuery { Instance = StartupOptions.Instance });

            // Then.
            Assert.AreEqual(iconPosDataMarkedByThisController.Count, queryResults.Count);
        }
    }
}
