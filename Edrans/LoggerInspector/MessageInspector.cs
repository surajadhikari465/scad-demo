using log4net;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace LoggerInspector
{
    public class MessageInspector : IDispatchMessageInspector
    {
        string Now() => DateTime.Now.ToString("dd/MMM/yyyy:HH:mm:ss");

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var prop = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
            var id = prop.Headers.GetValues("CorrelationID").FirstOrDefault();

            var ilog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            var action = OperationContext.Current.IncomingMessageHeaders.Action;
            var operationName = action.Substring(action.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);

            ilog.Info($"{ id } { operationName } { Now() }");

            var state = new CorrelationState(id, operationName, ilog);

            state.StartWatch();

            return state;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var state = (CorrelationState)correlationState;

            var logger = state.Logger;

            logger.Info($"{ state.CorrelationID } { state.methodName } { Now() } isFaulted={reply.IsFault} { state.Watch.ElapsedMilliseconds }ms");
        }
    }
}