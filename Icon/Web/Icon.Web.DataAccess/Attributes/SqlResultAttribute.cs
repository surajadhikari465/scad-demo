using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlResultAttribute : Attribute
    {
        public string SqlSortValue { get; set; }
    }
}
