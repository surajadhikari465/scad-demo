using GPMService.Producer.Model.DBModel;
using System.Collections.Generic;

namespace GPMService.Producer.DataAccess
{
    internal interface IEmergencyPriceProcessorDAL
    {
        bool EmergencyPricesExist();
        List<GetEmergencyPricesQueryModel> GetEmergencyPrices();
        void InsertPricesIntoEmergencyQueue(MammothPricesType emergencyMammothPrices);
    }
}
