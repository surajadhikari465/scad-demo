using Icon.ApiController.Common;
using Icon.ApiController.Controller.HistoryProcessors;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.RenewableContext;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.ApiController.Controller.ControllerBuilders
{
    public static class BuilderHelpers
    {
        public static MessageHistoryProcessor BuildMessageHistoryProcessor(string instance, int messageTypeId, IEsbProducer producer, IRenewableContext<IconContext> globalContext)
        {
            ApiControllerSettings settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);

            var messageHistoryProcessor = new MessageHistoryProcessor(
                settings,
                new NLogLoggerInstance<MessageHistoryProcessor>(instance),
                globalContext,
                new MarkUnsentMessagesAsInProcessCommandHandler(new NLogLoggerInstance<MarkUnsentMessagesAsInProcessCommandHandler>(instance), globalContext),
                new GetMessageHistoryQuery(new NLogLoggerInstance<GetMessageHistoryQuery>(instance), globalContext),
                new UpdateMessageHistoryStatusCommandHandler(new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), globalContext),
                new UpdateStagedProductStatusCommandHandler(new NLogLoggerInstance<UpdateStagedProductStatusCommandHandler>(instance), globalContext),
                new UpdateSentToEsbHierarchyTraitCommandHandler(new NLogLoggerInstance<UpdateSentToEsbHierarchyTraitCommandHandler>(instance), globalContext),
                producer,
                messageTypeId);

            return messageHistoryProcessor;
        }
    }
}
