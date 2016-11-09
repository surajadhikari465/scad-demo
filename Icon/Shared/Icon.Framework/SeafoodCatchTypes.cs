
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.SeafoodCatchType auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class SeafoodCatchTypes
    {
        public const int Wild = 1;
        public const int FarmRaised = 2;

		private static Dictionary<string, int> descriptionToIdDictionary = new Dictionary<string, int>
			{
				{ "Wild", 1 },
				{ "Farm Raised", 2 }
			};
		public static Dictionary<string, int> Ids { get { return descriptionToIdDictionary; } }
		
		public class Descriptions
		{
			public const string Wild = "Wild";
			public const string FarmRaised = "Farm Raised";

			private static string[] descriptions = new string[]
				{
					"Wild",
					"Farm Raised"
				};
			public static string[] AsArray { get { return descriptions; } }
		}
		
		private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "Wild" },
				{ 2, "Farm Raised" }
			};
		public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
	}
}
