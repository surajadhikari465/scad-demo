using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSupport.Services
{
    public interface IRefreshPriceService
    {
        RefreshPriceResponse RefreshPrices(string region, List<string> systems, List<string> stores, List<string> scanCodes);
    }
}