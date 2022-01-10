using Icon.ApiController.Common;
using Icon.ApiController.Controller.HistoryProcessors;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.DbContextFactory;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using Icon.ActiveMQ.Producer;

namespace Icon.ApiController.Controller.ControllerBuilders
{
    public static class BuilderHelpers
    {
        public static MessageHistoryProcessor BuildMessageHistoryProcessor(string instance, int messageTypeId, IEsbProducer producer, IDbContextFactory<IconContext> iconContextFactory, ActiveMQProducer activeMqProducer = null)
        {
            ApiControllerSettings settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);

            return new MessageHistoryProcessor(
                settings,
                new NLogLoggerInstance<MessageHistoryProcessor>(instance),
                new MarkUnsentMessagesAsInProcessCommandHandler(new NLogLoggerInstance<MarkUnsentMessagesAsInProcessCommandHandler>(instance), iconContextFactory),
                new GetMessageHistoryQuery(new NLogLoggerInstance<GetMessageHistoryQuery>(instance), iconContextFactory),
                new UpdateMessageHistoryStatusCommandHandler(new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), iconContextFactory),
                new UpdateStagedProductStatusCommandHandler(new NLogLoggerInstance<UpdateStagedProductStatusCommandHandler>(instance), iconContextFactory),
                new UpdateSentToEsbHierarchyTraitCommandHandler(new NLogLoggerInstance<UpdateSentToEsbHierarchyTraitCommandHandler>(instance), iconContextFactory),
                new IsMessageHistoryANonRetailProductMessageQuery(iconContextFactory),
                producer,
                messageTypeId,
                activeMqProducer);
        }
    }
}
