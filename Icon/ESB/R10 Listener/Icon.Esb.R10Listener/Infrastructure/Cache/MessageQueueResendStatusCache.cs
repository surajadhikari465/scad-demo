using Icon.Esb.R10Listener.Models;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Infrastructure.Cache
{
    public class MessageQueueResendStatusCache : IMessageQueueResendStatusCache
    {
        private const int CacheRefreshTime = 30;
        private MemoryCache productResendStatusCache;
        private MemoryCache itemLocaleResendStatusCache;
        private MemoryCache priceResendStatusCache;

        public MessageQueueResendStatusCache()
        {
            productResendStatusCache = new MemoryCache("Product");
            itemLocaleResendStatusCache = new MemoryCache("ItemLocale");
            priceResendStatusCache = new MemoryCache("Price");
        }

        public MessageQueueResendStatus Get(int messageTypeId, int messageQueueId)
        {
            var cache = GetCache(messageTypeId);
            var status = cache.Get(messageQueueId.ToString()) as MessageQueueResendStatus;

            if(status == null)
            {
                return new MessageQueueResendStatus { MessageQueueId = messageQueueId, NumberOfResends = 0 };
            }
            else
            {
                return status;
            }
        }

        public void AddOrUpdate(int messageTypeId, MessageQueueResendStatus resendStatus)
        {
            var cache = GetCache(messageTypeId);

            cache.Set(resendStatus.MessageQueueId.ToString(), resendStatus, DateTimeOffset.Now.AddMinutes(CacheRefreshTime));
        }

        private MemoryCache GetCache(int messageType)
        {
            switch (messageType)
            {
                case MessageTypes.Product:
                    return productResendStatusCache;
                case MessageTypes.ItemLocale:
                    return itemLocaleResendStatusCache;
                case MessageTypes.Price:
                    return priceResendStatusCache;
                default:
                    throw new ArgumentException(String.Format("Attempted to retrieve a MessageQueue cache for MessageType {0}. No cache is set for MessageType {0} is not supported.", messageType));
            }
        }
    }
}
