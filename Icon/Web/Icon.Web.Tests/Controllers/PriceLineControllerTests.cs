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
using Icon.FeatureFlags;

namespace Icon.Web.Tests.Unit.Controllers
{
    /// <summary>
    /// Price Line Controller Tests.
    /// </summary>
    [TestClass]
    public class PriceLineControllerTests
    {
        private PriceLineController controller;
        private List<ItemGroupModel> priceLineSource;
        private Mock<IQueryHandler<GetItemGroupParameters, List<ItemGroupModel>>> getItemGroupPageQuery;
        private Mock<IQueryHandler<GetItemGroupFilteredResultsCountQueryParameters, int>> getFilteredResultsCountQuery;
        private Mock<IQueryHandler<GetItemGroupUnfilteredResultsCountQueryParameters, int>> getUnfilteredResultsCountQuery;
        private Mock<IFeatureFlagService> featureFlagService;
        private List<ItemGroupModel> getItemGroupPageQueryResult;
        private int getFilteredResultsCountQueryResult;
        private int getUnfilteredResultsCountQueryResult;
        private DataTableAjaxPostModel dataTableAjaxPostModel;
        private string datatablePostJson = @"
                    {
                        ""draw"": 1,
                        ""start"": 0,
                        ""length"": 20,
                        ""columns"": [
                            {
                                ""data"": ""PriceLineId"",
                                ""name"": ""Price Line Id"",
                                ""searchable"": true,
                                ""orderable"": true,
                                ""search"": {
                                    ""value"": null,
                                    ""regex"": ""false""
                                }
                            },
                            {
                                ""data"": ""PriceLineDescription"",
                                ""name"": ""Description"",
                                ""searchable"": true,
                                ""orderable"": true,
                                ""search"": {
                                    ""value"": null,
                                    ""regex"": ""false""
                                }
                            },
                            {
                                ""data"": ""PriceLineSize"",
                                ""name"": ""Size"",
                                ""searchable"": true,
                                ""orderable"": true,
                                ""search"": {
                                    ""value"": null,
                                    ""regex"": ""false""
                                }
                            },
                            {
                                ""data"": ""PriceLineUOM"",
                                ""name"": ""UOM"",
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
                                ""data"": null,
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
            featureFlagService = new Mock<IFeatureFlagService>();

            priceLineSource = new List<ItemGroupModel>();
            
            List<string> products = new List<string> { "Tofu", "Cat food", "Lettuce", "Banana", "Pasta", "Avocado", "Ham" };
            List<string> adjectives = new List<string> { "Organic", "Premium", "Regular" };

            int counter = 0;
            foreach(var product in products)
            {
                foreach (var adjective in adjectives)
                {
                    priceLineSource.Add(new ItemGroupModel
                    {
                        ItemGroupId = 1000 + counter,
                        ItemGroupTypeId = ItemGroupTypeId.Priceline,
                        PriceLineDescription = $"{adjective} {product}",
                        PriceLineSize = (counter + 13).ToString(),
                        PriceLineUOM = "each",
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
            featureFlagService.Setup(m => m.IsEnabled(It.IsAny<string>()))
                .Returns(true);

            dataTableAjaxPostModel = JsonConvert.DeserializeObject<DataTableAjaxPostModel>(datatablePostJson);
            controller = new PriceLineController(getItemGroupPageQuery.Object, getFilteredResultsCountQuery.Object, getUnfilteredResultsCountQuery.Object, featureFlagService.Object);
        }

        /// <summary>
        /// Checks that the controller contructor validates input
        /// </summary>
        [TestMethod]
        public void Controller_contructor_should_validate_arguments()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineController(null, getFilteredResultsCountQuery.Object, getUnfilteredResultsCountQuery.Object, featureFlagService.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineController(getItemGroupPageQuery.Object, null, getUnfilteredResultsCountQuery.Object, featureFlagService.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineController(getItemGroupPageQuery.Object, getFilteredResultsCountQuery.Object, null, featureFlagService.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineController(getItemGroupPageQuery.Object, getFilteredResultsCountQuery.Object, getUnfilteredResultsCountQuery.Object, null));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineController(null, null, null, null));
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
        /// Checks that AllPriceline returns a correct json.
        /// </summary>
        [TestMethod]
        public void Controller_AllPriceline_no_search_should_return_json()
        {
            // Given.
            getItemGroupPageQueryResult = priceLineSource.Take(5).ToList();
            getFilteredResultsCountQueryResult = priceLineSource.Count;
            getUnfilteredResultsCountQueryResult = priceLineSource.Count;
            dataTableAjaxPostModel.search = null;

            // When.
            JsonResult result = controller.AllPriceline(dataTableAjaxPostModel) as JsonResult;

            // Then.
            Assert.IsNotNull(result);

            //Verify returned data
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var resultPriceLineData = JsonConvert.DeserializeObject<DataTableResponse<PriceLineViewModel>>(jsonResult);

            Assert.AreEqual(1, resultPriceLineData.draw);
            Assert.AreEqual(getFilteredResultsCountQueryResult, resultPriceLineData.recordsFiltered);
            Assert.AreEqual(getUnfilteredResultsCountQueryResult, resultPriceLineData.recordsTotal);
            Assert.AreEqual(5, resultPriceLineData.data.Count);

            for (int index =0; index < getItemGroupPageQueryResult.Count; index++)
            {
                Assert.AreEqual(getItemGroupPageQueryResult[index].ItemGroupId, resultPriceLineData.data[index].PriceLineId);
                Assert.AreEqual(getItemGroupPageQueryResult[index].ScanCode, resultPriceLineData.data[index].PrimaryItemUpc);
                Assert.AreEqual(getItemGroupPageQueryResult[index].PriceLineDescription, resultPriceLineData.data[index].PriceLineDescription);
                Assert.AreEqual(getItemGroupPageQueryResult[index].PriceLineSize, resultPriceLineData.data[index].PriceLineSize);
                Assert.AreEqual(getItemGroupPageQueryResult[index].PriceLineUOM, resultPriceLineData.data[index].PriceLineUOM);
                Assert.AreEqual(getItemGroupPageQueryResult[index].ItemCount, resultPriceLineData.data[index].CountOfItems);
            }
        }

        /// <summary>
        /// Checks that AllAllPriceline returns a correct json.
        /// </summary>
        [TestMethod]
        public void Controller_AllAllPriceline_with_search_should_return_json()
        {
            // Given.
            dataTableAjaxPostModel.search.value = "Pasta";

            var preResult = priceLineSource
                .Where(s => s.PriceLineDescription.Contains(dataTableAjaxPostModel.search.value))
                .ToList();

            getItemGroupPageQueryResult = preResult.Take(5).ToList();
            getFilteredResultsCountQueryResult = preResult.Count;
            getUnfilteredResultsCountQueryResult = priceLineSource.Count;

            // When.
            JsonResult result = controller.AllPriceline(dataTableAjaxPostModel) as JsonResult;

            // Then.
            Assert.IsNotNull(result);

            //Verify returned data
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var resultPriceLineData = JsonConvert.DeserializeObject<DataTableResponse<PriceLineViewModel>>(jsonResult);

            Assert.AreEqual(1, resultPriceLineData.draw);
            Assert.AreEqual(getFilteredResultsCountQueryResult, resultPriceLineData.recordsFiltered);
            Assert.AreEqual(getUnfilteredResultsCountQueryResult, resultPriceLineData.recordsTotal);
            Assert.AreEqual(getItemGroupPageQueryResult.Count, resultPriceLineData.data.Count);

            for (int index = 0; index < getItemGroupPageQueryResult.Count; index++)
            {
                Assert.AreEqual(getItemGroupPageQueryResult[index].ItemGroupId, resultPriceLineData.data[index].PriceLineId);
                Assert.AreEqual(getItemGroupPageQueryResult[index].ScanCode, resultPriceLineData.data[index].PrimaryItemUpc);
                Assert.AreEqual(getItemGroupPageQueryResult[index].PriceLineDescription, resultPriceLineData.data[index].PriceLineDescription);
                Assert.AreEqual(getItemGroupPageQueryResult[index].PriceLineSize, resultPriceLineData.data[index].PriceLineSize);
                Assert.AreEqual(getItemGroupPageQueryResult[index].PriceLineUOM, resultPriceLineData.data[index].PriceLineUOM);
                Assert.AreEqual(getItemGroupPageQueryResult[index].ItemCount, resultPriceLineData.data[index].CountOfItems);
            }
        }

    }
}
