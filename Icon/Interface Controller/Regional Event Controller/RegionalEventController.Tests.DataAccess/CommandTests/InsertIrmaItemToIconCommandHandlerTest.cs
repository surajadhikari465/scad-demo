using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Models;
using Moq;
using Icon.Logging;
using Icon.Testing.Builders;
using RegionalEventController.Common;

namespace RegionalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class InsertIrmaItemToIconCommandHandlerTest
    {
        private Mock<ILogger<InsertIrmaItemToIconCommandHandler>> mockLogger;
        private IconContext context;
        private InsertIrmaItemToIconCommand command;
        private InsertIrmaItemToIconCommandHandler handler;
        private IRMAItem irmaItemEntry;
        private DbContextTransaction transaction;

        private int defaultOrganicAgencyId;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<InsertIrmaItemToIconCommandHandler>>();
            this.context = new IconContext(); 
            this.command = new InsertIrmaItemToIconCommand();
            this.handler = new InsertIrmaItemToIconCommandHandler(this.mockLogger.Object, this.context);
            this.irmaItemEntry = new IRMAItem();
            this.defaultOrganicAgencyId = 99999;

            StartupOptions.Instance = 123;
            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        private void BuildEventQueueEntry()
        {
            var random = new Random();
            irmaItemEntry = new TestIrmaItemBuilder()
                .WithIdentifier(random.Next(1000000, 1000000000).ToString())
                .WithRegionCode("MW")
                .WithOrganicAgencyId(defaultOrganicAgencyId)
                .Build();

        }
        [TestMethod]
        public void InsertIrmaItemToIcon_IrmaItemIsReady_IrmaItemIsInserted()
        {
            // Given
            BuildEventQueueEntry();
            command.irmaNewItem = irmaItemEntry;

            // When
            this.handler.Execute(this.command);

            // Then
            Assert.IsTrue(context.IRMAItem.Any(ii => ii.identifier == irmaItemEntry.identifier && ii.regioncode == irmaItemEntry.regioncode && ii.OrganicAgencyId == defaultOrganicAgencyId));
        }
    }
}
