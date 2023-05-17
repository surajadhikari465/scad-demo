using System;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    /// <summary>
    /// Class that handles all logic around populating the DVS message header
    /// </summary>
    public class MessageHeaderBuilder : IMessageHeaderBuilder
    {
        private const string NonReceivingSystemsJmsProperty = "nonReceivingSysName";

        /// <summary>
        /// Returns a dictionary for the DVS message header. Includes setting NonReceivingSystemsJmsProperty.
        /// </summary>
        /// <param name="nonReceivingSystemsProduct"></param>
        /// <returns></returns>
        public Dictionary<string, string> BuildMessageHeader(List<string> nonReceivingSystemsProduct, string messageId)
        {
            var messageProperties = new Dictionary<string, string>
            {
               { "IconMessageID", messageId},
               { "Source", "Icon" },
               { "TransactionType", "Global Item" }
            };

            messageProperties.Add(NonReceivingSystemsJmsProperty, String.Join(",", nonReceivingSystemsProduct));

            return messageProperties;
        }
    }
}