using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
        protected TransactionScope transaction;

        [TestInitialize]
        public void BaseInitialize()
        {
            transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted });
            command = new TCommand();
            context = new TContext();
            Initialize();
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            context.Dispose();
            transaction.Dispose();
            Cleanup();
        }

        protected virtual void Initialize() { }
        protected virtual void Cleanup() { }
    }
}
