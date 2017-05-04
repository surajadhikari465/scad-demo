using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Framework;
using Icon.Common.Context;
using Moq;
using System.Data.Entity;
using System.Linq;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Collections.Generic;
using Icon.Infor.Listeners.HierarchyClass.Extensions;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateHierarchyClassesCommandHandlerTests
    {
        private AddOrUpdateHierarchyClassesCommandHandler commandHandler;
        private AddOrUpdateHierarchyClassesCommand command;
        private Mock<IRenewableContext<IconContext>> mockRenewableContext;
        private List<string> regions;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockRenewableContext = new Mock<IRenewableContext<IconContext>>();
            mockRenewableContext.SetupGet(m => m.Context).Returns(context);

            regions = new List<string> { "FL", "MA", "MW" };

            commandHandler = new AddOrUpdateHierarchyClassesCommandHandler(
                mockRenewableContext.Object, 
                regions);

            command = new AddOrUpdateHierarchyClassesCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if(transaction.UnderlyingTransaction.Connection != null)
                transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_BrandDoesNotExist_ShouldAddBrands()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesNotExist_ShouldAddHierarchyClasses(
                AddBrandHierarhyClassModel(),
                HierarchyLevels.Brand,
                this.regions.Count,
                1);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_BrandDoesExist_ShouldUpdateBrand()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesExist_ShouldUpdateHierarchyClass(
                this.AddBrandHierarhyClassModel(),
                HierarchyLevels.Brand,
                Traits.Codes.BrandAbbreviation,
                1,
                regions.Count * 2,
                2);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_TaxDoesNotExist_ShouldAddTaxClasses()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesNotExist_ShouldAddHierarchyClasses(
                AddTaxHierarhyClassModel(),
                HierarchyLevels.Tax,
                0,
                0);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_TaxDoesExist_ShouldUpdateTax()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesExist_ShouldUpdateHierarchyClass(
                this.AddTaxHierarhyClassModel(),
                HierarchyLevels.Tax,
                Traits.Codes.TaxAbbreviation,
                1,
                0,
                0);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_MerchDoesNotExist_ShouldAddMerchs()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesNotExist_ShouldAddHierarchyClasses(
                AddMerchHierarhyClassModel(),
                HierarchyLevels.Segment,
                0,
                1);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_MerchDoesExist_ShouldUpdateMerch()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesExist_ShouldUpdateHierarchyClass(
                this.AddMerchHierarhyClassModel(),
                HierarchyLevels.Segment,
                null,
                0,
                0,
                2);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_FinancialDoesNotExist_ShouldAddFinancialClasses()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesNotExist_ShouldAddHierarchyClasses(
                AddFinancialHierarhyClassModel(),
                HierarchyLevels.Financial,
                0,
                1);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_FinancialDoesExist_ShouldUpdateFinancial()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesExist_ShouldUpdateHierarchyClass(
                this.AddFinancialHierarhyClassModel(),
                HierarchyLevels.Financial,
                null,
                0,
                0,
                2);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_NationalDoesNotExist_ShouldAddNationalClasses()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesNotExist_ShouldAddHierarchyClasses(
                AddNationalHierarhyClassModel(),
                HierarchyLevels.NationalFamily,
                0,
                0);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_NationalDoesExist_ShouldUpdateNational()
        {
            this.AddOrUpdateHierarchyClasses_HierarchyClassDoesExist_ShouldUpdateHierarchyClass(
                this.AddNationalHierarhyClassModel(),
                HierarchyLevels.NationalFamily,
                Traits.Codes.NationalClassCode,
                1,
                0,
                0);
        }

        private void AddOrUpdateHierarchyClasses_HierarchyClassDoesNotExist_ShouldAddHierarchyClasses(
            InforHierarchyClassModel testModel,
            int hierarchyLevel,
            int expectedNumberOfEvents,
            int expectedNumberOfMessages)
        {
            //Then
            foreach (var model in command.HierarchyClasses)
            {
                Assert.IsNull(model.ErrorCode, $"Hierarchy Class '{model.HierarchyClassName}' has an error. ErrorCode: '{model.ErrorCode}' ErrorDetails: '{model.ErrorDetails}'");
            }
            var hierarchyClassId = testModel.HierarchyClassId;
            var hierarchyClass = context.HierarchyClass
                .AsNoTracking()
                .Include(hc => hc.HierarchyClassTrait)
                .Single(hc => hc.hierarchyClassID == hierarchyClassId);

            Assert.AreEqual(testModel.HierarchyClassName, hierarchyClass.hierarchyClassName);
            Assert.AreEqual(Hierarchies.Ids[testModel.HierarchyName], hierarchyClass.hierarchyID);
            Assert.AreEqual(testModel.ParentHierarchyClassId.ToHierarchyParentClassId(), hierarchyClass.hierarchyParentClassID);
            Assert.AreEqual(testModel.HierarchyClassTraits.Count, hierarchyClass.HierarchyClassTrait.Count);
            Assert.AreEqual(HierarchyLevels.Brand, hierarchyClass.hierarchyLevel);

            foreach (var hierarchyClassTrait in hierarchyClass.HierarchyClassTrait)
            {
                var testModelTrait = testModel.HierarchyClassTraits[Traits.Codes.AsDictionary[hierarchyClassTrait.traitID]];
                Assert.AreEqual(testModelTrait, hierarchyClassTrait.traitValue);
            };

            //Assert Events are generated
            var events = context.EventQueue.Where(e => e.EventMessage == testModel.HierarchyClassName);
            Assert.AreEqual(expectedNumberOfEvents, events.Count());

            //Assert Messages are generated
            string expectedMessageHierarchyClassId = testModel.HierarchyName == Hierarchies.Names.Financial
                ? testModel.HierarchyClassName.Split('(')[1].TrimEnd(')')
                : testModel.HierarchyClassId.ToString();
            var messages = context.MessageQueueHierarchy.Where(m => m.HierarchyClassId == expectedMessageHierarchyClassId).ToList();
            Assert.AreEqual(expectedNumberOfMessages, messages.Count);

            messages.ForEach(m =>
            {
                Assert.AreEqual(testModel.HierarchyClassName, m.HierarchyClassName);
                Assert.AreEqual(Hierarchies.Ids[testModel.HierarchyName], m.HierarchyId);
                Assert.AreEqual(testModel.ParentHierarchyClassId.ToHierarchyParentClassId(), m.HierarchyParentClassId);
                Assert.AreEqual(testModel.HierarchyLevelName, m.HierarchyLevelName);
                Assert.AreEqual(hierarchyLevel, m.HierarchyLevel);
            });
        }

        private void AddOrUpdateHierarchyClasses_HierarchyClassDoesExist_ShouldUpdateHierarchyClass(
            InforHierarchyClassModel testModel,
            int hierarchyLevel,
            string traitCode,
            int expectedNumberOfTraits,
            int expectedNumberOfEvents,
            int expectedNumberOfMessages)
        {
            //Given
            if (testModel.HierarchyName == Hierarchies.Names.Financial)
            {
                testModel.HierarchyClassName = string.Concat("Changed HierarchyClass ", "(", testModel.HierarchyClassName.Split('(')[1].TrimEnd(')'), ")");
            }
            else
            {
                testModel.HierarchyClassName = "Changed HierarchyClass";
            }

            if (!string.IsNullOrWhiteSpace(traitCode))
            {
                testModel.HierarchyClassTraits[traitCode] = "Changed HierarchyClass";
            }

            //When
            commandHandler.Execute(command);

            //Then
            foreach (var model in command.HierarchyClasses)
            {
                Assert.IsNull(model.ErrorCode, $"Hierarchy Class '{model.HierarchyClassName}' has an error. ErrorCode: '{model.ErrorCode}' ErrorDetails: '{model.ErrorDetails}'");
            }
            var hierarchyClassId = testModel.HierarchyClassId;
            var hierarchyClass = context.HierarchyClass
                .AsNoTracking()
                .Include(hc => hc.HierarchyClassTrait)
                .Single(hc => hc.hierarchyClassID == hierarchyClassId);

            Assert.AreEqual(testModel.HierarchyClassName, hierarchyClass.hierarchyClassName);
            Assert.AreEqual(Hierarchies.Ids[testModel.HierarchyName], hierarchyClass.hierarchyID);
            Assert.AreEqual(testModel.ParentHierarchyClassId.ToHierarchyParentClassId(), hierarchyClass.hierarchyParentClassID);
            Assert.AreEqual(expectedNumberOfTraits, hierarchyClass.HierarchyClassTrait.Count);
            Assert.AreEqual(testModel.HierarchyClassTraits.Count, hierarchyClass.HierarchyClassTrait.Count);
            Assert.AreEqual(hierarchyLevel, hierarchyClass.hierarchyLevel);

            foreach (var hierarchyClassTrait in hierarchyClass.HierarchyClassTrait)
            {
                var testModelTrait = testModel.HierarchyClassTraits[Traits.Codes.AsDictionary[hierarchyClassTrait.traitID]];
                Assert.AreEqual(testModelTrait, hierarchyClassTrait.traitValue);
            };

            //Assert Events are generated
            var events = context.EventQueue.Where(e => e.EventReferenceId == hierarchyClassId);
            Assert.AreEqual(expectedNumberOfEvents, events.Count());

            //Assert Messages are generated
            string expectedMessageHierarchyClassId = testModel.HierarchyName == Hierarchies.Names.Financial
                ? testModel.HierarchyClassName.Split('(')[1].TrimEnd(')')
                : testModel.HierarchyClassId.ToString();
            var messages = context.MessageQueueHierarchy.Where(m => m.HierarchyClassId == expectedMessageHierarchyClassId).ToList();
            Assert.AreEqual(expectedNumberOfMessages, messages.Count());

            if (expectedNumberOfMessages > 0)
            {
                var message = messages.Last();
                Assert.AreEqual(testModel.HierarchyClassName, message.HierarchyClassName);
                Assert.AreEqual(Hierarchies.Ids[testModel.HierarchyName], message.HierarchyId);
                Assert.AreEqual(testModel.ParentHierarchyClassId.ToHierarchyParentClassId(), message.HierarchyParentClassId);
                Assert.AreEqual(testModel.HierarchyLevelName, message.HierarchyLevelName);
                Assert.AreEqual(hierarchyLevel, message.HierarchyLevel);
            }
        }

        private InforHierarchyClassModel AddBrandHierarhyClassModel()
        {
            var newHierarchyClassModel = new InforHierarchyClassModel
            {
                HierarchyClassId = 87654321,
                HierarchyClassName = "Test HierarchyClass",
                HierarchyName = Hierarchies.Names.Brands,
                ParentHierarchyClassId = 0,
                HierarchyLevelName = HierarchyLevelNames.Brand,
                HierarchyClassTraits = new Dictionary<string, string>
                {
                    { Traits.Codes.BrandAbbreviation, "Test HierarchyClass" }
                }
            };
            command.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                newHierarchyClassModel
            };

            commandHandler.Execute(command);

            return newHierarchyClassModel;
        }

        private InforHierarchyClassModel AddTaxHierarhyClassModel()
        {
            var newHierarchyClassModel = new InforHierarchyClassModel
            {
                HierarchyClassId = 8765432,
                HierarchyClassName = "Test HierarchyClass",
                HierarchyName = Hierarchies.Names.Tax,
                ParentHierarchyClassId = 0,
                HierarchyLevelName = HierarchyLevelNames.Tax,
                HierarchyClassTraits = new Dictionary<string, string>
                {
                    { Traits.Codes.TaxAbbreviation, "Test HierarchyClass" }
                }
            };
            command.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                newHierarchyClassModel
            };

            commandHandler.Execute(command);

            return newHierarchyClassModel;
        }

        private InforHierarchyClassModel AddMerchHierarhyClassModel()
        {
            var newHierarchyClassModel = new InforHierarchyClassModel
            {
                HierarchyClassId = 87654322,
                HierarchyClassName = "Test HierarchyClass",
                HierarchyName = Hierarchies.Names.Merchandise,
                ParentHierarchyClassId = 0,
                HierarchyLevelName = HierarchyLevelNames.Segment,
                HierarchyClassTraits = new Dictionary<string, string>()
            };
            command.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                newHierarchyClassModel
            };

            commandHandler.Execute(command);

            return newHierarchyClassModel;
        }

        private InforHierarchyClassModel AddFinancialHierarhyClassModel()
        {
            var newHierarchyClassModel = new InforHierarchyClassModel
            {
                HierarchyClassId = 87654323,
                HierarchyClassName = "Test Financial HierarchyClass (1234)",
                HierarchyName = Hierarchies.Names.Financial,
                ParentHierarchyClassId = 0,
                HierarchyLevelName = HierarchyLevelNames.Financial,
                HierarchyClassTraits = new Dictionary<string, string>()
            };
            command.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                newHierarchyClassModel
            };

            commandHandler.Execute(command);

            return newHierarchyClassModel;
        }

        private InforHierarchyClassModel AddNationalHierarhyClassModel()
        {
            var newHierarchyClassModel = new InforHierarchyClassModel
            {
                HierarchyClassId = 87654323,
                HierarchyClassName = "Test National HierarchyClass",
                HierarchyName = Hierarchies.Names.National,
                ParentHierarchyClassId = 0,
                HierarchyLevelName = HierarchyLevelNames.NationalFamily,
                HierarchyClassTraits = new Dictionary<string, string>
                {
                    { Traits.Codes.NationalClassCode, "Test National HierarchyClass" }
                }
            };
            command.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                newHierarchyClassModel
            };

            commandHandler.Execute(command);

            return newHierarchyClassModel;
        }
    }
}
