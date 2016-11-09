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
using System.Text;
using System.Threading.Tasks;
using Testing.Core;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateFinancialHierarchyClassCommandHandlerTests
    {
        private AddOrUpdateFinancialHierarchyClassCommandHandler commandHandler;
        private AddOrUpdateFinancialHierarchyClassCommand command;
        private SqlDbProvider dbProvider;
        private DapperSqlFactory dapperSqlFactory;
        private ObjectBuilderFactory objectBuilderFactory;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new AddOrUpdateFinancialHierarchyClassCommandHandler(dbProvider);
            command = new AddOrUpdateFinancialHierarchyClassCommand { HierarchyClasses = new List<HierarchyClassModel>() };

            var assembly = Assembly.GetExecutingAssembly();
            dapperSqlFactory = new DapperSqlFactory(assembly);
            objectBuilderFactory = new ObjectBuilderFactory(assembly);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateFinancialHierarchyClass_WhenFinancialHierarchyClassDoesExist_UpdateTheNameAndModifiedDateOfHierarchyClass()
        {
            //Given
            string testName = "Test Financial HierarchyClass";
            Financial_SubTeam financialSubTeam = objectBuilderFactory
                .Build<Financial_SubTeam>()
                .With(m => m.PSNumber, 1234)
                .With(m => m.Name, "Before update")
                .With(m => m.AddedDate, DateTime.Now);
            var sql = dapperSqlFactory.BuildInsertSql<Financial_SubTeam>(true);
            var id = dbProvider.Connection.Query(sql, financialSubTeam, dbProvider.Transaction);

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 1234,
                HierarchyClassName = testName
            });

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbProvider.Connection.Query<dynamic>(@"SELECT * FROM Financial_SubTeam WHERE Name = @testName", new { testName = testName }, dbProvider.Transaction).Single();
            Assert.AreEqual(testName, result.Name);
            Assert.AreEqual(1234, result.PSNumber);
        }

        [TestMethod]
        public void AddOrUpdateFinancialHierarchyClass_WhenFinancialHierarchyClassDoesntExist_AddTheHierarchyClass()
        {
            //Given
            string testName = "Test Financial HierarchyClass";

            command.HierarchyClasses.Add(new HierarchyClassModel
            {
                HierarchyClassId = 1234,
                HierarchyClassName = testName
            });

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbProvider.Connection.Query<dynamic>(@"SELECT * FROM Financial_SubTeam WHERE Name = @testName", new { testName = testName }, dbProvider.Transaction).Single();
            Assert.AreEqual(testName, result.Name);
            Assert.AreEqual(1234, result.PSNumber);
        }
    }
}
