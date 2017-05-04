using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Esb.HierarchyClassListener.Commands;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Common.DataAccess;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateMerchandiseHierarchyLineageCommandHandlerTests
    {
        private AddOrUpdateMerchandiseHierarchyLineageCommandHandler commandHandler;
        private AddOrUpdateMerchandiseHierarchyLineageCommand command;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();
            commandHandler = new AddOrUpdateMerchandiseHierarchyLineageCommandHandler(dbProvider);
            command = new AddOrUpdateMerchandiseHierarchyLineageCommand { HierarchyClasses = new List<HierarchyClassModel>() };
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenASegmentThatDoesntExist_ShouldAddNewHierarchyLineageWithSegment()
        {
            //Given
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 55,
                HierarchyClassName = "Test Segment",
                HierarchyClassParentId = 0,
                HierarchyLevelName = "Segment",
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE SegmentHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            Assert.IsNull(merchandiseHierarchy.FamilyHcid);
            Assert.IsNull(merchandiseHierarchy.ClassHcid);
            Assert.IsNull(merchandiseHierarchy.BrickHcid);
            Assert.IsNull(merchandiseHierarchy.SubBrickHcid);
            Assert.IsNotNull(merchandiseHierarchy.AddedDate);
            Assert.IsNull(merchandiseHierarchy.ModifiedDate);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenASegmentThatAlreadyExists_ShouldNotAddSegment()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 55
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 55,
                HierarchyClassName = "Test Segment",
                HierarchyClassParentId = 0,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.Segment,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE SegmentHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            Assert.IsNull(merchandiseHierarchy.FamilyHcid);
            Assert.IsNull(merchandiseHierarchy.ClassHcid);
            Assert.IsNull(merchandiseHierarchy.BrickHcid);
            Assert.IsNull(merchandiseHierarchy.SubBrickHcid);
            Assert.IsNotNull(merchandiseHierarchy.AddedDate);
            Assert.IsNull(merchandiseHierarchy.ModifiedDate);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenAFamilyThatDoesntExist_ShouldAddNewHierarchyLineageWithFamily()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 54,
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 55,
                HierarchyClassName = "Test Family",
                HierarchyClassParentId = testMerchandiseHierarchy.SegmentHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.Family,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE FamilyHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            Assert.AreEqual(testMerchandiseHierarchy.SegmentHcid, merchandiseHierarchy.SegmentHcid);
            Assert.AreEqual(testMerchandiseHierarchy.ClassHcid, merchandiseHierarchy.ClassHcid);
            Assert.AreEqual(testMerchandiseHierarchy.BrickHcid, merchandiseHierarchy.BrickHcid);
            Assert.AreEqual(testMerchandiseHierarchy.SubBrickHcid, merchandiseHierarchy.SubBrickHcid);
            Assert.IsNotNull(merchandiseHierarchy.AddedDate);
            Assert.IsNull(merchandiseHierarchy.ModifiedDate);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenAFamilyThatAlreadyExists_ShouldNotAddFamily()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 54,
                FamilyHcid = 55
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 55,
                HierarchyClassName = "Test Family",
                HierarchyClassParentId = testMerchandiseHierarchy.SegmentHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.Family,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE FamilyHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertMerchandiseHierarchiesAreEqual(testMerchandiseHierarchy, merchandiseHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenMultipleFamiliesWithSameSegment_ShouldAddFamily()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 54,
                FamilyHcid = 56
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 54,
                FamilyHcid = 57
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 54,
                FamilyHcid = 55
            };
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testMerchandiseHierarchy.FamilyHcid.Value,
                HierarchyClassName = "Test Family",
                HierarchyClassParentId = testMerchandiseHierarchy.SegmentHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.Family,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE FamilyHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertMerchandiseHierarchiesAreEqual(testMerchandiseHierarchy, merchandiseHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenAClassThatDoesntExist_ShouldAddNewHierarchyLineageWithClass()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 1,
                FamilyHcid = 2

            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            testMerchandiseHierarchy.ClassHcid = 55;
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testMerchandiseHierarchy.ClassHcid.Value,
                HierarchyClassName = "Test Class",
                HierarchyClassParentId = testMerchandiseHierarchy.FamilyHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.Class,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE ClassHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertMerchandiseHierarchiesAreEqual(testMerchandiseHierarchy, merchandiseHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenAClassThatAlreadyExists_ShouldNotAddClass()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 1,
                FamilyHcid = 2,
                ClassHcid = 55
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testMerchandiseHierarchy.ClassHcid.Value,
                HierarchyClassName = "Test Class",
                HierarchyClassParentId = testMerchandiseHierarchy.FamilyHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.Class,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE ClassHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertMerchandiseHierarchiesAreEqual(testMerchandiseHierarchy, merchandiseHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenAGs1BrickThatDoesntExist_ShouldAddNewHierarchyLineageWithGs1Brick()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 1,
                FamilyHcid = 2,
                ClassHcid = 3
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            testMerchandiseHierarchy.BrickHcid = 55;
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testMerchandiseHierarchy.BrickHcid.Value,
                HierarchyClassName = "Test Brick",
                HierarchyClassParentId = testMerchandiseHierarchy.ClassHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.Gs1Brick,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE BrickHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertMerchandiseHierarchiesAreEqual(testMerchandiseHierarchy, merchandiseHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenAGs1BrickThatAlreadyExists_ShouldNotAddGs1Brick()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 1,
                FamilyHcid = 2,
                ClassHcid = 3,
                BrickHcid = 55
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testMerchandiseHierarchy.BrickHcid.Value,
                HierarchyClassName = "Test Brick",
                HierarchyClassParentId = testMerchandiseHierarchy.ClassHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.Gs1Brick,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE BrickHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertMerchandiseHierarchiesAreEqual(testMerchandiseHierarchy, merchandiseHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenASubBrickThatDoesntExist_ShouldAddNewHierarchyLineageWithSubBrick()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 1,
                FamilyHcid = 2,
                ClassHcid = 3,
                BrickHcid = 4
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            testMerchandiseHierarchy.SubBrickHcid = 55;
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testMerchandiseHierarchy.SubBrickHcid.Value,
                HierarchyClassName = "Test Sub Brick",
                HierarchyClassParentId = testMerchandiseHierarchy.BrickHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.SubBrick,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE SubBrickHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertMerchandiseHierarchiesAreEqual(testMerchandiseHierarchy, merchandiseHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_GivenASubBrickThatAlreadyExists_ShouldNotAddSubBrick()
        {
            //Given
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 1,
                FamilyHcid = 2,
                ClassHcid = 3,
                BrickHcid = 4,
                SubBrickHcid = 55
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testMerchandiseHierarchy.SubBrickHcid.Value,
                HierarchyClassName = "Test Sub Brick",
                HierarchyClassParentId = testMerchandiseHierarchy.BrickHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.SubBrick,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchy = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise WHERE SubBrickHCID = @Id",
                new { Id = 55 },
                dbProvider.Transaction)
                .Single();

            AssertMerchandiseHierarchiesAreEqual(testMerchandiseHierarchy, merchandiseHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_AddingANewSubBrickWhenOtherMerchandiseHierarchiesExist_ShouldAddNewSubBrickHierarchy()
        {
            //Given
            var existing = dbProvider.Connection.Query<int>(
                "select count(*) from dbo.Hierarchy_Merchandise",
                transaction: dbProvider.Transaction).First();

            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 1,
                FamilyHcid = 2,
                ClassHcid = 3,
                BrickHcid = 4,
                SubBrickHcid = 5
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            testMerchandiseHierarchy.SubBrickHcid = 6;
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            testMerchandiseHierarchy.SubBrickHcid = 7;
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            testMerchandiseHierarchy.SubBrickHcid = 8;
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            testMerchandiseHierarchy.SubBrickHcid = 9;
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);

            testMerchandiseHierarchy.SubBrickHcid = 55;
            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = testMerchandiseHierarchy.SubBrickHcid.Value,
                HierarchyClassName = "Test Sub Brick",
                HierarchyClassParentId = testMerchandiseHierarchy.BrickHcid.Value,
                HierarchyLevelName = Constants.Merchandise.HierarchyLevels.SubBrick,
                HierarchyId = Hierarchies.Merchandise
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchies = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise",
                null,
                dbProvider.Transaction)
                .ToList();
            var newSubBrickMerchandiseHierarchy = merchandiseHierarchies.Single(m => m.SubBrickHcid == testMerchandiseHierarchy.SubBrickHcid);

            Assert.AreEqual(existing + 6, merchandiseHierarchies.Count);
            AssertMerchandiseHierarchiesAreEqual(testMerchandiseHierarchy, newSubBrickMerchandiseHierarchy);
        }

        [TestMethod]
        public void AddOrUpdateMerchandiseHierarchyLineage_AddingMultipleNewSubBricksWhenOtherMerchandiseHierarchiesExist_ShouldAddNewSubBrickHierarchies()
        {
            //Given
            var expectedSubBrickIds = new List<int> { 5, 6, 7, 8, 9, 55, 56, 57, 58, 59 };
            var testMerchandiseHierarchy = new MerchandiseHierarchyModel
            {
                SegmentHcid = 1,
                FamilyHcid = 2,
                ClassHcid = 3,
                BrickHcid = 4,
                SubBrickHcid = 5
            };
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            testMerchandiseHierarchy.SubBrickHcid = 6;
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            testMerchandiseHierarchy.SubBrickHcid = 7;
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            testMerchandiseHierarchy.SubBrickHcid = 8;
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            testMerchandiseHierarchy.SubBrickHcid = 9;
            InsertTestMerchandiseHierarchy(testMerchandiseHierarchy);
            
            command.HierarchyClasses.AddRange(new List<HierarchyClassModel>
            {
                new HierarchyClassModel
                {
                    HierarchyClassId = 55,
                    HierarchyClassName = "Test Sub Brick 1",
                    HierarchyClassParentId = testMerchandiseHierarchy.BrickHcid.Value,
                    HierarchyLevelName = Constants.Merchandise.HierarchyLevels.SubBrick,
                    HierarchyId = Hierarchies.Merchandise
                },
                new HierarchyClassModel
                {
                    HierarchyClassId = 56,
                    HierarchyClassName = "Test Sub Brick 2",
                    HierarchyClassParentId = testMerchandiseHierarchy.BrickHcid.Value,
                    HierarchyLevelName = Constants.Merchandise.HierarchyLevels.SubBrick,
                    HierarchyId = Hierarchies.Merchandise
                },
                new HierarchyClassModel
                {
                    HierarchyClassId = 57,
                    HierarchyClassName = "Test Sub Brick 3",
                    HierarchyClassParentId = testMerchandiseHierarchy.BrickHcid.Value,
                    HierarchyLevelName = Constants.Merchandise.HierarchyLevels.SubBrick,
                    HierarchyId = Hierarchies.Merchandise
                },
                new HierarchyClassModel
                {
                    HierarchyClassId = 58,
                    HierarchyClassName = "Test Sub Brick 4",
                    HierarchyClassParentId = testMerchandiseHierarchy.BrickHcid.Value,
                    HierarchyLevelName = Constants.Merchandise.HierarchyLevels.SubBrick,
                    HierarchyId = Hierarchies.Merchandise
                },
                new HierarchyClassModel
                {
                    HierarchyClassId = 59,
                    HierarchyClassName = "Test Sub Brick 5",
                    HierarchyClassParentId = testMerchandiseHierarchy.BrickHcid.Value,
                    HierarchyLevelName = Constants.Merchandise.HierarchyLevels.SubBrick,
                    HierarchyId = Hierarchies.Merchandise
                },
            });

            //When
            commandHandler.Execute(command);

            //Then
            var merchandiseHierarchies = dbProvider.Connection.Query<MerchandiseHierarchyModel>(
                @"SELECT * FROM dbo.Hierarchy_Merchandise",
                null,
                dbProvider.Transaction)
                .Where(m => m.SubBrickHcid.HasValue && expectedSubBrickIds.Contains((int)m.SubBrickHcid))
                .ToList();

            Assert.AreEqual(10, merchandiseHierarchies.Count);
            foreach (var hierarchy in merchandiseHierarchies)
            {
                Assert.AreEqual(testMerchandiseHierarchy.SegmentHcid, hierarchy.SegmentHcid);
                Assert.AreEqual(testMerchandiseHierarchy.FamilyHcid, hierarchy.FamilyHcid);
                Assert.AreEqual(testMerchandiseHierarchy.ClassHcid, hierarchy.ClassHcid);
                Assert.AreEqual(testMerchandiseHierarchy.BrickHcid, hierarchy.BrickHcid);
            }
            var actualSubBrickIds = merchandiseHierarchies.Select(m => m.SubBrickHcid.Value)
                .OrderBy(id => id)
                .ToList();
            Assert.IsTrue(expectedSubBrickIds.SequenceEqual(actualSubBrickIds));
        }
        
        private void InsertTestMerchandiseHierarchy(MerchandiseHierarchyModel merchandiseHierarchy)
        {
            dbProvider.Connection.Execute(
                            @"INSERT INTO dbo.Hierarchy_Merchandise (SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID) 
                  VALUES (@SegmentHcid, @FamilyHcid, @ClassHcid, @BrickHcid, @SubBrickHcid)",
                            merchandiseHierarchy,
                            dbProvider.Transaction);
        }

        private static void AssertMerchandiseHierarchiesAreEqual(MerchandiseHierarchyModel testMerchandiseHierarchy, MerchandiseHierarchyModel merchandiseHierarchy)
        {
            Assert.AreEqual(testMerchandiseHierarchy.SegmentHcid, merchandiseHierarchy.SegmentHcid);
            Assert.AreEqual(testMerchandiseHierarchy.FamilyHcid, merchandiseHierarchy.FamilyHcid);
            Assert.AreEqual(testMerchandiseHierarchy.ClassHcid, merchandiseHierarchy.ClassHcid);
            Assert.AreEqual(testMerchandiseHierarchy.BrickHcid, merchandiseHierarchy.BrickHcid);
            Assert.AreEqual(testMerchandiseHierarchy.SubBrickHcid, merchandiseHierarchy.SubBrickHcid);
            Assert.IsNotNull(merchandiseHierarchy.AddedDate);
            Assert.IsNull(merchandiseHierarchy.ModifiedDate);
        }
    }
}
