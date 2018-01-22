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

        /// <summary>
        /// Available external systems which may need to receive a price refresh
        /// </summary>
        public static string[] JustInTimeDownstreamSystems = new[]
            {
                PriceRefreshConstants.R10,
                PriceRefreshConstants.IRMA
            };
    }

	public static class EventConstants
    {
        public const string ItemLocaleAddOrUpdateEvent = "ItemLocaleAddOrUpdate";
        public const string ItemPriceEvent = "Price";
	}

	public static class PriceResetConstants
	{
		public const string Spice = "Spice";
		public const string SLAW = "SLAW";
	}

    public static class PriceRefreshConstants
    {
        public const string R10 = "R10";
        public const string IRMA = "IRMA";
    }

    public static class PriceTypes
    {
        public static class Codes
        {
            public const string Reg = "REG";
            public const string Tpr = "TPR";
        }
    }
}
