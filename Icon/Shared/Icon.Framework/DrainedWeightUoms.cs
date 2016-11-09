
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.Trait auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class DrainedWeightUoms
    {
        public const string Oz = "OZ";
        public const string Ml = "ML";
	
		private static string[] descriptions = new string[]
				{
					"OZ",
					"ML"
				};
		public static string[] AsArray { get { return descriptions; } }
	
		
		public static IEnumerable<string> Values
		{
			get
			{
				return new List<string>
				{
					{ "OZ" },
					{ "ML" }
				};
			}
		}
	}
}
