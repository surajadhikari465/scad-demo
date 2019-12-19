using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace OOS.Model.Infrastructure
{
    public abstract class JsonRestService<T>
    {
        public string ServiceUrl { get; private set; }

        protected JsonSerializer jsonSerializer;
        protected abstract dynamic Deserialize(JsonReader jasonReader);

        protected JsonRestService(string serviceUrl)
        {
            ServiceUrl = serviceUrl;
            jsonSerializer = new JsonSerializer();
        }

        /// <summary>
        /// Queries the WCF attributes of the method being called, builds the REST URL and sends the http request
        /// based on the WCF attribute parameters. When the JSON response is returned, it is changed to a dynamic object.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        protected dynamic Send(Func<T, dynamic> func)
        {
            //find the method on the main type that is being called... i'm sure there's a better way to do this,
            //but this does work.
            //This will not work if there are methods with overloads.
            var methodNameMatch = Regex.Match(func.Method.Name, "<(.*?)>");
            if (!methodNameMatch.Success && methodNameMatch.Groups.Count == 2)
            {
                throw new MissingMethodException("Could not find method " + func.Method.Name);
            }
            var realMethodName = methodNameMatch.Groups[1].Value;
            var m = typeof(T).GetMethods().Where(x => x.Name == realMethodName).SingleOrDefault();
            if (m == null)
            {
                throw new MissingMethodException("Could not find method" + realMethodName + " on type " +
                                                    typeof(T).FullName);
            }

            //now that we have the method, find the wcf attributes
            var a = m.GetCustomAttributes(false);
            var webGet = a.OfType<WebGetAttribute>().SingleOrDefault();
            var webInvoke = a.OfType<WebInvokeAttribute>().SingleOrDefault();
            var httpMethod = webGet != null ? "GET" : webInvoke != null ? webInvoke.Method : string.Empty;
            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException("The WebGet or WebInvoke attribute is missing from the method " +
                                                realMethodName);
            }

            //now that we have the WCF attributes, build the REST url based on the url template and the
            //method being called with it's parameters
            var urlTemplate = webGet != null ? webGet.UriTemplate : webInvoke.UriTemplate;
            var urlWithParams = GetUrlWithParams(urlTemplate, func);
            var url = ServiceUrl + urlWithParams;

            string output = string.Empty;
            //Do the web requests
            if (httpMethod == "GET")
            {
                output = HttpGet(url);

            }
            else
            {
                //need to move the query string params to the http parameters
                var parts = url.Split('?');
                output = HttpInvoke(parts[0], httpMethod, parts.Length > 1 ? parts[1] : string.Empty);
            }

            //change the response to json and then dynamic          
            var stringReader = new StringReader(output);
            var jReader = new JsonTextReader(stringReader);
            return Deserialize(jReader);
        }

        /// <summary>
        /// Updates the url template with the correct parameters
        /// </summary>
        /// <param name="template"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string GetUrlWithParams(string template, Func<T, dynamic> expression)
        {
            //parse the template, get the matches
            foreach (var m in Regex.Matches(template, @"\{(.*?)\}").Cast<Match>())
            {
                if (m.Groups.Count == 2)
                {
                    var m1 = m;
                    //find the fields based on the expression(Func<T>), get their values and replace the tokens in the url template
                    var field = expression.Target.GetType().GetFields().Where(x => x.Name == m1.Groups[1].Value).Single();
                    template = template.Replace(m.Groups[0].Value, field.GetValue(expression.Target).ToString());
                }
            }
            return template;
        }

        /// <summary>
        /// Do an Http GET
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string HttpGet(string uri)
        {
            var webClient = new WebClient();
            var data = webClient.DownloadString(uri);
            return data;
        }

        /// <summary>
        /// Do an Http POST/PUT/etc...
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static string HttpInvoke(string uri, string method, string parameters)
        {
            var webClient = new WebClient();
            var data = webClient.UploadString(uri, method, parameters);
            return data;
        }

        
    }
}
