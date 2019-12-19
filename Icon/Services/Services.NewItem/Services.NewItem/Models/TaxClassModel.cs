using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Models
{
    public class TaxClassModel
    {
        public int HierarchyClassId { get; set; }
        public string Name { get; set; }
        public string TaxCode { get; set; }
    }
}
