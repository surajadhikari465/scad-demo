namespace KitBuilderWebApi.QueryParameters
{
	public class LocaleSearchParameters : BaseParameters
	{
		public string LocaleName { get; set; }
		public string StoreAbbreviation { get; set; }
		public int? BusinessUnitId { get; set; }
	}
}
