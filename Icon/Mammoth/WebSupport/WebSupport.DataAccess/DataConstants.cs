using System.Collections.Generic;

namespace WebSupport.DataAccess
{
	public static class DataConstants
	{
		/// <summary>
		/// Available regions abbreviations.
		/// </summary>
		public static string[] WholeFoodsRegions = new[]
			{
                RegionNameConstants.FL,
                RegionNameConstants.MA,
                RegionNameConstants.MW,
                RegionNameConstants.NA,
                RegionNameConstants.RM,
                RegionNameConstants.SO,
                RegionNameConstants.NC,
                RegionNameConstants.NE,
                RegionNameConstants.PN,
                RegionNameConstants.SP,
                RegionNameConstants.SW,
                RegionNameConstants.UK,
                RegionNameConstants.TS
			};

		/// <summary>
		/// Available external systems which may need to receive a price reset
		/// </summary>
		public static string[] DownstreamSystems = new[]
			{
				PriceResetConstants.Spice,
				PriceResetConstants.Slaw,
				PriceResetConstants.DataWarehouse,
				PriceResetConstants.Pricer
			};

        /// <summary>
        /// Available external systems which may need to receive a price refresh
        /// </summary>
        public static string[] JustInTimeDownstreamSystems = new[]
            {
                PriceRefreshConstants.R10,
                PriceRefreshConstants.IRMA
            };

        /// <summary>
        /// External systems which needs a full load process to populate data from Mammoth
        /// </summary>
        public static string[] EPlumESLSystems = new[]
            {
                EPlumESLConstants.EPlum,
                EPlumESLConstants.ESL
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
		public const string Slaw = "Slaw";
		public const string DataWarehouse = "DW";
		public const string Pricer = "Pricer";
	}

    public static class RegionNameConstants
    {
        public const string FL = "FL";
        public const string MA = "MA";
        public const string MW = "MW";
        public const string NA = "NA";
        public const string RM = "RM";
        public const string SO = "SO";
        public const string NC = "NC";
        public const string NE = "NE";
        public const string PN = "PN";
        public const string SP = "SP";
        public const string SW = "SW";
        public const string UK = "UK";
        public const string TS = "TS";
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
            public const string Rwd = "RWD";
        }
    }

    public static class EPlumESLConstants
    {
        public const string EPlum = "EPlum";
        public const string ESL = "ESL";
    }

    public static class QueueEventTypes
    {
        public static readonly Dictionary<string, string> Events = new Dictionary<string, string>
            {
                {"INV_ADJ", "Inventory Adjustment" },
                {"PO_CRE", "Purchase Order Creation" },
                {"PO_LINE_DEL", "Purchase Order Line Item Deletion" },
                {"PO_DEL", "Purchase Order Deletion" },
                {"PO_MOD", "Purchase Order Modification" },
                {"RCPT_CRE", "Order Receipt Creation" },
                {"TSF_CRE", "Transfer Order Creation" },
                {"TSF_DEL", "Transfer Order Deletion" },
                {"TSF_LINE_DEL", "Transfer Line Item Deletion" },
            };
    }
}
