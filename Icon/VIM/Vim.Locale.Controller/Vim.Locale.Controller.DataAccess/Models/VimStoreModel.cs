using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vim.Locale.Controller.DataAccess.Models
{
    public class VimStoreModel
    {
        public int PSBU { get; set; }
        public string RegStoreNum { get; set; }
        public string Region { get; set; }
        public string StoreName { get; set; }
        public string StoreAbbreviation { get; set; }
        public string PosType { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string LastUser { get; set; }
        public DateTime TimeStamp { get; set; }
        public string TimeZone { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
    }

}
