using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Movement
{
    public interface IMovementRepository
    {
        IMovement GetMovementData(string upc, string storePS_BU, DateTime oosDate);

        List<StellaMovementDataSupplimental> GetMovementDataSupplimental(List<string> upcs, string storePS_BU,
                                                                         DateTime oosDate);


        List<StellaMovementDataActualSales> GetActualSales(List<string> upcs, List<string> PS_BUs, DateTime begindate,
                                                           DateTime enddate);

        Dictionary<string, decimal?> GetMovementUnits(List<string> upcs, string storePS_BU, DateTime oosDate);
        decimal? GetMovementUnits(string upc, string storePS_BU, DateTime oosDate);
    }
}
