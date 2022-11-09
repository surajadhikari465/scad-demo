using System;
using Icon.Dvs.Model;

namespace Icon.Dvs.ListenerApplication
{
    public interface IListenerApplication
    {
        void Start();
        void Stop();
        void HandleMessage(DvsMessage message);
        void HandleException(Exception ex, DvsMessage message);
    }
}
