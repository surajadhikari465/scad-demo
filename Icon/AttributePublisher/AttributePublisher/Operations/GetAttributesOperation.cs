using AttributePublisher.DataAccess.Models;
using AttributePublisher.DataAccess.Queries;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace AttributePublisher.Operations
{
    public class GetAttributesOperation : OperationBase<AttributePublisherServiceParameters>
    {
        private IQueryHandler<GetAttributesParameters, List<AttributeModel>> getAttributesQueryHandler;
        private AttributePublisherServiceSettings settings;

        public GetAttributesOperation(
            IOperation<AttributePublisherServiceParameters> next, 
            IQueryHandler<GetAttributesParameters, List<AttributeModel>> getAttributesQueryHandler,
            AttributePublisherServiceSettings settings) : base(next)
        {
            this.getAttributesQueryHandler = getAttributesQueryHandler;
            this.settings = settings;
        }

        protected override void ExecuteImplementation(AttributePublisherServiceParameters parameters)
        {
            var attributeModels = getAttributesQueryHandler.Search(new GetAttributesParameters { RecordsPerQuery = settings.RecordsPerQuery });

            if (attributeModels.Any())
            {
                parameters.Attributes = attributeModels;
                parameters.ContinueProcessing = true;
            }
            else
            {
                parameters.ContinueProcessing = false;
            }
        }
    }
}
