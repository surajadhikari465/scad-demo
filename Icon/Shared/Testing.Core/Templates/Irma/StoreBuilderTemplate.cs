using Irma.Framework;
using System.Collections.Generic;

namespace Testing.Core.Templates.Irma
{
    public class StoreBuilderTemplate : IObjectBuilderTemplate<Store>, ISqlBuilderTemplate<Store>
    {
        public string TableName
        {
            get
            {
                return typeof(Store).Name;
            }
        }

        public string IdentityColumn { get { return null; } }

        public Dictionary<string, string> PropertyToColumnMapping { get { return null; } }

        public ObjectBuilder<Store> BuildDefaults()
        {
            return new ObjectBuilder<Store>()
                .With(x => x.Store_Name, "Test Store");
        }
    }
}
