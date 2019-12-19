using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using OOS.Model;
using OOSCommon;


namespace OutOfStock.ScanManagement
{
    public class RawScanRepository : IRawScanRepository
    {
        private readonly IConfigure _config;
        
        public RawScanRepository(IConfigure config)
        {
            _config = config;
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
            using (var cn = new SqlConnection(_config.GetOOSConnectionString()))
            {
                cn.Open();
                foreach (var id in ids)
                {
                    SetScanAsFailed(cn,id);
                }
            }
        }

        public void SetScansAsComplete(int[] ids)
        {
            using (var cn = new SqlConnection(_config.GetOOSConnectionString()))
            {
                cn.Open();
                foreach (var id in ids)
                {
                    SetScanAsComplete(cn, id);
                }
            }
        }

        private void SetScanAsComplete(SqlConnection cn, int id)
        {
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MarkScanAsComplete";
                cmd.Parameters.AddWithValue("id", id);
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
}