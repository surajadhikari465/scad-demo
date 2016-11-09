
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Mammoth.Common.DataAccess
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
		private static Dictionary<string, int> idToNamesDictionary = new Dictionary<string, int>
		{
				{ "Merchandise", 1 },
				{ "Brands", 2 },
				{ "Tax", 3 },
				{ "Browsing", 4 },
				{ "Financial", 5 },
				{ "National", 6 },
				{ "Certification Agency Management", 7 }
			};
		public static Dictionary<string, int> ByName { get { return idToNamesDictionary; } }

		public class Names
		{
			public const string Merchandise = "Merchandise";
			public const string Brands = "Brands";
			public const string Tax = "Tax";
			public const string Browsing = "Browsing";
			public const string Financial = "Financial";
			public const string National = "National";
			public const string CertificationAgencyManagement = "Certification Agency Management";
		
		}
	}
}
