using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KitBuilderWebApi.Helper
{
	public static class ApiHelper
	{
		public static HttpClient ApiClient { get; set; }

		public static void InitializeClient(string baseUri)
		{
			ApiClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
			ApiClient.BaseAddress = new Uri(baseUri);
			ApiClient.DefaultRequestHeaders.Accept.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
		}
	}
}