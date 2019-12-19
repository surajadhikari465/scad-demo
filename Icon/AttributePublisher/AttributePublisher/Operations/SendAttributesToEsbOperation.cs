using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.Esb.Producer;
using System.Threading;

namespace AttributePublisher.Operations
{
    public class SendAttributesToEsbOperation : OperationBase<AttributePublisherServiceParameters>
    {
        private IEsbProducer producer;

        public SendAttributesToEsbOperation(IOperation<AttributePublisherServiceParameters> next, IEsbProducer producer) : base(next)
        {
            this.producer = producer;
        }

        protected override void ExecuteImplementation(AttributePublisherServiceParameters parameters)
        {
            foreach (var message in parameters.AttributeMessages)
            {
                producer.Send(message.Message, message.MessageId, message.MessageHeaders);
            }
        }
    }
}
