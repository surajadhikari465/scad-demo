using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Common.DataAccess;
using Icon.Common.Models;

namespace Icon.Common.Validators.ItemAttributes
{
    public class ItemAttributesValidatorFactory : IItemAttributesValidatorFactory
    {
        private IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler;

        public ItemAttributesValidatorFactory(
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler)
        {
            this.getAttributesQueryHandler = getAttributesQueryHandler;
        }

        public IItemAttributesValidator CreateItemAttributesJsonValidator(string attributeName)
        {
            var attribute = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>()).SingleOrDefault(a => a.AttributeName == attributeName);
            if (attribute == null)
            {
                throw new ArgumentException(
                    $"Unable to create validator for attribute '{attributeName}'. Attribute could not be found.",
                    nameof(attributeName));
            }

            if (attribute.IsPickList)
            {
                return new ItemAttributesPickListValidator(attribute);
            }
            else if (attribute.DataTypeName == Constants.DataTypeNames.Text)
            {
                return new ItemAttributesTextValidator(attribute);
            }
            else if (attribute.DataTypeName == Constants.DataTypeNames.Number)
            {
                return new ItemAttributesNumericItemValidator(attribute);
            }
            else if (attribute.DataTypeName == Constants.DataTypeNames.Boolean)
            {
                return new ItemAttributesBooleanValidator(attribute);
            }
            else if (attribute.DataTypeName == Constants.DataTypeNames.Date)
            {
                return new ItemAttributesDateValidator(attribute);
            }
            else
            {
                throw new InvalidOperationException($"Unable to create validator for attribute '{attributeName}'. Attribute has data type '{attribute.DataTypeName}' which doesn't have a mapped validator.");
            }
        }
    }
}