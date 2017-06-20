using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Esb.HierarchyClassListener.Commands;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Testing.Core;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Commands
{
    [TestClass]
    public class DeleteBrandsCommandHandlerTests
    {
        private DeleteBrandsCommandHandler commandHandler;
        private DeleteBrandsCommand command;
        private SqlDbProvider dbProvider;
        private DapperSqlFactory dapperSqlFactory;
        private ObjectBuilderFactory objectBuilderFactory;
        private int brandHierarchyId;
        private int? maxBrandId;

        [TestInitialize]
        public void InitializeTest()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new DeleteBrandsCommandHandler(dbProvider);
            command = new DeleteBrandsCommand { Brands = new List<HierarchyClassModel>() };

            var assembly = Assembly.GetExecutingAssembly();
            dapperSqlFactory = new DapperSqlFactory(assembly);
            objectBuilderFactory = new ObjectBuilderFactory(assembly);

            brandHierarchyId = dbProvider.Connection
                .Query<int>("SELECT HierarchyID FROM Hierarchy WHERE HierarchyName = 'Brands'", transaction: this.dbProvider.Transaction)
                .First();

            maxBrandId = dbProvider.Connection
                .Query<int?>(@"
                    SELECT MAX(HierarchyClassID) FROM HierarchyClass hc WHERE hc.HierarchyID = @BrandHierarchyID",
                    new { BrandHierarchyID = brandHierarchyId },
                    transaction: dbProvider.Transaction)
                .FirstOrDefault();

            maxBrandId = maxBrandId == null ? 1 : maxBrandId;
        }

        [TestCleanup]
        public void CleanupTest()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void DeleteBrandsCommand_ReceiveBrandToDeleteAndBrandExistsInDb_DeletesBrandFromHierarchyClassTable()
        {
            // Given
            List<HierarchyClass> existingHierarchyClasses = new List<HierarchyClass>();
            for (int i = 0; i < 3; i++)
            {
                HierarchyClass hierarchyClass = objectBuilderFactory
                    .Build<HierarchyClass>()
                    .With(m => m.HierarchyClassID, maxBrandId.GetValueOrDefault() + i + 1)
                    .With(m => m.AddedDate, DateTime.Now)
                    .With(m => m.HierarchyClassName, $"TestBrand{i} To Be Deleted")
                    .With(m => m.HierarchyID, brandHierarchyId);

                existingHierarchyClasses.Add(hierarchyClass);
            }
            var sql = dapperSqlFactory.BuildInsertSql<HierarchyClass>(includeScopeIdentity: false);
            int affectedRowCount = dbProvider.Connection.Execute(sql, existingHierarchyClasses, dbProvider.Transaction);

            this.command.Brands.AddRange(existingHierarchyClasses.Select(ehc => MapDbHierarchyClassToHierarchyClassModel(ehc)).ToList());

            // When
            this.commandHandler.Execute(this.command);

            // Then
            List<HierarchyClass> actualBrands = dbProvider.Connection
                .Query<HierarchyClass>(@"
                    SELECT * FROM HierarchyClass WHERE HierarchyClassID IN @BrandIDs",
                    new { BrandIDs = this.command.Brands.Select(b => b.HierarchyClassId) },
                    dbProvider.Transaction)
                .ToList();

            Assert.AreEqual(0, actualBrands.Count, "All brands to be deleted were not deleted.");
        }

        private HierarchyClassModel MapDbHierarchyClassToHierarchyClassModel(HierarchyClass dbHierarchyClass)
        {
            HierarchyClassModel hierarchyClassModel = new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = dbHierarchyClass.HierarchyClassID,
                HierarchyClassName = dbHierarchyClass.HierarchyClassName,
                HierarchyClassParentId = 0,
                HierarchyId = dbHierarchyClass.HierarchyID,
                Timestamp = dbHierarchyClass.AddedDate
            };

            return hierarchyClassModel;
        }
    }
}
