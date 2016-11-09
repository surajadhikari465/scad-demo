
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.ItemType auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class ItemTypes
    {
        public const int RetailSale = 1;
        public const int Deposit = 2;
        public const int Tare = 3;
        public const int Return = 4;
        public const int Coupon = 5;
        public const int NonRetail = 6;
        public const int Fee = 7;
		
		private static Dictionary<string, int> codeToIdDictionary = new Dictionary<string, int>
			{
				{ "RTL", 1 },
				{ "DEP", 2 },
				{ "TAR", 3 },
				{ "RTN", 4 },
				{ "CPN", 5 },
				{ "NRT", 6 },
				{ "FEE", 7 }
			};
		public static Dictionary<string, int> Ids { get { return codeToIdDictionary; } }

		public static class Codes
		{
			public const string RetailSale = "RTL";
			public const string Deposit = "DEP";
			public const string Tare = "TAR";
			public const string Return = "RTN";
			public const string Coupon = "CPN";
			public const string NonRetail = "NRT";
			public const string Fee = "FEE";

			private static string[] codes = new string[]
				{
					"RTL",
					"DEP",
					"TAR",
					"RTN",
					"CPN",
					"NRT",
					"FEE"
				};
			public static string[] AsArray { get { return codes; } }
		
		
			private static Dictionary<int, string> idToCodesDictionary = new Dictionary<int, string>
			{
				{ 1, "RTL" },
				{ 2, "DEP" },
				{ 3, "TAR" },
				{ 4, "RTN" },
				{ 5, "CPN" },
				{ 6, "NRT" },
				{ 7, "FEE" }
			};
			public static Dictionary<int, string> AsDictionary { get { return idToCodesDictionary; } }
		}

		public static class Descriptions
		{
			public const string RetailSale = "Retail Sale";
			public const string Deposit = "Deposit";
			public const string Tare = "Tare";
			public const string Return = "Return";
			public const string Coupon = "Coupon";
			public const string NonRetail = "Non-Retail";
			public const string Fee = "Fee";

			private static string[] descriptions = new string[]
				{
					"Retail Sale",
					"Deposit",
					"Tare",
					"Return",
					"Coupon",
					"Non-Retail",
					"Fee"
				};
			public static string[] AsArray { get { return descriptions; } }
				
			private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "Retail Sale" },
				{ 2, "Deposit" },
				{ 3, "Tare" },
				{ 4, "Return" },
				{ 5, "Coupon" },
				{ 6, "Non-Retail" },
				{ 7, "Fee" }
			};
			public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
		}
	}
}
