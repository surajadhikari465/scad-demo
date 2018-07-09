using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using Mammoth.Price.Controller.Common;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Queries;
using System;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.ApplicationModules
{
    public class PriceQueueManager : IQueueManager
    {
        private PriceControllerApplicationSettings settings;
        private IQueryHandler<UpdateAndGetEventQueueInProcessParameters, List<EventQueueModel>> updateAndGetEventQueueInProcessQueryHandler;
        private ICommandHandler<DeleteEventQueueCommand> deleteEventQueueCommandHandler;
        private ILogger logger;

        public PriceQueueManager(PriceControllerApplicationSettings settings,
            IQueryHandler<UpdateAndGetEventQueueInProcessParameters, List<EventQueueModel>> updateAndGetEventQueueInProcessQueryHandler,
            ICommandHandler<DeleteEventQueueCommand> deleteEventQueueCommandHandler,
            ILogger logger)
        {
            this.settings = settings;
            this.updateAndGetEventQueueInProcessQueryHandler = updateAndGetEventQueueInProcessQueryHandler;
            this.deleteEventQueueCommandHandler = deleteEventQueueCommandHandler;
            this.logger = logger;
        }

        public List<EventQueueModel> GetEvents()
        {
            try
            {
                return updateAndGetEventQueueInProcessQueryHandler.Search(new UpdateAndGetEventQueueInProcessParameters
                {
                    Instance = settings.Instance,
                    MaxNumberOfRowsToMark = settings.MaxNumberOfRowsToMark
                });
            }
            catch (Exception ex)
            {
                logger.Error(new { Region = settings.CurrentRegion, Message = "Error occurred when getting events." }.ToJson(), ex);
                return new List<EventQueueModel>();
            }
        }

        public void DeleteInProcessEvents()
        {
            try
            {
                deleteEventQueueCommandHandler.Execute(new DeleteEventQueueCommand
                {
                    Instance = settings.Instance
                });
            }
            catch (Exception ex)
            {
                logger.Error(new { Region = settings.CurrentRegion, Message = "Error occurred when deleting events." }.ToJson(), ex);
            }
        }
    }
}