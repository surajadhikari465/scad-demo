using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Models
{
    public class NationalClassModel
    {
        public int HierarchyClassId { get; set; }
        public string Name { get; set; }
        public int HierarchyParentClassId { get; set; }
    }
}
