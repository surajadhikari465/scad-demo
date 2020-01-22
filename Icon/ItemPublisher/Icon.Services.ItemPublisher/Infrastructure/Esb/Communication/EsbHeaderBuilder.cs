using System;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    /// <summary>
    /// Class that handles all logic around populating the ESB message header
    /// </summary>
    public class EsbHeaderBuilder : IEsbHeaderBuilder
    {
        private const string NonReceivingSystemsJmsProperty = "nonReceivingSysName";

        /// <summary>
        /// Returns a dictionary for the ESB message header. Includes setting NonReceivingSystemsJmsProperty.
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