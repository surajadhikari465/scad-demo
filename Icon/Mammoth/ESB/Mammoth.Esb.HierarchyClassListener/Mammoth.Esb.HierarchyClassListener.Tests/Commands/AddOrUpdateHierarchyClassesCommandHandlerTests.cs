using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Esb.HierarchyClassListener.Commands;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Collections.Generic;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Common.DataAccess;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateHierarchyClassesCommandHandlerTests
    {
        AddOrUpdateHierarchyClassesCommandHandler commandHandler;
        AddOrUpdateHierarchyClassesCommand command;
        SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new AddOrUpdateHierarchyClassesCommandHandler(dbProvider);
            command = new AddOrUpdateHierarchyClassesCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClassesCommand_Given5NewHierarchyClasses_ShouldAddClasses()
        {
            //Given
            var existingNum = dbProvider.Connection.Query<int>(
               "select count(*) from dbo.HierarchyClass", transaction: dbProvider.Transaction).First();

            List<HierarchyClassModel> hierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel { HierarchyClassId = 1, HierarchyClassName = "Test HierarchyClass1", HierarchyId = Hierarchies.Merchandise },
                new HierarchyClassModel { HierarchyClassId = 2, HierarchyClassName = "Test HierarchyClass2", HierarchyId = Hierarchies.Merchandise },
                new HierarchyClassModel { HierarchyClassId = 3, HierarchyClassName = "Test HierarchyClass3", HierarchyId = Hierarchies.Merchandise },
                new HierarchyClassModel { HierarchyClassId = 4, HierarchyClassName = "Test HierarchyClass4", HierarchyId = Hierarchies.Merchandise },
                new HierarchyClassModel { HierarchyClassId = 5, HierarchyClassName = "Test HierarchyClass5", HierarchyId = Hierarchies.Merchandise }
            };
            command.HierarchyClasses = hierarchyClasses;

            //When
            commandHandler.Execute(command);

            //Then
            var actualHierarchyClasses = dbProvider.Connection.Query<dynamic>("SELECT * FROM dbo.HierarchyClass",
                null,
                dbProvider.Transaction).ToList();
            Assert.AreEqual(existingNum + 5, actualHierarchyClasses.Count);
            for (int i = 0; i < hierarchyClasses.Count; i++)
            {
                Assert.AreEqual(hierarchyClasses[i].HierarchyClassId, actualHierarchyClasses[i].HierarchyClassID);
                Assert.AreEqual(hierarchyClasses[i].HierarchyClassName, actualHierarchyClasses[i].HierarchyClassName);
                Assert.AreEqual(hierarchyClasses[i].HierarchyId, actualHierarchyClasses[i].HierarchyID);
            }
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClassesCommand_Given5ExistingHierarchyClasses_ShouldUpdateClasses()
        {
            //Given
            var existingNum = dbProvider.Connection.Query<int>(
                "select count(*) from dbo.HierarchyClass", transaction: dbProvider.Transaction).First();

            List<HierarchyClassModel> hierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel { HierarchyClassId = 1, HierarchyClassName = "Test HierarchyClass1", HierarchyId = Hierarchies.Merchandise },
                new HierarchyClassModel { HierarchyClassId = 2, HierarchyClassName = "Test HierarchyClass2", HierarchyId = Hierarchies.Merchandise },
                new HierarchyClassModel { HierarchyClassId = 3, HierarchyClassName = "Test HierarchyClass3", HierarchyId = Hierarchies.Merchandise },
                new HierarchyClassModel { HierarchyClassId = 4, HierarchyClassName = "Test HierarchyClass4", HierarchyId = Hierarchies.Merchandise },
                new HierarchyClassModel { HierarchyClassId = 5, HierarchyClassName = "Test HierarchyClass5", HierarchyId = Hierarchies.Merchandise }
            };
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(dbProvider.Connection as SqlConnection,
                SqlBulkCopyOptions.Default,
                dbProvider.Transaction as SqlTransaction))
            {
                sqlBulkCopy.DestinationTableName = "dbo.HierarchyClass";
                sqlBulkCopy.WriteToServer(hierarchyClasses
                    .Select(hc => new
                    {
                        hc.HierarchyClassId,
                        hc.HierarchyId,
                        hc.HierarchyClassName
                    }).ToList().ToDataTable());
            }

            foreach (var hc in hierarchyClasses)
            {
                hc.HierarchyClassName = hc.HierarchyClassName + "Updated";
            }

            command.HierarchyClasses = hierarchyClasses;

            //When
            commandHandler.Execute(command);

            //Then
            var actualHierarchyClasses = dbProvider.Connection.Query<dynamic>("SELECT * FROM dbo.HierarchyClass",
                null,
                dbProvider.Transaction).ToList();
            Assert.AreEqual(existingNum + 5, actualHierarchyClasses.Count);
            for (int i = 0; i < hierarchyClasses.Count; i++)
            {
                Assert.AreEqual(hierarchyClasses[i].HierarchyClassId, actualHierarchyClasses[i].HierarchyClassID);
                Assert.AreEqual(hierarchyClasses[i].HierarchyClassName, actualHierarchyClasses[i].HierarchyClassName);
                Assert.AreEqual(hierarchyClasses[i].HierarchyId, actualHierarchyClasses[i].HierarchyID);
            }
        }
    }
}
