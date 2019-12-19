using System.Collections.Generic;
using Icon.Esb.Schemas.Attributes.ContractTypes;

namespace Mammoth.Esb.AttributeListener.Commands
{
    public class AddOrUpdateAttributesCommand
    {
        public AttributesType Attributes { get; set; }
    }
}