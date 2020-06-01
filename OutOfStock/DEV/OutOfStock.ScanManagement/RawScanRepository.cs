using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using OOS.Model;
using OOSCommon;
using OOSCommon.DataContext;


namespace OutOfStock.ScanManagement
{
    public class RawScanRepository : IRawScanRepository
    {
        private readonly IConfigure _config;
        private readonly IScanOutOfStockItemService _scanItemService;
        private IOOSLog _logger;
        private IOOSEntitiesFactory _ioosEntitiesFactory;


        public RawScanRepository(IConfigure config, IScanOutOfStockItemService scanItemService, ILogService logService , IOOSEntitiesFactory oosEntitiesFactory)
        {
            _config = config;
            _scanItemService = scanItemService;
            _logger = logService.GetLogger();
            _ioosEntitiesFactory = oosEntitiesFactory;
        }
        

        public void SaveRawScan(string message)
        {
            using (var cn = new SqlConnection(_config.GetOOSConnectionString()))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SaveRawScan";
                    cmd.Parameters.AddWithValue("data", message);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<RawScanData> GetNextScans(int count)
        {
            var data = new DataTable();
            List<RawScanData> results;
            using (var cn = new SqlConnection(_config.GetOOSConnectionString()))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetNextScans";
                    cmd.Parameters.AddWithValue("count", count);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(data);
                    }

                    results = (from r in data.AsEnumerable()
                        select new RawScanData()
                        {
                            Id = r.Field<int>("id"),
                            Message = r.Field<string>("Message"),
                            CreatedOn= r.Field<DateTime>("CreatedOn")
                        }).ToList();
                }
            }

            return results;
        }

        public void SetScansAsFailed(int[] ids)
        {
            _logger.Warn($"ScansFailed: {string.Join(",",ids)} ");

            using (var cn = new SqlConnection(_config.GetOOSConnectionString()))
            {
                cn.Open();
                foreach (var id in ids)
                {
                    SetScanAsFailed(cn,id);
                }
            }
        }

        public void SetScanAsComplete(int id, long elapsedMs)
        {
            using (var cn = new SqlConnection(_config.GetOOSConnectionString()))
            {
                cn.Open();

                SetScanAsComplete(cn, id, elapsedMs);
                
            }
        }

        public long ProcessRawScan(ScanData scan)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                _scanItemService.ScanProductsByStoreAbbreviation(scan.RegionAbbrev, scan.StoreAbbrev, scan.Upcs, scan.ScanDate);
            }
            finally
            {
                sw.Stop();
            }

            return sw.ElapsedMilliseconds;

        }

        public string[] RegionNames()
        {
            return _scanItemService.RegionNames();
        }

        public string[] StoreNamesFor(string regionName)
        {
            return _scanItemService.StoreNamesFor(regionName);
        }

        public string[] RegionAbbreviations()
        {
            return _scanItemService.RegionAbbreviations();
        }

        public string[] StoreAbbreviationsFor(string regionAbbrev)
        {
            return _scanItemService.StoreAbbreviationsFor(regionAbbrev);
        }

        public string GetConfiguration(string region, string store, string sessionId)
        {
            
            string retval = string.Empty;
            using (var db = _ioosEntitiesFactory.New())
            {
                var regionConfig = (from r in db.REGION
                    join rc in db.RegionalAppConfiguration on r.ID equals rc.RegionId
                    where r.REGION_ABBR == region
                    select rc).FirstOrDefault();

                retval = regionConfig != null ? $"{regionConfig.UseBiztalk ?? true}|{regionConfig.BiztalkServiceURI}|{regionConfig.OOSServiceURI}" : "no configuration found";
            }


            _logger.Debug($"[mobile:config] {sessionId} {region}/{store} {retval}");
            return retval;
        }

        public void Login(string username, string useremail, string region ,string store, string sessionId, string ipAddress)
        {
            _logger.Debug($"[mobile:login] {sessionId} {ipAddress} {region}/{store} {username} [{useremail}]");

        }

        public bool ValidateRegionStore(string region, string store, string sessionId)
        {
            bool result;
            using (var db = _ioosEntitiesFactory.New())
            {
                var validatedStore = (from s in db.STORE
                    join r in db.REGION on s.REGION_ID equals r.ID
                    where s.STORE_ABBREVIATION == store && r.REGION_ABBR == region
                    select s).FirstOrDefault();

                result = validatedStore != null;

            }
            _logger.Debug($"[mobile:validateRegionStore] {sessionId} {region}/{store} [{result}]");

            return result;
        }

        private void SetScanAsComplete(SqlConnection cn, int id, long elapsedMs)
        {
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MarkScanAsComplete";
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("elapsedMs", elapsedMs);
                cmd.ExecuteNonQuery();
            }
        }
        private void SetScanAsFailed(SqlConnection cn, int id)
        {
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MarkScanAsFailed";
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class RawScanData
    {
        public  int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class ScanData
    {
        public DateTime ScanDate { get; set; }
        public string RegionAbbrev { get; set; }
        public string StoreAbbrev { get; set; }
        public string[] Upcs { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string SessionId { get; set; }
    }

}