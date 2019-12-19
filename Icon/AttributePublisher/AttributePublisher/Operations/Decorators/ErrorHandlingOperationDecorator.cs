using AttributePublisher.DataAccess.Commands;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.Common.DataAccess;
using Icon.Logging;
using System;
using System.Linq;

namespace AttributePublisher.Operations.Decorators
{
    public class ErrorHandlingOperationDecorator : IOperation<AttributePublisherServiceParameters>
    {
        private IOperation<AttributePublisherServiceParameters> operation;
        private ICommandHandler<AddAttributesToMessageQueueCommand> addAttributesToMessageQueueCommandHandler;
        private ILogger logger;

        public ErrorHandlingOperationDecorator(
            IOperation<AttributePublisherServiceParameters> operation,
            ICommandHandler<AddAttributesToMessageQueueCommand> addAttributesToMessageQueueCommandHandler,
            ILogger logger)
        {
            this.operation = operation;
            this.addAttributesToMessageQueueCommandHandler = addAttributesToMessageQueueCommandHandler;
            this.logger = logger;
        }

        public void Execute(AttributePublisherServiceParameters parameters)
        {
            try
            {
                operation.Execute(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                parameters.ContinueProcessing = false;
                try
                {
                    if (parameters != null && parameters.Attributes.Any())
                    {
                        logger.Info(AttributePublisherResources.RequeueingAttributesOnError);
                        addAttributesToMessageQueueCommandHandler.Execute(new AddAttributesToMessageQueueCommand
                        {
                            Attributes = parameters.Attributes
                        });
                    }
                }
                catch(Exception innerEx)
                {
                    logger.Error(innerEx.ToString());
                }
            }
        }
    }
}
