using GlobalEventController.DataAccess.Commands;
using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddOrUpdateNationalHierarchyCommandHandlerTests
    {
        private AddOrUpdateNationalHierarchyCommandHandler commandHandler;
        private AddOrUpdateNationalHierarchyCommand command;
        private IrmaDbContextFactory contextFactory;
        private IrmaContext context;
        private Mock<ILogger<AddOrUpdateNationalHierarchyCommandHandler>> mockLogger;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            this.transaction = new TransactionScope();
            this.context = new IrmaContext();
            this.command = new AddOrUpdateNationalHierarchyCommand();
            this.contextFactory = new IrmaDbContextFactory();
            this.mockLogger = new Mock<ILogger<AddOrUpdateNationalHierarchyCommandHandler>>();
            this.commandHandler = new AddOrUpdateNationalHierarchyCommandHandler(contextFactory, mockLogger.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_AddNewNationalFamily_ShouldSkipAddingNationalFamily()
        {
            //Given
            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyClassID = 100000,
                hierarchyLevel = HierarchyLevels.NationalFamily,
                hierarchyID = Hierarchies.National,
                hierarchyParentClassID = null,
                hierarchyClassName = "Test Family"
            };
            command.HierarchyClass = hierarchyClass;

            //When
            commandHandler.Handle(command);

            //Then
            var validatedNationalClass = context.ValidatedNationalClass.SingleOrDefault(vnc => vnc.IconId == hierarchyClass.hierarchyClassID);
            var family = context.NatItemFamily.SingleOrDefault(nif => nif.NatFamilyName == hierarchyClass.hierarchyClassName);

            Assert.IsNull(validatedNationalClass);
            Assert.IsNull(family);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_AddNewNationalCategory_ShouldAddNewNationalCategoryAndItsParentNationalFamily()
        {
            //Given
            NatItemFamily testNatItemFamily = context.NatItemFamily.Add(new NatItemFamily { NatFamilyName = "Test Family" });
            context.SaveChanges();
            HierarchyClass testParentHierarchyClass = new HierarchyClass()
            {
                hierarchyClassID = 100000,
                hierarchyLevel = HierarchyLevels.NationalFamily,
                hierarchyID = Hierarchies.National,
                hierarchyParentClassID = null,
                hierarchyClassName = "Test Family"
            };
            HierarchyClass testHierarchyClass = new HierarchyClass()
            {
                hierarchyClassID = 100001,
                hierarchyLevel = HierarchyLevels.NationalCategory,
                hierarchyID = Hierarchies.National,
                hierarchyParentClassID = testParentHierarchyClass.hierarchyParentClassID,
                hierarchyClassName = "Test Category"
            };
            command.ParentHierarchyClass = testParentHierarchyClass;
            command.HierarchyClass = testHierarchyClass;

            //When
            commandHandler.Handle(command);

            //Then
            var familyValidatedNationalClass = context.ValidatedNationalClass.SingleOrDefault(vnc => vnc.IconId == testParentHierarchyClass.hierarchyClassID);
            var categoryValidatedNationalClass = context.ValidatedNationalClass.SingleOrDefault(vnc => vnc.IconId == testHierarchyClass.hierarchyClassID);
            var natItemFamily = context.NatItemFamily.Single(nif => nif.NatFamilyID == categoryValidatedNationalClass.IrmaId);

            Assert.IsNotNull(familyValidatedNationalClass);
            Assert.IsNotNull(categoryValidatedNationalClass);
            Assert.IsNotNull(natItemFamily);
            Assert.AreNotEqual(familyValidatedNationalClass.IconId, categoryValidatedNationalClass.IconId);
            Assert.AreEqual(HierarchyLevels.NationalCategory, categoryValidatedNationalClass.Level);
            Assert.AreEqual($"{testParentHierarchyClass.hierarchyClassName} - {testHierarchyClass.hierarchyClassName}", natItemFamily.NatFamilyName);
            Assert.IsNull(natItemFamily.NatSubTeam_No);
            Assert.IsNull(natItemFamily.SubTeam_No);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_AddNewNationalSubCategory_ShouldAddNewNationalSubCategory()
        {
            //Given
            var testNatItemFamily = context.NatItemFamily.Add(new NatItemFamily
            {
                NatFamilyName = "Test Family - Test Category",
            });
            context.SaveChanges();
            context.ValidatedNationalClass.Add(new ValidatedNationalClass { IconId = 100000, IrmaId = testNatItemFamily.NatFamilyID, Level = HierarchyLevels.NationalCategory });
            context.SaveChanges();
            var testHierarchyClass = new HierarchyClass
            {
                hierarchyClassID = 100001,
                hierarchyClassName = "Test Sub Category",
                hierarchyLevel = HierarchyLevels.NationalSubCategory,
                hierarchyParentClassID = 100000,
                hierarchyID = Hierarchies.National
            };
            command.HierarchyClass = testHierarchyClass;

            //When
            commandHandler.Handle(command);

            //Then
            var validatedNationalClass = context.ValidatedNationalClass.SingleOrDefault(vnc => vnc.IconId == testHierarchyClass.hierarchyClassID);
            var natItemCat = context.NatItemCat.SingleOrDefault(nic => nic.NatCatID == validatedNationalClass.IrmaId);

            Assert.IsNotNull(validatedNationalClass);
            Assert.IsNotNull(natItemCat);
            Assert.AreEqual(testHierarchyClass.hierarchyClassName, natItemCat.NatCatName);
            Assert.AreEqual(testNatItemFamily.NatFamilyID, natItemCat.NatFamilyID);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_AddNewNationalClass_ShouldAddNewNationalClass()
        {
            //Given
            var expectedClassId = 8888888;
            var testNatItemCat = context.NatItemCat.Add(new NatItemCat
            {
                NatCatName = "Test Sub Category",
            });
            context.SaveChanges();
            context.ValidatedNationalClass.Add(new ValidatedNationalClass
                {
                    IconId = 100000,
                    IrmaId = testNatItemCat.NatCatID,
                    Level = HierarchyLevels.NationalSubCategory
                });
            context.SaveChanges();

            var testHierarchyClass = new HierarchyClass
            {
                hierarchyClassID = 100001,
                hierarchyClassName = "Test Class",
                hierarchyLevel = HierarchyLevels.NationalClass,
                hierarchyParentClassID = 100000,
                hierarchyID = Hierarchies.National,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.NationalClassCode, traitValue = expectedClassId.ToString() }
                }
            };
            command.HierarchyClass = testHierarchyClass;

            //When
            commandHandler.Handle(command);

            //Then
            var validatedNationalClass = context.ValidatedNationalClass.SingleOrDefault(vnc => vnc.IconId == testHierarchyClass.hierarchyClassID);
            var natItemClass = context.NatItemClass.SingleOrDefault(nic => nic.ClassID == validatedNationalClass.IrmaId);

            Assert.IsNotNull(validatedNationalClass);
            Assert.IsNotNull(natItemClass);
            Assert.AreEqual(expectedClassId, natItemClass.ClassID);
            Assert.AreEqual(testHierarchyClass.hierarchyClassName, natItemClass.ClassName);
            Assert.AreEqual(testNatItemCat.NatCatID, natItemClass.NatCatID);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_UpdateExistingNationalFamily_ShouldChangeTheNameOfTheNationalFamily()
        {
            //Given
            var testCategoryName = "Test Category";
            var testNatItemFamily = context.NatItemFamily.Add(new NatItemFamily
            {
                NatFamilyName = $"Test Family - {testCategoryName}"
            });
            context.SaveChanges();
            context.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 100000,
                IrmaId = testNatItemFamily.NatFamilyID,
                Level = HierarchyLevels.NationalFamily
            });
            context.SaveChanges();
            var testHierarchyClass = new HierarchyClass
            {
                hierarchyClassID = 100000,
                hierarchyLevel = HierarchyLevels.NationalFamily,
                hierarchyID = Hierarchies.National,
                hierarchyParentClassID = null,
                hierarchyClassName = "Test Family Updated"
            };
            command.HierarchyClass = testHierarchyClass;

            //When
            commandHandler.Handle(command);

            //Then
            var validatedNationalClass = context.ValidatedNationalClass.AsNoTracking().SingleOrDefault(vnc => vnc.IconId == testHierarchyClass.hierarchyClassID);
            var natItemFamily = context.NatItemFamily.AsNoTracking().SingleOrDefault(nif => nif.NatFamilyID == validatedNationalClass.IrmaId);

            Assert.IsNotNull(validatedNationalClass);
            Assert.IsNotNull(natItemFamily);
            Assert.AreEqual($"{testHierarchyClass.hierarchyClassName} - {testCategoryName}", natItemFamily.NatFamilyName);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_UpdateExistingNationalCategory_ShouldChangeTheNameOfTheNationalCategoryInTheNatItemFamily()
        {
            //Given
            var testFamilyName = "Test Family";
            var testNatItemFamily = context.NatItemFamily.Add(new NatItemFamily
            {
                NatFamilyName = $"{testFamilyName} - Test Category"
            });
            context.SaveChanges();
            context.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 100000,
                IrmaId = testNatItemFamily.NatFamilyID,
                Level = HierarchyLevels.NationalCategory
            });
            context.SaveChanges();
            var testHierarchyClass = new HierarchyClass
            {
                hierarchyClassID = 100000,
                hierarchyLevel = HierarchyLevels.NationalCategory,
                hierarchyID = Hierarchies.National,
                hierarchyParentClassID = null,
                hierarchyClassName = "Test Category Updated"
            };
            command.HierarchyClass = testHierarchyClass;

            //When
            commandHandler.Handle(command);

            //Then
            var validatedNationalClass = context.ValidatedNationalClass.AsNoTracking().SingleOrDefault(vnc => vnc.IconId == testHierarchyClass.hierarchyClassID);
            var natItemFamily = context.NatItemFamily.AsNoTracking().SingleOrDefault(nif => nif.NatFamilyID == validatedNationalClass.IrmaId);

            Assert.IsNotNull(validatedNationalClass);
            Assert.IsNotNull(natItemFamily);
            Assert.AreEqual($"{testFamilyName} - {testHierarchyClass.hierarchyClassName}", natItemFamily.NatFamilyName);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_UpdateExistingNationalSubCategory_ShouldChangeTheNameOfTheNationalSubCategory()
        {
            //Given
            var testNatItemCat = context.NatItemCat.Add(new NatItemCat
            {
                NatCatName = "Test Sub Category"
            });
            context.SaveChanges();
            context.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 100000,
                IrmaId = testNatItemCat.NatCatID,
                Level = HierarchyLevels.NationalSubCategory
            });
            context.SaveChanges();
            var testHierarchyClass = new HierarchyClass
            {
                hierarchyClassID = 100000,
                hierarchyLevel = HierarchyLevels.NationalSubCategory,
                hierarchyID = Hierarchies.National,
                hierarchyParentClassID = null,
                hierarchyClassName = "Test Sub Category Updated"
            };
            command.HierarchyClass = testHierarchyClass;

            //When
            commandHandler.Handle(command);

            //Then
            var validatedNationalClass = context.ValidatedNationalClass.AsNoTracking().SingleOrDefault(vnc => vnc.IconId == testHierarchyClass.hierarchyClassID);
            var natItemCat = context.NatItemCat.AsNoTracking().SingleOrDefault(nic => nic.NatCatID == validatedNationalClass.IrmaId);

            Assert.IsNotNull(validatedNationalClass);
            Assert.IsNotNull(natItemCat);
            Assert.AreEqual(testHierarchyClass.hierarchyClassName, natItemCat.NatCatName);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_UpdateExistingNationalClass_ShouldChangeTheNameOfTheNationalClass()
        {
            //Given
            var testNatItemClass = context.NatItemClass.Add(new NatItemClass
            {
                ClassName = "Test Class"
            });
            context.SaveChanges();
            context.ValidatedNationalClass.Add(new ValidatedNationalClass
            {
                IconId = 100000,
                IrmaId = testNatItemClass.ClassID,
                Level = HierarchyLevels.NationalClass
            });
            context.SaveChanges();
            var testHierarchyClass = new HierarchyClass
            {
                hierarchyClassID = 100000,
                hierarchyLevel = HierarchyLevels.NationalClass,
                hierarchyID = Hierarchies.National,
                hierarchyParentClassID = null,
                hierarchyClassName = "Test Class Updated"
            };
            command.HierarchyClass = testHierarchyClass;

            //When
            commandHandler.Handle(command);

            //Then
            var validatedNationalClass = context.ValidatedNationalClass.AsNoTracking().SingleOrDefault(vnc => vnc.IconId == testHierarchyClass.hierarchyClassID);
            var natItemClass = context.NatItemClass.AsNoTracking().SingleOrDefault(nicl => nicl.ClassID == validatedNationalClass.IrmaId);

            Assert.IsNotNull(validatedNationalClass);
            Assert.IsNotNull(natItemClass);
            Assert.AreEqual(testHierarchyClass.hierarchyClassName, natItemClass.ClassName);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_UpdateNationalFamilyThatHasMultipleNatItemFamilyInIrma_ShouldChangeTheNameOfAllNatItemFamiliesInIrma()
        {
            //Given
            var initialIconId = 1000000;
            var testCategoryName = "Test Category";
            var expectedNumberOfNationalCategories = 10;
            var testParentHierarchyClass = new HierarchyClass
            {
                hierarchyClassID = initialIconId,
                hierarchyClassName = "Test Family",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalFamily
            };
            command.ParentHierarchyClass = testParentHierarchyClass;

            for (int i = 1; i <= expectedNumberOfNationalCategories; i++)
            {
                var testHierarchyClass = new HierarchyClass
                {
                    hierarchyClassID = i + initialIconId,
                    hierarchyClassName = $"{testCategoryName} {i}",
                    hierarchyParentClassID = testParentHierarchyClass.hierarchyClassID,
                    hierarchyID = Hierarchies.National,
                    hierarchyLevel = HierarchyLevels.NationalCategory
                };
                command.HierarchyClass = testHierarchyClass;
                commandHandler.Handle(command);
            }

            //When
            command.HierarchyClass = new HierarchyClass
            {
                hierarchyClassID = initialIconId,
                hierarchyClassName = "Test Family Updated",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalFamily
            };
            command.ParentHierarchyClass = null;
            commandHandler.Handle(command);

            //Then
            var expectedNumberOfNationalFamilies = expectedNumberOfNationalCategories;
            var validatedNationalClasses = context.ValidatedNationalClass
                .Where(vnc => vnc.IconId >= initialIconId && vnc.IconId <= initialIconId + expectedNumberOfNationalCategories)
                .ToList();
            Assert.AreEqual(expectedNumberOfNationalFamilies + expectedNumberOfNationalCategories, validatedNationalClasses.Count);
            Assert.AreEqual(expectedNumberOfNationalFamilies, validatedNationalClasses.Count(vnc => vnc.IconId == initialIconId));

            var categoryIds = validatedNationalClasses
                .Where(vnc => vnc.IconId > initialIconId)
                .Select(vnc => vnc.IconId.Value)
                .OrderBy(id => id).ToList();
            var expectedCategoryIds = Enumerable.Range(initialIconId + 1, expectedNumberOfNationalCategories);
            Assert.IsTrue(categoryIds.SequenceEqual(expectedCategoryIds));

            var familyIrmaIds = validatedNationalClasses
                .Where(vnc => vnc.IconId == initialIconId)
                .Select(vnc => vnc.IrmaId)
                .ToList();
            var categoryIrmaIds = validatedNationalClasses
                .Where(vnc => vnc.IconId > initialIconId)
                .Select(vnc => vnc.IrmaId)
                .ToList();
            Assert.IsTrue(familyIrmaIds.SequenceEqual(categoryIrmaIds));

            var natItemFamilies = context.NatItemFamily
                .Where(nif => categoryIrmaIds.Contains(nif.NatFamilyID))
                .OrderBy(nif => nif.NatFamilyID)
                .ToList();
            for (int i = 0; i < expectedNumberOfNationalCategories; i++)
            {
                Assert.AreEqual($"{command.HierarchyClass.hierarchyClassName} - {testCategoryName} {i + 1}", natItemFamilies[i].NatFamilyName);
            }
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_SendAllLevelsOfANationalHierarchy_ShouldAddAllLevelsToIrma()
        {
            //Given
            int testNationalClassCode1 = 5555555;
            int testNationalClassCode2 = 5555556;

            var testFamily = new HierarchyClass
            {
                hierarchyClassID = 1000000,
                hierarchyClassName = "Test Family",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalFamily
            };
            var testCategory = new HierarchyClass
            {
                hierarchyClassID = 1000001,
                hierarchyClassName = "Test Category",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalCategory,
                hierarchyParentClassID = testFamily.hierarchyClassID
            };
            var testSubCategory = new HierarchyClass
            {
                hierarchyClassID = 1000002,
                hierarchyClassName = "Test Sub Category",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalSubCategory,
                hierarchyParentClassID = testCategory.hierarchyClassID
            };
            var testClass1 = new HierarchyClass
            {
                hierarchyClassID = 1000003,
                hierarchyClassName = "Test Class 1",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalClass,
                hierarchyParentClassID = testSubCategory.hierarchyClassID,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.NationalClassCode, traitValue = testNationalClassCode1.ToString()}
                }
            };
            var testClass2 = new HierarchyClass
            {
                hierarchyClassID = 1000004,
                hierarchyClassName = "Test Class 2",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalClass,
                hierarchyParentClassID = testSubCategory.hierarchyClassID,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.NationalClassCode, traitValue = testNationalClassCode2.ToString()}
                }
            };

            //When
            command.HierarchyClass = testFamily;
            commandHandler.Handle(command);

            command.HierarchyClass = testCategory;
            command.ParentHierarchyClass = testFamily;
            commandHandler.Handle(command);

            command.HierarchyClass = testSubCategory;
            command.ParentHierarchyClass = testCategory;
            commandHandler.Handle(command);

            command.HierarchyClass = testClass1;
            command.ParentHierarchyClass = testSubCategory;
            commandHandler.Handle(command);

            command.HierarchyClass = testClass2;
            command.ParentHierarchyClass = testSubCategory;
            commandHandler.Handle(command);


            //Then
            var familyValidatedNationalClass = context.ValidatedNationalClass.Single(vnc => vnc.IconId == testFamily.hierarchyClassID);
            var familyNatItemFamily = context.NatItemFamily.Single(nif => nif.NatFamilyID == familyValidatedNationalClass.IrmaId);
            Assert.AreEqual($"{testFamily.hierarchyClassName} - {testCategory.hierarchyClassName}", familyNatItemFamily.NatFamilyName);

            var categoryValidatedNationalClass = context.ValidatedNationalClass.Single(vnc => vnc.IconId == testFamily.hierarchyClassID);
            var categoryNatItemFamily = context.NatItemFamily.Single(nif => nif.NatFamilyID == categoryValidatedNationalClass.IrmaId);
            Assert.AreEqual($"{testFamily.hierarchyClassName} - {testCategory.hierarchyClassName}", categoryNatItemFamily.NatFamilyName);
            Assert.AreEqual(familyNatItemFamily.NatFamilyID, categoryNatItemFamily.NatFamilyID);

            var subCategoryValidatedNationalClass = context.ValidatedNationalClass.Single(vnc => vnc.IconId == testSubCategory.hierarchyClassID);
            var natItemCat = context.NatItemCat.Single(nic => nic.NatCatID == subCategoryValidatedNationalClass.IrmaId);
            Assert.AreEqual(testSubCategory.hierarchyClassName, natItemCat.NatCatName);
            Assert.AreEqual(categoryNatItemFamily.NatFamilyID, natItemCat.NatFamilyID);

            var classValidatedNationalClass1 = context.ValidatedNationalClass.Single(vnc => vnc.IconId == testClass1.hierarchyClassID);
            var natItemClass1 = context.NatItemClass.Single(nicl => nicl.ClassID == classValidatedNationalClass1.IrmaId);
            Assert.AreEqual(testClass1.hierarchyClassName, natItemClass1.ClassName);
            Assert.AreEqual(natItemCat.NatCatID, natItemClass1.NatCatID);
            Assert.AreEqual(testNationalClassCode1, natItemClass1.ClassID);

            var classValidatedNationalClass2 = context.ValidatedNationalClass.Single(vnc => vnc.IconId == testClass2.hierarchyClassID);
            var natItemClass2 = context.NatItemClass.Single(nicl => nicl.ClassID == classValidatedNationalClass2.IrmaId);
            Assert.AreEqual(testClass2.hierarchyClassName, natItemClass2.ClassName);
            Assert.AreEqual(natItemCat.NatCatID, natItemClass2.NatCatID);
            Assert.AreEqual(testNationalClassCode2, natItemClass2.ClassID);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchy_UpdateACategoryWithADashInItsName_ShouldSetNatFamilyNameToFamilyNameAndCategoryName()
        {
            //Given
            var testFamily = new HierarchyClass
            {
                hierarchyClassID = 1000000,
                hierarchyClassName = "Test Family",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalFamily
            };
            var testCategory = new HierarchyClass
            {
                hierarchyClassID = 1000001,
                hierarchyClassName = "Test Category - With Dash",
                hierarchyID = Hierarchies.National,
                hierarchyLevel = HierarchyLevels.NationalCategory,
                hierarchyParentClassID = testFamily.hierarchyClassID
            };

            //When
            command.HierarchyClass = testCategory;
            command.ParentHierarchyClass = testFamily;
            commandHandler.Handle(command);

            testCategory.hierarchyClassName = "Test Category - With Dash Updated";
            commandHandler.Handle(command);

            //Then
            var familyValidatedNationalClass = context.ValidatedNationalClass.Single(vnc => vnc.IconId == testFamily.hierarchyClassID);
            var familyNatItemFamily = context.NatItemFamily.Single(nif => nif.NatFamilyID == familyValidatedNationalClass.IrmaId);
            Assert.AreEqual($"{testFamily.hierarchyClassName} - {testCategory.hierarchyClassName}", familyNatItemFamily.NatFamilyName);

            var categoryValidatedNationalClass = context.ValidatedNationalClass.Single(vnc => vnc.IconId == testFamily.hierarchyClassID);
            var categoryNatItemFamily = context.NatItemFamily.Single(nif => nif.NatFamilyID == categoryValidatedNationalClass.IrmaId);
            Assert.AreEqual($"{testFamily.hierarchyClassName} - {testCategory.hierarchyClassName}", categoryNatItemFamily.NatFamilyName);
            Assert.AreEqual(familyNatItemFamily.NatFamilyID, categoryNatItemFamily.NatFamilyID);
        }
    }
}
