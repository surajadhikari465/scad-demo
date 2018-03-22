namespace Mammoth.Common
{
    public class Constants
    {
        public const int TimeoutErrorNumber = -2;

        public static class EventTypes
        {
            public const int ItemLocaleAddOrUpdate = 1;
            public const int ItemDelete = 2;
            public const int Price = 3;
            public const int PriceRollback = 4;
            public const int CancelAllSales = 5;
        }

        public static class SourceSystem
        {
            public const string MammothWebApi = "Mammoth Web Api";
            public const string MammothPriceController = "Mammoth Price Controller";
            public const string MammothItemLocaleController = "Mammoth ItemLocale Controller";
        }

        public static class ExceptionProperties
        {
            public const string ExceptionType = "ExceptionType";
            public const string ExceptionMessage = "ExceptionMessage";
        }
    }
}
