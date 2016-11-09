
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.MilkType auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class MilkTypes
    {
        public const int BuffaloMilk = 1;
        public const int CowMilk = 2;
        public const int GoatSheepMilk = 3;
        public const int GoatMilk = 4;
        public const int CowSheepMilk = 5;
        public const int CowGoatMilk = 6;
        public const int CowGoatSheepMilk = 7;
        public const int SheepMilk = 8;
        public const int YakMilk = 9;

		private static Dictionary<string, int> descriptionToIdDictionary = new Dictionary<string, int>
			{
				{ "Buffalo Milk", 1 },
				{ "Cow Milk", 2 },
				{ "Goat/Sheep Milk", 3 },
				{ "Goat Milk", 4 },
				{ "Cow/Sheep Milk", 5 },
				{ "Cow/Goat Milk", 6 },
				{ "Cow/Goat/Sheep Milk", 7 },
				{ "Sheep Milk", 8 },
				{ "Yak Milk", 9 }
			};
		public static Dictionary<string, int> Ids { get { return descriptionToIdDictionary; } }

		public class Descriptions
		{
			public const string BuffaloMilk = "Buffalo Milk";
			public const string CowMilk = "Cow Milk";
			public const string GoatSheepMilk = "Goat/Sheep Milk";
			public const string GoatMilk = "Goat Milk";
			public const string CowSheepMilk = "Cow/Sheep Milk";
			public const string CowGoatMilk = "Cow/Goat Milk";
			public const string CowGoatSheepMilk = "Cow/Goat/Sheep Milk";
			public const string SheepMilk = "Sheep Milk";
			public const string YakMilk = "Yak Milk";

			private static string[] descriptions = new string[]
				{
					"Buffalo Milk",
					"Cow Milk",
					"Goat/Sheep Milk",
					"Goat Milk",
					"Cow/Sheep Milk",
					"Cow/Goat Milk",
					"Cow/Goat/Sheep Milk",
					"Sheep Milk",
					"Yak Milk"
				};
			public static string[] AsArray { get { return descriptions; } }
		}
		
		private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "Buffalo Milk" },
				{ 2, "Cow Milk" },
				{ 3, "Goat/Sheep Milk" },
				{ 4, "Goat Milk" },
				{ 5, "Cow/Sheep Milk" },
				{ 6, "Cow/Goat Milk" },
				{ 7, "Cow/Goat/Sheep Milk" },
				{ 8, "Sheep Milk" },
				{ 9, "Yak Milk" }
			};
		public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
	}
}
