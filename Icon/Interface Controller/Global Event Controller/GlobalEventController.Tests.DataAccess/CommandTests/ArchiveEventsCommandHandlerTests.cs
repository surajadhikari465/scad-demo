using GlobalEventController.Common;
using GlobalEventController.DataAccess.Commands;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class ArchiveEventsCommandHandlerTests
    {
        private ArchiveEventsCommandHandler commandHandler;
        private ArchiveEventsCommand command;
        private IconContext context;
        private TransactionScope transaction;
        private IconDbContextFactory contextFactory;
        private List<ArchiveEventModelWrapper<FailedEvent>> testFailedEvents;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new IconContext();

            contextFactory = new IconDbContextFactory();
            commandHandler = new ArchiveEventsCommandHandler(contextFactory);
            command = new ArchiveEventsCommand();
            testFailedEvents = new List<ArchiveEventModelWrapper<FailedEvent>>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ArchiveEvents_EventsExist_InsertsEventsIntoArchiveTable()
        {
            //Given
            var numberOfEvents = 10;
            command.Events = BuildTestEvents(numberOfEvents);

            //When
            commandHandler.Handle(command);

            //Then
            //only look at recent records, then count
            var sql = "select * from (" +
                "    select * from app.EventQueueArchive" +
                "    where EventQueueArchiveId > (select max(EventQueueArchiveId) - 2000 from app.EventQueueArchive)" +
                ") as a where a.EventMessage like '%ArchiveEventsCommandHandlerTests'";
            var eventQueueArchiveCount = context.Database.SqlQuery<EventQueueArchive>(sql).Count();
            Assert.AreEqual(numberOfEvents, eventQueueArchiveCount);
        }

        private List<EventQueueArchive> BuildTestEvents(int numberOfEvents)
        {
            var events = new List<EventQueueArchive>();

            for (int i = 0; i < 10; i++)
            {
                events.Add(
                    new EventQueueArchive
                    {
                        Context = JsonConvert.SerializeObject(
                            new EventQueue
                            {
                                EventId = EventTypes.ItemUpdate,
                                EventMessage = i + "ArchiveEventsCommandHandlerTests",
                                EventReferenceId = i + 10,
                                InProcessBy = i.ToString() + "InProcessBy",
                                InsertDate = DateTime.Now,
                                ProcessFailedDate = DateTime.Now,
                                QueueId = i,
                                RegionCode = "FL"
                            }),
                        ErrorCode = "Test",
                        ErrorDetails = "Test",
                        EventId = EventTypes.ItemUpdate,
                        EventMessage = i + "ArchiveEventsCommandHandlerTests",
                        EventQueueInsertDate = DateTime.Now,
                        EventReferenceId = i + 10,
                        QueueId = i,
                        RegionCode = "FL"
                    });
            }

            return events;
        }
    }
}
