using AttributePublisher.Services;
using System.Collections.Generic;

namespace AttributePublisher.MessageBuilders
{
    public class AttributeMessageHeaderBuilder : IMessageHeaderBuilder
    {
        private const string NonReceivingSysName = "nonReceivingSysName";
        private const string TransactionType = "TransactionType";
        private const string Source = "Source";

        private AttributePublisherServiceSettings settings;

        public AttributeMessageHeaderBuilder(AttributePublisherServiceSettings settings)
        {
            this.settings = settings;
        }

        public Dictionary<string, string> BuildHeader()
        {
            return new Dictionary<string, string>
            {
                { NonReceivingSysName, settings.NonReceivingSystems },
                { TransactionType, AttributePublisherResources.MessageTransactionType },
                { Source, AttributePublisherResources.MessageSource }
            };
        }
    }
}
