
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.HealthyEatingRating auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class HealthyEatingRatings
    {
        public const int Good = 1;
        public const int Better = 2;
        public const int Best = 3;

		private static Dictionary<string, int> descriptionToIdDictionary = new Dictionary<string, int>
			{
				{ "Good", 1 },
				{ "Better", 2 },
				{ "Best", 3 }
			};
		public static Dictionary<string, int> Ids { get { return descriptionToIdDictionary; } }
		
		public class Descriptions
		{
			public const string Good = "Good";
			public const string Better = "Better";
			public const string Best = "Best";

			private static string[] descriptions = new string[]
				{
					"Good",
					"Better",
					"Best"
				};
			public static string[] AsArray { get { return descriptions; } }
		}
		
		private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "Good" },
				{ 2, "Better" },
				{ 3, "Best" }
			};
		public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
	}
}
