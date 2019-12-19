using Icon.Esb.Factory;
using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    /// <summary>
    /// Class that encapsulates ESB functionality
    /// </summary>
    public class EsbClient : IEsbClient
    {
        private IEsbConnectionFactory esbConnectionFactory;
        private IEsbHeaderBuilder esbHeaderBuilder;
        private IEsbProducer producer;

        public EsbClient(IEsbConnectionFactory esbConnectionFactory, IEsbHeaderBuilder esbHeaderBuilder)
        {
            this.esbConnectionFactory = esbConnectionFactory;
            this.esbHeaderBuilder = esbHeaderBuilder;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Sends a message to the ESB
        /// </summary>
        /// <param name="request">ESB request</param>
        /// <returns></returns>
        public async Task<EsbSendResult> SendMessage(string request, List<string> nonReceivingSystems)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            Guid messageId = Guid.NewGuid();
            Dictionary<string, string> headers = new Dictionary<string, string>();

            try
            {
                if (this.producer == null || !this.producer.IsConnected)
                {
                    this.producer = this.esbConnectionFactory.CreateProducer();
                }

                headers = this.esbHeaderBuilder.BuildMessageHeader(nonReceivingSystems);
                producer.Send(request, headers);
                return new EsbSendResult(true, string.Empty, request, headers, messageId);
            }
            catch (Exception ex)
            {
                this.producer = null;
                return new EsbSendResult(false, "Error Occurred", request, headers, messageId, null, ex);
            }
        }
    }
}