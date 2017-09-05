﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSupport.Models
{
    public static class ValidationConstants
    {
        public const string RegExForValidScanCode = @"^[1-9][0-9]{0,12}(\r?\n[1-9][0-9]{0,12})*(\r?\n)*$";


        public const string ErrorMsgForInvalidScanCode = "Invalid scan code format.";


		public const string PromptToSelectRegion = "- Please Select a Region";
	}
}