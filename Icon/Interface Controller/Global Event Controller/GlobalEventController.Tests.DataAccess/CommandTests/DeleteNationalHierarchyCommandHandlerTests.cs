using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using Irma.Testing.Builders;
using GlobalEventController.DataAccess.Commands;
using Moq;
using Icon.Logging;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Icon.Framework;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class DeleteNationalHierarchyCommandHandlerTests
    {
        private IrmaContext irmaContext;
        private IconContext iconContext;
        private DeleteNationalHierarchyCommand command;
        private DeleteNationalHierarchyCommandHandler handler;
        private Mock<ILogger<DeleteNationalHierarchyCommandHandler>> mockLogger;

        [TestInitialize]
        public void InitializeData()
        {
            this.irmaContext = new IrmaContext();
            this.iconContext = new IconContext();
            this.command = new DeleteNationalHierarchyCommand();
            this.mockLogger = new Mock<ILogger<DeleteNationalHierarchyCommandHandler>>();
            this.handler = new DeleteNationalHierarchyCommandHandler(this.irmaContext, mockLogger.Object);
        }

        [TestMethod]
        public void NationalHierarchyDelete_NationalHierarchyNotFoundInIrma_LoggerErrorCalled()
        {
            // Given
            this.command.iconId = irmaContext.NatItemClass.Select(nc => nc.NatCatID).FirstOrDefault();
            // When
            this.handler.Handle(this.command);

            // Then
            this.mockLogger.Verify(log => log.Error(It.IsAny<string>()), Times.Once, "Logging Error was not called one time.");
        }

        [TestMethod]
        public void NationalHierarchyDelete_NationalHierarchyHasChildRecords_LoggerErrorCalled()
        {
            // Given
            this.command.iconId = -1;

            // When
            this.handler.Handle(this.command);

            // Then
            this.mockLogger.Verify(log => log.Error(It.IsAny<string>()), Times.Once, "Logging Error was not called one time.");
        }

        [TestMethod]
        public void NationalHierarchyDelete_NationalHierarchyNoChildRecords_ShouldDelete()
        {
            // Given
            using (TransactionScope scope = new TransactionScope())
            {
                irmaContext.NatItemFamily.Add(new NatItemFamily()
                {
                    NatFamilyName = "testHierarchy",
                    NatSubTeam_No = null,
                    LastUpdateTimestamp = System.DateTime.Now,
                    SubTeam_No = null
                });
                irmaContext.SaveChanges();
                var hierarchyClass = irmaContext.NatItemFamily.Where(nif => nif.NatFamilyName == "testHierarchy").First();
                irmaContext.ValidatedNationalClass.Add(new ValidatedNationalClass()
                {
                    IrmaId = hierarchyClass.NatFamilyID,
                    IconId = 999999,
                    InsertDate = System.DateTime.Now,
                    Level = 1
                });
                irmaContext.SaveChanges();
                this.command.iconId = 999999;

                // When
                this.handler.Handle(this.command);

                // Then
                Boolean doesNatItemFamilyRecordExist = irmaContext.NatItemFamily.Where(nif => nif != null && nif.NatFamilyID == hierarchyClass.NatFamilyID).Any();
                Assert.AreEqual(false, doesNatItemFamilyRecordExist);
                scope.Dispose();
            }

        }
    }
}
