using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Movement
{
    public interface IMovementRepository
    {
        IMovement GetMovementData(string upc, string storePS_BU, DateTime oosDate);
        Dictionary<string, decimal?> GetMovementUnits(List<string> upcs, string storePS_BU, DateTime oosDate);
        decimal? GetMovementUnits(string upc, string storePS_BU, DateTime oosDate);
    }
}
