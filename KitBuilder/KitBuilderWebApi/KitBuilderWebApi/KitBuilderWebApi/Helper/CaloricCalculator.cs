using KitBuilder.DataAccess.Dto;
using KitBuilderWebApi.QueryParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace KitBuilderWebApi.Helper
{
	public class CaloricCalculator 
	{
		private int kitLocaleId;
		private int storeLocaleId;
		private IRepository<KitLocale> kitLocaleRepository;
		private ILogger<CaloricCalculator> logger;

		public CaloricCalculator(int kitLocaleId, int storeLocaleId, IRepository<KitLocale> kitLocaleRepository, ILogger<CaloricCalculator> logger)
		{
			this.logger = logger;
			this.kitLocaleId = kitLocaleId;
			this.storeLocaleId = storeLocaleId;
			this.kitLocaleRepository =  kitLocaleRepository;

	}

		public KitLocale Run()
		{
			KitLocale kitLocale = GetKitByKitLocaleId(kitLocaleId);
			kitLocale.KitLinkGroupLocale
			return  GetKitByKitLocaleId(kitLocaleId);
		}

		internal KitLocale GetKitByKitLocaleId(int kitLocaleId)
		{
			return (kitLocaleRepository.UnitOfWork.Context.KitLocale.Where(kl => kl.KitLocaleId == kitLocaleId)
					.Include(kll => kll.KitLinkGroupLocale).ThenInclude(k => k.KitLinkGroupItemLocale)).FirstOrDefault(); 
		}

		internal async Task<IEnumerable<ItemStorePriceModel>> GetAuthorizedStatus(ICollection<StoreItem> storeItems)
		{
			string url = "http://mammoth-test/api/price/";

			using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
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
	}
}
