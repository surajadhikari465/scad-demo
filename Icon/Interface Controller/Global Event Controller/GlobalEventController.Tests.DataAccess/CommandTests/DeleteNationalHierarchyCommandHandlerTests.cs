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
        private DeleteNationalHierarchyCommandHandler commandHandler;
        private DeleteNationalHierarchyCommand command;
        private Mock<ILogger<DeleteNationalHierarchyCommandHandler>> mockLogger;
        private IrmaContext irmaContext;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            this.irmaContext = new IrmaContext();
            this.command = new DeleteNationalHierarchyCommand();
            this.mockLogger = new Mock<ILogger<DeleteNationalHierarchyCommandHandler>>();
            this.commandHandler = new DeleteNationalHierarchyCommandHandler(this.irmaContext, mockLogger.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void DeleteNationalHierarchy_NationalHierarchyNotFoundInIrma_LoggerErrorCalled()
        {
            // Given
            this.command.IconId = irmaContext.NatItemClass.Select(nc => nc.NatCatID).FirstOrDefault();

            // When
            this.commandHandler.Handle(this.command);

            // Then
            this.mockLogger.Verify(log => log.Error(It.IsAny<string>()), Times.Once, "Logging Error was not called one time.");
        }

        [TestMethod]
        public void DeleteNationalHierarchy_NationalHierarchyHasChildRecords_LoggerErrorCalled()
        {
            // Given
            this.command.IconId = -1;

            // When
            this.commandHandler.Handle(this.command);

            // Then
            this.mockLogger.Verify(log => log.Error(It.IsAny<string>()), Times.Once, "Logging Error was not called one time.");
        }

        [TestMethod]
        public void DeleteNationalHierarchy_NationalHierarchyNoChildRecords_ShouldDelete()
        {
            // Given
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
            this.command.IconId = 999999;

            // When
            this.commandHandler.Handle(this.command);

            // Then
            Boolean doesNatItemFamilyRecordExist = irmaContext.NatItemFamily.Where(nif => nif != null && nif.NatFamilyID == hierarchyClass.NatFamilyID).Any();
            Assert.AreEqual(false, doesNatItemFamilyRecordExist);
        }

        [TestMethod]
        public void DeleteNationalHierarchy_DeleteEntireHierarchy_ShouldDeleteAllRecordsFromIrma()
        {
            //Given
            var testNatItemFamily = irmaContext.NatItemFamily.Add(new NatItemFamily
            {
                NatFamilyName = "Test Family - Test Category"
            });
            irmaContext.SaveChanges();
            var testNatItemCat = irmaContext.NatItemCat.Add(new NatItemCat
            {
                NatCatName = "Test Sub Category",
                NatFamilyID = testNatItemFamily.NatFamilyID
            });
            irmaContext.SaveChanges();
            var testNatItemClass = irmaContext.NatItemClass.Add(new NatItemClass
            {
                ClassID = 3000000,
                ClassName = "Test Class",
                NatCatID = testNatItemCat.NatCatID
            });
            irmaContext.SaveChanges();

            var validatedFamily = irmaContext.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 1000000,
                IrmaId = testNatItemFamily.NatFamilyID,
                Level = HierarchyLevels.NationalFamily
            });
            var validatedCategory = irmaContext.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 1000001,
                IrmaId = testNatItemFamily.NatFamilyID,
                Level = HierarchyLevels.NationalCategory
            });
            var validatedSubCategory = irmaContext.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 1000002,
                IrmaId = testNatItemCat.NatCatID,
                Level = HierarchyLevels.NationalSubCategory
            });
            var validatedClass = irmaContext.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 1000003,
                IrmaId = testNatItemClass.ClassID,
                Level = HierarchyLevels.NationalClass
            });
            irmaContext.SaveChanges();

            //When
            command.IconId = validatedClass.IconId.Value;
            commandHandler.Handle(command);
            command.IconId = validatedSubCategory.IconId.Value;
            commandHandler.Handle(command);
            command.IconId = validatedCategory.IconId.Value;
            commandHandler.Handle(command);
            command.IconId = validatedFamily.IconId.Value;
            commandHandler.Handle(command);

            //Then
            Assert.IsFalse(irmaContext.ValidatedNationalClass.Any(vnc => vnc.IconId >= 1000000 && vnc.IconId <= 1000003));
        }
    }
}
