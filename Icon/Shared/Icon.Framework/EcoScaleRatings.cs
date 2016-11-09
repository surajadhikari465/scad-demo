
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.EcoScaleRating auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class EcoScaleRatings
    {
        public const int BaselineOrange = 1;
        public const int PremiumYellow = 2;
        public const int UltraPremiumGreen = 3;

		private static Dictionary<string, int> descriptionToIdDictionary = new Dictionary<string, int>
			{
				{ "Baseline/Orange", 1 },
				{ "Premium/Yellow", 2 },
				{ "Ultra-Premium/Green", 3 }
			};
		public static Dictionary<string, int> Ids { get { return descriptionToIdDictionary; } }
		
		public class Descriptions
		{
			public const string BaselineOrange = "Baseline/Orange";
			public const string PremiumYellow = "Premium/Yellow";
			public const string UltraPremiumGreen = "Ultra-Premium/Green";

			private static string[] descriptions = new string[]
				{
					"Baseline/Orange",
					"Premium/Yellow",
					"Ultra-Premium/Green"
				};
			public static string[] AsArray { get { return descriptions; } }
		}
		
		private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "Baseline/Orange" },
				{ 2, "Premium/Yellow" },
				{ 3, "Ultra-Premium/Green" }
			};
		public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
	}
}
