using System;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    /// <summary>
    /// This class is responsible for converting and formatting values that are sent in ESB messages.
    /// There are business rules that state that Yes/No boolean attributes should be sent as 0 and 1
    /// along with other exceptions and special cases.
    /// </summary>
    public class ValueFormatter : IValueFormatter
    {
        public string FormatValueForMessage(ItemPublisher.Repositories.Entities.Attributes attribute, string attributeValue)
        {
            if (!attribute.IsSpecialTransform)
            {
                return attributeValue;
            }
            else
            {
                if (attribute.DataTypeName == ItemPublisherConstants.DataTypes.Boolean)
                {
                    return this.FormatBoolean(attributeValue);
                }
                else if (attribute.IsPickList)
                {
                    if (attribute.AttributeName == ItemPublisherConstants.Attributes.Kosher)
                    {
                        return this.FormatKosherAttribute(attributeValue);
                    }
                    else if (ItemPublisherConstants.Attributes.SpecialAttributesWithAgencyNames.Contains(attribute.AttributeName))
                    {
                        return this.FormatAttributesWithAgencyNames(attributeValue);
                    }
                    else
                    {
                        return this.FormatPickList(attributeValue);
                    }
                }
                else
                {
                    return attributeValue;
                }
            }
        }

        private string FormatBoolean(string attributeValue)
        {
            if (attributeValue.ToUpper() == "TRUE")
            {
                return "1";
            }
            else if (attributeValue.ToUpper() == "FALSE")
            {
                return "0";
            }
            else
            {
                throw new ArgumentException($@"Attribute value '{attributeValue}' is not a valid boolean value. Expected 'true' or 'false'");
            }
        }

        private string FormatAttributesWithAgencyNames(string attributeValue)
        {
            if (attributeValue.ToUpper() == "NO")
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }

        /// <summary>
        /// Kosher is a strange attribute where It only has a value of Yes now. But it is a picklist but it should not
        /// be translated into a 0/1. So we just return the value for now.
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        private string FormatKosherAttribute(string attributeValue)
        {
            return attributeValue;
        }

        private string FormatPickList(string attributeValue)
        {
            if (attributeValue.ToUpper() == "YES")
            {
                return "1";
            }
            else if (attributeValue.ToUpper() == "NO")
            {
                return "0";
            }
            else
            {
                return attributeValue;
            }
        }
    }
}