using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KitBuilderWebApi.Services
{
	public class GetNutritionService : IService<ItemNutritionRequestModel, Task<IEnumerable<ItemNutritionAttributesDictionary>>>
	{
        private readonly IApiHelper apiHelper;

        public GetNutritionService(IApiHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        public async Task<IEnumerable<ItemNutritionAttributesDictionary>> Run(ItemNutritionRequestModel parameters)
		{
			string pricesRequestJson = JsonConvert.SerializeObject(parameters);
			HttpContent inputContent = new StringContent(pricesRequestJson, Encoding.UTF8, "application/json");

            apiHelper.InitializeClient();
			string uri = apiHelper.BaseUri + "itemNutrition";
            apiHelper.ApiClient.BaseAddress = new Uri(uri);

			try
			{
				using (HttpResponseMessage response = await apiHelper.ApiClient.PostAsync(uri, inputContent))
				{
					if (response.IsSuccessStatusCode)
					{
						IEnumerable<ItemNutritionAttributesDictionary> itemCaloricInfo = await response.Content.ReadAsAsync<IEnumerable<ItemNutritionAttributesDictionary>>();
						return itemCaloricInfo;
					}
					else
					{
						throw new Exception(response.ReasonPhrase);
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}
