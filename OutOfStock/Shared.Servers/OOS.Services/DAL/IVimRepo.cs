using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using OOS.Services.DataModels;
using Oracle.ManagedDataAccess.Client;

namespace OOS.Services.DAL
{
    public interface IVimRepo
    {
        List<VimStore> GetVimStores();
    }

    public class VimRepo : IVimRepo
    {
        public List<VimStore> GetVimStores()
        {
            DataTable dt = new DataTable();

            var query = @"
SELECT
PS_BU 		   ,
REG_STORE_NUM  ,
REGION         ,
STORE_NAME     ,
STORE_ABBR     ,
POSTYPE        ,
ADDR1          ,
ADDR2          ,
CITY           ,
STATE_PROVINCE ,
POSTAL_CODE    ,
COUNTRY        ,
PHONE          ,
FAX            ,
SERVERNAME     ,
LASTUSER       ,
TIMESTAMP     FROM vim.store where substr(ps_bu,0,1) = '1' 
order by ps_bu
";

            var connex = ConfigurationManager.ConnectionStrings["vimConnectionString"].ConnectionString;
            using (var connection = new OracleConnection(connex))
            {
                connection.Open();
                using (var cmd = new OracleCommand(query, connection))
                {
                    using (var da = new OracleDataAdapter(cmd))
                    {

                        da.Fill(dt);
                    }
                }
                connection.Close();
            }

            var data = from store in dt.AsEnumerable()
                let status = SetStatus(store.Field<string>("POSTYPE"))
                select new VimStore
                {
                    PS_BU = store.Field<string>("PS_BU"),
                    REG_STORE_NUM = store.Field<string>("REG_STORE_NUM"),
                    REGION = store.Field<string>("REGION"),
                    STORE_NAME = store.Field<string>("STORE_NAME"),
                    STORE_ABBR = store.Field<string>("STORE_ABBR"),
                    POSTYPE = store.Field<string>("POSTYPE"),
                    ADDR1 = store.Field<string>("ADDR1"),
                    ADDR2 = store.Field<string>("ADDR2"),
                    CITY = store.Field<string>("CITY"),
                    STATE_PROVINCE = store.Field<string>("STATE_PROVINCE"),
                    POSTAL_CODE = store.Field<string>("POSTAL_CODE"),
                    COUNTRY = store.Field<string>("COUNTRY"),
                    PHONE = store.Field<string>("PHONE"),
                    FAX = store.Field<string>("FAX"),
                    SERVERNAME = store.Field<string>("SERVERNAME"),
                    LASTUSER = store.Field<string>("LASTUSER"),
                    TIMESTAMP = store.Field<DateTime>("TIMESTAMP"),
                    STATUS = status

                };
            return data.ToList();
        }

        private string SetStatus(string postype)
        {
            if (string.IsNullOrEmpty(postype) || string.IsNullOrWhiteSpace(postype))
                return "CLOSED";
            if (postype.ToLower().Equals("closed") )
                return "CLOSED";
            if (postype.ToLower().Equals("new"))
                return "NEW";
            return "OPEN";
        }
    }
}