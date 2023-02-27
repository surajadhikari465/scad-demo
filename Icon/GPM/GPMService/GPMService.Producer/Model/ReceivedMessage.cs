using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace GPMService.Producer.Model
{
    internal class ReceivedMessage
    {
        public SQSExtendedClientReceiveModel sqsExtendedClientMessage { get; set; }
        public ISQSExtendedClient sqsExtendedClient { get; set; }
        public SQSExtendedClientListenerSettings sqsExtendedClientSettings { get; set; }
    }
}
