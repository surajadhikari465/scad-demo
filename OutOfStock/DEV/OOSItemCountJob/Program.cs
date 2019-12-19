using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using OOSCommon.DataContext;
using Oracle.ManagedDataAccess.Client;
using OOSCommon;


namespace OOSItemCountJob
{
    class Program
    {
        static void Main(string[] args)
        {

            var sw = new Stopwatch();
            sw.Start();
            var regions = ConfigurationManager.AppSettings["Regions"].Split(',');
            var today = DateTime.Now;
            var daysBack = int.Parse(ConfigurationManager.AppSettings["DaysBack"]);
            DataTable allResults = null;
            allResults = GetItemCounts(regions, today, daysBack);
            SaveItemCounts(allResults, regions);
            sw.Stop();
            Console.WriteLine("ms: {0}",sw.ElapsedMilliseconds);            
        }

        private static void SaveItemCounts(DataTable allResults, string[] regions)
        {

            IList<Region> OOSRegions = null;

            using (var ctx = new OOSEntities())
            {

                OOSRegions =
                    (from r in ctx.REGION
                        where r.IS_VISIBLE == "true"
                        select new Region() {Id = r.ID, RegionAbbr = r.REGION_ABBR}).ToList();

                foreach (var region in regions)
                {
                    var _regionId = (from r in ctx.REGION where r.REGION_ABBR == region select r.ID).FirstOrDefault();
                    ResetItemCounts(_regionId);
                }

                foreach (DataRow row in allResults.Rows)
                {
                    var _region = row["region"].ToString();
                    var _regionid = (from r in OOSRegions where r.RegionAbbr == _region select r.Id).FirstOrDefault();
                    var _psbu = int.Parse(row["store"].ToString());
                    var _subteam = row["subteam_name"].ToString();
                    var _count = int.Parse(row["count"].ToString());

                    UpdateItemCounts(ctx, _regionid, _psbu, _subteam, _count);
                }


            }

        }
        private static void ResetItemCounts(int regionId)
        {
            using (var ctx = new OOSEntities())
            {
                ctx.ExecuteFunction("ResetItemCount",
                    new [] {new ObjectParameter("RegionId", regionId), new ObjectParameter("psbu", DBNull.Value)});
            }
            
        }
        private static void UpdateItemCounts(ObjectContext ctx, int regionId, int psbu, string subteam, int count)
        {
                ctx.ExecuteFunction("UpdateItemCount",
                    new [] { new ObjectParameter("RegionId", regionId), new ObjectParameter("PSBU", psbu), new ObjectParameter("SubTeam", subteam), new ObjectParameter("Count", count) });
           
        }
        //private static List<ValidSubteam> GetValidSubteams()
        //{
        //    using (var context = new OOSEntities())
        //    {
        //        return (from s in context.Subteams where s.DISABLED == false select new ValidSubteam() {Id=s.Id, Subteam=s.Subteam}).ToList();
        //    }
        //}
        private static DataTable GetItemCounts(IEnumerable<string> regions, DateTime today, int daysBack)
        {
            var allResults = new DataTable();
            var query = @"SELECT '{2}' region, store, subteam_name, count(distinct upc) as count
                          FROM    STELLA.MVMNT_DAILY m
                          inner join vim.store@vim_stella s on m.STORE = s.ps_bu
                          LEFT JOIN vim.crs_team_ref@vim_stella vcrs2 ON m.pos_dept = cast(vcrs2.pos_dept AS INT) AND m.store = vcrs2.ps_bu
                          WHERE   m.MVMNT_DATE BETWEEN to_date('{0}', 'MM/DD/YYYY') - {1} AND to_date('{0}', 'MM/DD/YYYY') and s.region = '{2}' and upper(s.POSTYPE) <> 'CLOSED' and substr(s.PS_BU,1,1) = '1'   and subteam_name is not null
                          group by store, subteam_name
                          order by store, subteam_name";

            Parallel.ForEach(regions, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, r =>
            {
                Console.WriteLine("Processing {0}", r);
                DataTable dt = new DataTable();
                var sql = string.Format(query, today.ToString("MM/dd/yyyy"), daysBack, r);
                var connection = new OracleConnection
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["Stella_DP"].ToString()
                };
                try
                {
                    connection.Open();

                    using (var cmd = new OracleCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600;

                        using (var da = new OracleDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }

                    allResults.Merge(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                    connection.Dispose();
                }
            });

            return allResults;
        }
        
    }
}
