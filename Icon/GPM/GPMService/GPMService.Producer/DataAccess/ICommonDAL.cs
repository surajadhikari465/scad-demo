using System.Collections.Generic;

namespace GPMService.Producer.DataAccess
{
    internal interface ICommonDAL
    {
        void UpdateStatusToRunning(int jobScheduleID);
        void UpdateStatusToReady(int jobScheduleID);
        IEnumerable<MammothPriceType> ArchiveActivePrice(MammothPricesType mammothPrices);
    }
}
