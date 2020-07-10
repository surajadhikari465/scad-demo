using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.Mvc.Controllers;
using Moq;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Queries;
using Icon.Web.DataAccess.Models;
using System.Web.Mvc;
using Icon.Web.Mvc.Models;
using Newtonsoft.Json;

namespace Icon.Web.Tests.Unit.Controllers
{
    /// <summary>
    /// SkuController Tests.
    /// </summary>
    [TestClass]
    public class SkuControllerTests
    {
        private SkuController controller;
        private Mock<IQueryHandler<GetItemGroupParameters, IEnumerable<ItemGroupModel>>> skuQuery;
        private Mock<IQueryHandler<GetItemGroupItemCountParameters, IEnumerable<SkuItemCountModel>>> skuItemCount;
        private List<ItemGroupModel> skus;
        private List<SkuItemCountModel>  skusItemCount;

        /// <summary>
        /// Initializes tests with:
        /// * Test data
        /// * Mock Objects
        /// * Controller
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            skuQuery = new Mock<IQueryHandler<GetItemGroupParameters, IEnumerable<ItemGroupModel>>>();
            skuItemCount = new Mock<IQueryHandler<GetItemGroupItemCountParameters, IEnumerable<SkuItemCountModel>>>();
            skuQuery.Setup(q => q.Search(It.IsAny<GetItemGroupParameters>())).Returns(() => this.skus);
            skuItemCount.Setup(q => q.Search(It.IsAny<GetItemGroupItemCountParameters>())).Returns(() => this.skusItemCount);

            skus = new List<ItemGroupModel>();
            skusItemCount = new List<SkuItemCountModel>();

            for (int i = 0; i < 10; i++)
            {
                skus.Add(new ItemGroupModel
                {
                    ItemGroupId = 1000 + i,
                    ItemGroupTypeId = ItemGroupTypeId.Sku,
                    ItemGroupAttributesJson = $"{{ \"SKUDescription\":\"SKU Description {i}\" }}",
                    ScanCode = (10000 + i).ToString().PadLeft(13, '0'),
                });
                skusItemCount.Add(new SkuItemCountModel
                {
                    ItemGroupId = 1000 + i,
                    CountOfItems = i + 1,
                });
            }

            controller = new SkuController(skuQuery.Object, skuItemCount.Object);
        }

        /// <summary>
        /// Checks that the controller contructor validates input
        /// </summary>
        [TestMethod]
        public void Controller_contructor_should_validate_arguments()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new SkuController(null, skuItemCount.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new SkuController(skuQuery.Object, null));
            Assert.ThrowsException<ArgumentNullException>(() => new SkuController(null, null));
        }


        /// <summary>
        /// Checks that index returna a view.
        /// </summary>
        [TestMethod]
        public void Controller_index_should_return_view()
        {
            // Given.

            // When.
            ViewResult result = controller.Index() as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Checks that AllSku returns a correct json.
        /// </summary>
        [TestMethod]
        public void Controller_AllSku_should_return_json()
        {
            // Given.

            // When.
            JsonResult result = controller.AllSku() as JsonResult;
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var allSkuData = JsonConvert.DeserializeObject<AllSkuJsonObject>(jsonResult);
            // Then.
            Assert.IsNotNull(result);
            
            //Verify returned data
            Assert.AreEqual(10, allSkuData.data.Count);
            for (int index =0; index < 10; index++)
            {
                Assert.AreEqual(skus[index].ItemGroupId, allSkuData.data[index].SkuId);
                Assert.AreEqual(skus[index].ScanCode, allSkuData.data[index].PrimaryItemUpc);
                Assert.AreEqual($"SKU Description {allSkuData.data[index].SkuId - 1000}", allSkuData.data[index].SkuDescription);
                Assert.IsNull(allSkuData.data[index].CountOfItems);

            }
        }

        /// <summary>
        /// Checks that AllSkuCount returns a correct json.
        /// </summary>

        [TestMethod]
        public void Controller_AllSkuCount_should_return_json()
        {
            // Given.

            // When.
            JsonResult result = controller.AllSkuCount() as JsonResult;
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var allSkuCountData = JsonConvert.DeserializeObject<List<SkuItemCountViewModel>>(jsonResult);
            // Then.
            Assert.IsNotNull(result);

            //Verify returned data
            Assert.AreEqual(10, allSkuCountData.Count);
            for (int index = 0; index < 10; index++)
            {
                Assert.AreEqual(skusItemCount[index].ItemGroupId, allSkuCountData[index].SkuId);
                Assert.AreEqual(skusItemCount[index].CountOfItems, allSkuCountData[index].CountOfItems);
            }
        }

        /// <summary>
        /// Class used to desirialize the return of AllSku.
        /// </summary>
        private class AllSkuJsonObject
        {
            /// <summary>
            /// Gets or sets the AllSku data.
            /// </summary>
            public List<SkuViewModel> data { get; set; }
        }
    }
}
