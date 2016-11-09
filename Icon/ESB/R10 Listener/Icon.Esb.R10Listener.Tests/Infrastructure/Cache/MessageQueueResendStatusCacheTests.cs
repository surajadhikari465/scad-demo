using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Esb.R10Listener.Infrastructure.Cache;
using System.Collections.Generic;
using Icon.Framework;
using Icon.Esb.R10Listener.Models;

namespace Icon.Esb.R10Listener.Tests.Infrastructure.Cache
{
    [TestClass]
    public class MessageQueueResendStatusCacheTests
    {
        private MessageQueueResendStatusCache cache;

        [TestInitialize]
        public void Initialize()
        {
            cache = new MessageQueueResendStatusCache();
        }

        [TestMethod]
        public void Get_NoResendStatusExists_ShouldReturnNewResendStatus()
        {
            //When
            var resendStatus = cache.Get(MessageTypes.Product, 5);

            //Then
            Assert.IsNotNull(resendStatus);
            Assert.AreEqual(5, resendStatus.MessageQueueId);
            Assert.AreEqual(0, resendStatus.NumberOfResends);
        }
        
        [TestMethod]
        public void Get_ResendStatusExists_ShouldReturnExistingResendStatus()
        {
            //Given
            cache.AddOrUpdate(MessageTypes.Product, new MessageQueueResendStatus { MessageQueueId = 5, NumberOfResends = 7 });

            //When
            var resendStatus = cache.Get(MessageTypes.Product, 5);

            //Then
            Assert.IsNotNull(resendStatus);
            Assert.AreEqual(5, resendStatus.MessageQueueId);
            Assert.AreEqual(7, resendStatus.NumberOfResends);
        }

        [TestMethod]
        public void AddOrUpdate_ResendStatusAlreadyExists_ShouldUpdateResendStatus()
        {
            //Given
            cache.AddOrUpdate(MessageTypes.Product, new MessageQueueResendStatus { MessageQueueId = 5 });

            //When
            cache.AddOrUpdate(MessageTypes.Product, new MessageQueueResendStatus { MessageQueueId = 5, NumberOfResends = 9 });
            var resendStatus = cache.Get(MessageTypes.Product, 5);

            //Then
            Assert.IsNotNull(resendStatus);
            Assert.AreEqual(5, resendStatus.MessageQueueId);
            Assert.AreEqual(9, resendStatus.NumberOfResends);
        }
    }
}
