
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
        public const int Vsg = 7;
        public const int Stck = 8;
        public const int Spry = 9;
        public const int Pst = 10;
        public const int Crys = 11;
        public const int Wps = 12;
        public const int Pckt = 13;
        public const int Shot = 14;
		
		public class Descriptions
		{
			public const string Cap = "CAP";
			public const string Chw = "CHW";
			public const string Lz = "LZ";
			public const string Sg = "SG";
			public const string Tb = "TB";
			public const string Vc = "VC";
			public const string Vsg = "VSG";
			public const string Stck = "STCK";
			public const string Spry = "SPRY";
			public const string Pst = "PST";
			public const string Crys = "CRYS";
			public const string Wps = "WPS";
			public const string Pckt = "PCKT";
			public const string Shot = "SHOT";

			private static string[] descriptions = new string[]
				{
					"CAP",
					"CHW",
					"LZ",
					"SG",
					"TB",
					"VC",
					"VSG",
					"STCK",
					"SPRY",
					"PST",
					"CRYS",
					"WPS",
					"PCKT",
					"SHOT"
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
				{ 7, "VSG" },
				{ 8, "STCK" },
				{ 9, "SPRY" },
				{ 10, "PST" },
				{ 11, "CRYS" },
				{ 12, "WPS" },
				{ 13, "PCKT" },
				{ 14, "SHOT" }
			};
		public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
	}
}
