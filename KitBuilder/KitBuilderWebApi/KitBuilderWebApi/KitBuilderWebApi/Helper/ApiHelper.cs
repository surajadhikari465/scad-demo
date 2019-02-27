using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KitBuilderWebApi.Helper
{
	public static class ApiHelper
	{
		public static HttpClient ApiClient { get; set; }

		public static string BasedUri { get; set; }

		public static void InitializeClient()
		{
			IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddJsonFile("appsettings.json");
			IConfiguration configuration = configurationBuilder.Build();

			BasedUri = configuration["WebApiBaseAddress:MammothBaseAddress"];

			ApiClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
			ApiClient.DefaultRequestHeaders.Accept.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
	}
}