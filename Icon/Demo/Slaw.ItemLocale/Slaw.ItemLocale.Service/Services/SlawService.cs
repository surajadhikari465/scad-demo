using Icon.Esb.Producer;
using Slaw.ItemLocale.Service.MessageBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slaw.ItemLocale.Service
{
    public class SlawService
    {
        private ItemLocaleMessageBuilder messageBuilder;
        private IEsbProducer producer;
        private ItemLocaleRepository repository;

        public SlawService(ItemLocaleRepository repository, ItemLocaleMessageBuilder messageBuilder, IEsbProducer producer)
        {
            this.repository = repository;
            this.messageBuilder = messageBuilder;
            this.producer = producer;
        }

        public void AddOrUpdateItemLocales(IEnumerable<ItemLocaleModel> models)
        {
            var message = messageBuilder.BuildMessage(models);
            var id = repository.SaveMessageHistory(message);
            producer.Send(message, new Dictionary<string, string> { { "IconMessageId", id.ToString() } });
        }
    }
}
