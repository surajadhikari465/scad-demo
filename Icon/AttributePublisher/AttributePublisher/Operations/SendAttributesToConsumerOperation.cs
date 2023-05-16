using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.ActiveMQ.Producer;

namespace AttributePublisher.Operations
{
    public class SendAttributesToConsumerOperation : OperationBase<AttributePublisherServiceParameters>
    {
        private IActiveMQProducer activeMQProducer;

        public SendAttributesToConsumerOperation(IOperation<AttributePublisherServiceParameters> next, IActiveMQProducer activeMQProducer) : base(next)
        {
            this.activeMQProducer = activeMQProducer;
        }

        protected override void ExecuteImplementation(AttributePublisherServiceParameters parameters)
        {
            foreach (var message in parameters.AttributeMessages)
            {
                activeMQProducer.Send(message.Message, message.MessageHeaders);
            }
        }
    }
}
