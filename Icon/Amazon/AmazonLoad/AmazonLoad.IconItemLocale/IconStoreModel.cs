using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.IconItemLocale
{
    public class IconStoreModel
    {
        public string RegionCode { get; set; }
        public int LocaleID { get; set; }
        public string LocaleName { get; set; }
        public int BusinessUnit { get; set; }
        public string LocaleTypeCode { get; set; }
        public string LocaleTypeDesc { get; set; }
    }
}
