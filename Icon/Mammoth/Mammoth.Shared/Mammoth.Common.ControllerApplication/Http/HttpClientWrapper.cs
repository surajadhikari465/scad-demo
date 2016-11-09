using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Mammoth.Common.ControllerApplication.Http
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private HttpClientSettings settings;

        public HttpClientWrapper(HttpClientSettings settings)
        {
            this.settings = settings;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string uri, T value)
        {
            using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }, true))
            {
                client.BaseAddress = new Uri(settings.BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string json = JsonConvert.SerializeObject(value);

                using (HttpContent content = new StringContent(json))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsJsonAsync(uri, value);
                    return response;
                }
            }
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string uri, T value)
        {
            using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }, true))
            {
                client.BaseAddress = new Uri(settings.BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string json = JsonConvert.SerializeObject(value);

                using(HttpContent content = new StringContent(json))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PutAsync(uri, content);
                    return response;
                }
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri, IEnumerable<int> ids)
        {
            using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }, true))
            {
                client.BaseAddress = new Uri(settings.BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                var queryString = HttpUtility.ParseQueryString(String.Empty);
                foreach (var id in ids)
                {
                    queryString.Add("ids", id.ToString());
                }

                var response = await client.DeleteAsync(uri + queryString.ToString());

                return response;
            }
        }
    }
}