using Wfm.Aws.ExtendedClient.SQS.Model;

namespace MammothR10Price.Message.Processor
{
    public interface IMessageProcessor
    {
        void ProcessReceivedMessage(SQSExtendedClientReceiveModel message);
    }
}
