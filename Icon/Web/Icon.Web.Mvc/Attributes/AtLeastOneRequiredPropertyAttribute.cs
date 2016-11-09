using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AtLeastOneRequiredPropertyAttribute : Attribute
    {
    }
}