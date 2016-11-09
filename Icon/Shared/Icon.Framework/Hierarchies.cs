
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.Hierarchy auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class Hierarchies
    {
        public const int Merchandise = 1;
        public const int Brands = 2;
        public const int Tax = 3;
        public const int Browsing = 4;
        public const int Financial = 5;
        public const int National = 6;
        public const int CertificationAgencyManagement = 7;

		private static Dictionary<string, int> descriptionToIdDictionary = new Dictionary<string, int>
			{
				{ "Merchandise", 1 },
				{ "Brands", 2 },
				{ "Tax", 3 },
				{ "Browsing", 4 },
				{ "Financial", 5 },
				{ "National", 6 },
				{ "Certification Agency Management", 7 }
			};
		public static Dictionary<string, int> Ids { get { return descriptionToIdDictionary; } }

		public static class Names
		{
			public const string Merchandise = "Merchandise";
			public const string Brands = "Brands";
			public const string Tax = "Tax";
			public const string Browsing = "Browsing";
			public const string Financial = "Financial";
			public const string National = "National";
			public const string CertificationAgencyManagement = "Certification Agency Management";

			private static string[] descriptions = new string[]
				{
					"Merchandise",
					"Brands",
					"Tax",
					"Browsing",
					"Financial",
					"National",
					"Certification Agency Management"
				};
			public static string[] AsArray { get { return descriptions; } }

			private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "Merchandise" },
				{ 2, "Brands" },
				{ 3, "Tax" },
				{ 4, "Browsing" },
				{ 5, "Financial" },
				{ 6, "National" },
				{ 7, "Certification Agency Management" }
			};
			public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
		}
	}
}
