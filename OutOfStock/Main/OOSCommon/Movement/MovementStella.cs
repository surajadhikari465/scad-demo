using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Movement
{

    public class MovementStella : IMovement
    {

        private string stellaServiceName;
        private string vimConnectionString;
        private IOOSLog logger;

        public string upc { get; set; }
        public decimal? MVMNT_UNITS { get; set; }    // OOS.Report_Detail.movement
        //public decimal MVMNT_DOLLARS {get;set;}   // Not used in OOS Db
        //public DateTime MVMNT_DATE {get;set;}     // Not used in OOS Db


        public MovementStella()
        {}

        public MovementStella(string stellaServiceName, string vimConnectionString, IOOSLog logger)
        {
            this.stellaServiceName = stellaServiceName;
            this.vimConnectionString = vimConnectionString;
            this.logger = logger;
        }

        /// <summary>
        /// Get movement data for UPC at store for the date range
        /// </summary>
        /// <param name="upc">UPC with left "0" pad to 12 (or more) and no check digit</param>
        /// <param name="storePS_BU">The store's ps_bu number (PeopleSoft business unit)</param>
        /// <param name="oosDate">Date of thee OOS reading</param>
        /// <returns>Movement data for the item</returns>
        public IMovement GetMovementData(string upc, string storePS_BU, DateTime oosDate)
        {
            var upcs = new List<string> {upc};
            var movementUnit = GetMovementUnits(upcs, storePS_BU, oosDate).FirstOrDefault();
            return new MovementStella {upc = movementUnit.Key, MVMNT_UNITS = movementUnit.Value};
        }

        public const int AverageUnitOpportunityPeriod = 84;

        public Dictionary<string, decimal?> GetMovementUnits(List<string> upcs, string storePS_BU, DateTime oosDate)
        {
            var translatedUpcs = MovementStellaUpcSpecification.TranslateUpc(upcs);
            string oracleQueryTemplate =
                "SELECT upc, MVMNT_UNITS, MVMNT_DOLLARS, MVMNT_DATE " +
                "FROM stella.MVMNT_DAILY " +
                "WHERE STORE='" + storePS_BU + "' " +
                "AND upc in ({0}) " +
                "AND MVMNT_DATE BETWEEN to_date('" + oosDate.ToString("MM/dd/yyyy") + "', 'MM/DD/YYYY') - " + AverageUnitOpportunityPeriod + " " +
                "AND to_date('" + oosDate.ToString("MM/dd/yyyy") + "', 'MM/DD/YYYY')";

            var movementData = QueryMovementData(oracleQueryTemplate, translatedUpcs);
            #if (TRACE_DEFINED)
                TraceMovementData(movementData); 
            #endif
            var transformed = Transform(movementData);
            #if (TRACE_DEFINED)
                TraceTransformedMovementData(transformed);
            #endif
            var inverted = InvertStellaUpc(transformed);
            return inverted;
        }

        private IEnumerable<MovementStella> QueryMovementData(string sql, IEnumerable<string> upcs)
        {
            var segments = new StellaUpcSet(6000).ToPureSqlStrings(upcs);
            return segments.Select(segment => MovementSelectLinkedSQL(sql, segment)).Select(Execute).SelectMany(p => p).ToList();
        }

        private string MovementSelectLinkedSQL(string sql, string upcSet)
        {
            var oracleQuery = string.Format(sql, upcSet).Replace("'", "''");
            return "SELECT upc, MVMNT_UNITS " + "FROM OPENQUERY(" + stellaServiceName + ", '" + oracleQuery + "')";
        }

        private IEnumerable<MovementStella> Execute(string sqlQuery)
        {
            using (var oosDataContext = new System.Data.Linq.DataContext(vimConnectionString))
            {
                var items = oosDataContext.ExecuteQuery<MovementStella>(sqlQuery, new object[] { });
                return items.ToList();
            }
        }

        private void TraceMovementData(IEnumerable<MovementStella> movementData)
        {
            foreach (var movementStella in movementData)
            {
                logger.Trace(string.Format("UPC='{0}' Movement='{1}'", movementStella.upc, movementStella.MVMNT_UNITS));
            }
        }

        private Dictionary<string, decimal?> Transform(IEnumerable<MovementStella> movementStellas)
        {
            var transform = (from c in movementStellas group c by c.upc into g
                             select new { UPC = g.Key, MVMNT_UNITS = g.Average(p => p.MVMNT_UNITS) }).ToDictionary(s => s.UPC, r => r.MVMNT_UNITS);
            return transform;
        }

        private void TraceTransformedMovementData(Dictionary<string, decimal?> transformed)
        {
            foreach (var @decimal in transformed)
            {
                logger.Trace(string.Format("UPC='{0}' Movement Units='{1}'", @decimal.Key, @decimal.Value));
            }
        }

        private Dictionary<string, decimal?> InvertStellaUpc(Dictionary<string, decimal?> movementData)
        {
            return movementData.ToDictionary(s => MovementStellaUpcSpecification.InvertUpc(new List<string>{s.Key}).FirstOrDefault(), r => r.Value);
        }


        public decimal? GetMovementUnits(string upc, string storePS_BU, DateTime oosDate)
        {
            var movement = (MovementStella)GetMovementData(upc, storePS_BU, oosDate);
            return movement.MVMNT_UNITS;
        }


    }
}
