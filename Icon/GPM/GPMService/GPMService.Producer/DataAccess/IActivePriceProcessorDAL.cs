using GPMService.Producer.Model.DBModel;
using Mammoth.Framework;
using System.Collections.Generic;

namespace GPMService.Producer.DataAccess
{
    internal interface IActivePriceProcessorDAL
    {
        void UpdateStatusToRunning(int jobScheduleId);
        void UpdateStatusToReady(int jobScheduleId);
        IEnumerable<GetActivePricesQueryModel> GetActivePrices(MammothContext mammothContext, string region);
    }
}
