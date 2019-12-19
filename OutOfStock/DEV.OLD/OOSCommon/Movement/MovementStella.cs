using System;
using System.Collections.Generic;
using System.Linq;

namespace OOSCommon.Movement
{

    public class StellaMovementDataSupplimental
    {
        public string UPC;
        public string Store;
        public DateTime? LastDateOfSales;
        public string DaysOfMovement;

        public StellaMovementDataSupplimental() {}
        public StellaMovementDataSupplimental(string upc, string store, DateTime? lastdateofsales, string daysofmovement)
        {
            this.UPC = upc;
            this.Store = store;
            this.LastDateOfSales = lastdateofsales;
            this.DaysOfMovement = daysofmovement;
        }
        
    }

    public class StellaMovementDataActualSales
    {
        public string UPC;
        public string Store;
        public decimal? ActualSales; 
                                   

        public StellaMovementDataActualSales() { }
        public StellaMovementDataActualSales(string upc, string store, decimal? actualsales)
        {
            this.UPC = upc;
            this.Store = store;
            this.ActualSales = actualsales;

        }

    }


    public class MovementStella : IMovement
    {

        private string stellaServiceName;
        private string vimConnectionString;
        private IOOSLog logger;

        public string upc { get; set; }
        public decimal? MVMNT_UNITS { get; set; }    // OOS.Report_Detail.movement
        public const int AverageUnitOpportunityPeriod = 12 * 7;  // 12 weeks (84 days)


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


        public List<StellaMovementDataSupplimental> GetLastMovementDates(List<string> upcs, string storePS_BU, DateTime oosDate)
        {
            //note: COUNT(MVMNT_DATE) is returned as a string by oracle. 
            //      The corresponding class property has been set to match.
            var translatedUpcs = MovementStellaUpcSpecification.TranslateUpc(upcs);
                var movementDataTemplate = string.Format(@"SELECT 
                            LPAD(upc,13,'0') as UPC,
                            STORE as Store,
                            MAX(MVMNT_DATE) as LastDateOfSales,
                            COUNT(MVMNT_DATE) DaysOfMovement
                    FROM    STELLA.MVMNT_DAILY 
                    WHERE   STORE='{0}' AND 
                            UPC in ({1}) AND
                            MVMNT_DATE BETWEEN to_date('{2}', 'MM/DD/YYYY') - {3} AND to_date('{2}', 'MM/DD/YYYY')
                            GROUP BY UPC, STORE",
                                storePS_BU,
                                "{0}", // leave a template here because it is used later in MovementSelectLinedSQL
                                oosDate.ToString("MM/dd/yyyy"),
                                AverageUnitOpportunityPeriod);

            var UPCSegments = new StellaUpcSet(6000).ToPureSqlStrings(translatedUpcs);
            var results = UPCSegments.Select(segment =>
                {
                    return string.Format("select * from openquery({0},'{1}')", stellaServiceName,
                                         string.Format(movementDataTemplate, segment).Replace("'", "''"));
                }).Select(ExecuteForOtherStellaData).SelectMany(p => p).ToList();
                    

            return results;
        }

        public List<StellaMovementDataActualSales> GetActualSales(List<String> upcs, List<string> PS_BUs, DateTime begindate,
                                                           DateTime enddate)
        {
            var translatedUpcs = MovementStellaUpcSpecification.TranslateUpc(upcs);
            var actualSalesTemplate = string.Format(@" select
                                                        upc,
                                                        store as Store, 
                                                        mvmnt_dollars as ActualSales
                                             from       STELLA.MVMNT_DAILY 
                                             where      mvmnt_date between to_date('{2}', 'MM/DD/YYYY') and to_date('{3}', 'MM/DD/YYYY') and
                                                        store in ('{0}') and
                                                        upc in ({1}) ",
                                                                            "{0}",
                                                                            "{1}",
                                                                            //string.Join(",", PS_BUs.Select(p=> "'"+p+"'").ToArray()),
                                                                            begindate.ToString("MM/dd/yyyy"),
                                                                            enddate.ToString("MM/dd/yyyy"));
            
            var UPCSegments = new StellaUpcSet(500).ToPureSqlStrings(translatedUpcs);
            var query = new List<string>();
            
            foreach (var segment in UPCSegments)
            {
                query.AddRange(PS_BUs.Select(psBU => string.Format("select * FROM OPENQUERY({0},'{1}')", stellaServiceName, string.Format(actualSalesTemplate, psBU, segment).Replace("'","''"))));
            }
            
              
                
              var x = query.Select(ExecuteForActualSales).AsParallel().SelectMany(p => p).ToList();


            return x;

        }

        public Dictionary<string, decimal?> GetMovementUnits(List<string> upcs, string storePS_BU, DateTime oosDate)
        {
            var translatedUpcs = MovementStellaUpcSpecification.TranslateUpc(upcs);

            //string oracleQueryTemplatex =
            //    "SELECT upc, MVMNT_UNITS, MVMNT_DOLLARS, MVMNT_DATE " +
            //    "FROM stella.MVMNT_DAILY " +
            //    "WHERE STORE='" + storePS_BU + "' " +
            //    "AND upc in ({0}) " +
            //    "AND MVMNT_DATE BETWEEN to_date('" + oosDate.ToString("MM/dd/yyyy") + "', 'MM/DD/YYYY') - " + AverageUnitOpportunityPeriod + " " +
            //    "AND to_date('" + oosDate.ToString("MM/dd/yyyy") + "', 'MM/DD/YYYY')";
            var movementDataTemplate = string.Format(@"SELECT 
                        UPC, 
                        MVMNT_UNITS, 
                        MVMNT_DOLLARS, 
                        MVMNT_DATE 
                FROM    STELLA.MVMNT_DAILY 
                WHERE   STORE='{0}' AND 
                        UPC in ({1}) AND
                        MVMNT_DATE BETWEEN to_date('{2}', 'MM/DD/YYYY') - {3} AND to_date('{2}', 'MM/DD/YYYY')",
                            storePS_BU,
                            "{0}", // leave a template here because it is used later in MovementSelectLinedSQL
                            oosDate.ToString("MM/dd/yyyy"),
                            AverageUnitOpportunityPeriod);


            var movementData = QueryMovementData(movementDataTemplate, translatedUpcs);
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
            return segments.Select(segment => MovementSelectLinkedSQL(sql, segment)).Select(ExecuteForMovement).SelectMany(p => p).ToList();
        }

        private string MovementSelectLinkedSQL(string sql, string upcSet)
        {
            var oracleQuery = string.Format(sql, upcSet).Replace("'", "''");
            return "SELECT UPC, MVMNT_UNITS " + "FROM OPENQUERY(" + stellaServiceName + ", '" + oracleQuery + "')";
        }

        private IEnumerable<MovementStella> ExecuteForMovement(string sqlQuery)
        {
            using (var oosDataContext = new System.Data.Linq.DataContext(vimConnectionString))
            {
                var items = oosDataContext.ExecuteQuery<MovementStella>(sqlQuery, new object[] { });
                return items.ToList();
            }
        }

        private IEnumerable<StellaMovementDataActualSales> ExecuteForActualSales(string sqlQuery)
        {

            using (var oosDataContext = new System.Data.Linq.DataContext(vimConnectionString))
            {
                oosDataContext.CommandTimeout = 120;
                var items = oosDataContext.ExecuteQuery<StellaMovementDataActualSales>(sqlQuery, new object[] { });
                return items.ToList();
            }
        }

        private IEnumerable<StellaMovementDataSupplimental> ExecuteForOtherStellaData(string sqlQuery)
        {
            using (var oosDataContext = new System.Data.Linq.DataContext(vimConnectionString))
            {
                var items = oosDataContext.ExecuteQuery<StellaMovementDataSupplimental>(sqlQuery, new object[] { });
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
