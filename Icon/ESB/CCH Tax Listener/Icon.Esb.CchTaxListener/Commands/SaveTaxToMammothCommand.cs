using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.CchTax.Models;
using System.Data.SqlClient;

namespace Icon.Esb.CchTax.Commands
{
    public class SaveTaxToMammothCommand
    {
        public List<TaxHierarchyClassModel> TaxHierarchyClasses { get; set; }
    }
}
