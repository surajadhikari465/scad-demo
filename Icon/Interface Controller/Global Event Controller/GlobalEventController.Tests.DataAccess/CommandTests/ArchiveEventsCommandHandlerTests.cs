using GlobalEventController.Common;
using GlobalEventController.DataAccess.Commands;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Icon.Logging;
using Moq;


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
        private Mock<ILogger<ArchiveEventsCommandHandler>> mockLogger;
        private List<ArchiveEventModelWrapper<FailedEvent>> testFailedEvents;
        
        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new IconContext();

            contextFactory = new IconDbContextFactory();
            this.mockLogger = new Mock<ILogger<ArchiveEventsCommandHandler>>();
            commandHandler = new ArchiveEventsCommandHandler(contextFactory, mockLogger.Object);
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

        [TestMethod]
        public void ArchiveEvents_ScanCodeExistsConstraintErrorExists_DeleteDateUpdated()
        {
            //Given
            var numberOfEvents = 5;
            command.Events = BuildTestEventsScanCodeExistsConstraint(numberOfEvents);
            foreach (var eventQueue in command.Events)
            {
                context.IRMAItemSubscription.Add(new IRMAItemSubscription()
                {
                    IRMAItemSubscriptionID = eventQueue.EventId,
                    regioncode = eventQueue.RegionCode,
                    identifier = eventQueue.EventMessage,
                    insertDate = DateTime.Now,
                    deleteDate = null
                });
                context.SaveChanges();

            }

            //When
            commandHandler.Handle(command);

            //Then
            var sql = "select * from app.IRMAItemSubscription" +
                      " where identifier like '%ScanCode%'";
            var iRMAItemSubscription = context.Database.SqlQuery<IRMAItemSubscription>(sql);
            Assert.AreEqual(5, iRMAItemSubscription.Count());
            Assert.AreEqual(iRMAItemSubscription.FirstOrDefault().deleteDate.Value.Date, DateTime.Now.Date);
        }

        [TestMethod]
        public void
            ArchiveEvents_ScanCodeExistsConstraintErrorExistsAndDeleteDateHasValue_SubscriptionDeleteDateNotUpdated()
        {
            //Given
            var numberOfEvents = 5;
            command.Events = BuildTestEventsScanCodeExistsConstraint(numberOfEvents);
            foreach (var eventQueue in command.Events)
            {
                context.IRMAItemSubscription.Add(new IRMAItemSubscription()
                {
                    IRMAItemSubscriptionID = eventQueue.EventId,
                    regioncode = eventQueue.RegionCode,
                    identifier = eventQueue.EventMessage,
                    insertDate = DateTime.Now,
                    deleteDate = DateTime.Now
                });
                context.SaveChanges();
            }

            //When
            commandHandler.Handle(command);

            //Then
            var sql = "select * from app.IRMAItemSubscription" +
                      " where identifier like '%ScanCode%'";
            var iRMAItemSubscription = context.Database.SqlQuery<IRMAItemSubscription>(sql);
            Assert.AreEqual(5, iRMAItemSubscription.Count());
            Assert.AreNotEqual(iRMAItemSubscription.FirstOrDefault().deleteDate, DateTime.Now);

        }


        [TestMethod]
        public void ArchiveEvents_NoErrorExists_SubscriptionDeleteDateNotUpdated()
        {
            //Given
            var numberOfEvents = 5;
            command.Events = BuildTestEventsScanCodeNoErrorExist(numberOfEvents);
            foreach (var eventQueue in command.Events)
            {
                context.IRMAItemSubscription.Add(new IRMAItemSubscription()
                {
                    IRMAItemSubscriptionID = eventQueue.EventId,
                    regioncode = eventQueue.RegionCode,
                    identifier = eventQueue.EventMessage,
                    insertDate = DateTime.Now,
                    deleteDate = null
                });
                context.SaveChanges();
            }

            //When
            commandHandler.Handle(command);

            //Then
            var sql = "select * from app.IRMAItemSubscription" +
                      " where identifier like '%ScanCode%'";
            var iRMAItemSubscription = context.Database.SqlQuery<IRMAItemSubscription>(sql);
            Assert.AreEqual(5, iRMAItemSubscription.Count());
            Assert.AreEqual(iRMAItemSubscription.FirstOrDefault().deleteDate, null);

        }

        [TestMethod]
        public void ArchiveEvents_ScanCodeExistsConstraintErrorExists_LoggerCalled()
        {
            //Given
            var numberOfEvents = 5;
            command.Events = BuildTestEventsScanCodeExistsConstraint(numberOfEvents);
            foreach (var eventQueue in command.Events)
            {
                context.IRMAItemSubscription.Add(new IRMAItemSubscription()
                {
                    IRMAItemSubscriptionID = eventQueue.EventId,
                    regioncode = eventQueue.RegionCode,
                    identifier = eventQueue.EventMessage,
                    insertDate = DateTime.Now,
                    deleteDate = null
                });
                context.SaveChanges();

            }

            //When
            commandHandler.Handle(command);

            //Then
            mockLogger.Verify(log => log.Error(It.IsAny<string>()),Times.Exactly(5),
                "Error was caught and the subscription was marked as deleted for Errorcode:ScanCodeExistsConstraint.");

        }


        [TestMethod]
        public void ArchiveEvents_ScanCodeExistsConstraintErrorDoesNotExists_LoggerNotCalled()
        {
            //Given
            var numberOfEvents = 5;
            command.Events = BuildTestEventsScanCodeNoErrorExist(numberOfEvents);
            foreach (var eventQueue in command.Events)
            {
                context.IRMAItemSubscription.Add(new IRMAItemSubscription()
                {
                    IRMAItemSubscriptionID = eventQueue.EventId,
                    regioncode = eventQueue.RegionCode,
                    identifier = eventQueue.EventMessage,
                    insertDate = DateTime.Now,
                    deleteDate = null
                });
                context.SaveChanges();
            }

            //When
            commandHandler.Handle(command);

            //Then
            mockLogger.Verify(log => log.Error(It.IsAny<string>()), Times.Never,
                "Error was caught and the subscription was marked as deleted for Errorcode:ScanCodeExistsConstraint.");

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

        private List<EventQueueArchive> BuildTestEventsScanCodeExistsConstraint(int numberOfEvents)
        {
            var events = new List<EventQueueArchive>();

            for (int i = 0; i < 5; i++)
            {
                events.Add(
                    new EventQueueArchive
                    {
                        Context = JsonConvert.SerializeObject(
                            new EventQueue
                            {
                                EventId = EventTypes.ItemUpdate,
                                EventMessage = i + "ScanCode",
                                EventReferenceId = i + 5,
                                InProcessBy = i.ToString() + "InProcessBy",
                                InsertDate = DateTime.Now,
                                ProcessFailedDate = DateTime.Now,
                                QueueId = i,
                                RegionCode = "FL"
                            }),
                        ErrorCode = "Test",
                        ErrorDetails = "ScanCodeExistsConstraint",
                        EventId = EventTypes.ItemUpdate,
                        EventMessage = i + "ScanCode",
                        EventQueueInsertDate = DateTime.Now,
                        EventReferenceId = i + 5,
                        QueueId = i,
                        RegionCode = "FL"
                    });
            }

            return events;
        }

        private List<EventQueueArchive> BuildTestEventsScanCodeNoErrorExist(int numberOfEvents)
        {
            var events = new List<EventQueueArchive>();

            for (int i = 0; i < 5; i++)
            {
                events.Add(
                    new EventQueueArchive
                    {
                        Context = JsonConvert.SerializeObject(
                            new EventQueue
                            {
                                EventId = EventTypes.ItemUpdate,
                                EventMessage = i + "ScanCode",
                                EventReferenceId = i + 5,
                                InProcessBy = i.ToString() + "InProcessBy",
                                InsertDate = DateTime.Now,
                                ProcessFailedDate = DateTime.Now,
                                QueueId = i,
                                RegionCode = "FL"
                            }),
                        ErrorCode = "Test",
                        ErrorDetails = "TestError",
                        EventId = EventTypes.ItemUpdate,
                        EventMessage = i + "ScanCode",
                        EventQueueInsertDate = DateTime.Now,
                        EventReferenceId = i + 5,
                        QueueId = i,
                        RegionCode = "FL"
                    });
            }

            return events;
        }
    }
}