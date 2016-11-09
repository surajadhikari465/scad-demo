using Icon.Common.Context;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class ValidateHierarchyClassesCommandHandlerTests
    {
        private ValidateHierarchyClassesCommandHandler commandHandler;
        private ValidateHierarchyClassesCommand command;
        private Mock<IRenewableContext<IconContext>> mockGlobalContext;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockGlobalContext = new Mock<IRenewableContext<IconContext>>();
            mockGlobalContext.SetupGet(m => m.Context).Returns(context);

            commandHandler = new ValidateHierarchyClassesCommandHandler(mockGlobalContext.Object);
            command = new ValidateHierarchyClassesCommand { HierarchyClasses = new List<HierarchyClassModel>() };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void ValidateHierarchyClasses_NoDuplicates_NoError()
        {
            //Given
            command.HierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel { HierarchyClassId = 1234, HierarchyClassName = "Duplicate Test", HierarchyName = Hierarchies.Names.Brands, HierarchyLevelName = HierarchyLevelNames.Brand }
            };

            //When
            commandHandler.Execute(command);

            //Then
            AssertValidationTest(null, null);
        }

        [TestMethod]
        public void ValidateHierarchyClasses_HierarchyMismatch_HierarchyMismatchError()
        {
            //Given
            var hierarchyClass = context.HierarchyClass.Add(new Framework.HierarchyClass
            {
                hierarchyClassName = "HierarchyMismatch Test",
                hierarchyID = Hierarchies.Tax,
                hierarchyLevel = HierarchyLevels.Tax
            });
            context.SaveChanges();
            command.HierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel
                {
                    HierarchyClassId = hierarchyClass.hierarchyClassID,
                    HierarchyClassName = "Add Or Update Hierarchy Class",
                    HierarchyName = Hierarchies.Names.National,
                    HierarchyLevelName = HierarchyLevelNames.NationalFamily
                }
            };

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorCode = "HierarchyMismatch";
            string expectedErrorDetails = "A hierarchy class with the same ID exists under a different hierarchy than 'National'.";
            AssertValidationTest(expectedErrorCode, expectedErrorDetails);
        }

        [TestMethod]
        public void ValidateHierarchyClasses_DuplicateBrand_DuplicateHierarchyClassError()
        {
            //Given
            command.HierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel { HierarchyClassId = 1234, HierarchyClassName = "Duplicate Test", HierarchyName = Hierarchies.Names.Brands, HierarchyLevelName = HierarchyLevelNames.Brand }
            };
            context.HierarchyClass.Add(new Framework.HierarchyClass
            {
                hierarchyClassName = "Duplicate Test",
                hierarchyID = Hierarchies.Brands,
                hierarchyLevel = HierarchyLevels.Brand
            });
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorCode = "DuplicateHierarchyClass";
            string expectedErrorDetails = "A hierarchy class already exists with the name of 'Duplicate Test' and with the same hierarchy, hierarchy level, and parent hierarchy class.";
            AssertValidationTest(expectedErrorCode, expectedErrorDetails);
        }

        [TestMethod]
        public void ValidateHierarchyClasses_DuplicateMerchandise_DuplicateHierarchyClassError()
        {
            //Given
            command.HierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel { HierarchyClassId = 1234, HierarchyClassName = "Duplicate Test", HierarchyName = Hierarchies.Names.Merchandise, HierarchyLevelName = HierarchyLevelNames.Segment }
            };
            context.HierarchyClass.Add(new Framework.HierarchyClass
            {
                hierarchyClassName = "Duplicate Test",
                hierarchyID = Hierarchies.Merchandise,
                hierarchyLevel = HierarchyLevels.Segment
            });
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorCode = "DuplicateHierarchyClass";
            string expectedErrorDetails = "A hierarchy class already exists with the name of 'Duplicate Test' and with the same hierarchy, hierarchy level, and parent hierarchy class.";
            AssertValidationTest(expectedErrorCode, expectedErrorDetails);
        }

        [TestMethod]
        public void ValidateHierarchyClasses_DuplicateTax_DuplicateHierarchyClassError()
        {
            //Given
            command.HierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel { HierarchyClassId = 1234, HierarchyClassName = "Duplicate Test", HierarchyName = Hierarchies.Names.Tax, HierarchyLevelName = HierarchyLevelNames.Tax }
            };
            context.HierarchyClass.Add(new Framework.HierarchyClass
            {
                hierarchyClassName = "Duplicate Test",
                hierarchyID = Hierarchies.Tax,
                hierarchyLevel = HierarchyLevels.Tax
            });
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorCode = "DuplicateHierarchyClass";
            string expectedErrorDetails = "A hierarchy class already exists with the name of 'Duplicate Test' and with the same hierarchy, hierarchy level, and parent hierarchy class.";
            AssertValidationTest(expectedErrorCode, expectedErrorDetails);
        }

        [TestMethod]
        public void ValidateHierarchyClasses_NoDuplicateSubBrickCode_NoError()
        {
            //Given
            command.HierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel
                {
                    HierarchyClassId = 1234,
                    HierarchyClassName = "Duplicate Test",
                    HierarchyName = Hierarchies.Names.Merchandise,
                    HierarchyLevelName = HierarchyLevelNames.SubBrick,
                    HierarchyClassTraits = new Dictionary<string, string>
                    {
                        { TraitCodes.SubBrickCode, "123456789" }
                    }
                }
            };
            context.HierarchyClass.Add(new Framework.HierarchyClass
            {
                hierarchyClassName = "Duplicate Test 2",
                hierarchyID = Hierarchies.Merchandise,
                hierarchyLevel = HierarchyLevels.SubBrick,
                HierarchyClassTrait = new List<HierarchyClassTrait> { new HierarchyClassTrait { traitID = Traits.SubBrickCode, traitValue = "123456789145" } }
            });
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorCode = null;
            string expectedErrorDetails = null;
            AssertValidationTest(expectedErrorCode, expectedErrorDetails);
        }

        [TestMethod]
        public void ValidateHierarchyClasses_DuplicateSubBrickCode_DuplicateSubBrickCodeError()
        {
            //Given
            context.HierarchyClass.Add(new Framework.HierarchyClass
            {
                hierarchyClassName = "Duplicate Test",
                hierarchyID = Hierarchies.Merchandise,
                hierarchyLevel = HierarchyLevels.SubBrick,
                HierarchyClassTrait = new List<HierarchyClassTrait> { new HierarchyClassTrait { traitID = Traits.SubBrickCode, traitValue = "123456789" } }
            });
            context.SaveChanges();

            command.HierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel
                {
                    HierarchyClassId = 1234,
                    HierarchyClassName = "Duplicate Test 2",
                    HierarchyName = Hierarchies.Names.Merchandise,
                    HierarchyLevelName = HierarchyLevelNames.SubBrick,
                    HierarchyClassTraits = new Dictionary<string, string>
                    {
                        { TraitCodes.SubBrickCode, "123456789" }
                    }
                }
            };

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorCode = "DuplicateSubBrickCode";
            string expectedErrorDetails = "Sub-Brick Code '123456789' already exists. Sub-Brick Codes must be unique.";
            AssertValidationTest(expectedErrorCode, expectedErrorDetails);
        }

        private void AssertValidationTest(string expectedErrorCode, string expectedErrorDetails)
        {
            Assert.AreEqual(expectedErrorCode, command.HierarchyClasses.First().ErrorCode);
            Assert.AreEqual(expectedErrorDetails, command.HierarchyClasses.First().ErrorDetails);
        }
    }
}
