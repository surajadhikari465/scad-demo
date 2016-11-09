namespace Testing.Core.Helpers
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class PropertyHelper
    {
        /// <summary>
        /// Returns the name of a property.
        /// </summary>
        /// <example>
        /// GetPropertyName[MyObjectType, MyPropertyType](o => o.MyAge)
        /// will return "MyAge"
        /// </example>
        /// <typeparam name="TObject">The object's type.</typeparam>
        /// <typeparam name="TProperty">The property's type.</typeparam>
        /// <param name="propertyExpression">The public property.</param>
        /// <returns>The name of the public property.</returns>
        public static string GetPropertyName<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            var property = memberExpression.Member as PropertyInfo;
            var getMethod = property.GetGetMethod(true);
            return property.Name;
        }
    }
}
