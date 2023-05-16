using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.ActiveMQ.Producer;

namespace AttributePublisher.Operations.Decorators
{
    public class ManageConsumerConnectionOperationDecorator : IOperation<AttributePublisherServiceParameters>
    {
        private IOperation<AttributePublisherServiceParameters> operation;
        private IActiveMQProducer activeMQProducer;

        public ManageConsumerConnectionOperationDecorator(
            IOperation<AttributePublisherServiceParameters> operation,
            IActiveMQProducer activeMQProducer)
        {
            this.operation = operation;
            this.activeMQProducer = activeMQProducer;
        }

        public void Execute(AttributePublisherServiceParameters parameters)
        {
            operation.Execute(parameters);

            activeMQProducer.Dispose();
        }
    }
}
