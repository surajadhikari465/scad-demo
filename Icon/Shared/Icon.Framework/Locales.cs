
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.Locale auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class Locales
    {
        public const int WholeFoods = 1;
        public const int ThreeSixtyFive = 1100;
		
		public static class Names
		{
			public const string WholeFoods = "Whole Foods";
			public const string ThreeSixtyFive = "365";
		
			private static List<string> names = new List<string>
			{
				"Whole Foods",
				"365"
			};
			public static List<string> List { get { return names; } }
		}
	}
}
