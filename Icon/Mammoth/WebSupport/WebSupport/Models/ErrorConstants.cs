using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSupport.Models
{
    public static class ErrorConstants
    {
        public static class Codes
        {
            public const string NoPricesExist = "NoPricesExist";
        }

        public static class Details
        {
            public const string NoPricesExist = "Unable to send prices because no prices were found for the given scan codes and business units.";
        }
    }
}