using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Movement
{
    public class MovementRepository : IMovementRepository
    {
        // Sample usage
        //  OOSCommon.Movement.IMovement movement = null;
        //  string oosMovementServiceName = "STELLA_DEV";
        //      //OOSCommon.AppConfig.AppSettings[oosMovementServiceNameAppSetting];
        //  string oosConnectionStringName = "OOSConnectionString";
        //  string oosConnectionString = "Data Source=OOSDbTest;Initial Catalog=OOS;Integrated Security=True"; 
        //      // OOSCommon.AppConfig.ConnectionStrings[oosConnectionStringName].ConnectionString;
        //  movement = new OOSCommon.Movement.MovementStella(oosConnectionString,
        //      oosMovementServiceName, OutOfStock.MvcApplication.oosLog);

        public IOOSLog oosLog { get; set; }
        public static string oosStellaServiceName { get; set; }
        public static string oosConnectionString { get; set; }

        public MovementRepository(string oosConnectionString, string oosMovementServiceName, IOOSLog oosLog)
        {
            MovementRepository.oosConnectionString = oosConnectionString;
            MovementRepository.oosStellaServiceName = oosMovementServiceName;
            this.oosLog = oosLog;
        }

        public IMovement GetMovementData(string upc, string storePS_BU, DateTime oosDate)
        {
            var movementStella = new MovementStella(oosStellaServiceName, oosConnectionString, oosLog);
            IMovement result = movementStella.GetMovementData(upc, storePS_BU, oosDate);
            return result;
        }
        

        public List<StellaMovementDataSupplimental> GetMovementDataSupplimental(List<string> upcs, string storePS_BU, DateTime oosDate)
        {
            var movementStella = new MovementStella(oosStellaServiceName, oosConnectionString, oosLog);
            var result = movementStella.GetLastMovementDates(upcs, storePS_BU, oosDate);
            return result;
        }

        public List<StellaMovementDataActualSales> GetActualSales(List<string> upcs, List<string> PS_BUs, DateTime begindate, DateTime enddate)
        {
            var movementStella = new MovementStella(oosStellaServiceName, oosConnectionString, oosLog);
            var result = movementStella.GetActualSales(upcs,  PS_BUs, begindate, enddate);
            return result;
        }

        public Dictionary<string, decimal?> GetMovementUnits(List<string> upcs, string storePS_BU, DateTime oosDate)
        {
            var movementStella = new MovementStella(oosStellaServiceName, oosConnectionString, oosLog);
            var result = movementStella.GetMovementUnits(upcs, storePS_BU, oosDate);
            return result;
        }

        public decimal? GetMovementUnits(string upc, string storePS_BU, DateTime oosDate)
        {
            var movementStella = new MovementStella(oosStellaServiceName, oosConnectionString, oosLog);
            decimal? result = movementStella.GetMovementUnits(upc, storePS_BU, oosDate);
            return result;
        }

    }
}
