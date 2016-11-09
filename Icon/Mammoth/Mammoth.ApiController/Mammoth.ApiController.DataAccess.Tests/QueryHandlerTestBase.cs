using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        protected DbContextTransaction transaction;

        [TestInitialize]
        public void BaseInitialize()
        {
            context = new TContext();
            parameters = new TParameters();
            transaction = context.Database.BeginTransaction();
            Initialize();
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
            Cleanup();
        }

        protected virtual void Initialize() { }
        protected virtual void Cleanup() { }
    }
}
