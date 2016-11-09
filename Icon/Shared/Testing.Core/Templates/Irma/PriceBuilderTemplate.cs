using Irma.Framework;
using System;
using System.Collections.Generic;
using Testing.Core.Helpers;

namespace Testing.Core.Templates.Irma
{
    public class PriceBuilderTemplate : IObjectBuilderTemplate<Price>, ISqlBuilderTemplate<Price>
    {
        public string TableName { get { return typeof(Price).Name; } }

        public string IdentityColumn { get { return null; } }

        private Lazy<Dictionary<string, string>> propertyToColumnMapping = new Lazy<Dictionary<string, string>>(
            () => new Dictionary<string, string> { { PropertyHelper.GetPropertyName((Price p) => p.Price1), "Price" } });

        public Dictionary<string, string> PropertyToColumnMapping { get { return this.propertyToColumnMapping.Value; } }

        public ObjectBuilder<Price> BuildDefaults()
        {
            return new ObjectBuilder<Price>();
        }
    }
}
