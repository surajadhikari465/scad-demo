using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOS.Services.DataModels
{
    public class StoreDb
    {

// ReSharper disable InconsistentNaming
        public int ID { get; set; }
        public string PS_BU { get; set; }
        public string STORE_ABBREVIATION { get; set; }
        public string STORE_NAME { get; set; }
        public int REGION_ID { get; set; }
        public int STATUS_ID { get; set; }
        public DateTime LAST_UPDATED_DATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }

// ReSharper restore InconsistentNaming
    }


    public class StoreStatus
    {
        public int ID { get; set; }
        public string STATUS { get; set; }
    }

    public class Region
    {
        public int ID { get; set; }
        public string REGION_ABBR { get; set; }
    }
}
