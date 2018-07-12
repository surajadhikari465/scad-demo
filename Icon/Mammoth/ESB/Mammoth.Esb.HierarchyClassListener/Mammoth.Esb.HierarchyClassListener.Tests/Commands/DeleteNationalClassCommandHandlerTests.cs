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
    public class DeleteNationalClassesCommandHandlerTests
    {
        private DeleteNationalClassCommandHandler commandHandler;
        private DeleteNationalClassParameter cmdParams;
        private SqlDbProvider dbProvider;
        private DapperSqlFactory dapperSqlFactory;
        private ObjectBuilderFactory objectBuilderFactory;
        private int nationalHierarchyId;
        private int? maxHierarchyClassId;

        [TestInitialize]
        public void InitializeTest()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new DeleteNationalClassCommandHandler(dbProvider);
            cmdParams = new DeleteNationalClassParameter { NationalClasses = new List<HierarchyClassModel>() };

            var assembly = Assembly.GetExecutingAssembly();
            dapperSqlFactory = new DapperSqlFactory(assembly);
            objectBuilderFactory = new ObjectBuilderFactory(assembly);

            nationalHierarchyId = dbProvider.Connection
                .Query<int>("SELECT HierarchyID FROM Hierarchy WHERE HierarchyName = 'National'", transaction: this.dbProvider.Transaction)
                .First();

            maxHierarchyClassId = dbProvider.Connection
                .Query<int?>(@"
                    SELECT MAX(HierarchyClassID) FROM HierarchyClass hc",
                    transaction: dbProvider.Transaction)
                .FirstOrDefault()
                 ?? 1;
        }

        [TestCleanup]
        public void CleanupTest()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void DeleteNationalClassesCommand_ReceiveNationalClassToDeleteAndExistsInDb_DeletesFromHierarchyClassTable()
        {
            // Given
            List<HierarchyClass> existingHierarchyClasses = new List<HierarchyClass>();
            for (int i = 0; i < 3; i++)
            {
                HierarchyClass hierarchyClass = objectBuilderFactory
                    .Build<HierarchyClass>()
                    .With(m => m.HierarchyClassID, maxHierarchyClassId.GetValueOrDefault() + i + 1)
                    .With(m => m.AddedDate, DateTime.Now)
                    .With(m => m.HierarchyClassName, $"TestNationalClass{i} To Be Deleted")
                    .With(m => m.HierarchyID, nationalHierarchyId);

                existingHierarchyClasses.Add(hierarchyClass);
            }
            var sql = dapperSqlFactory.BuildInsertSql<HierarchyClass>(includeScopeIdentity: false);
            int affectedRowCount = dbProvider.Connection.Execute(sql, existingHierarchyClasses, dbProvider.Transaction);

            this.cmdParams.NationalClasses.AddRange(existingHierarchyClasses.Select(ehc => MapDbHierarchyClassToHierarchyClassModel(ehc)).ToList());

            // When
            this.commandHandler.Execute(this.cmdParams);

            // Then
            List<HierarchyClass> actualNationalClasses = dbProvider.Connection
                .Query<HierarchyClass>(@"
                    SELECT * FROM HierarchyClass WHERE HierarchyClassID IN @NationalClassIDs",
                    new { NationalClassIDs = this.cmdParams.NationalClasses.Select(b => b.HierarchyClassId) },
                    dbProvider.Transaction)
                .ToList();

            Assert.AreEqual(0, actualNationalClasses.Count, "All National classes to be deleted were not deleted.");
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