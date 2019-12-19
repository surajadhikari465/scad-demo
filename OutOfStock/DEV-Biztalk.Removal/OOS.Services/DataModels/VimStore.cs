using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace OOS.Services.DataModels
{
   public class VimStore
    {
       
        

        public string PS_BU { get; set; }
        public string REG_STORE_NUM { get; set; }
        public string REGION { get; set; }
        public string STORE_NAME { get; set; }
        public string STORE_ABBR { get; set; }
        public string POSTYPE { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string CITY { get; set; }
        public string STATE_PROVINCE { get; set; }
        public string POSTAL_CODE { get; set; }
        public string COUNTRY { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public string SERVERNAME { get; set; }
        public string LASTUSER { get; set; }
        public DateTime TIMESTAMP { get; set; }
        public string STATUS { get; set; }
    }
}
