using AttributePublisher.DataAccess.Models;
using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using System.Collections.Generic;
using System.Linq;

namespace AttributePublisher.MessageBuilders
{
    public class AttributeMessageBuilder : IMessageBuilder<List<AttributeModel>>
    {
        ISerializer<AttributesType> serializer;

        public AttributeMessageBuilder(ISerializer<AttributesType> serializer)
        {
            this.serializer = serializer;
        }

        public string BuildMessage(List<AttributeModel> request)
        {
            AttributesType attributesType = new AttributesType
            {
                Attribute = request.Select(a => new AttributeType
                {
                    DataType = a.DataType,
                    Description = a.XmlTraitDescription,
                    Group = a.AttributeGroupName,
                    Name = a.AttributeName,
                    TraitCode = a.TraitCode
                }).ToArray()
            };

            return serializer.Serialize(attributesType);
        }
    }
}
