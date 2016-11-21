using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Icon.Esb.Factory;
using Icon.Infor.Listeners.HierarchyClass.Requests;
using System;
using System.Linq;
using Icon.Esb;
using Icon.Infor.Listeners.HierarchyClass.Constants;

namespace Icon.Infor.Listeners.HierarchyClass.EsbService
{
    public class HierarchyClassEsbService : IEsbService<HierarchyClassEsbServiceRequest>
    {
        private IEsbConnectionFactory esbConnectionFactory;
        private IMessageBuilder<HierarchyClassEsbServiceRequest> messageBuilder;

        public EsbConnectionSettings Settings
        {
            get { return esbConnectionFactory.Settings; }
            set { esbConnectionFactory.Settings = value; }
        }

        public HierarchyClassEsbService(
            IEsbConnectionFactory esbConnectionFactory,
            IMessageBuilder<HierarchyClassEsbServiceRequest> messageBuilder)
        {
            this.esbConnectionFactory = esbConnectionFactory;
            this.messageBuilder = messageBuilder;
        }

        public EsbServiceResponse Send(HierarchyClassEsbServiceRequest request)
        {
            EsbServiceMessage message = new EsbServiceMessage
            {
                MessageId = request.MessageId
            };

            try
            {
                using (var producer = esbConnectionFactory.CreateProducer(Settings))
                {
                    producer.OpenConnection();

                    message.Text = messageBuilder.BuildMessage(request);
                    
                    producer.Send(message.Text, request.MessageId);
                }

                return new EsbServiceResponse
                {
                    Status = EsbServiceResponseStatus.Sent,
                    Message = message
                };
            }
            catch (Exception ex)
            {
                return new EsbServiceResponse
                {
                    Message = message,
                    Status = EsbServiceResponseStatus.Failed,
                    ErrorCode = ApplicationErrors.Codes.UnableToSendHierarchyClassesToVim,
                    ErrorDetails = ex.ToString()
                };
            }
        }
    }
}
