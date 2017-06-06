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
using System.Data.SqlClient;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateHierarchyClassesCommandHandlerTests : BaseHierarchyClassesCommandTest
    {
        private AddOrUpdateHierarchyClassesCommandHandler commandHandler;
        private AddOrUpdateHierarchyClassesCommand command;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new AddOrUpdateHierarchyClassesCommandHandler(
                mockRenewableContext.Object,
                regions);

            command = new AddOrUpdateHierarchyClassesCommand();
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClassesCommand_WhenBrandDoesNotExist_ShouldAddBrandClass()
        {
            //Given
            var testModel = base.CreateInforHierarchyClassModel(HierarchyNames.Brands,
                HierarchyLevelNames.Brand, ActionEnum.AddOrUpdate, new Dictionary<string, string>
                {
                    { Traits.Codes.BrandAbbreviation, "Test HierarchyClass" }
                });
            AddHierarchyClassModelToCommandData(command, testModel);
            var countBefore = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.Brands);
            //var testModel = this.GetAndPrepModelForAddTest(command, base.GetBrandHierarchyClassModel());
            //When
            commandHandler.Execute(command);
            //Then
            var countAfter = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.Brands);
            Assert.AreEqual(countAfter, countBefore + 1, $"Should have found 1 more hierarchy class after Add");
            this.AssertNewHierarchyClassWasAdded(
                testModel: testModel,
                hierarchyLevel: HierarchyLevels.Brand,
                expectedNumberOfEvents: this.regions.Count,
                expectedNumberOfMessages: 1);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClassesCommand_WhenBrandExists_ShouldUpdateBrandClass()
        {
            //Given
            var testModel = base.CreateInforHierarchyClassModel(HierarchyNames.Brands,
                HierarchyLevelNames.Brand, ActionEnum.AddOrUpdate, new Dictionary<string, string>
                {
                    { Traits.Codes.BrandAbbreviation, "Test HierarchyClass" }
                });
            this.PrepModelForUpdateTest(command, testModel);
            var countBefore = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.Brands);
            //When
            commandHandler.Execute(command);
            //Then
            var countAfter = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.Brands);
            Assert.AreEqual(countAfter, countBefore, $"Should have found same count of hierarchy classes after Update");
            this.AssertExistingHierarchyClassWasUpdated(
                testModel: testModel,
                hierarchyLevel: HierarchyLevels.Brand,
                traitCode: Traits.Codes.BrandAbbreviation,
                expectedNumberOfTraits: 1,
                expectedNumberOfEvents: regions.Count * 2,
                expectedNumberOfMessages: 2);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClassesCommand_WhenNationalDoesNotExist_ShouldAddNationalClass()
        {
            //Given
            var testModel = base.CreateInforHierarchyClassModel(HierarchyNames.National,
                HierarchyLevelNames.NationalFamily, ActionEnum.AddOrUpdate, new Dictionary<string, string>
                {
                     { Traits.Codes.NationalClassCode, "Test National HierarchyClass" }
                });
            AddHierarchyClassModelToCommandData(command, testModel);
            var countBefore = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.National);
            //When
            commandHandler.Execute(command);
            //Then
            var countAfter = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.National);
            Assert.AreEqual(countAfter, countBefore + 1, $"Should have found 1 more hierarchy class after Add");
            this.AssertNewHierarchyClassWasAdded(
                testModel: testModel,
                hierarchyLevel: HierarchyLevels.NationalFamily,
                expectedNumberOfEvents: 0,
                expectedNumberOfMessages: 0);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClassesCommand_WhenNationalExists_ShouldUpdateNationalClass()
        {
            //Given
            var testModel = base.CreateInforHierarchyClassModel(HierarchyNames.National,
                HierarchyLevelNames.NationalFamily, ActionEnum.AddOrUpdate, new Dictionary<string, string>
                {
                     { Traits.Codes.NationalClassCode, "Test National HierarchyClass" }
                });
            this.PrepModelForUpdateTest(command, testModel);
            var countBefore = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.National);
            //When
            commandHandler.Execute(command);
            //Then
            var countAfter = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.National);
            Assert.AreEqual(countAfter, countBefore, $"Should have found same count of hierarchy classes after Update");
            this.AssertExistingHierarchyClassWasUpdated(
                testModel: testModel,
                hierarchyLevel: HierarchyLevels.NationalFamily,
                traitCode: Traits.Codes.NationalClassCode,
                expectedNumberOfTraits: 1,
                expectedNumberOfEvents: 0,
                expectedNumberOfMessages: 0);
        }

        protected InforHierarchyClassModel PrepModelForUpdateTest(
            AddOrUpdateHierarchyClassesCommand testCommand,
            InforHierarchyClassModel testModel,
            string traitCode = null)
        {
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

            SaveModelToDb(context, testCommand, testModel);

            AddHierarchyClassModelToCommandData(testCommand, testModel);

            return testModel;
        }

        protected void SaveModelToDb(IconContext iconContext,
            AddOrUpdateHierarchyClassesCommand testCommand,
            InforHierarchyClassModel model)
        {
            testCommand.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                model
            };

            commandHandler.Execute(command);
        }

        protected void AddHierarchyClassModelToCommandData(
            AddOrUpdateHierarchyClassesCommand addOrUpdateHierarchyClassesCommand,
            InforHierarchyClassModel hierarchyClassModel)
        {
            addOrUpdateHierarchyClassesCommand.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                hierarchyClassModel
            };
        }

        protected void AssertNewHierarchyClassWasAdded(
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
            var queuedEvents = GetQueuedEvents(context, testModel.HierarchyClassName);
            Assert.AreEqual(expectedNumberOfEvents, queuedEvents.Count());

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

        protected void AssertExistingHierarchyClassWasUpdated(
            InforHierarchyClassModel testModel,
            int hierarchyLevel,
            string traitCode,
            int expectedNumberOfTraits,
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
            Assert.AreEqual(expectedNumberOfTraits, hierarchyClass.HierarchyClassTrait.Count);
            Assert.AreEqual(testModel.HierarchyClassTraits.Count, hierarchyClass.HierarchyClassTrait.Count);
            Assert.AreEqual(hierarchyLevel, hierarchyClass.hierarchyLevel);

            foreach (var hierarchyClassTrait in hierarchyClass.HierarchyClassTrait)
            {
                var testModelTrait = testModel.HierarchyClassTraits[Traits.Codes.AsDictionary[hierarchyClassTrait.traitID]];
                Assert.AreEqual(testModelTrait, hierarchyClassTrait.traitValue);
            };

            AssertExpectedEventsAndMessagesWereGenerated(testModel, expectedNumberOfEvents, expectedNumberOfMessages);

            if (expectedNumberOfMessages > 0)
            {
                string expectedMessageHierarchyClassId = GetExpectedMessageHierarchyClassId(testModel);
                var message = context.MessageQueueHierarchy
                    .Where(m => m.HierarchyClassId == expectedMessageHierarchyClassId)
                    .OrderByDescending(m=>m.MessageQueueId).FirstOrDefault();
                Assert.AreEqual(testModel.HierarchyClassName, message.HierarchyClassName);
                Assert.AreEqual(Hierarchies.Ids[testModel.HierarchyName], message.HierarchyId);
                Assert.AreEqual(testModel.ParentHierarchyClassId.ToHierarchyParentClassId(), message.HierarchyParentClassId);
                Assert.AreEqual(testModel.HierarchyLevelName, message.HierarchyLevelName);
                Assert.AreEqual(hierarchyLevel, message.HierarchyLevel);
            }
        }

        protected static string GetExpectedMessageHierarchyClassId(InforHierarchyClassModel testModel)
        {
            string expectedMessageHierarchyClassId = testModel.HierarchyName == Hierarchies.Names.Financial
                ? testModel.HierarchyClassName.Split('(')[1].TrimEnd(')')
                : testModel.HierarchyClassId.ToString();
            return expectedMessageHierarchyClassId;
        }

        protected void AssertExpectedEventsAndMessagesWereGenerated( InforHierarchyClassModel testModel,
            int expectedNumberOfEvents, int expectedNumberOfMessages)
        {
            //Assert Events are generated
            var queuedEvents = GetQueuedEvents(context, testModel.HierarchyClassId);
            Assert.AreEqual(expectedNumberOfEvents, queuedEvents.Count());

            //Assert Messages are generated
            string expectedMessageHierarchyClassId = GetExpectedMessageHierarchyClassId(testModel);
            var messageCount = context.MessageQueueHierarchy
                .Where(m => m.HierarchyClassId == expectedMessageHierarchyClassId).Count();
            Assert.AreEqual(expectedNumberOfMessages, messageCount);
        }
    }
}