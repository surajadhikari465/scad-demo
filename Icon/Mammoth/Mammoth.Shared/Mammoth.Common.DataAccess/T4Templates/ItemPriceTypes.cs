
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Mammoth.Common.DataAccess
{
    /// <summary>
    /// dbo.ItemPriceType auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class ItemPriceTypes
    {
		public const int RegularPrice = 1;
		public const int TemporaryPriceReduction = 2;
		public const int Competitive = 3;
		public const int EverydayValue = 4;
		public const int Clearance = 5;
		public const int Discontinued = 6;
		public const int MarketSale = 7;
		public const int SpecialSale = 8;
		public const int InStoreSpecial = 9;
		public const int GloballySetPrice = 10;
		
		public class Descriptions
		{
			public const string RegularPrice = "Regular Price";
			public const string TemporaryPriceReduction = "Temporary Price Reduction";
			public const string Competitive = "Competitive";
			public const string EverydayValue = "Everyday Value";
			public const string Clearance = "Clearance";
			public const string Discontinued = "Discontinued";
			public const string MarketSale = "Market Sale";
			public const string SpecialSale = "Special Sale";
			public const string InStoreSpecial = "In Store Special";
			public const string GloballySetPrice = "Globally Set Price";
		
			private static Dictionary<string, string> codeToDescriptionsDictionary = new Dictionary<string, string>
			{
				{ "REG", "Regular Price" },
				{ "TPR", "Temporary Price Reduction" },
				{ "CMP", "Competitive" },
				{ "EDV", "Everyday Value" },
				{ "CLR", "Clearance" },
				{ "DIS", "Discontinued" },
				{ "MSAL", "Market Sale" },
				{ "SSAL", "Special Sale" },
				{ "ISS", "In Store Special" },
				{ "GSP", "Globally Set Price" }
			};
			public static Dictionary<string, string> ByCode { get { return codeToDescriptionsDictionary; } }
		}

		public class Codes
		{
			public const string RegularPrice = "REG";
			public const string TemporaryPriceReduction = "TPR";
			public const string Competitive = "CMP";
			public const string EverydayValue = "EDV";
			public const string Clearance = "CLR";
			public const string Discontinued = "DIS";
			public const string MarketSale = "MSAL";
			public const string SpecialSale = "SSAL";
			public const string InStoreSpecial = "ISS";
			public const string GloballySetPrice = "GSP";
		}
	}
}
