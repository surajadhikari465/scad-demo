
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// vim.StorePosType auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class StorePosTypes
    {
        public const int Xnew = 1;
        public const int Ibm = 2;
        public const int Rtx = 3;
        public const int Acs = 4;
        public const int Bean = 5;
        public const int Ccp = 6;
        public const int Closed = 7;
		
		private static Dictionary<int, string> idToDescriptionsDictionary = new Dictionary<int, string>
			{
				{ 1, "xNEW" },
				{ 2, "IBM" },
				{ 3, "RTX" },
				{ 4, "ACS" },
				{ 5, "BEAN" },
				{ 6, "CCP" },
				{ 7, "CLOSED" }
			};
		public static Dictionary<int, string> AsDictionary { get { return idToDescriptionsDictionary; } }
	}
}
