namespace Icon.Web.Common.Utility
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class PropertyUtility
    {
        public static string GetPropertyName<T,P>(
            Expression<Func<T,P>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            var property = memberExpression.Member as PropertyInfo;
            var getMethod = property.GetGetMethod(true);
            return property.Name;
        }
    }
}
