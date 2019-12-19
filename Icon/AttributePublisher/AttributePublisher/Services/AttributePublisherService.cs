using AttributePublisher.DataAccess.Commands;
using AttributePublisher.Infrastructure;
using AttributePublisher.Infrastructure.Operations;
using Icon.Common.DataAccess;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AttributePublisher.Services
{
    public class AttributePublisherService : RecurringServiceBase
    {
        private RecurringServiceSettings settings;
        private IOperation<AttributePublisherServiceParameters> operation;
        private ICommandHandler<AddAttributesToMessageQueueCommand> addAttributesToMessageQueueCommandHandler;
        private AttributePublisherServiceParameters parameters;
        private ILogger logger;

        public AttributePublisherService(
            RecurringServiceSettings settings,
            IOperation<AttributePublisherServiceParameters> operation,
            ICommandHandler<AddAttributesToMessageQueueCommand> addAttributesToMessageQueueCommandHandler,
            AttributePublisherServiceParameters parameters,
            ILogger logger) : base(settings)
        {
            this.settings = settings;
            this.operation = operation;
            this.addAttributesToMessageQueueCommandHandler = addAttributesToMessageQueueCommandHandler;
            this.parameters = parameters;
            this.logger = logger;
        }

        public override void Run()
        {
            do
            {
                parameters.ContinueProcessing = true;
                operation.Execute(parameters);
            } while (parameters.ContinueProcessing);
        }

        public override void HandleRunException(Exception ex)
        {
            logger.Error(ex.ToString());
        }

        public override void HandleServiceStart()
        {
            logger.Info(AttributePublisherResources.StartServiceLogMessage);
        }

        public override void HandleServiceStop()
        {
            logger.Info(AttributePublisherResources.StopServiceLogMessage);
            parameters.ContinueProcessing = false;

            if (parameters.Attributes.Any())
            {
                try
                {
                    logger.Info(AttributePublisherResources.RequeueingAttributesOnStop);
                    addAttributesToMessageQueueCommandHandler.Execute(new AddAttributesToMessageQueueCommand
                    {
                        Attributes = parameters.Attributes
                    });
                }
                catch (Exception ex)
                {
                    logger.Error($"{AttributePublisherResources.RequeueingAttributesOnStopError}: {ex.ToString()}");
                }
            }
        }
    }
}
