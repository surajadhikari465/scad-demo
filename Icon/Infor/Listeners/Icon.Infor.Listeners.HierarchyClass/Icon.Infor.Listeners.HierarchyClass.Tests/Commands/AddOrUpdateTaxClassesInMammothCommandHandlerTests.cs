using Dapper;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateTaxClassesInMammothCommandHandlerTests
    {
        private AddOrUpdateTaxClassesInMammothCommandHandler commandHandler;
        private AddOrUpdateTaxClassesInMammothCommand command;
        private SqlConnection connection;
        private int taxId;
        private int testHierarchyClassId1 = 12345;
        private int testHierarchyClassId2 = 12346;
        private int testHierarchyClassId3 = 12347;

        [TestInitialize]
        public void Initialize()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            connection = new SqlConnection(connectionString);

            commandHandler = new AddOrUpdateTaxClassesInMammothCommandHandler();
            command = new AddOrUpdateTaxClassesInMammothCommand();

            connection.Open();

            taxId = connection.Query<int>("SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Tax'").First();
        }

        [TestCleanup]
        public void Cleanup()
        {
            connection.Execute($"DELETE FROM dbo.Tax_Attributes WHERE TaxHCID in ({testHierarchyClassId1}, {testHierarchyClassId2}, {testHierarchyClassId3})");
            connection.Execute($"DELETE FROM dbo.HierarchyClass WHERE HierarchyClassID in ({testHierarchyClassId1}, {testHierarchyClassId2}, {testHierarchyClassId3})");
        }

        [TestMethod]
        public void SaveTaxToMammoth_NewTaxClasses_ShouldAddTaxClassesToMammoth()
        {
            //Given
            var testTaxClasses = new List<InforHierarchyClassModel>
            {
                new InforHierarchyClassModel { HierarchyClassId = testHierarchyClassId1, HierarchyClassName = "1111111 Test Tax 1" },
                new InforHierarchyClassModel { HierarchyClassId = testHierarchyClassId2, HierarchyClassName = "1111112 Test Tax 2" },
                new InforHierarchyClassModel { HierarchyClassId = testHierarchyClassId3, HierarchyClassName = "1111113 Test Tax 3" }
            };
            command.TaxHierarchyClasses = testTaxClasses;

            //When
            commandHandler.Execute(command);

            //Then
            var actualTaxClasses = connection.Query(
                @"SELECT *
                  FROM dbo.HierarchyClass hc
                  JOIN dbo.Tax_Attributes ta on hc.HierarchyClassID = ta.TaxHCID 
                  WHERE hc.HierarchyClassID in (@testHierarchyClassId1, @testHierarchyClassId2, @testHierarchyClassId3) AND hc.HierarchyID = @TaxHierarchyId",
                  new
                  {
                      TaxHierarchyId = this.taxId,
                      testHierarchyClassId1,
                      testHierarchyClassId2,
                      testHierarchyClassId3
                  })
                .ToList();
            Assert.AreEqual(testTaxClasses.Count, actualTaxClasses.Count);
            for (int i = 0; i < testTaxClasses.Count; i++)
            {
                Assert.AreEqual(testTaxClasses[i].HierarchyClassId, actualTaxClasses[i].HierarchyClassID);
                Assert.AreEqual(testTaxClasses[i].HierarchyClassName, actualTaxClasses[i].HierarchyClassName);
                Assert.AreEqual(testTaxClasses[i].HierarchyClassName.Substring(0, 7), actualTaxClasses[i].TaxCode);
            }
        }

        [TestMethod]
        public void SaveTaxToMammoth_ExistingTaxClasses_ShouldUpdateTaxClassesInMammoth()
        {
            // Given
            var testTaxClasses = new List<InforHierarchyClassModel>
            {
                new InforHierarchyClassModel { HierarchyClassId = testHierarchyClassId1, HierarchyClassName = "1111111 Test Tax 1" },
                new InforHierarchyClassModel { HierarchyClassId = testHierarchyClassId2, HierarchyClassName = "1111112 Test Tax 2" },
                new InforHierarchyClassModel { HierarchyClassId = testHierarchyClassId3, HierarchyClassName = "1111113 Test Tax 3" }
            };
            AddTestTaxClassesToDatabase(testTaxClasses);

            var editedExistingTaxClasses = new List<InforHierarchyClassModel>
            {
                new InforHierarchyClassModel { HierarchyClassId = testHierarchyClassId1, HierarchyClassName = "1111111 Test Tax 1 Edited" },
                new InforHierarchyClassModel { HierarchyClassId = testHierarchyClassId2, HierarchyClassName = "1111112 Test Tax 2 Edited" },
                new InforHierarchyClassModel { HierarchyClassId = testHierarchyClassId3, HierarchyClassName = "1111113 Test Tax 3 Edited" }
            };
            command.TaxHierarchyClasses = editedExistingTaxClasses;

            // When
            commandHandler.Execute(command);

            // Then
            var actualTaxClasses = connection.Query(
                @"SELECT *
                  FROM dbo.HierarchyClass hc
                  JOIN dbo.Tax_Attributes ta on hc.HierarchyClassID = ta.TaxHCID 
                  WHERE hc.HierarchyClassID in (@testHierarchyClassId1, @testHierarchyClassId2, @testHierarchyClassId3) AND hc.HierarchyID = @TaxHierarchyId",
                new
                {
                    TaxHierarchyId = this.taxId,
                    testHierarchyClassId1,
                    testHierarchyClassId2,
                    testHierarchyClassId3
                })
                .ToList();
            Assert.AreEqual(testTaxClasses.Count, actualTaxClasses.Count);
            for (int i = 0; i < testTaxClasses.Count; i++)
            {
                Assert.AreEqual(editedExistingTaxClasses[i].HierarchyClassId, actualTaxClasses[i].HierarchyClassID);
                Assert.AreEqual(editedExistingTaxClasses[i].HierarchyClassName, actualTaxClasses[i].HierarchyClassName);
                Assert.AreEqual(editedExistingTaxClasses[i].HierarchyClassName.Substring(0, 7), actualTaxClasses[i].TaxCode);
            }
        }

        private void AddTestTaxClassesToDatabase(List<InforHierarchyClassModel> taxClasses)
        {
            int taxHierarchyId = this.connection.Query<int>("SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Tax'").First();
            string sql = @" INSERT INTO dbo.HierarchyClass
                            (
	                            HierarchyClassID,
	                            HierarchyClassName,
	                            HierarchyID,
	                            AddedDate
                            )
                            VALUES
                            (
	                            @HierarchyClassID,
	                            @HierarchyClassName,
	                            @HierarchyID,
	                            @AddedDate
                            )";

            this.connection.Execute(sql, taxClasses.Select(tc => new
            {
                HierarchyClassID = tc.HierarchyClassId,
                HierarchyClassName = tc.HierarchyClassName,
                HierarchyID = taxHierarchyId,
                AddedDate = DateTime.Now
            }));

            sql = @"INSERT INTO dbo.Tax_Attributes
                    (
	                    TaxHCID,
	                    TaxCode,
	                    AddedDate
                    )
                    VALUES
                    (
	                    @TaxHCID,
	                    @TaxCode,
	                    @AddedDate
                    )";

            this.connection.Execute(sql, taxClasses.Select(tc => new
            {
                TaxHCID = tc.HierarchyClassId,
                TaxCode = tc.HierarchyClassName.Substring(0, 7),
                AddedDate = DateTime.Now
            }).ToList());
        }
    }
}
