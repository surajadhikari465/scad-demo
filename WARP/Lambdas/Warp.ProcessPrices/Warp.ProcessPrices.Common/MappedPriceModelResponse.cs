namespace Warp.ProcessPrices.Common
{
    public class MappedPriceModelResponse
    {
        public bool IsValid { get; set; }
        public PriceModel PriceModel { get; set; }
        public string Message { get; set; }

    }
}