
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.ScanCodeType auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class ScanCodeTypes
    {
        public const int Upc = 1;
        public const int PosPlu = 2;
        public const int ScalePlu = 3;
		
		private static Dictionary<string, int> descriptionToIdDictionary = new Dictionary<string, int>
			{
				{ "UPC", 1 },
				{ "POS PLU", 2 },
				{ "Scale PLU", 3 }
			};
		public static Dictionary<string, int> Ids { get { return descriptionToIdDictionary; } }

		public static class Descriptions
		{
			public const string Upc = "UPC";
			public const string PosPlu = "POS PLU";
			public const string ScalePlu = "Scale PLU";

			private static string[] descriptions = new string[]
				{
					"UPC",
					"POS PLU",
					"Scale PLU"
				};
			public static string[] AsArray { get { return descriptions; } }
				
			private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "UPC" },
				{ 2, "POS PLU" },
				{ 3, "Scale PLU" }
			};
			public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
		}
	}
}
