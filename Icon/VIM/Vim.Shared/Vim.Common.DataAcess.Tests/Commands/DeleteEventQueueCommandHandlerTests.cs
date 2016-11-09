using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vim.Common.DataAccess.Commands;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using Dapper;

namespace Vim.Common.DataAccess.Tests.Commands
{
    [TestClass]
    public class DeleteEventQueueCommandHandlerTests
    {
        private DeleteEventQueueCommandHandler commandHandler;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new DeleteEventQueueCommandHandler(dbProvider);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void DeleteLocaleQueueData_DataExist_ShouldDeleteData()
        {
            //Given
            int testEventReferenceId = 1234567;
            var queueIds = dbProvider.Connection.Query<int>(@"
                declare @QueueIds table(QueueId int)

                insert into vim.EventQueue(EventTypeId, EventReferenceId, InsertDate)
                    output inserted.QueueId 
                        into @QueueIds
                values (@EventTypeId, @EventReferenceId, @InsertDate)
                
                select * from @QueueIds",
                new { EventTypeId = EventTypes.LocaleAdd, EventReferenceId = testEventReferenceId, InsertDate = DateTime.Now },
                dbProvider.Transaction);

            Assert.AreEqual(1, dbProvider.Connection.Query<int>(@"select count(*) from vim.EventQueue where EventReferenceId = @Id", new { Id = testEventReferenceId }, transaction: dbProvider.Transaction).First());

            //When
            commandHandler.Execute(new DeleteEventQueueCommand
            {
                QueueIds = queueIds.ToList()
            });

            //Then
            Assert.AreEqual(0, dbProvider.Connection.Query<int>(@"select count(*) from vim.EventQueue where EventReferenceId = @Id", new { Id = testEventReferenceId }, transaction: dbProvider.Transaction).First());
        }
    }
}
