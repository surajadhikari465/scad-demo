using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Interfaces;
using NutritionWebApi.Common.Models;
using NutritionWebApi.Controllers;
using NutritionWebApi.DataAccess.Commands;
using NutritionWebApi.DataAccess.Queries;
using NutritionWebApi.Tests.Common.Builders;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace NutritionWebApi.Tests.Unit.Controllers
{
    /// <summary>
    /// Summary description for NutritionControllerTests
    /// </summary>
    [TestClass]
    public class NutritionControllerTests
    {
        private Mock<IQueryHandler<GetNutritionItemQuery, List<NutritionItemModel>>> getNutritionItemQuery;
        private Mock<ICommandHandler<AddOrUpdateNutritionItemCommand>> addOrUpdateNutritionItemCommandHandler;
        private Mock<ICommandHandler<DeleteNutritionCommand>> deleteNutritionCommandHandler;
        private Mock<ILogger> logger;
        private NutritionController controller;

        [TestInitialize]
        public void Initialize()
        {
            getNutritionItemQuery = new Mock<IQueryHandler<GetNutritionItemQuery, List<NutritionItemModel>>>();
            addOrUpdateNutritionItemCommandHandler = new Mock<ICommandHandler<AddOrUpdateNutritionItemCommand>>();
            deleteNutritionCommandHandler = new Mock<ICommandHandler<DeleteNutritionCommand>>();
            getNutritionItemQuery.Setup(iq => iq.Search(It.IsAny<GetNutritionItemQuery>())).Returns(new List<NutritionItemModel>());
            logger = new Mock<ILogger>();
            controller = new NutritionController(getNutritionItemQuery.Object, addOrUpdateNutritionItemCommandHandler.Object, deleteNutritionCommandHandler.Object, logger.Object);
        }

        [TestMethod]
        public void NutritionController_GetAll_ReturnsList()
        {
            // Given.
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // When
            var response = controller.GetNutritionItems();

            // Assert            
            Assert.IsInstanceOfType(response, typeof(List<NutritionItemModel>));
        }

        [TestMethod]
        public void NutritionController_Post_SuccessfullyInsertsItem()
        {
            // Given.
            addOrUpdateNutritionItemCommandHandler.Setup(ai => ai.Execute(It.IsAny<AddOrUpdateNutritionItemCommand>())).Returns("Success: nutrition items are inserted successfully");

            List<NutritionItemModel> list = BuildNutritionItemList(1);
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // When
            var response = controller.AddOrUpdateNutritionItem(list) as OkNegotiatedContentResult<string>;

            // Assert    
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Content, "Success: nutrition items are inserted successfully");
        }

        [TestMethod]
        public void NutritionController_Put_SuccessfullyUpdatesItem()
        {
            // Given.
            addOrUpdateNutritionItemCommandHandler.Setup(ai => ai.Execute(It.IsAny<AddOrUpdateNutritionItemCommand>())).Returns("Success: nutrition items are updated successfully");
            List<NutritionItemModel> itemModel = BuildNutritionItemList(1);

            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // When
            var response = controller.AddOrUpdateNutritionItem(itemModel) as OkNegotiatedContentResult<string>;

            // Then   
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Content, "Success: nutrition items are updated successfully");
        }


        [TestMethod]
        public void NutritionController_ErrorDuringPut_ExceptionShouldBeThrown()
        {
            // Given.
            addOrUpdateNutritionItemCommandHandler.Setup(ai => ai.Execute(It.IsAny<AddOrUpdateNutritionItemCommand>())).Throws(new Exception());

            List<NutritionItemModel> itemModel = BuildNutritionItemList(1);
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // When.
            var response = controller.AddOrUpdateNutritionItem(itemModel) as BadRequestErrorMessageResult;

            // Then.
            Assert.IsTrue(response != null);
        }

        [TestMethod]
        public void NutritionController_Put_NoParamaters_BadRequest()
        {
					//Given
					List<NutritionItemModel> itemModel = null;
					
					// When
					var response = controller.AddOrUpdateNutritionItem(itemModel);
					
					//Then
					Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult), NutritionController.INVALID_REQUEST);
        }

        private List<NutritionItemModel> BuildNutritionItemList(int count)
        {
            List<NutritionItemModel> itemList = new List<NutritionItemModel>();
            for (int i = 1; i <= count; i++)
            {
                NutritionItemModelBuilder builder = new NutritionItemModelBuilder().WithPlu((i).ToString()).WithRecipeName("Unit Test Recipe " + (i).ToString());
                itemList.Add(builder);
            }
            return itemList;
        }
    }
}
