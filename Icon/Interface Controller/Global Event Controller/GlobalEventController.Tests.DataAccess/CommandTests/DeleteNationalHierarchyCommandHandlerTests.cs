using GlobalEventController.DataAccess.Commands;
using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class DeleteNationalHierarchyCommandHandlerTests
    {
        private DeleteNationalHierarchyCommandHandler commandHandler;
        private DeleteNationalHierarchyCommand command;
        private Mock<ILogger<DeleteNationalHierarchyCommandHandler>> mockLogger;
        private IrmaDbContextFactory contextFactory;
        private IrmaContext context;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            this.transaction = new TransactionScope();
            this.context = new IrmaContext();
            this.command = new DeleteNationalHierarchyCommand();
            this.mockLogger = new Mock<ILogger<DeleteNationalHierarchyCommandHandler>>();
            this.contextFactory = new IrmaDbContextFactory();
            this.commandHandler = new DeleteNationalHierarchyCommandHandler(contextFactory, mockLogger.Object);
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
            this.command.IconId = context.NatItemClass.Select(nc => nc.NatCatID).FirstOrDefault();

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
            context.NatItemFamily.Add(new NatItemFamily()
            {
                NatFamilyName = "testHierarchy",
                NatSubTeam_No = null,
                LastUpdateTimestamp = System.DateTime.Now,
                SubTeam_No = null
            });
            context.SaveChanges();
            var hierarchyClass = context.NatItemFamily.Where(nif => nif.NatFamilyName == "testHierarchy").First();
            context.ValidatedNationalClass.Add(new ValidatedNationalClass()
            {
                IrmaId = hierarchyClass.NatFamilyID,
                IconId = 999999,
                InsertDate = System.DateTime.Now,
                Level = 1
            });
            context.SaveChanges();
            this.command.IconId = 999999;

            // When
            this.commandHandler.Handle(this.command);

            // Then
            Boolean doesNatItemFamilyRecordExist = context.NatItemFamily.Where(nif => nif != null && nif.NatFamilyID == hierarchyClass.NatFamilyID).Any();
            Assert.AreEqual(false, doesNatItemFamilyRecordExist);
        }

        [TestMethod]
        public void DeleteNationalHierarchy_DeleteEntireHierarchy_ShouldDeleteAllRecordsFromIrma()
        {
            //Given
            var testNatItemFamily = context.NatItemFamily.Add(new NatItemFamily
            {
                NatFamilyName = "Test Family - Test Category"
            });
            context.SaveChanges();
            var testNatItemCat = context.NatItemCat.Add(new NatItemCat
            {
                NatCatName = "Test Sub Category",
                NatFamilyID = testNatItemFamily.NatFamilyID
            });
            context.SaveChanges();
            var testNatItemClass = context.NatItemClass.Add(new NatItemClass
            {
                ClassID = 3000000,
                ClassName = "Test Class",
                NatCatID = testNatItemCat.NatCatID
            });
            context.SaveChanges();

            var validatedFamily = context.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 1000000,
                IrmaId = testNatItemFamily.NatFamilyID,
                Level = HierarchyLevels.NationalFamily
            });
            var validatedCategory = context.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 1000001,
                IrmaId = testNatItemFamily.NatFamilyID,
                Level = HierarchyLevels.NationalCategory
            });
            var validatedSubCategory = context.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 1000002,
                IrmaId = testNatItemCat.NatCatID,
                Level = HierarchyLevels.NationalSubCategory
            });
            var validatedClass = context.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 1000003,
                IrmaId = testNatItemClass.ClassID,
                Level = HierarchyLevels.NationalClass
            });
            context.SaveChanges();

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
            Assert.IsFalse(context.ValidatedNationalClass.Any(vnc => vnc.IconId >= 1000000 && vnc.IconId <= 1000003));
        }
    }
}
