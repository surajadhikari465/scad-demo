using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.ActiveMQ.Producer;
using Icon.Esb.Producer;
using System.Threading;

namespace AttributePublisher.Operations
{
    public class SendAttributesToConsumerOperation : OperationBase<AttributePublisherServiceParameters>
    {
        private IEsbProducer producer;
        private IActiveMQProducer activeMQProducer;

        public SendAttributesToConsumerOperation(IOperation<AttributePublisherServiceParameters> next, IEsbProducer producer, IActiveMQProducer activeMQProducer) : base(next)
        {
            this.producer = producer;
            this.activeMQProducer = activeMQProducer;
        }

        protected override void ExecuteImplementation(AttributePublisherServiceParameters parameters)
        {
            foreach (var message in parameters.AttributeMessages)
            {
                activeMQProducer.Send(message.Message, message.MessageHeaders);
                producer.Send(message.Message, message.MessageHeaders);
            }
        }
    }
}
