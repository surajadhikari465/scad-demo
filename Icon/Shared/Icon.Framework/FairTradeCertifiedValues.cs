
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.Trait auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class FairTradeCertifiedValues
    {
        public const string FairTradeUsa = "Fair Trade USA";
        public const string FairTradeInternational = "Fair Trade International";
        public const string ImoUsa = "IMO USA";
        public const string RainforestAlliance = "Rainforest Alliance";
        public const string WholeFoodsMarket = "Whole Foods Market";

		private static string[] descriptions = new string[]
				{
					"Fair Trade USA",
					"Fair Trade International",
					"IMO USA",
					"Rainforest Alliance",
					"Whole Foods Market"
				};
		public static string[] AsArray { get { return descriptions; } }

		public static IEnumerable<string> Values
		{
			get
			{
				return new List<string>
				{
					{ "Fair Trade USA" },
					{ "Fair Trade International" },
					{ "IMO USA" },
					{ "Rainforest Alliance" },
					{ "Whole Foods Market" }
				};
			}
		}
	}
}
