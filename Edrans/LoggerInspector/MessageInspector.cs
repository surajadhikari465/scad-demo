using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace LoggerInspector
{
    public class MessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var prop = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
            var id = prop.Headers.GetValues("CorrelationID").FirstOrDefault();
            Debug.WriteLine(id);
            return new CorrelationState(id).StartWatch();
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var state = (CorrelationState)correlationState;
            Debug.WriteLine($"{ state.CorrelationID } { state.Watch.ElapsedMilliseconds }ms");
        }
    }
}
