namespace Testing.Core
{
    using System;
    using System.Linq.Expressions;

    using Helpers;
    
    public class ObjectBuilder<TObject> where TObject : class
    {
        private TObject concreteObject;

        public TObject CreatedObject { get { return concreteObject; } }

        public ObjectBuilder()
        {
            this.concreteObject = Activator.CreateInstance(typeof(TObject)) as TObject;
        }

        /// <summary>
        /// Set a property to a specific value.
        /// </summary>
        /// <example>
        /// MyObject myObject = new ObjectBuilder[MyObject]().With(x => x.MyProperty, someValue);
        /// </example>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ObjectBuilder<TObject> With<TProperty>(
            Expression<Func<TObject, TProperty>> propertyExpression,
            TProperty value)
        {
            var propertyName = PropertyHelper.GetPropertyName(propertyExpression);
            var propertyInfo = typeof(TObject).GetProperty(propertyName);
            propertyInfo.SetValue(this.concreteObject, value);

            return this;
        }

        // This will implicitly convert the objectbuilder to your object type.
        public static implicit operator TObject(ObjectBuilder<TObject> builder)
        {

            return builder.concreteObject;
        }
    }
}
