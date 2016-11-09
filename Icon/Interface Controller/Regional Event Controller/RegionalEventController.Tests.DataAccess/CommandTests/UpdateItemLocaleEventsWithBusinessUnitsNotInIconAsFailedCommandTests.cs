using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionalEventController.Common.Models;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegionalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommandTests
    {
        private UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommandHandler commandHandler;
        private UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommand command;
        private Mock<IIconContextManager> mockContextManager;

        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockContextManager = new Mock<IIconContextManager>();
            mockContextManager.SetupGet(m => m.Context)
                .Returns(context);

            commandHandler = new UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommandHandler(mockContextManager.Object);
            command = new UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailedCommand { ItemLocaleEvents = new List<ItemLocaleEventModel>() };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void UpdateItemLocaleEventsWithBusinessUnitsNotInIconAsFailed_BusinessUnitsDontExistInIcon_ShouldUpdateEventAsFailed()
        {
            //Given
            int businessUnitThatExists = 1234;
            int businessUnitThatDoesntExist = 4321;
            Locale testLocale = new TestLocaleBuilder().WithBusinessUnitId(businessUnitThatExists);
            context.Locale.Add(testLocale);
            context.SaveChanges();

            command.ItemLocaleEvents.Add(new ItemLocaleEventModel { BusinessUnit = businessUnitThatExists });
            command.ItemLocaleEvents.Add(new ItemLocaleEventModel { BusinessUnit = businessUnitThatDoesntExist });

            //When
            commandHandler.Execute(command);

            //Then
            Assert.IsNull(command.ItemLocaleEvents[0].OutputError);
            Assert.AreEqual("Invalid entry. Business Unit 4321 does not exist in Icon.", command.ItemLocaleEvents[1].OutputError);
        }
    }
}
