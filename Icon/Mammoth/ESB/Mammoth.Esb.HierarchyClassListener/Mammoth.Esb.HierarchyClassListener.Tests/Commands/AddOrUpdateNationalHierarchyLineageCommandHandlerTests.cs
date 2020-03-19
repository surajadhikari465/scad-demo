using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using Mammoth.Esb.HierarchyClassListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateNationalHierarchyLineageCommandHandlerTests
    {
        private AddOrUpdateNationalHierarchyLineageCommandHandler commandHandler;
        private AddOrUpdateNationalHierarchyLineageCommand command;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();
            commandHandler = new AddOrUpdateNationalHierarchyLineageCommandHandler(dbProvider);
            command = new AddOrUpdateNationalHierarchyLineageCommand { HierarchyClasses = new List<HierarchyClassModel>() };
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenAFamilyThatDoesntExist_ShouldAddNewHierarchyLineageWithFamily()
        {
            //Given
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 55,
                HierarchyClassName = "Test Family",
                HierarchyClassParentId = 0,
                HierarchyLevelName = "National Family",
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var NationalHierarchy = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE FamilyHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            Assert.IsNull(NationalHierarchy.CategoryHcid);
            Assert.IsNull(NationalHierarchy.SubcategoryHcid);
            Assert.IsNull(NationalHierarchy.ClassHcid);
            Assert.IsNotNull(NationalHierarchy.AddedDate);
            Assert.IsNull(NationalHierarchy.ModifiedDate);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenAFamilyThatAlreadyExists_ShouldNotAddFamily()
        {
            //Given
            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 55
            };
            InsertTestNationalHierarchy(testNationalHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 55,
                HierarchyClassName = "Test Family",
                HierarchyClassParentId = 0,
                HierarchyLevelName = Constants.National.HierarchyLevels.Family,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var NationalHierarchy = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE FamilyHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            Assert.IsNull(NationalHierarchy.CategoryHcid);
            Assert.IsNull(NationalHierarchy.SubcategoryHcid);
            Assert.IsNull(NationalHierarchy.ClassHcid);
            Assert.IsNotNull(NationalHierarchy.AddedDate);
            Assert.IsNull(NationalHierarchy.ModifiedDate);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenACategoryThatDoesntExist_ShouldAddNewHierarchyLineageWithCategory()
        {
            //Given
            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 54,
            };
            InsertTestNationalHierarchy(testNationalHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 55,
                HierarchyClassName = "Test Category",
                HierarchyClassParentId = testNationalHierarchy.FamilyHcid.Value,
                HierarchyLevelName = Constants.National.HierarchyLevels.Category,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var NationalHierarchy = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE CategoryHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            Assert.AreEqual(testNationalHierarchy.FamilyHcid, NationalHierarchy.FamilyHcid);
            Assert.AreEqual(testNationalHierarchy.SubcategoryHcid, NationalHierarchy.SubcategoryHcid);
            Assert.AreEqual(testNationalHierarchy.ClassHcid, NationalHierarchy.ClassHcid);
            Assert.IsNotNull(NationalHierarchy.AddedDate);
            Assert.IsNotNull(NationalHierarchy.ModifiedDate);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenACategoryThatAlreadyExists_ShouldNotAddCategory()
        {
            //Given
            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 54,
                CategoryHcid = 55
            };
            InsertTestNationalHierarchy(testNationalHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 55,
                HierarchyClassName = "Test Category",
                HierarchyClassParentId = testNationalHierarchy.FamilyHcid.Value,
                HierarchyLevelName = Constants.National.HierarchyLevels.Category,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var NationalHierarchy = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE CategoryHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertNationalHierarchiesAreEqual(testNationalHierarchy, NationalHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenMultipleCategoriesWithSameFamily_ShouldAddSingleCategory()
        {
            //Given
            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 54,
                CategoryHcid = 56
            };
            InsertTestNationalHierarchy(testNationalHierarchy);
            testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 54,
                CategoryHcid = 57
            };
            InsertTestNationalHierarchy(testNationalHierarchy);

            testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 54,
                CategoryHcid = 55
            };
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testNationalHierarchy.CategoryHcid.Value,
                HierarchyClassName = "Test Category",
                HierarchyClassParentId = testNationalHierarchy.FamilyHcid.Value,
                HierarchyLevelName = Constants.National.HierarchyLevels.Category,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var NationalHierarchy = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE CategoryHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertNationalHierarchiesAreEqual(testNationalHierarchy, NationalHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenMultipleCategoriesWithSameFamily_ShouldUpdateExistingAndAddNew()
        {
            //Given
            const int Family_ID = 54;
            const int existingCategory_ID = 57;
            const int newCategory_ID1 = 55;
            const int newCategory_ID2 = 56;
            var existingMerchHierarchyA = new NationalHierarchyModel
            {
                FamilyHcid = Family_ID,
                CategoryHcid = null
            };
            var existingMerchHierarchyB = new NationalHierarchyModel
            {
                FamilyHcid = Family_ID,
                CategoryHcid = existingCategory_ID
            };
            InsertTestNationalHierarchy(existingMerchHierarchyA);
            InsertTestNationalHierarchy(existingMerchHierarchyB);

            var updatedMerchHierarchyA = new NationalHierarchyModel
            {
                FamilyHcid = Family_ID,
                CategoryHcid = newCategory_ID1
            };
            var newMerchHierarchyC = new NationalHierarchyModel
            {
                FamilyHcid = Family_ID,
                CategoryHcid = newCategory_ID2
            };
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = newCategory_ID1,
                HierarchyClassName = "Test Category 1",
                HierarchyClassParentId = Family_ID,
                HierarchyLevelName = Constants.National.HierarchyLevels.Category,
                HierarchyId = Hierarchies.National
            });
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = newCategory_ID2,
                HierarchyClassName = "Test Category 2",
                HierarchyClassParentId = Family_ID,
                HierarchyLevelName = Constants.National.HierarchyLevels.Category,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var actualCategory2Lineage = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE CategoryHCID = @Id",
                new { Id = newCategory_ID2 },
                dbProvider.Transaction)
                .Single();
            AssertNationalHierarchiesAreEqual(newMerchHierarchyC, actualCategory2Lineage, false);

            var actualCategory1Lineage = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE CategoryHCID = @Id",
                new { Id = newCategory_ID1 },
                dbProvider.Transaction)
                .Single();
            Assert.IsNotNull(actualCategory1Lineage.ModifiedDate);
            AssertNationalHierarchiesAreEqual(updatedMerchHierarchyA, actualCategory1Lineage, true);
            Assert.IsTrue(actualCategory1Lineage.ModifiedDate > actualCategory1Lineage.AddedDate);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenASubcategoryThatDoesntExist_ShouldAddNewHierarchyLineageWithSubcategory()
        {
            //Given
            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 1,
                CategoryHcid = 2
            };
            InsertTestNationalHierarchy(testNationalHierarchy);

            testNationalHierarchy.SubcategoryHcid = 55;
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testNationalHierarchy.SubcategoryHcid.Value,
                HierarchyClassName = "Test Class",
                HierarchyClassParentId = testNationalHierarchy.CategoryHcid.Value,
                HierarchyLevelName = Constants.National.HierarchyLevels.SubCategory,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var NationalHierarchy = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE SubcategoryHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertNationalHierarchiesAreEqual(testNationalHierarchy, NationalHierarchy, true);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenASubcategoryThatAlreadyExists_ShouldNotAddSubcategory()
        {
            //Given
            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 1,
                CategoryHcid = 2,
                SubcategoryHcid = 55
            };
            InsertTestNationalHierarchy(testNationalHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testNationalHierarchy.SubcategoryHcid.Value,
                HierarchyClassName = "Test Class",
                HierarchyClassParentId = testNationalHierarchy.CategoryHcid.Value,
                HierarchyLevelName = Constants.National.HierarchyLevels.NationalClass,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var NationalHierarchy = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE SubcategoryHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertNationalHierarchiesAreEqual(testNationalHierarchy, NationalHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenAClassThatDoesntExist_ShouldAddNewHierarchyLineageWithClass()
        {
            //Given
            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 1,
                CategoryHcid = 2,
                SubcategoryHcid = 3
            };
            InsertTestNationalHierarchy(testNationalHierarchy);

            testNationalHierarchy.ClassHcid = 55;
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testNationalHierarchy.ClassHcid.Value,
                HierarchyClassName = "Test Class",
                HierarchyClassParentId = testNationalHierarchy.SubcategoryHcid.Value,
                HierarchyLevelName = Constants.National.HierarchyLevels.NationalClass,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var NationalHierarchy = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE ClassHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertNationalHierarchiesAreEqual(testNationalHierarchy, NationalHierarchy, true);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_GivenAClassThatAlreadyExists_ShouldNotAddClass()
        {
            //Given
            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 1,
                CategoryHcid = 2,
                SubcategoryHcid = 3,
                ClassHcid = 4,
            };
            InsertTestNationalHierarchy(testNationalHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testNationalHierarchy.ClassHcid.Value,
                HierarchyClassName = "Test Class",
                HierarchyClassParentId = testNationalHierarchy.SubcategoryHcid.Value,
                HierarchyLevelName = Constants.National.HierarchyLevels.NationalClass,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var NationalHierarchy = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass WHERE ClassHCID = @Id",
                new { Id = 4 },
                dbProvider.Transaction)
                .Single();

            AssertNationalHierarchiesAreEqual(testNationalHierarchy, NationalHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_AddingANewClassWhenOtherNationalHierarchiesExist_ShouldAddNewClassHierarchy()
        {
            //Given
            var existing = dbProvider.Connection.Query<int>(
                "select count(*) from dbo.Hierarchy_NationalClass",
                transaction: dbProvider.Transaction).First();

            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 1,
                CategoryHcid = 2,
                SubcategoryHcid = 3,
                ClassHcid = 4,
            };
            InsertTestNationalHierarchy(testNationalHierarchy);
            testNationalHierarchy.ClassHcid = 5;
            InsertTestNationalHierarchy(testNationalHierarchy);
            testNationalHierarchy.ClassHcid = 6;
            InsertTestNationalHierarchy(testNationalHierarchy);
            testNationalHierarchy.ClassHcid = 7;
            InsertTestNationalHierarchy(testNationalHierarchy);
            testNationalHierarchy.ClassHcid = 8;
            InsertTestNationalHierarchy(testNationalHierarchy);

            testNationalHierarchy.ClassHcid = 55;
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testNationalHierarchy.ClassHcid.Value,
                HierarchyClassName = "Test Class",
                HierarchyClassParentId = testNationalHierarchy.SubcategoryHcid.Value,
                HierarchyLevelName = Constants.National.HierarchyLevels.NationalClass,
                HierarchyId = Hierarchies.National
            });

            //When
            commandHandler.Execute(command);

            //Then
            var nationalHierarchies = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass",
                null,
                dbProvider.Transaction)
                .ToList();
            var newNationalHierarchy = nationalHierarchies.Single(m => m.ClassHcid == testNationalHierarchy.ClassHcid);

            Assert.AreEqual(existing + 6, nationalHierarchies.Count);
            AssertNationalHierarchiesAreEqual(testNationalHierarchy, newNationalHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateNationalHierarchyLineage_AddingMultipleNewClassesWhenOtherNationalHierarchiesExist_ShouldAddNewClassHierarchies()
        {
            //Given
            var expectedClassIds = new List<int> { 6, 7, 8, 9, 55, 56, 57, 58, 59 };
            var testNationalHierarchy = new NationalHierarchyModel
            {
                FamilyHcid = 1,
                CategoryHcid = 2,
                SubcategoryHcid = 3,
                ClassHcid = 4,
            };
            InsertTestNationalHierarchy(testNationalHierarchy);
            testNationalHierarchy.ClassHcid = 6;
            InsertTestNationalHierarchy(testNationalHierarchy);
            testNationalHierarchy.ClassHcid = 7;
            InsertTestNationalHierarchy(testNationalHierarchy);
            testNationalHierarchy.ClassHcid = 8;
            InsertTestNationalHierarchy(testNationalHierarchy);
            testNationalHierarchy.ClassHcid = 9;
            InsertTestNationalHierarchy(testNationalHierarchy);

            command.HierarchyClasses.AddRange(new List<HierarchyClassModel>
            {
                new HierarchyClassModel
                {
                    HierarchyClassId = 55,
                    HierarchyClassName = "Test Class 1",
                    HierarchyClassParentId = testNationalHierarchy.SubcategoryHcid.Value,
                    HierarchyLevelName = Constants.National.HierarchyLevels.NationalClass,
                    HierarchyId = Hierarchies.National
                },
                new HierarchyClassModel
                {
                    HierarchyClassId = 56,
                    HierarchyClassName = "Test Class 2",
                    HierarchyClassParentId = testNationalHierarchy.SubcategoryHcid.Value,
                    HierarchyLevelName = Constants.National.HierarchyLevels.NationalClass,
                    HierarchyId = Hierarchies.National
                },
                new HierarchyClassModel
                {
                    HierarchyClassId = 57,
                    HierarchyClassName = "Test Class 3",
                    HierarchyClassParentId = testNationalHierarchy.SubcategoryHcid.Value,
                    HierarchyLevelName = Constants.National.HierarchyLevels.NationalClass,
                    HierarchyId = Hierarchies.National
                },
                new HierarchyClassModel
                {
                    HierarchyClassId = 58,
                    HierarchyClassName = "Test Class 4",
                    HierarchyClassParentId = testNationalHierarchy.SubcategoryHcid.Value,
                    HierarchyLevelName = Constants.National.HierarchyLevels.NationalClass,
                    HierarchyId = Hierarchies.National
                },
                new HierarchyClassModel
                {
                    HierarchyClassId = 59,
                    HierarchyClassName = "Test Class 5",
                    HierarchyClassParentId = testNationalHierarchy.SubcategoryHcid.Value,
                    HierarchyLevelName = Constants.National.HierarchyLevels.NationalClass,
                    HierarchyId = Hierarchies.National
                },
            });

            //When
            commandHandler.Execute(command);

            //Then
            var nationalHierarchies = dbProvider.Connection.Query<NationalHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_NationalClass",
                null,
                dbProvider.Transaction)
                .Where(m => m.ClassHcid.HasValue && expectedClassIds.Contains((int)m.ClassHcid))
                .ToList();

            Assert.AreEqual(9, nationalHierarchies.Count);
            foreach (var hierarchy in nationalHierarchies)
            {
                Assert.AreEqual(testNationalHierarchy.FamilyHcid, hierarchy.FamilyHcid);
                Assert.AreEqual(testNationalHierarchy.CategoryHcid, hierarchy.CategoryHcid);
                Assert.AreEqual(testNationalHierarchy.SubcategoryHcid, hierarchy.SubcategoryHcid);
            }
            var actualClassIds = nationalHierarchies.Select(m => m.ClassHcid.Value)
                .OrderBy(id => id)
                .ToList();
            Assert.IsTrue(expectedClassIds.SequenceEqual(actualClassIds));
        }

		[TestMethod]
		[ExpectedException(typeof(System.Exception),"Invalid Hierarchy Level: Class")]
        public void AddOrUpdateNationalHierarchyLineage_WithInvalidHierarchyLevel_ShouldThrowException()
		{
			//Given
			command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 2,
                HierarchyClassName = "Test Class",
                HierarchyClassParentId = 1,
                HierarchyLevelName = "Class",
                HierarchyId = Hierarchies.National
            });

			//Then
			 commandHandler.Execute(command);
		}

        private void InsertTestNationalHierarchy(NationalHierarchyModel NationalHierarchy)
        {
            dbProvider.Connection.Execute(
                            @"INSERT INTO dbo.Hierarchy_NationalClass (FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID) 
                  VALUES (@FamilyHcid, @CategoryHcid, @SubcategoryHcid, @ClassHcid)",
                            NationalHierarchy,
                            dbProvider.Transaction);
        }

        private static void AssertNationalHierarchiesAreEqual(
            NationalHierarchyModel testNationalHierarchy,
            NationalHierarchyModel NationalHierarchy, 
            bool expectToBeModified = false)
        {
            Assert.AreEqual(testNationalHierarchy.FamilyHcid, NationalHierarchy.FamilyHcid);
            Assert.AreEqual(testNationalHierarchy.CategoryHcid, NationalHierarchy.CategoryHcid);
            Assert.AreEqual(testNationalHierarchy.SubcategoryHcid, NationalHierarchy.SubcategoryHcid);
            Assert.AreEqual(testNationalHierarchy.ClassHcid, NationalHierarchy.ClassHcid);
            Assert.IsNotNull(NationalHierarchy.AddedDate);
            if (expectToBeModified)
            {
                Assert.IsNotNull(NationalHierarchy.ModifiedDate);
            }
            else
            {
                Assert.IsNull(NationalHierarchy.ModifiedDate);
            }
        }
    }
}
