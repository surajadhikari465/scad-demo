
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.AnimalWelfareRating auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class AnimalWelfareRatings
    {
        public const int NoStep = 1;
        public const int Step1 = 2;
        public const int Step2 = 3;
        public const int Step3 = 4;
        public const int Step4 = 5;
        public const int Step5 = 6;
        public const int Step5Plus = 7;

		private static Dictionary<string, int> descriptionToIdDictionary = new Dictionary<string, int>
			{
				{ "No Step", 1 },
				{ "Step 1", 2 },
				{ "Step 2", 3 },
				{ "Step 3", 4 },
				{ "Step 4", 5 },
				{ "Step 5", 6 },
				{ "Step 5+", 7 }
			};
		public static Dictionary<string, int> Ids { get { return descriptionToIdDictionary; } }

		public class Descriptions
		{
			public const string NoStep = "No Step";
			public const string Step1 = "Step 1";
			public const string Step2 = "Step 2";
			public const string Step3 = "Step 3";
			public const string Step4 = "Step 4";
			public const string Step5 = "Step 5";
			public const string Step5Plus = "Step 5+";

			private static string[] descriptions = new string[]
				{
					"No Step",
					"Step 1",
					"Step 2",
					"Step 3",
					"Step 4",
					"Step 5",
					"Step 5+"
				};
			public static string[] AsArray { get { return descriptions; } }
		}

		private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "No Step" },
				{ 2, "Step 1" },
				{ 3, "Step 2" },
				{ 4, "Step 3" },
				{ 5, "Step 4" },
				{ 6, "Step 5" },
				{ 7, "Step 5+" }
			};
		public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
	}
}
