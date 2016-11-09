using Icon.Esb.R10Listener.Context;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace Icon.Esb.R10Listener.Tests
{
    [TestClass]
    public class CommandHandlerTestBase<CommandHandlerType, CommandType>
    {
        protected CommandHandlerType commandHandler;
        protected CommandType command;
        protected GlobalContext context;
        protected DbContextTransaction transaction;

        [TestInitialize]
        public void BaseInitialize()
        {
            context = new GlobalContext(new IconContext());
            transaction = context.Context.Database.BeginTransaction();
            Initialize();
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            Cleanup();
            transaction.Rollback();
            transaction.Dispose();
            context.Context.Dispose();
        }

        protected virtual void Initialize() { }

        protected virtual void Cleanup() { }
    }
}
