using System;

namespace Icon.Web.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlResultAttribute : Attribute
    {
        public string SqlSortValue { get; set; }
    }
}
