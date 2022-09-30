using System.Collections.Generic;
using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Icon.Services.ItemPublisher.Tests.Unit.Infrastructure.Filters
{
    [TestClass]
    public class UKItemFilterTests
    {
        private ItemModel itemModel = new ItemModel();

        [TestMethod]
        public void FilterUKItem_UKItemSetToYes_Test()
        {
            Mock<ILogger<UKItemFilter>> loggerMock = new Mock<ILogger<UKItemFilter>>();
            IFilter filter = new UKItemFilter(loggerMock.Object);

            Dictionary<string, string> itemAttributes = new Dictionary<string, string>
            {
                { "UKItem", "Yes" }
            };
            itemModel.ItemAttributes = itemAttributes;

            Assert.IsTrue(filter.Filter(itemModel));
        }

        [TestMethod]
        public void FilterUKItem_UKItemNotPresent_Test()
        {
            Mock<ILogger<UKItemFilter>> loggerMock = new Mock<ILogger<UKItemFilter>>();
            IFilter filter = new UKItemFilter(loggerMock.Object);

            itemModel.ItemAttributes = new Dictionary<string, string>();

            Assert.IsFalse(filter.Filter(itemModel));
        }

        [TestMethod]
        public void FilterUKItem_UKItemNull_Test()
        {
            Mock<ILogger<UKItemFilter>> loggerMock = new Mock<ILogger<UKItemFilter>>();
            IFilter filter = new UKItemFilter(loggerMock.Object);

            Dictionary<string, string> itemAttributes = new Dictionary<string, string>
            {
                { "UKItem", null }
            };
            itemModel.ItemAttributes = itemAttributes;

            Assert.IsFalse(filter.Filter(itemModel));
        }

        [TestMethod]
        public void FilterUKItem_UKItemEmpty_Test()
        {
            Mock<ILogger<UKItemFilter>> loggerMock = new Mock<ILogger<UKItemFilter>>();
            IFilter filter = new UKItemFilter(loggerMock.Object);

            Dictionary<string, string> itemAttributes = new Dictionary<string, string>
            {
                { "UKItem", "" }
            };
            itemModel.ItemAttributes = itemAttributes;

            Assert.IsFalse(filter.Filter(itemModel));
        }

        [TestMethod]
        public void FilterUKItem_UKItemSetToNo_Test()
        {
            Mock<ILogger<UKItemFilter>> loggerMock = new Mock<ILogger<UKItemFilter>>();
            IFilter filter = new UKItemFilter(loggerMock.Object);

            Dictionary<string, string> itemAttributes = new Dictionary<string, string>
            {
                { "UKItem", "No" }
            };
            itemModel.ItemAttributes = itemAttributes;

            Assert.IsFalse(filter.Filter(itemModel));
        }
    } 
}
