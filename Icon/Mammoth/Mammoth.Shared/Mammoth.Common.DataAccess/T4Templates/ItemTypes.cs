
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Mammoth.Common.DataAccess
{
    /// <summary>
    /// dbo.ItemTypes auto generated Ids
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
		
		public class Descriptions
		{
			public const string RetailSale = "Retail Sale";
			public const string Deposit = "Deposit";
			public const string Tare = "Tare";
			public const string Return = "Return";
			public const string Coupon = "Coupon";
			public const string NonRetail = "Non-Retail";
			public const string Fee = "Fee";
		
			private static Dictionary<string, string> codeToDescriptionsDictionary = new Dictionary<string, string>
			{
				{ "RTL", "Retail Sale" },
				{ "DEP", "Deposit" },
				{ "TAR", "Tare" },
				{ "RTN", "Return" },
				{ "CPN", "Coupon" },
				{ "NRT", "Non-Retail" },
				{ "FEE", "Fee" }
			};
			public static Dictionary<string, string> ByCode { get { return codeToDescriptionsDictionary; } }
		}

		public class Codes
		{
			public const string RetailSale = "RTL";
			public const string Deposit = "DEP";
			public const string Tare = "TAR";
			public const string Return = "RTN";
			public const string Coupon = "CPN";
			public const string NonRetail = "NRT";
			public const string Fee = "FEE";
		}
	}
}
