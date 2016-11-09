using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddVimLocaleEventCommandHandlerTests
    {
        private AddVimEventCommandHandler commandHandler;
        private AddVimEventCommand command;
        private IconContext context;
        private DbContextTransaction transaction;
        private Locale testLocale;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            commandHandler = new AddVimEventCommandHandler(context);
            command = new AddVimEventCommand();

            testLocale = new TestLocaleBuilder()
                .WithLocaleName("Test Locale");

            context.Locale.Add(testLocale);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void AddVimLocaleEventCommandHandler_LocaleAddEventType_ShouldAddEvent()
        {
            //Given
            command.EventReferenceId = testLocale.localeID;
            command.EventTypeId = VimEventTypes.LocaleAdd;

            //When
            commandHandler.Execute(command);

            //Then
            var addEvent = context.VimEventQueue.Single(e => e.EventReferenceId == command.EventReferenceId);
            Assert.AreEqual(VimEventTypes.LocaleAdd, addEvent.EventTypeId);
        }

        [TestMethod]
        public void AddVimLocaleEventCommandHandler_LocaleUpdateEventType_ShouldAddEvent()
        {
            //Given
            command.EventReferenceId = testLocale.localeID;
            command.EventTypeId = VimEventTypes.LocaleUpdate;

            //When
            commandHandler.Execute(command);

            //Then
            var addEvent = context.VimEventQueue.Single(e => e.EventReferenceId == command.EventReferenceId);
            Assert.AreEqual(VimEventTypes.LocaleUpdate, addEvent.EventTypeId);
        }
    }
}
