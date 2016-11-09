namespace Testing.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ObjectBuilderFactory
    {
        /// <summary>
        /// Store the templates with key of your item type and value of your template.
        /// </summary>
        private Dictionary<string, Type> builderTemplates;

        public ObjectBuilderFactory(Assembly templateAssembly)
        {
            var interfaceName = typeof(IObjectBuilderTemplate<>).Name;

            // Get all the types that have the IObjectBuilderTemplate and store them in the template dictionary.
            this.builderTemplates = templateAssembly.GetTypes()
                .Where(t => t.GetInterface(interfaceName) != null)
                .ToDictionary(
                    t => t.GetInterface(interfaceName).GetGenericArguments().First().Name);
        }

        /// <summary>
        /// Returns a builder of desired object type with any defaults from any existing templates.
        /// </summary>
        /// <typeparam name="TObject">The object type.</typeparam>
        /// <returns>An object builder with default values.</returns>
        public ObjectBuilder<TObject> Build<TObject>() where TObject : class
        {
            var objectTypeName = typeof(TObject).Name;

            return this.builderTemplates.ContainsKey(objectTypeName)
                ? (Activator.CreateInstance(this.builderTemplates[objectTypeName]) as IObjectBuilderTemplate<TObject>).BuildDefaults()
                : new ObjectBuilder<TObject>();
        }
    }
}