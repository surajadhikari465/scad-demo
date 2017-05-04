using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Mammoth.ApiController.DataAccess.Tests
{
    public class QueryHandlerTestBase<TQueryHandler, TParameters, TContext>
        where TQueryHandler : class
        where TParameters : class, new()
        where TContext : DbContext, new()
    {
        protected TQueryHandler queryHandler;
        protected TParameters parameters;
        protected TContext context;
        protected TransactionScope transaction;

        [TestInitialize]
        public void BaseInitialize()
        {
            transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
            parameters = new TParameters();
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
