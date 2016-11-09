using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetIconPosDataForUdmQueryTests
    {
        private GetIconPosDataForUdmQueryHandler getIconPosDataForUdmQueryHandler;
        private Mock<ILogger<GetIconPosDataForUdmQueryHandler>> mockLogger;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private List<IRMAPush> iconPosDataNotMarkedByThisController;
        private List<IRMAPush> iconPosDataMarkedByThisController;
        private int instance;

        [TestInitialize]
        public void Initialize()
        {
            instance = 99;
            context = new GlobalIconContext(new IconContext());

            mockLogger = new Mock<ILogger<GetIconPosDataForUdmQueryHandler>>();
            
            var random = new Random(10000);

            iconPosDataNotMarkedByThisController = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(2).WithEsbReadyDate(DateTime.Now),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(2).WithEsbReadyDate(DateTime.Now),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(2).WithEsbReadyDate(DateTime.Now),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(2).WithEsbReadyDate(DateTime.Now),
            };

            iconPosDataMarkedByThisController = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithIdentifier("11111").WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(instance).WithEsbReadyDate(DateTime.Now),
                new TestIrmaPushBuilder().WithIdentifier("22222").WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(instance).WithEsbReadyDate(DateTime.Now),
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
            getIconPosDataForUdmQueryHandler = new GetIconPosDataForUdmQueryHandler(mockLogger.Object, context);
        }

        private void StageDuplicateIrmaPush()
        {
            var duplicateIrmaPush = new TestIrmaPushBuilder().WithIdentifier("22222").WithInProcessBy(instance);
            context.Context.IRMAPush.Add(duplicateIrmaPush);
            context.Context.SaveChanges();
        }

        [TestMethod]
        public void GetPosDataForUdm_NoPosDataIsMarkedByThisController_NoRecordsShouldBeReturned()
        {
            // Given.
            BuildQueryHandler();

            // When.
            var queryResults = getIconPosDataForUdmQueryHandler.Execute(new GetIconPosDataForUdmQuery { Instance = instance });

            // Then.
            bool onlyReadyDataIsReturned = !queryResults.Intersect(iconPosDataNotMarkedByThisController).Any();
            Assert.IsTrue(onlyReadyDataIsReturned);
        }

        [TestMethod]
        public void GetPosDataForUdm_PosDataIsMarkedByThisController_PosDataReadyForUdmShouldBeReturned()
        {
            // Given.
            BuildQueryHandler();

            // When.
            var queryResults = getIconPosDataForUdmQueryHandler.Execute(new GetIconPosDataForUdmQuery { Instance = instance });

            // Then.
            Assert.AreEqual(iconPosDataMarkedByThisController.Count, queryResults.Count);
        }

        [TestMethod]
        public void GetPosDataForUdm_TwoRowsWithDuplicateIdentifierAndBusinessUnitId_OnlyOneOfTheDuplicatesShouldBeReturned()
        {
            // Given.
            BuildQueryHandler();
            StageDuplicateIrmaPush();

            // When.
            var queryResults = getIconPosDataForUdmQueryHandler.Execute(new GetIconPosDataForUdmQuery { Instance = instance });

            // Then.
            Assert.AreEqual(iconPosDataMarkedByThisController.Count, queryResults.Count);
        }
    }
}
