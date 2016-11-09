using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlParameterAttribute : Attribute
    {
        public string ParamName { get; set; }
        public SqlDbType SqlDbType { get; set; }
    }
}
