using Wfm.Aws.ExtendedClient.SQS.Model;

namespace MammothR10Price.Message.Parser
{
    public interface IMessageParser<T>
    {
        T ParseMessage(SQSExtendedClientReceiveModel receivedMessage);
    }
}
