using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.ActiveMQ.Producer;
using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributePublisher.Operations.Decorators
{
    public class ManageConsumerConnectionOperationDecorator : IOperation<AttributePublisherServiceParameters>
    {
        private IOperation<AttributePublisherServiceParameters> operation;
        private IEsbProducer producer;
        private IActiveMQProducer activeMQProducer;

        public ManageConsumerConnectionOperationDecorator(
            IOperation<AttributePublisherServiceParameters> operation,
            IEsbProducer producer,
            IActiveMQProducer activeMQProducer)
        {
            this.operation = operation;
            this.producer = producer;
            this.activeMQProducer = activeMQProducer;
        }

        public void Execute(AttributePublisherServiceParameters parameters)
        {
            // activeMQProducer will open connection by itself during send operation
            producer.OpenConnection();
            
            operation.Execute(parameters);
            
            producer.Dispose();
            activeMQProducer.Dispose();
        }
    }
}
