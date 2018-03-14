
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// Auto-generated validation class for the FairTradeCertified Trait (FTC)
	/// (reads regular expression patterns from the dbo.Trait table)
    /// </summary>
    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class FairTradeCertifiedTraitValidation
    {
        public const string ValidRegExPattern1 = @"[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Z}].{0,255}$";
		private static string[] traitValidationPatterns = new string[]
		{
		      ValidRegExPattern1,
		};
		public static string[] AsArray { get { return traitValidationPatterns; } }

		public static IList<string> Values
		{
			get
			{
			    return new List<string>( traitValidationPatterns );
			}
		}
	}
}