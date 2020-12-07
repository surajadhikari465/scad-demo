using System;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading;
using System.Threading.Tasks;

namespace LoggerMiddleware.Implementation.EndpointBehavior
{
    public class CorrelationIDDelegatingHandler : DelegatingHandler
    {
        string correlationID;
        string correlationIDHeaderName;

        public CorrelationIDDelegatingHandler(HttpClientHandler handler, string correlationID, string correlationIDHeaderName)
        {
            InnerHandler = handler;
            this.correlationID = correlationID;
            this.correlationIDHeaderName = correlationIDHeaderName;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add(correlationIDHeaderName, correlationID);
            return base.SendAsync(request, cancellationToken);
        }
    }

    public class CorrelationIDEndpointBehavior : IEndpointBehavior
    {
        string correlationID;
        string correlationIDHeaderName;

        public CorrelationIDEndpointBehavior(string correlationID, string correlationIDHeaderName)
        {
            this.correlationID = correlationID;
            this.correlationIDHeaderName = correlationIDHeaderName;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            bindingParameters.Add(new Func<HttpClientHandler, HttpMessageHandler>(x => new CorrelationIDDelegatingHandler(x, correlationID, correlationIDHeaderName)));
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }
        public void Validate(ServiceEndpoint endpoint) { }
    }
}
