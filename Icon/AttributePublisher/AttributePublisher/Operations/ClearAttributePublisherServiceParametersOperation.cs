using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;

namespace AttributePublisher.Operations
{
    public class ClearAttributePublisherServiceParametersOperation : OperationBase<AttributePublisherServiceParameters>
    {
        public ClearAttributePublisherServiceParametersOperation() : base(null)
        {
        }

        protected override void ExecuteImplementation(AttributePublisherServiceParameters parameters)
        {
            parameters.AttributeMessages.Clear();
            parameters.Attributes.Clear();
        }
    }
}
