using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Commands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class StagePosDataRowByRowCommandTests
    {
        private StagePosDataRowByRowCommandHandler stagePosDataRowByRowCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private List<IRMAPush> posDataToInsert;
        private string testScanCode;
        private string invalidIdentifier;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            stagePosDataRowByRowCommandHandler = new StagePosDataRowByRowCommandHandler(context);

            testScanCode = "2222222";
            invalidIdentifier = new string('1', 20);

            transaction = context.Context.Database.BeginTransaction();

            var random = new Random(1000);
            posDataToInsert = new List<IRMAPush>();
            for (int i = 0; i < 10; i++)
            {
                posDataToInsert.Add(new TestIrmaPushBuilder().WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next())));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void StageIrmaPushData_ValidListOfPosData_PosDataShouldBeSavedToTheDatabase()
        {
            // Given.
            var command = new StagePosDataRowByRowCommand();

            // When.
            foreach (var posDataRecord in posDataToInsert)
            {
                command.PosDataEntity = posDataRecord;
                stagePosDataRowByRowCommandHandler.Execute(command);
            }

            // Then.
            var insertedMessages = context.Context.IRMAPush.Where(ip => ip.Identifier == this.testScanCode).ToList();

            Assert.AreEqual(posDataToInsert.Count, insertedMessages.Count);
        }

        [TestMethod]
        public void StageIrmaPushData_InsertFails_EntityShouldBeDetachedFromTheContext()
        {
            // Given.
            var command = new StagePosDataRowByRowCommand
            {
                PosDataEntity = new TestIrmaPushBuilder().WithIdentifier(invalidIdentifier)
            };

            // When.
            try { stagePosDataRowByRowCommandHandler.Execute(command); }
            catch (Exception) { }

            // Then.
            Assert.AreEqual(EntityState.Detached, context.Context.Entry(command.PosDataEntity).State);
        }
    }
}
