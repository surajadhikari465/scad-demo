using Icon.Esb.CchTax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.CchTax.Commands
{
    public class SaveTaxHierarchyClassesParameters 
    {
        public List<TaxHierarchyClassModel> TaxHierarchyClasses { get; set; }
        public IEnumerable<RegionModel> Regions { get; set; }
        public string CchTaxMessage { get; set; }
        public int DaysToKeepArchivedMessages { get; set; }
    }
}
