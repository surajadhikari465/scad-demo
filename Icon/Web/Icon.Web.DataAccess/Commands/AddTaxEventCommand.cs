using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class AddTaxEventCommand
    {
        public string TaxAbbreviation { get; set; }
        public int HierarchyClassId { get; set; }
    }
}
