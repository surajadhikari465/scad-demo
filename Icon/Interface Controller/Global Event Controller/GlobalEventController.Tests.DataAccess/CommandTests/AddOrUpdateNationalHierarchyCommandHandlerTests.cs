using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using GlobalEventController.DataAccess.Commands;
using Moq;
using Icon.Logging;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Icon.Framework;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddOrUpdateNationalHierarchyCommandHandlerTests
    {
        private IrmaContext irmaContext;
        private IconContext iconContext;
        private AddOrUpdateNationalHierarchyCommand command;
        private GetHierarchyClassQuery getHierarchyClassQuery;
        private AddOrUpdateNationalHierarchyCommandHandler handler;
        private Mock<ILogger<AddOrUpdateNationalHierarchyCommandHandler>> mockLogger;
        private Mock<IQueryHandler<GetHierarchyClassQuery, HierarchyClass>> mockGetHierarchyClassQuery;

        [TestInitialize]
        public void InitializeData()
        {
            this.irmaContext = new IrmaContext();
            this.iconContext = new IconContext();
            this.command = new AddOrUpdateNationalHierarchyCommand();
            this.getHierarchyClassQuery = new GetHierarchyClassQuery();
            this.mockLogger = new Mock<ILogger<AddOrUpdateNationalHierarchyCommandHandler>>();
            this.mockGetHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyClassQuery, HierarchyClass>>();
            this.handler = new AddOrUpdateNationalHierarchyCommandHandler(this.irmaContext, mockLogger.Object);
        }
      
        [TestMethod]
        public void AddNationalHierarchy_NationalHierarchyNotFoundInIrma_ShouldAddNationalHierarchyToIrma()
        {
            // Given
            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyClassID = 10000,
                hierarchyLevel = 1,
                hierarchyID = 6,
                hierarchyParentClassID = null,
                hierarchyClassName = "test Hierarchy"
            };

            mockGetHierarchyClassQuery.Setup(mgs => mgs.Handle(It.IsAny<GetHierarchyClassQuery>())).Returns(hierarchyClass);
            this.command.IconId = hierarchyClass.hierarchyClassID;
            this.command.Name = hierarchyClass.hierarchyClassName;
            this.command.HierarchyClass = hierarchyClass;

            // When
            using (TransactionScope scope = new TransactionScope())
            {
                this.handler.Handle(this.command);

                // Then
                int irmaId = (int)irmaContext.ValidatedNationalClass.Where(nc => nc.IconId == this.command.IconId).Select(nc => nc.IrmaId).FirstOrDefault();
                var validatedNationalClass = irmaContext.NatItemFamily.AsNoTracking().Single(nif => nif.NatFamilyID == irmaId);
                Assert.AreEqual(validatedNationalClass.NatFamilyName.Split(new char[] { '-' }, 2)[0], command.Name);
                scope.Dispose();
            }
        }

        [TestMethod]
        public void AddNationalHierarchyItemClass_ItemClassNotFoundInIrma_ShouldAddItemClassToIrma()
        {
            // Given
            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyClassID = 100000,
                hierarchyLevel = 4,
                hierarchyID = 6,
                hierarchyParentClassID = 121179,
                hierarchyClassName = "test Hierarchy Nat Class"
            };
            hierarchyClass.HierarchyClassTrait.Add(new HierarchyClassTrait()
            {
                traitID= 69,
                traitValue= "977777"
            });
            mockGetHierarchyClassQuery.Setup(mgs => mgs.Handle(It.IsAny<GetHierarchyClassQuery>())).Returns(hierarchyClass);
            this.command.IconId = hierarchyClass.hierarchyClassID;
            this.command.Name = hierarchyClass.hierarchyClassName;
            this.command.HierarchyClass = hierarchyClass;

            // When
            using (TransactionScope scope = new TransactionScope())
            {
                this.handler.Handle(this.command);

                // Then
                int irmaId = (int)irmaContext.ValidatedNationalClass.Where(nc => nc.IconId == this.command.IconId).Select(nc => nc.IrmaId).FirstOrDefault();
                var validatedNationalClass = irmaContext.NatItemClass.AsNoTracking().Single(nif => nif.ClassID == irmaId);
                Assert.AreEqual(validatedNationalClass.ClassName, command.Name);
                scope.Dispose();
            }
        }

        [TestMethod]
        public void UpdateNationalHierarchy_NationalHierarchyFoundInIrma_ShouldUpdateNationalHierarchyToIrma()
        {
            // Given
            var hierarchyClass = iconContext.HierarchyClass.Where(x => x.hierarchyID == 6 && x.hierarchyLevel == HierarchyLevels.NationalClass).FirstOrDefault();
            this.command.Name = "test Hierarchy1";
            this.command.IconId = hierarchyClass.hierarchyClassID;
            this.command.HierarchyClass = hierarchyClass;
            mockGetHierarchyClassQuery.Setup(mgs => mgs.Handle(It.IsAny<GetHierarchyClassQuery>())).Returns(hierarchyClass);
            using (TransactionScope scope = new TransactionScope())
            {
                // When
                this.handler.Handle(this.command);

                // Then
                int irmaId = (int)irmaContext.ValidatedNationalClass.Where(nc => nc.IconId == this.command.IconId).Select(nc => nc.IrmaId).FirstOrDefault();
                var nationalClass = irmaContext.NatItemClass.AsNoTracking().Single(nif => nif.ClassID == irmaId);
                Assert.AreEqual(nationalClass.ClassName.Split(new char[] { '-' }, 2)[0].TrimEnd(), command.Name);
                scope.Dispose();
            }
        }

        [TestMethod]
        public void UpdateNationalHierarchy_NationalHierarchyCategoryFoundInIrma_ShouldUpdateNationalHierarchyCatergoryToIrma()
        {
            // Given
            var hierarchyClass = iconContext.HierarchyClass.Where(x => x.hierarchyID == 6 && x.hierarchyLevel == HierarchyLevels.NationalCategory).FirstOrDefault();
            this.command.Name = "test Hierarchy1";
            this.command.IconId = hierarchyClass.hierarchyClassID;
            this.command.HierarchyClass = hierarchyClass;
            mockGetHierarchyClassQuery.Setup(mgs => mgs.Handle(It.IsAny<GetHierarchyClassQuery>())).Returns(hierarchyClass);
            using (TransactionScope scope = new TransactionScope())
            {
                // When
                this.handler.Handle(this.command);

                // Then
                int irmaId = (int)irmaContext.ValidatedNationalClass.Where(nc => nc.IconId == this.command.IconId).Select(nc => nc.IrmaId).FirstOrDefault();
                var natItemFamilyClass = irmaContext.NatItemFamily.AsNoTracking().Single(nif => nif.NatFamilyID == irmaId);
                Assert.AreEqual(natItemFamilyClass.NatFamilyName.Split(new char[] { '-' }, 2)[1].TrimStart(), command.Name);
                scope.Dispose();
            }
        }
    }
}
