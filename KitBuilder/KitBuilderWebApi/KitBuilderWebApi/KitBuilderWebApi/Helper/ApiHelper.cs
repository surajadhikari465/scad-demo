using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KitBuilderWebApi.Helper
{
	public static class ApiHelper
	{
		public static HttpClient ApiClient { get; set; }

		public static void InitializeClient()
		{
			ApiClient = new HttpClient();
			//ApiClient.BaseAddress = new Uri(baseUri);
			ApiClient.DefaultRequestHeaders.Accept.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
		}
	}
}
