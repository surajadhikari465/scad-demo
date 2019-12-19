using System;
using System.Data;

namespace Icon.Web.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlParameterAttribute : Attribute
    {
        public string ParamName { get; set; }
        public SqlDbType SqlDbType { get; set; }
    }
}
