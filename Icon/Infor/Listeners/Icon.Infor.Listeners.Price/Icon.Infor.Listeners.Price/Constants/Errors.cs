using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.Constants
{
    public static class Errors
    {
        public static class Codes
        {
            public static readonly string AddPricesError = "AddPriceError";
            public static readonly string ArchivePricesError = "ArchivePricesError";
            public static readonly string DeletePricesError = "DeletePricesError";
            public static readonly string ReplacePricesDeleteError = "ReplacePricesDeleteError";
            public static readonly string ReplacePricesAddError = "ReplacePricesAddError";
            public static readonly string SendFailedPricesToEsbError = "SendFailedPricesToEsbError";
            public static readonly string SendPricesToEsbError = "SendPricesToEsbError";
            public static readonly string UnexpectedError = "UnexpectedError";
        }

        public static class Details
        {
            public static readonly string AddPriceAlreadyExists = "Price could not be added because it already exists.";
            public static readonly string DeletePriceDoesNotExist = "Price cannot be deleted because it does not exist.";
            public static readonly string ReplacePriceDoesNotExist = "Price to be replaced cannot be deleted because it does not exist.";
            public static readonly string ReplacePriceAddAlreadyExists = "Price to be added could not be because it already exists.";
        }
    }
}
