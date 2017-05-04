﻿using Icon.ApiController.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Icon.ApiController.Tests.Commands
{
    [TestClass]
    public class UpdateSentToEsbHierarchyTraitCommandHandlerIntegrationTests
    {
        private UpdateSentToEsbHierarchyTraitCommandHandler commandHandler;
        private Mock<ILogger<UpdateSentToEsbHierarchyTraitCommandHandler>> mockLogger;
        private IconContext context;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            context = new IconContext();

            mockLogger = new Mock<ILogger<UpdateSentToEsbHierarchyTraitCommandHandler>>();
            commandHandler = new UpdateSentToEsbHierarchyTraitCommandHandler(mockLogger.Object, new IconDbContextFactory());
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void UpdateSentToEsbHierarchyTrait_InvalidInput_ErrorShouldBeLogged()
        {
            // Given.
            UpdateSentToEsbHierarchyTraitCommand nullList = new UpdateSentToEsbHierarchyTraitCommand
            {
                PublishedHierarchyClasses = null
            };
            UpdateSentToEsbHierarchyTraitCommand emptyList = new UpdateSentToEsbHierarchyTraitCommand
            {
                PublishedHierarchyClasses = new List<int>()
            };

            // When.
            commandHandler.Execute(nullList);
            commandHandler.Execute(emptyList);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void UpdateSentToEsbHierarchyTrait_HierarchyExistsWithSentToEsbTrait_SentToEsbTraitIsUpdatedToCurrentTime()
        {
            // Given.
            var testHierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestHierarchyClass",
                hierarchyLevel = HierarchyLevels.Parent,
                hierarchyID = Hierarchies.Brands
            };

            context.HierarchyClass.Add(testHierarchyClass);
            context.SaveChanges();

            testHierarchyClass.HierarchyClassTrait.Add(new HierarchyClassTrait
            {
                Trait = context.Trait.Single(t => t.traitCode == TraitCodes.SentToEsb),
                traitID = Traits.SentToEsb,
                hierarchyClassID = testHierarchyClass.hierarchyClassID,
                HierarchyClass = testHierarchyClass,
                traitValue = null
            });

            context.SaveChanges();

            // When.
            commandHandler.Execute(new UpdateSentToEsbHierarchyTraitCommand
            {
                PublishedHierarchyClasses = new List<int> { testHierarchyClass.hierarchyClassID }
            });

            // Then.
            var result = context.HierarchyClass
                .AsNoTracking()
                .Single(hc => hc.hierarchyClassID == testHierarchyClass.hierarchyClassID)
                .HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.SentToEsb)
                .traitValue;

            Assert.IsNotNull(result);
            Assert.IsTrue(DateTime.Parse(result).Date == DateTime.Today);
        }

        [TestMethod]
        public void UpdateSentToEsbHierarchyTrait_HierarchyExistsButDoesNotHaveSentToEsbTrait_SentToEsbTraitShouldBeCreatedAndPopulated()
        {
            // Given.
            var testHierarchyClass = new HierarchyClass
            {
                hierarchyClassName = "TestHierarchyClass",
                hierarchyLevel = HierarchyLevels.Parent,
                hierarchyID = Hierarchies.Brands
            };

            context.HierarchyClass.Add(testHierarchyClass);
            context.SaveChanges();

            // When.
            commandHandler.Execute(new UpdateSentToEsbHierarchyTraitCommand
            {
                PublishedHierarchyClasses = new List<int> { testHierarchyClass.hierarchyClassID }
            });

            // Then.
            var result = context.HierarchyClass
                .AsNoTracking()
                .Single(hc => hc.hierarchyClassID == testHierarchyClass.hierarchyClassID)
                .HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.SentToEsb)
                .traitValue;

            Assert.IsNotNull(result);
            Assert.IsTrue(DateTime.Parse(result).Date == DateTime.Today);
        }
    }
}
