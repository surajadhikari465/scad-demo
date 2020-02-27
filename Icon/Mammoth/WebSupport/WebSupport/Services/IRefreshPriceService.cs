using System.Collections.Generic;

namespace WebSupport.Services
{
    public interface IRefreshPriceService
    {
        RefreshPriceResponse RefreshPrices(string region, List<string> systems, List<string> stores, List<string> scanCodes);
    }
}