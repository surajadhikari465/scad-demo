using Icon.Esb.CchTax.Commands;
using Icon.Esb.CchTax.Infrastructure;
using Icon.Esb.CchTax.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Icon.Esb.CchTax.Integration.Commands
{
    [TestClass]
    public class SaveTaxToMammothCommandHandlerTests
    {
        private SaveTaxToMammothCommandHandler commandHandler;
        private SaveTaxToMammothCommand command;
        private Mock<IDataConnectionManager> mockConnectionManager;
        private DataConnection connection;
        private IDbTransaction transaction;
        private CchTaxListenerApplicationSettings settings;
        private int taxId;

        [TestInitialize]
        public void Initialize()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            connection = new DataConnection { Connection = new SqlConnection(connectionString) };
            settings = new CchTaxListenerApplicationSettings { UpdateMammoth = true };
            
            mockConnectionManager = new Mock<IDataConnectionManager>();
            mockConnectionManager.SetupGet(m => m.Connection)
                .Returns(connection);

            commandHandler = new SaveTaxToMammothCommandHandler(mockConnectionManager.Object, settings);
            command = new SaveTaxToMammothCommand();

            connection.Connection.Open();
            transaction = connection.BeginTransaction();

            taxId = connection.Query<int>("SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Tax'").First();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            connection.Dispose();
        }

        [TestMethod]
        public void SaveTaxToMammoth_NewTaxClasses_ShouldAddTaxClassesToMammoth()
        {
            //Given
            var testTaxClasses = new List<TaxHierarchyClassModel>
            {
                new TaxHierarchyClassModel { TaxCode = "1111111", HierarchyClassId = 12345, HierarchyClassName = "Test Tax 1" },
                new TaxHierarchyClassModel { TaxCode = "1111112", HierarchyClassId = 12346, HierarchyClassName = "Test Tax 2" },
                new TaxHierarchyClassModel { TaxCode = "1111113", HierarchyClassId = 12347, HierarchyClassName = "Test Tax 3" }
            };
            command.TaxHierarchyClasses = testTaxClasses;

            //When
            commandHandler.Execute(command);

            //Then
            var actualTaxClasses = connection.Query(
                @"SELECT *
                  FROM dbo.HierarchyClass hc
                  JOIN dbo.Tax_Attributes ta on hc.HierarchyClassID = ta.TaxHCID 
                  WHERE hc.HierarchyClassID in (12345, 12346, 12347) AND hc.HierarchyID = @TaxHierarchyId",
                  new { TaxHierarchyId = this.taxId })
                .ToList();
            Assert.AreEqual(testTaxClasses.Count, actualTaxClasses.Count);
            for (int i = 0; i < testTaxClasses.Count; i++)
            {
                Assert.AreEqual(testTaxClasses[i].HierarchyClassId, actualTaxClasses[i].HierarchyClassID);
                Assert.AreEqual(testTaxClasses[i].HierarchyClassName, actualTaxClasses[i].HierarchyClassName);
                Assert.AreEqual(testTaxClasses[i].TaxCode, actualTaxClasses[i].TaxCode);
            }
        }

        [TestMethod]
        public void SaveTaxToMammoth_ExistingTaxClasses_ShouldUpdateTaxClassesInMammoth()
        {
            // Given
            var testTaxClasses = new List<TaxHierarchyClassModel>
            {
                new TaxHierarchyClassModel { TaxCode = "1111111", HierarchyClassId = 12345, HierarchyClassName = "Test Tax 1" },
                new TaxHierarchyClassModel { TaxCode = "1111112", HierarchyClassId = 12346, HierarchyClassName = "Test Tax 2" },
                new TaxHierarchyClassModel { TaxCode = "1111113", HierarchyClassId = 12347, HierarchyClassName = "Test Tax 3" }
            };
            AddTestTaxClassesToDatabase(testTaxClasses);

            var editedExistingTaxClasses = new List<TaxHierarchyClassModel>
            {
                new TaxHierarchyClassModel { TaxCode = "1111111", HierarchyClassId = 12345, HierarchyClassName = "Test Tax 1 Edited" },
                new TaxHierarchyClassModel { TaxCode = "1111112", HierarchyClassId = 12346, HierarchyClassName = "Test Tax 2 Edited" },
                new TaxHierarchyClassModel { TaxCode = "1111113", HierarchyClassId = 12347, HierarchyClassName = "Test Tax 3 Edited" }
            };
            command.TaxHierarchyClasses = editedExistingTaxClasses;

            // When
            commandHandler.Execute(command);

            // Then
            var actualTaxClasses = connection.Query(
                @"SELECT *
                  FROM dbo.HierarchyClass hc
                  JOIN dbo.Tax_Attributes ta on hc.HierarchyClassID = ta.TaxHCID 
                  WHERE hc.HierarchyClassID in (12345, 12346, 12347) AND hc.HierarchyID = @TaxHierarchyId",
                new { TaxHierarchyId = this.taxId })
                .ToList();
            Assert.AreEqual(testTaxClasses.Count, actualTaxClasses.Count);
            for (int i = 0; i < testTaxClasses.Count; i++)
            {
                Assert.AreEqual(editedExistingTaxClasses[i].HierarchyClassId, actualTaxClasses[i].HierarchyClassID);
                Assert.AreEqual(editedExistingTaxClasses[i].HierarchyClassName, actualTaxClasses[i].HierarchyClassName);
                Assert.AreEqual(editedExistingTaxClasses[i].TaxCode, actualTaxClasses[i].TaxCode);
            }
        }

        [TestMethod]
        public void SaveTaxToMammoth_UpdateMammothIsFalse_ShouldNotUpdateMammoth()
        {
            //Given
            settings.UpdateMammoth = false;

            //When
            commandHandler.Execute(command);

            //Then
            mockConnectionManager.VerifyGet(m => m.Connection, Times.Never);
        }

        private void AddTestTaxClassesToDatabase(List<TaxHierarchyClassModel> taxClasses)
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
                TaxCode = tc.TaxCode,
                AddedDate = DateTime.Now
            }));
        }
    }
}
