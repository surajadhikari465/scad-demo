using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Models.Builders;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Tests
{
    [TestClass()]
    public class MessageQueueItemModelTests
    {
        /// <summary>
        /// This tests test that hospitality and kitchen attributed are removed from the attribute collection because
        /// they are elements not attributes in the ESB message
        /// </summary>
        [TestMethod]
        public void Build_WhenModelLoaded_KitchenAttributesExist_KitchenAttributesAreRemovedFromAttributeCollection()
        {
            // Given.
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());

            // When.
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
                new Item()
                {
                    ItemAttributesJson = @"{
                         'IsActive': '1',
                         'IsHospitalityItem':'1' ,
                         'IsHospitalityItemSpecified':'1',
                         'IsKitchenItem':'1',
                         'IsKitchenItemSpecified':'1'
                    }"
                },
              new List<Hierarchy>(),
              new Nutrition());

            // Then.
            // the following attributes should be removed from the attributes collection and set on the item properties
            Assert.IsFalse(model.Item.ItemAttributes.Any(x => x.Key == "IsKitchenItemSpecified"));
            Assert.IsFalse(model.Item.ItemAttributes.Any(x => x.Key == "IsKitchenItem"));
            Assert.IsFalse(model.Item.ItemAttributes.Any(x => x.Key == "IsHospitalityItemSpecified"));
            Assert.IsFalse(model.Item.ItemAttributes.Any(x => x.Key == "IsHospitalityItem"));
        }

        /// <summary>
        /// There are rows in the attributes table that are really properties on the Item. This test that when the attributes
        /// exist that the value is set on the Item table
        /// </summary>
        [TestMethod]
        public void Build_WhenModelLoaded_KitchenAttributesExist_KitchenAttributesAreTranslated()
        {
            // Given.
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());

            // When.
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
               new Item()
               {
                   ItemAttributesJson = @"{
                         'IsActive': '1',
                         'IsHospitalityItem':'1' ,
                         'IsHospitalityItemSpecified':'1',
                         'IsKitchenItem':'1',
                         'IsKitchenItemSpecified':'1'
                    }"
               },
              new List<Hierarchy>(),
              new Nutrition());

            // Then.
            Assert.IsTrue(model.Item.IsHospitalityItem);
            Assert.IsTrue(model.Item.IsHospitalityItemSpecified);
            Assert.IsTrue(model.Item.IsKitchenItem);
            Assert.IsTrue(model.Item.IsKitchenItemSpecified);
        }

        /// <summary>
        /// There are rows in the attributes table that are really properties on the Item. This test that when the attributes
        /// do not exist that the value is not set on the Item table
        /// </summary>
        [TestMethod]
        public void Build_WhenModelLoaded_KitchenAttributesDoNotExist_AttributesAreNotTranslated()
        {
            // Given.
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());

            // When.
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
              new Item()
              {
                  ItemAttributesJson = "{'IsActive':1}"
              },
              new List<Hierarchy>(),
              new Nutrition());

            // Then.
            Assert.IsFalse(model.Item.IsHospitalityItem);
            Assert.IsFalse(model.Item.IsHospitalityItemSpecified);
            Assert.IsFalse(model.Item.IsKitchenItem);
            Assert.IsFalse(model.Item.IsKitchenItemSpecified);
        }
    }
}