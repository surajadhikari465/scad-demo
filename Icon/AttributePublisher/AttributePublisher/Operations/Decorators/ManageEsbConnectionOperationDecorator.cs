using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributePublisher.Operations.Decorators
{
    public class ManageEsbConnectionOperationDecorator : IOperation<AttributePublisherServiceParameters>
    {
        private IOperation<AttributePublisherServiceParameters> operation;
        private IEsbProducer producer;

        public ManageEsbConnectionOperationDecorator(
            IOperation<AttributePublisherServiceParameters> operation,
            IEsbProducer producer)
        {
            this.operation = operation;
            this.producer = producer;
        }

        public void Execute(AttributePublisherServiceParameters parameters)
        {
            producer.OpenConnection();
            
            operation.Execute(parameters);
            
            producer.Dispose();
        }
    }
}
