using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ApiController.DataAccess.Tests
{
    public class CommandHandlerTestBase<TCommandHandler, TCommand, TContext>
        where TCommandHandler : class
        where TCommand : class, new()
        where TContext : DbContext, new()
    {
        protected TCommandHandler commandHandler;
        protected TCommand command;
        protected TContext context;
        protected DbContextTransaction transaction;

        [TestInitialize]
        public void BaseInitialize()
        {
            context = new TContext();
            command = new TCommand();
            transaction = context.Database.BeginTransaction();
            Initialize();
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            if (transaction.UnderlyingTransaction.Connection != null && transaction.UnderlyingTransaction.Connection.State == ConnectionState.Open)
            {
                transaction.Rollback();
                transaction.Dispose();
                context.Dispose();
            }
            Cleanup();
        }

        protected virtual void Initialize() { }
        protected virtual void Cleanup() { }
    }
}
