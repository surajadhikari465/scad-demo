using KitBuilder.DataAccess.Dto;
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
	public class GetAuthorizedStatusAndPriceService : IService<IEnumerable<StoreItem>, Task<IEnumerable<ItemStorePriceModel>>>
    {
        private readonly IApiHelper apiHelper;
        public GetAuthorizedStatusAndPriceService(IApiHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        public async Task<IEnumerable<ItemStorePriceModel>> Run(IEnumerable<StoreItem> parameters)
		{
			PriceCollectionRequestModel pricesRequestModel = new PriceCollectionRequestModel
			{
				StoreItems = parameters,
				IncludeFuturePrices = false,
				PriceType = "REG"
			};

			string pricesRequestJson = JsonConvert.SerializeObject(pricesRequestModel);
			HttpContent inputContent = new StringContent(pricesRequestJson, Encoding.UTF8, "application/json");

            apiHelper.InitializeClient();
			string uri = apiHelper.BaseUri + "price";
            apiHelper.ApiClient.BaseAddress = new Uri(uri);

			try
			{
				using (HttpResponseMessage response = await apiHelper.ApiClient.PostAsync(uri, inputContent))
				{
					if (response.IsSuccessStatusCode)
					{
						IEnumerable<ItemStorePriceModel> itemStorePrices = await response.Content.ReadAsAsync<IEnumerable<ItemStorePriceModel>>();
						return itemStorePrices;
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
