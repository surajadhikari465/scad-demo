using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;

namespace AttributePublisher.Operations.Decorators
{
    public class CheckContinueProcessingOperationDecorator : IOperation<AttributePublisherServiceParameters>
    {
        private IOperation<AttributePublisherServiceParameters> operation;

        public CheckContinueProcessingOperationDecorator(IOperation<AttributePublisherServiceParameters> operation)
        {
            this.operation = operation;
        }

        public void Execute(AttributePublisherServiceParameters parameters)
        {
            if(parameters.ContinueProcessing)
                operation.Execute(parameters);
        }
    }
}
