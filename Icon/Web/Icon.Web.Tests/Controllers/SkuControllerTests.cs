using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<ItemGroupModel> skusSource;
        private Mock<IQueryHandler<GetItemGroupParameters, List<ItemGroupModel>>> getItemGroupPageQuery;
        private Mock<IQueryHandler<GetItemGroupFilteredResultsCountQueryParameters, int>> getFilteredResultsCountQuery;
        private Mock<IQueryHandler<GetItemGroupUnfilteredResultsCountQueryParameters, int>> getUnfilteredResultsCountQuery;
        private List<ItemGroupModel> getItemGroupPageQueryResult;
        private int getFilteredResultsCountQueryResult;
        private int getUnfilteredResultsCountQueryResult;
        private DataTableAjaxPostModel dataTableAjaxPostModel;
        private string datatablePostJson = @"
                {
                    ""draw"": 1,
                    ""start"": 0,
                    ""length"": 5,
                    ""columns"": [
                        {
                            ""data"": ""SkuId"",
                            ""name"": ""SkuId"",
                            ""searchable"": true,
                            ""orderable"": true,
                            ""search"": {
                                ""value"": null,
                                ""regex"": ""false""
                            }
                        },
                        {
                            ""data"": ""SkuDescription"",
                            ""name"": ""Sku Description"",
                            ""searchable"": true,
                            ""orderable"": true,
                            ""search"": {
                                ""value"": null,
                                ""regex"": ""false""
                            }
                        },
                        {
                            ""data"": ""PrimaryItemUpc"",
                            ""name"": ""Primary Item Upc"",
                            ""searchable"": true,
                            ""orderable"": true,
                            ""search"": {
                                ""value"": null,
                                ""regex"": ""false""
                            }
                        },
                        {
                            ""data"": ""CountOfItems"",
                            ""name"": ""Count Of Items"",
                            ""searchable"": false,
                            ""orderable"": true,
                            ""search"": {
                                ""value"": null,
                                ""regex"": ""false""
                            }
                        },
                        {
                            ""data"": ""SkuId"",
                            ""name"": null,
                            ""searchable"": false,
                            ""orderable"": false,
                            ""search"": {
                                ""value"": null,
                                ""regex"": ""false""
                            }
                        }
                    ],
                    ""search"": {
                        ""value"": null,
                        ""regex"": ""false""
                    },
                    ""order"": [
                        {
                            ""column"": 0,
                            ""dir"": ""asc""
                        }
                    ]
                }";

        /// <summary>
        /// Initializes tests with:
        /// * Test data
        /// * Mock Objects
        /// * Controller
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            getItemGroupPageQuery = new Mock<IQueryHandler<GetItemGroupParameters, List<ItemGroupModel>>>();
            getFilteredResultsCountQuery = new Mock<IQueryHandler<GetItemGroupFilteredResultsCountQueryParameters, int>>();
            getUnfilteredResultsCountQuery = new Mock<IQueryHandler<GetItemGroupUnfilteredResultsCountQueryParameters, int>>();

            skusSource = new List<ItemGroupModel>();
            
            List<string> products = new List<string> { "Tofu", "Cat food", "Lettuce", "Banana", "Pasta", "Avocado", "Ham" };
            List<string> adjectives = new List<string> { "Organic", "Premium", "Regular" };

            int counter = 0;
            foreach(var product in products)
            {
                foreach (var adjective in adjectives)
                {
                    skusSource.Add(new ItemGroupModel
                    {
                        ItemGroupId = 1000 + counter,
                        ItemGroupTypeId = ItemGroupTypeId.Sku,
                        SKUDescription = $"{adjective} {product}",
                        ScanCode = (10000 + counter).ToString().PadLeft(13, '0'),
                        ItemCount = counter % 7,
                    });
                    counter++;
                }
            }

            getItemGroupPageQuery.Setup(m => m.Search(It.IsAny<GetItemGroupParameters>()))
                .Returns(() => getItemGroupPageQueryResult);
            getFilteredResultsCountQuery.Setup(m => m.Search(It.IsAny<GetItemGroupFilteredResultsCountQueryParameters>()))
                .Returns(() => getFilteredResultsCountQueryResult);
            getUnfilteredResultsCountQuery.Setup(m => m.Search(It.IsAny<GetItemGroupUnfilteredResultsCountQueryParameters>()))
                .Returns(() => getUnfilteredResultsCountQueryResult);

            dataTableAjaxPostModel = JsonConvert.DeserializeObject<DataTableAjaxPostModel>(datatablePostJson);
            controller = new SkuController(getItemGroupPageQuery.Object, getFilteredResultsCountQuery.Object, getUnfilteredResultsCountQuery.Object);
        }

        /// <summary>
        /// Checks that the controller contructor validates input
        /// </summary>
        [TestMethod]
        public void Controller_contructor_should_validate_arguments()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new SkuController(null, getFilteredResultsCountQuery.Object, getUnfilteredResultsCountQuery.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new SkuController(getItemGroupPageQuery.Object, null, getUnfilteredResultsCountQuery.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new SkuController(getItemGroupPageQuery.Object, getFilteredResultsCountQuery.Object, null));
            Assert.ThrowsException<ArgumentNullException>(() => new SkuController(null, null, null));
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
        public void Controller_AllSku_no_search_should_return_json()
        {
            // Given.
            getItemGroupPageQueryResult = skusSource.Take(5).ToList();
            getFilteredResultsCountQueryResult = skusSource.Count;
            getUnfilteredResultsCountQueryResult = skusSource.Count;
            dataTableAjaxPostModel.search = null;

            // When.
            JsonResult result = controller.AllSku(dataTableAjaxPostModel) as JsonResult;

            // Then.
            Assert.IsNotNull(result);

            //Verify returned data
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var allSkuData = JsonConvert.DeserializeObject<DataTableResponse<SkuViewModel>>(jsonResult);

            Assert.AreEqual(1, allSkuData.draw);
            Assert.AreEqual(getFilteredResultsCountQueryResult, allSkuData.recordsFiltered);
            Assert.AreEqual(getUnfilteredResultsCountQueryResult, allSkuData.recordsTotal);
            Assert.AreEqual(5, allSkuData.data.Count);

            for (int index =0; index < getItemGroupPageQueryResult.Count; index++)
            {
                Assert.AreEqual(getItemGroupPageQueryResult[index].ItemGroupId, allSkuData.data[index].SkuId);
                Assert.AreEqual(getItemGroupPageQueryResult[index].ScanCode, allSkuData.data[index].PrimaryItemUpc);
                Assert.AreEqual(getItemGroupPageQueryResult[index].SKUDescription, allSkuData.data[index].SkuDescription);
                Assert.AreEqual(getItemGroupPageQueryResult[index].ItemCount, allSkuData.data[index].CountOfItems);
            }
        }

        /// <summary>
        /// Checks that AllSku returns a correct json.
        /// </summary>
        [TestMethod]
        public void Controller_AllSku_with_search_should_return_json()
        {
            // Given.
            dataTableAjaxPostModel.search.value = "Pasta";

            var preResult = skusSource
                .Where(s => s.SKUDescription.Contains(dataTableAjaxPostModel.search.value))
                .ToList();

            getItemGroupPageQueryResult = preResult.Take(5).ToList();
            getFilteredResultsCountQueryResult = preResult.Count;
            getUnfilteredResultsCountQueryResult = skusSource.Count;

            // When.
            JsonResult result = controller.AllSku(dataTableAjaxPostModel) as JsonResult;

            // Then.
            Assert.IsNotNull(result);

            //Verify returned data
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var allSkuData = JsonConvert.DeserializeObject<DataTableResponse<SkuViewModel>>(jsonResult);

            Assert.AreEqual(1, allSkuData.draw);
            Assert.AreEqual(getFilteredResultsCountQueryResult, allSkuData.recordsFiltered);
            Assert.AreEqual(getUnfilteredResultsCountQueryResult, allSkuData.recordsTotal);
            Assert.AreEqual(getItemGroupPageQueryResult.Count, allSkuData.data.Count);

            for (int index = 0; index < getItemGroupPageQueryResult.Count; index++)
            {
                Assert.AreEqual(getItemGroupPageQueryResult[index].ItemGroupId, allSkuData.data[index].SkuId);
                Assert.AreEqual(getItemGroupPageQueryResult[index].ScanCode, allSkuData.data[index].PrimaryItemUpc);
                Assert.AreEqual(getItemGroupPageQueryResult[index].SKUDescription, allSkuData.data[index].SkuDescription);
                Assert.AreEqual(getItemGroupPageQueryResult[index].ItemCount, allSkuData.data[index].CountOfItems);
            }
        }

    }
}
