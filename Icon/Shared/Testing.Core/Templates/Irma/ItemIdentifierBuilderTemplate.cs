using Irma.Framework;
using System.Collections.Generic;
using Testing.Core.Helpers;

namespace Testing.Core.Templates.Irma
{
    public class ItemIdentifierBuilderTemplate : IObjectBuilderTemplate<ItemIdentifier>, ISqlBuilderTemplate<ItemIdentifier>
    {
        public string TableName { get { return typeof(ItemIdentifier).Name; } }

        public string IdentityColumn
        {
            get
            {
                return PropertyHelper.GetPropertyName((ItemIdentifier i) => i.Identifier_ID);
            }
        }

        public Dictionary<string, string> PropertyToColumnMapping { get { return null; } }

        public ObjectBuilder<ItemIdentifier> BuildDefaults()
        {
            return new ObjectBuilder<ItemIdentifier>()
                .With(x => x.Identifier, "1234598760")
                .With(x => x.CheckDigit, "0")
                .With(x => x.IdentifierType, "B");
        }
    }
}
