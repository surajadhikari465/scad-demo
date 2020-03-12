using System;
using TIBCO.EMS;

namespace Icon.Esb.Subscriber
{
    public interface IEsbSubscriber : IEsbConnection
    {
        event EventHandler<EsbMessageEventArgs> MessageReceived;
        
        void BeginListening();

    }
}
