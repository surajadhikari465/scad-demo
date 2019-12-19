using System;

namespace Icon.Web.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AtLeastOneRequiredPropertyAttribute : Attribute
    {
    }
}