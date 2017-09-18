using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Icon.Esb.Factory;
using System;

namespace Icon.Esb.Services.ConfirmationBod
{
    public class ConfirmationBodEsbService : IEsbService<ConfirmationBodEsbRequest>
    {
        private IEsbConnectionFactory esbConnectionFactory;
        private IMessageBuilder<ConfirmationBodEsbRequest> confirmationBodMessageBuilder;

        public EsbConnectionSettings Settings { get; set ; }

        public ConfirmationBodEsbService(
            EsbConnectionSettings settings,
            IEsbConnectionFactory esbConnectionFactory,
            IMessageBuilder<ConfirmationBodEsbRequest> confirmationBodMessageBuilder)
        {
            this.Settings = settings;
            this.esbConnectionFactory = esbConnectionFactory;
            this.confirmationBodMessageBuilder = confirmationBodMessageBuilder;
        }

        public EsbServiceResponse Send(ConfirmationBodEsbRequest request)
        {
            var message = confirmationBodMessageBuilder.BuildMessage(request);
            
            using (var producer = esbConnectionFactory.CreateProducer(Settings))
            {
                producer.OpenConnection();
                producer.Send(message, request.EsbMessageProperties);
            }

            return new EsbServiceResponse
            {
                Status = EsbServiceResponseStatus.Sent,
                Message = new EsbServiceMessage
                {
                    Text = message                    
                }
            };
        }
    }
}
