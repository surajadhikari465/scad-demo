using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using RegionalEventController.DataAccess.Queries;
using InterfaceController.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data.Entity;
using System.Configuration;

namespace RegionalEventController.Controller.ProcessorModules
{
    public class NewItemProcessingModule : INewItemProcessingModule
    {
        private ILogger<NewItemProcessingModule> logger;
        private ICommandHandler<InsertEventQueueToIconCommand> insertEventQueueToIconCommandHandler;
        private ICommandHandler<InsertIrmaItemSubscriptionToIconCommand> insertIrmaItemSubscriptionToIconCommandHandler;
        private ICommandHandler<InsertIrmaItemToIconCommand> insertIrmaItemToIconCommandHandler;

        public NewItemProcessingModule(
            ILogger<NewItemProcessingModule> logger,
            ICommandHandler<InsertEventQueueToIconCommand> insertEventQueueToIconCommandHandler,
            ICommandHandler<InsertIrmaItemSubscriptionToIconCommand> insertIrmaItemSubscriptionToIconCommandHandler,
            ICommandHandler<InsertIrmaItemToIconCommand> insertIrmaItemToIconCommandHandler)
        {
            this.logger = logger;
            this.insertEventQueueToIconCommandHandler = insertEventQueueToIconCommandHandler;
            this.insertIrmaItemSubscriptionToIconCommandHandler = insertIrmaItemSubscriptionToIconCommandHandler;
            this.insertIrmaItemToIconCommandHandler = insertIrmaItemToIconCommandHandler;
        }

        public void CreateEventQueueEntry(int eventId, string eventMessage, int eventReferenceId, string RegionCode)
        { 
            var command = new InsertEventQueueToIconCommand 
            { 
                eventQueueEntry = new EventQueue
                {
                    EventId = eventId,
                    EventMessage = eventMessage,
                    EventReferenceId = eventReferenceId,
                    RegionCode = RegionCode,
                    InsertDate = DateTime.Now
                }
            };

            insertEventQueueToIconCommandHandler.Execute(command);
        }
        public void CreateIrmaItemSubscription(string regioncode, string identifier)
        {
            var command = new InsertIrmaItemSubscriptionToIconCommand
            {
                irmaNewItemSubscription = new IRMAItemSubscription
                {
                    regioncode = regioncode,
                    identifier = identifier,
                    insertDate = DateTime.Now
                }
            };

            insertIrmaItemSubscriptionToIconCommandHandler.Execute(command);
        }
        public void InsertIrmaItemToIcon(IRMAItem irmaNewItem)
        {
            var command = new InsertIrmaItemToIconCommand
            {
                irmaNewItem = irmaNewItem
            };

            insertIrmaItemToIconCommandHandler.Execute(command);
        }

    }
}
