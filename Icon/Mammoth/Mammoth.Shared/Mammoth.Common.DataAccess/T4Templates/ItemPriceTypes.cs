
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
		
		public class Descriptions
		{
			public const string RegularPrice = "Regular Price";
			public const string TemporaryPriceReduction = "Temporary Price Reduction";
		
			private static Dictionary<string, string> codeToDescriptionsDictionary = new Dictionary<string, string>
			{
				{ "REG", "Regular Price" },
				{ "TPR", "Temporary Price Reduction" }
			};
			public static Dictionary<string, string> ByCode { get { return codeToDescriptionsDictionary; } }
		}

		public class Codes
		{
			public const string RegularPrice = "REG";
			public const string TemporaryPriceReduction = "TPR";
		}
	}
}
