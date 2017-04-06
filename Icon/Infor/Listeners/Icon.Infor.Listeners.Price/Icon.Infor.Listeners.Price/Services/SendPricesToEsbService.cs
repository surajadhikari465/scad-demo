using Esb.Core.MessageBuilders;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.Constants;
using Icon.Infor.Listeners.Price.EsbFactory;
using Icon.Infor.Listeners.Price.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.Services
{
    public class SendPricesToEsbService : IService<PriceModel>
    {
        private EsbConnectionSettings settings;
        private IEsbConnectionSettingsFactory settingsFactory;
        private IEsbConnectionFactory esbConnectionFactory;
        private IMessageBuilder<IEnumerable<PriceModel>> messageBuilder;

        public SendPricesToEsbService(
            EsbConnectionSettings settings,
            IEsbConnectionSettingsFactory settingsFactory,
            IEsbConnectionFactory esbConnectionFactory,
            IMessageBuilder<IEnumerable<PriceModel>> messageBuilder)
        {
            this.settings = settingsFactory.CreateConnectionSettings(typeof(SendPricesToEsbService));
            this.settingsFactory = settingsFactory;
            this.esbConnectionFactory = esbConnectionFactory;
            this.messageBuilder = messageBuilder;
        }

        public void Process(IEnumerable<PriceModel> data, IEsbMessage message)
        {
            var failedPrices = data.Where(price => price.ErrorCode == null)
                .ToList();

            if (!failedPrices.Any())
            {
                return;
            }

            try
            {
                using (var producer = esbConnectionFactory.CreateProducer(settings))
                {
                    producer.OpenConnection();
                    var failedPricesMessage = messageBuilder.BuildMessage(failedPrices);
                    producer.Send(failedPricesMessage, message.GetProperty("MessageID"));
                }
            }
            catch (Exception ex)
            {
                foreach (var price in failedPrices)
                {
                    price.ErrorCode = Errors.Codes.SendPricesToEsbError;
                    price.ErrorDetails = ex.ToString();
                }
            }
        }
    }
}
