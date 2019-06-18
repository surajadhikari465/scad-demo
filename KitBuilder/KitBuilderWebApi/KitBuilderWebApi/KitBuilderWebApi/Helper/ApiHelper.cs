using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KitBuilderWebApi.Helper
{
    public class ApiHelper : IApiHelper
    {
        private readonly IConfiguration configuration;

        public ApiHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public  HttpClient ApiClient { get; set; }
        public string BaseUri { get; set ; }

        public  void InitializeClient()
		{
			BaseUri = configuration["WebApiBaseAddress:MammothBaseAddress"];

			ApiClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
			ApiClient.DefaultRequestHeaders.Accept.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
	}
}