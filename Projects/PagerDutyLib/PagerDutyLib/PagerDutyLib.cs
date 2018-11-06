using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace PagerDutyLib
{
    public class APIClientInfo
    {
        readonly string name;
        readonly string url;

        public APIClientInfo(string name, string url)
        {
            this.name = name;
            this.url = url;
        }

        public string Name
        {
            get { return name; }
        }
        public string Url
        {
            get { return url; }
        }
    }

    // <summary> 
    // Base class for context support. This allows you to send
    // extra rich data along with a trigger event.
    // </summary>
    public class Context
    {
        public string @type { get; set; }

        protected Context(string context_type)
        {
            this.@type = context_type;
        }
    }

    // <summary>
    // A link-type context
    // </summary>
    public class Link : Context
    {
        public string href { get; set; }
        public string text { get; set; }

        public Link(string href, string text) : base("link")
        {
            this.href = href;
            this.text = text;
        }
    }

    // <summary>
    // An image-type context
    // </summary>
    public class Image : Context
    {
        public string src { get; set; }
        public string href { get; set; }

        public Image(string src, string href) : base("image")
        {
            this.src = src;
            this.href = href;
        }
    }


    class BaseRequest
    {
        public string service_key { get; set; }
        public string event_type { get; set; }
        public string description { get; set; }
        public string incident_key { get; set; }
        public Object details { get; set; }
    }

    // <summary>
    // Wrapper representing the JSON body for a Trigger Event
    // </summary>
    class TriggerRequest : BaseRequest
    {
        public string client { get; set; }
        public string client_url { get; set; }
        public List<Context> contexts { get; set; }

        public static TriggerRequest MakeRequest(
            APIClientInfo client, string serviceKey,
            string description, string incidentKey, Object details, List<Context> contexts)
        {

            return new TriggerRequest()
            {
                event_type = "trigger",
                service_key = serviceKey,
                description = description,
                incident_key = incidentKey,
                client = client.Name,
                client_url = client.Url,
                details = details,
                contexts = contexts,
            };
        }
    }

    // <summary>
    // Wrapper representing the JSON body for an Acknowledge Event
    // </summary>
    class AcknowledgeRequest : BaseRequest
    {
        public static AcknowledgeRequest MakeRequest(
            string serviceKey, string description,
            string incidentKey, string data)
        {

            return new AcknowledgeRequest()
            {
                event_type = "acknowledge",
                service_key = serviceKey,
                description = description,
                incident_key = incidentKey,
                details = data
            };
        }
    }

    // <summary>
    // Wrapper representing the JSON body for a Resolve Event
    // </summary>
    class ResolveRequest : BaseRequest
    {
        public static ResolveRequest MakeRequest(
             string serviceKey,
             string description, string incidentKey, string data)
        {

            return new ResolveRequest()
            {
                event_type = "resolve",
                service_key = serviceKey,
                description = description,
                incident_key = incidentKey,
                details = data
            };
        }
    }

    // <summary>
    // Representation of API response
    // </summary>
    public class EventAPIResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string incident_key { get; set; }
    }

    // <summary>
    // Integration API main class. Instance of this class are bound
    // to a certain service key and API client to make the argument
    // list on actual event sending methods as small as possible
    // </summary>
    public class IntegrationAPI
    {
        const string EVENT_API_URL = "https://events.pagerduty.com//generic/2010-04-15/create_event.json";
        const string REGISTRY_PATH = "\\Software\\PagerDuty\\ServiceKeys";
        
        // Default retry: 0, 5, 10, 20 seconds.
        readonly Retry DEFAULT_RETRY = new Retry(TimeSpan.FromSeconds(5), 3, true);

        // <summary>
        // Make a new instance of a client, bound to the indicated client information
        // and service key.
        // </summary>
        // <param name=apiClientInfo>The client information for this instance</param>
        // <param name=serviceKey>The service key to use</param>
        // <param name=retry>The Retry instance to use</param>
        public static IntegrationAPI MakeClient(
                APIClientInfo apiClientInfo,
                string serviceKey,
                Retry retry = null)
        {
            RestClient client = new RestClient(EVENT_API_URL);
            return new IntegrationAPI(client, apiClientInfo, serviceKey, retry);
        }

        // <summary>
        // Make a new instance of a client, getting the key from the Windows registry.
        // </summary>
        // <param name=apiClientInfo>The client information for this instance</param>
        // <param name=root>The registry root to use</param>
        // <param name=serviceName>The service name to look up</param>
        // <param name=retry>The Retry instance to use</param>
        public static IntegrationAPI MakeClient(
                APIClientInfo apiClientInfo,
                string root,
                string serviceName,
                Retry retry = null)
        {
            var path = root + REGISTRY_PATH;
            var key = (string)Microsoft.Win32.Registry.GetValue(path, serviceName, "notfound");
            if (key == null || key.Equals("notfound"))
            {
                throw new ApplicationException("Registry value for service " + serviceName + " not found in " + path);
            }
            return MakeClient(apiClientInfo, key, retry);
        }

        readonly RestClient client;
        readonly APIClientInfo apiClientInfo;
        readonly string serviceKey;
        readonly Retry retry;

        private IntegrationAPI(RestClient client, APIClientInfo apiClientInfo, string serviceKey, Retry retry)
        {
            this.client = client;
            this.serviceKey = serviceKey;
            this.apiClientInfo = apiClientInfo;
            this.retry = retry ?? DEFAULT_RETRY;
        }

        // <summary>
        // Send a trigger to PagerDuty
        // </summary>
        // <param name="description">The description (summary) of the trigger</param>
        // <param name="data">Extra optional data to send along</param>
        // <param name="incidentKey">The incidentKey (if null, PagerDuty will create one)</param>
        public EventAPIResponse Trigger(string description, Object data, string incidentKey = null, List<Context> contexts = null)
        {
            var trigger = TriggerRequest.MakeRequest(apiClientInfo, serviceKey, description, incidentKey, data, contexts);
            return Execute(trigger);
        }


        // <summary>
        // Send an acknowledgement to PagerDuty
        // </summary>
        // <param name="incidentKey">The incident key for the open incident</param>
        // <param name="description">Description for the acknowledgement</param>
        // <param name="details">Extra optional data to send along</param>
        public EventAPIResponse Acknowledge(string incidentKey, string description, string details)
        {
            var acknowledge = AcknowledgeRequest.MakeRequest(serviceKey, description, incidentKey, details);
            return Execute(acknowledge);
        }

        // <summary>
        // Send a resolve to PagerDuty
        // </summary>
        // <param name="incidentKey">The incident key for the open incident</param>
        // <param name="description">Description for the resolve</param>
        // <param name="data">Extra optional data to send along</param>
        public EventAPIResponse Resolve(string incidentKey, string description, string data)
        {
            var resolve = ResolveRequest.MakeRequest(serviceKey, description, incidentKey, data);
            return Execute(resolve);
        }

        // Send the request and return the resulting data (or an error)
        private EventAPIResponse Execute(BaseRequest request)
        {
            return retry.Do(() => TryExecute(request));
        }

        private IEither<Exception, EventAPIResponse> TryExecute(BaseRequest request)
        {
            var restRequest = new RestRequest(Method.POST);
            restRequest.AddJsonBody(request);
            var response = client.Execute<EventAPIResponse>(restRequest);
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details";
                return Either.Left<Exception, EventAPIResponse>(
                    new ApplicationException(message, response.ErrorException));
            }

            if (response.StatusCode == HttpStatusCode.Forbidden ||
                response.StatusCode == HttpStatusCode.InternalServerError ||
                response.StatusCode == HttpStatusCode.BadGateway ||
                response.StatusCode == HttpStatusCode.ServiceUnavailable ||
                response.StatusCode == HttpStatusCode.GatewayTimeout)
            {

                return Either.Left<Exception, EventAPIResponse>(
                    new ApplicationException("Bad HTTP Status Code: " + response.StatusCode));
            }

            return Either.Right<Exception, EventAPIResponse>(response.Data);
        }
    }

    public static class Either
    {
        public static IEither<L, R> Left<L, R>(L value)
        {
            return new ILeft<L, R>(value);
        }
        public static IEither<L, R> Right<L, R>(R value)
        {
            return new IRight<L, R>(value);
        }
    }

    public interface IEither<L, R>
    {
        void OnLeft(Action<L> f);
        void OnRight(Action<R> f);
    }
    class ILeft<L, R> : IEither<L, R>
    {
        L value;
        public ILeft(L value) { this.value = value; }
        public void OnLeft(Action<L> f) { f(value); }
        public void OnRight(Action<R> f) { }
    }
    class IRight<L, R> : IEither<L, R>
    {
        R value;
        public IRight(R value) { this.value = value; }
        public void OnLeft(Action<L> f) { }
        public void OnRight(Action<R> f) { f(value); }
    }

    public class Retry
    {
        readonly TimeSpan retryInterval;
        readonly int retryCount;
        readonly bool exponentialBackoff;

        public Retry(TimeSpan retryInterval, int retryCount = 3, bool exponentialBackoff = true)
        {
            this.retryInterval = retryInterval;
            this.retryCount = retryCount;
            this.exponentialBackoff = exponentialBackoff;
        }

        // <summary>
        // Execute the function under retry logic
        // </summary>
        public R Do<R>(Func<IEither<Exception, R>> fun)
        {
            var nothing = default(R);
            var exceptions = new List<Exception>();
            var currentTimeout = retryInterval;
            for (int retry = 0; retry < retryCount; retry++)
            {
                var response = fun();
                R result = nothing;
                response.OnLeft(exceptions.Add);
                response.OnRight((r) => result = r);

                if (!ReferenceEquals(result, nothing))
                {
                    return result;
                }
                else {
                    Thread.Sleep(currentTimeout);
                    currentTimeout = exponentialBackoff ?
                        currentTimeout + currentTimeout :
                        currentTimeout;
                }
            }
            throw new AggregateException(exceptions);
        }
    }
}
