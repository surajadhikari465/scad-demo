using GlobalEventController.Common;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class ArchiveEventsCommandHandlerTests
    {
        private ArchiveEventsCommandHandler commandHandler;
        private ArchiveEventsCommand command;
        private Mock<IContextManager> mockContextManager;
        private IconContext context;
        private List<ArchiveEventModelWrapper<FailedEvent>> testFailedEvents;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();

            mockContextManager = new Mock<IContextManager>();
            mockContextManager.SetupGet(m => m.IconContext).Returns(context);
            commandHandler = new ArchiveEventsCommandHandler(mockContextManager.Object);
            command = new ArchiveEventsCommand();
            testFailedEvents = new List<ArchiveEventModelWrapper<FailedEvent>>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            var sql = "delete from app.EventQueueArchive" +
                " where EventQueueArchiveId > (select max(EventQueueArchiveId) - 2000 from app.EventQueueArchive)" +
                " and EventMessage like '%ArchiveEventsCommandHandlerTests'";
            context.Database.ExecuteSqlCommand(sql);
            context.Dispose();
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
