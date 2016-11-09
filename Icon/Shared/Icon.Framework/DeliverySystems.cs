
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// dbo.DeliverySystem auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class DeliverySystems
    {
        public const int Cap = 1;
        public const int Chw = 2;
        public const int Lz = 3;
        public const int Sg = 4;
        public const int Tb = 5;
        public const int Vc = 6;
        public const int Vs = 7;
		
		public class Descriptions
		{
			public const string Cap = "CAP";
			public const string Chw = "CHW";
			public const string Lz = "LZ";
			public const string Sg = "SG";
			public const string Tb = "TB";
			public const string Vc = "VC";
			public const string Vs = "VS";

			private static string[] descriptions = new string[]
				{
					"CAP",
					"CHW",
					"LZ",
					"SG",
					"TB",
					"VC",
					"VS"
				};
			public static string[] AsArray { get { return descriptions; } }
		}
		
		private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "CAP" },
				{ 2, "CHW" },
				{ 3, "LZ" },
				{ 4, "SG" },
				{ 5, "TB" },
				{ 6, "VC" },
				{ 7, "VS" }
			};
		public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
	}
}
