
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Icon.Framework
{
    /// <summary>
    /// Auto-generated validation class for the SelfCheckoutItemTareGroup Trait (ITG)
	/// (reads regular expression patterns from the dbo.Trait table)
    /// </summary>
    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class SelfCheckoutItemTareGroupTraitValidation
    {
        public const string ValidRegExPattern1 = @"[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Z}].{0,60}$";
	}
}