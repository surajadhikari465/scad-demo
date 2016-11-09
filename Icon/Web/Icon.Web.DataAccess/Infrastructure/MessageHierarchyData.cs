using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Infrastructure
{
    public class MessageHierarchyData
    {
        public HierarchyClass HierarchyClass { get; set; }
        public bool ClassNameChange { get; set; }
        public bool DeleteMessage { get; set; }
    }
}
