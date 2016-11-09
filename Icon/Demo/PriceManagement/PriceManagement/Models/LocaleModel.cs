using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceManagement.Models
{
    public class LocaleModel
    {
        public int LocaleId { get; set; }
        public string LocaleName { get; set; }
        public int BusinessUnitId { get; set; }
        public string RegionCode { get; set; }
    }
}
