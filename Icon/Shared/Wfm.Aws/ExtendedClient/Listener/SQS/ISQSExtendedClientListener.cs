using System;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace Wfm.Aws.ExtendedClient.Listener.SQS
{
    public interface ISQSExtendedClientListener
    {
        void Start();
        void Stop();
        void HandleMessage(SQSExtendedClientReceiveModel message);
        void HandleException(Exception ex, SQSExtendedClientReceiveModel message);
    }
}
