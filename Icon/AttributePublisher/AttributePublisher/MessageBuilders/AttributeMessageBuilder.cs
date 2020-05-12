using AttributePublisher.DataAccess.Models;
using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AttributePublisher.MessageBuilders
{
    public class AttributeMessageBuilder : IMessageBuilder<List<AttributeModel>>
    {
        ISerializer<TraitsType> serializer;
        
        public AttributeMessageBuilder(ISerializer<TraitsType> serializer)
        {
            this.serializer = serializer;
        }
        public string BuildMessage(List<AttributeModel> request)
        {
            if(request != null && request.Any())
            {
                return serializer.Serialize(new TraitsType
                {
                    traits = request.Select(a => CreateTraitType(a)).ToArray()
                });
            }
            else
            {
                throw new ArgumentException("Unable to build message. No attributes received.");
            }
        }

        private TraitType CreateTraitType(AttributeModel a)
        {
            TraitType traitType = new TraitType();
            traitType.Action = ActionEnum.AddOrUpdate;
            traitType.ActionSpecified = true;
            traitType.code = a.TraitCode;
            traitType.dataType = a.DataType;
            traitType.maxValue = a.MaximumNumber;
            traitType.minValue = a.MinimumNumber;
            traitType.pattern = a.CharacterSetRegexPattern;
            traitType.definition = a.Description;
            traitType.id = a.AttributeGroupId.ToString();
            traitType.group = new TraitGroupType
            {
                code = a.AttributeGroupId,
                description = a.AttributeGroupName
            };
            traitType.type = new TraitTypeType
            {
                description = a.XmlTraitDescription
            };

            if (!string.IsNullOrWhiteSpace(a.MaximumNumber) && double.TryParse(a.MaximumNumber, out _)
                && !string.IsNullOrWhiteSpace(a.NumberOfDecimals) && int.TryParse(a.NumberOfDecimals, out _))
            {
                //Precision should equal the sum of the number of digits left of the decimal place and the attribute's NumberOfDecimals
                traitType.decimalPrecision = (a.MaximumNumber.Split('.')[0].Length + int.Parse(a.NumberOfDecimals)).ToString();
            }

            if (!string.IsNullOrWhiteSpace(a.NumberOfDecimals) && int.TryParse(a.NumberOfDecimals, out _))
            {
                traitType.decimalScale = int.Parse(a.NumberOfDecimals).ToString();
            }

            if (a.MaxLengthAllowed.HasValue)
            {
                traitType.maxLength = a.MaxLengthAllowed.Value;
                traitType.maxLengthSpecified = true;
            }

            if(a.IsPickList && a.PickListValues != null && a.PickListValues.Any())
            {
                traitType.type.valuesList = a.PickListValues.ToArray();
            }

            return traitType;
        }
    }
}