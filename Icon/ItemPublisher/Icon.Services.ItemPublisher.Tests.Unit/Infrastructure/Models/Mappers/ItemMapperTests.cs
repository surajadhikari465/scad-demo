using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Tests
{
    [TestClass]
    public class ItemMapperTests
    {
        [TestMethod]
        public void MapEntityToModel_ModelIsMappedCorrectly()
        {
            ItemMapper mapper = new ItemMapper();

            var entity = new Item()
            {
                ItemId = 1,
                ItemTypeId = 2,
                ItemTypeDescription = "ItemTypeDescription",
                ScanCode = "ScanCode",
                ScanCodeId = 5,
                ScanCodeTypeDesc = "ScanCodeTypeDesc",
                ScanCodeTypeId = 6,
                ItemAttributesJson = "{'property':'value'}",
                SysEndTimeUtc = DateTime.Parse("1900-01-03"),
                SysStartTimeUtc = DateTime.Parse("1900-01-04")
            };

            ItemModel model = mapper.MapEntityToModel(entity);

            Assert.AreEqual("value", model.ItemAttributes["property"]);
            Assert.AreEqual(1, model.ItemId);
            Assert.AreEqual("ItemTypeDescription", model.ItemTypeDescription);
            Assert.AreEqual(2, model.ItemTypeId);
            Assert.AreEqual("ScanCode", model.ScanCode);
            Assert.AreEqual(5, model.ScanCodeId);
            Assert.AreEqual("ScanCodeTypeDesc", model.ScanCodeTypeDesc);
            Assert.AreEqual(6, model.ScanCodeTypeId);
            Assert.AreEqual(DateTime.Parse("1900-01-03"), model.SysEndTimeUtc);
            Assert.AreEqual(DateTime.Parse("1900-01-04"), model.SysStartTimeUtc);
            Assert.IsNull(model.ImageUrl);
            Assert.IsNull(model.KitchenDescription);
        }

        [TestMethod]
        public void MapEntityToModel_ItemAttributesJsonHasKitchenItemAsYes_KitchenItemTrueAndRemovedFromDictionary()
        {
            ItemMapper mapper = new ItemMapper();

            var entity = new Item()
            {
                ItemId = 1,
                ItemTypeId = 2,
                ItemTypeDescription = "ItemTypeDescription",
                ScanCode = "ScanCode",
                ScanCodeId = 5,
                ScanCodeTypeDesc = "ScanCodeTypeDesc",
                ScanCodeTypeId = 6,
                ItemAttributesJson = $"{{'property':'value','{ItemPublisherConstants.Attributes.KitchenItem}':'Yes'}}",
                SysEndTimeUtc = DateTime.Parse("1900-01-03"),
                SysStartTimeUtc = DateTime.Parse("1900-01-04")
            };

            ItemModel model = mapper.MapEntityToModel(entity);

            Assert.AreEqual("value", model.ItemAttributes["property"]);
            Assert.IsFalse(model.ItemAttributes.TryGetValue(ItemPublisherConstants.Attributes.KitchenItem, out _));
            Assert.AreEqual(1, model.ItemId);
            Assert.AreEqual("ItemTypeDescription", model.ItemTypeDescription);
            Assert.AreEqual(2, model.ItemTypeId);
            Assert.AreEqual("ScanCode", model.ScanCode);
            Assert.AreEqual(5, model.ScanCodeId);
            Assert.AreEqual("ScanCodeTypeDesc", model.ScanCodeTypeDesc);
            Assert.AreEqual(6, model.ScanCodeTypeId);
            Assert.AreEqual(DateTime.Parse("1900-01-03"), model.SysEndTimeUtc);
            Assert.AreEqual(DateTime.Parse("1900-01-04"), model.SysStartTimeUtc);
            Assert.AreEqual(false, model.IsHospitalityItem);
            Assert.AreEqual(false, model.IsHospitalityItemSpecified);
            Assert.AreEqual(true, model.IsKitchenItem);
            Assert.AreEqual(true, model.IsKitchenItemSpecified);
            Assert.IsNull(model.ImageUrl);
            Assert.IsNull(model.KitchenDescription);
        }

        [TestMethod]
        public void MapEntityToModel_ItemAttributesJsonHasKitchenItemAsNo_KitchenItemFalseAndRemovedFromDictionary()
        {
            ItemMapper mapper = new ItemMapper();

            var entity = new Item()
            {
                ItemId = 1,
                ItemTypeId = 2,
                ItemTypeDescription = "ItemTypeDescription",
                ScanCode = "ScanCode",
                ScanCodeId = 5,
                ScanCodeTypeDesc = "ScanCodeTypeDesc",
                ScanCodeTypeId = 6,
                ItemAttributesJson = $"{{'property':'value','{ItemPublisherConstants.Attributes.KitchenItem}':'No'}}",
                SysEndTimeUtc = DateTime.Parse("1900-01-03"),
                SysStartTimeUtc = DateTime.Parse("1900-01-04")
            };

            ItemModel model = mapper.MapEntityToModel(entity);

            Assert.AreEqual("value", model.ItemAttributes["property"]);
            Assert.IsFalse(model.ItemAttributes.TryGetValue(ItemPublisherConstants.Attributes.KitchenItem, out _));
            Assert.AreEqual(1, model.ItemId);
            Assert.AreEqual("ItemTypeDescription", model.ItemTypeDescription);
            Assert.AreEqual(2, model.ItemTypeId);
            Assert.AreEqual("ScanCode", model.ScanCode);
            Assert.AreEqual(5, model.ScanCodeId);
            Assert.AreEqual("ScanCodeTypeDesc", model.ScanCodeTypeDesc);
            Assert.AreEqual(6, model.ScanCodeTypeId);
            Assert.AreEqual(DateTime.Parse("1900-01-03"), model.SysEndTimeUtc);
            Assert.AreEqual(DateTime.Parse("1900-01-04"), model.SysStartTimeUtc);
            Assert.AreEqual(false, model.IsHospitalityItem);
            Assert.AreEqual(false, model.IsHospitalityItemSpecified);
            Assert.AreEqual(false, model.IsKitchenItem);
            Assert.AreEqual(true, model.IsKitchenItemSpecified);
            Assert.IsNull(model.ImageUrl);
            Assert.IsNull(model.KitchenDescription);
        }

        [TestMethod]
        public void MapEntityToModel_ItemAttributesJsonHasHospitalityItemAsYes_HospitalityItemTrueAndRemovedFromDictionary()
        {
            ItemMapper mapper = new ItemMapper();

            var entity = new Item()
            {
                ItemId = 1,
                ItemTypeId = 2,
                ItemTypeDescription = "ItemTypeDescription",
                ScanCode = "ScanCode",
                ScanCodeId = 5,
                ScanCodeTypeDesc = "ScanCodeTypeDesc",
                ScanCodeTypeId = 6,
                ItemAttributesJson = $"{{'property':'value','{ItemPublisherConstants.Attributes.HospitalityItem}':'Yes'}}",
                SysEndTimeUtc = DateTime.Parse("1900-01-03"),
                SysStartTimeUtc = DateTime.Parse("1900-01-04")
            };

            ItemModel model = mapper.MapEntityToModel(entity);

            Assert.AreEqual("value", model.ItemAttributes["property"]);
            Assert.IsFalse(model.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.HospitalityItem));
            Assert.AreEqual(1, model.ItemId);
            Assert.AreEqual("ItemTypeDescription", model.ItemTypeDescription);
            Assert.AreEqual(2, model.ItemTypeId);
            Assert.AreEqual("ScanCode", model.ScanCode);
            Assert.AreEqual(5, model.ScanCodeId);
            Assert.AreEqual("ScanCodeTypeDesc", model.ScanCodeTypeDesc);
            Assert.AreEqual(6, model.ScanCodeTypeId);
            Assert.AreEqual(DateTime.Parse("1900-01-03"), model.SysEndTimeUtc);
            Assert.AreEqual(DateTime.Parse("1900-01-04"), model.SysStartTimeUtc);
            Assert.AreEqual(true, model.IsHospitalityItem);
            Assert.AreEqual(true, model.IsHospitalityItemSpecified);
            Assert.AreEqual(false, model.IsKitchenItem);
            Assert.AreEqual(false, model.IsKitchenItemSpecified);
            Assert.IsNull(model.ImageUrl);
            Assert.IsNull(model.KitchenDescription);
        }

        [TestMethod]
        public void MapEntityToModel_ItemAttributesJsonHasHospitalityItemAsNo_HospitalityItemFalseAndRemovedFromDictionary()
        {
            ItemMapper mapper = new ItemMapper();

            var entity = new Item()
            {
                ItemId = 1,
                ItemTypeId = 2,
                ItemTypeDescription = "ItemTypeDescription",
                ScanCode = "ScanCode",
                ScanCodeId = 5,
                ScanCodeTypeDesc = "ScanCodeTypeDesc",
                ScanCodeTypeId = 6,
                ItemAttributesJson = $"{{'property':'value','{ItemPublisherConstants.Attributes.HospitalityItem}':'No'}}",
                SysEndTimeUtc = DateTime.Parse("1900-01-03"),
                SysStartTimeUtc = DateTime.Parse("1900-01-04")
            };

            ItemModel model = mapper.MapEntityToModel(entity);

            Assert.AreEqual("value", model.ItemAttributes["property"]);
            Assert.IsFalse(model.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.HospitalityItem));
            Assert.AreEqual(1, model.ItemId);
            Assert.AreEqual("ItemTypeDescription", model.ItemTypeDescription);
            Assert.AreEqual(2, model.ItemTypeId);
            Assert.AreEqual("ScanCode", model.ScanCode);
            Assert.AreEqual(5, model.ScanCodeId);
            Assert.AreEqual("ScanCodeTypeDesc", model.ScanCodeTypeDesc);
            Assert.AreEqual(6, model.ScanCodeTypeId);
            Assert.AreEqual(DateTime.Parse("1900-01-03"), model.SysEndTimeUtc);
            Assert.AreEqual(DateTime.Parse("1900-01-04"), model.SysStartTimeUtc);
            Assert.AreEqual(false, model.IsHospitalityItem);
            Assert.AreEqual(true, model.IsHospitalityItemSpecified);
            Assert.AreEqual(false, model.IsKitchenItem);
            Assert.AreEqual(false, model.IsKitchenItemSpecified);
            Assert.IsNull(model.ImageUrl);
            Assert.IsNull(model.KitchenDescription);
        }

        [TestMethod]
        public void MapEntityToModel_ItemAttributesJsonHasKitchenDescriptionValue_KitchenDescriptionPropertySetAndRemovedFromDictionary()
        {
            ItemMapper mapper = new ItemMapper();

            var entity = new Item()
            {
                ItemId = 1,
                ItemTypeId = 2,
                ItemTypeDescription = "ItemTypeDescription",
                ScanCode = "ScanCode",
                ScanCodeId = 5,
                ScanCodeTypeDesc = "ScanCodeTypeDesc",
                ScanCodeTypeId = 6,
                ItemAttributesJson = $"{{'property':'value','{ItemPublisherConstants.Attributes.KitchenDescription}':'Test Kitchen Description'}}",
                SysEndTimeUtc = DateTime.Parse("1900-01-03"),
                SysStartTimeUtc = DateTime.Parse("1900-01-04")
            };

            ItemModel model = mapper.MapEntityToModel(entity);

            Assert.AreEqual("value", model.ItemAttributes["property"]);
            Assert.IsFalse(model.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.KitchenDescription));
            Assert.AreEqual(1, model.ItemId);
            Assert.AreEqual("ItemTypeDescription", model.ItemTypeDescription);
            Assert.AreEqual(2, model.ItemTypeId);
            Assert.AreEqual("ScanCode", model.ScanCode);
            Assert.AreEqual(5, model.ScanCodeId);
            Assert.AreEqual("ScanCodeTypeDesc", model.ScanCodeTypeDesc);
            Assert.AreEqual(6, model.ScanCodeTypeId);
            Assert.AreEqual(DateTime.Parse("1900-01-03"), model.SysEndTimeUtc);
            Assert.AreEqual(DateTime.Parse("1900-01-04"), model.SysStartTimeUtc);
            Assert.AreEqual(false, model.IsHospitalityItem);
            Assert.AreEqual(false, model.IsHospitalityItemSpecified);
            Assert.AreEqual(false, model.IsKitchenItem);
            Assert.AreEqual(false, model.IsKitchenItemSpecified);
            Assert.AreEqual("Test Kitchen Description", model.KitchenDescription);
            Assert.IsNull(model.ImageUrl);
        }

        [TestMethod]
        public void MapEntityToModel_ItemAttributesJsonHasImageUrlValue_ImageUrlPropertySetAndRemovedFromDictionary()
        {
            ItemMapper mapper = new ItemMapper();

            var entity = new Item()
            {
                ItemId = 1,
                ItemTypeId = 2,
                ItemTypeDescription = "ItemTypeDescription",
                ScanCode = "ScanCode",
                ScanCodeId = 5,
                ScanCodeTypeDesc = "ScanCodeTypeDesc",
                ScanCodeTypeId = 6,
                ItemAttributesJson = $"{{'property':'value','{ItemPublisherConstants.Attributes.Url1}':'Test ImageUrl'}}",
                SysEndTimeUtc = DateTime.Parse("1900-01-03"),
                SysStartTimeUtc = DateTime.Parse("1900-01-04")
            };

            ItemModel model = mapper.MapEntityToModel(entity);

            Assert.AreEqual("value", model.ItemAttributes["property"]);
            Assert.IsFalse(model.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.Url1));
            Assert.AreEqual(1, model.ItemId);
            Assert.AreEqual("ItemTypeDescription", model.ItemTypeDescription);
            Assert.AreEqual(2, model.ItemTypeId);
            Assert.AreEqual("ScanCode", model.ScanCode);
            Assert.AreEqual(5, model.ScanCodeId);
            Assert.AreEqual("ScanCodeTypeDesc", model.ScanCodeTypeDesc);
            Assert.AreEqual(6, model.ScanCodeTypeId);
            Assert.AreEqual(DateTime.Parse("1900-01-03"), model.SysEndTimeUtc);
            Assert.AreEqual(DateTime.Parse("1900-01-04"), model.SysStartTimeUtc);
            Assert.AreEqual(false, model.IsHospitalityItem);
            Assert.AreEqual(false, model.IsHospitalityItemSpecified);
            Assert.AreEqual(false, model.IsKitchenItem);
            Assert.AreEqual(false, model.IsKitchenItemSpecified);
            Assert.AreEqual("Test ImageUrl", model.ImageUrl);
            Assert.IsNull(model.KitchenDescription);
        }
    }
}