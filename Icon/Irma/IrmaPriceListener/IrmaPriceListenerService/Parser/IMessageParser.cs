using Wfm.Aws.ExtendedClient.SQS.Model;

namespace IrmaPriceListenerService.Service.Parser
{
    public interface IMessageParser<T>
    {
        T ParseMessage(SQSExtendedClientReceiveModel receivedMessage);
    }
}
