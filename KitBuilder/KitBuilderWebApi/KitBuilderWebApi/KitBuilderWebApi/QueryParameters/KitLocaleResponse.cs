namespace KitBuilderWebApi.QueryParameters
{
	public class KitLocaleResponse
	{
		public int? KitId { get; set; }
		public int? LocaleId { get; set; }
		public int? KitLocaleId { get; set; }
		public string LocaleName { get; set; }
		public int LocaleTypeId { get; set; }
		public int? StoreId { get; set; }
		public int? MetroId { get; set; }
		public int? RegionId { get; set; }
		public int? ChainId { get; set; }
		public string StoreAbbreviation { get; set; }
		public string RegionCode { get; set; }
		public int? BusinessUnitId { get; set; }
		public bool? Exclude { get; set; }
		public int? StatusId { get; set; }
	}
}
