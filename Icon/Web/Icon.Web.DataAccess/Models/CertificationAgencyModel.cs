using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Models
{
    public class CertificationAgencyModel : HierarchyClassModel
    {
        public string GlutenFree { get; set; }
        public string Kosher { get; set; }
        public string NonGMO { get; set; }
        public string Organic { get; set; }
        public string Vegan { get; set; }
        public string DefaultOrganic { get; set; }
    }
}
