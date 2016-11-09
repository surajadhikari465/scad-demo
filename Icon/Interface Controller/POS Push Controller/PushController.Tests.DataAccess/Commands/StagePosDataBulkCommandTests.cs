using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.DataAccess.Commands;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class StagePosDataBulkCommandTests
    {
        private StagePosDataBulkCommandHandler stagePosDataCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<StagePosDataBulkCommandHandler>> mockLogger;
        private List<IRMAPush> posDataToInsert;
        private string testScanCode;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            testScanCode = "2222222";

            mockLogger = new Mock<ILogger<StagePosDataBulkCommandHandler>>();
            stagePosDataCommandHandler = new StagePosDataBulkCommandHandler(mockLogger.Object, context);

            transaction = context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void SetupTestPosData()
        {
            posDataToInsert = new List<IRMAPush>();

            for (int i = 0; i < 10; i++)
            {
                posDataToInsert.Add(new TestIrmaPushBuilder().WithIdentifier(this.testScanCode).WithBusinessUnitId(99990 + i));
            }
        }

        [TestMethod]
        public void StagePosData_EmptyListOfPosData_WarningShouldBeLogged()
        {
            // Given.
            posDataToInsert = new List<IRMAPush>();

            // When.
            var command = new StagePosDataBulkCommand
            {
                PosData = posDataToInsert
            };

            stagePosDataCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void StagePosData_NullListOfPosData_WarningShouldBeLogged()
        {
            // Given.
            posDataToInsert = null;

            // When.
            var command = new StagePosDataBulkCommand
            {
                PosData = posDataToInsert
            };

            stagePosDataCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void StagePosData_ValidListOfPosData_PosDataShouldBeSavedToTheDatabase()
        {
            // Given.
            SetupTestPosData();

            // When.
            var command = new StagePosDataBulkCommand
            {
                PosData = posDataToInsert
            };

            stagePosDataCommandHandler.Execute(command);

            // Then.
            var stagedPosData = context.Context.IRMAPush.Where(pos => pos.Identifier == this.testScanCode).ToList();
            Assert.AreEqual(posDataToInsert.Count, stagedPosData.Count);
        }
    }
}
