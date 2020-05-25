using Icon.Esb.CchTax.Commands;
using Icon.Esb.CchTax.Infrastructure;
using Icon.Esb.CchTax.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Data;

namespace Icon.Esb.CchTax.Integration.Commands
{
    [TestClass]
    public class SaveTaxHierarchyClassesCommandHandlerTests
    {
        private SaveTaxHierarchyClassesCommandHandler commandHandler;
        private SaveTaxHierarchyClassesCommand command;
        private Mock<IDataConnectionManager> mockConnectionManager;
        private DataConnection connection;
        private IDbTransaction transaction;
        private CchTaxListenerApplicationSettings settings;
        private int taxId;
        private List<RegionModel> regions;

        [TestInitialize]
        public void Initialize()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            connection = new DataConnection { Connection = new SqlConnection(connectionString) };
            settings = new CchTaxListenerApplicationSettings();
            regions = new List<RegionModel>();

            mockConnectionManager = new Mock<IDataConnectionManager>();
            mockConnectionManager.SetupGet(m => m.Connection)
                .Returns(connection);

            commandHandler = new SaveTaxHierarchyClassesCommandHandler(settings);
            command = new SaveTaxHierarchyClassesCommand();

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
        public void SaveTaxHierarchyClasses_NewTaxClasses_ShouldAddTaxClassesToIcon()
        {
            //Given
            var testTaxClasses = new List<TaxHierarchyClassModel>
            {
                new TaxHierarchyClassModel { TaxCode = "1111111",  HierarchyClassName = "TestTax1" },
                new TaxHierarchyClassModel { TaxCode = "1111112",  HierarchyClassName = "TestTax2" },
                new TaxHierarchyClassModel { TaxCode = "1111113",  HierarchyClassName = "TestTax3" }
            };

            regions.AddRange(new List<RegionModel>
                {
                    new RegionModel { RegionAbbr = "FL"},
                    new RegionModel { RegionAbbr = "SP"},
                    new RegionModel { RegionAbbr = "NE"}
                });
            command.TaxHierarchyClasses = testTaxClasses;
            command.Regions = regions;

            command.CchTaxMessage = "test";
            //When
            commandHandler.Execute(command);

            //Then
            var actualTaxClasses = connection.Query(
                @"SELECT *
                  FROM dbo.HierarchyClass hc
                  Join dbo.HierarchyClassTrait hct on hct.HierarchyClassid = hc.HierarchyClassid
                  WHERE hc.HierarchyClassName like '%TestTax%'
                  AND hct.traitID = 51  
                  AND hc.HierarchyID = @TaxHierarchyId",
                  new { TaxHierarchyId = this.taxId })
                .ToList();

            Assert.AreEqual(testTaxClasses.Count, actualTaxClasses.Count);

            for (int i = 0; i < testTaxClasses.Count; i++)
            {
                Assert.AreEqual(testTaxClasses[i].HierarchyClassName, actualTaxClasses[i].hierarchyClassName);
            }
        }

        [TestMethod]
        public void SaveTaxHierarchyClasses_WhenTaxAbbrMoreThen50Characters_ShouldAddOnly50CharactersToDB()
        {
            //Given
            var testTaxClasses = new List<TaxHierarchyClassModel>
            {
                new TaxHierarchyClassModel { TaxCode = "1111111", HierarchyClassName = "TEST TAX11 WITH MORE THAN 50 CHARACTERS FOR TAX ABBREVIATION TEST" },
                new TaxHierarchyClassModel { TaxCode = "1111112", HierarchyClassName = "TEST TAX12 WITH MORE THAN 50 CHARACTERS FOR TAX ABBREVIATION TEST" },
                new TaxHierarchyClassModel { TaxCode = "1111113", HierarchyClassName = "TEST TAX13 WITH MORE THAN 50 CHARACTERS FOR TAX ABBREVIATION TEST" }
            };

            regions.AddRange(new List<RegionModel>
                {
                    new RegionModel { RegionAbbr = "FL"},
                    new RegionModel { RegionAbbr = "SP"},
                    new RegionModel { RegionAbbr = "NE"}
                });
            command.TaxHierarchyClasses = testTaxClasses;
            command.Regions = regions;

            command.CchTaxMessage = "test";
            //When
            commandHandler.Execute(command);

            //Then
            var actualTaxClasses = connection.Query(
                @"SELECT *
                  FROM dbo.HierarchyClass hc
                  Join dbo.HierarchyClassTrait hct on hct.HierarchyClassid = hc.HierarchyClassid
                  WHERE hct.TraitValue like '%TEST TAX1%'
                  AND hct.traitID = 51
                  AND hc.HierarchyID = @TaxHierarchyId",
                  new { TaxHierarchyId = this.taxId })
                .ToList();
            Assert.AreEqual(testTaxClasses.Count, actualTaxClasses.Count);
            for (int i = 0; i < testTaxClasses.Count; i++)
            {
                Assert.AreNotSame(testTaxClasses[i].HierarchyClassName, actualTaxClasses[i].traitValue);
                Assert.AreEqual(testTaxClasses[i].HierarchyClassName.Substring(0, 50), actualTaxClasses[i].traitValue);
            }               
        }
    }
}