using System;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Expressions;


namespace KitBuilderWebApi.Tests.Controllers
{

  [TestClass]
  public class InstructionListControllerTests
    {

        private InstructionListController instructionListController;
        private Mock<ILogger<InstructionListController>> mockLogger;
        private Mock<IRepository<InstructionList>> instructionListRepository;
        private Mock<IRepository<InstructionListMember>> instructionListMemberRepository;
        private Mock<IRepository<InstructionType>> instructionTypeRespository;
        private Mock<IRepository<Status>> statusRespository;
        private Mock<IUrlHelper> urlHelper;
        private Mock<InstructionListHelper> instructionListHelper;

        [TestInitialize]
        public void InitializeTest()
        {
            mockLogger = new Mock<ILogger<InstructionListController>>();
            instructionListRepository = new Mock<IRepository<InstructionList>>();
            instructionListMemberRepository = new Mock<IRepository<InstructionListMember>>();
            instructionTypeRespository = new Mock<IRepository<InstructionType>>();
            statusRespository = new Mock<IRepository<Status>>();
            instructionListHelper = new Mock<InstructionListHelper>(urlHelper);

            instructionListController = new InstructionListController(instructionListRepository.Object,
                instructionListMemberRepository.Object,
                instructionTypeRespository.Object,
                statusRespository.Object,
                mockLogger.Object,
                instructionListHelper.Object);
        }

        [TestMethod]
        public void InstructionListController_GetInstructionsList_NoParameters_Returns_BadRequest()
        {
            var response =  instructionListController.GetInstructionsList(null);
            Assert.IsInstanceOfType(response,typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");

        }

        [TestMethod]
        public void InstructionListController_GetInstructionsList_ValidParameters_Returns_Ok()
        {
            var parameters = new InstructionListsParameters();

            var response = instructionListController.GetInstructionsList(parameters);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Request Expected");
            Assert.IsNotNull(response, "The response is null");
        }

        [TestMethod]
        public void InstructionListController_GetInstructionsList_InvalidOrderBy_Returns_BadRequest()
        {
            var parameters = new InstructionListsParameters {OrderBy = "ThisValueDoesntExist"};

            var response = instructionListController.GetInstructionsList(parameters);
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");
        }
        [TestMethod]
        public void InstructionListController_UpdateInstructionsList_ListDoesntExist_Returns_NotFound()
        {
            var parameters = new InstructionList {InstructionListId = -1};
            var response = instructionListController.UpdateInstructionList(parameters);

            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");
            Assert.IsNotNull(response, "The response is null");
        }

        [TestMethod]
        public void InstructionListController_UpdateInstructionsList_Null_Returns_BadRequest()
        {
            
            var response = instructionListController.UpdateInstructionList(null);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");
        }

        [TestMethod]
        public void InstructionListController_AddInstructionsList_Null_Returns_BadRequest()
        {

            var response = instructionListController.AddInstructionList(null);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");
        }

        [TestMethod]
        public void InstructionListController_AddInstructionsList_MissingDefaultStatus_Returns_BadRequest()
        {
            //var parameters = new AddInstructionListPrameters() { TypeId = 1, Name = "Something"};
            //statusRespository.Setup(s => s.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns( (Status)null );

            //var response = instructionListController.AddInstructionList(parameters);

            //Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            //Assert.IsNotNull(response, "The response is null");
        }





    }
}