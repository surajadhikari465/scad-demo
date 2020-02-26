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
    public class DeleteManufacturerCommandHandlerTests
    {
        private DeleteManufacturerCommandHandler commandHandler;
        private DeleteManufacturerParameter command;
        private SqlDbProvider dbProvider;
        private DapperSqlFactory dapperSqlFactory;
        private ObjectBuilderFactory objectBuilderFactory;
        private int manufacturerHierarchyId;
        private int? maxManufacturerId;

        [TestInitialize]
        public void InitializeTest()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new DeleteManufacturerCommandHandler(dbProvider);
            command = new DeleteManufacturerParameter { Manufacturer = new List<HierarchyClassModel>() };

            var assembly = Assembly.GetExecutingAssembly();
            dapperSqlFactory = new DapperSqlFactory(assembly);
            objectBuilderFactory = new ObjectBuilderFactory(assembly);

            manufacturerHierarchyId = dbProvider.Connection
                .Query<int>("SELECT HierarchyID FROM Hierarchy WHERE HierarchyName = 'Manufacturer'", transaction: this.dbProvider.Transaction)
                .First();

            maxManufacturerId = dbProvider.Connection
                .Query<int?>(@"
                    SELECT MAX(HierarchyClassID) FROM HierarchyClass hc WHERE hc.HierarchyID = @ManufacturerHierarchyID",
                    new { ManufacturerHierarchyID = manufacturerHierarchyId },
                    transaction: dbProvider.Transaction)
                .FirstOrDefault();

            maxManufacturerId = maxManufacturerId == null ? 1 : maxManufacturerId;
        }

        [TestCleanup]
        public void CleanupTest()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void DeleteManufacturerCommand_ReceiveManufacturerToDeleteAndManufacturerExistsInDb_DeletesManufacturerFromHierarchyClassTable()
        {
            // Given
            List<HierarchyClass> existingHierarchyClasses = new List<HierarchyClass>();
            for (int i = 0; i < 3; i++)
            {
                HierarchyClass hierarchyClass = objectBuilderFactory
                    .Build<HierarchyClass>()
                    .With(m => m.HierarchyClassID, maxManufacturerId.GetValueOrDefault() + i + 1)
                    .With(m => m.AddedDate, DateTime.Now)
                    .With(m => m.HierarchyClassName, $"TestManufacturer{i} To Be Deleted")
                    .With(m => m.HierarchyID, manufacturerHierarchyId);

                existingHierarchyClasses.Add(hierarchyClass);
            }
            var sql = dapperSqlFactory.BuildInsertSql<HierarchyClass>(includeScopeIdentity: false);
            int affectedRowCount = dbProvider.Connection.Execute(sql, existingHierarchyClasses, dbProvider.Transaction);

            this.command.Manufacturer.AddRange(existingHierarchyClasses.Select(ehc => MapDbHierarchyClassToHierarchyClassModel(ehc)).ToList());

            // When
            this.commandHandler.Execute(this.command);

            // Then
            List<HierarchyClass> actualManufacturer = dbProvider.Connection
                .Query<HierarchyClass>(@"
                    SELECT * FROM HierarchyClass WHERE HierarchyClassID IN @ManufacturerIDs",
                    new { ManufacturerIDs = this.command.Manufacturer.Select(b => b.HierarchyClassId) },
                    dbProvider.Transaction)
                .ToList();

            Assert.AreEqual(0, actualManufacturer.Count, "All manufacturer to be deleted were not deleted.");
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