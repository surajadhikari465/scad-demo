namespace WebSupport.DataAccess
{
	public static class DataConstants
	{
		/// <summary>
		/// Available regions abbreviations.
		/// </summary>
		public static string[] WholeFoodsRegions = new[]
			{
				"FL", "MA", "MW", "NA", "RM", "SO", "NC", "NE", "PN", "SP", "SW", "UK"
			};

		/// <summary>
		/// Available external systems which may need to receive a price reset
		/// </summary>
		public static string[] DownstreamSystems = new[]
			{
				PriceResetConstants.Spice,
				PriceResetConstants.SLAW
			};
	}

	public static class EventConstants
    {
        public const string ItemLocaleAddOrUpdateEvent = "ItemLocaleAddOrUpdate";
        public const string ItemPriceEvent = "IRMAPrice";
	}

	public static class PriceResetConstants
	{
		public const string Spice = "Spice";
		public const string SLAW = "SLAW";
	}
}
