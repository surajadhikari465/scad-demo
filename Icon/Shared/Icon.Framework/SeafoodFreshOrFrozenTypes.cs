
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.SeafoodFreshOrFrozen auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class SeafoodFreshOrFrozenTypes
    {
        public const int Fresh = 1;
        public const int PreviouslyFrozen = 2;
        public const int Frozen = 3;

		private static Dictionary<string, int> descriptionToIdDictionary = new Dictionary<string, int>
			{
				{ "Fresh", 1 },
				{ "Previously Frozen", 2 },
				{ "Frozen", 3 }
			};
		public static Dictionary<string, int> Ids { get { return descriptionToIdDictionary; } }
		
		public class Descriptions
		{
			public const string Fresh = "Fresh";
			public const string PreviouslyFrozen = "Previously Frozen";
			public const string Frozen = "Frozen";

			private static string[] descriptions = new string[]
				{
					"Fresh",
					"Previously Frozen",
					"Frozen"
				};
			public static string[] AsArray { get { return descriptions; } }
		}
		
		private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "Fresh" },
				{ 2, "Previously Frozen" },
				{ 3, "Frozen" }
			};
		public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
	}
}
