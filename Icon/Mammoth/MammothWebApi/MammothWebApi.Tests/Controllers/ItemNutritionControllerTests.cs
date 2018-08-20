using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.Controllers;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace MammothWebApi.Tests.Controllers
{


    [TestClass]
    public class ItemNutritionControllerTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetItemNutritionAttributesByItemIdQuery, IEnumerable<ItemNutritionAttributes>>> getItemNutritionAttributesByItemIdHandler;
        private ItemNutritionController controller;


        [TestInitialize]
        public void InitializeTest()
        {
            
            this.getItemNutritionAttributesByItemIdHandler = new Mock<IQueryHandler<GetItemNutritionAttributesByItemIdQuery, IEnumerable<ItemNutritionAttributes>>>();
            this.mockLogger = new Mock<ILogger>();
            this.controller = new ItemNutritionController(this.mockLogger.Object,this.getItemNutritionAttributesByItemIdHandler.Object);
        }

        [TestMethod]
        public void ItemNutritionController_NoItemIdsRequested_ReturnsBadRequestErrorMessageResult()
        {
            // Given
            var expectedMessage = "At least one ItemId must be included";
            var itemNutritionRequestModel = new ItemNutritionRequestModel()
            { 
                ItemIds = null
            };
            // When
            var response = this.controller.GetItemNutrition(itemNutritionRequestModel) as BadRequestErrorMessageResult;
            

            // Then

            Assert.IsNotNull(response, "The BadRequestErrorMessageResult response is null.");
            Assert.AreEqual( expectedMessage, response.Message, "The BadRequestErrorMessageResult Message did not match the expected error message.");

        }

        [TestMethod]
        public void ItemNutritionController_OneItemId_ReturnsResults()
        {
            // Given
            var itemNutritionRequestModel = new ItemNutritionRequestModel()
            {
                ItemIds = new [] { 1882959 }
            };

            this.getItemNutritionAttributesByItemIdHandler
                .Setup(h => h.Search(It.IsAny<GetItemNutritionAttributesByItemIdQuery>()))
                .Returns(new List<ItemNutritionAttributes>() {
                    new ItemNutritionAttributes()
                    {
                        ItemId =1882959, 
                        Calories = 100,
                        ServingPerContainer = "1", 
                        ServingSizeDesc = "1 ea", 
                        ServingsPerPortion = 1.0m
                    },
                });
            // When
            var response = (JsonResult<IOrderedEnumerable<KeyValuePair<int, ItemNutritionAttributes>>>) this.controller.GetItemNutrition(itemNutritionRequestModel);
            
            var data = response.Content.ToDictionary(p => p.Key, p => p.Value);

            

            // Then

            Assert.IsNotNull(response, "The Result response is null.");
            Assert.IsNotNull(data[1882959], "A response was returned data was not found");
            Assert.IsTrue(data[1882959].Calories==100, "A response was returned but unexpected data was found");

            //Assert.AreEqual(expectedMessage, response.Message, "The BadRequestErrorMessageResult Message did not match the expected error message.");

        }

    }
}
