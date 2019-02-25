using System.Collections.Generic;

namespace KitBuilderWebApi.QueryParameters
{
	public class PriceCollectionRequestModel
	{
		public IEnumerable<StoreItem> StoreItems { get; set; }
		public bool IncludeFuturePrices { get; set; }
		public string PriceType { get; set; }
	}
}