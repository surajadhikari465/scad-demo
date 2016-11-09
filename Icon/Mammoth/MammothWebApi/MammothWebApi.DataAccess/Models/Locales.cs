using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Locales
    {
        public string Region { get; set; }
        public int LocaleID { get; set; }
        public int BusinessUnitID { get; set; }
        public string StoreName { get; set; }
        public string StoreAbbrev { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
